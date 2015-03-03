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
    class CommandEssReload : Command
    {
        public CommandEssReload()
        {
            base.commandName = "EssReload";
            base.commandHelp = "Reload essentials";
            base.commandInfo = "EssReload";
        }

        protected override void execute(SteamPlayerID sender, string args)
        {
            Plugin.instance.MainConfig.Load();
            Plugin.instance.I18N.Load();
            RocketChatManager.Say(sender.CSteamID, "commands.essreload.message".I18N());
            Logger.Log(sender.CharacterName + " reload essentials");
            base.execute(sender, args);
        }
    }
}
