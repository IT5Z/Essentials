using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;

namespace Commands
{
    class CommandWhois : Command
    {
        public CommandWhois()
        {
            base.commandName = "Whois";
            base.commandHelp = "Whois [SteamID | Player]";
            base.commandInfo = "Display Player Information";
        }

        protected override void execute(SteamPlayerID b, string K)
        {
            if(K.Length != 0)
            {
                SteamPlayer p;
                if (SteamPlayerlist.tryGetSteamPlayer(K, out p))
                {
                    SteamPlayerID pi = p.SteamPlayerID;
                    RocketChatManager.Say(b.CSteamID, "人物名称: " + pi.CharacterName + " | Steam名称: " + pi.SteamName + " | 存档ID: " + pi.Y + "-" + pi.x);
                    PlayerLife life = p.Player.PlayerLife;
                    RocketChatManager.Say(b.CSteamID, "死亡: " + life.Dead + " | 出血: " + life.Bleeding + " | 骨折: " + life.Broken + " | 冻伤: " + life.Freezing + " | 氧气: " + life.Breath + "%");
                    RocketChatManager.Say(b.CSteamID, "生命: " + life.v + "% | 饱食: " + life.Hunger + "% | 含水: " + life.Thirst + "% | 健康: " + life.Infection + "% | 耐力: " + life.Stamina + "%");
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
