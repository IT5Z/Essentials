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
    class CommandKill : Command
    {
        public CommandKill()
        {
            base.commandName = "Kill";
            base.commandHelp = "Kill [SteamID | Player]";
            base.commandInfo = "Kill designated player";
        }

        protected override void execute(SteamPlayerID b, string K)
        {
            if (!string.IsNullOrEmpty(K))
            {
                SteamPlayer p;
                if (SteamPlayerlist.tryGetSteamPlayer(K, out p))
                {
                    p.Player.PlayerLife.askDamage(100, Vector3.up * 10f, EDeathCause.KILL, ELimb.SKULL, b.CSteamID);
                    RocketChatManager.Say(b.CSteamID, "已杀死 " + p.SteamPlayerID.CharacterName);
                }
                else
                {
                    RocketChatManager.Say(b.CSteamID, "玩家不存在");
                }
            }
            else
            {
                RocketChatManager.Say(b.CSteamID, base.commandHelp);
            }
            base.execute(b, K);
        }
    }
}
