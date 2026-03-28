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
        public WildcardSettings Wildcards { get; set; } = new WildcardSettings();
        public EditorSettings Editor { get; set; } = new EditorSettings();

        [XmlAttribute("Version")]
        public string Version { get; set; } = "1.0";

        [XmlAttribute("ShowGitBranch")]
        public bool ShowGitBranch { get; set; } = true;

        [XmlAttribute("KeepScreenAlive")]
        public bool KeepScreenAlive { get; set; } = false;

        [XmlAttribute("AlwaysShowSymbols")]
        public bool AlwaysShowSymbols { get; set; } = true;

        [XmlAttribute("AutoCompressGit")]
        public bool AutoCompressGit { get; set; } = false;

        [XmlAttribute("TrackFailedItems")]
        public bool TrackFailedItems { get; set; } = true;

        [XmlAttribute("DetectDeprecatedSquealerObjects")]
        public bool DetectDeprecatedSquealerObjects { get; set; } = true;

        [XmlAttribute("AutoEditNewFiles")]
        public bool AutoEditNewFiles { get; set; } = true;

        [XmlAttribute("LastProjectFolder")]
        public string LastProjectFolder { get; set; } = string.Empty;

        [XmlAttribute("RecentProjectFolders")]
        public string RecentProjectFolders { get; set; } = string.Empty;

        [XmlAttribute("EnableEzObjects")]
        public bool EnableEzObjects { get; set; } = false;

        [XmlAttribute("EzSchema")]
        public string EzSchema { get; set; } = "ez";

        [XmlAttribute("LastVersionCheckDate")]
        public string LastVersionCheckDateStr { get; set; } = string.Empty;

        [XmlIgnore]
        public DateTime LastVersionCheckDate
        {
            get => DateTime.TryParse(LastVersionCheckDateStr, out var d) ? d : DateTime.MinValue;
            set => LastVersionCheckDateStr = value.ToString();
        }

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

    public class WildcardSettings
    {
        [XmlAttribute("UseEdges")]
        public bool UseEdges { get; set; } = false;

        [XmlAttribute("UseSpaces")]
        public bool UseSpaces { get; set; } = false;
    }

    public class EditorSettings
    {
        [XmlAttribute("Path")]
        public string Path { get; set; } = "notepad.exe";
    }

}
