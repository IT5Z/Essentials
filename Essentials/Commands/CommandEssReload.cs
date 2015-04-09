using System;
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

        public string Help
        {
            get
            {
                return "Reload essentials";
            }
        }

        public void Execute(RocketPlayer caller, string command)
        {
            Plugin.instance.MainConfig.Load();
            Plugin.instance.I18N.Load();
            RocketChatManager.Say(caller, "commands.essreload.message".I18N());
            Logger.Log(caller.CharacterName + " reload essentials");
        }
    }
}
