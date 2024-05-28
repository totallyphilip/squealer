using SquealerConsoleCSharp.CustomCommands;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SquealerConsoleCSharp.MyXml;
using SquealerConsoleCSharp.Models;
using Spectre.Console;
using SquealerConsoleCSharp.Extensions;
using System.Reflection;
using System.Windows.Forms;
using TextCopy;
using System.CommandLine.Invocation;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class GenCommand : BaseDirCommand
    {
        public GenCommand() : base("gen", "Generate SQL Scripts")
        {

        }

        public override Command CreateCommand()
        {
            var command = base.CreateCommand();

            // Add additional options specific to the derived class
            var modeOpt = new Option<string?>(
                aliases: new[] { "-mode" },
                description: "-mode alt|e|t; only useful when there is text output.\n" +
                            "alt - alter\n" +
                            "e - encrption\n",
                getDefaultValue: () => string.Empty
                );

            command.AddOption(modeOpt);

            // Update the handler to handle the additional options
            command.SetHandler((InvocationContext context) =>
            {
                // Retrieve the values for base options
                bool proc = context.ParseResult.GetValueForOption(Helper.CreateFlagOption("-p", "proc"));
                bool scalarFunction = context.ParseResult.GetValueForOption(Helper.CreateFlagOption("-fn", "scalar function"));
                bool inlineTVF = context.ParseResult.GetValueForOption(Helper.CreateFlagOption("-if", "inline table-valued function"));
                bool multiStatementTVF = context.ParseResult.GetValueForOption(Helper.CreateFlagOption("-tf", "multi-statement table-valued function"));
                bool view = context.ParseResult.GetValueForOption(Helper.CreateFlagOption("-v", "view"));
                bool unCommitted = context.ParseResult.GetValueForOption(Helper.CreateFlagOption("-u", "uncommited files"));
                string? diff = context.ParseResult.GetValueForOption(new Option<string?>("-diff", getDefaultValue: () => string.Empty));
                string? modes = context.ParseResult.GetValueForOption(new Option<string?>("-mode", getDefaultValue: () => string.Empty));
                string? searchText = context.ParseResult.GetValueForArgument(new Argument<string?>("searchtext", getDefaultValue: () => null));

                // Call the base handle method
                BasicHandling(proc, scalarFunction, inlineTVF, multiStatementTVF, view, unCommitted, diff, searchText);

                // Handle additional logic
                ExtraImplementation(proc, scalarFunction, inlineTVF, multiStatementTVF, view, modes, searchText);
            });

            return command;
        }

        private void ExtraImplementation(string? modes)
        {

            bool alter = false, encrypt = false;

            if (!string.IsNullOrWhiteSpace(modes))
            {
                var modeSet = modes.Split('|').Select(m => m.Trim().ToLower()).Distinct().ToHashSet();
                if (modeSet.Contains("alt"))
                    alter = true;
                if (modeSet.Contains("e"))
                    encrypt = true;
                if (!modeSet.Any(m => new[] { "alt", "e",  }.Contains(m)) && modeSet.Count > 0)
                {
                    Console.WriteLine("Invalid mode(s) specified.");
                    return;
                }
            }

            if (_xmlToSqls.Count == 0)
            {
                return;
            }

            var config = ConfigObject.GetConfig();

            StringBuilder output = new StringBuilder();

            output.AppendLine(MyResources.Resources.TrackFailedItems_Start);
            output.Append(MyResources.Resources._TopScript);
            

            foreach (var item in _xmlToSqls.Select((xml,index)=> (xml, index)))
            {
                var version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                var countString = $"{item.index+1}/{_xmlToSqls.Count}";

                output.AppendLine(Helper.GetSqlOfOneFile(item.xml, config, countString, alter, false, encrypt));
            }

            output.AppendLine();
            output.Append(MyResources.Resources.TrackFailedItems_End);


            AnsiConsole.WriteLine("# Output copied to Windows clipboard.");

            if (AppState.Instance.Settings.Output.CopyToClipboard)
            {
                AnsiConsole.MarkupLine("\n#Output copied to clipboard.");
                ClipboardService.SetText(output.ToString());
            }

            if(AppState.Instance.Settings.Output.ExportToSqlFile) 
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
