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
        public FixCommand() : base("fix", "Regenerate sqlr files", false)
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
                bool proc = context.ParseResult.GetValueForOption(_procOpt);
                bool scalarFunction = context.ParseResult.GetValueForOption(_scalarFunctionOpt);
                bool inlineTVF = context.ParseResult.GetValueForOption(_inlineTVFOpt);
                bool multiStatementTVF = context.ParseResult.GetValueForOption(_multiStatementTVFOpt);
                bool view = context.ParseResult.GetValueForOption(_viewOpt);
                bool unCommitted = context.ParseResult.GetValueForOption(_unCommittedOpt);
                string? diff = context.ParseResult.GetValueForOption(_diffOpt);
                string? searchText = context.ParseResult.GetValueForArgument(_pathArgument);
                string? convertOpt = context.ParseResult.GetValueForOption(convertaOpt);

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
