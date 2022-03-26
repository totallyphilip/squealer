Public Class VersionCheck

    Private Const VersionInfoUrl As String = "https://s3-us-west-1.amazonaws.com/public-10ec013b-b521-4150-9eab-56e1e1bb63a4/Squealer/version.xml"
    Private Const DownloadFileUrl As String = "https://s3-us-west-1.amazonaws.com/public-10ec013b-b521-4150-9eab-56e1e1bb63a4/Squealer/Squealer{0}-Install.zip"

    Private Class Metadata

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

        Public Sub New()
        End Sub

        Public Sub New(ver As Version, updated As DateTime, hidden As Boolean, about As String)
            Me._Version = ver
            Me._Updated = updated
            Me._Hidden = hidden
            Me._About = about
            Me._ZipFile = String.Format(DownloadFileUrl, _Version.ToString.Replace(".", ""))
        End Sub

    End Class

#Region " Output "

    Public Sub CreateMetadata()
        Dim info As New Metadata(My.Application.Info.Version, DateTime.UtcNow, False, My.Resources.WhatsNew)
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

#End Region

#Region " Input "

    Public Sub DisplayVersionCheckResults()

        Dim m As Metadata = DownloadMetadata()
        If m.IsUpdateAvailable Then
            Textify.SayBulletLine(Textify.eBullet.Hash, "A new version is available.", New Textify.ColorScheme(ConsoleColor.Yellow))
            Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("Squealer {0} released on {1}.", m.Version, m.Updated.ToShortDateString))
            Console.WriteLine()
            Console.WriteLine(m.About)
            Console.WriteLine()
            Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("Get the latest version at {0}", Constants.HomePage))
        Else
            Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("You have the latest version of Squealer ({0}).", My.Application.Info.Version), New Textify.ColorScheme(ConsoleColor.White))
        End If

    End Sub

    Private Function DownloadMetadata() As Metadata

        Dim d As New Metadata()
        Dim client As New Net.WebClient
        Dim x As New Xml.XmlDocument
        Dim r As New IO.StreamReader(client.OpenRead(VersionInfoUrl))
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

#End Region

End Class
