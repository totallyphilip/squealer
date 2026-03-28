using System.CommandLine;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class ExitCommand : ICustomCommand
    {
        public Command CreateCommand()
        {
            var command = new Command("exit", "Exit Squealer.");
            command.AddAlias("quit");
            command.SetHandler(() => Environment.Exit(0));
            return command;
        }
    }
}
