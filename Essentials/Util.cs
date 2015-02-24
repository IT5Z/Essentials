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
            ItemManager manager = (ItemManager)typeof(ItemManager).GetField("n", BindingFlags.NonPublic | BindingFlags.Static).GetValue(new ItemManager());
            FieldInfo info = typeof(ItemManager).GetField("U", BindingFlags.NonPublic | BindingFlags.Static);
            ItemRegion[,] temp = (ItemRegion[,])info.GetValue(manager);
            typeof(ItemManager).GetMethod("X", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(manager, new object[] { Level.R + 1 });
            ItemRegion[,] region = (ItemRegion[,])info.GetValue(manager);
            for (byte b = 0; b < Regions.u; b++)
            {
                for (byte b2 = 0; b2 < Regions.u; b2++)
                {
                    foreach (ItemData data in temp[b, b2].p)
                    {
                        manager.SteamChannel.send("tellTakeItem", ESteamCall.CLIENTS, b, b2, ItemManager.D, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.O });
                    }
                    foreach (ItemData data in region[b, b2].p)
                    {
                        manager.SteamChannel.send("tellItem", ESteamCall.CLIENTS, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { b, b2, data.r.ItemID, data.r.Metadata, data.O });
                    }
                }
            }
        }
    }
}
