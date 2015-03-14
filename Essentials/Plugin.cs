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
        public Dictionary<string, ProtectInfo> protectPlayers;
        public Dictionary<string, Vector3> frozenPlayers;
        public HashSet<string> vanishedPlayers;

        protected override void Load()
        {
            plugin = this;
            mainconfig = new MainConfig();
            i18n = new I18N();
            this.protectPlayers = new Dictionary<string, ProtectInfo>();
            this.frozenPlayers = new Dictionary<string, Vector3>();
            this.vanishedPlayers = new HashSet<string>();
            RocketServerEvents.OnPlayerConnected += this.playerJoin;
            RocketServerEvents.OnPlayerDisconnected += this.playerLeave;
            Logger.Log("Essentials by Android is load");
            base.Load();
        }

        public void playerJoin(Player player)
        {
            if (mainconfig.PlayerProtectEnabled)
            {
                RocketChatManager.Say(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID, "playerprotect.message".I18N(mainconfig.PlayerProtectTime));
                this.protectPlayers.Add(player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName, new ProtectInfo(player.transform.position, MeasurementTool.angleToByte(player.transform.rotation.eulerAngles.y), DateTime.Now.AddSeconds(mainconfig.PlayerProtectTime)));
            }
        }

        public void playerLeave(Player player)
        {
            string name = player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName;
            frozenPlayers.Remove(name);
            vanishedPlayers.Remove(name);
            if (protectPlayers.ContainsKey(name))
            {
                player.sendTeleport(protectPlayers[name].position, protectPlayers[name].angle);
                player.save();
                protectPlayers.Remove(name);
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
                this.protectPlayers = new Dictionary<string, ProtectInfo>();
            }
            if (mainconfig.PlayerProtectEnabled)
            {
                string[] keys = this.protectPlayers.Keys.ToArray();
                foreach (string name in keys)
                {
                    ProtectInfo info = this.protectPlayers[name];
                    if (!string.IsNullOrEmpty(name) && info != null)
                    {
                        SteamPlayer steamplayer;
                        if (SteamPlayerlist.tryGetSteamPlayer(name, out steamplayer))
                        {
                            try
                            {
                                if (DateTime.Now < info.time)
                                {
                                    steamplayer.Player.sendTeleport(new Vector3(0, 24, 0), 0);
                                }
                                else
                                {
                                    steamplayer.Player.sendTeleport(info.position, info.angle);
                                    this.protectPlayers.Remove(name);
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
                this.frozenPlayers = new Dictionary<string, Vector3>();
            }
            foreach (KeyValuePair<string, Vector3> item in this.frozenPlayers)
            {
                if (!string.IsNullOrEmpty(item.Key) && item.Value != null)
                {
                    SteamPlayer p;
                    if (SteamPlayerlist.tryGetSteamPlayer(item.Key, out p))
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
                this.vanishedPlayers = new HashSet<string>();
            }
            foreach (string name in this.vanishedPlayers)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    SteamPlayer p;
                    if (SteamPlayerlist.tryGetSteamPlayer(name, out p))
                    {
                        p.Player.Movement.SteamChannel.send("tellPosition", ESteamCall.NOT_OWNER & ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { Vector3.zero });
                    }
                }
            }
        }

        public void Update()
        {
            if (RocketPlugin.Loaded)
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
