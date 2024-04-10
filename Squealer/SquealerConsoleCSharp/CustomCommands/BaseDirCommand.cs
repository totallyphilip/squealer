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

        private List<XmlToSqlConverter> _xmlToSqls;

        protected BaseDirCommand(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public Command CreateCommand()
        {
            var command = new Command(_name, $"{_description} \n Usage: {_name} [[-p] [-fn] [-_if] [-tf] [-v]] [-u]] && [-diff <branch-name>]] && [searchText]");

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
            var unCommittedOpt =
                Helper.CreateFlagOption("-u", "uncommited files");
            var diffOpt = new Option<string?>(
                aliases: new[] { "-diff" },
                description: "-diff <Target Branch Name>",
                getDefaultValue: () => string.Empty
                );



            command.AddOption(procOpt);
            command.AddOption(scalarFunctionOpt);
            command.AddOption(inlineTVFOpt);
            command.AddOption(multiStatementTVFOpt);
            command.AddOption(viewOpt);
            command.AddOption(unCommittedOpt);
            command.AddOption(diffOpt);

            var pathArgument = new Argument<string?>(
                    name: "searchtext",
                    description: "searchtext",
                    getDefaultValue: () => null // Optional: Provide a default value or leave it as null if not provided
                );

            command.AddArgument(pathArgument);

            command.SetHandler(HandleCommand, 
                procOpt, 
                scalarFunctionOpt, 
                inlineTVFOpt, 
                multiStatementTVFOpt, 
                viewOpt, 
                unCommittedOpt,
                diffOpt,
                pathArgument);

            return command;
        }

        protected void HandleCommand(bool p, bool fn, bool _if, bool tf, bool v, bool u, string? diff_targetBranch, string? searchtext)
        {
            if (!Helper.VadilateFolder())
                return;

            if (searchtext != null && searchtext.StartsWith("-"))
            {
                AnsiConsole.MarkupLineInterpolated($"[red]{searchtext} is not a valid flag.[/]");
                return;
            }

            try
            {
                var filePaths = Helper.SearchSqlrFilesInFolder(searchtext);
                _xmlToSqls = filePaths.Select(x => new XmlToSqlConverter(x)).ToList();

                if (p || fn || _if || tf || v)
                {
                    _xmlToSqls = _xmlToSqls
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

                if (u)
                {
                    var uncommmitedFileNames = Helper.GitHelper.GetGitUnCommittedFiles().ToHashSet();

                    _xmlToSqls = _xmlToSqls
                        .Where(x => uncommmitedFileNames.Contains(x.SqlrFileInfo.FileName))
                        .ToList();

                }

                if (!string.IsNullOrWhiteSpace(diff_targetBranch))
                {
                    if (!Helper.GitHelper.IsBranchExists(diff_targetBranch))
                    {
                        AnsiConsole.MarkupLine($"[red]Invalid banchName {diff_targetBranch}[/]");
                        return;
                    }
                    var diffFiles = Helper.GitHelper.GetDiffFiles(diff_targetBranch).ToHashSet();

                    _xmlToSqls = _xmlToSqls
                        .Where(x => diffFiles.Contains(x.SqlrFileInfo.FileName))
                        .ToList();
                }

                Helper.PrintTable(_xmlToSqls);


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

        // Define an abstract or virtual method for extra implementation.
        // If abstract, derived classes are forced to implement it.
        // If virtual, you can provide a default implementation that can be overridden.
        protected abstract void ExtraImplementation(bool p, bool fn, bool _if, bool tf, bool v, string? searchtext);
    }

}
