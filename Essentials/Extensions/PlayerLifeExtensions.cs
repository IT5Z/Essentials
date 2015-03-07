using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SDG;

namespace Essentials.Extensions
{
    static class PlayerLifeExtensions
    {
        public static byte getBlood(this PlayerLife life)
        {
            return Util.getOffsetField<PlayerLife, byte>(life, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, "Health", 1);
        }
    }
}
