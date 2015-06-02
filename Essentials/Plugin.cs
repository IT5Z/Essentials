using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Unturned;
using Rocket.Unturned.Plugins;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using Rocket.Unturned.Events;
using SDG;
using UnityEngine;
using Steamworks;
using Essentials.Commands;
using Essentials.ConfigManager;
using Essentials.Extensions;
using Essentials.Model;

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

        public void playerJoin(RocketPlayer player)
        {
            if (MainConfig.PlayerProtectEnabled)
            {
                RocketChat.Say(player, "playerprotect.message".I18N(MainConfig.PlayerProtectTime));
                this.protectPlayers.Add(player.CSteamID.m_SteamID, new ProtectInfo(player.Position, player.Rotation, DateTime.Now.AddSeconds(MainConfig.PlayerProtectTime)));
            }
        }

        public void playerLeave(RocketPlayer player)
        {
            ulong steamid = player.CSteamID.m_SteamID;
            frozenPlayers.Remove(steamid);
            vanishedPlayers.Remove(steamid);
            if (protectPlayers.ContainsKey(steamid))
            {
                player.Features.setGodMode(false);
                player.Teleport(protectPlayers[steamid].position, protectPlayers[steamid].rotation);
                player.Player.save();
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
                    RocketChat.Say("autosave.message".I18N());
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
                    RocketChat.Say("autoresetitems.message".I18N());
                    this.autoresetitemsTime = DateTime.Now.AddSeconds(this.MainConfig.AutoResetItemsInterval);
                    this.resetitemswarningSend = false;
                }
                if (!this.resetitemswarningSend && (autoresetitemsTime.Value - DateTime.Now).TotalSeconds <= this.MainConfig.AutoResetItemsWarningTime)
                {
                    RocketChat.Say("autoresetitems.warningmessage".I18N(this.MainConfig.AutoResetItemsWarningTime));
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
                    if (steamid != 0)
                    {
                        RocketPlayer player = RocketPlayer.FromCSteamID(new CSteamID(steamid));
                        if (player != null)
                        {
                            try
                            {
                                if (DateTime.Now < info.time)
                                {
                                    player.Features.setGodMode(true);
                                    player.Teleport(new Vector3(0, 24, 0), 0);
                                }
                                else
                                {
                                    player.Features.setGodMode(false);
                                    player.Teleport(info.position, info.rotation);
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
                        p.Player.SteamChannel.send("tellPosition", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { item.Value });
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
                        p.Player.SteamChannel.send("tellPosition", ESteamCall.NOT_OWNER & ESteamCall.CLIENTS, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { new Vector3(0f, -3f, 0f) });
                    }
                }
            }
        }

        public void hideadmin()
        {
            if (MainConfig.HideAdminEnabled)
            {
                SteamPlayer[] players = PlayerTool.getSteamPlayers();
                HashSet<byte> adminids = new HashSet<byte>();
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].IsAdmin)
                    {
                        adminids.Add((byte)i);
                    }
                }
                foreach (SteamPlayer player in players)
                {
                    foreach (byte adminid in adminids)
                    {
                        Steam.send(player.SteamPlayerID.CSteamID, player.IsAdmin ? ESteamPacket.ADMINED : ESteamPacket.UNADMINED, new byte[] { player.IsAdmin ? (byte)7 : (byte)8, adminid }, 2, 0);
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
                this.hideadmin();
            }
        }
    }
}
