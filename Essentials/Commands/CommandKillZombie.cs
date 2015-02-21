using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;
using UnityEngine;

namespace Commands
{
    class CommandTime : Command
    {
        public CommandTime()
        {
            base.commandName = "KillZombies";
            base.commandHelp = "KillZombies";
            base.commandInfo = "Kill all the zombies";
        }
        protected override void execute(SteamPlayerID b, string K)
        {
            Zombie[] Zombies = UnityEngine.Object.FindObjectsOfType(typeof(Zombie)) as Zombie[];
            foreach (Zombie Zombie in Zombies)
            {
                Zombie.tellDead(Vector3.up * 10f);
            }

            base.execute(b, K);
        }
    }
}
