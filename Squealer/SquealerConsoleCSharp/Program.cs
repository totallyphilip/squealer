using Spectre.Console;
using SquealerConsoleCSharp.CustomCommands;
using SquealerConsoleCSharp.Models;
using System.CommandLine;
using System.Resources;

namespace SquealerConsoleCSharp
{
    internal class Program
    {
        private static KeepAlive? _keepAlive;

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

                // Restore last opened folder if it still exists.
                var lastFolder = AppState.Instance.Settings.LastProjectFolder;
                if (!string.IsNullOrWhiteSpace(lastFolder) && Directory.Exists(lastFolder))
                {
                    AppState.Instance.LastOpenedPath = lastFolder;
                    AppState.Instance.GitProjectName = GitHelper.GetGitProject();
                }

                ApplyStartupSettings(AppState.Instance.Settings);
            }
            catch (Exception ex)
            {
                Logging.WriteLog($"Cannot load settings: {ex.Message}");
                AnsiConsole.MarkupLine("[red]Cannot load settings.[/]");
            }


            var rootCommand = new RootCommand("Squealer");

            rootCommand.AddCommand(new OpenCommand().CreateCommand());
            rootCommand.AddCommand(new DirectoryCommand().CreateCommand());
            rootCommand.AddCommand(new GenCommand().CreateCommand());
            rootCommand.AddCommand(new NewCommand().CreateCommand());
            rootCommand.AddCommand(new ClearCommand().CreateCommand());
            rootCommand.AddCommand(new SetCommand().CreateCommand());
            rootCommand.AddCommand(new TestCommand().CreateCommand());
            rootCommand.AddCommand(new FixCommand().CreateCommand());
            rootCommand.AddCommand(new ExitCommand().CreateCommand());
            rootCommand.AddCommand(new AboutCommand().CreateCommand());
            rootCommand.AddCommand(new EditCommand().CreateCommand());

            while (true)
            {
                string directoryCharacter = Helper.GetPromptText() + " >";
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

        private static void ApplyStartupSettings(Settings settings)
        {
            if (settings.KeepScreenAlive)
            {
                _keepAlive = new KeepAlive();
                _keepAlive.KeepMonitorActive();
            }

            // Check for updates at most once per day.
            if (settings.LastVersionCheckDate.Date < DateTime.Today)
            {
                try
                {
                    var checker = new VersionCheck();
                    checker.DisplayVersionCheckResults("https://s3-us-west-1.amazonaws.com/public-10ec013b-b521-4150-9eab-56e1e1bb63a4/Squealer/");
                    settings.LastVersionCheckDate = DateTime.Today;
                    Settings.SaveSettings(settings, Helper.GetSettingsPath());
                }
                catch (Exception ex)
                {
                    Logging.WriteLog($"Version check failed: {ex.Message}");
                }
            }
        }

    }
}
