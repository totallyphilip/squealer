Imports System.Collections.ObjectModel
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces

Public Class GitShell

    Public Const GitErrorMessage As String = "\git-error\"

    Public Shared Function ChangedFiles(folder As String, gc As String, glob As String, includeDeleted As Boolean) As List(Of String)

        Dim files As New List(Of String)
        For Each s As String In FileSearch(folder, gc, glob)

            Dim f As String = String.Format("{0}\{1}", folder, s.Substring(3)) ' substring removes git status character from beginning
            If My.Computer.FileSystem.FileExists(f) OrElse includeDeleted Then
                files.Add(f)
            End If
        Next

        Return files

    End Function

    Public Shared Function FileSearch(folder As String, gc As String, glob As String) As List(Of String)
        ' globbing is wildcard matching
        Return Results(folder, String.Format("{0} glob ""{1}""", gc, glob))
    End Function

    Public Shared Sub DisplayResults(folder As String, gc As String, errormessage As String)

        Try
            For Each s As String In Results(folder, gc)
                Console.WriteLine()
                Textify.Write(s, ConsoleColor.Cyan, Textify.eLineMode.Truncate)
            Next
        Catch ex As Exception
            My.Logging.WriteLog(ex.Message & vbCrLf & ex.StackTrace)
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Public Shared Function CurrentBranch(folder As String) As String
        Try
            Return Results(folder, "git symbolic-ref HEAD")(0).Trim.Replace("refs/heads/", String.Empty)
        Catch ex As Exception
            My.Logging.WriteLog("CANNOT GET GIT BRANCH: " & ex.Message & vbCrLf & ex.StackTrace)
            Return GitErrorMessage
        End Try
    End Function

    Private Shared Function Results(folder As String, gc As String) As List(Of String)

        ' do git stuff and return all non-blank results
        Dim runspace As Runspace = RunspaceFactory.CreateRunspace()
        runspace.Open()
        Dim pipeline As Pipeline = runspace.CreatePipeline()
        pipeline.Commands.AddScript($"cd ""{folder}""")
        pipeline.Commands.AddScript("Out-String")
        pipeline.Commands.AddScript(gc.Replace("$", "`$"))
        Dim psobjects As Collection(Of PSObject) = pipeline.Invoke()
        runspace.Close()

        Dim sl As New List(Of String)

        For Each o As PSObject In psobjects
            If Not String.IsNullOrEmpty(o.ToString.TrimEnd) Then
                sl.Add(o.ToString.TrimEnd)
            End If
        Next

        Return sl

    End Function

End Class
