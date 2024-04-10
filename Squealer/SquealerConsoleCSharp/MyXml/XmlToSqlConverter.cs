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
            SqlrFileInfo = new SqlrFileInfo(filePath);

            string xmlContent = File.ReadAllText(SqlrFileInfo.FilePath);

            using (StringReader reader = new StringReader(xmlContent))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SquealerObject));
                SquealerObject = (SquealerObject)serializer.Deserialize(reader);

            }
        }


        public string GetSqlScript() 
        {
            string create = SquealerObject.Type switch
            {
                EType.StoredProcedure => MyResources.Resources.P_Create,
                EType.ScalarFunction or EType.InlineTableFunction or EType.MultiStatementTableFunction => MyResources.Resources.FN_Create,
                EType.View => MyResources.Resources.V_Create,
                _ => throw new InvalidOperationException()
            };

            create = create.Replace("{Schema}", SqlrFileInfo.Schema);
            create = create.Replace("{RootProgramName}", SqlrFileInfo.RootProgramName);
            return create;
        }
    }
}
