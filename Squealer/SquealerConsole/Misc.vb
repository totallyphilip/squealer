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

    Public Function IsTodayStarWarsDay() As Boolean

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

    Public Enum eEncryptionMode
        Stupid
        Smart
    End Enum

    Private StupidCryptOffset As Integer = 77

    Public Function EncryptedBytes(s As String, mode As eEncryptionMode) As Byte()

        Dim csbytes As Byte() = System.Text.Encoding.Unicode.GetBytes(s.Trim)

        If mode = eEncryptionMode.Smart Then
            Dim entropy As Byte() = {1, 9, 1, 1, 4, 5}

            Return System.Security.Cryptography.ProtectedData.Protect(csbytes, entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)

        Else

            ' This just does a dumb value shift to obscure the original text

            For i As Integer = 0 To csbytes.Length - 1
                Dim newbyte As Integer = csbytes(i)
                newbyte += StupidCryptOffset
                If newbyte > 255 Then
                    newbyte -= 256
                End If
                csbytes(i) = Convert.ToByte(newbyte)
            Next

            Return csbytes

        End If


    End Function

    Public Function DecryptedString(b As Byte(), mode As eEncryptionMode) As String

        If mode = eEncryptionMode.Smart Then

            Dim entropy As Byte() = {1, 9, 1, 1, 4, 5}
            Dim decrypted As Byte() = System.Security.Cryptography.ProtectedData.Unprotect(b, entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)

            Return System.Text.Encoding.Unicode.GetString(decrypted)

        Else

            For i As Integer = 0 To b.Length - 1
                Dim newbyte As Integer = b(i)
                newbyte -= StupidCryptOffset
                If newbyte < 0 Then
                    newbyte += 256
                End If
                b(i) = Convert.ToByte(newbyte)
            Next

            Return System.Text.Encoding.Unicode.GetString(b)

        End If


    End Function

End Module
