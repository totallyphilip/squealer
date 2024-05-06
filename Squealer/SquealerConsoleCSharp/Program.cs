using Spectre.Console;
using SquealerConsoleCSharp.CustomCommands;
using SquealerConsoleCSharp.Models;
using System.CommandLine;
using System.Resources;

namespace SquealerConsoleCSharp
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            AnsiConsole.Write(
                new FigletText("Squealer")
                    .LeftJustified()
                    .Color(Color.Blue));

            try
            {
                var settingsPaths = Helper.GetSettingsPath();
                if (!File.Exists(settingsPaths))
                {
                    var newSettings = Settings.GetNewSettings();
                    Settings.SaveSettings(newSettings, settingsPaths);
                }

                AppState.Instance.Settings = Settings.LoadSettings(settingsPaths);

                AnsiConsole.MarkupLine($"Load Settings from {settingsPaths}");

            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("Cannot load settings");
            }


            var rootCommand = new RootCommand("Squealer");

            rootCommand.AddCommand(new OpenCommand().CreateCommand());
            rootCommand.AddCommand(new DirectoryCommand().CreateCommand());
            rootCommand.AddCommand(new GenCommand().CreateCommand());
            rootCommand.AddCommand(new NewCommand().CreateCommand());
            rootCommand.AddCommand(new ClearCommand().CreateCommand());
            rootCommand.AddCommand(new SetCommand().CreateCommand());


            while (true)
            {
                string directoryCharacter = GitHelper.GetGitProejctBranchName() + " >";
                string[] inputArgs;


                AnsiConsole.Markup($"{directoryCharacter}");
                string? input = Console.ReadLine();
                AnsiConsole.WriteLine();
                if ( string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if(input.Length > 1000)
                {
                    AnsiConsole.MarkupLine("[red]Cannot exceed 1000 chars limit.[/]");
                    continue;
                }

                if (input.StartsWith("open", StringComparison.OrdinalIgnoreCase))
                {
                    var indexOfFirstSpace = input.IndexOf(' ');
                    if (indexOfFirstSpace != -1)
                    {
                        inputArgs = new string[] { input.Substring(0, indexOfFirstSpace), input.Substring(indexOfFirstSpace) };
                    }
                    else
                    {
                        inputArgs = input.Split(' ');
                    }
                }
                else 
                { 
                    inputArgs = input.Split(' ');
                }
                // Invoke the command system with the input arguments
                await rootCommand.InvokeAsync(inputArgs);
                AnsiConsole.WriteLine();
            }
        }



    }
}
