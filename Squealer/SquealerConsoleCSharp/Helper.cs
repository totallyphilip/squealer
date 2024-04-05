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

        public static void HandlePatterns(string patterns)
        {
            var patternList = patterns.Split('|');
            var baseDirectory = "path/to/xml/files"; // Update this path to your XML files directory

            foreach (var pattern in patternList)
            {
                // Convert wildcard to regex pattern (e.g., *abc* -> .*abc.*)
                var regexPattern = WildcardToRegex(pattern);

                var matchingFiles = Directory.GetFiles(baseDirectory, "*.xml", SearchOption.TopDirectoryOnly)
                    .Where(filePath => Regex.IsMatch(Path.GetFileName(filePath), regexPattern, RegexOptions.IgnoreCase));

                foreach (var file in matchingFiles)
                {
                    // Here, process each file to generate SQL script
                    Console.WriteLine($"Processing {file}");
                    // Add your logic to parse the XML and generate SQL script here
                }
            }
        }

        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$";
        }

    }
}
