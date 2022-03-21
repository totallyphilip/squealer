Public Class UserSettingsClass

    Private _TextEditor As String
    Public Property TextEditor As String
        Get
            Return _TextEditor
        End Get
        Set(value As String)
            _TextEditor = value
        End Set
    End Property

    Private _ShowLeaderboard As Boolean
    Public Property ShowLeaderboardAtStartup As Boolean
        Get
            Return _ShowLeaderboard
        End Get
        Set(value As Boolean)
            _ShowLeaderboard = value
        End Set
    End Property

    Private _EditNew As Boolean
    Public Property EditNew As Boolean
        Get
            Return _EditNew
        End Get
        Set(value As Boolean)
            _EditNew = value
        End Set
    End Property

    Private _DirStyle As String
    Public Property DirStyle As String
        Get
            Return _DirStyle
        End Get
        Set(value As String)
            _DirStyle = value
        End Set
    End Property

    Private _RecentFolders As Integer
    Public Property RecentFolders As Integer
        Get
            Return _RecentFolders
        End Get
        Set(value As Integer)
            _RecentFolders = value
        End Set
    End Property

    Private _AutoSearch As Boolean
    Public Property AutoSearch As Boolean
        Get
            Return _AutoSearch
        End Get
        Set(value As Boolean)
            _AutoSearch = value
        End Set
    End Property

    Private _UseClipboard As Boolean
    Public Property UseClipboard As Boolean
        Get
            Return _UseClipboard
        End Get
        Set(value As Boolean)
            _UseClipboard = value
        End Set
    End Property

    Private _ShowBranch As Boolean
    Public Property ShowBranch As Boolean
        Get
            Return _ShowBranch
        End Get
        Set(value As Boolean)
            _ShowBranch = value
        End Set
    End Property

    Private _SpacesAreWildcards As Boolean
    Public Property WildcardSpaces As Boolean
        Get
            Return _SpacesAreWildcards
        End Get
        Set(value As Boolean)
            _SpacesAreWildcards = value
        End Set
    End Property

    Public LastRunVersion As String = String.Empty ' this is just to generate an intellisense name

    Private _DetectSquealerObjects As Boolean
    Public Property DetectSquealerObjects As Boolean
        Get
            Return _DetectSquealerObjects
        End Get
        Set(value As Boolean)
            _DetectSquealerObjects = value
        End Set
    End Property

    Private _LeaderboardConnectionString As String
    Public Property LeaderboardConnectionString As String
        Get
            Return _LeaderboardConnectionString
        End Get
        Set(value As String)
            _LeaderboardConnectionString = value
        End Set
    End Property

End Class