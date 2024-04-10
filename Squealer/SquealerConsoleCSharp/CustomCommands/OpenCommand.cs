using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class OpenCommand: ICustomeCommand
    {
        public Command CreateCommand()
        {
            var pathArgument = new Argument<string?>(
                name: "path",
                description: "Absolute file path.",
                getDefaultValue: () => null // Optional: Provide a default value or leave it as null if not provided
            );

            var command = new Command("open", "Open folder [<path>]. This folder path will be saved for quick access.")
            {
                pathArgument
            };


            command.SetHandler(HandleOpenCommand, pathArgument);

            return command;
        }

        private void HandleOpenCommand(string? path)
        {
            if (path != null) 
            {
                path = path.Trim();
                if (Directory.Exists(path))
                {
                    AppState.Instance.LastOpenedPath = path;
                    Console.WriteLine($"Currect path: {AppState.Instance.LastOpenedPath}");
                    var gitProjectName = Helper.GitHelper.GetGitProject();
                    if(!string.IsNullOrEmpty( gitProjectName )) 
                    { 
                        AppState.Instance.GitPprojectName = gitProjectName;
                    }

                }
                else
                {
                    Console.WriteLine("Invalid Path");
                }
            }
            
        }
    }
}
