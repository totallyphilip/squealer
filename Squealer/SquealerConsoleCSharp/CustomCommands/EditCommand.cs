using Spectre.Console;
using System.CommandLine;
using System.Diagnostics;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class EditCommand : ICustomCommand
    {
        public Command CreateCommand()
        {
            var fileArgument = new Argument<string?>(
                name: "filename",
                description: "Name of the .sqlr file to open (without extension).",
                getDefaultValue: () => null);

            var command = new Command("edit", "Open a .sqlr file in the configured text editor.")
            {
                fileArgument
            };

            command.SetHandler(Handle, fileArgument);
            return command;
        }

        private static void Handle(string? filename)
        {
            if (!Helper.VadilateFolder()) return;

            if (string.IsNullOrWhiteSpace(filename))
            {
                AnsiConsole.MarkupLine("[red]<filename> not provided.[/]");
                return;
            }

            var withExt = filename.EndsWith(Constants.SquealerFileExtension, StringComparison.OrdinalIgnoreCase)
                ? filename
                : filename + Constants.SquealerFileExtension;

            var filePath = Path.Combine(AppState.Instance.LastOpenedPath, withExt);

            if (!File.Exists(filePath))
            {
                AnsiConsole.MarkupLine($"[red]File not found: {withExt}[/]");
                return;
            }

            OpenFile(filePath, AppState.Instance.Settings.Editor.Path);
        }

        public static void OpenFile(string fullFilePath, string editorPath)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = editorPath,
                    Arguments = $"\"{fullFilePath}\"",
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch
            {
                // Fall back to system default if the configured editor fails.
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = fullFilePath, UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                }
            }
        }
    }
}
