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

        protected override void ExtraImplementation(bool p, bool fn, bool _if, bool tf, bool v, bool alt, bool e, string? searchtext)
        {
            return;
        }
    }
}
