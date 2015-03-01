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
            ItemManager manager = typeof(ItemManager).GetField("g", BindingFlags.NonPublic | BindingFlags.Static).GetValue(new ItemManager()) as ItemManager;
            FieldInfo info = typeof(ItemManager).GetField("K", BindingFlags.NonPublic | BindingFlags.Static);
            ItemRegion[,] temp = info.GetValue(manager) as ItemRegion[,];
            typeof(ItemManager).GetMethod("J", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(manager, new object[] { Level.e + 1 });
            ItemRegion[,] region = info.GetValue(manager) as ItemRegion[,];
            for (byte b = 0; b < Regions.J; b++)
            {
                for (byte b2 = 0; b2 < Regions.J; b2++)
                {
                    foreach (ItemData data in temp[b, b2].I)
                    {
                        manager.SteamChannel.send("tellTakeItem", ESteamCall.CLIENTS, b, b2, ItemManager.c, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.L });
                    }
                    foreach (ItemData data in region[b, b2].I)
                    {
                        manager.SteamChannel.send("tellItem", ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.k.ItemID, data.k.Metadata, data.L });
                    }
                }
            }
        }
    }
}
