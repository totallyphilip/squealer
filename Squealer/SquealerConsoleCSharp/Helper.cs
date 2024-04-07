using Spectre.Console;
using SquealerConsoleCSharp.Extensions;
using SquealerConsoleCSharp.Models;
using SquealerConsoleCSharp.MyXml;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp
{
    public class Helper
    {

        public static bool CheckFolderValid()
        {
            if (string.IsNullOrWhiteSpace(AppState.Instance.LastOpenedPath))
            {
                Console.WriteLine("use 'open' to Open a folder first.");
                return false;
            }
            else if (!Directory.Exists(AppState.Instance.LastOpenedPath))
            {
                Console.WriteLine("Invalid Directory");
                return false;
            }
            return true;
                
        }

        public static string GetFilePath(string fileName)
        {
            if (CheckFolderValid())
            {
                var filePath = Path.Combine(AppState.Instance.LastOpenedPath, fileName);
                return filePath;
            }
            else
            {
                return string.Empty;
            }
        }

        public static T ParseDescriptionToEnum<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException($"Not found: {description}", nameof(description));
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static List<string> SearchSqlrFilesInFolder(string? patterns)
        {
            if (string.IsNullOrEmpty(patterns))
            {
                patterns = "*";
            }
            List<string> searchPatterns = patterns.Split('|').ToList();


            searchPatterns = searchPatterns.Select(x => x + ".sqlr").ToList();
            

            Console.WriteLine($"# finding all files matching {string.Join('|', searchPatterns)}");

            // Get files matching any of the patterns and remove duplicates
            var filePaths = searchPatterns.SelectMany(pattern =>
                             Directory.EnumerateFiles(AppState.Instance.LastOpenedPath, pattern))
                             .Distinct()
                             .ToList();

            return filePaths.ToList();
        }


        public static void PrintTable(List<XmlToSqlConverter> xmlToSqlList)
        {

            var table = new Table();
            table.AddColumn("Type");
            table.AddColumn("Name");
            foreach (var x in xmlToSqlList)
            {
                var attr = x.SquealerObject.Type.GetObjectTypeAttribute();
                table.AddRow(attr.ShortName, $"{x.SqlrFileInfo.SqlObjectName}[green]{ attr.NumericSymbol}[/]");
            }
            AnsiConsole.Write(table);

            AnsiConsole.Write($"\n# {xmlToSqlList.Count} files.\n");
        }


        public static Option<bool> CreateFlagOption(string alias, string description)
        {
            return new Option<bool>(
                aliases: new[] { alias },
                description: description,
                getDefaultValue: () => false
                );
        }

        public class GitHelper
        {
            public static string GetGitProejctBranchName()
            {
                return string.IsNullOrEmpty(AppState.Instance.LastOpenedPath) ?
                                "[red](git?)[/]" :
                                $"[green][[{GetGitProject()}]] ({GetGitBranch()})[/]";
            }

            public static string GetGitProject()
            {
                var res = ExecuteGitCommand(EGitCommand.GetProject);
                return res.Count != 0 ? Path.GetFileName(res[0]) : string.Empty;
            }

            public static string GetGitBranch()
            {
                var res = ExecuteGitCommand(EGitCommand.GetBranch);
                return res.Count != 0 ? Path.GetFileName(res[0]) : string.Empty;
            }

            public static string GetGitUnCommittedFiles()
            {
                var res = ExecuteGitCommand(EGitCommand.GetUnCommitedFiles);
                return res.Count != 0 ? Path.GetFileName(res[0]) : string.Empty;
            }

            private static List<string> ExecuteGitCommand(EGitCommand gitCommand)
            {
                if (string.IsNullOrWhiteSpace(AppState.Instance.LastOpenedPath))
                    return new List<string>();

                var attribute = gitCommand.GetGitCommandAttribute();
                if (attribute == null)
                {
                    throw new InvalidOperationException("The specified command does not have an associated Git command string.");
                }

                List<string> results = new List<string>();
                ProcessStartInfo startInfo = new ProcessStartInfo("git", attribute.Command)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = AppState.Instance.LastOpenedPath
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();

                    if (attribute.MultiLineOutput)
                    {
                        string line;
                        while ((line = process.StandardOutput.ReadLine()) != null)
                        {
                            results.Add(line);
                        }
                    }
                    else
                    {
                        string result = process.StandardOutput.ReadLine();
                        if (!string.IsNullOrEmpty(result))
                        {
                            results.Add(result);
                        }
                    }

                    process.WaitForExit();
                }

                return results;
            }
        }

        
    }
}
