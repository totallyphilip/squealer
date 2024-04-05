using SquealerConsoleCSharp.CustomCommands;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SquealerConsoleCSharp.MyXml;

namespace SquealerConsoleCSharp.CustomCommands
{
    public class GenCommand : ICustomeCommand
    {
        public Command CreateCommand()
        {
            var pathArgument = new Argument<string?>(
                name: "searchtext",
                description: "searchtext",
                getDefaultValue: () => null // Optional: Provide a default value or leave it as null if not provided
            );

            var command = new Command("gen", "generate -p -fn -if -tf -v -err -x -cs -code -u -diff -m:alt|t|e [<wildcard>|#] [/<searchtext>]")
            {
                pathArgument
            };


            command.SetHandler(HandleCommand, pathArgument);

            return command;
        }

        private void HandleCommand(string? searchtext)
        {
            if (Helper.CheckFolderValid() && !string.IsNullOrWhiteSpace(searchtext))
            {

                var xmlToSql = new XmlToSql(searchtext);


                Console.WriteLine(xmlToSql.GetSqlScript());
            }
        }
    }
}
