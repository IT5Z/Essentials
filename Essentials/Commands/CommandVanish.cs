using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
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

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length == 1)
            {
                HashSet<ulong> players = Plugin.instance.vanishedPlayers;
                if (players != null)
                {
                    SteamPlayer steamplayer;
                    if (PlayerTool.tryGetSteamPlayer(command[0], out steamplayer))
                    {
                        SteamPlayerID playerid = steamplayer.SteamPlayerID;
                        ulong steamid = playerid.CSteamID.m_SteamID;
                        string playername = playerid.CharacterName;
                        if (players.Contains(steamid))
                        {
                            players.Remove(steamid);
                            RocketChat.Say(caller, "commands.vanish.sender.off".I18N(playername));
                            if (steamid != caller.CSteamID.m_SteamID)
                            {
                                RocketChat.Say(playerid.CSteamID, "commands.vanish.target.off".I18N(caller.CharacterName));
                            }
                            Logger.Log(playername + " closed stealth");
                        }
                        else
                        {
                            players.Add(steamid);
                            RocketChat.Say(caller, "commands.vanish.sender.on".I18N(playername));
                            if (steamid != caller.CSteamID.m_SteamID)
                            {
                                RocketChat.Say(playerid.CSteamID, "commands.vanish.target.on".I18N(caller.CharacterName));
                            }
                            Logger.Log(playername + " opened stealth");
                        }
                    }
                    else
                    {
                        RocketChat.Say(caller, "commands.generic.player.notFound".I18N());
                    }
                }
            }
            else
            {
                RocketChat.Say(caller, "Vanish [SteamID | Player]");
            }
        }
    }
}
