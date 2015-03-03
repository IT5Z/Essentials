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
        public HashSet<string> vanishPlayers;

        protected override void Load()
        {
            plugin = this;
            mainconfig = new MainConfig();
            i18n = new I18N();
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
