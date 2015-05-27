using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Essentials.ConfigManager
{
    class MainConfig
    {
        public Config config;

        public string Lang
        {
            get
            {
                return config.getString(new string[] { "Essentials", "Lang" });
            }

            set
            {
                config.set(new string[] { "Essentials", "Lang" }, value);
            }
        }

        public bool AutoSaveEnabled
        {
            get
            {
                return config.getBoolean(new string[] { "Essentials", "AutoSave", "Enabled" });
            }

            set
            {
                config.set(new string[] { "Essentials", "AutoSave", "Enabled" }, value);
            }
        }

        public int AutoSaveInterval
        {
            get
            {
                return config.getInt(new string[] { "Essentials", "AutoSave", "Interval" });
            }

            set
            {
                config.set(new string[] { "Essentials", "AutoSave", "Interval" }, value);
            }
        }

        public bool AutoResetItemsEnabled
        {
            get
            {
                return config.getBoolean(new string[] { "Essentials", "AutoResetItems", "Enabled" });
            }

            set
            {
                config.set(new string[] { "Essentials", "AutoResetItems", "Enabled" }, value);
            }
        }

        public int AutoResetItemsInterval
        {
            get
            {
                return config.getInt(new string[] { "Essentials", "AutoResetItems", "Interval" });
            }

            set
            {
                config.set(new string[] { "Essentials", "AutoResetItems", "Interval" }, value);
            }
        }

        public int AutoResetItemsWarningTime
        {
            get
            {
                return config.getInt(new string[] { "Essentials", "AutoResetItems", "WarningTime" });
            }

            set
            {
                config.set(new string[] { "Essentials", "AutoResetItems", "WarningTime" }, value);
            }
        }

        public bool PlayerProtectEnabled
        {
            get
            {
                return config.getBoolean(new string[] { "Essentials", "PlayerProtect", "Enabled" });
            }

            set
            {
                config.set(new string[] { "Essentials", "PlayerProtect", "Enabled" }, value);
            }
        }

        public int PlayerProtectTime
        {
            get
            {
                return config.getInt(new string[] { "Essentials", "PlayerProtect", "Time" });
            }

            set
            {
                config.set(new string[] { "Essentials", "PlayerProtect", "Time" }, value);
            }
        }

        public bool HideAdminEnabled
        {
            get
            {
                return config.getBoolean(new string[] { "Essentials", "HideAdmin", "Enabled" });
            }

            set
            {
                config.set(new string[] { "Essentials", "HideAdmin", "Enabled" }, value);
            }
        }

        public MainConfig()
        {
            this.Init();
        }

        public void Init()
        {
            config = new Config(Util.getPluginFilePath("Essentials.config"));
            config.setDefault(new string[] { "Essentials", "Lang" }, "zh_CN", config.isString);
            config.setDefault(new string[] { "Essentials", "AutoSave", "Enabled" }, true, config.isBoolean);
            config.setDefault(new string[] { "Essentials", "AutoSave", "Interval" }, 600, config.isInt);
            config.setDefault(new string[] { "Essentials", "AutoResetItems", "Enabled" }, true, config.isBoolean);
            config.setDefault(new string[] { "Essentials", "AutoResetItems", "Interval" }, 3600, config.isInt);
            config.setDefault(new string[] { "Essentials", "AutoResetItems", "WarningTime" }, 180, config.isInt);
            config.setDefault(new string[] { "Essentials", "PlayerProtect", "Enabled" }, true, config.isBoolean);
            config.setDefault(new string[] { "Essentials", "PlayerProtect", "Time" }, 10, config.isInt);
            config.setDefault(new string[] { "Essentials", "HideAdmin", "Enabled" }, false, config.isBoolean);
            this.Save();
        }

        public void Load()
        {
            try
            {
                config.Load();
            }
            catch(IOException)
            {
                Init();
            }
        }

        public void Save()
        {
            config.Save();
        }
    }
}
