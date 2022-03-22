Imports System.Collections.ObjectModel
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces

Public Class EasyShell

    Public Shared Sub StartProcess(process As String)
        StartProcess(process, Nothing)
    End Sub

    Public Shared Sub StartProcess(process As String, args As String)
        Dim runspace As Runspace = RunspaceFactory.CreateRunspace()
        runspace.Open()
        Dim pipeline As Pipeline = runspace.CreatePipeline()
        If String.IsNullOrEmpty(args) Then
            pipeline.Commands.AddScript(String.Format("Start-Process ""{0}""", process))
        Else
            pipeline.Commands.AddScript(String.Format("Start-Process ""{0}"" '""{1}""'", process, args))
        End If
        pipeline.Invoke()
        runspace.Close()
    End Sub

End Class
