using SquealerConsoleCSharp.CustomCommands;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SquealerConsoleCSharp.MyXml;
using SquealerConsoleCSharp.Models;
using Spectre.Console;
using SquealerConsoleCSharp.Extensions;
using System.Reflection;
using System.Windows.Forms;
using TextCopy;

namespace SquealerConsoleCSharp.CustomCommands
{
    internal class GenCommand : BaseDirCommand
    {
        public GenCommand() : base("gen", "Generate SQL Scripts")
        {

        }

        protected override void ExtraImplementation(bool p, bool fn, bool _if, bool tf, bool v, bool alter, bool test, bool encryption, string? searchtext)
        {
            if(_xmlToSqls.Count == 0)
            {
                return;
            }

            var config = ConfigObject.GetConfig();

            StringBuilder output = new StringBuilder();
            if (!test)
            {
                output.AppendLine(MyResources.Resources.TrackFailedItems_Start);
                output.Append(MyResources.Resources._TopScript);
            }

            foreach (var item in _xmlToSqls.Select((xml,index)=> (xml, index)))
            {
                var version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                var countString = $"{item.index+1}/{_xmlToSqls.Count}";

                output.AppendLine(GetSqlOfOneFile(item.xml, version, config, countString, alter, test, encryption));
            }

            if (!test)
            {
                output.AppendLine();
                output.Append(MyResources.Resources.TrackFailedItems_End);
            }

            AnsiConsole.WriteLine("# Output copied to Windows clipboard.");

            if (AppState.Instance.Settings.Output.CopyToClipboard)
            {
                AnsiConsole.MarkupLine("\n#Output copied to clipboard.");
                ClipboardService.SetText(output.ToString());
            }

            if(AppState.Instance.Settings.Output.ExportToSqlFile) 
            {
                // Get the system temp directory
                string tempBase = Path.GetTempPath();

                // Define a subdirectory specific to your application
                string appTempDir = Path.Combine(tempBase, "SqlrTempFiles");

                Directory.CreateDirectory(appTempDir);

                // Generate a unique filename within the temp folder
                string tempFile = Path.Combine(appTempDir, $"TempFile_{Guid.NewGuid()}.sql");

               

                File.WriteAllText(tempFile, output.ToString());

                Helper.OpenFileWithDefaultProgram(tempFile);
            }


        }


        private string GetSqlOfOneFile(XmlToSqlConverter xml, string version, ConfigObject config, string countString, bool alter, bool test, bool encrytion)
        {
            var methodName = xml.SqlrFileInfo.SqlObjectName_w_Bracket;
            var type = xml.SquealerObject.Type;


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
            if(type == EType.View && xml.SquealerObject.Table.Columns.Count == 0 && getWithOption().Length == 0)
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
                .Replace("{Parameters}", $"{(xml.SquealerObject.Parameters.Count == 0 ? "": "\n")}" + string.Join('\n', GetParaErrorList(xml)))
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


            oneSql.Append($"----- {methodName} ".PadRight(107, '-') +" <EOF>\n");


            oneSql.Replace("``this``", methodName);

            foreach (var replacement in config.StringReplacements)
            {
                oneSql.Replace(replacement.Original, replacement.Replacement);
            }


            
            return oneSql.ToString();
        }

        #region private

        // para declarations
        private List<string> GetParaDeclarationList(XmlToSqlConverter xml)
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
        private List<string> GetVariablesDeclarationList(XmlToSqlConverter xml)
        {
            return xml.SquealerObject.Parameters
                        .Select((p, index) => (p, index))
                        .Select(item =>
                            $"declare @{item.p.Name} {item.p.DataType} = {item.p.DefaultValue};" +
                            $"{(!string.IsNullOrWhiteSpace(item.p.Comments) ? $" -- {item.p.Comments}" : "")}")
                        .ToList();
        }

        // table columns
        private List<string> GetTableColumns(XmlToSqlConverter xml)
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
        private List<string> GetParaErrorList(XmlToSqlConverter xml) 
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
