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
            ItemManager manager = typeof(ItemManager).GetField("p", BindingFlags.NonPublic | BindingFlags.Static).GetValue(new ItemManager()) as ItemManager;
            FieldInfo info = typeof(ItemManager).GetField("m", BindingFlags.NonPublic | BindingFlags.Static);
            ItemRegion[,] temp = info.GetValue(manager) as ItemRegion[,];
            typeof(ItemManager).GetMethod("U", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(manager, new object[] { Level.k + 1 });
            ItemRegion[,] region = info.GetValue(manager) as ItemRegion[,];
            for (byte b = 0; b < Regions.U; b++)
            {
                for (byte b2 = 0; b2 < Regions.U; b2++)
                {
                    foreach (ItemData data in temp[b, b2].e)
                    {
                        manager.SteamChannel.send("tellTakeItem", ESteamCall.CLIENTS, b, b2, ItemManager.o, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.t });
                    }
                    foreach (ItemData data in region[b, b2].e)
                    {
                        manager.SteamChannel.send("tellItem", ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.A.ItemID, data.A.Metadata, data.t });
                    }
                }
            }
        }
    }
}
