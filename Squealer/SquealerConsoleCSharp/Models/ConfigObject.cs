using System.Collections.Generic;
using System.Xml.Serialization;


namespace SquealerConsoleCSharp.Models
{

    [XmlRoot(ElementName = "Settings")]
    internal class ConfigObject
    {
        [XmlAttribute(AttributeName = "ProjectName")]
        public string ProjectName { get; set; } = string.Empty;

        // Directly declare the Users array within ConfigObject
        [XmlArray("DefaultUsers")]
        [XmlArrayItem("User")]
        public List<User> Users { get; set; } = new List<User>();

        // Directly declare the StringReplacements array within ConfigObject
        [XmlArray("StringReplacements")]
        [XmlArrayItem("String")]
        public List<StringReplacement> StringReplacements { get; set; } = new List<StringReplacement>();

        public static ConfigObject GetConfig()
        {
            var filePath = Helper.GetFilePath("Squealer.config");
            if (!Path.Exists(filePath))
            {
                throw new InvalidOperationException("Cannot find Squealer.config");
            }

            string xmlContent = File.ReadAllText(filePath);

            using (StringReader reader = new StringReader(xmlContent))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigObject));
                var res = (ConfigObject)serializer.Deserialize(reader);

                return res;
            }
        }


    }

    public class StringReplacement
    {
        [XmlAttribute(AttributeName = "Original")]
        public string Original { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "Replacement")]
        public string Replacement { get; set; } = string.Empty;

        // If a comment is optional and might not always be provided, initializing it as string.Empty is useful.
        [XmlAttribute(AttributeName = "Comment")]
        public string Comment { get; set; } = string.Empty;
    }

}
