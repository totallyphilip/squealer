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

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class GenCommand : BaseDirCommand
    {
        public GenCommand() : base("gen", "Generate SQL Scripts")
        {

        }

        protected override void ExtraImplementation(bool p, bool fn, bool _if, bool tf, bool v, bool alter, bool encryption, string? searchtext)
        {
            if(_xmlToSqls.Count == 0)
            {
                return;
            }

            var config = ConfigObject.GetConfig();

            StringBuilder output = new StringBuilder();
            //if (!test)
            //{
            //    output.AppendLine(MyResources.Resources.TrackFailedItems_Start);
            //    output.Append(MyResources.Resources._TopScript);
            //}

            foreach (var item in _xmlToSqls.Select((xml,index)=> (xml, index)))
            {
                var version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                var countString = $"{item.index+1}/{_xmlToSqls.Count}";

                output.AppendLine(Helper.GetSqlOfOneFile(item.xml, config, countString, alter, false, encryption));
            }

            //if (!test)
            //{
            //    output.AppendLine();
            //    output.Append(MyResources.Resources.TrackFailedItems_End);
            //}

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
