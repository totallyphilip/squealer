using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    public class DirectoryCommand : BaseDirCommand
    {
        public DirectoryCommand() : base("dir", "xxx")
        {

        }

        protected override void ExtraImplementation(bool p, bool fn, bool _if, bool tf, bool v, string? searchtext)
        {
            return;
        }
    }
}
