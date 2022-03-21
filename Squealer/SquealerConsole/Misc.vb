Public Class Misc
    Public Shared Function WildcardInterpreter(s As String, SpaceIsWild As Boolean, EdgeIsWild As Boolean, FindExact As Boolean) As String

        If SpaceIsWild Then
            s = s.Replace(" "c, "*"c)
        End If

        If String.IsNullOrWhiteSpace(s) Then
            s = "*"
        ElseIf EdgeIsWild AndAlso Not FindExact Then
            s = "*" & s & "*"
        End If
        While s.Contains("**")
            s = s.Replace("**", "*")
        End While
        Return s & MyConstants.ObjectFileExtension

    End Function

End Class
