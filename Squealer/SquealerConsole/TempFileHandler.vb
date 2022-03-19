Public Class TempFileHandler

    Private _Filename As String
    Public ReadOnly Property Filename As String
        Get
            Return _Filename
        End Get
    End Property

    Sub Writeline()
        My.Computer.FileSystem.WriteAllText(_Filename, vbCrLf, True)
    End Sub
    Sub Writeline(s As String)
        My.Computer.FileSystem.WriteAllText(_Filename, s & vbCrLf, True)
    End Sub

    Public Sub New(ext As String)
        ' Make sure it starts with a period, ex: .txt, .config
        If Not ext.StartsWith(".") Then
            ext = "." & ext
        End If
        _Filename = My.Computer.FileSystem.GetTempFileName()
        Dim path As String = My.Computer.FileSystem.GetFileInfo(_Filename).DirectoryName
        Dim newname As String = My.Computer.FileSystem.GetFileInfo(_Filename).Name.Replace(".tmp", ext)
        My.Computer.FileSystem.RenameFile(_Filename, newname)
        _Filename = path & "\" & newname
    End Sub

    Sub Show()
        EasyShell.StartProcess(_Filename)
    End Sub

End Class
