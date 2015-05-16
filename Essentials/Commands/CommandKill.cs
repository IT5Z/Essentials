using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;
using UnityEngine;
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

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length == 1)
            {
                SteamPlayer p;
                if (PlayerTool.tryGetSteamPlayer(command[0], out p))
                {
                    EPlayerKill ePlayerKill;
                    p.Player.PlayerLife.askDamage(100, Vector3.up * 10f, EDeathCause.KILL, ELimb.SKULL, caller.CSteamID, out ePlayerKill);
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
