using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SDG.Unturned;

namespace Essentials.Extensions
{
    static class ItemManagerExtensions
    {
        public static ItemManager getItemManager()
        {
            return Util.getField<ItemManager, ItemManager>(null, BindingFlags.NonPublic | BindingFlags.Static);
        }

        public static ItemRegion[,] getItemRegion()
        {
            return Util.getField<ItemManager, ItemRegion[,]>(null, BindingFlags.NonPublic | BindingFlags.Static);
        }

        public static void resetItems(this ItemManager manager)
        {
            Util.invokeMethod<ItemManager>(manager, BindingFlags.NonPublic | BindingFlags.Instance, typeof(void), new Type[] { typeof(int) }, new object[] { int.MaxValue });
        }
    }
}
