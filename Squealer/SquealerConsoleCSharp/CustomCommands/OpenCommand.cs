using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class OpenCommand: ICustomCommand
    {
        public Command CreateCommand()
        {
            var pathArgument = new Argument<string?>(
                name: "path",
                description: "Absolute file path.",
                getDefaultValue: () => null // Optional: Provide a default value or leave it as null if not provided
            );

            var command = new Command("open", "Open folder [<path>]. This folder path will be saved for quick access.")
            {
                pathArgument
            };


            command.SetHandler(HandleOpenCommand, pathArgument);

            return command;
        }

        private void HandleOpenCommand(string? path)
        {
            // If no path given, try to restore the last used folder.
            if (string.IsNullOrWhiteSpace(path))
            {
                path = AppState.Instance.Settings.LastProjectFolder;
                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.WriteLine("No path provided and no last folder saved.");
                    return;
                }
            }

            path = path.Trim();
            if (Directory.Exists(path))
            {
                AppState.Instance.LastOpenedPath = path;
                Console.WriteLine($"Current path: {AppState.Instance.LastOpenedPath}");

                var gitProjectName = GitHelper.GetGitProject();
                if (!string.IsNullOrEmpty(gitProjectName))
                {
                    AppState.Instance.GitProjectName = gitProjectName;
                }

                // Save last opened folder to settings.
                AppState.Instance.Settings.LastProjectFolder = path;
                Models.Settings.SaveSettings(AppState.Instance.Settings, Helper.GetSettingsPath());
            }
            else
            {
                Console.WriteLine("Invalid Path");
            }
        }
    }
}
