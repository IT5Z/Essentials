using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;
using Rocket.Logging;
using Essentials.Extensions;

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
            if (!string.IsNullOrEmpty(args))
            {
                HashSet<ulong> players = Plugin.instance.vanishedPlayers;
                if (players != null)
                {
                    SteamPlayer steamplayer;
                    if (SteamPlayerlist.tryGetSteamPlayer(args, out steamplayer))
                    {
                        SteamPlayerID playerid = steamplayer.SteamPlayerID;
                        ulong steamid = playerid.CSteamID.m_SteamID;
                        string name = playerid.CharacterName;
                        if (players.Contains(steamid))
                        {
                            players.Remove(steamid);
                            RocketChatManager.Say(sender.CSteamID, "commands.vanish.sender.off".I18N(name));
                            if (steamid != sender.CSteamID.m_SteamID)
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.vanish.target.off".I18N(sender.CharacterName));
                            }
                            Logger.Log(name + " closed stealth");
                        }
                        else
                        {
                            players.Add(steamid);
                            RocketChatManager.Say(sender.CSteamID, "commands.vanish.sender.on".I18N(name));
                            if (steamid != sender.CSteamID.m_SteamID)
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.vanish.target.on".I18N(sender.CharacterName));
                            }
                            Logger.Log(name + " opened stealth");
                        }
                    }
                    else
                    {
                        RocketChatManager.Say(sender.CSteamID, "commands.generic.player.notFound".I18N());
                    }
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
