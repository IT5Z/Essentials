using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Unturned.Player;

namespace Essentials.Extensions
{
    static class RocketPlayerFeaturesExtensions
    {
        public static void setGodMode(this RocketPlayerFeatures player, bool mode)
        {
            if (player.GodMode != mode)
            {
                player.GodMode = mode;
            }
        }
    }
}
