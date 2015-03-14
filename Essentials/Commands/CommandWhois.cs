using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;
using Essentials.Extensions;
using UnityEngine;

namespace Essentials.Commands
{
    class CommandWhois : Command
    {
        public CommandWhois()
        {
            base.commandName = "Whois";
            base.commandHelp = "Display Player Information";
            base.commandInfo = "Whois [SteamID | Player]";
        }

        protected override void execute(SteamPlayerID sender, string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                SteamPlayer p;
                if (SteamPlayerlist.tryGetSteamPlayer(args, out p))
                {
                    SteamPlayerID pi = p.SteamPlayerID;
                    string[] info = pi.ToString().Split(' ');
                    string position = p.Player.transform.position.ToString();
                    PlayerLife life = p.Player.PlayerLife;
                    RocketChatManager.Say(sender.CSteamID, "commands.whois.info".I18N(pi.CharacterName, pi.SteamName, info[1] + "-" + info[2], position.Substring(1, position.Length - 2)));
                    RocketChatManager.Say(sender.CSteamID, "commands.whois.buff".I18N(life.Dead, life.Bleeding, life.Broken, life.Freezing, life.Breath));
                    RocketChatManager.Say(sender.CSteamID, "commands.whois.state".I18N(life.getBlood(), life.Hunger, life.Thirst, life.Infection, life.Stamina));
                }
                else
                {
                    RocketChatManager.Say(sender.CSteamID, "commands.generic.player.notFound".I18N());
                }
            }
            else
            {
                RocketChatManager.Say(sender.CSteamID, base.commandInfo);
            }
            base.execute(sender, args);
        }
    }
}
