using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.RocketAPI;

namespace Essentials
{
    public class Configuration : RocketConfiguration
    {
        public bool EnabledAutoSave;
        public int AutoSaveInterval;
        public string AutoSaveMessage;

        public RocketConfiguration DefaultConfiguration
        {
            get
            {
                return new Configuration
                {
                    EnabledAutoSave = true,
                    AutoSaveInterval = 600,
                    AutoSaveMessage = "服务器自动保存完成"
                };
            }
        }
    }
}
