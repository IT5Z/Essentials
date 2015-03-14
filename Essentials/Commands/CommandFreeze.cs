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
    class CommandFreeze : Command
    {
        public CommandFreeze()
        {
            base.commandName = "Freeze";
            base.commandHelp = "Toggles freeze";
            base.commandInfo = "Freeze [SteamID | Player]";
        }

        protected override void execute(SteamPlayerID sender, string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                Dictionary<ulong, Vector3> players = Plugin.instance.frozenPlayers;
                if (players != null)
                {
                    SteamPlayer steamplayer;
                    if (SteamPlayerlist.tryGetSteamPlayer(args, out steamplayer))
                    {
                        SteamPlayerID playerid = steamplayer.SteamPlayerID;
                        ulong steamid = playerid.CSteamID.m_SteamID;
                        string name = playerid.CharacterName;
                        if (players.ContainsKey(steamid))
                        {
                            players.Remove(steamid);
                            RocketChatManager.Say(sender.CSteamID, "commands.freeze.sender.off".I18N(name));
                            if (steamid != sender.CSteamID.m_SteamID)
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.freeze.target.off".I18N(sender.CharacterName));
                            }
                            Logger.Log("Freeze " + name);
                        }
                        else
                        {
                            players.Add(steamid, steamplayer.Player.transform.position);
                            RocketChatManager.Say(sender.CSteamID, "commands.freeze.sender.on".I18N(name));
                            if (steamid != sender.CSteamID.m_SteamID)
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.freeze.target.on".I18N(sender.CharacterName));
                            }
                            Logger.Log("Unfreeze " + name);
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
