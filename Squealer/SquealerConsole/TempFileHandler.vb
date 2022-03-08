Public Class TempFileHandler

    Private f As String = My.Computer.FileSystem.GetTempFileName

    Sub Writeline()
        My.Computer.FileSystem.WriteAllText(f, vbCrLf, True)
    End Sub
    Sub Writeline(s As String)
        My.Computer.FileSystem.WriteAllText(f, s & vbCrLf, True)
    End Sub

    Sub Show(texteditor As String)

        Try
            Process.Start(texteditor, f)
        Catch ex As Exception
            Process.Start("notepad.exe", f)
        End Try

    End Sub

End Class
