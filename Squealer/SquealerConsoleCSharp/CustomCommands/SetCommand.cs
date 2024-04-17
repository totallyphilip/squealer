using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class SetCommand : ICustomeCommand
    {
        public Command CreateCommand()
        {
            var command = new Command("Set", "Edit Setting File.")
            {
                
            };

            command.SetHandler(HandleOpenCommand);

            return command;
        }

        private void HandleOpenCommand()
        {
            

        }
    }
}
