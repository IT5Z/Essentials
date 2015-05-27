using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using UnityEngine;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
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

        public string Syntax
        {
            get
            {
                return "[SteamID | Player]";
            }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length == 1)
            {
                Dictionary<ulong, Vector3> players = Plugin.instance.frozenPlayers;
                if (players != null)
                {
                    SteamPlayer steamplayer;
                    if (PlayerTool.tryGetSteamPlayer(command[0], out steamplayer))
                    {
                        SteamPlayerID playerid = steamplayer.SteamPlayerID;
                        ulong steamid = playerid.CSteamID.m_SteamID;
                        string playername = playerid.CharacterName;
                        if (players.ContainsKey(steamid))
                        {
                            players.Remove(steamid);
                            RocketChat.Say(caller, "commands.freeze.sender.off".I18N(playername));
                            if (caller == null || (steamid != caller.CSteamID.m_SteamID))
                            {
                                RocketChat.Say(playerid.CSteamID, "commands.freeze.target.off".I18N(caller != null ? caller.CharacterName : "Console"));
                            }
                            Logger.Log("Unfreeze " + playername);
                        }
                        else
                        {
                            players.Add(steamid, steamplayer.Player.transform.position);
                            RocketChat.Say(caller, "commands.freeze.sender.on".I18N(playername));
                            if (caller == null || (steamid != caller.CSteamID.m_SteamID))
                            {
                                RocketChat.Say(playerid.CSteamID, "commands.freeze.target.on".I18N(caller != null ? caller.CharacterName : "Console"));
                            }
                            Logger.Log("Freeze " + playername);
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
                RocketChat.Say(caller, "Freeze [SteamID | Player]");
            }
        }
    }
}
