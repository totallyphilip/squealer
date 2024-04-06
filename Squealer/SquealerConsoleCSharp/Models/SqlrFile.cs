using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Models
{
    public class SqlrFile
    {
        public string FileName { get => Path.GetFileName(FilePath) };

        public string FilePath { get; }

        public string Schema { get => FileName.Split(".")[0]; }

        public string RootProgramName { get => FileName.Split(".")[1]; }

        public SqlrFile(string filePath)
        {
            FilePath = filePath;
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException();
            }
        }
    }
}
