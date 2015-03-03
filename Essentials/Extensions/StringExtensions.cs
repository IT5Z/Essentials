using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essentials.Extensions
{
    static class StringExtensions
    {
        public static string I18N(this string key, params object[] args)
        {
            return string.Format(Plugin.instance.I18N.Format(key), args);
        }
    }
}
