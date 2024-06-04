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


        public static void PrintTable(List<XmlToSqlConverter> xmlToSqlList, List<GitFileInfo> gitFileInfos, HashSet<string>? fixedFiles = null, EType? newType = null) 
        {
            var gitFileInfosDict = gitFileInfos.ToDictionary(x => x.FileName, x => x.Status);

            string statusText = newType == null ? "Fixed" : "Convert";

            var table = new Spectre.Console.Table();
            table.AddColumn("Type");
            table.AddColumn("Name");
            table.AddColumn("Git Status");
            if(fixedFiles != null)
            {
                table.AddColumn("Fixed Or Not");
            }
            foreach (var x in xmlToSqlList)
            {
                var attr = x.SquealerObject.Type.GetObjectTypeAttribute();
                
                var hasGitInfo = gitFileInfosDict.TryGetValue(x.SqlrFileInfo.FileName, out var gitStatus);
                if (fixedFiles != null)
                {
                    var fileNameDisplay = fixedFiles.Contains(x.SqlrFileInfo.FileName) ? $"[yellow]{x.SqlrFileInfo.SqlObjectName}[/]" : x.SqlrFileInfo.SqlObjectName;

                    table.AddRow(
                        attr.ObjectTypeCode, 
                        $"{fileNameDisplay}[green]{attr.NumericSymbol}[/]", 
                        hasGitInfo ? $"{gitStatus}" : "",
                        fixedFiles.Contains(x.SqlrFileInfo.FileName) ? $"[yellow]{statusText}[/]" : ""
                        );
                }
                else
                    table.AddRow(
                        attr.ObjectTypeCode, 
                        $"{x.SqlrFileInfo.SqlObjectName}[green]{attr.NumericSymbol}[/]", 
                        hasGitInfo ? $"{gitStatus}" : ""
                        );

            }
            AnsiConsole.Write(table);

            
            string fixedCount = fixedFiles != null ?
                $"{statusText} {fixedFiles.Count.ToString()} files" :
                "";

            AnsiConsole.Write($"\n# {xmlToSqlList.Count} files.{fixedCount}\n");
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

        public static string GetSqlOfOneFile(XmlToSqlConverter xml, ConfigObject config, string countString, bool alter, bool test, bool encrytion)
        {
            var methodName = xml.SqlrFileInfo.SqlObjectName_w_Bracket;
            var type = xml.SquealerObject.Type;
            var version = Assembly.GetEntryAssembly().GetName().Version.ToString();


            string GetSqlResourceByType(ESqlResourseType type)
            {
                var mode = test ? EsqlScriptMode.Test :
                xml.SquealerObject.NoMagic ? EsqlScriptMode.NoMagic : EsqlScriptMode.Normal;
                return xml.SquealerObject.GetSqlResource(type, mode);
            }


            StringBuilder oneSql = new StringBuilder();

            oneSql.AppendLine($"----- {methodName} ".PadRight(107, '-') + " <BOF>\n");
            if (!test)
            {
                oneSql.AppendLine($"-- additional code to execute after {type.GetObjectTypeAttribute().Name} is created");
                oneSql.AppendLine($"print '{countString} creating {methodName}, {type.GetObjectTypeAttribute().Name}'\r\ngo");


                if (xml.SquealerObject.PreCode.Trim().Length > 0)
                {
                    oneSql.Append($"\n\n{xml.SquealerObject.PreCode.Trim()}\n\n\ngo\n\n");
                }
                else
                {
                    oneSql.Append($"\r\n\r\n\r\n\r\n\r\ngo\n\n");
                }
            }


            if (!alter && !test)
            {
                oneSql.AppendLine($"if object_id('{methodName}','p') is not null\r\n" +
                    $"\tdrop procedure {methodName};\r\n" +
                    $"if object_id('{methodName}','fn') is not null\r\n" +
                    $"\tdrop function {methodName};\r\n" +
                    $"if object_id('{methodName}','if') is not null\r\n" +
                    $"\tdrop function {methodName};\r\n" +
                    $"if object_id('{methodName}','tf') is not null\r\n" +
                    $"\tdrop function {methodName};\r\nif object_id('{methodName}','v') is not null\r\n" +
                    $"\tdrop view {methodName};\r\n\r\n" +
                    $"go\n");
            }


            oneSql.AppendLine("/***********************************************************************\r\n\r\n" +
                                $"title : {xml.SqlrFileInfo.SqlObjectName}\r\n\r\n\r\n" +
                                $"{xml.SquealerObject.Comments.Trim()}" +
                                $"\r\n\r\n\r\n[generated with Squealer {version}]\r\n\r\n" +
                                $"***********************************************************************/\n");

            if (!test)
            {
                // CREATE
                var createScript = alter ?
                    Helper.ReplaceFirstOccurrence(GetSqlResourceByType(ESqlResourseType.Create), "create", "alter") :
                    GetSqlResourceByType(ESqlResourseType.Create);

                oneSql.AppendLine(createScript
                    .Replace("[{Schema}].[{RootProgramName}]", methodName)
                    .Replace("{ReturnDataType}", xml.SquealerObject.Returns.Type)
                    .Trim() + "\n");
            }

            // declare para
            if (xml.SquealerObject.Parameters.Count > 0)
            {
                var declarations = test ? GetVariablesDeclarationList(xml) : GetParaDeclarationList(xml);
                {
                    oneSql.AppendLine(string.Join('\n', declarations));
                }
                if (test)
                    oneSql.AppendLine();
            }

            // local fucntion for with options
            string getWithOption()
            {
                if (encrytion)
                {
                    if (string.IsNullOrWhiteSpace(xml.SquealerObject.WithOptions))
                        return "with encryption";
                    return $"with {xml.SquealerObject.WithOptions},encryption";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(xml.SquealerObject.WithOptions))
                        return string.Empty;
                    return $"with {xml.SquealerObject.WithOptions}";
                }
            }

            // TABLE
            oneSql.Append(GetSqlResourceByType(ESqlResourseType.Table)
                    .Trim());


            if (xml.SquealerObject.Table.Columns.Count > 0)
            {
                var colStr = string.Join('\n', GetTableColumns(xml));
                if (type == EType.View)
                    colStr = $"(\n{colStr}\n\n)";
                else
                    colStr = $"\n{colStr}";
                oneSql.Append($"{colStr}\n\n");

                // to match format
                if (type == EType.View)
                    oneSql.AppendLine();
            }

            // to match format
            if (type == EType.View && xml.SquealerObject.Table.Columns.Count == 0 && getWithOption().Length == 0)
            {
                oneSql.Append("\n");
            }

            // BEGIN
            oneSql.Append(GetSqlResourceByType(ESqlResourseType.Begin)
                    .Replace("{WithOptions}", getWithOption())
                    .Replace("{ReturnDataType}", xml.SquealerObject.Returns.Type)
                    .Trim());

            // to match format
            if (type == EType.View || type == EType.MultiStatementTableFunction)
            {
                oneSql.AppendLine();
            }

            //code
            oneSql.Append($"\n\n{xml.SquealerObject.Code.Trim()}\n\n");

            //End
            oneSql.AppendLine(
                GetSqlResourceByType(ESqlResourseType.End)
                .Replace("{Parameters}", $"{(xml.SquealerObject.Parameters.Count == 0 ? "" : "\n")}" + string.Join('\n', GetParaErrorList(xml)))
                );

            if (!alter && !test && xml.SquealerObject.Users.Count > 0)
            {
                // permission
                oneSql.Append("go\r\n" +
                    $"if object_id('{methodName}','{type.GetObjectTypeAttribute().ObjectTypeCode}') is not null\r\n" +
                    $"begin\n");
                foreach (var user in xml.SquealerObject.Users)
                {
                    oneSql.Append($"grant {type.GetObjectTypeAttribute().Permission} on {methodName} to [{user.Name}];\n");
                }

                oneSql.Append("end\r\n" +
                    $"else\r\n" +
                    $"begin\r\n" +
                    $"print 'Permissions not granted on {methodName}.';\r\n" +
                    $"insert ##RetryFailedSquealerItems (ProcName) values ('{methodName}');\r\n" +
                    $"end\r\n" +
                    $"go\n\n");
            }
            else
            {
                if (!test)
                    oneSql.Append("go\n\n");
                else
                    oneSql.AppendLine();
            }


            //Post Code
            if (!string.IsNullOrWhiteSpace(xml.SquealerObject.PostCode))
            {
                oneSql.Append($"-- additional code to execute after {type.GetObjectTypeAttribute().Name} is created\n" +
                    $"if object_id('{xml.SqlrFileInfo.SqlObjectName_w_Bracket}','{type.GetObjectTypeAttribute().ObjectTypeCode}') is not null\n" +
                    $"begin\n");

                oneSql.Append($"\n\n{xml.SquealerObject.PostCode.Trim()}\n\n\n");

                oneSql.Append("end\n" +
                    "else print 'PostCode not executed.'\n" +
                    "go\n\n");
            }


            oneSql.Append($"----- {methodName} ".PadRight(107, '-') + " <EOF>\n");


            oneSql.Replace("``this``", methodName);

            foreach (var replacement in config.StringReplacements)
            {
                oneSql.Replace(replacement.Original, replacement.Replacement);
            }



            return oneSql.ToString();
        }

        #region private

        // para declarations
        private static List<string> GetParaDeclarationList(XmlToSqlConverter xml)
        {
            return xml.SquealerObject.Parameters
                        .Select((p, index) => (p, index))
                        .Select(item =>
                            $"{(item.index == 0 ? "" : ",")}@{item.p.Name} {item.p.DataType}" +
                            $"{(!string.IsNullOrWhiteSpace(item.p.DefaultValue) ? $" = {item.p.DefaultValue}" : "")}" +
                            $"{(item.p.Output ? " output" : "")}" +
                            $"{(item.p.ReadOnly ? " readonly" : "")}" +
                            $"{(!string.IsNullOrWhiteSpace(item.p.Comments) ? $" -- {item.p.Comments}" : "")}")
                        .ToList();
        }

        // para declarations for TEST
        private static List<string> GetVariablesDeclarationList(XmlToSqlConverter xml)
        {
            return xml.SquealerObject.Parameters
                        .Select((p, index) => (p, index))
                        .Select(item =>
                            $"declare @{item.p.Name} {item.p.DataType} = {item.p.DefaultValue};" +
                            $"{(!string.IsNullOrWhiteSpace(item.p.Comments) ? $" -- {item.p.Comments}" : "")}")
                        .ToList();
        }

        // table columns
        private static List<string> GetTableColumns(XmlToSqlConverter xml)
        {
            string notNull()
            {
                return xml.SquealerObject.Type == EType.View ? "" : " Not null";
            }

            return xml.SquealerObject.Table.Columns
                        .Select((c, index) => (c, index))
                        .Select(item =>
                            $"{(item.index == 0 ? "" : ",")}[{item.c.Name}] {item.c.Type}" +
                            $"{(item.c.Nullable ? " null" : notNull())}{(item.c.Identity ? " identity" : "")}" +
                            $"{(item.c.IncludeInPrimaryKey ? $"\nprimary key clustered ({item.c.Name})" : "")}"
                            )
                        .ToList();
        }


        // para error log
        private static List<string> GetParaErrorList(XmlToSqlConverter xml)
        {
            return xml.SquealerObject.Parameters
                    .Select((p, index) => (p, index))
                    .Select(item => {
                        if (item.p.DataType.ToLower().Contains("max") || item.p.ReadOnly)
                        {
                            return $"\t\t--parameter @{item.p.Name} cannot be logged due to its 'max' or 'readonly' definition";
                        }
                        return $"\t\tset @Squealer_ErrorMessage =\r\n" +
                                $"\t\t\t@Squealer_ErrorMessage\r\n" +
                                $"\t\t\t+ char(10)\r\n" +
                                $"\t\t\t+ '@{item.p.Name} = '\r\n" +
                                $"\t\t\t+ isnull(convert(varchar(max),@{item.p.Name}),'[NULL]');";
                    })
                    .ToList();
        }

        #endregion

    }
}
