Public Class StarWarsClass

    Private _ForceCommands As New List(Of ForceCommand)
    Public ReadOnly Property ForceCommands As List(Of ForceCommand)
        Get
            Return _ForceCommands
        End Get
    End Property

    Private _StarWarsDay As Boolean = False
    Public Property StarWarsDay As Boolean
        Get
            Return _StarWarsDay
        End Get
        Set(value As Boolean)
            _StarWarsDay = value
        End Set
    End Property

    Public Sub New()
        _StarWarsDay = Today.Month = 5 AndAlso Today.Day = 4
    End Sub

    Public Sub AddCommand(ByVal fc As ForceCommand)
        ForceCommands.Add(fc)
    End Sub

    Public ReadOnly Property FoundCount As Integer
        Get
            Return ForceCommands.Where(Function(x) x.Found = True).Count
        End Get
    End Property

    Public ReadOnly Property PossibleCount As Integer
        Get
            Return ForceCommands.Count
        End Get
    End Property

    Public ReadOnly Property FoundAll As Boolean
        Get
            Return Me.FoundCount = Me.PossibleCount
        End Get
    End Property


    Public Function ForcePowerFind(ByVal k As String) As ForceCommand

        Dim fc As ForceCommand = ForceCommands.Find(Function(x) x.Keyword = k)
        If fc Is Nothing Then
            Return New ForceCommand("error", False, "error")
        Else
            Return fc
        End If

    End Function

End Class
