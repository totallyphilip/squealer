﻿using Spectre.Console;
using SquealerConsoleCSharp.Models;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TextCopy;
using System.Security.Cryptography;
using System.Diagnostics.Metrics;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class FixCommand: BaseDirCommand
    {
        public FixCommand() : base("fix", "Regenerate sqlr files", false)
        {

        }

        public override Command CreateCommand()
        {
            var command = base.CreateCommand();

            // Add additional options specific to the derived class
            var convertaOpt = new Option<string?>(
                aliases: new[] { "-mode" },
                description: "",
                getDefaultValue: () => string.Empty
                );

            command.AddOption(convertaOpt);

            // Update the handler to handle the additional options
            command.SetHandler((InvocationContext context) =>
            {
                // Retrieve the values for base options
                bool proc = context.ParseResult.GetValueForOption(_procOpt);
                bool scalarFunction = context.ParseResult.GetValueForOption(_scalarFunctionOpt);
                bool inlineTVF = context.ParseResult.GetValueForOption(_inlineTVFOpt);
                bool multiStatementTVF = context.ParseResult.GetValueForOption(_multiStatementTVFOpt);
                bool view = context.ParseResult.GetValueForOption(_viewOpt);
                bool unCommitted = context.ParseResult.GetValueForOption(_unCommittedOpt);
                string? diff = context.ParseResult.GetValueForOption(_diffOpt);
                string? searchText = context.ParseResult.GetValueForArgument(_pathArgument);
                string? convertOpt = context.ParseResult.GetValueForOption(convertaOpt);

                // Call the base handle method
                BasicHandling(proc, scalarFunction, inlineTVF, multiStatementTVF, view, unCommitted, diff, searchText);

                // Handle additional logic
                ExtraImplementation(convertOpt);
            });

            return command;
        }

        private void ExtraImplementation(string? convertOpt)
        {

            if (!string.IsNullOrWhiteSpace(convertOpt))
            {
                //var modeSet = convertOpt.Split('|').Select(m => m.Trim().ToLower()).Distinct().ToHashSet();
                //if (modeSet.Contains("alt"))
                //    alter = true;
                //if (modeSet.Contains("e"))
                //    encrypt = true;
                //if (!modeSet.Any(m => new[] { "alt", "e", }.Contains(m)) && modeSet.Count > 0)
                //{
                //    Console.WriteLine("Invalid mode(s) specified.");
                //    return;
                //}
            }

            HashSet<string> fixedFiles = new HashSet<string>();
            foreach(var file in _xmlToSqls) 
            {
                // save the output in a temp file
                // use checksum to see if the temp file same as the original
                // if changed, re
                var filePath = file.SqlrFileInfo.FilePath;
                var tempFilePath = GenerateTempFilePath();
                file.SquealerObject.ExportXmlFile(tempFilePath);

                if (IsFileChanged(filePath, tempFilePath))
                {
                    // Replace the original file with the fixed file
                    File.Copy(tempFilePath, filePath, overwrite: true);
                    fixedFiles.Add(file.SqlrFileInfo.FileName);
                }
            }

            Helper.PrintTable(_xmlToSqls, _gitFileInfos, fixedFiles);

        }

        private static string GenerateTempFilePath()
        {
            string tempPath = Path.GetTempPath();
            string tempFileName = Guid.NewGuid().ToString() + "sqlr";
            return Path.Combine(tempPath, tempFileName);
        }

        private static bool IsFileChanged(string originalFilePath, string fixedFilePath)
        {
            string originalChecksum = ComputeChecksum(originalFilePath);
            string fixedChecksum = ComputeChecksum(fixedFilePath);
            return !originalChecksum.Equals(fixedChecksum);
        }

        private static string ComputeChecksum(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
