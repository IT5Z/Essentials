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
        public AutoSave ConfigAutoSave;

        public class AutoSave
        {
            public bool Enabled;
            public int Interval;
            public string Message;
        }

        public RocketConfiguration DefaultConfiguration
        {
            get
            {
                return new Configuration
                {
                    ConfigAutoSave = new AutoSave
                    {
                        Enabled = true,
                        Interval = 600,
                        Message = "服务器自动保存完成"
                    }
                };
            }
        }
    }
}
