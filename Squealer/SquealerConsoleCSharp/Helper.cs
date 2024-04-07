using Spectre.Console;
using SquealerConsoleCSharp.Extensions;
using SquealerConsoleCSharp.Models;
using SquealerConsoleCSharp.MyXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp
{
    public class Helper
    {

        public static bool CheckFolderValid()
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

        public static string GetFilePath(string fileName)
        {
            if (CheckFolderValid())
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

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
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


        public static void PrintTable(List<XmlToSqlConverter> xmlToSqlList)
        {

            var table = new Table();
            table.AddColumn("Type");
            table.AddColumn("Name");
            foreach (var x in xmlToSqlList)
            {
                var attr = x.SquealerObject.Type.GetObjectTypeAttribute();
                table.AddRow(attr.ShortName, $"{x.SqlrFileInfo.SqlObjectName}[green]{ attr.NumericSymbol}[/]");
            }
            AnsiConsole.Write(table);

            AnsiConsole.Write($"\n# {xmlToSqlList.Count} files.\n");
        }


    }
}
