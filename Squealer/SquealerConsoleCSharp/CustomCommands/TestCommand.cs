using Spectre.Console;
using SquealerConsoleCSharp.Models;
using SquealerConsoleCSharp.MyXml;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using TextCopy;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class TestCommand : ICustomCommand
    {
        public Command CreateCommand()
        {
            var command = new Command("test", "generate text sql");

            var pathArgument = new Argument<string?>(
                    name: "searchtext",
                    description: "searchtext",
                    getDefaultValue: () => null // Optional: Provide a default value or leave it as null if not provided
                );
            command.AddArgument(pathArgument);

            command.SetHandler((InvocationContext context) =>
            {
                // Retrieve the values for options and arguments
                string? searchText = context.ParseResult.GetValueForArgument(pathArgument);

                // Call your method to handle the command
                HandleCommand(searchText);
            });

            return command;
        }

        private void HandleCommand(string? searchText)
        {
            if (searchText != null)
                searchText = searchText.Replace("[", "").Replace("]", "");


            var filePaths = Helper.SearchSqlrFilesInFolder(searchText);
            var xmlToSqls = filePaths.Select(x => new XmlToSqlConverter(x)).ToList();

            if(xmlToSqls.Count == 0)
            {
                return;
            }

            if(xmlToSqls.Count > 1)
            {
                AnsiConsole.MarkupLine("[red]More then 1 files selected[/]");
                return;
            }

            var config = ConfigObject.GetConfig();

            StringBuilder output = new StringBuilder();

            output.AppendLine(Helper.GetSqlOfOneFile(xmlToSqls.First(), config, "1/1", false, true, false));

            if (AppState.Instance.Settings.Output.CopyToClipboard)
            {
                AnsiConsole.MarkupLine("\n#Output copied to clipboard.");
                ClipboardService.SetText(output.ToString());
            }

            if (AppState.Instance.Settings.Output.ExportToSqlFile)
            {
                // Get the system temp directory
                string tempBase = Path.GetTempPath();

                // Define a subdirectory specific to your application
                string appTempDir = Path.Combine(tempBase, "SqlrTempFiles");

                Directory.CreateDirectory(appTempDir);

                // Generate a unique filename within the temp folder
                string tempFile = Path.Combine(appTempDir, $"TempFile_{Guid.NewGuid()}.sql");



                File.WriteAllText(tempFile, output.ToString());

                Helper.OpenFileWithDefaultProgram(tempFile);
            }
        }
    }
}
