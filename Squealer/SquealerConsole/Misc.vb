Module Misc

    Public Function WildcardInterpreter(s As String, spaces As Boolean, edges As Boolean, FindExact As Boolean) As String
        WildcardInterpreter = WildcardInterpreter(s, New Settings.WildcardBehaviorClass With {.UseEdges = edges, .UseSpaces = spaces}, FindExact)
    End Function

    Public Function WildcardInterpreter(s As String, w As Settings.WildcardBehaviorClass, FindExact As Boolean) As String

        If w.UseSpaces Then
            s = s.Replace(" "c, "*"c)
        End If

        If String.IsNullOrWhiteSpace(s) Then
            s = "*"
        ElseIf w.UseEdges AndAlso Not FindExact Then
            s = "*" & s & "*"
        End If
        While s.Contains("**")
            s = s.Replace("**", "*")
        End While
        Return s & Constants.SquealerFileExtension

    End Function

    Public Function IsStarWarsDay() As Boolean

        'todo: turn star wars day back on
        If Now.Month = 5 AndAlso Now.Day = 4 AndAlso False Then ' added false to hide this for now
            Return True
        Else
            Return False
        End If

    End Function

    Public Function TitleText(showproject As Boolean, showfolder As Boolean, workingfolder As String) As String

        Dim title As String = ""
        If showproject Then
            title &= "[" & ProjectName(workingfolder) & "]"
        End If
        If showfolder Then
            title &= " " & workingfolder
        End If
        If Not title = "" Then
            title &= " |"
        End If
        title &= " " & My.Application.Info.Title

        Return title.Trim

    End Function

    Public Function ProjectName(ByVal WorkingFolder As String) As String

        Try
            Dim Reader As New Xml.XmlDocument
            Reader.Load(WorkingFolder & "\" & Constants.ConfigFilename)
            Dim Node As Xml.XmlNode = Reader.SelectSingleNode("/Settings")
            Dim s As String = Node.Attributes("ProjectName").Value.ToString.Trim()
            If String.IsNullOrWhiteSpace(s) Then
                Throw New SystemException("nope")
                s = My.Computer.FileSystem.GetDirectoryInfo(WorkingFolder).Name
            End If
            If s.Length > 30 Then
                s = s.Substring(0, 30)
            End If
            Return s
        Catch ex As Exception
            Return My.Computer.FileSystem.GetDirectoryInfo(WorkingFolder).Name
        End Try

    End Function

    Public Function EncryptedBytes(s As String) As Byte()

        Dim entropy As Byte() = {1, 9, 1, 1, 4, 5}
        Dim csbytes As Byte() = System.Text.Encoding.Unicode.GetBytes(s.Trim)

        Return System.Security.Cryptography.ProtectedData.Protect(csbytes, entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)

    End Function

    Public Function DecryptedString(b As Byte()) As String

        Dim entropy As Byte() = {1, 9, 1, 1, 4, 5}
        Dim decrypted As Byte() = System.Security.Cryptography.ProtectedData.Unprotect(b, entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)

        Return System.Text.Encoding.Unicode.GetString(decrypted)

    End Function

End Module
