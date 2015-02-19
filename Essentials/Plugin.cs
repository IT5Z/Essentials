using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.RocketAPI;
using Rocket.Logging;
using SDG;
using Commands;

namespace Essentials
{
    internal class Plugin : RocketPlugin<Configuration>
    {
        public DateTime? autosavetime;

        protected override void Load()
        {
            Logger.Log("Essentials by Android is load");
            base.Load();
        }

        public void autoSave()
        {
            if(RocketPlugin<Configuration>.Configuration.EnabledAutoSave)
            {
                if(RocketPlugin.Loaded)
                {
                    if(!this.autosavetime.HasValue)
                    {
                        this.autosavetime = DateTime.Now.AddSeconds(RocketPlugin<Configuration>.Configuration.AutoSaveInterval);
                    }
                    if(DateTime.Now >= autosavetime) {
                        SaveManager.save();
                        RocketChatManager.Say(RocketPlugin<Configuration>.Configuration.AutoSaveMessage);
                        this.autosavetime = DateTime.Now.AddSeconds(RocketPlugin<Configuration>.Configuration.AutoSaveInterval);
                    }
                }
            }
        }

        public void Update()
        {
            autoSave();
        }
    }
}
