using System;
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
        private DateTime? autosaveTime;
        private DateTime? autoresetitemsTime;
        private bool resetitemswarningSend;
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
                if(!this.autosaveTime.HasValue)
                {
                    this.autosaveTime = DateTime.Now.AddSeconds(RocketPlugin<Configuration>.Configuration.ConfigAutoSave.Interval);
                }
                if(DateTime.Now >= autosaveTime)
                {
                    SaveManager.save();
                    RocketChatManager.Say(RocketPlugin<Configuration>.Configuration.ConfigAutoSave.Message);
                    this.autosaveTime = DateTime.Now.AddSeconds(RocketPlugin<Configuration>.Configuration.ConfigAutoSave.Interval);
                }
            }
        }

        public void autoResetItems()
        {
            if(RocketPlugin<Configuration>.Configuration.ConfigAutoResetItems != null && RocketPlugin<Configuration>.Configuration.ConfigAutoResetItems.Enabled)
            {
                if(!this.autoresetitemsTime.HasValue)
                {
                    this.autoresetitemsTime = DateTime.Now.AddSeconds(RocketPlugin<Configuration>.Configuration.ConfigAutoResetItems.Interval);
                }
                if(DateTime.Now >= autoresetitemsTime)
                {
                    Util.ResetItems();
                    RocketChatManager.Say(RocketPlugin<Configuration>.Configuration.ConfigAutoResetItems.Message);
                    this.autoresetitemsTime = DateTime.Now.AddSeconds(RocketPlugin<Configuration>.Configuration.ConfigAutoResetItems.Interval);
                    this.resetitemswarningSend = false;
                }
                if(!this.resetitemswarningSend && (autoresetitemsTime.Value - DateTime.Now).TotalSeconds <= RocketPlugin<Configuration>.Configuration.ConfigAutoResetItems.WarningTime)
                {
                    RocketChatManager.Say(RocketPlugin<Configuration>.Configuration.ConfigAutoResetItems.WarningMessage);
                    this.resetitemswarningSend = true;
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
                        p.Player.Movement.SteamChannel.send("tellPosition", ESteamCall.NOT_OWNER & ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { new Vector3(0f, 0f, 0f) });
                    }
                }
            }
        }

        public void Update()
        {
            if (RocketPlugin.Loaded)
            {
                autoSave();
                autoResetItems();
                vanish();
            }
        }
    }
}
