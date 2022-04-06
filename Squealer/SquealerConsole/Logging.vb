Namespace My

    '<HideModuleName()> ' No, don't hide it from IntelliSense!
    Module Logging

#Region " General Stuff "

        ' The root folder for all the settings files.
        Private _AppDataFolder As String = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\", My.Application.Info.CompanyName, "\", My.Application.Info.Title)
        Private _DefaultLogFile As String = _AppDataFolder & "\" & "application.log"

#End Region
        Private Sub SplitLogOver10MB(filename As String)

            If My.Computer.FileSystem.FileExists(filename) Then

                Dim info As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(filename)

                If info.Length > 10000000 Then
                    Dim newname As String = info.Name.Replace(info.Extension, "") & String.Format("{0}-{1}-{2}", Now.Year, Now.Month.ToString("00"), Now.Day.ToString("00")) & info.Extension
                    If Not My.Computer.FileSystem.FileExists(info.DirectoryName & "\" & newname) Then
                        My.Computer.FileSystem.RenameFile(filename, newname)
                    End If
                End If
            End If

        End Sub


        Public Sub WriteLog(message As String)
            WriteLog(_DefaultLogFile, message)
        End Sub

        Public Sub WriteLog(filename As String, message As String)

            SplitLogOver10MB(filename)

            If Not My.Computer.FileSystem.DirectoryExists(_AppDataFolder) Then
                My.Computer.FileSystem.CreateDirectory(_AppDataFolder)
            End If

            My.Computer.FileSystem.WriteAllText(filename, String.Format("[{0}] {1}", Now.ToString, message & vbCrLf), True)

        End Sub

    End Module

End Namespace