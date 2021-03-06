﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using Essentials.Extensions;

namespace Essentials.Commands
{
    class CommandEssReload : IRocketCommand
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
                return "essreload";
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
                return "Reload essentials";
            }
        }

        public string Syntax
        {
            get
            {
                return string.Empty;
            }
        }

        public void Execute(RocketPlayer caller, string[] command)
        {
            Plugin.instance.MainConfig.Load();
            Plugin.instance.I18N.Load();
            RocketChat.Say(caller, "commands.essreload.message".I18N());
            Logger.Log(caller != null ? caller.CharacterName : "Console" + " reload essentials");
        }
    }
}
