using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using System.Reflection;

namespace Essentials
{
    class Util
    {
        public static void ResetItems()
        {
            ItemManager manager = typeof(ItemManager).GetField("e", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as ItemManager;
            FieldInfo info = typeof(ItemManager).GetField("V", BindingFlags.NonPublic | BindingFlags.Static);
            ItemRegion[,] temp = info.GetValue(null) as ItemRegion[,];
            typeof(ItemManager).GetMethod("V", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(manager, new object[] { Level.e + 1 });
            ItemRegion[,] region = info.GetValue(null) as ItemRegion[,];
            for (byte b = 0; b < Regions.e; b ++)
            {
                for (byte b2 = 0; b2 < Regions.e; b2 ++)
                {
                    foreach (ItemData data in temp[b, b2].Q)
                    {
                        manager.SteamChannel.send("tellTakeItem", ESteamCall.CLIENTS, b, b2, ItemManager.X, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.m });
                    }
                    foreach (ItemData data in region[b, b2].Q)
                    {
                        manager.SteamChannel.send("tellItem", ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.s.ItemID, data.s.Metadata, data.m });
                    }
                }
            }
        }
    }
}
