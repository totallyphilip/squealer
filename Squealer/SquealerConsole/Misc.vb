Public Class Misc
    Public Shared Function WildcardInterpreter(s As String, spaces As Boolean, edges As Boolean, FindExact As Boolean) As String
        WildcardInterpreter = WildcardInterpreter(s, New Settings.WildcardClass With {.UseEdges = edges, .UseSpaces = spaces}, FindExact)
    End Function

    Public Shared Function WildcardInterpreter(s As String, w As Settings.WildcardClass, FindExact As Boolean) As String

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
        Return s & MyConstants.SquealerFileExtension

    End Function

    Public Shared Function IsStarWarsDay() As Boolean

        'todo: turn star wars day back on
        If Now.Month = 5 AndAlso Now.Day = 4 AndAlso False Then ' added false to hide this for now
            Return True
        Else
            Return False
        End If

    End Function

End Class
