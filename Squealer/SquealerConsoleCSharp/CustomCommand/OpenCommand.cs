﻿using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.CustomCommand
{
    public class OpenCommand: ICustomeCommand
    {
        public Command CreateCommand()
        {
            var pathArgument = new Argument<string?>(
                name: "path",
                description: "Absolute file path.",
                getDefaultValue: () => null // Optional: Provide a default value or leave it as null if not provided
            );

            var command = new Command("open", "Open folder [<path>]. This folder path will be saved for quick access.")
            {
                pathArgument
            };


            command.SetHandler(HandleOpenCommand, pathArgument);

            return command;
        }

        private void HandleOpenCommand(string? path)
        {
            if (path != null) 
            {
                AppState.Instance.LastOpenedPath = path.Trim();
            }
            Console.WriteLine($"Currect path: {AppState.Instance.LastOpenedPath}");
        }
    }
}
