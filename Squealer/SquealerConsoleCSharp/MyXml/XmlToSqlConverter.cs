using Spectre.Console;
using SquealerConsoleCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SquealerConsoleCSharp.MyXml
{
    internal class XmlToSqlConverter
    {

        public SquealerObject SquealerObject { get;}

        public SqlrFileInfo SqlrFileInfo { get;}

        public XmlToSqlConverter(string filePath)
        {
            try
            {
                SqlrFileInfo = new SqlrFileInfo(filePath);

                string xmlContent = File.ReadAllText(SqlrFileInfo.FilePath);

                using (StringReader reader = new StringReader(xmlContent))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SquealerObject));
                    SquealerObject = (SquealerObject)serializer.Deserialize(reader);

                }
            }
            catch (Exception ex) 
            {
                AnsiConsole.MarkupLine($"[yellow]Error on {filePath}[/]");
                throw;
            }
        }

    }
}
