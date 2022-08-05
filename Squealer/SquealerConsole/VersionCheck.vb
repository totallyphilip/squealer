Public Class VersionCheck

    Private Const VersionInfoFilename As String = "version.xml"
    Private Const DownloadFilename As String = "Squealer{0}-Install.zip"

    Private Class Metadata

#Region " Properties "

        Private _Version As Version
        Public Property Version As Version
            Get
                Return _Version
            End Get
            Set(value As Version)
                _Version = value
            End Set
        End Property

        Public ReadOnly Property IsUpdateAvailable As Boolean
            Get
                Return My.Application.Info.Version.CompareTo(_Version) < 0
            End Get
        End Property

        Private _Updated As DateTime
        Public Property Updated As DateTime
            Get
                Return _Updated
            End Get
            Set(value As DateTime)
                _Updated = value
            End Set
        End Property

        Private _Hidden As Boolean
        Public Property Hidden As Boolean
            Get
                Return _Hidden
            End Get
            Set(value As Boolean)
                _Hidden = value
            End Set
        End Property

        Private _About As String
        Public Property About As String
            Get
                Return _About
            End Get
            Set(value As String)
                _About = value
            End Set
        End Property

        Private _ZipFile As String
        Public Property ZipFile As String
            Get
                Return _ZipFile
            End Get
            Set(value As String)
                _ZipFile = value
            End Set
        End Property

#End Region

        Public Sub New()
        End Sub

        Public Sub New(ver As Version, updated As DateTime, hidden As Boolean, about As String, sourceurl As String)
            Me._Version = ver
            Me._Updated = updated
            Me._Hidden = hidden
            Me._About = about
            Me._ZipFile = String.Format(sourceurl & DownloadFilename, _Version.ToString.Replace(".", ""), sourceurl)
        End Sub

    End Class

#Region " Output "

    Public Sub CreateMetadata(sourceurl As String)
        Dim info As New Metadata(My.Application.Info.Version, DateTime.UtcNow, False, WhatsNew, sourceurl)
        Dim f As New TempFileHandler(".xml")
        Generate(info).Save(f.Filename)
        f.Show()
    End Sub

    Private Function Generate(ByRef info As Metadata) As Xml.XmlDocument

        Dim doc As New Xml.XmlDocument
        doc.AppendChild(doc.CreateXmlDeclaration("1.0", "us-ascii", Nothing))
        Dim squealer As Xml.XmlElement = doc.CreateElement("Squealer")
        doc.AppendChild(squealer)
        squealer.SetAttribute(NameOf(info.Version), info.Version.ToString)
        squealer.SetAttribute(NameOf(info.Updated), info.Updated.ToString)
        squealer.SetAttribute(NameOf(info.Hidden), info.Hidden.ToString)
        squealer.SetAttribute(NameOf(info.ZipFile), info.ZipFile)
        Dim about As Xml.XmlElement = doc.CreateElement(NameOf(info.About))
        squealer.AppendChild(about)
        Dim abouttext As Xml.XmlCDataSection = doc.CreateCDataSection("")
        about.AppendChild(abouttext)
        abouttext.InnerText = info.About
        Generate = doc

    End Function

    Public Shared Function WhatsNew() As String
        Return My.Resources.ChangeLog.Remove(My.Resources.ChangeLog.IndexOf("^break")).Replace(My.Application.Info.Version.ToString, String.Empty).Trim
    End Function
    Public Shared Function ChangeLog() As String
        Return My.Resources.ChangeLog.Replace("^break", String.Empty).Trim
    End Function

#End Region

#Region " Input "

    Public Sub DisplayVersionCheckResults(sourceurl As String, ismediadefaultlocation As Boolean)

        Dim m As Metadata = DownloadMetadata(sourceurl)
        If m.IsUpdateAvailable Then
            Textify.SayBulletLine(Textify.eBullet.Hash, "A new version is available.", New Textify.ColorScheme(ConsoleColor.Yellow))
            Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("Squealer {0} released on {1}.", m.Version, m.Updated.ToShortDateString))
            Console.WriteLine()
            Console.WriteLine(m.About)
            Console.WriteLine()
            Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("Get the latest version at {0}", IIf(ismediadefaultlocation, Constants.HomePage, m.ZipFile)))
        Else
            Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("You have the latest version of Squealer ({0}).", My.Application.Info.Version), New Textify.ColorScheme(ConsoleColor.White))
        End If

    End Sub

    Private Function DownloadMetadata(sourceurl As String) As Metadata

        Dim d As New Metadata()
        Dim client As New Net.WebClient
        Dim x As New Xml.XmlDocument
        Dim r As New IO.StreamReader(client.OpenRead(sourceurl & VersionInfoFilename))
        x.LoadXml(r.ReadToEnd)

        Try
            Dim Node As Xml.XmlNode = x.SelectSingleNode("/Squealer")
            d.Version = Version.Parse(Node.Attributes(NameOf(d.Version)).Value)
            d.Updated = DateTime.Parse(Node.Attributes(NameOf(d.Updated)).Value)
            d.Hidden = Boolean.Parse(Node.Attributes(NameOf(d.Hidden)).Value)
            d.ZipFile = Node.Attributes(NameOf(d.ZipFile)).Value.ToString
            Node = x.SelectSingleNode("/Squealer/About")
            d.About = x.InnerText
            Return d
        Catch ex As Exception
            Return Nothing
        End Try

        '<?xml version="1.0" encoding="us-ascii"?>
        '<Squealer Version = "1.2.3.45" Updated="mm/dd/yyyy 12:00:00 AM" Hidden="False" Filename="https://s3_xxx/Squealerabcde12345-Install.zip">
        '  <About><![CDATA[Release notes]]></About>
        '</Squealer>

    End Function

    Public Sub DownloadLatestInstaller(sourceurl As String)

        Dim d As Metadata = DownloadMetadata(sourceurl)
        Dim dest As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\" & System.IO.Path.GetFileName(d.ZipFile)
        Textify.Write("Downloading ", New Textify.ColorScheme(ConsoleColor.Green))
        Textify.Write(d.ZipFile, New Textify.ColorScheme(ConsoleColor.DarkYellow))
        Textify.Write(" to ", New Textify.ColorScheme(ConsoleColor.Green))
        Textify.WriteLine(dest, New Textify.ColorScheme(ConsoleColor.White))
        Console.WriteLine()

        Using client As New System.Net.WebClient()
            client.DownloadFile(d.ZipFile, dest)
        End Using

    End Sub

    Public Sub DownloadLatestEzBinary(sourceurl As String, destfile As String)
        ' silently get the latest script
        Try
            Using client As New System.Net.WebClient()
                client.DownloadFile(sourceurl, destfile)
            End Using
        Catch ex As Exception
        End Try
    End Sub

#End Region

End Class
