using Spectre.Console;
using SquealerConsoleCSharp.Models;
using SquealerConsoleCSharp.Models.Git;
using SquealerConsoleCSharp.MyXml;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal abstract class BaseDirCommand : ICustomeCommand
    {
        private readonly string _name;
        private readonly string _description;

        protected List<XmlToSqlConverter> _xmlToSqls = [];
        protected List<GitFileInfo> _gitFileInfos = [];

        protected BaseDirCommand(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public Command CreateCommand()
        {
            var command = new Command(_name, $"{_description} \n Note on Syntax:\r\n- [ ] indicates an optional group of options or arguments. " +
                $"Anything inside these brackets is not required for the command to run but specifies additional filters or parameters when included.\r\n" +
                $"- | (pipe) signifies an \"or\" relationship between options within a group. You can choose one or more options to apply.\r\n" +
                $"- && means \"and\" and is used to indicate that options or groups of options can be used in combination with each other.\r\n" +
                $"- {_name} [ -p | -fn | -if | -tf | -v ] && [ -u | -diff <branch-name> ] && [searchText]\r\n");

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
            var modeOpt = new Option<string?>(
                aliases: new[] { "-mode" },
                description: "-mode alt|e|t; only useful when there is text output.\n" +
                            "alt - alter\n" +
                            "e - encrption\n" +
                            "t - test: one object only",
                getDefaultValue: () => string.Empty
                );

            command.AddOption(procOpt);
            command.AddOption(scalarFunctionOpt);
            command.AddOption(inlineTVFOpt);
            command.AddOption(multiStatementTVFOpt);
            command.AddOption(viewOpt);
            command.AddOption(unCommittedOpt);
            command.AddOption(diffOpt);
            command.AddOption(modeOpt);

            var pathArgument = new Argument<string?>(
                    name: "searchtext",
                    description: "searchtext",
                    getDefaultValue: () => null // Optional: Provide a default value or leave it as null if not provided
                );

            command.AddArgument(pathArgument);

            command.SetHandler((InvocationContext context) =>
            {
                // Retrieve the values for options and arguments
                bool proc = context.ParseResult.GetValueForOption(procOpt);
                bool scalarFunction = context.ParseResult.GetValueForOption(scalarFunctionOpt);
                bool inlineTVF = context.ParseResult.GetValueForOption(inlineTVFOpt);
                bool multiStatementTVF = context.ParseResult.GetValueForOption(multiStatementTVFOpt);
                bool view = context.ParseResult.GetValueForOption(viewOpt);
                bool unCommitted = context.ParseResult.GetValueForOption(unCommittedOpt);
                string? diff = context.ParseResult.GetValueForOption(diffOpt);
                string? modes = context.ParseResult.GetValueForOption(modeOpt);
                string? searchText = context.ParseResult.GetValueForArgument(pathArgument);

                // Call your method to handle the command
                HandleCommand(proc, scalarFunction, inlineTVF, multiStatementTVF, view, unCommitted, diff, modes, searchText);
            });

            return command;
        }

        protected void HandleCommand(bool p, bool fn, bool _if, bool tf, bool v, bool u, string? diff_targetBranch, string? modes, string? searchtext)
        {
            if (!Helper.VadilateFolder())
                return;

            if (searchtext != null)
                searchtext = searchtext.Replace("[", "").Replace("]", "");

            bool alt = false, e = false, t = false;

            if(!string.IsNullOrWhiteSpace(modes))
            {
                var modeSet = modes.Split('|').Select(m => m.Trim().ToLower()).Distinct().ToHashSet();
                if (modeSet.Contains("alt"))
                    alt = true;
                if (modeSet.Contains("e"))
                    e = true;
                if (modeSet.Contains("t"))
                    t = true;
                if (!modeSet.Any(m => new[] { "alt", "e", "t" }.Contains(m)) && modeSet.Count > 0)
                {
                    Console.WriteLine("Invalid mode(s) specified.");
                    return;
                }
            }


            if (!string.IsNullOrWhiteSpace(searchtext) && searchtext.StartsWith("-"))
            {
                AnsiConsole.MarkupLineInterpolated($"[red]{searchtext} is not a valid flag.[/]");
                return;
            }

            if (!string.IsNullOrWhiteSpace(diff_targetBranch))
            {
                if (!Helper.GitHelper.IsBranchExists(diff_targetBranch))
                {
                    AnsiConsole.MarkupLine($"[red]Invalid banchName {diff_targetBranch}[/]");
                    return;
                }
            }

            try
            {
                var filePaths = Helper.SearchSqlrFilesInFolder(searchtext);
                _xmlToSqls = filePaths.Select(x => new XmlToSqlConverter(x)).ToList();
                _gitFileInfos = Helper.GitHelper.GetUnTrackedFiles();

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

                if (u || !string.IsNullOrWhiteSpace(diff_targetBranch))
                {
                    // only change _gitFileInfos in -diff option
                    if (!string.IsNullOrWhiteSpace(diff_targetBranch))
                    {
                        var diffFiles = Helper.GitHelper.GetDiffFiles(diff_targetBranch);
                        if (u)
                        {

                            _gitFileInfos.AddRange(diffFiles);
                            _gitFileInfos = _gitFileInfos.DistinctBy(x => x.FileName).ToList();
                        }
                        else
                        {
                            _gitFileInfos = diffFiles;
                        }
                    }

                    var fileNameSet = _gitFileInfos.Select(x => x.FileName).ToHashSet();

                    _xmlToSqls = _xmlToSqls
                        .Where(x => fileNameSet.Contains(x.SqlrFileInfo.FileName))
                        .ToList();
                }

                // re order 
                _xmlToSqls = _xmlToSqls.OrderBy(x => x.SqlrFileInfo.SqlObjectName).ToList();

                if (t && _xmlToSqls.Count != 1)
                {
                    AnsiConsole.MarkupLine($"[red]More than 1 object Selected.[/]");
                    return;
                }


                if (!string.IsNullOrWhiteSpace(diff_targetBranch))
                {
                    if (!Helper.GitHelper.IsBranchExists(diff_targetBranch))
                    {
                        AnsiConsole.MarkupLine($"[red]Invalid banchName {diff_targetBranch}[/]");
                        return;
                    }
                    var gitFilesSet = _gitFileInfos.Select(x=>x.FileName).ToHashSet() ;
                    _xmlToSqls = _xmlToSqls.Where(x=>gitFilesSet.Contains(x.SqlrFileInfo.FileName)).ToList();

                }

                Helper.PrintTable(_xmlToSqls, _gitFileInfos);


                //extra implemenation

            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupInterpolated($"[underline red]{ex.Message}[/]\n");
                throw;
            }

            // At the point where you want to allow for extension:
            ExtraImplementation(p, fn, _if, tf, v, alt, t, e, searchtext);
            
        }

        // Define an abstract or virtual method for extra implementation.
        // If abstract, derived classes are forced to implement it.
        // If virtual, you can provide a default implementation that can be overridden.
        protected abstract void ExtraImplementation(bool p, bool fn, bool _if, bool tf, bool v, bool alt, bool t, bool e, string? searchtext);
    }

}
