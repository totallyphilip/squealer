﻿using Spectre.Console;
using SquealerConsoleCSharp.CustomCommands;
using SquealerConsoleCSharp.Models;
using System.CommandLine;
using System.Resources;

namespace SquealerConsoleCSharp
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            AnsiConsole.Write(
                new FigletText("Squealer")
                    .LeftJustified()
                    .Color(Color.Blue));

            

            var rootCommand = new RootCommand("Interactive Shell Example");

            rootCommand.AddCommand(new OpenCommand().CreateCommand());
            rootCommand.AddCommand(new DirectoryCommand().CreateCommand());
            rootCommand.AddCommand(new GenCommand().CreateCommand());
            rootCommand.AddCommand(new NewCommand().CreateCommand());
            rootCommand.AddCommand(new ClearCommand().CreateCommand());


            while (true)
            {
                string directoryCharacter = Helper.GitHelper.GetGitProejctBranchName() + " >";
                string[] inputArgs;


                AnsiConsole.Markup($"{directoryCharacter}");
                string? input = Console.ReadLine();
                AnsiConsole.WriteLine();
                if ( string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if(input.Length > 1000)
                {
                    AnsiConsole.MarkupLine("[red]Cannot exceed 1000 chars limit.[/]");
                    continue;
                }

                if (input.StartsWith("open", StringComparison.OrdinalIgnoreCase))
                {
                    var indexOfFirstSpace = input.IndexOf(' ');
                    if (indexOfFirstSpace != -1)
                    {
                        inputArgs = new string[] { input.Substring(0, indexOfFirstSpace), input.Substring(indexOfFirstSpace) };
                    }
                    else
                    {
                        inputArgs = input.Split(' ');
                    }
                }
                else 
                { 
                    inputArgs = input.Split(' ');
                }
                // Invoke the command system with the input arguments
                await rootCommand.InvokeAsync(inputArgs);
                AnsiConsole.WriteLine();
            }
        }

    }
}
