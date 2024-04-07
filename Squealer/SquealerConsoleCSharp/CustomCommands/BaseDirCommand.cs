using Spectre.Console;
using SquealerConsoleCSharp.Models;
using SquealerConsoleCSharp.MyXml;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    public abstract class BaseDirCommand : ICustomeCommand
    {
        private readonly string _name;
        private readonly string _description;

        protected BaseDirCommand(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public Command CreateCommand()
        {
            var command = new Command(_name, _description);

            var pathArgument = new Argument<string?>(
                name: "searchtext",
                description: "searchtext",
                getDefaultValue: () => null // Optional: Provide a default value or leave it as null if not provided
            );

            command.AddArgument(pathArgument);

            var procOpt =
                Helper.CreateFlagOption("-p", "proc");
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

            command.SetHandler((bool p, bool fn, bool _if, bool tf, bool v, string? searchtext) =>
            {
                HandleCommand(p, fn, _if, tf, v, searchtext);
            }, procOpt, scalarFunctionOpt, inlineTVFOpt, multiStatementTVFOpt, viewOpt, pathArgument);

            return command;
        }

        protected void HandleCommand(bool p, bool fn, bool _if, bool tf, bool v, string? searchtext)
        {
            if (Helper.CheckFolderValid())
            {

                if (searchtext != null && searchtext.StartsWith("-"))
                {
                    AnsiConsole.MarkupLineInterpolated($"[red]{searchtext} is not a valid flag.[/]");
                    return;
                }

                try
                {
                    var filePaths = Helper.SearchSqlrFilesInFolder(searchtext);
                    var xmlToSqlList = filePaths.Select(x => new XmlToSqlConverter(x)).ToList();

                    if (p || fn || _if || tf || v)
                    {
                        xmlToSqlList = xmlToSqlList
                            .Where(x =>
                            {
                                return (x.SquealerObject.Type == EType.StoredProcedure && p) ||
                                        (x.SquealerObject.Type == EType.ScalarFunction && fn) ||
                                        (x.SquealerObject.Type == EType.InlineTableFunction && _if) ||
                                        (x.SquealerObject.Type == EType.MultiStatementTableFunction && tf) ||
                                        (x.SquealerObject.Type == EType.View && v);
                            })
                            .ToList();
                    }

                    Helper.PrintTable(xmlToSqlList);


                    //extra implemenation

                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupInterpolated($"[underline red]{ex.Message}[/]\n");
                    throw;
                }

                // At the point where you want to allow for extension:
                ExtraImplementation(p, fn, _if, tf, v, searchtext);
            }
        }

        // Define an abstract or virtual method for extra implementation.
        // If abstract, derived classes are forced to implement it.
        // If virtual, you can provide a default implementation that can be overridden.
        protected abstract void ExtraImplementation(bool p, bool fn, bool _if, bool tf, bool v, string? searchtext);
    }

}
