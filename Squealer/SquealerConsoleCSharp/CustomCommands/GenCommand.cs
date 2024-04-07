using SquealerConsoleCSharp.CustomCommands;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SquealerConsoleCSharp.MyXml;
using SquealerConsoleCSharp.Models;
using Spectre.Console;

namespace SquealerConsoleCSharp.CustomCommands
{
    public class GenCommand : BaseDirCommand
    {
        public GenCommand() : base("gen", "xxxxx")
        {

        }

        protected override void ExtraImplementation(bool p, bool fn, bool _if, bool tf, bool v, string? searchtext)
        {
            AnsiConsole.MarkupLine("[red] nothing to gen yet!![/]");
        }
    }
}
