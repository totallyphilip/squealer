namespace SquealerConsoleCSharp
{
    internal static class Logging
    {
        private static readonly string AppDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Squealer");

        private static readonly string DefaultLogFile = Path.Combine(AppDataFolder, "application.log");

        public static void WriteLog(string message) => WriteLog(DefaultLogFile, message);

        public static void WriteLog(string filename, string message)
        {
            try
            {
                SplitLogOver10MB(filename);
                Directory.CreateDirectory(Path.GetDirectoryName(filename)!);
                File.AppendAllText(filename, $"[{DateTime.Now}] {message}{Environment.NewLine}");
            }
            catch
            {
                // Suppress logging errors — don't crash the app over a log write failure.
            }
        }

        private static void SplitLogOver10MB(string filename)
        {
            if (!File.Exists(filename)) return;
            var info = new FileInfo(filename);
            if (info.Length <= 10_000_000) return;

            var rotated = Path.Combine(
                info.DirectoryName!,
                Path.GetFileNameWithoutExtension(filename) +
                $"-{DateTime.Now:yyyy-MM-dd}" +
                info.Extension);

            if (!File.Exists(rotated))
                File.Move(filename, rotated);
        }
    }
}
