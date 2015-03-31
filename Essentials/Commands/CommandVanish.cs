using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;
using Rocket.Logging;
using Steamworks;
using Essentials.Extensions;

namespace Essentials.Commands
{
    class CommandVanish : IRocketCommand
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
                return "vanish";
            }
        }

        public string Help
        {
            get
            {
                return "Toggles invisibility";
            }
        }

        public void Execute(CSteamID caller, string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                HashSet<ulong> players = Plugin.instance.vanishedPlayers;
                if (players != null)
                {
                    SteamPlayer steamplayer;
                    if (PlayerTool.tryGetSteamPlayer(command, out steamplayer))
                    {
                        SteamPlayerID playerid = steamplayer.SteamPlayerID;
                        ulong steamid = playerid.CSteamID.m_SteamID;
                        string playername = playerid.CharacterName;
                        string sendername = PlayerTool.getSteamPlayer(caller).SteamPlayerID.CharacterName;
                        if (players.Contains(steamid))
                        {
                            players.Remove(steamid);
                            RocketChatManager.Say(caller, "commands.vanish.sender.off".I18N(playername));
                            if (steamid != caller.m_SteamID)
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.vanish.target.off".I18N(sendername));
                            }
                            Logger.Log(playername + " closed stealth");
                        }
                        else
                        {
                            players.Add(steamid);
                            RocketChatManager.Say(caller, "commands.vanish.sender.on".I18N(playername));
                            if (steamid != caller.m_SteamID)
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.vanish.target.on".I18N(sendername));
                            }
                            Logger.Log(playername + " opened stealth");
                        }
                    }
                    else
                    {
                        RocketChatManager.Say(caller, "commands.generic.player.notFound".I18N());
                    }
                }
            }
            else
            {
                RocketChatManager.Say(caller, "Vanish [SteamID | Player]");
            }
        }
    }
}
