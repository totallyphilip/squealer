using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class DirectoryCommand : BaseDirCommand
    {
        public DirectoryCommand() : base("dir", "Directory")
        {

        }

    }
}
