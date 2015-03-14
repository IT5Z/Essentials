using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Essentials
{
    class I18N
    {
        private static readonly Dictionary<string, string> DefaultLang;
        private static readonly string DefaultLangName;
        private Dictionary<string, string> lang;

        static I18N()
        {
            DefaultLang = new Dictionary<string, string>();
            DefaultLangName = "zh_CN";
            DefaultLang.Add("autosave.message", "服务器自动保存完成");
            DefaultLang.Add("autoresetitems.message", "服务器已刷新地面物品");
            DefaultLang.Add("autoresetitems.warningmessage", "服务器将在{0}秒内刷新地面物品，请拾好重要物品。");
            DefaultLang.Add("playerprotect.message", "为了保护你的安全，你将在{0}秒后被传送回原位置。");
            DefaultLang.Add("commands.generic.player.notFound", "找不到玩家");
            DefaultLang.Add("commands.essreload.message", "已重载Essentials");
            DefaultLang.Add("commands.freeze.sender.on", "已冻结{0}");
            DefaultLang.Add("commands.freeze.sender.off", "已解冻{0}");
            DefaultLang.Add("commands.freeze.target.on", "你已被{0}冻结");
            DefaultLang.Add("commands.freeze.target.off", "你已被{0}解冻");
            DefaultLang.Add("commands.kill.message", "已处死{0}");
            DefaultLang.Add("commands.resetitems.message", "已刷新地面物品");
            DefaultLang.Add("commands.vanish.sender.on", "已开启{0}的隐身");
            DefaultLang.Add("commands.vanish.sender.off", "已关闭{0}的隐身");
            DefaultLang.Add("commands.vanish.target.on", "你已被{0}开启隐身");
            DefaultLang.Add("commands.vanish.target.off", "你已被{0}关闭隐身");
            DefaultLang.Add("commands.whois.info", "角色名称: {0} | Steam名称: {1} | 存档ID: {2} | 位置: {3}");
            DefaultLang.Add("commands.whois.buff", "死亡: {0} | 出血: {1} | 骨折: {2} | 冻伤: {3} | 氧气: {4}%");
            DefaultLang.Add("commands.whois.state", "血量: {0}% | 饱食: {1}% | 含水: {2}% | 健康: {3}% | 耐力: {4}%");
        }

        public I18N()
        {
            this.Load();
        }

        public void Load()
        {
            this.Load(Plugin.instance.MainConfig.Lang);
        }

        public void Load(string langname)
        {
            StreamReader reader = null;
            try
            {
                lang = new Dictionary<string, string>();
                reader = new StreamReader(Util.getPluginFilePath("Lang/" + langname + ".lang"));
                string[] temp = null;
                while (!reader.EndOfStream)
                {
                    temp = reader.ReadLine().Split(new char[] { '=' }, 2);
                    lang.Add(temp[0], temp[1]);
                }
            }
            catch (IOException)
            {
                I18N.WriteDefault();
                this.Load(I18N.DefaultLangName);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        public static void WriteDefault()
        {
            StreamWriter writer = null;
            try
            {
                string dirpath = Util.getPluginFilePath("Lang");
                if (!Directory.Exists(dirpath)) Directory.CreateDirectory(dirpath);
                writer = new StreamWriter(Util.getPluginFilePath("Lang/" + I18N.DefaultLangName + ".lang"));
                foreach(KeyValuePair<string, string> item in I18N.DefaultLang)
                {
                    writer.WriteLine(item.Key + "=" + item.Value);
                }
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

        public string Format(string key)
        {
            try
            {
                return this.lang != null ? this.lang[key] : null;
            }
            catch(KeyNotFoundException)
            {
                return I18N.DefaultLang != null ? I18N.DefaultLang[key] : null;
            }
        }
    }
}
