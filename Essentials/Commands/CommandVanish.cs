using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;
using Rocket.Logging;

namespace Essentials.Commands
{
    class CommandVanish : Command
    {
        public CommandVanish()
        {
            base.commandName = "Vanish";
            base.commandHelp = "Toggles invisibility";
            base.commandInfo = "Vanish [SteamID | Player]";
        }

        protected override void execute(SteamPlayerID sender, string args)
        {
            HashSet<string> players = Plugin.instance.vanishPlayers;
            if (players != null)
            {
                SteamPlayer player;
                if (SteamPlayerlist.tryGetSteamPlayer(args, out player))
                {
                    SteamPlayerID playerid = player.SteamPlayerID;
                    string name = playerid.CharacterName;
                    if (players.Contains(name))
                    {
                        players.Remove(name);
                        RocketChatManager.Say(sender.CSteamID, "已关闭" + name + "的隐身");
                        string sendername = sender.CharacterName;
                        if (!name.Equals(sendername))
                        {
                            RocketChatManager.Say(playerid.CSteamID, "你已被" + sendername + "关闭隐身");
                        }
                        Logger.Log(name + " closed stealth");
                    }
                    else
                    {
                        players.Add(name);
                        RocketChatManager.Say(sender.CSteamID, "已开启" + name + "的隐身");
                        string sendername = sender.CharacterName;
                        if (!name.Equals(sendername))
                        {
                            RocketChatManager.Say(playerid.CSteamID, "你已被" + sendername + "开启隐身");
                        }
                        Logger.Log(name + "opened stealth");
                    }
                }
                else
                {
                    RocketChatManager.Say(sender.CSteamID, "找不到玩家");
                }
            }
            base.execute(sender, args);
        }
    }
}
