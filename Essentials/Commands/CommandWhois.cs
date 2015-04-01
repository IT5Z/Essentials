using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;
using Steamworks;
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

        public void Execute(CSteamID caller, string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                SteamPlayer p;
                if (PlayerTool.tryGetSteamPlayer(command, out p))
                {
                    SteamPlayerID pi = p.SteamPlayerID;
                    Vector3 position = p.Player.transform.position;
                    PlayerLife life = p.Player.PlayerLife;
                    RocketChatManager.Say(caller, "commands.whois.info".I18N(pi.CharacterName, pi.SteamName, pi.ToString().Split(' ')[1], string.Format("{0}, {1}, {2}", position.x, position.y, position.z)));
                    RocketChatManager.Say(caller, "commands.whois.buff".I18N(life.Dead, life.Bleeding, life.Broken, life.Freezing, life.Breath));
                    RocketChatManager.Say(caller, "commands.whois.state".I18N(life.getBlood(), life.Hunger, life.Thirst, life.Infection, life.Stamina));
                }
                else
                {
                    RocketChatManager.Say(caller, "commands.generic.player.notFound".I18N());
                }
            }
            else
            {
                RocketChatManager.Say(caller, "Whois [SteamID | Player]");
            }
        }
    }
}
