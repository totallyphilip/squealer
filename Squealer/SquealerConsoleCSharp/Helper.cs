using Spectre.Console;
using SquealerConsoleCSharp.Attributes;
using SquealerConsoleCSharp.Extensions;
using SquealerConsoleCSharp.Models;
using SquealerConsoleCSharp.Models.Git;
using SquealerConsoleCSharp.MyXml;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp
{
    internal class Helper
    {

        public static bool VadilateFolder()
        {
            if (string.IsNullOrWhiteSpace(AppState.Instance.LastOpenedPath))
            {
                Console.WriteLine("use 'open' to Open a folder first.");
                return false;
            }
            else if (!Directory.Exists(AppState.Instance.LastOpenedPath))
            {
                Console.WriteLine("Invalid Directory");
                return false;
            }
            return true;
                
        }


        public static string GetSettingsPath()
        {
            string fileName = "settings.sqlrxml";
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(localAppDataPath, "Squealer");

            Directory.CreateDirectory(appFolder);

            string fullPath = Path.Combine(appFolder, fileName);
            return fullPath;
        }

        public static string GetFilePath(string fileName)
        {
            if (VadilateFolder())
            {
                var filePath = Path.Combine(AppState.Instance.LastOpenedPath, fileName);
                return filePath;
            }
            else
            {
                return string.Empty;
            }
        }

        public static T ParseDescriptionToEnum<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException($"Not found: {description}", nameof(description));
        }


        public static List<string> SearchSqlrFilesInFolder(string? patterns)
        {
            if (string.IsNullOrEmpty(patterns))
            {
                patterns = "*";
            }
            List<string> searchPatterns = patterns.Split('|').ToList();


            searchPatterns = searchPatterns.Select(x => x + ".sqlr").ToList();
            

            Console.WriteLine($"# finding all files matching {string.Join('|', searchPatterns)}");

            // Get files matching any of the patterns and remove duplicates
            var filePaths = searchPatterns.SelectMany(pattern =>
                             Directory.EnumerateFiles(AppState.Instance.LastOpenedPath, pattern))
                             .Distinct()
                             .ToList();

            return filePaths.ToList();
        }


        public static void PrintTable(List<XmlToSqlConverter> xmlToSqlList, List<GitFileInfo> gitFileInfos) 
        {
            var gitFileInfosDict = gitFileInfos.ToDictionary(x => x.FileName, x => x.Status);
            var table = new Spectre.Console.Table();
            table.AddColumn("Type");
            table.AddColumn("Name");
            table.AddColumn("Git Status");
            foreach (var x in xmlToSqlList)
            {
                var attr = x.SquealerObject.Type.GetObjectTypeAttribute();
                
                var hasGitInfo = gitFileInfosDict.TryGetValue(x.SqlrFileInfo.FileName, out var gitStatus);

                table.AddRow(attr.ObjectTypeCode, $"{x.SqlrFileInfo.SqlObjectName}[green]{ attr.NumericSymbol}[/]", hasGitInfo ? $"{gitStatus}" : "");
            }
            AnsiConsole.Write(table);

            AnsiConsole.Write($"\n# {xmlToSqlList.Count} files.\n");
        }


        public static Option<bool> CreateFlagOption(string alias, string description)
        {
            return new Option<bool>(
                aliases: new[] { alias },
                description: description,
                getDefaultValue: () => false
                );
        }

        public static bool IsValidFilename(string filename)
        {
            // Check for invalid characters.
            if (filename.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            {
                return false;
            }

            // Optional: Add check for reserved names here if targeting Windows.

            // Check for length. This is a simple example; you may need to adjust based on your requirements.
            if (filename.Length > 255) // Using 255 as a generic safe limit.
            {
                return false;
            }

            // Add any additional checks you require here.

            return true; // Passed all checks.
        }

        

        public static void OpenFileWithDefaultProgram(string filename)
        {
            var filePath = Path.Combine(AppState.Instance.LastOpenedPath, filename);
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true // Open the file with the system's default associated application
                };

                Process.Start(startInfo); // Start the process with this configuration
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
            }
        }

        public static string ReplaceFirstOccurrence(string source, string find, string replace)
        {
            int place = source.IndexOf(find);
            if (place < 0)
                return source; // No occurrence found

            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

           
    }
}
