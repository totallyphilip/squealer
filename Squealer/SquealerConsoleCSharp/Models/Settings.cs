using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SquealerConsoleCSharp.Models
{
    [XmlRoot("Settings")]
    public class Settings
    {
        public OutputSettings Output { get; set; } = new OutputSettings();
        public PromptSettings Prompt { get; set; } = new PromptSettings();

        public string Version { get; set; } = "1.0";

        public static Settings GetNewSettings()
        {
            return new Settings();
        }

        public static void SaveSettings(Settings settings, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, settings);
            }
        }

        public static Settings LoadSettings(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (Settings)serializer.Deserialize(reader);
            }
        }


    }

    public class OutputSettings
    {
        [XmlAttribute("CopyToClipboard")]
        public bool CopyToClipboard { get; set; } = true;

        [XmlAttribute("ExportToSqlFile")]
        public bool ExportToSqlFile { get; set; }
    }

    public class PromptSettings
    {
        public ProjectName ProjectName { get; set; } = new ProjectName();
        public BranchName BranchName { get; set; } = new BranchName();
    }

    public class ProjectName
    {
        [XmlAttribute("Show")]
        public bool Show { get; set; } = true;

        [XmlIgnore]
        public int? MaxLength { get; set; } = null;

        [XmlAttribute("MaxLength")]
        public string OutputString
        {
            get
            {
                if (MaxLength != null)
                    return MaxLength.ToString();
                return string.Empty;
            }
            set
            {
                if (int.TryParse(value, out int result))
                {
                    MaxLength = result;
                }
                else
                {
                    MaxLength = null;
                }
            }
        }
    }

    public class BranchName
    {
        [XmlAttribute("Show")]
        public bool Show { get; set; } = true;

        [XmlIgnore]
        public int? MaxLength { get; set; } = null;

        [XmlAttribute("MaxLength")]
        public string OutputString
        {
            get
            {
                if (MaxLength != null)
                    return MaxLength.ToString();
                return string.Empty;
            }
            set
            {
                if (int.TryParse(value, out int result))
                {
                    MaxLength = result;
                }
                else
                {
                    MaxLength = null;
                }
            }
        }
    }

}
