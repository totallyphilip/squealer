Public Class Settings

    Public Class WildcardBehaviorClass

        Private _UseEdges As Boolean
        Public Property UseEdges As Boolean
            Get
                Return _UseEdges
            End Get
            Set(value As Boolean)
                _UseEdges = value
            End Set
        End Property

        Private _UseSpaces As Boolean
        Public Property UseSpaces As Boolean
            Get
                Return _UseSpaces
            End Get
            Set(value As Boolean)
                _UseSpaces = value
            End Set
        End Property

    End Class

    Public Class OpenWithDefaultClass

        Private _SqlFiles As Boolean
        Public Property SqlFiles As Boolean
            Get
                Return _SqlFiles
            End Get
            Set(value As Boolean)
                _SqlFiles = value
            End Set
        End Property

        Private _ConfigFiles As Boolean
        Public Property ConfigFiles As Boolean
            Get
                Return _ConfigFiles
            End Get
            Set(value As Boolean)
                _ConfigFiles = value
            End Set
        End Property

        Private _SquealerFiles As Boolean
        Public Property SquealerFiles As Boolean
            Get
                Return _SquealerFiles
            End Get
            Set(value As Boolean)
                _SquealerFiles = value
            End Set
        End Property

    End Class

    Private _WildcardBehavior As New WildcardBehaviorClass
    Public ReadOnly Property WildcardBehavior As WildcardBehaviorClass
        Get
            Return _WildcardBehavior
        End Get
    End Property

    Private _OpenWithDefault As New OpenWithDefaultClass
    Public ReadOnly Property OpenWithDefault As OpenWithDefaultClass
        Get
            Return _OpenWithDefault
        End Get
    End Property

    Private _LastVersionCheckDate As DateTime
    Public Property LastVersionCheckDate As DateTime
        Get
            Return _LastVersionCheckDate
        End Get
        Set(value As DateTime)
            _LastVersionCheckDate = value
        End Set
    End Property

    Private _TextEditorPath As String
    Public Property TextEditorPath As String
        Get
            Return _TextEditorPath
        End Get
        Set(value As String)
            _TextEditorPath = value
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

    Private _AutoEditNewFiles As Boolean
    Public Property AutoEditNewFiles As Boolean
        Get
            Return _AutoEditNewFiles
        End Get
        Set(value As Boolean)
            _AutoEditNewFiles = value
        End Set
    End Property

    Private _ProjectFoldersLimit As Integer
    Public Property ProjectFoldersLimit As Integer
        Get
            Return _ProjectFoldersLimit
        End Get
        Set(value As Integer)
            _ProjectFoldersLimit = value
        End Set
    End Property

    Public Property RecentProjectFolders As String
        Get
            Return My.Configger.LoadSetting(NameOf(Me.RecentProjectFolders), String.Empty)
        End Get
        Set(value As String)
            My.Configger.SaveSetting(NameOf(Me.RecentProjectFolders), value)
        End Set
    End Property

    Public Property LastProjectFolder As String
        Get
            Return My.Configger.LoadSetting(NameOf(Me.LastProjectFolder), Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        End Get
        Set(value As String)
            My.Configger.SaveSetting(NameOf(Me.LastProjectFolder), value)
        End Set
    End Property

    Private _OutputToClipboard As Boolean
    Public Property OutputToClipboard As Boolean
        Get
            Return _OutputToClipboard
        End Get
        Set(value As Boolean)
            _OutputToClipboard = value
        End Set
    End Property

    Private _ShowGitBranch As Boolean
    Public Property ShowGitBranch As Boolean
        Get
            Return _ShowGitBranch
        End Get
        Set(value As Boolean)
            _ShowGitBranch = value
        End Set
    End Property

    Public LastVersionNumberExecuted As String = String.Empty

    Private _DetectDeprecatedSquealerObjects As Boolean
    Public Property DetectDeprecatedSquealerObjects As Boolean
        Get
            Return _DetectDeprecatedSquealerObjects
        End Get
        Set(value As Boolean)
            _DetectDeprecatedSquealerObjects = value
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

    Public Enum OutputStepStyle
        Detailed
        Percentage
    End Enum

    Public Function OutputStepStyleParse(s As String) As OutputStepStyle
        Return DirectCast([Enum].Parse(GetType(OutputStepStyle), s), OutputStepStyle)
    End Function

    Private _OutputStepStyle As OutputStepStyle
    Public Property OutputStepStyleSelected As OutputStepStyle
        Get
            Return _OutputStepStyle
        End Get
        Set(value As OutputStepStyle)
            _OutputStepStyle = value
        End Set
    End Property

    Public Enum DirectoryStyle
        Full
        Compact
        Symbolic
    End Enum

    Public Function DirectoryStyleParse(s As String) As DirectoryStyle
        Return DirectCast([Enum].Parse(GetType(DirectoryStyle), s), DirectoryStyle)
    End Function

    Private _DirectoryStyleSelected As DirectoryStyle
    Public Property DirectoryStyleSelected As DirectoryStyle
        Get
            Return _DirectoryStyleSelected
        End Get
        Set(value As DirectoryStyle)
            _DirectoryStyleSelected = value
        End Set
    End Property

    Private _OutputPercentageIncrement As Integer
    Public Property OutputPercentageIncrement As Integer
        Get
            Return _OutputPercentageIncrement
        End Get
        Set(value As Integer)
            _OutputPercentageIncrement = value
        End Set
    End Property

    Private _EnableEzObjects As Boolean
    Public Property EnableEzObjects As Boolean
        Get
            Return _EnableEzObjects
        End Get
        Set(value As Boolean)
            _EnableEzObjects = value
        End Set
    End Property

    Private _ShowProjectNameInTitleBar As Boolean
    Public Property ShowProjectNameInTitleBar As Boolean
        Get
            Return _ShowProjectNameInTitleBar
        End Get
        Set(value As Boolean)
            _ShowProjectNameInTitleBar = value
        End Set
    End Property

    Private _ShowProjectDirectoryInTitleBar As Boolean
    Public Property ShowProjectDirectoryInTitleBar As Boolean
        Get
            Return _ShowProjectDirectoryInTitleBar
        End Get
        Set(value As Boolean)
            _ShowProjectDirectoryInTitleBar = value
        End Set
    End Property

    Private _ShowProjectNameInCommandPrompt As Boolean
    Public Property ShowProjectNameInCommandPrompt As Boolean
        Get
            Return _ShowProjectNameInCommandPrompt
        End Get
        Set(value As Boolean)
            _ShowProjectNameInCommandPrompt = value
        End Set
    End Property

    Private _KeepScreenAlive As Boolean
    Public Property KeepScreenAlive As Boolean
        Get
            Return _KeepScreenAlive
        End Get
        Set(value As Boolean)
            _KeepScreenAlive = value
        End Set
    End Property

    Private _LockWindowSize As Boolean
    Public Property LockWindowSize As Boolean
        Get
            Return _LockWindowSize
        End Get
        Set(value As Boolean)
            _LockWindowSize = value
        End Set
    End Property

    Dim CdaPath As String = "S:\Software\Squealer\"
    Dim s3Path As String = "https://s3-us-west-1.amazonaws.com/public-10ec013b-b521-4150-9eab-56e1e1bb63a4/Squealer/"
    Public ReadOnly Property MediaSourceUrl As String
        Get
            Try
                If Environment.UserDomainName = "CDA" Then
                    Return CdaPath
                Else
                    Return s3Path
                End If
            Catch ex As Exception
                Return s3Path
            End Try
        End Get
    End Property

    Public ReadOnly Property IsDefaultMediaSource As Boolean
        Get
            Return MediaSourceUrl = s3Path
        End Get
    End Property

    Private _TrackFailedItems As Boolean
    Public Property TrackFailedItems As Boolean
        Get
            Return _TrackFailedItems
        End Get
        Set(value As Boolean)
            _TrackFailedItems = value
        End Set
    End Property

    Private _AlwaysShowSymbols As Boolean
    Public Property AlwaysShowSymbols As Boolean
        Get
            Return _AlwaysShowSymbols
        End Get
        Set(value As Boolean)
            _AlwaysShowSymbols = value
        End Set
    End Property

    Private _AutoCompressGit As Boolean
    Public Property AutoCompressGit As Boolean
        Get
            Return _AutoCompressGit
        End Get
        Set(value As Boolean)
            _AutoCompressGit = value
        End Set
    End Property

    Public Sub New()
        ' Use this when you just want an empty settings object.
        Me.New(False)
    End Sub

    Public Sub New(doload As Boolean)
        ' Use this to load saved settings from disk.
        If doload Then
            LoadSettings()
        End If
    End Sub

    Public Sub LoadSettings()

        ' Load settings.
        Me.LastVersionCheckDate = My.Configger.LoadSetting(NameOf(Me.LastVersionCheckDate), New DateTime(0))
        Me.AlwaysShowSymbols = My.Configger.LoadSetting(NameOf(Me.AlwaysShowSymbols), True)
        Me.TrackFailedItems = My.Configger.LoadSetting(NameOf(Me.TrackFailedItems), True)
        Me.ShowProjectNameInTitleBar = My.Configger.LoadSetting(NameOf(Me.ShowProjectNameInTitleBar), True)
        Me.KeepScreenAlive = My.Configger.LoadSetting(NameOf(Me.KeepScreenAlive), False)
        Me.LockWindowSize = My.Configger.LoadSetting(NameOf(Me.LockWindowSize), False)
        Me.AutoCompressGit = My.Configger.LoadSetting(NameOf(Me.AutoCompressGit), False)
        Me.ShowProjectDirectoryInTitleBar = My.Configger.LoadSetting(NameOf(Me.ShowProjectDirectoryInTitleBar), True)
        Me.ShowProjectNameInCommandPrompt = My.Configger.LoadSetting(NameOf(Me.ShowProjectNameInCommandPrompt), True)
        Me.OpenWithDefault.SqlFiles = My.Configger.LoadSetting(NameOf(Me.OpenWithDefault.SqlFiles), False)
        Me.OpenWithDefault.ConfigFiles = My.Configger.LoadSetting(NameOf(Me.OpenWithDefault.ConfigFiles), False)
        Me.OpenWithDefault.SquealerFiles = My.Configger.LoadSetting(NameOf(Me.OpenWithDefault.SquealerFiles), False)
        Me.TextEditorPath = My.Configger.LoadSetting(NameOf(Me.TextEditorPath), "notepad.exe")
        Me.LeaderboardConnectionString = My.Configger.LoadSetting(NameOf(Me.LeaderboardConnectionString), String.Empty)
        Me.ProjectFoldersLimit = My.Configger.LoadSetting(NameOf(Me.ProjectFoldersLimit), 20)
        Me.WildcardBehavior.UseEdges = My.Configger.LoadSetting(NameOf(Me.WildcardBehavior.UseEdges), False)
        Me.AutoEditNewFiles = My.Configger.LoadSetting(NameOf(Me.AutoEditNewFiles), True)
        Me.OutputToClipboard = My.Configger.LoadSetting(NameOf(Me.OutputToClipboard), True)
        Me.ShowLeaderboardAtStartup = My.Configger.LoadSetting(NameOf(Me.ShowLeaderboardAtStartup), False)
        Me.DetectDeprecatedSquealerObjects = My.Configger.LoadSetting(NameOf(Me.DetectDeprecatedSquealerObjects), True)
        Me.ShowGitBranch = My.Configger.LoadSetting(NameOf(Me.ShowGitBranch), True)
        Me.EnableEzObjects = My.Configger.LoadSetting(NameOf(Me.EnableEzObjects), False)
        Me.WildcardBehavior.UseSpaces = My.Configger.LoadSetting(NameOf(Me.WildcardBehavior.UseSpaces), False)
        Me.DirectoryStyleSelected = DirectoryStyleParse(My.Configger.LoadSetting(NameOf(Me.DirectoryStyleSelected), DirectoryStyle.Full.ToString))
        Me.OutputStepStyleSelected = OutputStepStyleParse(My.Configger.LoadSetting(NameOf(Me.OutputStepStyleSelected), OutputStepStyle.Detailed.ToString))
        Me.OutputPercentageIncrement = My.Configger.LoadSetting(NameOf(Me.OutputPercentageIncrement), 5)
        If Not (Me.OutputPercentageIncrement = 5 OrElse Me.OutputPercentageIncrement = 10 OrElse Me.OutputPercentageIncrement = 20 OrElse Me.OutputPercentageIncrement = 25) Then
            Me.OutputPercentageIncrement = 5
        End If
        Textify.ErrorAlert.Beep = My.Configger.LoadSetting(NameOf(Textify.ErrorAlert.Beep), False)

    End Sub

    Public Sub Show()

        Dim f As New SettingsForm
        f.ddIncrement.SelectedIndex = f.ddIncrement.FindString(Me.OutputPercentageIncrement.ToString) ' must set this before radio button because rb checked triggers an event using this value
        Select Case Me.OutputStepStyleSelected
            Case OutputStepStyle.Detailed
                f.rbDetailed.Checked = True
            Case OutputStepStyle.Percentage
                f.rbPercentage.Checked = True
        End Select
        Select Case Me.DirectoryStyleSelected
            Case DirectoryStyle.Compact
                f.rbCompact.Checked = True
            Case DirectoryStyle.Full
                f.rbFull.Checked = True
            Case DirectoryStyle.Symbolic
                f.rbSymbolic.Checked = True
        End Select
        f.chkShowProjectNameInTitleBar.Checked = Me.ShowProjectNameInTitleBar
        f.chkKeepScreenOn.Checked = Me.KeepScreenAlive
        f.chkLockWindowSize.Checked = Me.LockWindowSize
        f.chkAutoCompressGit.Checked = Me.AutoCompressGit
        f.chkShowProjectDirectoryInTitleBar.Checked = Me.ShowProjectDirectoryInTitleBar
        f.chkShowProjectNameInCommandPrompt.Checked = Me.ShowProjectNameInCommandPrompt
        f.chkOutputDefaultEditor.Checked = Me.OpenWithDefault.SqlFiles
        f.chkConfigDefaultEditor.Checked = Me.OpenWithDefault.ConfigFiles
        f.chkSquealerDefaultEditor.Checked = Me.OpenWithDefault.SquealerFiles
        f.txtEditorProgram.Text = Me.TextEditorPath
        f.updnFolderSaves.Value = Me.ProjectFoldersLimit
        f.chkSpacesWild.Checked = Me.WildcardBehavior.UseSpaces
        f.chkEdgesWild.Checked = Me.WildcardBehavior.UseEdges
        f.optEditNewFiles.Checked = Me.AutoEditNewFiles
        If Me.OutputToClipboard Then
            f.rbClipboard.Checked = True
        Else
            f.rbTempFile.Checked = True
        End If
        f.optShowGitBranch.Checked = Me.ShowGitBranch
        f.chkEnableEzObjects.Checked = Me.EnableEzObjects
        f.optBeep.Checked = Textify.ErrorAlert.Beep
        f.optDetectOldSquealerObjects.Checked = Me.DetectDeprecatedSquealerObjects
        f.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
        f.chkTrackFailedItems.Checked = Me.TrackFailedItems
        f.chkAlwaysShowSymbols.Checked = Me.AlwaysShowSymbols

        f.ShowDialog()

        Me.OutputPercentageIncrement = CInt(f.ddIncrement.SelectedItem.ToString)
        If f.rbDetailed.Checked Then
            Me.OutputStepStyleSelected = OutputStepStyle.Detailed
        Else
            Me.OutputStepStyleSelected = OutputStepStyle.Percentage
        End If
        If f.rbCompact.Checked Then
            Me.DirectoryStyleSelected = DirectoryStyle.Compact
        ElseIf f.rbFull.Checked Then
            Me.DirectoryStyleSelected = DirectoryStyle.Full
        Else
            Me.DirectoryStyleSelected = DirectoryStyle.Symbolic
        End If
        Me.ShowProjectNameInTitleBar = f.chkShowProjectNameInTitleBar.Checked
        Me.KeepScreenAlive = f.chkKeepScreenOn.Checked
        Me.LockWindowSize = f.chkLockWindowSize.Checked
        Me.AutoCompressGit = f.chkAutoCompressGit.Checked
        Me.ShowProjectDirectoryInTitleBar = f.chkShowProjectDirectoryInTitleBar.Checked
        Me.ShowProjectNameInCommandPrompt = f.chkShowProjectNameInCommandPrompt.Checked
        Me.OpenWithDefault.SqlFiles = f.chkOutputDefaultEditor.Checked
        Me.OpenWithDefault.ConfigFiles = f.chkConfigDefaultEditor.Checked
        Me.OpenWithDefault.SquealerFiles = f.chkSquealerDefaultEditor.Checked
        Me.TextEditorPath = f.txtEditorProgram.Text
        Me.ProjectFoldersLimit = CInt(f.updnFolderSaves.Value)
        Me.WildcardBehavior.UseSpaces = f.chkSpacesWild.Checked
        Me.WildcardBehavior.UseEdges = f.chkEdgesWild.Checked
        Me.AutoEditNewFiles = f.optEditNewFiles.Checked
        Me.OutputToClipboard = f.rbClipboard.Checked
        Me.ShowGitBranch = f.optShowGitBranch.Checked
        Me.EnableEzObjects = f.chkEnableEzObjects.Checked
        Textify.ErrorAlert.Beep = f.optBeep.Checked
        Me.DetectDeprecatedSquealerObjects = f.optDetectOldSquealerObjects.Checked
        Me.TrackFailedItems = f.chkTrackFailedItems.Checked
        Me.AlwaysShowSymbols = f.chkAlwaysShowSymbols.Checked

        My.Configger.SaveSetting(NameOf(Me.MediaSourceUrl), Me.MediaSourceUrl)
        My.Configger.SaveSetting(NameOf(Me.OutputPercentageIncrement), Me.OutputPercentageIncrement)
        My.Configger.SaveSetting(NameOf(Me.OutputStepStyleSelected), Me.OutputStepStyleSelected.ToString)
        My.Configger.SaveSetting(NameOf(Me.DirectoryStyleSelected), Me.DirectoryStyleSelected.ToString)
        My.Configger.SaveSetting(NameOf(Me.OpenWithDefault.SqlFiles), Me.OpenWithDefault.SqlFiles)
        My.Configger.SaveSetting(NameOf(Me.OpenWithDefault.ConfigFiles), Me.OpenWithDefault.ConfigFiles)
        My.Configger.SaveSetting(NameOf(Me.OpenWithDefault.SquealerFiles), Me.OpenWithDefault.SquealerFiles)
        My.Configger.SaveSetting(NameOf(Me.TextEditorPath), Me.TextEditorPath)
        My.Configger.SaveSetting(NameOf(Me.LeaderboardConnectionString), Me.LeaderboardConnectionString)
        My.Configger.SaveSetting(NameOf(Me.ProjectFoldersLimit), Me.ProjectFoldersLimit)
        My.Configger.SaveSetting(NameOf(Me.WildcardBehavior.UseSpaces), Me.WildcardBehavior.UseSpaces)
        My.Configger.SaveSetting(NameOf(Me.WildcardBehavior.UseEdges), Me.WildcardBehavior.UseEdges)
        My.Configger.SaveSetting(NameOf(Me.ShowProjectNameInTitleBar), Me.ShowProjectNameInTitleBar)
        My.Configger.SaveSetting(NameOf(Me.ShowProjectDirectoryInTitleBar), Me.ShowProjectDirectoryInTitleBar)
        My.Configger.SaveSetting(NameOf(Me.KeepScreenAlive), Me.KeepScreenAlive)
        My.Configger.SaveSetting(NameOf(Me.LockWindowSize), Me.LockWindowSize)
        My.Configger.SaveSetting(NameOf(Me.AutoCompressGit), Me.AutoCompressGit)
        My.Configger.SaveSetting(NameOf(Me.ShowProjectNameInCommandPrompt), Me.ShowProjectNameInCommandPrompt)
        My.Configger.SaveSetting(NameOf(Me.AutoEditNewFiles), Me.AutoEditNewFiles)
        My.Configger.SaveSetting(NameOf(Me.ShowLeaderboardAtStartup), Me.ShowLeaderboardAtStartup)
        My.Configger.SaveSetting(NameOf(Me.OutputToClipboard), Me.OutputToClipboard)
        My.Configger.SaveSetting(NameOf(Me.DetectDeprecatedSquealerObjects), Me.DetectDeprecatedSquealerObjects)
        My.Configger.SaveSetting(NameOf(Me.TrackFailedItems), Me.TrackFailedItems)
        My.Configger.SaveSetting(NameOf(Me.AlwaysShowSymbols), Me.AlwaysShowSymbols)
        My.Configger.SaveSetting(NameOf(Me.ShowGitBranch), Me.ShowGitBranch)
        My.Configger.SaveSetting(NameOf(Me.EnableEzObjects), Me.EnableEzObjects)
        My.Configger.SaveSetting(NameOf(Textify.ErrorAlert.Beep), Textify.ErrorAlert.Beep)

    End Sub

End Class