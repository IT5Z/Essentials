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
                Dictionary<string, Vector3> players = Plugin.instance.frozenPlayers;
                if (players != null)
                {
                    SteamPlayer steamplayer;
                    if (SteamPlayerlist.tryGetSteamPlayer(args, out steamplayer))
                    {
                        SteamPlayerID playerid = steamplayer.SteamPlayerID;
                        string name = playerid.CharacterName;
                        if (players.ContainsKey(name))
                        {
                            players.Remove(name);
                            RocketChatManager.Say(sender.CSteamID, "commands.freeze.sender.off".I18N(name));
                            string sendername = sender.CharacterName;
                            if (!name.Equals(sendername))
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.freeze.target.off".I18N(sendername));
                            }
                            Logger.Log("Freeze " + name);
                        }
                        else
                        {
                            players.Add(name, steamplayer.Player.transform.position);
                            RocketChatManager.Say(sender.CSteamID, "commands.freeze.sender.on".I18N(name));
                            string sendername = sender.CharacterName;
                            if (!name.Equals(sendername))
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.freeze.target.on".I18N(sendername));
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
