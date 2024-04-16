using Microsoft.VisualBasic.FileIO;
using SquealerConsoleCSharp;
using SquealerConsoleCSharp.Models;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SquealerConsoleCSharp.Extensions;
using System.Xml;
using System.Reflection.Metadata;


namespace SquealerConsoleCSharp.Models
{
    #region Squealer
    [XmlRoot("Squealer")]
    public class SquealerObject
    {

        public SquealerObject(EType type, bool isNewFile)
        {
            Type = type;
            IsNewFile = isNewFile;
        }

        // empty constructor for xml serilization
        public SquealerObject()
        {
            
        }

        [XmlIgnore]
        public bool IsNewFile { get; } = false;

        [XmlIgnore]
        public EType Type { get; set; } = default;

        [XmlAttribute("Type")]
        public string TypeString
        {
            get => Type.GetObjectTypeAttribute().Name;
            set => Type = typeof(EType).GetFileTypeByName(value);
        }

        [XmlAttribute("Flags")]
        public string Flags { get; set; } = default!;

        [XmlAttribute("WithOptions")]
        public string WithOptions { get; set; } = default!;

        [XmlElement("PreCode")]
        public string PreCode { get; set; } = string.Empty;

        [XmlElement("Comments")]
        public string Comments { get; set; } = string.Empty;

        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();


        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; } = new Table();


        [XmlElement(ElementName = "Returns")]
        public Returns Returns { get; set; } = new Returns();

        [XmlElement("Code")]
        public string Code { get; set; } = string.Empty;

        public bool NoMagic { get
            {
                return Code.Trim().StartsWith("--nomagic");
            } }


        [XmlArray("Users")]
        [XmlArrayItem("User")]
        public List<User> Users { get; set; } = new List<User>();

        [XmlElement("PostCode")]
        public string PostCode { get; set; } = string.Empty;




        public string GetSqlResource(ESqlResourseType resourseType, EsqlScriptMode mode)
        {
            if (sqlResourceMap.TryGetValue((Type, resourseType, mode), out var resource))
            {
                return resource;
            }

            throw new ArgumentException("Invalid type or resource type");
        }


        


        /***
         * Combine the config to create xml file
         *  - 
         */
        public void ExportXmlFile(string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true, // For readability
                Encoding = System.Text.Encoding.ASCII,
                
            };

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                // Start the document
                writer.WriteStartDocument();
                writer.WriteComment(" Flags example: \"x;exclude from project|r;needs refactoring\" (recommend single-character flags) ");

                // Start the Squealer element
                writer.WriteStartElement("Squealer");
                writer.WriteAttributeString("Type", Type.GetObjectTypeAttribute().Name);
                writer.WriteAttributeString("Flags", Flags);
                writer.WriteAttributeString("WithOptions", WithOptions);

                // PreCode with comment
                writer.WriteComment(" Optional T-SQL to execute before the main object is created. ");
                writer.WriteStartElement("PreCode");
                writer.WriteCData(PreCode);
                writer.WriteEndElement(); // PreCode

                // Comments with comment
                writer.WriteComment($" Describe the purpose of this {Type.GetObjectTypeAttribute().FriendlyName} and any difficult concepts. ");
                writer.WriteStartElement("Comments");
                writer.WriteCData(Comments);
                writer.WriteEndElement(); // Comments


  
                // PARAMETERS
                if (Type == EType.StoredProcedure || Type == EType.ScalarFunction || Type == EType.MultiStatementTableFunction || Type == EType.InlineTableFunction)
                {
                    writer.WriteStartElement("Parameters");
                    if (IsNewFile)
                    {
                        var outputString = Type == EType.StoredProcedure ? "Output=\"False\" " : "";
                        writer.WriteComment($"<Parameter Name=\"MyParameter\" Type=\"varchar(50)\" {outputString}DefaultValue=\"\" Comments=\"\" />");
                    }
                    // Assuming Parameter is a class with Name, Type, Output, DefaultValue, Comments
                    foreach (var parameter in Parameters)
                    {
                        writer.WriteStartElement("Parameter");
                        writer.WriteAttributeString("Name", parameter.Name);
                        writer.WriteAttributeString("Type", parameter.DataType);
                        writer.WriteAttributeString("Output", parameter.Output.ToString());
                        writer.WriteAttributeString("DefaultValue", parameter.DefaultValue);
                        writer.WriteAttributeString("Comments", parameter.Comments);
                        writer.WriteEndElement(); // Parameter
                    }
                    writer.WriteEndElement(); // Parameters
                }

                // TABLE
                if (Type == EType.View || Type == EType.MultiStatementTableFunction)
                {
                    if (Type == EType.View)
                    {
                        writer.WriteComment(" Define the column(s) to return from this view. ");
                    }
                    else if (Type == EType.MultiStatementTableFunction)
                    {
                        writer.WriteComment(" Define the column(s) for @TableValue, your table-valued return variable. ");
                    }

                    // Table
                    writer.WriteStartElement("Table");
                    if (Type == EType.MultiStatementTableFunction)
                    {
                        writer.WriteAttributeString("PrimaryKeyClustered", Table.PrimaryKeyClustered);
                    }
                    if (IsNewFile && Type == EType.View)
                    {
                        writer.WriteComment("<Column Name=\"MyColumn\" Comments=\"\" />");

                    }
                    foreach (var col in Table.Columns)
                    {
                        // Add the Column element within the Table.
                        writer.WriteStartElement("Column");
                        writer.WriteAttributeString("Name", col.Name);
                        if (Type == EType.MultiStatementTableFunction)
                        {
                            writer.WriteAttributeString("Type", col.Type);
                            writer.WriteAttributeString("Nullable", col.NullableString);
                            writer.WriteAttributeString("Identity", col.IdentityString);
                            writer.WriteAttributeString("IncludeInPrimaryKey", col.IncludeInPrimaryKeyString);
                            writer.WriteAttributeString("Comments", col.Comments);
                        }
                        writer.WriteEndElement(); // End of Column
                    }

                    // End the Table element.
                    writer.WriteEndElement();
                }

                // RETURNS
                if (Type == EType.ScalarFunction)
                {
                    writer.WriteComment(" Define the data type For @Result, your scalar Return variable. ");
                    writer.WriteStartElement("Returns");
                    writer.WriteAttributeString("Type", Returns.Type);
                    writer.WriteEndElement(); // Returns
                }
                

                // Code
                writer.WriteStartElement("Code");
                writer.WriteCData(Code);
                writer.WriteEndElement(); // Code

                // Users
                writer.WriteStartElement("Users");

                // add the config users

                if (Users.Count == 0)
                    Users.Add(new User { Name = "$DBUSER$" });
                foreach (var user in Users)
                {
                    writer.WriteStartElement("User");
                    writer.WriteAttributeString("Name", user.Name);
                    writer.WriteEndElement(); // User
                }
                writer.WriteEndElement(); // Users
                

                // PostCode with comment
                writer.WriteComment(" Optional T-SQL to execute after the main object is created. ");
                writer.WriteStartElement("PostCode");
                writer.WriteCData(PostCode);
                writer.WriteEndElement(); // PostCode

                // End the Squealer element
                writer.WriteEndElement();

                // End the document
                writer.WriteEndDocument();
            }
        }


        #region static
        private static Dictionary<(EType, ESqlResourseType, EsqlScriptMode), string> sqlResourceMap = new Dictionary<(EType, ESqlResourseType, EsqlScriptMode), string>
        {
            {(EType.StoredProcedure, ESqlResourseType.Create, EsqlScriptMode.Normal), MyResources.Resources.P_Create},
            {(EType.StoredProcedure, ESqlResourseType.Table, EsqlScriptMode.Normal), string.Empty},
            {(EType.StoredProcedure, ESqlResourseType.Begin, EsqlScriptMode.Normal), MyResources.Resources.P_Begin},
            {(EType.StoredProcedure, ESqlResourseType.End, EsqlScriptMode.Normal), MyResources.Resources.P_End},
            {(EType.StoredProcedure, ESqlResourseType.Create, EsqlScriptMode.NoMagic), MyResources.Resources.P_Create},
            {(EType.StoredProcedure, ESqlResourseType.Table, EsqlScriptMode.NoMagic), string.Empty},
            {(EType.StoredProcedure, ESqlResourseType.Begin, EsqlScriptMode.NoMagic), MyResources.Resources.P_BeginNoMagic},
            {(EType.StoredProcedure, ESqlResourseType.End, EsqlScriptMode.NoMagic), MyResources.Resources.P_EndNoMagic},
            {(EType.StoredProcedure, ESqlResourseType.Create, EsqlScriptMode.Test), string.Empty},
            {(EType.StoredProcedure, ESqlResourseType.Table, EsqlScriptMode.Test), string.Empty},
            {(EType.StoredProcedure, ESqlResourseType.Begin, EsqlScriptMode.Test), MyResources.Resources.P_BeginTest},
            {(EType.StoredProcedure, ESqlResourseType.End, EsqlScriptMode.Test), MyResources.Resources.P_EndTest},

            {(EType.ScalarFunction, ESqlResourseType.Create, EsqlScriptMode.Normal), MyResources.Resources.FN_Create},
            {(EType.ScalarFunction, ESqlResourseType.Table, EsqlScriptMode.Normal), string.Empty},
            {(EType.ScalarFunction, ESqlResourseType.Begin, EsqlScriptMode.Normal), MyResources.Resources.FN_Begin},
            {(EType.ScalarFunction, ESqlResourseType.End, EsqlScriptMode.Normal), MyResources.Resources.FN_End},
            {(EType.ScalarFunction, ESqlResourseType.Create, EsqlScriptMode.NoMagic), MyResources.Resources.FN_Create},
            {(EType.ScalarFunction, ESqlResourseType.Table, EsqlScriptMode.NoMagic), string.Empty},
            {(EType.ScalarFunction, ESqlResourseType.Begin, EsqlScriptMode.NoMagic), MyResources.Resources.FN_Begin},
            {(EType.ScalarFunction, ESqlResourseType.End, EsqlScriptMode.NoMagic), MyResources.Resources.FN_End},
            {(EType.ScalarFunction, ESqlResourseType.Create, EsqlScriptMode.Test), string.Empty},
            {(EType.ScalarFunction, ESqlResourseType.Table, EsqlScriptMode.Test), string.Empty},
            {(EType.ScalarFunction, ESqlResourseType.Begin, EsqlScriptMode.Test), MyResources.Resources.FN_BeginTest},
            {(EType.ScalarFunction, ESqlResourseType.End, EsqlScriptMode.Test), MyResources.Resources.FN_EndTest},


            {(EType.InlineTableFunction, ESqlResourseType.Create, EsqlScriptMode.Normal), MyResources.Resources.FN_Create},
            {(EType.InlineTableFunction, ESqlResourseType.Table, EsqlScriptMode.Normal), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.Begin, EsqlScriptMode.Normal), MyResources.Resources.IF_Begin},
            {(EType.InlineTableFunction, ESqlResourseType.End, EsqlScriptMode.Normal), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.Create, EsqlScriptMode.NoMagic), MyResources.Resources.FN_Create},
            {(EType.InlineTableFunction, ESqlResourseType.Table, EsqlScriptMode.NoMagic), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.Begin, EsqlScriptMode.NoMagic), MyResources.Resources.IF_Begin},
            {(EType.InlineTableFunction, ESqlResourseType.End, EsqlScriptMode.NoMagic), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.Create, EsqlScriptMode.Test), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.Table, EsqlScriptMode.Test), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.Begin, EsqlScriptMode.Test), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.End, EsqlScriptMode.Test), string.Empty},

            {(EType.MultiStatementTableFunction, ESqlResourseType.Create, EsqlScriptMode.Normal), MyResources.Resources.FN_Create},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Table, EsqlScriptMode.Normal), MyResources.Resources.TF_Table},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Begin, EsqlScriptMode.Normal), MyResources.Resources.Tf_Begin},
            {(EType.MultiStatementTableFunction, ESqlResourseType.End, EsqlScriptMode.Normal),MyResources.Resources.TF_End},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Create, EsqlScriptMode.NoMagic), MyResources.Resources.FN_Create},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Table, EsqlScriptMode.NoMagic), MyResources.Resources.TF_Table},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Begin, EsqlScriptMode.NoMagic), MyResources.Resources.Tf_Begin},
            {(EType.MultiStatementTableFunction, ESqlResourseType.End, EsqlScriptMode.NoMagic),MyResources.Resources.TF_End},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Create, EsqlScriptMode.Test), string.Empty},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Table, EsqlScriptMode.Test), MyResources.Resources.TF_TableTest},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Begin, EsqlScriptMode.Test), MyResources.Resources.Tf_BeginTest},
            {(EType.MultiStatementTableFunction, ESqlResourseType.End, EsqlScriptMode.Test),MyResources.Resources.TF_EndTest},

            {(EType.View, ESqlResourseType.Create, EsqlScriptMode.Normal), MyResources.Resources.V_Create},
            {(EType.View, ESqlResourseType.Table, EsqlScriptMode.Normal), string.Empty},
            {(EType.View, ESqlResourseType.Begin, EsqlScriptMode.Normal), MyResources.Resources.V_Begin},
            {(EType.View, ESqlResourseType.End, EsqlScriptMode.Normal), string.Empty},
            {(EType.View, ESqlResourseType.Create, EsqlScriptMode.NoMagic), MyResources.Resources.V_Create},
            {(EType.View, ESqlResourseType.Table, EsqlScriptMode.NoMagic), string.Empty},
            {(EType.View, ESqlResourseType.Begin, EsqlScriptMode.NoMagic), MyResources.Resources.V_Begin},
            {(EType.View, ESqlResourseType.End, EsqlScriptMode.NoMagic), string.Empty},
            {(EType.View, ESqlResourseType.Create, EsqlScriptMode.Test), string.Empty},
            {(EType.View, ESqlResourseType.Table, EsqlScriptMode.Test), string.Empty},
            {(EType.View, ESqlResourseType.Begin, EsqlScriptMode.Test), string.Empty},
            {(EType.View, ESqlResourseType.End, EsqlScriptMode.Test), string.Empty},
        };

        public static SquealerObject GetNewObject(EType type)
        {
            var res = new SquealerObject(type, true);
            string selectPlaceHolder = type switch
            {
                EType.StoredProcedure => "select 'hello world! love, ``this``'\r\n\r\n\r\n--optional (see https://docs.microsoft.com/en-us/sql/t-sql/language-elements/return-transact-sql?view=sql-server-ver15)\r\n--set @Squealer_ReturnValue = [ integer_expression ]",
                EType.ScalarFunction => "set @Result = 'hello world! love, ``this``'",
                EType.InlineTableFunction => "select 'hello world! love, ``this``' as [MyColumn]",
                EType.MultiStatementTableFunction => "insert @TableValue select 'hello world! love, ``this``'",
                EType.View => "select 'hello world! love, ``this``' as hello",
                _ => throw new InvalidOperationException()
            };

            res.Code = $"\n\n" +
                $"/***********************************************************************\n" +
                $"\tComments.\n" +
                $"***********************************************************************/\n" +
                $"\n{selectPlaceHolder}\n\n";

            res.PreCode = "\n\n\n\n";
            res.Comments = "\n\n\n\n";
            res.PostCode = "\n\n\n\n";

            var config = ConfigObject.GetConfig();
            if (config != null && config.Users.Count > 0)
            {
                foreach (var u in config.Users)
                {
                    res.Users.Add(new User { Name = u.Name });
                }
            }

            if (type == EType.MultiStatementTableFunction)
            {
                res.Table.Columns.Add(new Column
                {
                    Name = "MyColumn",
                    Type = "varchar(50)",
                    Nullable = false,
                    Identity = false,
                    IncludeInPrimaryKey = false,
                    Comments = ""
                });
            }

            if (type == EType.ScalarFunction)
            {
                res.Returns.Type = "varchar(100)";
            }


            return res;

        }


        #endregion

    }
    #endregion

    #region parameter

    public class Parameter
    {
        [XmlAttribute("Name")]
        public string Name { get; set; } = default!;

        [XmlAttribute("Type")]
        public string DataType { get; set; } = default!;

        [XmlIgnore]
        public bool Output { get; set; }

        [XmlAttribute("Output")]
        public string OutputString
        {
            get => Output.ToString();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    Output = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for Output. Expected 'true' or 'false'.");
                }
            }
        }

        [XmlIgnore]
        public bool ReadOnly { get; set; }

        [XmlAttribute("ReadOnly")]
        public string ReadOnlyString
        {
            get => ReadOnly.ToString();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    ReadOnly = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for Output. Expected 'true' or 'false'.");
                }
            }
        }

        [XmlAttribute("DefaultValue")]
        public string DefaultValue { get; set; } = default!;

        [XmlAttribute("Comments")]
        public string Comments { get; set; } = default!;
    }

    #endregion

    #region User

    public class User
    {
        [XmlAttribute("Name")]
        public string Name { get; set; } = default!;
    }

    #endregion

    public class Table
    {
        [XmlAttribute("PrimaryKeyClustered")]
        public string PrimaryKeyClustered { get; set; } = "False";

        [XmlElement(ElementName = "Column")]
        public List<Column> Columns { get; set; } = new List<Column>();
    }

    public class Column
    {
        [XmlAttribute("Name")]
        public string Name { get; set; } = default!;

        [XmlAttribute("Type")]
        public string Type { get; set; } = default!;

        [XmlIgnore]
        public bool Nullable { get; set; }

        [XmlAttribute("Nullable")]
        public string NullableString
        {
            get => Nullable.ToString();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    Nullable = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for Parameter => Nullable. Expected 'true' or 'false'.");
                }
            }
        }

        [XmlIgnore]
        public bool Identity { get; set; }

        [XmlAttribute("Identity")]
        public string IdentityString
        {
            get => Identity.ToString();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    Identity = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for Parameter => Identity. Expected 'true' or 'false'.");
                }
            }
        }

        [XmlIgnore]
        public bool IncludeInPrimaryKey { get; set; }

        [XmlAttribute("IncludeInPrimaryKey")]
        public string IncludeInPrimaryKeyString
        {
            get => IncludeInPrimaryKey.ToString();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    IncludeInPrimaryKey = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for Parameter => IncludeInPrimaryKey. Expected 'true' or 'false'.");
                }
            }
        }

        [XmlAttribute("Comments")]
        public string Comments { get; set; } = string.Empty;
    }
    public class Returns
    {
        [XmlAttribute("Type")]
        public string Type { get; set; } = string.Empty;
    }

}