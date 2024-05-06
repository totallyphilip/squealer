using SquealerConsoleCSharp.Models;
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
        private FileSystemWatcher _watcher;
        public Command CreateCommand()
        {
            var command = new Command("set", "Edit Setting File.")
            {
                
            };

            command.SetHandler(HandleOpenCommand);

            return command;
        }

        private void HandleOpenCommand()
        {
            var settingsPaths = Helper.GetSettingsPath();

            Helper.OpenFileWithDefaultProgram(settingsPaths);

            _watcher = new FileSystemWatcher();
            _watcher.Path = Path.GetDirectoryName(settingsPaths);
            _watcher.Filter = Path.GetFileName(settingsPaths);
            _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;

            // Add event handlers
            _watcher.Changed += OnChanged;
            _watcher.Deleted += OnChanged;
            _watcher.Renamed += OnChanged;

            // Begin watching
            _watcher.EnableRaisingEvents = true;

        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File {e.FullPath} has been modified or deleted.");
            try
            {
                // Reload the settings
                AppState.Instance.Settings = Settings.LoadSettings(e.FullPath);
                Console.WriteLine("Settings reloaded successfully.\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reloading settings: {ex.Message}");
            }
        }
    }
}
