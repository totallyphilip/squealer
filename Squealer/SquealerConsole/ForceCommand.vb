Public Class ForceCommand

    Private _Keyword As String = String.Empty
    Public ReadOnly Property Keyword As String
        Get
            Return _Keyword
        End Get
    End Property

    Private _Crawl As Boolean = False
    Public ReadOnly Property Crawl As Boolean
        Get
            Return _Crawl
        End Get
    End Property

    Private _Response As String = String.Empty
    Public ReadOnly Property Response As String
        Get
            Return _Response
        End Get
    End Property

    Private _Activated As Boolean = False
    Public Property Found As Boolean
        Get
            Return _Activated
        End Get
        Set(value As Boolean)
            _Activated = value
        End Set
    End Property

    Public Sub New(ByVal keyword As String, ByVal crawl As Boolean, ByVal response As String)
        _Keyword = keyword
        _Crawl = crawl
        _Response = response
    End Sub

End Class
