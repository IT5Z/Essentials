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

namespace Essentials.Commands
{
    class CommandWhois : IRocketCommand
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
                return "whois";
            }
        }

        public string Help
        {
            get
            {
                return "Display Player Information";
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
                    SteamPlayerID pi = p.SteamPlayerID;
                    Vector3 position = p.Player.transform.position;
                    PlayerLife life = p.Player.PlayerLife;
                    RocketChat.Say(caller, "commands.whois.info".I18N(pi.CharacterName, pi.SteamName, pi.ToString().Split(' ')[1], string.Format("{0}, {1}, {2}", Convert.ToInt32(position.x), Convert.ToInt32(position.y), Convert.ToInt32(position.z))));
                    RocketChat.Say(caller, "commands.whois.buff".I18N(life.Dead, life.Bleeding, life.Broken, life.Freezing, life.Breath));
                    RocketChat.Say(caller, "commands.whois.state".I18N(life.getBlood(), life.Hunger, life.Thirst, life.Infection, life.Stamina));
                }
                else
                {
                    RocketChat.Say(caller, "commands.generic.player.notFound".I18N());
                }
            }
            else
            {
                RocketChat.Say(caller, "Whois [SteamID | Player]");
            }
        }
    }
}
