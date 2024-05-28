using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal interface ICustomCommand
    {
        public Command CreateCommand();

    }


    internal static class CommandBuilder
    {
        public static T Build<T>() where T : ICustomCommand, new()
        {
            return new T();
        }
    }
}
