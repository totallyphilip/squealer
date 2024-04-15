﻿using Microsoft.VisualBasic.FileIO;
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

        [XmlIgnore]
        public bool RunLog { get; set; }

        [XmlAttribute("RunLog")]
        public string RunLogString
        {
            get => RunLog.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    RunLog = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for RunLog. Expected 'true' or 'false'.");
                }
            }
        }

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


        // 
        private static Dictionary<(EType, ESqlResourseType, bool), string> sqlResourceMap = new Dictionary<(EType, ESqlResourseType, bool), string>
        {
            {(EType.StoredProcedure, ESqlResourseType.Create, false), MyResources.Resources.P_Create},
            {(EType.StoredProcedure, ESqlResourseType.Table, false), string.Empty},
            {(EType.StoredProcedure, ESqlResourseType.Begin, false), MyResources.Resources.P_Begin},
            {(EType.StoredProcedure, ESqlResourseType.End, false), MyResources.Resources.P_End},
            {(EType.StoredProcedure, ESqlResourseType.Create, true), MyResources.Resources.P_Create},
            {(EType.StoredProcedure, ESqlResourseType.Table, true), string.Empty},
            {(EType.StoredProcedure, ESqlResourseType.Begin, true), MyResources.Resources.P_BeginNoMagic},
            {(EType.StoredProcedure, ESqlResourseType.End, true), MyResources.Resources.P_EndNoMagic},

            {(EType.ScalarFunction, ESqlResourseType.Create, false), MyResources.Resources.FN_Create},
            {(EType.ScalarFunction, ESqlResourseType.Begin, false), MyResources.Resources.FN_Begin},
            {(EType.ScalarFunction, ESqlResourseType.Table, false), string.Empty},
            {(EType.ScalarFunction, ESqlResourseType.End, false), MyResources.Resources.FN_End},
            {(EType.ScalarFunction, ESqlResourseType.Create, true), MyResources.Resources.FN_Create},
            {(EType.ScalarFunction, ESqlResourseType.Begin, true), MyResources.Resources.FN_Begin},
            {(EType.ScalarFunction, ESqlResourseType.Table, true), string.Empty},
            {(EType.ScalarFunction, ESqlResourseType.End, true), MyResources.Resources.FN_End},

            {(EType.InlineTableFunction, ESqlResourseType.Create, false), MyResources.Resources.FN_Create},
            {(EType.InlineTableFunction, ESqlResourseType.Begin, false), MyResources.Resources.IF_Begin},
            {(EType.InlineTableFunction, ESqlResourseType.Table, false), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.End, false), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.Create, true), MyResources.Resources.FN_Create},
            {(EType.InlineTableFunction, ESqlResourseType.Begin, true), MyResources.Resources.IF_Begin},
            {(EType.InlineTableFunction, ESqlResourseType.Table, true), string.Empty},
            {(EType.InlineTableFunction, ESqlResourseType.End, true), string.Empty},

            {(EType.MultiStatementTableFunction, ESqlResourseType.Create, false), MyResources.Resources.FN_Create},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Begin, false), MyResources.Resources.Tf_Begin},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Table, false), MyResources.Resources.TF_Table},
            {(EType.MultiStatementTableFunction, ESqlResourseType.End, false),MyResources.Resources.TF_End},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Create, true), MyResources.Resources.FN_Create},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Begin, true), MyResources.Resources.Tf_Begin},
            {(EType.MultiStatementTableFunction, ESqlResourseType.Table, true), MyResources.Resources.TF_Table},
            {(EType.MultiStatementTableFunction, ESqlResourseType.End, true),MyResources.Resources.TF_End},


            {(EType.View, ESqlResourseType.Create, false), MyResources.Resources.V_Create},
            {(EType.View, ESqlResourseType.Begin, false), MyResources.Resources.V_Begin},
            {(EType.View, ESqlResourseType.Table, false), string.Empty},
            {(EType.View, ESqlResourseType.End, false), string.Empty},
            {(EType.View, ESqlResourseType.Create, true), MyResources.Resources.V_Create},
            {(EType.View, ESqlResourseType.Begin, true), MyResources.Resources.V_Begin},
            {(EType.View, ESqlResourseType.Table, true), string.Empty},
            {(EType.View, ESqlResourseType.End, true), string.Empty},
        };

        public string GetSqlResource(ESqlResourseType resourseType)
        {
            if (sqlResourceMap.TryGetValue((Type, resourseType, NoMagic), out var resource))
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
            // Override Property for 
            string? placeHolder_code = null;
            if (IsNewFile)
            {
                string selectPlaceHolder = Type switch
                {
                    EType.StoredProcedure => "select 'hello world! love, ``this``'\r\n\r\n\r\n--optional (see https://docs.microsoft.com/en-us/sql/t-sql/language-elements/return-transact-sql?view=sql-server-ver15)\r\n--set @Squealer_ReturnValue = [ integer_expression ]",
                    EType.ScalarFunction => "set @Result = 'hello world! love, ``this``'",
                    EType.InlineTableFunction => "select 'hello world! love, ``this``' as [MyColumn]",
                    EType.MultiStatementTableFunction => "insert @TableValue select 'hello world! love, ``this``'",
                    EType.View => "select 'hello world! love, ``this``' as hello",
                    _ => throw new InvalidOperationException()
                };

                placeHolder_code = $"\n\n" +
                    $"/***********************************************************************\n" +
                    $"\tComments.\n" +
                    $"***********************************************************************/\n" +
                    $"\n{selectPlaceHolder}\n\n";

            }

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
                if (Type == EType.StoredProcedure || Type == EType.ScalarFunction || Type == EType.MultiStatementTableFunction)
                {
                    writer.WriteStartElement("Parameters");
                    if (IsNewFile)
                    {
                        var outputString = Type == EType.StoredProcedure ? "Output=\"False\" " : "";
                        writer.WriteComment($"<Parameter Name=\"MyParameter\" Type=\"varchar(50)\" {outputString}DefaultValue=\"\" Comments=\"\"/>");
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
                    writer.WriteComment(" Define the column(s) to return from this view. ");

                    // Table
                    writer.WriteStartElement("Table");
                    if (Type == EType.MultiStatementTableFunction)
                    {
                        writer.WriteAttributeString("PrimaryKeyClustered", Table.PrimaryKeyClustered);
                    }
                    if (IsNewFile)
                    {
                        var tableComment = Type switch
                        {
                            EType.View => "<Column Name=\"MyColumn\" Comments=\"\" />",
                            EType.MultiStatementTableFunction => "<!--<Column Name=\"MyColumn\" Type=\"varchar(50)\" Nullable=\"False\" Identity=\"False\" IncludeInPrimaryKey=\"False\" Comments=\"\" />-->",
                            _ => ""
                        };
                        // Write the Column element as a comment.
                        writer.WriteComment(tableComment);

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
                writer.WriteCData(placeHolder_code ?? Code);
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
            get => Output.ToString().ToLower();
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
            get => ReadOnly.ToString().ToLower();
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

        [XmlIgnore]
        public bool RunLog { get; set; }

        [XmlAttribute("RunLog")]
        public string RunLogString
        {
            get => RunLog.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    RunLog = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for Parameter => RunLog. Expected 'true' or 'false'.");
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
            get => Nullable.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    Nullable = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for Parameter => RunLog. Expected 'true' or 'false'.");
                }
            }
        }

        [XmlIgnore]
        public bool Identity { get; set; }

        [XmlAttribute("Identity")]
        public string IdentityString
        {
            get => Identity.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    Identity = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for Parameter => RunLog. Expected 'true' or 'false'.");
                }
            }
        }

        [XmlIgnore]
        public bool IncludeInPrimaryKey { get; set; }

        [XmlAttribute("IncludeInPrimaryKey")]
        public string IncludeInPrimaryKeyString
        {
            get => IncludeInPrimaryKey.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                {
                    IncludeInPrimaryKey = result;
                }
                else
                {
                    throw new FormatException($"The value '{value}' is not a valid Boolean value for Parameter => RunLog. Expected 'true' or 'false'.");
                }
            }
        }

        [XmlAttribute("Comments")]
        public string Comments { get; set; } = default!;
    }
    public class Returns
    {
        [XmlAttribute("Type")]
        public string Type { get; set; } = "varchar(100)";
    }

}