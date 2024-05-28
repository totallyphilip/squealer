using Spectre.Console;
using SquealerConsoleCSharp.Models;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TextCopy;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class FixCommand: BaseDirCommand
    {
        public FixCommand() : base("fix", "Regenerate sqlr files")
        {

        }

        public override Command CreateCommand()
        {
            var command = base.CreateCommand();

            // Add additional options specific to the derived class
            var convertaOpt = new Option<string?>(
                aliases: new[] { "-mode" },
                description: "",
                getDefaultValue: () => string.Empty
                );

            command.AddOption(convertaOpt);

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
                string? convertOpt = context.ParseResult.GetValueForOption(convertaOpt);
                string? searchText = context.ParseResult.GetValueForArgument(new Argument<string?>("searchtext", getDefaultValue: () => null));

                // Call the base handle method
                BasicHandling(proc, scalarFunction, inlineTVF, multiStatementTVF, view, unCommitted, diff, searchText);

                // Handle additional logic
                ExtraImplementation(convertOpt);
            });

            return command;
        }

        private void ExtraImplementation(string? convertOpt)
        {

            foreach(var file in _xmlToSqls) 
            {
                var filePath = file.SqlrFileInfo.FilePath;
                file.SquealerObject.ExportXmlFile(filePath);
            }


        }
    }
}
