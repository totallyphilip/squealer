using Spectre.Console;
using System.CommandLine;
using System.Reflection;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class AboutCommand : ICustomCommand
    {
        public Command CreateCommand()
        {
            var command = new Command("about", "Display version and application info.");
            command.SetHandler(Handle);
            return command;
        }

        private static void Handle()
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            AnsiConsole.MarkupLine($"[bold blue]Squealer[/] [green]{version}[/]");
            AnsiConsole.MarkupLine("A Windows console application to simplify creation and management of");
            AnsiConsole.MarkupLine("stored procedures, views, and functions.");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[link]{Constants.HomePage}[/]");
        }
    }
}
