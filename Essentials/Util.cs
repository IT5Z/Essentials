using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using System.Reflection;
using Rocket;

namespace Essentials
{
    class Util
    {
        public static string getPluginFilePath(string filename)
        {
            return string.Format("{0}Plugins/{1}/{2}", RocketSettings.HomeFolder, typeof(Plugin).Assembly.GetName().Name, filename);
        }

        public static void ResetItems()
        {
            ItemManager manager = typeof(ItemManager).GetField("U", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as ItemManager;
            FieldInfo info = typeof(ItemManager).GetField("T", BindingFlags.NonPublic | BindingFlags.Static);
            ItemRegion[,] temp = info.GetValue(null) as ItemRegion[,];
            typeof(ItemManager).GetMethod("U", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(manager, new object[] { int.MaxValue });
            ItemRegion[,] region = info.GetValue(null) as ItemRegion[,];
            for (byte b = 0; b < 64; b ++)
            {
                for (byte b2 = 0; b2 < 64; b2 ++)
                {
                    foreach (ItemData data in temp[b, b2].V)
                    {
                        manager.SteamChannel.send("tellTakeItem", ESteamCall.CLIENTS, b, b2, 1, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.A });
                    }
                    foreach (ItemData data in region[b, b2].V)
                    {
                        manager.SteamChannel.send("tellItem", ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.C.ItemID, data.C.Metadata, data.A });
                    }
                }
            }
        }
    }
}
