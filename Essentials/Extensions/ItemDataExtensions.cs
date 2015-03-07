using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SDG;
using UnityEngine;

namespace Essentials.Extensions
{
    static class ItemDataExtensions
    {
        public static Vector3 getVector3(this ItemData itemdata)
        {
            return Util.getField<ItemData, Vector3>(itemdata, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static Item getItem(this ItemData itemdata)
        {
            return Util.getField<ItemData, Item>(itemdata, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}
