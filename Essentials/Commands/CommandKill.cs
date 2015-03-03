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
    class CommandKill : Command
    {
        public CommandKill()
        {
            base.commandName = "Kill";
            base.commandHelp = "Kill designated player";
            base.commandInfo = "Kill [SteamID | Player]";
        }

        protected override void execute(SteamPlayerID sender, string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                SteamPlayer p;
                if (SteamPlayerlist.tryGetSteamPlayer(args, out p))
                {
                    p.Player.PlayerLife.askDamage(100, Vector3.up * 10f, EDeathCause.KILL, ELimb.SKULL, sender.CSteamID);
                    RocketChatManager.Say(sender.CSteamID, "commands.kill.message".I18N(p.SteamPlayerID.CharacterName));
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
