using SquealerConsoleCSharp.CustomCommands;
using System.CommandLine;

namespace SquealerConsoleCSharp
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {

            var rootCommand = new RootCommand("Interactive Shell Example");


            rootCommand.AddCommand(CommandBuilder.Build<OpenCommand>().CreateCommand());
            rootCommand.AddCommand(CommandBuilder.Build<DirectoryCommand>().CreateCommand());
            rootCommand.AddCommand(CommandBuilder.Build<GenCommand>().CreateCommand());


            while (true)
            {
                string directoryCharacter = string.IsNullOrWhiteSpace(AppState.Instance.LastOpenedPath) ? "?" : ">";

                Console.Write($"{directoryCharacter} ");
                string input = Console.ReadLine();
                if (input.ToLower() == "exit") break;

                string[] inputArgs;

                if (input.StartsWith("open", StringComparison.OrdinalIgnoreCase))
                {
                    var indexOfFirstSpace = input.IndexOf(' ');
                    inputArgs = new string[]{ input.Substring(0, indexOfFirstSpace), input.Substring(indexOfFirstSpace)};
                }
                else 
                { 
                    inputArgs = input.Split(' ');
                }
                // Invoke the command system with the input arguments
                await rootCommand.InvokeAsync(inputArgs);
            }

            return 0;
        }

    }
}
