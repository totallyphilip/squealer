using Spectre.Console;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class ClearCommand : ICustomCommand
    {
        public Command CreateCommand()
        {
            var clearCommand = new Command("clear", "Clears the console window.");
            clearCommand.AddAlias("cls");

            clearCommand.SetHandler(() =>
            {
                Console.Clear();
            });

            return clearCommand;

        }
    }
}
