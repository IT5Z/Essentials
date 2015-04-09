using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using Rocket.RocketAPI;
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

        public string Help
        {
            get
            {
                return "Teleport to position";
            }
        }

        public void Execute(RocketPlayer caller, string command)
        {
            string[] componentsFromSerial = Parser.getComponentsFromSerial(command, '/');
            int length = componentsFromSerial.Length;
            if (length == 3 || length == 4)
            {
                Player player = null;
                switch (length)
                {
                    case 3:
                        player = caller.Player;
                        break;
                    case 4:
                        player = PlayerTool.getPlayer(componentsFromSerial[0]);
                        break;
                }
                if (player != null)
                {
                    try
                    {
                        Vector3 position = player.transform.position;
                        float x = Util.parsePosition(componentsFromSerial[length - 3], position.x);
                        float y = Util.parsePosition(componentsFromSerial[length - 2], position.y);
                        float z = Util.parsePosition(componentsFromSerial[length - 1], position.z);
                        player.sendTeleport(new Vector3(x, y, z), MeasurementTool.angleToByte(player.transform.rotation.eulerAngles.y));
                        string target = string.Format("{0}, {1}, {2}", x, y, z);
                        SteamPlayerID steamplayerid = player.SteamChannel.SteamPlayer.SteamPlayerID;
                        RocketChatManager.Say(caller, "commands.tppos.sender.message".I18N(steamplayerid.CharacterName, target));
                        if (caller.CSteamID.m_SteamID != steamplayerid.CSteamID.m_SteamID)
                        {
                            RocketChatManager.Say(steamplayerid.CSteamID, "commands.tppos.target.message".I18N(caller.CharacterName, target));
                        }
                    }
                    catch (FormatException)
                    {
                        RocketChatManager.Say(caller, "commands.generic.args.invalid".I18N());
                    }
                }
                else
                {
                    RocketChatManager.Say(caller, "commands.generic.player.notFound".I18N());
                }
            }
            else
            {
                RocketChatManager.Say(caller, "TpPos <SteamID | Player>/[X]/[Y]/[Z]");
            }
        }
    }
}
