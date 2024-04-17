using SquealerConsoleCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp
{
    internal class AppState
    {
        private static AppState? instance;
        public static AppState Instance => instance ??= new AppState();

        public string LastOpenedPath { get; set; } = string.Empty;

        public string GitPprojectName { get; set; } = string.Empty;

        public Settings Settings { get; set; } = default!;

        // Private constructor to prevent instantiation outside
        private AppState() { }
    }
}
