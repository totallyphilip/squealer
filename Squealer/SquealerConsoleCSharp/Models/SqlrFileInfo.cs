using Spectre.Console;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SquealerConsoleCSharp.Models
{
    public class SqlrFileInfo
    {
        private SquealerObject? _squealerObject;

        public SquealerObject SquealerObject { get
            {
                if (_squealerObject == null)
                {
                    try
                    {
                        string xmlContent = File.ReadAllText(FilePath);

                        using (StringReader reader = new StringReader(xmlContent))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(SquealerObject));

                            var x = serializer.Deserialize(reader);

                            _squealerObject = (SquealerObject)serializer.Deserialize(reader);

                            return _squealerObject;
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupInterpolated($"{FileName} is not valid xml");
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
                else
                {
                    return _squealerObject;
                }
            }
        }

        public string FileName { get => Path.GetFileName(FilePath); }

        public string FilePath { get; }

        public string Schema { get => FileName.Split(".")[0]; }

        public string RootProgramName { get => FileName.Split(".")[1]; }

        public string SqlObjectName { get => Path.GetFileNameWithoutExtension(FileName);  }

        public SqlrFileInfo(string filePath)
        {
            FilePath = filePath;
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException();
            }

            if(Path.GetExtension(filePath) != ".sqlr")
            {
                throw new InvalidOperationException($"{FileName} is not a sqlr file.");
            }

            if(FileName.Split(".").Length != 3)
            {
                throw new InvalidOperationException($"The file name '{FileName}' is invalid.\nFile names must follow the format: [Schema].[ObjectName].sqlr, for example, 'dbo.TableExample.sqlr'. Please rename your file accordingly.\n");
            }

            

            //try
            //{
            //    using (StringReader reader = new StringReader(xmlContent))
            //    {
            //        XmlSerializer serializer = new XmlSerializer(typeof(SquealerObject));

            //        var x = serializer.Deserialize(reader);

            //        SquealerObject = (SquealerObject)serializer.Deserialize(reader);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    AnsiConsole.MarkupInterpolated($"{FileName} is not valid xml");
            //    Console.WriteLine(ex.Message);
            //    throw;
            //}
        }
    }
}
