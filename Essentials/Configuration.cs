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
        public AutoResetItems ConfigAutoResetItems;

        public class AutoSave
        {
            public bool Enabled;
            public int Interval;
            public string Message;
        }

        public class AutoResetItems
        {
            public bool Enabled;
            public int Interval;
            public string Message;
            public int WarningTime;
            public string WarningMessage;
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
                    },
                    ConfigAutoResetItems = new AutoResetItems
                    {
                        Enabled = true,
                        Interval = 3600,
                        Message = "服务器已刷新地面物品",
                        WarningTime = 180,
                        WarningMessage = "服务器将在3分钟内刷新地面物品，请拾好重要物品。"
                    }
                };
            }
        }
    }
}
