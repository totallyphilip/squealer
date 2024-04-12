using Spectre.Console;
using SquealerConsoleCSharp.Attributes;
using SquealerConsoleCSharp.Extensions;
using SquealerConsoleCSharp.Models;
using SquealerConsoleCSharp.Models.Git;
using SquealerConsoleCSharp.MyXml;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp
{
    internal class Helper
    {

        public static bool VadilateFolder()
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
            if (VadilateFolder())
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


        public static void PrintTable(List<XmlToSqlConverter> xmlToSqlList, List<GitFileInfo> gitFileInfos) 
        {
            var gitFileInfosDict = gitFileInfos.ToDictionary(x => x.FileName, x => x.Status);
            var table = new Spectre.Console.Table();
            table.AddColumn("Type");
            table.AddColumn("Name");
            table.AddColumn("Git Status");
            foreach (var x in xmlToSqlList)
            {
                var attr = x.SquealerObject.Type.GetObjectTypeAttribute();
                
                var hasGitInfo = gitFileInfosDict.TryGetValue(x.SqlrFileInfo.FileName, out var gitStatus);

                table.AddRow(attr.Acronym, $"{x.SqlrFileInfo.SqlObjectName}[green]{ attr.NumericSymbol}[/]", hasGitInfo ? $"{gitStatus}" : "");
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

        public static bool IsValidFilename(string filename)
        {
            // Check for invalid characters.
            if (filename.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            {
                return false;
            }

            // Optional: Add check for reserved names here if targeting Windows.

            // Check for length. This is a simple example; you may need to adjust based on your requirements.
            if (filename.Length > 255) // Using 255 as a generic safe limit.
            {
                return false;
            }

            // Add any additional checks you require here.

            return true; // Passed all checks.
        }

        public static void SaveAndOpenFileWithDefaultProgram(string filename, string content)
        {
            var filePath = Path.Combine(AppState.Instance.LastOpenedPath, filename);
            File.WriteAllText(filePath, content);
            Process.Start("explorer.exe", filePath);
        }
        public class GitHelper
        {
            public static string GetGitProejctBranchName()
            {
                return string.IsNullOrEmpty(AppState.Instance.LastOpenedPath) ?
                                "[red](git?)[/]" :
                                $"[green][[{AppState.Instance.GitPprojectName}]][/] [blue]({GetGitBranch()})[/]";
            }

            public static string GetGitProject()
            {
                var res = ExecuteGitCommand("rev-parse --show-toplevel");
                return res.Count != 0 ? Path.GetFileName(res[0]) : string.Empty;
            }

            public static string GetGitBranch()
            {
                var res = ExecuteGitCommand("rev-parse --abbrev-ref HEAD");
                return res.Count != 0 ? Path.GetFileName(res[0]) : string.Empty;
            }

            public static List<GitFileInfo> GetUnTrackedFiles()
            {
                var lines = ExecuteGitCommand("status --porcelain");
                var res = lines
                    .Select(line =>
                    {
                        var x = line;
                        var parts = line.Split(' ');
                        var status = parts[0];
                        var filename = parts[1];
                        return new GitFileInfo
                        {
                            FileName = Path.GetFileName(filename),
                            Status = status
                        };
                    })
                    .ToList();
                return res;
            }

            public static List<GitFileInfo> GetDiffFiles(string targetBranch)
            {
                var command = $"diff --name-status {targetBranch} -- \"{AppState.Instance.LastOpenedPath}\"";
                var lines = ExecuteGitCommand(command);
                var res = lines
                    .Select(line => {
                        // Split the line by tabs (\t)
                        var parts = line.Split('\t');

                        // Check the number of parts to determine if it's a rename or an add/modify
                        if (parts.Length == 3) // Renamed file
                        {
                            var status = parts[0];
                            var oldFilename = parts[1];
                            var newFilename = parts[2];
                            return new GitFileInfo
                            {
                                FileName = Path.GetFileName(newFilename),
                                Status = $"{status} {Path.GetFileName(oldFilename)}"
                            };
                        }
                        else // Added or Modified file
                        {
                            var status = parts[0];
                            var filename = parts[1];
                            return new GitFileInfo
                            {
                                FileName = Path.GetFileName(filename),
                                Status = status
                            };
                        }
                    }).ToList();

                 return res;
            }


            public static bool IsBranchExists(string branch)
            {
                return ExecuteGitCommand($"branch --list {branch}").Count != 0;
            }


            private static List<string> ExecuteGitCommand(string command)
            {
                
                List<string> results = new List<string>();
                ProcessStartInfo startInfo = new ProcessStartInfo("git", command)
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

                    string line;
                    while ((line = process.StandardOutput.ReadLine()) != null)
                    {
                        results.Add(line);
                    }

                    process.WaitForExit();
                }

                return results;
            }



        }

        
    }
}
