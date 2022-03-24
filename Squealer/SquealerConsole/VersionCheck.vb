Public Class VersionCheck

    Dim _Hidden As Boolean = True
    Dim _Version As String = My.Application.Info.Version.ToString
    Public Sub XmlTest()

        Dim f As New TempFileHandler(".xml")
        CurrentVersionXml().Save(f.Filename)

        f.Show()

    End Sub

    Private Function CurrentVersionXml() As Xml.XmlDocument

        Dim OutputXml As New Xml.XmlDocument
        OutputXml.AppendChild(OutputXml.CreateXmlDeclaration("1.0", "us-ascii", Nothing))
        Dim OutRoot As Xml.XmlElement = OutputXml.CreateElement(My.Application.Info.ProductName)
        OutputXml.AppendChild(OutRoot)
        OutRoot.SetAttribute("Version", Me._Version)
        OutRoot.SetAttribute("Updated", DateTime.UtcNow.ToString)
        OutRoot.SetAttribute("Hidden", Me._Hidden.ToString)
        CurrentVersionXml = OutputXml

    End Function

    Public Sub Check()

    End Sub

End Class
