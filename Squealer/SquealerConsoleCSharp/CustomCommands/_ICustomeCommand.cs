using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommands
{
    public interface ICustomeCommand
    {

        public Command CreateCommand();

    }


    public static class CommandBuilder
    {
        public static T Build<T>() where T : ICustomeCommand, new()
        {
            return new T();
        }
    }
}
