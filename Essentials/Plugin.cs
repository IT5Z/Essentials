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
using Essentials.ConfigManager;
using Essentials.Extensions;
using Essentials.Model;
using UnityEngine;

namespace Essentials
{
    internal class Plugin : RocketPlugin
    {
        private static Plugin plugin;
        public static Plugin instance
        {
            get
            {
                return plugin;
            }
        }
        private MainConfig mainconfig;
        public MainConfig MainConfig
        {
            get
            {
                return this.mainconfig;
            }
        }
        private I18N i18n;
        public I18N I18N
        {
            get
            {
                return this.i18n;
            }
        }
        private DateTime? autosaveTime;
        private DateTime? autoresetitemsTime;
        private bool resetitemswarningSend;
        public Dictionary<ulong, ProtectInfo> protectPlayers;
        public Dictionary<ulong, Vector3> frozenPlayers;
        public HashSet<ulong> vanishedPlayers;

        protected override void Load()
        {
            Plugin.plugin = this;
            this.mainconfig = new MainConfig();
            this.i18n = new I18N();
            this.protectPlayers = new Dictionary<ulong, ProtectInfo>();
            this.frozenPlayers = new Dictionary<ulong, Vector3>();
            this.vanishedPlayers = new HashSet<ulong>();
            RocketServerEvents.OnPlayerConnected += this.playerJoin;
            RocketServerEvents.OnPlayerDisconnected += this.playerLeave;
            Logger.Log("Essentials by Android is load");
            base.Load();
        }

        public void playerJoin(Player player)
        {
            if (MainConfig.PlayerProtectEnabled)
            {
                RocketChatManager.Say(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID, "playerprotect.message".I18N(MainConfig.PlayerProtectTime));
                this.protectPlayers.Add(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID.m_SteamID, new ProtectInfo(player.transform.position, MeasurementTool.angleToByte(player.transform.rotation.eulerAngles.y), DateTime.Now.AddSeconds(MainConfig.PlayerProtectTime)));
            }
        }

        public void playerLeave(Player player)
        {
            ulong steamid = player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID.m_SteamID;
            frozenPlayers.Remove(steamid);
            vanishedPlayers.Remove(steamid);
            if (protectPlayers.ContainsKey(steamid))
            {
                player.gameObject.transform.GetComponent<RocketPlayerFeatures>().setGodMode(false);
                player.sendTeleport(protectPlayers[steamid].position, protectPlayers[steamid].angle);
                player.save();
                protectPlayers.Remove(steamid);
            }
        }

        public void autoSave()
        {
            if(this.MainConfig.AutoSaveEnabled)
            {
                if(!this.autosaveTime.HasValue)
                {
                    this.autosaveTime = DateTime.Now.AddSeconds(this.MainConfig.AutoSaveInterval);
                }
                if(DateTime.Now >= autosaveTime)
                {
                    SaveManager.save();
                    RocketChatManager.Say("autosave.message".I18N());
                    this.autosaveTime = DateTime.Now.AddSeconds(this.MainConfig.AutoSaveInterval);
                }
            }
        }

        public void autoResetItems()
        {
            if (this.MainConfig.AutoResetItemsEnabled)
            {
                if(!this.autoresetitemsTime.HasValue)
                {
                    this.autoresetitemsTime = DateTime.Now.AddSeconds(this.MainConfig.AutoResetItemsInterval);
                }
                if(DateTime.Now >= autoresetitemsTime)
                {
                    Util.ResetItems();
                    RocketChatManager.Say("autoresetitems.message".I18N());
                    this.autoresetitemsTime = DateTime.Now.AddSeconds(this.MainConfig.AutoResetItemsInterval);
                    this.resetitemswarningSend = false;
                }
                if (!this.resetitemswarningSend && (autoresetitemsTime.Value - DateTime.Now).TotalSeconds <= this.MainConfig.AutoResetItemsWarningTime)
                {
                    RocketChatManager.Say("autoresetitems.warningmessage".I18N(this.MainConfig.AutoResetItemsWarningTime));
                    this.resetitemswarningSend = true;
                }
            }
        }

        public void protect()
        {
            if (protectPlayers == null)
            {
                this.protectPlayers = new Dictionary<ulong, ProtectInfo>();
            }
            if (MainConfig.PlayerProtectEnabled)
            {
                ulong[] keys = this.protectPlayers.Keys.ToArray();
                foreach (ulong steamid in keys)
                {
                    ProtectInfo info = this.protectPlayers[steamid];
                    if (steamid != 0 && info != null)
                    {
                        SteamPlayer steamplayer;
                        if (PlayerTool.tryGetSteamPlayer(steamid.ToString(), out steamplayer))
                        {
                            try
                            {
                                if (DateTime.Now < info.time)
                                {
                                    steamplayer.Player.gameObject.transform.GetComponent<RocketPlayerFeatures>().setGodMode(true);
                                    steamplayer.Player.sendTeleport(new Vector3(0, 24, 0), 0);
                                }
                                else
                                {
                                    steamplayer.Player.gameObject.transform.GetComponent<RocketPlayerFeatures>().setGodMode(false);
                                    steamplayer.Player.sendTeleport(info.position, info.angle);
                                    this.protectPlayers.Remove(steamid);
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }

        public void freeze()
        {
            if (frozenPlayers == null)
            {
                this.frozenPlayers = new Dictionary<ulong, Vector3>();
            }
            foreach (KeyValuePair<ulong, Vector3> item in this.frozenPlayers)
            {
                if (item.Key != 0 && item.Value != null)
                {
                    SteamPlayer p;
                    if (PlayerTool.tryGetSteamPlayer(item.Key.ToString(), out p))
                    {
                        p.Player.Movement.SteamChannel.send("tellPosition", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { item.Value });
                    }
                }
            }
        }

        public void vanish()
        {
            if (this.vanishedPlayers == null)
            {
                this.vanishedPlayers = new HashSet<ulong>();
            }
            foreach (ulong steamid in this.vanishedPlayers)
            {
                if (steamid != 0)
                {
                    SteamPlayer p;
                    if (PlayerTool.tryGetSteamPlayer(steamid.ToString(), out p))
                    {
                        p.Player.Movement.SteamChannel.send("tellPosition", ESteamCall.NOT_OWNER & ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { Vector3.zero });
                    }
                }
            }
        }

        public void Update()
        {
            if (this.Loaded)
            {
                this.autoSave();
                this.autoResetItems();
                this.protect();
                this.freeze();
                this.vanish();
            }
        }
    }
}
