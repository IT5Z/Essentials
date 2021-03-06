﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using UnityEngine;
using Essentials.Extensions;

namespace Essentials.Commands
{
    class CommandTpPos : IRocketCommand
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
                return "tppos";
            }
        }

        public List<string> Aliases
        {
            get
            {
                return new List<string>();
            }
        }

        public string Help
        {
            get
            {
                return "Teleport to position";
            }
        }

        public string Syntax
        {
            get
            {
                return "<SteamID | Player> [X] [Y] [Z]";
            }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            if (command.Length == 3 || command.Length == 4)
            {
                Player player = null;
                switch (command.Length)
                {
                    case 3:
                        player = caller != null ? caller.Player : null;
                        break;
                    case 4:
                        player = PlayerTool.getPlayer(command[0]);
                        break;
                }
                if (player != null)
                {
                    try
                    {
                        Vector3 position = player.transform.position;
                        float x = Util.parsePosition(command[command.Length - 3], position.x);
                        float y = Util.parsePosition(command[command.Length - 2], position.y);
                        float z = Util.parsePosition(command[command.Length - 1], position.z);
                        player.sendTeleport(new Vector3(x, y, z), MeasurementTool.angleToByte(player.transform.rotation.eulerAngles.y));
                        string target = string.Format("{0}, {1}, {2}", x, y, z);
                        SteamPlayerID steamplayerid = player.SteamChannel.SteamPlayer.SteamPlayerID;
                        RocketChat.Say(caller, "commands.tppos.sender.message".I18N(steamplayerid.CharacterName, target));
                        if (caller.CSteamID.m_SteamID != steamplayerid.CSteamID.m_SteamID)
                        {
                            RocketChat.Say(steamplayerid.CSteamID, "commands.tppos.target.message".I18N(caller.CharacterName, target));
                        }
                    }
                    catch (FormatException)
                    {
                        RocketChat.Say(caller, "commands.generic.args.invalid".I18N());
                    }
                }
                else
                {
                    RocketChat.Say(caller, "commands.generic.player.notFound".I18N());
                }
            }
            else
            {
                RocketChat.Say(caller, "TpPos <SteamID | Player> [X] [Y] [Z]");
            }
        }
    }
}
