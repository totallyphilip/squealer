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

namespace SquealerConsoleCSharp.CustomCommands
{
    public class GenCommand : ICustomeCommand
    {
        public Command CreateCommand()
        {
            var pathArgument = new Argument<string?>(
                name: "searchtext",
                description: "searchtext",
                getDefaultValue: () => null // Optional: Provide a default value or leave it as null if not provided
            );


            var procOpt = new Option<bool>(
                aliases: new[] { "-p" },
                description: "proc",
                getDefaultValue: () => false // Optional: Provide a default value or leave it as null if not provided
            );

            var scalarFunctionOpt = new Option<bool>(
                aliases: new[] { "-fn" },
                description: "scalar function",
                getDefaultValue: () => false // Optional: Provide a default value or leave it as null if not provided
            );

            var inlineTVFOpt = new Option<bool>(
                aliases: new[] { "-if" },
                description: "inline table-valued function",
                getDefaultValue: () => false // Optional: Provide a default value or leave it as null if not provided
            );

            var multiStatementTVFOpt = new Option<bool>(
                aliases: new[] { "-tf" },
                description: "inline table-valued function",
                getDefaultValue: () => false // Optional: Provide a default value or leave it as null if not provided
            );

            var command = new Command("gen", "generate -p -fn -if -tf -v -err -x -cs -code -u -diff -m:alt|t|e [<wildcard>|#] [/<searchtext>]")
            {
                procOpt,
                scalarFunctionOpt,
                inlineTVFOpt,
                multiStatementTVFOpt,
                pathArgument,
            };


            command.SetHandler(HandleCommand, 
                procOpt,
                scalarFunctionOpt,
                inlineTVFOpt,
                multiStatementTVFOpt,
                pathArgument);

            return command;
        }

        private void HandleCommand(bool p, bool fn, bool _if, bool tf, string? searchtext)
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
                    var xmlToSqlList = filePaths.Select(x=> new XmlToSqlConverter(x)).ToList();

                    if(p) xmlToSqlList = xmlToSqlList
                            .Where(x=>
                            {
                                return (x.SquealerObject.Type == EType.StoredProcedure && p) ||
                                        (x.SquealerObject.Type == EType.ScalarFunction && fn) ||
                                        (x.SquealerObject.Type == EType.InlineTableFunction && _if) ||
                                        (x.SquealerObject.Type == EType.MultiStatementTableFunction && tf);
                            })
                            .ToList();


                    Helper.PrintTable(xmlToSqlList);

                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupInterpolated($"[underline red]{ex.Message}[/]\n");
                    throw;
                }




                //var xmlToSql = new XmlToSql(searchtext);


                //Console.WriteLine(xmlToSql.GetSqlScript());
            }
        }
    }
}
