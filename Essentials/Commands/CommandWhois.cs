using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;

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
                    RocketChatManager.Say(sender.CSteamID, "人物名称: " + pi.CharacterName + " | Steam名称: " + pi.SteamName + " | 存档ID: " + pi.N + "-" + pi.d);
                    PlayerLife life = p.Player.PlayerLife;
                    RocketChatManager.Say(sender.CSteamID, "死亡: " + life.Dead + " | 出血: " + life.Bleeding + " | 骨折: " + life.Broken + " | 冻伤: " + life.Freezing + " | 氧气: " + life.Breath + "%");
                    RocketChatManager.Say(sender.CSteamID, "生命: " + life.a + "% | 饱食: " + life.Hunger + "% | 含水: " + life.Thirst + "% | 健康: " + life.Infection + "% | 耐力: " + life.Stamina + "%");
                }
                else
                {
                    RocketChatManager.Say(sender.CSteamID, "找不到玩家");
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
