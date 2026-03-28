using Spectre.Console;
using System.Reflection;
using System.Xml;

namespace SquealerConsoleCSharp
{
    internal class VersionCheck
    {
        private const string VersionInfoFilename = "version.xml";

        private class Metadata
        {
            public Version? Version { get; set; }
            public DateTime Updated { get; set; }
            public bool Hidden { get; set; }
            public string About { get; set; } = string.Empty;
            public string ZipFile { get; set; } = string.Empty;

            public bool IsUpdateAvailable
            {
                get
                {
                    var current = Assembly.GetEntryAssembly()?.GetName().Version;
                    return current != null && Version != null && current.CompareTo(Version) < 0;
                }
            }
        }

        public void DisplayVersionCheckResults(string sourceUrl)
        {
            var m = DownloadMetadata(sourceUrl);
            if (m.Version == null) return;

            if (m.IsUpdateAvailable)
            {
                AnsiConsole.MarkupLine("[yellow]# A new version is available.[/]");
                AnsiConsole.MarkupLine($"# Squealer {m.Version} released on {m.Updated.ToShortDateString()}.");
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine(m.About);
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"# Get the latest version at {Constants.HomePage}");
            }
            else
            {
                var ver = Assembly.GetEntryAssembly()?.GetName().Version;
                AnsiConsole.MarkupLine($"[white]# You have the latest version of Squealer ({ver}).[/]");
            }
        }

        public void DownloadLatestInstaller(string sourceUrl)
        {
            var m = DownloadMetadata(sourceUrl);
            if (string.IsNullOrEmpty(m.ZipFile)) return;

            var dest = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Path.GetFileName(m.ZipFile));

            AnsiConsole.MarkupLine($"[green]Downloading[/] [darkyellow]{m.ZipFile}[/] [green]to[/] [white]{dest}[/]");
            AnsiConsole.WriteLine();

            using var client = new HttpClient();
            var data = client.GetByteArrayAsync(m.ZipFile).GetAwaiter().GetResult();
            File.WriteAllBytes(dest, data);
        }

        private static Metadata DownloadMetadata(string sourceUrl)
        {
            var m = new Metadata();
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                var xml = client.GetStringAsync(sourceUrl + VersionInfoFilename).GetAwaiter().GetResult();

                var doc = new XmlDocument();
                doc.LoadXml(xml);
                var node = doc.SelectSingleNode("/Squealer");
                if (node?.Attributes == null) return m;

                m.Version = Version.Parse(node.Attributes["Version"]!.Value);
                m.Updated = DateTime.Parse(node.Attributes["Updated"]!.Value);
                m.Hidden = bool.Parse(node.Attributes["Hidden"]!.Value);
                m.ZipFile = node.Attributes["ZipFile"]?.Value ?? string.Empty;
                m.About = node.SelectSingleNode("About")?.InnerText ?? string.Empty;
            }
            catch
            {
                AnsiConsole.MarkupLine("[red]Could not check for updates. Check your connection and try again.[/]");
            }
            return m;
        }
    }
}
