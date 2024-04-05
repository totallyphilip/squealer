using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Models
{
    public class SqlrFile
    {
        public string FileName { get; }

        public string FilePath { get => Helper.GetFilePath(FileName); }

        public string Schema { get => FileName.Split(".")[0]; }

        public string RootProgramName { get => FileName.Split(".")[1]; }

        public SqlrFile(string fileName)
        {
            FileName = fileName;
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException();
            }
        }
    }
}
