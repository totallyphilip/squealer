using SquealerConsoleCSharp.Models.Git;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp
{
    internal class GitHelper
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
            var lines = ExecuteGitCommand($"status --porcelain -- \"{AppState.Instance.LastOpenedPath}\"");
            var res = lines
                .Select(line =>
                {
                    var parts = line.Trim().Split(' ');
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
