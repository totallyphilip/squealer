using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommand
{
    public class DirectoryCommand : ICustomeCommand
    {
        public Command CreateCommand()
        {

            var command = new Command("dir", "Directory.")
            {
            };


            command.SetHandler(HandleOpenCommand);

            return command;
        }

        private void HandleOpenCommand()
        {
            if (Helper.CheckFolderValid())
            {
                string[] files = Directory.GetFiles(AppState.Instance.LastOpenedPath);
                foreach (string file in files.OrderBy(x => x))
                {
                    Console.WriteLine(Path.GetFileName(file));
                }
            }  
        }
    }
}
