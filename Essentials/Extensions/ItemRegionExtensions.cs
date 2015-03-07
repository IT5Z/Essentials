using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SDG;

namespace Essentials.Extensions
{
    static class ItemRegionExtensions
    {
        public static List<ItemData> getItemData(this ItemRegion region)
        {
            return Util.getField<ItemRegion, List<ItemData>>(region, BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
