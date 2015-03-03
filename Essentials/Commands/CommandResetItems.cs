using System;
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
    class CommandResetItems : Command
    {
        public CommandResetItems()
        {
            base.commandName = "ResetItems";
            base.commandHelp = "Reset ground items";
            base.commandInfo = "ResetItems";
        }

        protected override void execute(SteamPlayerID sender, string args)
        {
            Util.ResetItems();
            RocketChatManager.Say(sender.CSteamID, "commands.resetitems.message".I18N());
            Logger.Log(sender.CharacterName + " reset ground items");
            base.execute(sender, args);
        }
    }
}
