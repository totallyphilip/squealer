using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SquealerConsoleCSharp.CustomCommand
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
                var filePath = Helper.GetFilePath(searchtext);

                XDocument doc = XDocument.Load(filePath); // Load the XML file

                // Query elements using LINQ
                var childNodes = from node in doc.Descendants("Child")
                                 select new
                                 {
                                     Content = node.Value
                                 };

                foreach (var child in childNodes)
                {
                    Console.WriteLine(child.Content);
                }
            }
        }
    }
}
