using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using UnityEngine;
using Rocket.RocketAPI;
using Rocket.Logging;
using Essentials.Extensions;

namespace Essentials.Commands
{
    class CommandFreeze : IRocketCommand
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
                return "freeze";
            }
        }

        public string Help
        {
            get
            {
                return "Toggles freeze";
            }
        }

        public void Execute(RocketPlayer caller, string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                Dictionary<ulong, Vector3> players = Plugin.instance.frozenPlayers;
                if (players != null)
                {
                    SteamPlayer steamplayer;
                    if (PlayerTool.tryGetSteamPlayer(command, out steamplayer))
                    {
                        SteamPlayerID playerid = steamplayer.SteamPlayerID;
                        ulong steamid = playerid.CSteamID.m_SteamID;
                        string playername = playerid.CharacterName;
                        if (players.ContainsKey(steamid))
                        {
                            players.Remove(steamid);
                            RocketChatManager.Say(caller, "commands.freeze.sender.off".I18N(playername));
                            if (steamid != caller.CSteamID.m_SteamID)
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.freeze.target.off".I18N(caller.CharacterName));
                            }
                            Logger.Log("Freeze " + playername);
                        }
                        else
                        {
                            players.Add(steamid, steamplayer.Player.transform.position);
                            RocketChatManager.Say(caller, "commands.freeze.sender.on".I18N(playername));
                            if (steamid != caller.CSteamID.m_SteamID)
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.freeze.target.on".I18N(caller.CharacterName));
                            }
                            Logger.Log("Unfreeze " + playername);
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
                RocketChatManager.Say(caller, "Freeze [SteamID | Player]");
            }
        }
    }
}
