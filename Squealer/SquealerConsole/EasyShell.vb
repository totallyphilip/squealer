Imports System.Collections.ObjectModel
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces

Public Class EasyShell

    Public Shared Sub StartProcess(s As String)
        Dim runspace As Runspace = RunspaceFactory.CreateRunspace()
        runspace.Open()
        Dim pipeline As Pipeline = runspace.CreatePipeline()
        pipeline.Commands.AddScript("Start-Process " & s)
        pipeline.Invoke()
        runspace.Close()
    End Sub

End Class
