Public Class VersionCheck

    Private Const VersionInfoUrl As String = "https://s3-us-west-1.amazonaws.com/public-10ec013b-b521-4150-9eab-56e1e1bb63a4/Squealer/version.xml"
    Private Const DownloadFileUrl As String = "https://s3-us-west-1.amazonaws.com/public-10ec013b-b521-4150-9eab-56e1e1bb63a4/Squealer/Squealer{0}-Install.zip"

    Private Class Metadata

        Private _Version As String
        Public ReadOnly Property Version As String
            Get
                Return _Version
            End Get
        End Property

        Private _Updated As DateTime
        Public ReadOnly Property Updated As DateTime
            Get
                Return _Updated
            End Get
        End Property

        Private _Hidden As Boolean
        Public ReadOnly Property Hidden As Boolean
            Get
                Return _Hidden
            End Get
        End Property

        Private _About As String
        Public ReadOnly Property About As String
            Get
                Return _About
            End Get
        End Property

        Private _ZipFile As String
        Public ReadOnly Property ZipFile As String
            Get
                Return _ZipFile
            End Get
        End Property

        Public Sub New(ver As String, updated As DateTime, hidden As Boolean, about As String)
            Me._Version = ver
            Me._Updated = updated
            Me._Hidden = hidden
            Me._About = about
            Me._ZipFile = String.Format(DownloadFileUrl, _Version.Replace(".", ""))
        End Sub

    End Class

#Region " Output "

    Public Sub CreateMetadata()
        Dim info As New Metadata(My.Application.Info.Version.ToString, DateTime.UtcNow, False, My.Resources.WhatsNew)
        Dim f As New TempFileHandler(".xml")
        Generate(info).Save(f.Filename)
        f.Show()
    End Sub

    Private Function Generate(ByRef info As Metadata) As Xml.XmlDocument

        Dim doc As New Xml.XmlDocument
        doc.AppendChild(doc.CreateXmlDeclaration("1.0", "us-ascii", Nothing))
        Dim squealer As Xml.XmlElement = doc.CreateElement("Squealer")
        doc.AppendChild(squealer)
        squealer.SetAttribute("Version", info.Version)
        squealer.SetAttribute("Updated", info.Updated.ToString)
        squealer.SetAttribute("Hidden", info.Hidden.ToString)
        squealer.SetAttribute("Filename", info.ZipFile)
        Dim about As Xml.XmlElement = doc.CreateElement("About")
        squealer.AppendChild(about)
        Dim abouttext As Xml.XmlCDataSection = doc.CreateCDataSection("")
        about.AppendChild(abouttext)
        abouttext.InnerText = info.About
        Generate = doc

    End Function

#End Region

#Region " Input "

    Public Sub Check()

        Try
            Throw New Exception("nope")
        Catch ex As Exception
            Textify.SayError(ex.Message)
            Textify.SayBulletLine(Textify.eBullet.Star, "Visit www.asciimotive.com to obtain the latest version of Squealer.")
            Console.WriteLine()
        End Try

    End Sub

#End Region

End Class
