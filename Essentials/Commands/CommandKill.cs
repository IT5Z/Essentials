using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;
using UnityEngine;
using Steamworks;
using Essentials.Extensions;

namespace Essentials.Commands
{
    class CommandKill : IRocketCommand
    {
        public bool RunFromConsole
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return "kill";
            }
        }

        public string Help
        {
            get
            {
                return "Kill designated player";
            }
        }

        public void Execute(CSteamID caller, string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                SteamPlayer p;
                if (PlayerTool.tryGetSteamPlayer(command, out p))
                {
                    p.Player.PlayerLife.askDamage(100, Vector3.up * 10f, EDeathCause.KILL, ELimb.SKULL, caller);
                    RocketChatManager.Say(caller, "commands.kill.message".I18N(p.SteamPlayerID.CharacterName));
                }
                else
                {
                    RocketChatManager.Say(caller, "commands.generic.player.notFound".I18N());
                }
            }
            else
            {
                RocketChatManager.Say(caller, "Kill [SteamID | Player]");
            }
        }
    }
}
