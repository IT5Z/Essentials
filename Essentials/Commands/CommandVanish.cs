﻿using System;
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
                HashSet<string> players = Plugin.instance.vanishedPlayers;
                if (players != null)
                {
                    SteamPlayer steamplayer;
                    if (SteamPlayerlist.tryGetSteamPlayer(args, out steamplayer))
                    {
                        SteamPlayerID playerid = steamplayer.SteamPlayerID;
                        string name = playerid.CharacterName;
                        if (players.Contains(name))
                        {
                            players.Remove(name);
                            RocketChatManager.Say(sender.CSteamID, "commands.vanish.sender.off".I18N(name));
                            string sendername = sender.CharacterName;
                            if (!name.Equals(sendername))
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.vanish.target.off".I18N(sendername));
                            }
                            Logger.Log(name + " closed stealth");
                        }
                        else
                        {
                            players.Add(name);
                            RocketChatManager.Say(sender.CSteamID, "commands.vanish.sender.on".I18N(name));
                            string sendername = sender.CharacterName;
                            if (!name.Equals(sendername))
                            {
                                RocketChatManager.Say(playerid.CSteamID, "commands.vanish.target.on".I18N(sendername));
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
