using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using UnityEngine;
using Essentials.Extensions;
using Steamworks;

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

        public List<string> Aliases
        {
            get
            {
                return new List<string>();
            }
        }

        public string Help
        {
            get
            {
                return "Kill designated player";
            }
        }

        public string Syntax
        {
            get
            {
                return "[SteamID | Player]";
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
                    p.Player.PlayerLife.askDamage(100, Vector3.up * 10f, EDeathCause.KILL, ELimb.SKULL, caller != null ? caller.CSteamID : new CSteamID(), out ePlayerKill);
                    RocketChat.Say(caller, "commands.kill.message".I18N(p.SteamPlayerID.CharacterName));
                }
                else
                {
                    RocketChat.Say(caller, "commands.generic.player.notFound".I18N());
                }
            }
            else
            {
                RocketChat.Say(caller, "Kill [SteamID | Player]");
            }
        }
    }
}
