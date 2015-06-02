using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG;
using System.Reflection;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
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
                return "Reset ground items";
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
            Util.ResetItems();
            RocketChat.Say(caller, "commands.resetitems.message".I18N());
            Logger.Log(caller != null ? caller.CharacterName : "Console" + " reset ground items");
        }
    }
}
