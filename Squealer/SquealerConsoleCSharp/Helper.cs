using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static string? GetFilePath(string searchText)
        {
            if (CheckFolderValid())
            {
                var filePath = Path.Combine(AppState.Instance.LastOpenedPath, searchText);
                return filePath;
            }
            else
            {
                return null;
            }
        }
    }
}
