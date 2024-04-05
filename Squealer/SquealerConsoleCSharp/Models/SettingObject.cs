using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SquealerConsoleCSharp.Models
{
    [XmlRoot("Settings")]
    public class SettingObject
    {
        [XmlAttribute("ProjectName")]
        public string ProjectName { get; set; } = string.Empty;

        [XmlArray("DefaultUsers")]
        [XmlArrayItem("User")]
        public List<User> DefaultUsers { get; set; } = new List<User>();

        [XmlArray("StringReplacements")]
        [XmlArrayItem("String")]
        public List<StringReplacement> StringReplacements { get; set; } = new List<StringReplacement>();
    }


    public class StringReplacement
    {
        [XmlAttribute("Original")]
        public string Original { get; set; } = string.Empty;

        [XmlAttribute("Replacement")]
        public string Replacement { get; set; } = string.Empty;

        [XmlAttribute("Comment")]
        public string Comment { get; set; } = string.Empty;
    }
}
