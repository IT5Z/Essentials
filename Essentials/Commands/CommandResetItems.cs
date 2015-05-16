﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using System.Reflection;
using Rocket.RocketAPI;
using Rocket.Logging;
using Essentials.Extensions;

namespace Essentials.Commands
{
    class CommandResetItems : IRocketCommand
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
                return "resetitems";
            }
        }

        public string Help
        {
            get
            {
                return "Reset ground items";
            }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            Util.ResetItems();
            RocketChatManager.Say(caller, "commands.resetitems.message".I18N());
            Logger.Log(caller.CharacterName + " reset ground items");
        }
    }
}
