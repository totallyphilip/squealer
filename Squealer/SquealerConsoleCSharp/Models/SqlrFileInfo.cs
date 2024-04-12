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
    internal class SqlrFileInfo
    {
        public string FileName { get => Path.GetFileName(FilePath); }

        public string FilePath { get; }

        public string Schema { get => FileName.Split(".")[0]; }

        public string RootProgramName { get => FileName.Split(".")[1]; }

        public string SqlObjectName_w_Bracket { get => $"[{Schema}].[{RootProgramName}]"; }

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

            if (FileName.Split(".").Length != 3)
            {
                throw new InvalidOperationException($"The file name '{FileName}' is invalid.\nFile names must follow the format: [Schema].[ObjectName].sqlr, for example, 'dbo.TableExample.sqlr'. Please rename your file accordingly.\n");
            }
        }

        public static bool ValidateSqlrFileName(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                AnsiConsole.MarkupLine("[red]<filename> not provided.");
                return false;
            }

            if (!Helper.IsValidFilename(filename))
            {
                AnsiConsole.MarkupLine("[red]<filename> not valid.");
                return false;
            }

            if (filename.Split(".").Length != 3)
            {
                AnsiConsole.MarkupLine("The file name '{FileName}' is invalid.\nFile names must follow the format: [Schema].[ObjectName].sqlr, for example, 'dbo.TableExample.sqlr'. Please rename your file accordingly.");
                return false;
            }

            return true;
        }
    }
}
