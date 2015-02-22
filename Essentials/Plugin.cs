﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Logging;
using Rocket.RocketAPI;
using Rocket.RocketAPI.Events;
using SDG;
using Essentials.Commands;
using UnityEngine;
using System.Reflection;

namespace Essentials
{
    internal class Plugin : RocketPlugin<Configuration>
    {
        private static Plugin plugin;
        public static Plugin instance
        {
            get
            {
                return plugin;
            }
        }
        public DateTime? autosavetime;
        public HashSet<string> vanishPlayers;

        protected override void Load()
        {
            plugin = this;
            RocketServerEvents.OnPlayerDisconnected += playerLeave;
            Logger.Log("Essentials by Android is load");
            base.Load();
        }

        public void playerLeave(Player player)
        {
            vanishPlayers.Remove(player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName);
        }

        public void autoSave()
        {
            if(RocketPlugin<Configuration>.Configuration.ConfigAutoSave != null && RocketPlugin<Configuration>.Configuration.ConfigAutoSave.Enabled)
            {
                if(!this.autosavetime.HasValue)
                {
                    this.autosavetime = DateTime.Now.AddSeconds(RocketPlugin<Configuration>.Configuration.ConfigAutoSave.Interval);
                }
                if(DateTime.Now >= autosavetime) {
                    SaveManager.save();
                    RocketChatManager.Say(RocketPlugin<Configuration>.Configuration.ConfigAutoSave.Message);
                    this.autosavetime = DateTime.Now.AddSeconds(RocketPlugin<Configuration>.Configuration.ConfigAutoSave.Interval);
                }
            }
        }

        public void vanish()
        {
            if (this.vanishPlayers == null)
            {
                this.vanishPlayers = new HashSet<string>();
            }
            foreach (string name in this.vanishPlayers)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    SteamPlayer p;
                    if (SteamPlayerlist.tryGetSteamPlayer(name, out p))
                    {
                        PlayerMovement move = p.Player.Movement;
                        InteractableVehicle vehicle = move.getVehicle();
                        if (vehicle)
                        {
                            byte b = 0;
                            while(b < vehicle.G.Length)
                            {
                                if (vehicle.G[b] != null && vehicle.G[b].g != null && p.Equals(vehicle.G[b].g))
                                {
                                    VehicleManager vm = (VehicleManager)typeof(VehicleManager).GetField("b", BindingFlags.NonPublic | BindingFlags.Static).GetValue(new VehicleManager());
                                    vm.SteamChannel.send("tellExitVehicle", ESteamCall.NOT_OWNER & ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { vehicle.Y, b, new Vector3(0, 0, 0), 0 });
                                    break;
                                }
                                b += 1;
                            }
                        }
                        else
                        {
                            move.SteamChannel.send("tellPosition", ESteamCall.NOT_OWNER & ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { new Vector3(0f, 0f, 0f) });
                        }
                    }
                }
            }
        }

        public void Update()
        {
            if (RocketPlugin.Loaded)
            {
                autoSave();
                vanish();
            }
        }
    }
}
