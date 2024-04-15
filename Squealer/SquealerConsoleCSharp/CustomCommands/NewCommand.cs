using Spectre.Console;
using SquealerConsoleCSharp.Models;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class NewCommand : ICustomeCommand
    {
        public Command CreateCommand()
        {
            var command = new Command("new", "Create new file.");

            var procOpt =
            Helper.CreateFlagOption("-p", "proc (Default)");
            var scalarFunctionOpt =
                Helper.CreateFlagOption("-fn", "scalar function");
            var inlineTVFOpt =
                Helper.CreateFlagOption("-if", "inline table-valued function");
            var multiStatementTVFOpt =
                Helper.CreateFlagOption("-tf", "multi-statement table-valued function");
            var viewOpt =
                Helper.CreateFlagOption("-v", "view");

            command.AddOption(procOpt);
            command.AddOption(scalarFunctionOpt);
            command.AddOption(inlineTVFOpt);
            command.AddOption(multiStatementTVFOpt);
            command.AddOption(viewOpt);

            var fileNameArgument = new Argument<string>(
                name: "filename",
                description: "filename",
                getDefaultValue: () => string.Empty // Optional: Provide a default value or leave it as null if not provided
            );

            command.Add(fileNameArgument);

            command.SetHandler(HandleOpenCommand,
                            procOpt,
                            scalarFunctionOpt,
                            inlineTVFOpt,
                            multiStatementTVFOpt,
                            viewOpt,
                            fileNameArgument);


            return command;
        }

        private void HandleOpenCommand(bool p, bool fn, bool _if, bool tf, bool v, string filename)
        {
            if (!Helper.VadilateFolder())
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                AnsiConsole.MarkupLine("[red]<filename> not provided.");
                return;
            }
            else if(filename.Split('.').Length == 1)
            {
                filename = "dbo." + filename.Trim();
            }

            var filename_w_ext = $"{filename}.sqlr";

            if(!SqlrFileInfo.ValidateSqlrFileName(filename_w_ext))
            {
                return;
            }

            string filePath = Helper.GetFilePath(filename_w_ext);
            if (File.Exists(filePath))
            {
                AnsiConsole.MarkupLine("[red]File already exists.[/]\n");
                return;
            }


            EType type;
            if (fn)
                type = EType.ScalarFunction;
            else if (_if)
                type = EType.InlineTableFunction;
            else if (tf)
                type = EType.MultiStatementTableFunction;
            else if (v)
                type = EType.View;
            else
                type = EType.StoredProcedure; // default

            var mySquealer = SquealerObject.GetNewObject(type);

            mySquealer.ExportXmlFile(filePath);

            AnsiConsole.MarkupLine($"file [yellow]{filename_w_ext}[/] Created.");
        }
    }
}
