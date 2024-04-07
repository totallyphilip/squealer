using Microsoft.VisualBasic.FileIO;
using SquealerConsoleCSharp;
using SquealerConsoleCSharp.Models;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SquealerConsoleCSharp.Extensions;


namespace SquealerConsoleCSharp.Models
{
    #region Squealer
    [XmlRoot("Squealer")]
    public class SquealerObject
    {
        [XmlIgnore]
        public EType Type { get; set; } = default;

        [XmlAttribute("Type")]
        public string TypeString
        {
            get => Helper.GetEnumDescription(Type);
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
        public string PreCode { get; set; } = default!;

        [XmlElement("Comments")]
        public string Comments { get; set; } = default!;

        [XmlArray("Parameters")]
        [XmlArrayItem("Parameter")]
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        [XmlElement("Code")]
        public string Code { get; set; } = default!;

        [XmlArray("Users")]
        [XmlArrayItem("User")]
        public List<User> Users { get; set; } = new List<User>();

        [XmlElement("PostCode")]
        public string PostCode { get; set; } = default!;


        public string GetNumericaSymbolString() 
        {
            string res = Type switch
            {
                EType.StoredProcedure => "",
                EType.ScalarFunction or EType.InlineTableFunction or EType.MultiStatementTableFunction => "[green3]()[/]",
                EType.View => "[green3]*[/]",
                _ => throw new InvalidOperationException()
            };
            return res;
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

}