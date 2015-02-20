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
            if(RocketPlugin.Loaded && RocketPlugin<Configuration>.Configuration.ConfigAutoSave != null && RocketPlugin<Configuration>.Configuration.ConfigAutoSave.Enabled)
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

        public void Update()
        {
            autoSave();
        }
    }
}
