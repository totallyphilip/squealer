Imports System.Windows.Forms

Public Class GitFlags

    Private _ShowUncommittedChanges As Boolean
    Public Property ShowUncommittedChanges As Boolean
        Get
            Return _ShowUncommittedChanges
        End Get
        Set(value As Boolean)
            _ShowUncommittedChanges = value
        End Set
    End Property

    Private _ShowHistory As Boolean
    Public Property ShowHistory As Boolean
        Get
            Return _ShowHistory
        End Get
        Set(value As Boolean)
            _ShowHistory = value
        End Set
    End Property

    Private _UndoChanges As Boolean
    Public Property UndoChanges As Boolean
        Get
            Return _UndoChanges
        End Get
        Set(value As Boolean)
            _UndoChanges = value
        End Set
    End Property

    Public ReadOnly Property GitEnabled As Boolean
        Get
            Return _ShowHistory Or _ShowUncommittedChanges Or _UndoChanges
        End Get
    End Property

    Public Sub New(Optional unc As Boolean = False, Optional undo As Boolean = False, Optional history As Boolean = False)
        _ShowUncommittedChanges = unc
        _UndoChanges = undo
        _ShowHistory = history
    End Sub

End Class

Module Main

    Private MyCommands As New CommandCatalog.CommandDefinitionList
    Private StarWars As New StarWarsClass
    Private BadCommandMessage As String = "Bad command or options."
    Private UserSettings As New UserSettingsClass

    Const cDefaultConnectionString As String = "Server=MySqlServer;Initial Catalog=AdventureWorks;Trusted_Connection=True"
    Const s3VersionText As String = "https://s3-us-west-1.amazonaws.com/public-10ec013b-b521-4150-9eab-56e1e1bb63a4/Squealer/ver.txt"
    Const s3ZipFile As String = "https://s3-us-west-1.amazonaws.com/public-10ec013b-b521-4150-9eab-56e1e1bb63a4/Squealer/Squealer.zip"

    Public Class UserSettingsClass

        Private _EditNew As Boolean
        Public Property EditNew As Boolean
            Get
                Return _EditNew
            End Get
            Set(value As Boolean)
                _EditNew = value
            End Set
        End Property

        Private _TextEditor As String
        Public Property TextEditor As String
            Get
                Return _TextEditor
            End Get
            Set(value As String)
                _TextEditor = value
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

        Private _TextEditorSwitches As String
        Public Property TextEditorSwitches As String
            Get
                Return _TextEditorSwitches
            End Get
            Set(value As String)
                _TextEditorSwitches = value
            End Set
        End Property

        Private _ShowClock As Boolean
        Public Property ShowClock As Boolean
            Get
                Return _ShowClock
            End Get
            Set(value As Boolean)
                _ShowClock = value
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

    End Class

    Public Class MyConstants
        Public Shared ReadOnly Property ObjectFileExtension As String
            Get
                Return ".sqlr"
            End Get
        End Property

        Public Shared ReadOnly Property ConfigFilename As String
            Get
                Return My.Application.Info.ProductName & ".config"
            End Get
        End Property

        Public Shared ReadOnly Property ConnectionStringFilename As String
            Get
                Return ".connectionstring"
            End Get
        End Property

        Public Shared ReadOnly Property AutocreateFilename As String
            Get
                Return String.Format("({0})", My.Application.Info.ProductName)
            End Get
        End Property

    End Class

#Region " Enums "

    Public Enum eDirectoryStyle
        full
        compact
        symbolic
        invalid
    End Enum

    Private Enum eFileAction
        directory
        git
        edit
        generate
        fix
        compare
        delete
    End Enum

    Private Enum eMode
        normal
        flags
        encrypt
        test
        alter
        convert
        permanent
        [string]
    End Enum

    Private Enum eCommandType
        [git]
        [command]
        [nerfherder]
        [about]
        [contact]
        [changelog]
        [clear]
        [config]
        [delete]
        [directory]
        [edit]
        [exit]
        [explore]
        [fix]
        [forget]
        [generate]
        [help]
        [list]
        [new]
        [nuke]
        [open]
        [raiserror]
        [setting]
        [connection]
        [compare]
        [test]
        [update]
        [use]
        [usetheforce]
        make
    End Enum

#End Region

#Region " Folders "

    ' Set a new working folder and remember it for later.
    Private Sub ChangeFolder(ByVal newpath As String, ByRef ProjectFolder As String)

        My.Computer.FileSystem.CurrentDirectory = newpath ' this will throw an error if the path is not valid
        ProjectFolder = newpath
        RememberFolder(newpath)
        Textify.SayBulletLine(Textify.eBullet.Hash, "OK")
        Textify.SayNewLine()

        ' Temporary code to rename existing connection strings 4/3/2019
        Dim oldcs As String = newpath & "\.Squealer_cs"
        Try
            If My.Computer.FileSystem.FileExists(oldcs) Then
                My.Computer.FileSystem.RenameFile(oldcs, MyConstants.ConnectionStringFilename)
            End If
        Catch ex As Exception
            ' suppress errors
        End Try

    End Sub

    Private Function FolderCollection() As List(Of String)

        Dim folders As New List(Of String)
        Dim unsplit As String = My.Configger.LoadSetting("Folders", String.Empty)
        If Not String.IsNullOrWhiteSpace(unsplit) Then
            folders.AddRange(My.Configger.LoadSetting("Folders", "nothing").Split(New Char() {"|"c}))
        End If
        While folders.Count > UserSettings.RecentFolders
            folders.RemoveAt(folders.Count - 1)
        End While
        Return folders

    End Function

    Private Function InvalidFolderIndex() As Integer

        Dim f As String = FolderCollection().Find(Function(x) Not My.Computer.FileSystem.DirectoryExists(x))
        If String.IsNullOrEmpty(f) Then ' couldn't find any bad directories
            f = FolderCollection().Find(Function(x) My.Computer.FileSystem.GetFiles(x, FileIO.SearchOption.SearchTopLevelOnly, "*" & MyConstants.ObjectFileExtension).Count = 0)
            If String.IsNullOrEmpty(f) Then ' couldn't find any unused directories
                InvalidFolderIndex = -1
            Else
                InvalidFolderIndex = FolderCollection().IndexOf(f)
            End If
        Else
            InvalidFolderIndex = FolderCollection().IndexOf(f)
        End If

    End Function

    Private Sub AutoRemoveFolders()

        If InvalidFolderIndex() = -1 Then
            Textify.WriteLine("All folders contain *" & MyConstants.ObjectFileExtension)
        Else
            While InvalidFolderIndex() > -1
                Dim i As Integer = InvalidFolderIndex()
                Textify.WriteLine(eCommandType.forget.ToString & " " & FolderCollection(i), ConsoleColor.Red)
                ForgetFolder(i)
            End While
        End If

    End Sub

    Private Function FolderString(ByVal folders As List(Of String)) As String

        Dim s As String = String.Empty
        For Each item As String In folders
            s &= item & "|"
        Next
        If s.Length > 0 Then
            s = s.Remove(s.Length - 1)
        End If

        FolderString = s

    End Function


    ' List all remembered folders.
    Sub ListFolders(ByVal WorkingFolder As String)

        Dim folders As List(Of String) = FolderCollection()
        Dim longestnickname As Integer = 0

        If folders.Count = 0 Then
            Throw New Exception("No remembered folders.")
        Else

            Dim farray(folders.Count - 1, 3) As String

            For i As Integer = 0 To folders.Count - 1
                farray(i, 0) = i.ToString
                farray(i, 1) = GetProjectNickname(folders(i))
                farray(i, 2) = folders(i)
                If Not My.Computer.FileSystem.DirectoryExists(farray(i, 2)) Then
                    farray(i, 1) = "**********"
                    farray(i, 2) = "<<not found>>" & farray(i, 2)
                End If
                If farray(i, 1).Length > longestnickname Then
                    longestnickname = farray(i, 1).Length
                End If
            Next

            Dim highestnumber As Integer = farray.GetLength(0) - 1

            For i As Integer = 0 To highestnumber
                Textify.SayBullet(Textify.eBullet.Star, String.Format("{0} | ", farray(i, 0).PadLeft(highestnumber.ToString.Length)))
                Textify.Write(farray(i, 1).PadRight(longestnickname), ConsoleColor.Cyan)
                Textify.WriteLine(String.Format(" | {0}", farray(i, 2)))
            Next

        End If

        Textify.SayNewLine()

    End Sub

    ' Set a remembered folder as the current working folder.
    Sub LoadFolder(ByVal NewFolder As String, ByRef WorkingFolder As String)

        Try
            Dim n As Integer
            If Integer.TryParse(NewFolder, n) AndAlso n < 100 Then
                ' Load by project number
                ChangeFolder(FolderCollection(n), WorkingFolder)
            Else
                ' Load by project name
                Dim s As String = FolderCollection.Find(Function(x) GetProjectNickname(x).ToLower.StartsWith(NewFolder.ToLower))
                If String.IsNullOrEmpty(s) Then
                    s = FolderCollection.Find(Function(x) GetProjectNickname(x).ToLower.Contains(NewFolder.ToLower))
                End If
                ChangeFolder(s, WorkingFolder)
            End If

        Catch ex As Exception
            Throw New Exception("Invalid folder specification.")
        End Try

    End Sub

    ' Remove a folder from the list of projects.
    Sub ForgetFolder(index As String)
        ForgetFolder(CInt(index))
    End Sub
    Sub ForgetFolder(index As Integer)

        Try
            Dim folders As List(Of String) = FolderCollection()
            Dim folder As String = folders(index)
            folders.Remove(folder)
            My.Configger.SaveSetting("Folders", FolderString(folders))
            Textify.SayBulletLine(Textify.eBullet.Hash, "OK")
        Catch ex As Exception
            Throw New Exception("Invalid folder number.")
        End Try

    End Sub

    ' Save the folder to the list of projects.
    Sub RememberFolder(ByVal folder As String)

        Dim folders As List(Of String) = FolderCollection()
        While folders.Contains(folder)
            folders.Remove(folder)
        End While
        folders.Insert(0, folder)
        My.Configger.SaveSetting("Folders", FolderString(folders))

    End Sub

#End Region

#Region " Main Functions "

    ' Main module. Start here.
    Sub Main()

        DefineCommands()

        ' Increase input buffer size.
        Console.SetIn(New IO.StreamReader(Console.OpenStandardInput(8192)))

        ' Load settings.
        UserSettings.TextEditor = My.Configger.LoadSetting(NameOf(UserSettings.TextEditor), "notepad.exe")
        UserSettings.RecentFolders = My.Configger.LoadSetting(NameOf(UserSettings.RecentFolders), 20)
        UserSettings.TextEditorSwitches = My.Configger.LoadSetting(NameOf(UserSettings.TextEditorSwitches), "")
        UserSettings.ShowClock = My.Configger.LoadSetting(NameOf(UserSettings.ShowClock), False)
        UserSettings.AutoSearch = My.Configger.LoadSetting(NameOf(UserSettings.AutoSearch), False)
        UserSettings.EditNew = My.Configger.LoadSetting(NameOf(UserSettings.EditNew), True)
        UserSettings.UseClipboard = My.Configger.LoadSetting(NameOf(UserSettings.UseClipboard), True)
        UserSettings.ShowBranch = My.Configger.LoadSetting(NameOf(UserSettings.ShowBranch), True)
        UserSettings.WildcardSpaces = My.Configger.LoadSetting(NameOf(UserSettings.WildcardSpaces), False)
        UserSettings.DirStyle = My.Configger.LoadSetting(NameOf(UserSettings.DirStyle), eDirectoryStyle.compact.ToString)
        Textify.ErrorAlert.Beep = My.Configger.LoadSetting(NameOf(Textify.ErrorAlert.Beep), False)

        ' Restore the previous working folder
        Dim WorkingFolder As String = My.Configger.LoadSetting("PreviousFolder", My.Computer.FileSystem.SpecialDirectories.MyDocuments)
        If My.Computer.FileSystem.DirectoryExists(WorkingFolder) Then
            ChangeFolder(WorkingFolder, WorkingFolder)
        End If
        Console.Clear()

        ' Restore the previous window size
        Try
            ' This fails if the console was in full-screen mode at previous exit
            Console.SetWindowSize(My.Configger.LoadSetting("WindowWidth", 130), My.Configger.LoadSetting("WindowHeight", 30))
        Catch ex As Exception
        End Try
        Console.BufferWidth = Console.WindowWidth

        ' Happy Star Wars day.
        If StarWars.StarWarsDay Then
            BeginStarWarsDay()
        End If

        ' Change log.

        'Dim fileName$ = System.Reflection.Assembly.GetExecutingAssembly().Location
        'Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(fileName)
        'Dim fvAsString$ = fvi.FileVersion ' but other useful properties exist too.

        Dim ver As New Version(My.Configger.LoadSetting(NameOf(UserSettings.LastRunVersion), "0.0.0.0"))
        If My.Application.Info.Version.CompareTo(ver) > 0 Then
            ReadChangeLog()
            My.Configger.SaveSetting(NameOf(UserSettings.LastRunVersion), My.Application.Info.Version.ToString)
        End If

        'If Not My.Configger.LoadSetting(NameOf(UserSettings.LastRunVersion), "0.0.0.0") = fvi.FileVersion Then
        '    ReadChangeLog()
        '    My.Configger.SaveSetting(NameOf(UserSettings.LastRunVersion), fvi.FileVersion)
        'End If

        ' Main process
        Console.WriteLine()
        CheckS3(True)
        HandleUserInput(WorkingFolder)

        ' Save the window size
        My.Configger.SaveSetting("WindowWidth", Console.WindowWidth)
        My.Configger.SaveSetting("WindowHeight", Console.WindowHeight)

        ' Save the current working folder for next time
        My.Configger.SaveSetting("PreviousFolder", WorkingFolder)

        ' Delete any settings that weren't referenced
        My.Configger.PruneSettings()

    End Sub

    Private Function FilesToProcess(ByVal ProjectFolder As String, ByVal Wildcard As String, SearchText As String, usedialog As Boolean, filter As SquealerObjectTypeCollection, ignoreCase As Boolean, FindExact As Boolean, todayonly As Boolean) As List(Of String)

        Wildcard = Wildcard.Replace("[", "").Replace("]", "")

        Dim plaincolor As New Textify.ColorScheme(ConsoleColor.Gray, ConsoleColor.Black)
        Dim highlightcolor As New Textify.ColorScheme(ConsoleColor.Cyan, ConsoleColor.Black)

        Textify.SayBullet(Textify.eBullet.Hash, "")
        Textify.Write("finding", plaincolor)

        Dim comma As String = Nothing

        If todayonly Then
            Textify.Write(" today's", highlightcolor)
        End If
        If filter.AllSelected Then
            Textify.Write(" all", highlightcolor)
        Else
            comma = " "

            For Each t As SquealerObjectType In filter.Items.Where(Function(x) x.Selected)
                Textify.Write(comma & t.LongType.ToString, highlightcolor)
                comma = ", "
            Next
        End If

        If usedialog Then
            Textify.Write(" hand-picked", highlightcolor)
        End If

        Dim EverythingIncludingDuplicates As New List(Of String)
        Textify.Write(" files", plaincolor)
        If Not String.IsNullOrEmpty(SearchText) Then
            Textify.Write(" containing ", plaincolor)
            Textify.Write("""" & SearchText & """" & IIf(ignoreCase, "", "(case-sensitive)").ToString, highlightcolor)
        End If
        Textify.Write(" matching", plaincolor)

        comma = ""

        For Each s As String In Wildcard.Split((New Char() {"|"c}))
            If s.ToLower.Contains(MyConstants.ObjectFileExtension.ToLower) Then
                Console.WriteLine()
                Throw New ArgumentException(s.Trim & " search term contains explicit reference To " & MyConstants.ObjectFileExtension)
            End If
            s = WildcardInterpreter(s.Trim, FindExact)
            Textify.Write(comma & " " & s, highlightcolor)
            comma = ", "

            If String.IsNullOrEmpty(SearchText) Then
                EverythingIncludingDuplicates.AddRange(My.Computer.FileSystem.GetFiles(ProjectFolder, FileIO.SearchOption.SearchTopLevelOnly, s).ToList)
            Else
                EverythingIncludingDuplicates.AddRange(My.Computer.FileSystem.FindInFiles(ProjectFolder, SearchText, ignoreCase, FileIO.SearchOption.SearchTopLevelOnly, s).ToList)
            End If
        Next

        Dim DistinctFiles As New List(Of String)
        DistinctFiles.AddRange(From s In EverythingIncludingDuplicates Distinct Order By s)

        Console.WriteLine()
        Console.WriteLine()

        ' Remove any results that don't match hand picked files.
        If usedialog Then
            Dim pickedfiles As List(Of String) = GetFileList(ProjectFolder)
            DistinctFiles.RemoveAll(Function(x) Not pickedfiles.Exists(Function(y) y = x))
        End If

        ' Remove any results that don't match the requested object types
        For Each t As SquealerObjectType In filter.Items.Where(Function(x) Not x.Selected)
            DistinctFiles.RemoveAll(Function(x) SquealerObjectType.Eval(XmlGetObjectType(x)) = t.LongType)
        Next

        ' Remove any results that don't match the time constraint
        If todayonly Then
            With My.Computer.FileSystem
                DistinctFiles.RemoveAll(Function(x) Not (.GetFileInfo(x).LastWriteTime.Year = Now.Year AndAlso .GetFileInfo(x).LastWriteTime.DayOfYear = Now.DayOfYear))
            End With
        End If

        Return DistinctFiles

    End Function

    ' Enumerate through files in the working folder and take some action on them.
    Private Sub ProcessFiles(ByVal FileListing As List(Of String), ByVal Action As eFileAction, mode As eMode, ByVal TargetFileType As SquealerObjectType.eType, git As GitFlags, MakePretty As Boolean)

        Dim FileCount As Integer = 0
        Dim SkippedFiles As Integer = 0
        Dim GeneratedOutput As String = String.Empty

        If mode = eMode.string Then
            Console.Write(eCommandType.directory.ToString.ToLower & " - x ")
        Else

            If UserSettings.DirStyle = eDirectoryStyle.full.ToString Then
                Textify.Write("Type Flags ")
            ElseIf UserSettings.DirStyle = eDirectoryStyle.compact.ToString Then
                Textify.Write("   ")
            ElseIf UserSettings.DirStyle = eDirectoryStyle.symbolic.ToString Then
                Textify.Write(" ")
            End If

            Textify.WriteLine("FileName")

            If UserSettings.DirStyle = eDirectoryStyle.full.ToString Then
                Textify.Write("---- ----- ")
            ElseIf UserSettings.DirStyle = eDirectoryStyle.compact.ToString Then
                Textify.Write("-- ")
            End If

            Textify.WriteToEol("-"c)
            Console.WriteLine()

        End If

        For Each FileName As String In FileListing

            If Console.KeyAvailable() Then
                Throw New System.Exception("Keyboard interrupt.")
            End If

            BracketCheck(FileName)

            Dim info As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(FileName)

            Dim obj As New SquealerObject(FileName)

            If mode = eMode.string Then
                If FileCount > 0 Then
                    Console.Write("|")
                End If
                Console.Write(info.Name.Replace(MyConstants.ObjectFileExtension, ""))
            Else
                Dim fg As ConsoleColor = ConsoleColor.Gray




                If obj.Type.LongType = SquealerObjectType.eType.Invalid Then
                    fg = ConsoleColor.Red
                End If


                If UserSettings.DirStyle = eDirectoryStyle.full.ToString Then
                    Textify.Write(" " & obj.Type.ShortType.ToString.PadRight(4) & obj.FlagsSummary)
                ElseIf UserSettings.DirStyle = eDirectoryStyle.compact.ToString Then
                    Textify.Write(obj.Type.ShortType.ToString.PadRight(2))
                    If String.IsNullOrWhiteSpace(obj.FlagsSummary) Then
                        Textify.Write(" ")
                    Else
                        Textify.Write("*")
                    End If
                ElseIf UserSettings.DirStyle = eDirectoryStyle.symbolic.ToString Then
                    If String.IsNullOrWhiteSpace(obj.FlagsSummary) Then
                        Textify.Write(" ")
                    Else
                        Textify.Write("*")
                    End If
                End If

                Textify.Write(info.Name.Replace(MyConstants.ObjectFileExtension, ""), fg)

                Dim symbol As String = String.Empty
                Select Case obj.Type.LongType
                    Case SquealerObjectType.eType.StoredProcedure
                        symbol = ""
                    Case SquealerObjectType.eType.ScalarFunction
                        symbol = "()"
                    Case SquealerObjectType.eType.InlineTableFunction
                        symbol = "*"
                    Case SquealerObjectType.eType.MultiStatementTableFunction
                        symbol = "**"
                    Case SquealerObjectType.eType.View
                        symbol = "+"
                End Select

                If UserSettings.DirStyle = eDirectoryStyle.symbolic.ToString Then
                    Textify.Write(symbol, ConsoleColor.Green)
                End If


            End If

            Try

                Select Case Action
                    Case eFileAction.directory
                        If mode = eMode.flags AndAlso obj.FlagsList.Count > 0 Then
                            Console.WriteLine()
                            Console.WriteLine("           {")
                            For Each s As String In obj.FlagsList
                                Console.WriteLine("             " & s)
                            Next
                            Console.Write("           }")
                        End If
                    Case eFileAction.fix
                        If mode = eMode.normal Then
                            If RepairXmlFile(False, info.FullName, MakePretty) Then
                                Textify.Write(String.Format(" ... {0}", eCommandType.fix.ToString.ToUpper), ConsoleColor.Green)
                            Else
                                SkippedFiles += 1
                            End If
                        Else
                            If ConvertXmlFile(info.FullName, TargetFileType, MakePretty) Then
                                Textify.Write(String.Format(" ... {0}", eMode.convert.ToString.ToUpper))
                            Else
                                SkippedFiles += 1
                            End If
                        End If
                    Case eFileAction.git
                        If git.ShowUncommittedChanges Then
                            Textify.Write(" " & GitResults(info.DirectoryName, "git status -s " & info.Name)(0).Replace(info.Name, "").TrimStart, ConsoleColor.Red)

                        End If
                        If git.ShowHistory Then
                            GitCommandDo(info.DirectoryName, "git log --pretty=format:""%h (%cr) %s"" " & info.Name, " (no history)")
                        End If




                    Case eFileAction.edit
                        FileEdit(info.FullName)
                    Case eFileAction.generate
                        GeneratedOutput &= ExpandIndividual(info, GetStringReplacements(My.Computer.FileSystem.GetFileInfo(FileListing(0)).DirectoryName), mode)
                    Case eFileAction.compare
                        Dim RootName As String = info.Name.Replace(MyConstants.ObjectFileExtension, "")
                        GeneratedOutput &= String.Format("insert #CodeToDrop ([Type], [Schema], [Name]) values ('{0}','{1}','{2}');", obj.Type.GeneralType, SchemaName(RootName), RoutineName(RootName)) & vbCrLf
                    Case eFileAction.delete
                        Dim trashcan As FileIO.RecycleOption = FileIO.RecycleOption.SendToRecycleBin
                        If mode = eMode.permanent Then
                            trashcan = FileIO.RecycleOption.DeletePermanently
                        End If
                        My.Computer.FileSystem.DeleteFile(info.FullName, FileIO.UIOption.OnlyErrorDialogs, trashcan)
                End Select
                If Not mode = eMode.string Then
                    Console.WriteLine()
                End If
                FileCount += 1
            Catch ex As Exception
                Textify.WriteLine(" ... FAILED!", ConsoleColor.Red)
                Throw New Exception(ex.Message)
            End Try

        Next

        If FileCount > 0 Then
            If mode = eMode.string Then
                Textify.SayNewLine()
            End If
            Textify.SayNewLine()
        End If

        Dim SummaryLine As String = "{4}/{0} files ({3} skipped) (action:{1}, mode:{2})"
        If SkippedFiles = 0 Then
            SummaryLine = "{0} files (action:{1}, mode:{2})"
        End If

        Textify.SayBulletLine(Textify.eBullet.Hash, String.Format(SummaryLine, FileCount.ToString, Action.ToString, mode.ToString, SkippedFiles.ToString, (FileCount - SkippedFiles).ToString))

        If (Action = eFileAction.generate OrElse Action = eFileAction.compare) AndAlso FileCount > 0 Then

            If Action = eFileAction.compare Then
                GeneratedOutput = My.Resources.SqlDropOrphanedRoutines.Replace("{RoutineList}", GeneratedOutput).Replace("{ExcludeFilename}", MyConstants.AutocreateFilename)
            End If

            If UserSettings.UseClipboard Then
                Console.WriteLine()
                Textify.SayBulletLine(Textify.eBullet.Hash, "Output copied to Windows clipboard.")
                Clipboard.SetText(GeneratedOutput)
            Else
                With My.Computer.FileSystem
                    Dim tempfile As String = .GetTempFileName
                    .WriteAllText(tempfile, GeneratedOutput, False)
                    Process.Start(UserSettings.TextEditor, tempfile)
                End With
            End If

        End If

        Textify.SayNewLine()

    End Sub

#End Region

#Region " Commands "

    Private Sub DefineCommands()

        Dim cmd As CommandCatalog.CommandDefinition
        Dim opt As CommandCatalog.CommandSwitch

        ' the un-command
        cmd = New CommandCatalog.CommandDefinition({eCommandType.nerfherder.ToString, "nerf"}, {"This command is as useless as a refrigerator on Hoth."}, CommandCatalog.eCommandCategory.other)
        cmd.Visible = False
        MyCommands.Items.Add(cmd)

        ' open folder
        cmd = New CommandCatalog.CommandDefinition({eCommandType.open.ToString}, {"Open folder {options}.", "This folder path will be saved for quick access. See " & eCommandType.list.ToString.ToUpper & " command."}, CommandCatalog.eCommandCategory.folder, "<path>", True)
        cmd.Examples.Add("% " & My.Computer.FileSystem.SpecialDirectories.MyDocuments)
        cmd.Examples.Add("% C:\Some Folder\Spaced Are OK")
        MyCommands.Items.Add(cmd)

        ' list folders
        cmd = New CommandCatalog.CommandDefinition({eCommandType.list.ToString, "l"}, {"List the saved folders."}, CommandCatalog.eCommandCategory.folder)
        MyCommands.Items.Add(cmd)

        ' use folder
        cmd = New CommandCatalog.CommandDefinition({eCommandType.use.ToString}, {"Reopen a saved folder.", "See " & eCommandType.list.ToString.ToUpper & " command."}, CommandCatalog.eCommandCategory.folder, "<project name or folder number>", True)
        cmd.Examples.Add("% 3")
        cmd.Examples.Add("% northwind")
        MyCommands.Items.Add(cmd)

        ' forget folder
        cmd = New CommandCatalog.CommandDefinition({eCommandType.forget.ToString}, {"Forget a saved folder.", "See " & eCommandType.list.ToString.ToUpper & " command. Either specify a folder to forget, or automatically forget all folders that do not contain any " & MyConstants.ObjectFileExtension & " files."}, CommandCatalog.eCommandCategory.folder, "<folder number>", False)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("auto;detect invalid folders"))
        cmd.Examples.Add("% 3")
        cmd.Examples.Add("% -auto")
        MyCommands.Items.Add(cmd)

        ' file explorer
        cmd = New CommandCatalog.CommandDefinition({eCommandType.explore.ToString, "fe"}, {"Open File Explorer.", "Opens the current working folder. If {options} is specified, the first matching " & My.Application.Info.ProductName & " object will be selected."}, CommandCatalog.eCommandCategory.folder, CommandCatalog.CommandDefinition.WildcardText, False)
        MyCommands.Items.Add(cmd)

        ' command
        cmd = New CommandCatalog.CommandDefinition({eCommandType.command.ToString, "cmd"}, {"Open a command prompt."}, CommandCatalog.eCommandCategory.folder)
        MyCommands.Items.Add(cmd)


        ' dir
        cmd = New CommandCatalog.CommandDefinition({eCommandType.directory.ToString, "dir"}, {"Directory.", String.Format("List {0} objects in the current working folder.", My.Application.Info.ProductName)}, CommandCatalog.eCommandCategory.file, False, True)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("f;show flags"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("str;string output"))
        cmd.Examples.Add("% -cs dbo.* / Han shot first -- find all dbo.* files containing "" Han shot first"" (with leading space and capital H)")
        cmd.Examples.Add("% -p -v /Solo -- find all stored procedures and views containing ""Solo"" (or ""solo"" or ""SOLO"" or ""soLO"")")
        MyCommands.Items.Add(cmd)

        ' new file
        cmd = New CommandCatalog.CommandDefinition({eCommandType.new.ToString}, {String.Format("Create a new {0} object.", My.Application.Info.ProductName), "Default schema is ""dbo""."}, CommandCatalog.eCommandCategory.file, CommandCatalog.CommandDefinition.FilenameText, True)
        For Each s As String In New SquealerObjectTypeCollection().ObjectTypesOptionString(False).Split((New Char() {"|"c}))
            cmd.Options.Items.Add(New CommandCatalog.CommandSwitch(s, s.StartsWith("p")))
        Next
        cmd.Examples.Add("% AddEmployee -- create new stored procedure dbo.AddEmployee")
        cmd.Examples.Add("% -v myschema.Employees -- create new view myschema.Employees")
        MyCommands.Items.Add(cmd)

        ' edit files
        cmd = New CommandCatalog.CommandDefinition({eCommandType.edit.ToString, "e"}, {String.Format("Edit {0} objects.", My.Application.Info.ProductName), String.Format("Uses your configured text editor. See {0} command.", eCommandType.setting.ToString.ToUpper)}, CommandCatalog.eCommandCategory.file, False, True)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("all;override file limit"))
        cmd.Examples.Add("% dbo.AddEmployee")
        cmd.Examples.Add("% dbo.*")
        MyCommands.Items.Add(cmd)

        ' fix files
        cmd = New CommandCatalog.CommandDefinition({eCommandType.fix.ToString}, {String.Format("Rewrite {0} objects (DESTRUCTIVE).", My.Application.Info.ProductName), String.Format("Original files will be rewritten To {0} specifications. Optionally convert objects to a different type.", My.Application.Info.ProductName)}, CommandCatalog.eCommandCategory.file, False, True)
        opt = New CommandCatalog.CommandSwitch("c;convert to")
        For Each s As String In New SquealerObjectTypeCollection().ObjectTypesOptionString(False).Split((New Char() {"|"c}))
            opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(s))
        Next
        cmd.Options.Items.Add(opt)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("format;beautify code"))
        cmd.Examples.Add("% dbo.*")
        cmd.Examples.Add("% -c:p * -- convert everything to stored procedures")
        cmd.Examples.Add("% -v -p -c:if * -- convert views and stored procedures to inline table-valued functions")
        MyCommands.Items.Add(cmd)

        ' generate
        cmd = New CommandCatalog.CommandDefinition({eCommandType.generate.ToString, "gen"}, {"Generate SQL Server objects.", String.Format("Output is written to a temp file and opened with your configured text editor. See {0} command.", eCommandType.setting.ToString.ToUpper)}, CommandCatalog.eCommandCategory.file, False, True)
        opt = New CommandCatalog.CommandSwitch("m;output mode")
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption("alt;alter, do not drop original"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption("t;test script, limit 1 object"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption("e;with encryption"))
        cmd.Options.Items.Add(opt)
        cmd.Examples.Add("% dbo.*")
        cmd.Examples.Add("% -alt -v dbo.* -- generate ALTER scripts for dbo.* VIEW objects")
        MyCommands.Items.Add(cmd)

        ' compare
        cmd = New CommandCatalog.CommandDefinition({eCommandType.compare.ToString}, {String.Format("Compare {0} with SQL Server.", My.Application.Info.ProductName), String.Format("This generates a T-SQL query to discover any SQL Server objects that are not in {0}, and any {0} objects that are not in SQL Server.", My.Application.Info.ProductName)}, CommandCatalog.eCommandCategory.file, False, True)
        MyCommands.Items.Add(cmd)

        ' delete
        cmd = New CommandCatalog.CommandDefinition({eCommandType.delete.ToString, "del"}, {String.Format("Delete {0} objects.", My.Application.Info.ProductName), "Objects will be sent to the Recycle Bin by default."}, CommandCatalog.eCommandCategory.file, True, True)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("e;permanently erase"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("all;override file limit"))
        cmd.Examples.Add("% dbo.AddEmployee")
        cmd.Examples.Add("% dbo.*")
        MyCommands.Items.Add(cmd)

        ' nuke
        cmd = New CommandCatalog.CommandDefinition({eCommandType.nuke.ToString}, {String.Format("Quick delete {0} objects.", My.Application.Info.ProductName), "This performs an operating system delete command, so it's extremely fast but irreversible and unforgiving."}, CommandCatalog.eCommandCategory.file, CommandCatalog.CommandDefinition.WildcardText, True)
        cmd.Examples.Add(String.Format("% dbo.* -- delete dbo.*{0}", MyConstants.ObjectFileExtension))
        cmd.Examples.Add(String.Format("% * -- delete *{0}", MyConstants.ObjectFileExtension))
        MyCommands.Items.Add(cmd)



        ' git
        cmd = New CommandCatalog.CommandDefinition({eCommandType.git.ToString}, {"A limited Git interface.", String.Format("Similar to {0}, but for Git commands.", eCommandType.directory.ToString)}, CommandCatalog.eCommandCategory.file, False, True)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("ch;show uncommitted changes", True))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("h;show history"))
        MyCommands.Items.Add(cmd)



        ' kessel
        cmd = New CommandCatalog.CommandDefinition({eCommandType.make.ToString}, {String.Format("Automatically create {0} objects.", My.Application.Info.ProductName), "Create default insert, update, read, and delete objects for the target database. Define the target database with the " & eCommandType.connection.ToString.ToUpper & " command."}, CommandCatalog.eCommandCategory.file)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch(String.Format("nosave;generate everything, {0} local output", eCommandType.nuke.ToString.ToUpper)))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch(String.Format("r;replace existing {0} objects only", My.Application.Info.ProductName)))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("nocomment;omit data source and timestamp from comment section"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch(String.Format("nuke;{2} *{0}*{1}", MyConstants.AutocreateFilename, MyConstants.ObjectFileExtension, eCommandType.nuke.ToString.ToUpper)))
        MyCommands.Items.Add(cmd)


        ' help
        cmd = New CommandCatalog.CommandDefinition({eCommandType.help.ToString, "h"}, {"{command} for command list, or {command} {options} for details of a single command.", "Switches are ignored if a command is specified."}, CommandCatalog.eCommandCategory.other, "<command>", False)
        cmd.Examples.Add("% " & eCommandType.generate.ToString)
        MyCommands.Items.Add(cmd)


        ' config
        cmd = New CommandCatalog.CommandDefinition({eCommandType.config.ToString, "c"}, {"Display or edit " & MyConstants.ConfigFilename & ".", "This file configures how " & My.Application.Info.ProductName & " operates in your current working folder."}, CommandCatalog.eCommandCategory.other)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("e;edit existing file"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("new;create new default file"))
        MyCommands.Items.Add(cmd)

        ' setting
        cmd = New CommandCatalog.CommandDefinition({eCommandType.setting.ToString, "set"}, {"Display or change application settings."}, CommandCatalog.eCommandCategory.other, "<new setting>", False)
        opt = New CommandCatalog.CommandSwitch("u;update setting, cannot be used with any other switches")
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(Textify.ErrorAlert.Beep).ToLower & ";beep on error"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(UserSettings.UseClipboard).ToLower & ";output to clipboard"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(UserSettings.WildcardSpaces).ToLower & ";spaces for asterisks"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(UserSettings.AutoSearch).ToLower & ";auto-search expansion"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(UserSettings.ShowClock).ToLower & ";clock display"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(UserSettings.TextEditor).ToLower & ";text editor program"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(UserSettings.EditNew).ToLower & ";edit on new file"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(UserSettings.ShowBranch).ToLower & ";show Git branch"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(UserSettings.RecentFolders).ToLower & ";maximum recent folders"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(NameOf(UserSettings.DirStyle).ToLower & ";directory style"))
        cmd.Options.Items.Add(opt)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("switches;define text editor command line switches, cannot be used with any other switches"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("noswitches;clear any text editor command line switches, cannot be used with any other switches"))
        cmd.Examples.Add(String.Format("% -u:{0} false -- turn off the clock display", (NameOf(UserSettings.ShowClock).ToLower)))
        cmd.Examples.Add(String.Format("% -u:{0} c:\windows\notepad.exe -- set Notepad as your text editor", (NameOf(UserSettings.TextEditor).ToLower)))
        cmd.Examples.Add("% -switches -- open the switch configurator")
        MyCommands.Items.Add(cmd)

        ' connection string
        cmd = New CommandCatalog.CommandDefinition({eCommandType.connection.ToString, "cs"}, {"Define the SQL Server connection string.", String.Format("The connection string is encrypted for the current local user and current working folder, and is required for some {0} commands. If you are using version control, you should add ""{1}"" to your ignore list.", My.Application.Info.ProductName, MyConstants.ConnectionStringFilename)}, CommandCatalog.eCommandCategory.other, "<connectionstring>", False)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("e;edit current connection string"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("t;test connection", True))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("set;encrypt and save the connection string"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("show;display the connection string"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("forget;discard the saved connection string"))
        cmd.Examples.Add("% -set " & cDefaultConnectionString)
        cmd.Examples.Add("% -get")
        MyCommands.Items.Add(cmd)

        ' email the developer
        cmd = New CommandCatalog.CommandDefinition({eCommandType.contact.ToString}, {"Email the developer.", "Use this to report a bug or to request a feature."}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)

        ' cls
        cmd = New CommandCatalog.CommandDefinition({eCommandType.clear.ToString, "cc"}, {"Clear the console."}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)


        ' raiserror
        cmd = New CommandCatalog.CommandDefinition({eCommandType.raiserror.ToString, "err"}, {String.Format("Display the T-SQL for raising errors inside a {0} object.", My.Application.Info.ProductName), My.Application.Info.ProductName & " has specific rules about how to raise errors."}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)

        ' about
        cmd = New CommandCatalog.CommandDefinition({eCommandType.about.ToString}, {"About this program."}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)

        ' check for updates
        cmd = New CommandCatalog.CommandDefinition({eCommandType.update.ToString}, {"Check for updates."}, CommandCatalog.eCommandCategory.other)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("get;download the latest version"))
        MyCommands.Items.Add(cmd)

        ' change log
        cmd = New CommandCatalog.CommandDefinition({eCommandType.changelog.ToString}, {"View the change log."}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)

        ' exit
        cmd = New CommandCatalog.CommandDefinition({eCommandType.exit.ToString, "x"}, {"Quit."}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)

        ' force
        cmd = New CommandCatalog.CommandDefinition({eCommandType.usetheforce.ToString, "force"}, {"It's not about lifting rocks. Use the force, [Luke]!", "Do not prematurely speak blasphemy. It MUST be the last thing you say."}, CommandCatalog.eCommandCategory.other, "?????", False)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("e;list found easter eggs"))
        cmd.Examples.Add("% [?????]")
        cmd.Examples.Add("% 5/4")
        cmd.Visible = StarWars.StarWarsDay
        MyCommands.Items.Add(cmd)

        ' test 
        cmd = New CommandCatalog.CommandDefinition({eCommandType.test.ToString}, {"Hidden command. Debugging/testing only."}, CommandCatalog.eCommandCategory.other)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("release;generate version text file"))
        cmd.Visible = False
        MyCommands.Items.Add(cmd)

        ' Star Wars commands
        StarWars.AddCommand(New ForceCommand("luke", False, "OK Google, navigate [x-wing] to Tosche Station."))
        StarWars.AddCommand(New ForceCommand("yoda", False, "Shot first, Han did."))
        StarWars.AddCommand(New ForceCommand("greedo", False, "Don't say it!"))
        StarWars.AddCommand(New ForceCommand("greedo shot first", False, ""))
        StarWars.AddCommand(New ForceCommand("han", False, "Han who? You mean that scoundrel famous for his speed on the Kessel Run?"))
        StarWars.AddCommand(New ForceCommand("han solo", False, "Who shot first?"))
        StarWars.AddCommand(New ForceCommand("han shot first", False, "May the Force be with you! If you ever say [Greedo] shot first, it'll be the *last* thing you do."))
        StarWars.AddCommand(New ForceCommand("leia", False, "[I love you]."))
        StarWars.AddCommand(New ForceCommand("c-3po", False, "These aren't the [droids] you're looking for. Might I suggest looking for one with a bad motivator?"))
        StarWars.AddCommand(New ForceCommand("r2-d2", False, "These aren't the [droids] you're looking for. And this one is filthy."))
        StarWars.AddCommand(New ForceCommand("r5-d4", False, "POW! fzzzt *smoke*"))
        StarWars.AddCommand(New ForceCommand("clean r2-d2", False, "(A hologram flickers on.) Help me Obi-Wan Kenobi! You're my only hope."))
        StarWars.AddCommand(New ForceCommand("obi-wan kenobi", False, "I wonder if you mean old Ben Kenobi?"))
        StarWars.AddCommand(New ForceCommand("ben kenobi", False, "I'm so powerful, I can make Vader block himself with my lightsaber while I practically stand still."))
        StarWars.AddCommand(New ForceCommand("darth vader", False, vbCrLf & My.Resources.eggVader))
        StarWars.AddCommand(New ForceCommand("i love you", False, "I know."))
        StarWars.AddCommand(New ForceCommand("droids", False, vbCrLf & My.Resources.eggDroids))
        StarWars.AddCommand(New ForceCommand("empire", False, vbCrLf & My.Resources.eggEmpire))
        StarWars.AddCommand(New ForceCommand("rebels", False, vbCrLf & My.Resources.eggRebels))
        StarWars.AddCommand(New ForceCommand("x-wing", False, vbCrLf & My.Resources.eggXwing))
        StarWars.AddCommand(New ForceCommand("tie fighter", False, ""))
        StarWars.AddCommand(New ForceCommand("crawl i", True, My.Resources.eggCrawl_I))
        StarWars.AddCommand(New ForceCommand("crawl ii", True, My.Resources.eggSeagulls))
        StarWars.AddCommand(New ForceCommand("crawl iii", True, My.Resources.eggCrawl_III))
        StarWars.AddCommand(New ForceCommand("crawl iv", True, My.Resources.eggCrawl_IV))
        StarWars.AddCommand(New ForceCommand("crawl v", True, My.Resources.eggCrawl_V))
        StarWars.AddCommand(New ForceCommand("crawl vi", True, My.Resources.eggCrawl_VI))
        StarWars.AddCommand(New ForceCommand("crawl vii", True, My.Resources.eggCrawl_VII))
        StarWars.AddCommand(New ForceCommand("crawl viii", True, My.Resources.eggCrawl_VIII))
        StarWars.AddCommand(New ForceCommand("hoth", False, "That planet is inhothpitable!"))
        StarWars.AddCommand(New ForceCommand("tosche station", False, "CLOSED FOR THE SUMMER. And we're sold out of power converters."))
        StarWars.AddCommand(New ForceCommand("kessel run", False, "Word of the day:" & vbCrLf & vbCrLf & "par·sec" & vbCrLf & "/ˈpärsek/" & vbCrLf & "noun" & vbCrLf & "a unit of distance used in astronomy, equal to about 3.26 light years (3.086 × 1013 kilometers). One parsec corresponds to the distance at which the mean radius of the earth's orbit subtends an angle of one second of arc."))

    End Sub


    Private Function WildcardInterpreter(s As String, FindExact As Boolean) As String

        If UserSettings.WildcardSpaces Then
            s = s.Replace(" "c, "*"c)
        End If

        If String.IsNullOrWhiteSpace(s) Then
            s = "*"
        ElseIf UserSettings.AutoSearch AndAlso Not FindExact Then
            s = "*" & s & "*"
        End If
        While s.Contains("**")
            s = s.Replace("**", "*")
        End While
        Return s & MyConstants.ObjectFileExtension

    End Function

    Private Function StringInList(l As List(Of String), s As String) As Boolean
        Return l.Exists(Function(x) x.ToLower = s.ToLower)
    End Function


    ' The main command interface loop.
    Private Sub HandleUserInput(ByRef WorkingFolder As String)

        Dim MySwitches As New List(Of String)
        Dim UserInput As String = Nothing

        Textify.SayBulletLine(Textify.eBullet.Hash, "Type HELP to get started.")
        Console.WriteLine()

        Dim MyCommand As CommandCatalog.CommandDefinition = MyCommands.FindCommand(eCommandType.nerfherder.ToString)
        Dim SwitchesValidated As Boolean = True
        Dim MySearchText As String = String.Empty
        Dim ObjectTypeFilter As New SquealerObjectTypeCollection



        While Not (MyCommand IsNot Nothing AndAlso MyCommand.Keyword = eCommandType.exit.ToString AndAlso SwitchesValidated AndAlso String.IsNullOrEmpty(UserInput))

            Try

                If MyCommand IsNot Nothing AndAlso MyCommand.Keyword = eCommandType.nerfherder.ToString Then

                    ' do nothing


                ElseIf Not SwitchesValidated Then

                    Throw New Exception("Invalid command switch.")


                ElseIf MyCommand IsNot Nothing AndAlso MyCommand.ParameterRequired AndAlso String.IsNullOrEmpty(UserInput) Then

                    Throw New Exception("Required parameter is missing.")


                ElseIf MyCommand IsNot Nothing AndAlso String.IsNullOrEmpty(MyCommand.ParameterDefinition) AndAlso Not String.IsNullOrEmpty(UserInput) Then

                    Throw New Exception("Unexpected command parameter.")


                ElseIf MyCommand Is Nothing Then

                    Throw New System.Exception(BadCommandMessage)


                ElseIf MyCommand.Keyword = eCommandType.about.ToString Then

                    AboutInfo()


                ElseIf MyCommand.Keyword = eCommandType.update.ToString Then

                    If StringInList(MySwitches, "get") Then
                        Dim wc As New Net.WebClient
                        Dim fn As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Squealer.zip"
                        wc.DownloadFile(s3ZipFile, fn)
                        Textify.SayBulletLine(Textify.eBullet.Hash, "File downloaded to " & fn)
                        Textify.SayNewLine()
                    Else
                        CheckS3(False)
                    End If


                ElseIf MyCommand.Keyword = eCommandType.contact.ToString Then 'AndAlso Command.SwitchesOK(CommandSwitches) AndAlso CommandParameters.Count = 0 Then

                    With My.Application.Info
                        System.Diagnostics.Process.Start("mailto:philip.bigbang@gmail.com?subject=" & .ProductName & " " & .Version.ToString & " user feedback")
                    End With


                ElseIf MyCommand.Keyword = eCommandType.[changelog].ToString Then

                    ReadChangeLog()


                ElseIf MyCommand.Keyword = eCommandType.clear.ToString Then

                    Console.Clear()


                ElseIf MyCommand.Keyword = eCommandType.command.ToString Then

                    Dim cw As New ProcessStartInfo("cmd.exe")
                    cw.WorkingDirectory = WorkingFolder
                    Process.Start(cw)


                ElseIf MyCommand.Keyword = eCommandType.[config].ToString Then

                    ' Try to make a new file
                    If StringInList(MySwitches, "new") Then
                        If My.Computer.FileSystem.FileExists(WorkingFolder & "\" & MyConstants.ConfigFilename) Then
                            Throw New Exception("Config file already exists.")
                        Else
                            My.Computer.FileSystem.WriteAllText(WorkingFolder & "\" & MyConstants.ConfigFilename, My.Resources.UserConfig, False)
                            SayFileAction("config file created", WorkingFolder, MyConstants.ConfigFilename)
                            Textify.SayNewLine()
                        End If
                        Textify.SayNewLine()
                    End If

                    ' Now edit 
                    If StringInList(MySwitches, "e") Then
                        FileEdit(WorkingFolder & "\" & MyConstants.ConfigFilename)
                    End If

                    ' No switches, so just display
                    If MySwitches.Count = 0 Then
                        ShowFile(WorkingFolder, MyConstants.ConfigFilename)
                        Textify.SayNewLine()
                    End If


                ElseIf MyCommand.Keyword = eCommandType.[delete].ToString _
                    OrElse MyCommand.Keyword = eCommandType.directory.ToString _
                    OrElse MyCommand.Keyword = eCommandType.[generate].ToString _
                    OrElse MyCommand.Keyword = eCommandType.edit.ToString _
                    OrElse MyCommand.Keyword = eCommandType.fix.ToString _
                    OrElse MyCommand.Keyword = eCommandType.compare.ToString _
                    OrElse MyCommand.Keyword = eCommandType.git.ToString Then


                    Dim FileLimit As Integer = Integer.MaxValue
                    Dim action As eFileAction = eFileAction.directory
                    Dim mode As eMode = eMode.normal
                    Dim targetftype As SquealerObjectType.eType = SquealerObjectType.eType.Invalid ' for object conversion only
                    Dim todayonly As Boolean = StringInList(MySwitches, "today")
                    Dim git As New GitFlags()
                    Dim pretty As Boolean = False

                    If Not MyCommand.ParameterRequired AndAlso String.IsNullOrWhiteSpace(UserInput) Then
                        UserInput = "*"
                    End If

                    Dim usedialog As Boolean = False
                    If Not String.IsNullOrWhiteSpace(UserInput) AndAlso UserInput = "#" Then
                        usedialog = True
                        UserInput = "*"
                    End If

                    If MyCommand.Keyword = eCommandType.delete.ToString Then

                        action = eFileAction.delete

                        If StringInList(MySwitches, "e") Then
                            mode = eMode.permanent
                        End If

                        FileLimit = 20


                    ElseIf MyCommand.Keyword = eCommandType.directory.ToString Then

                        If StringInList(MySwitches, "f") Then
                            mode = eMode.flags
                        ElseIf StringInList(MySwitches, "str") Then
                            mode = eMode.string
                        End If


                    ElseIf MyCommand.Keyword = eCommandType.git.ToString Then

                        action = eFileAction.git

                        If StringInList(MySwitches, "h") Then
                            git.ShowHistory = True
                        End If
                        If StringInList(MySwitches, "ch") Then
                            git.ShowUncommittedChanges = True
                        End If

                        If Not git.GitEnabled Then
                            git.ShowUncommittedChanges = True ' this is the default switch if nothing else was specified
                        End If


                    ElseIf MyCommand.Keyword = eCommandType.edit.ToString Then

                        action = eFileAction.edit

                        FileLimit = 10


                    ElseIf MyCommand.Keyword = eCommandType.generate.ToString Then

                        action = eFileAction.generate

                        If StringInList(MySwitches, "m:t") Then
                            mode = eMode.test
                            FileLimit = 1
                        ElseIf StringInList(MySwitches, "m:e") Then
                            mode = eMode.encrypt
                        ElseIf StringInList(MySwitches, "m:alt") Then
                            mode = eMode.alter
                        End If

                    ElseIf MyCommand.Keyword = eCommandType.fix.ToString Then

                        action = eFileAction.fix

                        If StringInList(MySwitches, "format") Then
                            pretty = True
                        End If

                        Dim convertswitch As String = MySwitches.Find(Function(x) x.Split(New Char() {":"c})(0).ToLower = "c")
                        If Not String.IsNullOrWhiteSpace(convertswitch) Then
                            targetftype = SquealerObjectType.Eval(convertswitch.Split(New Char() {":"c})(1))
                        End If

                        If Not targetftype = SquealerObjectType.eType.Invalid Then
                            mode = eMode.convert
                        End If

                    ElseIf MyCommand.Keyword = eCommandType.compare.ToString Then

                        action = eFileAction.compare

                    End If

                    Dim ignoreCase As Boolean = Not StringInList(MySwitches, "cs")
                    Dim findexact As Boolean = StringInList(MySwitches, "x")
                    Dim ignorefilelimit As Boolean = StringInList(MySwitches, "all")

                    Dim SelectedFiles As List(Of String) = FilesToProcess(WorkingFolder, UserInput, MySearchText, usedialog, ObjectTypeFilter, ignoreCase, findexact, todayonly)

                    ' Remove any files that don't have uncommitted git statuses
                    If git.ShowUncommittedChanges Then
                        Try
                            Dim changedfiles As List(Of String) = GitChangedFiles(WorkingFolder, "git status -s")
                            SelectedFiles.RemoveAll(Function(x) Not changedfiles.Exists(Function(y) y = x))
                        Catch ex As Exception
                        End Try
                    End If

                    ThrowErrorIfOverFileLimit(FileLimit, SelectedFiles.Count, ignorefilelimit)

                    ProcessFiles(SelectedFiles, action, mode, targetftype, git, pretty)




                ElseIf MyCommand.Keyword = eCommandType.[forget].ToString AndAlso StringInList(MySwitches, "auto") AndAlso String.IsNullOrEmpty(UserInput) Then

                    AutoRemoveFolders()
                    Textify.SayNewLine()

                ElseIf MyCommand.Keyword = eCommandType.[forget].ToString AndAlso Not StringInList(MySwitches, "auto") AndAlso Not String.IsNullOrEmpty(UserInput) Then

                    ForgetFolder(UserInput)
                    Textify.SayNewLine()




                ElseIf MyCommand.Keyword = eCommandType.[help].ToString Then

                    If String.IsNullOrEmpty(UserInput) Then
                        MyCommands.ShowHelpCatalog()
                    Else
                        Dim HelpWithCommand As CommandCatalog.CommandDefinition = MyCommands.FindCommand(UserInput)

                        If HelpWithCommand IsNot Nothing Then
                            HelpWithCommand.ShowHelp()
                        Else
                            Throw New Exception("Unknown command.")
                        End If
                    End If



                    'ElseIf MyCommand.Keyword = eCommandType.[uncommitted].ToString Then

                    '    Textify.Write("Uncommitted changes in ")
                    '    Textify.Write(WorkingFolder, ConsoleColor.White)
                    '    Textify.WriteLine(":")

                    '    GitCommandDo(WorkingFolder, "git status -s", "no changes")

                    '    Textify.SayNewLine(2)


                ElseIf MyCommand.Keyword = eCommandType.[list].ToString Then

                    ListFolders(WorkingFolder)


                ElseIf MyCommand.Keyword = eCommandType.[new].ToString Then

                    Dim filetype As SquealerObjectType.eType = SquealerObjectType.eType.StoredProcedure
                    If ObjectTypeFilter.SelectedCount > 0 Then
                        filetype = ObjectTypeFilter.Items.Find(Function(x) x.Selected).LongType
                    End If

                    BracketCheck(UserInput)

                    Dim f As String = CreateNewFile(WorkingFolder, filetype, UserInput)

                    If UserSettings.EditNew AndAlso Not String.IsNullOrEmpty(f) Then
                        FileEdit(f)
                    End If


                ElseIf MyCommand.Keyword = eCommandType.[open].ToString Then

                    ChangeFolder(UserInput, WorkingFolder)


                ElseIf MyCommand.Keyword = eCommandType.[raiserror].ToString Then

                    Console.WriteLine(My.Resources.RaiseErrors)


                ElseIf MyCommand.Keyword = eCommandType.setting.ToString AndAlso MySwitches.Count = 0 AndAlso String.IsNullOrWhiteSpace(UserInput) Then

                    SettingsView()


                ElseIf MyCommand.Keyword = eCommandType.setting.ToString AndAlso Not String.IsNullOrWhiteSpace(UserInput) Then

                    SettingChange(MySwitches(0).Split(New Char() {":"c})(1), UserInput)
                    Textify.SayNewLine()


                ElseIf MyCommand.Keyword = eCommandType.setting.ToString AndAlso StringInList(MySwitches, "switches") Then

                    Dim switches As String = Microsoft.VisualBasic.Interaction.InputBox("Text Editor Switches", "", UserSettings.TextEditorSwitches).Trim
                    If String.IsNullOrWhiteSpace(switches) Then
                        Textify.SayBulletLine(Textify.eBullet.Hash, "no change")
                        Textify.SayNewLine()
                        switches = UserSettings.TextEditorSwitches
                    Else
                        Textify.SayBulletLine(Textify.eBullet.Hash, "text editor switches updated")
                        UserSettings.TextEditorSwitches = switches
                        My.Configger.SaveSetting(NameOf(UserSettings.TextEditorSwitches), switches)
                    End If

                    Textify.SayBulletLine(Textify.eBullet.Arrow, UserSettings.TextEditor & " " & switches & " <FileName>")
                    Textify.SayNewLine()


                ElseIf MyCommand.Keyword = eCommandType.setting.ToString AndAlso StringInList(MySwitches, "noswitches") Then

                    Textify.SayBulletLine(Textify.eBullet.Hash, "text editor switches cleared")

                    UserSettings.TextEditorSwitches = ""
                    My.Configger.SaveSetting(NameOf(UserSettings.TextEditorSwitches), "")
                    Textify.SayNewLine()


                ElseIf MyCommand.Keyword = eCommandType.explore.ToString Then

                    OpenExplorer(WildcardInterpreter(UserInput, False), WorkingFolder)





                ElseIf MyCommand.Keyword = eCommandType.[use].ToString Then

                    LoadFolder(UserInput, WorkingFolder)


                ElseIf MyCommand.Keyword = eCommandType.[usetheforce].ToString Then

                    If StringInList(MySwitches, "e") Then

                        Textify.SayBullet(Textify.eBullet.Hash, "Easter eggs found so far:")
                        Textify.SayNewLine()
                        For Each fc As ForceCommand In StarWars.ForceCommands.Where(Function(x) x.Found)
                            Textify.SayBullet(Textify.eBullet.Arrow, fc.Keyword)
                        Next
                        If Not StarWars.ForceCommands.Exists(Function(x) x.Found) Then
                            Textify.SayBullet(Textify.eBullet.Arrow, "Commander, tear this app apart until you've found those eggs!")
                        End If
                        Textify.SayNewLine()
                        Textify.SayBullet(Textify.eBullet.Hash, "(Found: " & StarWars.FoundCount & " of " & StarWars.PossibleCount & ")")
                        Textify.SayNewLine()

                    Else
                        UseTheForce(UserInput.ToLower)
                    End If


                ElseIf MyCommand.Keyword = eCommandType.connection.ToString AndAlso StringInList(MySwitches, "set") AndAlso Not String.IsNullOrEmpty(UserInput) Then
                    SetConnectionString(WorkingFolder, UserInput)
                ElseIf MyCommand.Keyword = eCommandType.connection.ToString AndAlso StringInList(MySwitches, "show") AndAlso String.IsNullOrEmpty(UserInput) Then
                    Textify.SayBulletLine(Textify.eBullet.Arrow, GetConnectionString(WorkingFolder))
                    Textify.SayNewLine()
                ElseIf MyCommand.Keyword = eCommandType.connection.ToString AndAlso (StringInList(MySwitches, "t") OrElse MySwitches.Count = 0) AndAlso String.IsNullOrEmpty(UserInput) Then
                    TestConnectionString(WorkingFolder)
                ElseIf MyCommand.Keyword = eCommandType.connection.ToString AndAlso StringInList(MySwitches, "forget") AndAlso String.IsNullOrEmpty(UserInput) Then
                    ForgetConnectionString(WorkingFolder)
                ElseIf MyCommand.Keyword = eCommandType.connection.ToString AndAlso StringInList(MySwitches, "e") AndAlso String.IsNullOrEmpty(UserInput) Then
                    Dim cs As String
                    Try
                        cs = GetConnectionString(WorkingFolder)
                    Catch ex As Exception
                        cs = cDefaultConnectionString
                    End Try
                    cs = Microsoft.VisualBasic.Interaction.InputBox("Connection String", "", cs)
                    If Not String.IsNullOrWhiteSpace(cs) Then
                        SetConnectionString(WorkingFolder, cs)
                        Textify.SayBulletLine(Textify.eBullet.Arrow, cs)
                        Textify.SayNewLine()
                    End If


                ElseIf MyCommand.Keyword = eCommandType.[nuke].ToString Then

                    NukeFiles(WorkingFolder, UserInput)


                ElseIf MyCommand.Keyword = eCommandType.make.ToString Then

                    If StringInList(MySwitches, "nosave") Then

                        NukeFiles(WorkingFolder, "*" & MyConstants.AutocreateFilename & "*")
                        Automagic(GetConnectionString(WorkingFolder), WorkingFolder, StringInList(MySwitches, "r"), Not StringInList(MySwitches, "nocomment"))
                        Dim SelectedFiles As List(Of String) = FilesToProcess(WorkingFolder, "*" & MyConstants.AutocreateFilename & "*", String.Empty, False, ObjectTypeFilter, False, False, False)
                        ProcessFiles(SelectedFiles, eFileAction.generate, eMode.normal, SquealerObjectType.eType.Invalid, New GitFlags(), False)
                        NukeFiles(WorkingFolder, "*" & MyConstants.AutocreateFilename & "*")

                    ElseIf StringInList(MySwitches, "nuke") Then
                        NukeFiles(WorkingFolder, "*" & MyConstants.AutocreateFilename & "*")
                    Else
                        Automagic(GetConnectionString(WorkingFolder), WorkingFolder, StringInList(MySwitches, "r"), Not StringInList(MySwitches, "nocomment"))
                    End If


                ElseIf MyCommand.Keyword = "test" Then

                    For Each s As String In GitChangedFiles(WorkingFolder, "git status -s")
                        Textify.WriteLine(s)
                    Next






                    ' footest


                    'Textify.SayNewLine()
                    'Textify.SayNewLine()
                    'Dim command As String = "cmd.exe"
                    'Dim arguments As String = String.Format("/c git symbolic-ref HEAD")
                    'Textify.Write(String.Format("{0} {1}", command, arguments))
                    'Dim ps As New ProcessStartInfo
                    'ps.WorkingDirectory = WorkingFolder
                    'ps.FileName = command
                    'ps.Arguments = arguments
                    'ps.WindowStyle = ProcessWindowStyle.Hidden
                    'Dim oProcess As New Process()
                    'ps.RedirectStandardOutput = True
                    'ps.UseShellExecute = False
                    'oProcess.StartInfo = ps
                    'oProcess.Start()
                    'Textify.SayNewLine()
                    'Textify.SayNewLine()

                    'Dim sOutput As String
                    'Using oStreamReader As System.IO.StreamReader = oProcess.StandardOutput
                    '    sOutput = oStreamReader.ReadToEnd()
                    'End Using
                    'Console.WriteLine(sOutput)

                    'Textify.SayNewLine()
                    'Textify.SayNewLine()



                    Textify.SayBullet(Textify.eBullet.Hash, "--for debugging only - hidden command--")
                    Textify.SayBullet(Textify.eBullet.Hash, "random guid: " & Guid.NewGuid.ToString)

                    Dim fileName$ = System.Reflection.Assembly.GetExecutingAssembly().Location
                    'Dim fileName2$ = Application.ExecutablePath ' this grabs the filename too,
                    ' but #2 ends with ".EXE" while the 1st ends with ".exe" in my Windows 7.
                    ' either fileName or fileName2 works for me.
                    Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(fileName)
                    ' now this fvi has all the properties for the FileVersion information.
                    Dim fvAsString$ = fvi.FileVersion ' but other useful properties exist too.
                    'Console.WriteLine(fvAsString$)



                    If StringInList(MySwitches, "release") Then
                        Dim s As String = WorkingFolder & "\ver.txt"
                        My.Computer.FileSystem.WriteAllText(s, My.Application.Info.Version.ToString, False)
                        Console.WriteLine()
                        Console.WriteLine()
                        Console.WriteLine("generated " & s & " with " & My.Application.Info.Version.ToString)
                        Console.WriteLine()
                    End If


                    'dbcc opentran (odindev15) with tableresults

                    'Using DbTables As SqlClient.SqlConnection = New SqlClient.SqlConnection(GetConnectionString(WorkingFolder))
                    '    DbTables.Open()
                    '    Dim TableReader As SqlClient.SqlDataReader = New SqlClient.SqlCommand("dbcc opentran with tableresults", DbTables).ExecuteReader
                    '    If TableReader.HasRows Then
                    '        Console.WriteLine("uncommitted transactions")
                    '    Else
                    '        Console.WriteLine("all good")
                    '    End If
                    'End Using






                Else
                    Throw New System.Exception(BadCommandMessage)
                End If

            Catch ex As Exception

                Textify.SayError(ex.Message)

                If MyCommand Is Nothing Then
                    Textify.SayBulletLine(Textify.eBullet.Hash, "Try: HELP")
                Else
                    Textify.SayBulletLine(Textify.eBullet.Hash, "Try: HELP " & MyCommand.Keyword.ToUpper)
                End If

                Textify.SayNewLine()

                'Textify.Write(MyCommand.Keyword)
                'For Each s As String In MySwitches
                '    Textify.Write(s)
                'Next

                'Textify.Write(UserInput)


            End Try

            Dim ProjectName As String = GetProjectNickname(WorkingFolder)

            Console.Title = String.Format("[{0}] {1} - {2}", ProjectName, WorkingFolder, My.Application.Info.Title) ' Info may have changed. Update the title bar on every pass. 

            Textify.Write(String.Format("{1}[{0}]", ProjectName, IIf(UserSettings.ShowClock, Now.ToString("(hh:mmtt) "), "")), ConsoleColor.DarkYellow)
            If UserSettings.ShowBranch Then
                Textify.Write(CurrentBranch(WorkingFolder, " ({0})"), ConsoleColor.DarkGreen)
            End If
            Textify.Write(" > ", ConsoleColor.DarkYellow)
            ClearKeyboard()
            UserInput = Console.ReadLine
            Textify.SayNewLine()

            ' Separate command text from search text
            If UserInput.Contains("/") Then
                Dim n As Integer = UserInput.IndexOf("/")
                MySearchText = UserInput.Substring(n + 1)
                UserInput = UserInput.Substring(0, n)
            Else
                MySearchText = String.Empty
            End If


            Dim SplitInput As New List(Of String)
            SplitInput.AddRange(UserInput.Trim.Split(New Char() {" "c}))
            UserInput = String.Empty
            MySwitches.Clear()

            ' Go through each piece of the user command and pull out all the switches.
            While SplitInput.Count > 0

                Dim rawinput As String = SplitInput(0)

                If rawinput.StartsWith("-") Then ' -Ex:Opt:JUNK

                    Dim switchinput As String = rawinput.Remove(0, 1).ToLower ' -Ex:Opt:JUNK -> ex:opt:junk

                    Dim switchkeyword As String = switchinput.Split(New Char() {":"c})(0) ' ex:opt:junk -> ex
                    Dim switchoption As String = String.Empty
                    If switchinput.Contains(":") Then
                        switchoption = ":" & switchinput.Split(New Char() {":"c})(1) ' ex:opt:junk -> :opt
                    End If

                    MySwitches.Add(switchkeyword & switchoption)

                Else
                    UserInput &= " " & rawinput
                End If

                SplitInput.RemoveAt(0)

            End While


            ' Separate the command from everything after it
            UserInput = UserInput.Trim
            If String.IsNullOrEmpty(UserInput) Then
                MyCommand = MyCommands.FindCommand(eCommandType.nerfherder.ToString)
            Else
                Dim keyword As String = UserInput.Split(New Char() {" "c})(0)
                MyCommand = MyCommands.FindCommand(keyword)
                UserInput = UserInput.Remove(0, keyword.Length).Trim
            End If


            ' Test the switches
            SwitchesValidated = True
            If MyCommand IsNot Nothing Then
                For Each s As String In MySwitches

                    Dim opt As CommandCatalog.CommandSwitch = MyCommand.Options.Items.Find(Function(x) x.Keyword = s.Split(New Char() {":"c})(0))

                    If opt Is Nothing Then ' Is the switch legit?
                        SwitchesValidated = False
                    Else

                        If opt.Options.Items.Count > 0 AndAlso Not s.Contains(":") Then ' Did user omit required switch option?
                            SwitchesValidated = False
                        End If

                        If s.Contains(":") Then
                            If Not opt.Options.Items.Exists(Function(x) x.Keyword = s.Split(New Char() {":"c})(1)) Then ' Is the switch option legit?
                                SwitchesValidated = False
                            End If
                        End If

                    End If
                Next
            End If

            ' Move object types from the switch list to the object type list
            ObjectTypeFilter.SetAllFlags(False)

            While MySwitches.Exists(Function(x) SquealerObjectType.Validated(x))
                Dim t As SquealerObjectType.eType = SquealerObjectType.Eval(MySwitches.Find(Function(x) Not SquealerObjectType.Eval(x) = SquealerObjectType.eType.Invalid))
                ObjectTypeFilter.SetOneFlag(t, True)
                MySwitches.Remove(SquealerObjectType.ToShortType(t).ToString)
            End While


            If ObjectTypeFilter.NoneSelected Then
                ObjectTypeFilter.SetAllFlags(True)
            End If

        End While

    End Sub

    Private Function GetFileList(ByVal WhichFolder As String) As List(Of String)

        Dim dialog As New OpenFileDialog

        dialog.FileName = "" ' Default file name
        dialog.DefaultExt = ".sqlr" ' Default file extension
        dialog.Filter = "Text Files (*.sqlr)|*.sqlr"
        dialog.Multiselect = True
        dialog.RestoreDirectory = True
        dialog.InitialDirectory = WhichFolder

        ' Show open file dialog box
        dialog.ShowDialog()

        Dim fnames As New List(Of String)
        For Each fn As String In dialog.FileNames
            fnames.Add(fn)
        Next

        Return fnames

    End Function

#End Region

#Region " Settings "

    Private Sub SettingViewOne(name As String, value As String)
        Textify.SayBullet(Textify.eBullet.Arrow, "")
        Textify.Write(name, ConsoleColor.Green)
        Textify.Write(": ")
        Textify.WriteLine(value, ConsoleColor.White)
        Console.WriteLine()
    End Sub

    Private Sub SettingsView()

        SettingViewOne(NameOf(UserSettings.AutoSearch), UserSettings.AutoSearch.ToString)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Set TRUE for automatic wildcards, FALSE for strict filename searches.")
        Textify.SayBulletLine(Textify.eBullet.Hash, "When set to true, filename input is treated as if surrounded by asterisks (ex: *filename*).")
        Textify.SayNewLine()
        SettingViewOne(NameOf(UserSettings.WildcardSpaces), UserSettings.WildcardSpaces.ToString)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Set TRUE to treat spaces as asterisks, FALSE for literal spaces.")
        Textify.SayBulletLine(Textify.eBullet.Hash, "When set to true, spaces in filenames are treated as wildcards (ex: ""foo bar"" = ""foo*bar"".")
        Textify.SayNewLine()
        SettingViewOne(NameOf(UserSettings.ShowClock), UserSettings.ShowClock.ToString)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Set TRUE to show the clock, FALSE to hide.")
        Textify.SayBulletLine(Textify.eBullet.Hash, "When set to true, the clock will display in the command prompt.")
        Textify.SayNewLine()
        SettingViewOne(NameOf(UserSettings.EditNew), UserSettings.EditNew.ToString)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Set TRUE to edit new files, FALSE to create only.")
        Textify.SayBulletLine(Textify.eBullet.Hash, "When set to true, new files will automatically be opened in the configured text editor.")
        Textify.SayNewLine()
        SettingViewOne(NameOf(UserSettings.UseClipboard), UserSettings.UseClipboard.ToString)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Set TRUE to save output to clipboard, FALSE to open in text editor.")
        Textify.SayBulletLine(Textify.eBullet.Hash, "When set to true, output will be copied directly to the Windows clipboard instead of into a temp file.")
        Textify.SayNewLine()
        SettingViewOne(NameOf(UserSettings.TextEditor), UserSettings.TextEditor)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Specify the full path and file name of your preferred text editor.")
        Textify.SayBulletLine(Textify.eBullet.Hash, "The path may be omitted if the executable is in your Windows environment search path.")
        Textify.SayNewLine()
        SettingViewOne(NameOf(UserSettings.TextEditorSwitches), UserSettings.TextEditorSwitches)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Specify any switches (optional) you want to pass to your text editor.")
        Textify.SayBulletLine(Textify.eBullet.Hash, UserSettings.TextEditor & " " & PadRightIfNotEmpty(UserSettings.TextEditorSwitches) & "<FileName>")
        Textify.SayNewLine()
        SettingViewOne(NameOf(UserSettings.ShowBranch), UserSettings.ShowBranch.ToString)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Set TRUE to show the branch, FALSE to hide.")
        Textify.SayBulletLine(Textify.eBullet.Hash, "When set to true, the checked-out Git branch will display in the command prompt.")
        Textify.SayNewLine()
        SettingViewOne(NameOf(UserSettings.RecentFolders), UserSettings.RecentFolders.ToString)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Set the maximum number of recent folders to remember.")
        Textify.SayBulletLine(Textify.eBullet.Hash, "When set to 0, no folders will be remembered.")
        Textify.SayNewLine()
        SettingViewOne(NameOf(UserSettings.DirStyle), UserSettings.DirStyle.ToString)
        Textify.SayBulletLine(Textify.eBullet.Hash, "Set the directory style.")
        Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("Available styles are '{0}', '{1}', and '{2}'.", eDirectoryStyle.compact.ToString, eDirectoryStyle.full.ToString, eDirectoryStyle.symbolic.ToString))
        Textify.SayNewLine()
        Textify.SayBulletLine(Textify.eBullet.Hash, "Try: " & eCommandType.help.ToString.ToUpper & " " & eCommandType.setting.ToString.ToUpper)
        Textify.SayNewLine()

    End Sub

    Private Sub SettingChange(ByVal name As String, ByVal value As String)

        Dim previous As String = String.Empty

        Select Case name.ToLower

            Case NameOf(Textify.ErrorAlert.Beep).ToLower
                previous = Textify.ErrorAlert.Beep.ToString
                Textify.ErrorAlert.Beep = CBool(value)
                My.Configger.SaveSetting(NameOf(Textify.ErrorAlert.Beep), CBool(value))

            Case NameOf(UserSettings.DirStyle).ToLower
                previous = UserSettings.DirStyle.ToString
                If ValidDirectoryStyle(value) Then
                    UserSettings.DirStyle = value
                    My.Configger.SaveSetting(NameOf(UserSettings.DirStyle), value)
                Else
                    Textify.SayError("Invalid directory style: " & value, Textify.eSeverity.warning)
                    Textify.SayNewLine()
                    value = previous
                End If

            Case NameOf(UserSettings.TextEditor).ToLower
                previous = UserSettings.TextEditor
                My.Configger.SaveSetting(name, value)
                UserSettings.TextEditor = value
                If Not My.Computer.FileSystem.FileExists(value) Then
                    Textify.SayError("Cannot find " & value, Textify.eSeverity.warning)
                    Textify.SayNewLine()
                End If

            Case NameOf(UserSettings.ShowClock).ToLower
                previous = UserSettings.ShowClock.ToString
                UserSettings.ShowClock = CBool(value)
                My.Configger.SaveSetting(NameOf(UserSettings.ShowClock), CBool(value))

            Case NameOf(UserSettings.EditNew).ToLower
                previous = UserSettings.EditNew.ToString
                UserSettings.EditNew = CBool(value)
                My.Configger.SaveSetting(NameOf(UserSettings.EditNew), CBool(value))

            Case NameOf(UserSettings.RecentFolders).ToLower
                previous = UserSettings.RecentFolders.ToString
                UserSettings.RecentFolders = CInt(value)
                My.Configger.SaveSetting(NameOf(UserSettings.RecentFolders), CInt(value))

            Case NameOf(UserSettings.WildcardSpaces).ToLower
                previous = UserSettings.WildcardSpaces.ToString
                UserSettings.WildcardSpaces = CBool(value)
                My.Configger.SaveSetting(NameOf(UserSettings.WildcardSpaces), CBool(value))

            Case NameOf(UserSettings.ShowBranch).ToLower
                previous = UserSettings.ShowBranch.ToString
                UserSettings.ShowBranch = CBool(value)
                My.Configger.SaveSetting(NameOf(UserSettings.ShowBranch), CBool(value))

            Case NameOf(UserSettings.AutoSearch).ToLower
                previous = UserSettings.AutoSearch.ToString
                UserSettings.AutoSearch = CBool(value)
                My.Configger.SaveSetting(NameOf(UserSettings.AutoSearch), CBool(value))

            Case NameOf(UserSettings.UseClipboard).ToLower
                previous = UserSettings.UseClipboard.ToString
                UserSettings.UseClipboard = CBool(value)
                My.Configger.SaveSetting(NameOf(UserSettings.UseClipboard), CBool(value))

        End Select

        Textify.SayBulletLine(Textify.eBullet.Hash, "old setting: " & previous)
        Textify.SayBulletLine(Textify.eBullet.Hash, "new setting: " & value)

    End Sub

#End Region

#Region " XML Default Values "

    ' If the first parameter is null, return the second.
    Private Function AttributeDefaultBoolean(ByVal attr As Xml.XmlNode, ByVal deefalt As Boolean) As Boolean

        If attr Is Nothing Then
            Return deefalt
        Else
            Return CBool(IIf(String.IsNullOrWhiteSpace(attr.Value), deefalt, attr.Value))
        End If

    End Function

    ' If the first parameter is null, return the second.
    Private Function AttributeDefaultString(ByVal attr As Xml.XmlNode, ByVal deefalt As String) As String

        If attr Is Nothing Then
            Return deefalt
        Else
            Return IIf(String.IsNullOrWhiteSpace(attr.Value), deefalt, attr.Value).ToString
        End If

    End Function

    ' If the first parameter is null, return the second.
    Private Function AttributeDefaultInteger(ByVal attr As Xml.XmlNode, ByVal deefalt As Integer) As Integer

        If attr Is Nothing Then
            Return deefalt
        Else
            Return Integer.Parse(IIf(String.IsNullOrWhiteSpace(attr.Value), deefalt, attr.Value).ToString)
        End If

    End Function

#End Region

#Region " XML Reading "

    Private Function XmlGetObjectType(ByVal FileName As String) As String

        Try
            Dim Reader As New Xml.XmlDocument
            Reader.Load(FileName)
            Dim Node As Xml.XmlNode = Reader.SelectSingleNode("/" & My.Application.Info.ProductName)

            ' Get the type.
            Return Node.Attributes("Type").Value.ToString
        Catch ex As Exception
            Return SquealerObjectType.eShortType.err.ToString
        End Try

    End Function

    ' Get all the parameters.
    Private Function GetParameters(ByVal InXml As Xml.XmlDocument) As DataTable

        Dim Parameters As New DataTable

        With Parameters.Columns
            .Add("Name", GetType(String))
            .Add("Type", GetType(String))
            .Add("Output", GetType(Boolean))
            .Add("DefaultValue", GetType(String))
            .Add("Comments", GetType(String))
        End With

        For Each Node As Xml.XmlNode In InXml.SelectNodes(My.Application.Info.ProductName & "/Parameters/Parameter")

            Parameters.Rows.Add(
                AttributeDefaultString(Node.Attributes.GetNamedItem("Name"), String.Empty),
                AttributeDefaultString(Node.Attributes.GetNamedItem("Type"), String.Empty),
                AttributeDefaultString(Node.Attributes.GetNamedItem("Output"), Boolean.FalseString),
                AttributeDefaultString(Node.Attributes.GetNamedItem("DefaultValue"), String.Empty),
                AttributeDefaultString(Node.Attributes.GetNamedItem("Comments"), String.Empty)
            )

        Next

        GetParameters = Parameters

    End Function

    ' Get all the table columns.
    Private Function GetColumns(ByVal InXml As Xml.XmlDocument) As DataTable

        Dim Columns As New DataTable

        With Columns.Columns
            .Add("Name", GetType(String))
            .Add("Type", GetType(String))
            .Add("Nullable", GetType(Boolean))
            .Add("Identity", GetType(Boolean))
            .Add("IncludeInPrimaryKey", GetType(Boolean))
            .Add("Comments", GetType(String))
        End With

        For Each Node As Xml.XmlNode In InXml.SelectNodes(My.Application.Info.ProductName & "/Table/Column")

            Columns.Rows.Add(
                    AttributeDefaultString(Node.Attributes.GetNamedItem("Name"), String.Empty),
                    AttributeDefaultString(Node.Attributes.GetNamedItem("Type"), String.Empty),
                    AttributeDefaultBoolean(Node.Attributes.GetNamedItem("Nullable"), True),
                    AttributeDefaultBoolean(Node.Attributes.GetNamedItem("Identity"), False),
                    AttributeDefaultBoolean(Node.Attributes.GetNamedItem("IncludeInPrimaryKey"), False),
                    AttributeDefaultString(Node.Attributes.GetNamedItem("Comments"), String.Empty)
                )
        Next

        GetColumns = Columns

    End Function

    ' Get all the users.
    Private Function GetUsers(ByVal InXml As Xml.XmlDocument) As DataTable

        Dim Users As New DataTable

        With Users.Columns
            .Add("Name", GetType(String))
        End With

        For Each Node As Xml.XmlNode In InXml.SelectNodes(My.Application.Info.ProductName & "/Users/User")

            Users.Rows.Add(AttributeDefaultString(Node.Attributes.GetNamedItem("Name"), String.Empty))

        Next

        GetUsers = Users

    End Function

#End Region

#Region " XML Processing "

    ' Load and clean up the XML keeping the original file type.
    Private Function FixedXml(ByVal ApplyDefaultUsers As Boolean, fqfn As String, MakePretty As Boolean) As Xml.XmlDocument
        Dim obj As New SquealerObject(fqfn)
        Return FixedXml(ApplyDefaultUsers, fqfn, obj, MakePretty)
    End Function

    ' Load and clean up the XML using the specified target file type.
    Private Function FixedXml(ByVal ApplyDefaultUsers As Boolean, fqfn As String, ByVal obj As SquealerObject, ByVal MakePretty As Boolean) As Xml.XmlDocument

        Dim OutputXml As Xml.XmlDocument = New Xml.XmlDocument

        Dim InputXml As Xml.XmlDocument = New Xml.XmlDocument

        InputXml.Load(fqfn)

        Dim InRoot As Xml.XmlElement = DirectCast(InputXml.SelectSingleNode(My.Application.Info.ProductName), Xml.XmlElement)

        OutputXml.AppendChild(OutputXml.CreateXmlDeclaration("1.0", "us-ascii", Nothing))

        ' Header
        OutputXml.AppendChild(OutputXml.CreateComment(" Flags example: ""x;exclude from project|r;needs refactoring"" (recommend single-character flags) "))

        Dim OutRoot As Xml.XmlElement = OutputXml.CreateElement(My.Application.Info.ProductName)
        OutputXml.AppendChild(OutRoot)

        OutRoot.SetAttribute("Type", obj.Type.LongType.ToString)
        OutRoot.SetAttribute("Flags", obj.Flags)
        OutRoot.SetAttribute("WithOptions", obj.WithOptions)


        ' Pre-Code.
        Dim OutPreCode As Xml.XmlElement = OutputXml.CreateElement("PreCode")
        Dim CDataPreCode As Xml.XmlCDataSection = OutputXml.CreateCDataSection("") ' CData disables the XML parser so that special characters can exist in the inner text.

        OutRoot.AppendChild(OutputXml.CreateComment(" Optional T-SQL to execute before the main object is created. "))
        OutRoot.AppendChild(OutPreCode)

        Dim InPreCode As String = String.Empty
        Try
            InPreCode = InRoot.SelectSingleNode("PreCode").InnerText
        Catch ex As Exception
            InPreCode = ""
        End Try

        CDataPreCode.InnerText = String.Concat(vbCrLf, vbCrLf, InPreCode.Trim, vbCrLf, vbCrLf)
        OutPreCode.AppendChild(CDataPreCode)


        ' Comments.
        ' -- comment help--
        Dim CommentHelp As Xml.XmlComment = Nothing

        Select Case obj.Type.LongType
            Case SquealerObjectType.eType.StoredProcedure
                CommentHelp = OutputXml.CreateComment(" Describe the purpose of this procedure, the return values, and any difficult concepts. ")
            Case SquealerObjectType.eType.InlineTableFunction, SquealerObjectType.eType.MultiStatementTableFunction
                CommentHelp = OutputXml.CreateComment(" Describe the output of this view and any difficult concepts. ")
            Case SquealerObjectType.eType.ScalarFunction
                CommentHelp = OutputXml.CreateComment(" Describe the output of this scalar function and any difficult concepts. ")
            Case SquealerObjectType.eType.View
                CommentHelp = OutputXml.CreateComment(" Describe the output of this view and any difficult concepts. ")
            Case Else
                Throw New Exception("Missing or invalid object type. Check: <Squealer Type=""???""> in " & fqfn)
        End Select
        OutRoot.AppendChild(CommentHelp)

        ' -- actual comment--
        Dim OutComments As Xml.XmlElement = OutputXml.CreateElement("Comments")
        Dim CDataComment As Xml.XmlCDataSection = OutputXml.CreateCDataSection("") ' CData disables the XML parser so that special characters can exist in the inner text.
        OutRoot.AppendChild(OutComments)

        Try
            CDataComment.InnerText = InRoot.SelectSingleNode("Comments").InnerText.Replace("/*", String.Empty).Replace("*/", String.Empty)
        Catch ex As Exception
            CDataComment.InnerText = String.Empty
        End Try

        CDataComment.InnerText = String.Concat(vbCrLf, vbCrLf, CDataComment.InnerText.Trim, vbCrLf, vbCrLf)

        OutComments.AppendChild(CDataComment)


        ' Parameters.
        If Not obj.Type.LongType = SquealerObjectType.eType.View Then

            Dim OutParameters As Xml.XmlElement = OutputXml.CreateElement("Parameters")
            OutRoot.AppendChild(OutParameters)

            Dim InParameters As DataTable = GetParameters(InputXml)

            If InParameters.Rows.Count = 0 Then
                OutParameters.AppendChild(OutputXml.CreateComment("<Parameter Name=""MyParameter"" Type=""varchar(50)"" " & IIf(obj.Type.LongType = SquealerObjectType.eType.StoredProcedure, "Output=""False"" ", String.Empty).ToString & "DefaultValue="""" Comments="""" />"))
            Else
                For Each InParameter As DataRow In InParameters.Select()
                    Dim OutParameter As Xml.XmlElement = OutputXml.CreateElement("Parameter")
                    OutParameter.SetAttribute("Name", InParameter.Item("Name").ToString)
                    OutParameter.SetAttribute("Type", InParameter.Item("Type").ToString)
                    If obj.Type.LongType = SquealerObjectType.eType.StoredProcedure Then
                        OutParameter.SetAttribute("Output", InParameter.Item("Output").ToString)
                    End If
                    OutParameter.SetAttribute("DefaultValue", InParameter.Item("DefaultValue").ToString)
                    OutParameter.SetAttribute("Comments", InParameter.Item("Comments").ToString)
                    OutParameters.AppendChild(OutParameter)
                Next
            End If

        End If

        ' Returns.
        If obj.Type.LongType = SquealerObjectType.eType.ScalarFunction Then
            OutRoot.AppendChild(OutputXml.CreateComment(" Define the data type For @Result, your scalar Return variable. "))
            Dim OutReturns As Xml.XmlElement = OutputXml.CreateElement("Returns")
            OutRoot.AppendChild(OutReturns)
            Dim Returns As String = Nothing
            Try
                Returns = DirectCast(InRoot.SelectSingleNode("Returns"), Xml.XmlElement).GetAttribute("Type")
            Catch ex As Exception
                Returns = String.Empty
            End Try
            OutReturns.SetAttribute("Type", Returns)
        End If

        ' Table.
        If obj.Type.LongType = SquealerObjectType.eType.MultiStatementTableFunction OrElse obj.Type.LongType = SquealerObjectType.eType.View Then

            If obj.Type.LongType = SquealerObjectType.eType.MultiStatementTableFunction Then
                OutRoot.AppendChild(OutputXml.CreateComment(" Define the column(s) for @TableValue, your table-valued return variable. "))
            Else
                OutRoot.AppendChild(OutputXml.CreateComment(" Define the column(s) to return from this view. "))
            End If
            Dim OutTable As Xml.XmlElement = OutputXml.CreateElement("Table")
            OutRoot.AppendChild(OutTable)

            If obj.Type.LongType = SquealerObjectType.eType.MultiStatementTableFunction Then
                Dim Clustered As Boolean = False
                Try
                    Clustered = CBool(DirectCast(InRoot.SelectSingleNode("Table"), Xml.XmlElement).GetAttribute("PrimaryKeyClustered"))
                Catch ex As Exception
                    Clustered = False
                End Try
                OutTable.SetAttribute("PrimaryKeyClustered", Clustered.ToString)
            End If

            Dim InColumns As DataTable = GetColumns(InputXml)

            If InColumns.Rows.Count = 0 Then
                ' Create a dummy/example column.
                If obj.Type.LongType = SquealerObjectType.eType.MultiStatementTableFunction Then
                    Dim OutColumn As Xml.XmlElement = OutputXml.CreateElement("Column")
                    OutColumn.SetAttribute("Name", "MyColumn")
                    OutColumn.SetAttribute("Type", "varchar(50)")
                    OutColumn.SetAttribute("Nullable", "False")
                    OutColumn.SetAttribute("Identity", "False")
                    OutColumn.SetAttribute("IncludeInPrimaryKey", "False")
                    OutColumn.SetAttribute("Comments", "")
                    OutTable.AppendChild(OutColumn)
                Else
                    OutTable.AppendChild(OutputXml.CreateComment("<Column Name=""MyColumn"" Comments="""" />"))
                End If
            Else
                For Each InColumn As DataRow In InColumns.Select()
                    Dim Nullable As Boolean = CBool(InColumn.Item("Nullable"))
                    Dim Identity As Boolean = CBool(InColumn.Item("Identity"))
                    Dim IncludeInPrimaryKey As Boolean = CBool(InColumn.Item("IncludeInPrimaryKey"))
                    Dim Type As String = InColumn.Item("Type").ToString
                    If Type.IndexOf(" Not null") > 0 Then
                        Nullable = False
                        Type = Type.Replace(" Not null", String.Empty)
                    ElseIf Type.IndexOf(" null") > 0 Then
                        Nullable = True
                        Type = Type.Replace(" null", String.Empty)
                    End If
                    Dim OutColumn As Xml.XmlElement = OutputXml.CreateElement("Column")
                    OutColumn.SetAttribute("Name", InColumn.Item("Name").ToString)
                    If obj.Type.LongType = SquealerObjectType.eType.MultiStatementTableFunction Then
                        OutColumn.SetAttribute("Type", Type)
                        OutColumn.SetAttribute("Nullable", Nullable.ToString)
                        OutColumn.SetAttribute("Identity", Identity.ToString)
                        OutColumn.SetAttribute("IncludeInPrimaryKey", IncludeInPrimaryKey.ToString)
                    End If
                    OutColumn.SetAttribute("Comments", InColumn.Item("Comments").ToString)
                    OutTable.AppendChild(OutColumn)
                Next
            End If

        End If

        ' Code.
        Dim OutCode As Xml.XmlElement = OutputXml.CreateElement("Code")
        Dim CDataCode As Xml.XmlCDataSection = OutputXml.CreateCDataSection("") ' CData disables the XML parser so that special characters can exist in the inner text.

        OutRoot.AppendChild(OutCode)

        Dim InCode As String = String.Empty
        Try
            InCode = InRoot.SelectSingleNode("Code").InnerText
            If MakePretty Then
                InCode = BeautifiedCode(InCode)
            End If
        Catch ex As Exception
            InCode = "Code."
        End Try

        'If InCode.Trim = String.Empty Then
        'End If

        CDataCode.InnerText = String.Concat(vbCrLf, vbCrLf, InCode.Trim, vbCrLf, vbCrLf)
        OutCode.AppendChild(CDataCode)


        ' Users.
        Dim OutUsers As Xml.XmlElement = OutputXml.CreateElement("Users")
        OutRoot.AppendChild(OutUsers)
        Dim InUsers As DataTable
        If ApplyDefaultUsers Then
            InUsers = GetDefaultUsers(My.Computer.FileSystem.GetFileInfo(fqfn).DirectoryName)
        Else
            InUsers = GetUsers(InputXml)
        End If

        If InUsers.Rows.Count = 0 Then
            OutUsers.AppendChild(OutputXml.CreateComment(" <User Name= ""MyUser""/> "))
        Else
            For Each User As DataRow In InUsers.Select("", "Name asc")
                Dim OutUser As Xml.XmlElement = OutputXml.CreateElement("User")
                OutUser.SetAttribute("Name", User.Item("Name").ToString)
                OutUsers.AppendChild(OutUser)
            Next
        End If


        ' Post-Code.
        Dim OutPostCode As Xml.XmlElement = OutputXml.CreateElement("PostCode")
        Dim CDataPostCode As Xml.XmlCDataSection = OutputXml.CreateCDataSection("") ' CData disables the XML parser so that special characters can exist in the inner text.

        OutRoot.AppendChild(OutputXml.CreateComment(" Optional T-SQL to execute after the main object is created. "))
        OutRoot.AppendChild(OutPostCode)

        Dim InPostCode As String = String.Empty
        Try
            InPostCode = InRoot.SelectSingleNode("PostCode").InnerText
        Catch ex As Exception
            InPostCode = ""
        End Try

        CDataPostCode.InnerText = String.Concat(vbCrLf, vbCrLf, InPostCode.Trim, vbCrLf, vbCrLf)
        OutPostCode.AppendChild(CDataPostCode)


        FixedXml = OutputXml

    End Function

    Private Function BeautifiedCode(code As String) As String

        Dim options As New PoorMansTSqlFormatterLib.Formatters.TSqlStandardFormatterOptions
        With options
            .BreakJoinOnSections = True
            .ExpandBetweenConditions = True
            .ExpandBooleanExpressions = True
            .ExpandCaseStatements = True
            .ExpandCommaLists = True
            .ExpandInLists = True
            .IndentString = vbTab
            .KeywordStandardization = True
            .NewClauseLineBreaks = 1
            .NewStatementLineBreaks = 2
            .SpacesPerTab = 8
            .TrailingCommas = False
            .UppercaseKeywords = False
        End With

        Dim formatter As New PoorMansTSqlFormatterLib.Formatters.TSqlStandardFormatter(options)

        Dim beautify As New PoorMansTSqlFormatterLib.SqlFormattingManager(formatter)

        code = beautify.Format(code)

        Return code

    End Function

    ' Fix a root file and replace the original.
    Private Function ConvertXmlFile(fqfn As String, ByVal oType As SquealerObjectType.eType, MakePretty As Boolean) As Boolean

        Dim obj As New SquealerObject(fqfn)
        obj.Type.LongType = oType
        Dim NewXml As Xml.XmlDocument = FixedXml(False, fqfn, obj, MakePretty) ' Fix it.

        Return IsXmlReplaced(fqfn, NewXml)

    End Function

    ' Fix a root file and replace the original.
    Private Function RepairXmlFile(ByVal IsNew As Boolean, fqfn As String, MakePretty As Boolean) As Boolean
        Dim NewXml As Xml.XmlDocument = FixedXml(IsNew, fqfn, MakePretty) ' Fix it.
        Return IsXmlReplaced(fqfn, NewXml)
    End Function

    Private Function IsXmlReplaced(existingfilename As String, newxml As Xml.XmlDocument) As Boolean

        Dim ExistingXml As New Xml.XmlDocument
        ExistingXml.Load(existingfilename)

        Dim different As Boolean = False

        If Not newxml.InnerXml = ExistingXml.InnerXml Then
            ' Is XML different?
            newxml.Save(existingfilename)
            different = True
        Else
            ' XML is the same, but is whitespace different?
            Dim tempfilename As String = My.Computer.FileSystem.GetTempFileName
            newxml.Save(tempfilename)
            If Not My.Computer.FileSystem.ReadAllText(tempfilename) = My.Computer.FileSystem.ReadAllText(existingfilename) Then
                My.Computer.FileSystem.MoveFile(tempfilename, existingfilename, True)
                different = True
            End If
        End If

        Return different

    End Function

    ' Create a new proc or function.
    Private Function CreateNewFile(ByVal WorkingFolder As String, ByVal FileType As SquealerObjectType.eType, ByVal filename As String) As String

        Dim Template As String = String.Empty
        Select Case FileType
            Case SquealerObjectType.eType.InlineTableFunction
                Template = My.Resources.SqlTemplateInlineTableFunction
            Case SquealerObjectType.eType.MultiStatementTableFunction
                Template = My.Resources.SqlTemplateMultiStatementTableFunction
            Case SquealerObjectType.eType.ScalarFunction
                Template = My.Resources.SqlTemplateScalarFunction
            Case SquealerObjectType.eType.StoredProcedure
                Template = My.Resources.SqlTemplateProcedure
            Case SquealerObjectType.eType.View
                Template = My.Resources.SqlTemplateView
        End Select
        Template = Template.Replace("{RootType}", FileType.ToString)

        ' Did user forget the "-" prefix before the object type switch? ex: tf instead of -tf
        For Each s As String In System.Enum.GetNames(GetType(SquealerObjectType.eShortType))
            If filename.ToLower.StartsWith(s.ToLower & " ") Then
                Textify.SayError("Did you mean:  " & eCommandType.[new].ToString & " -" & s & " " & filename.Remove(0, s.Length + 1), Textify.eSeverity.warning)
            End If
        Next

        ' Make sure all new programs have a schema.
        If Not filename.Contains(".") Then
            filename = String.Concat("dbo.", filename)
        End If

        'Dim fqTemp As String = My.Computer.FileSystem.GetTempFileName
        Dim fqTarget As String = WorkingFolder & "\" & filename & MyConstants.ObjectFileExtension

        ' Careful not to overwrite existing file.
        If My.Computer.FileSystem.FileExists(fqTarget) Then
            Textify.SayError("File already exists.")
            CreateNewFile = String.Empty
        Else
            My.Computer.FileSystem.WriteAllText(fqTarget, Template, False, System.Text.Encoding.ASCII)
            RepairXmlFile(True, fqTarget, False)
            Textify.SayBullet(Textify.eBullet.Hash, "OK")
            Textify.WriteLine(" (" & filename & ")")
            CreateNewFile = fqTarget
        End If

        Textify.SayNewLine()

    End Function


#End Region

#Region " Proc Generation "

    ' Expand one root file.
    Private Function ExpandIndividual(info As IO.FileInfo, StringReplacements As DataTable, genmode As eMode) As String

        Dim oType As SquealerObjectType.eType = SquealerObjectType.Eval(XmlGetObjectType(info.FullName))
        Dim RootName As String = info.Name.Replace(MyConstants.ObjectFileExtension, "")

        Dim InXml As Xml.XmlDocument = FixedXml(False, info.FullName, False)
        Dim InRoot As Xml.XmlElement = DirectCast(InXml.SelectSingleNode(My.Application.Info.ProductName), Xml.XmlElement)
        Dim Block As String = Nothing

        Dim CodeBlocks As New List(Of String)


        ' Pre-Code
        If Not genmode = eMode.test Then
            Dim InPreCode As String = String.Empty
            Try
                InPreCode = InRoot.SelectSingleNode("PreCode").InnerText
            Catch ex As Exception
            End Try

            If Not String.IsNullOrWhiteSpace(InPreCode) Then
                InPreCode = vbCrLf & "-- additional code to execute after " & oType.ToString & " is created" & vbCrLf & InPreCode
                CodeBlocks.Add(InPreCode)
            End If

        End If


        ' Drop 
        If Not genmode = eMode.test AndAlso Not genmode = eMode.alter Then
            CodeBlocks.Add(My.Resources.SqlDrop.Replace("{RootProgramName}", RoutineName(RootName)).Replace("{Schema}", SchemaName(RootName)).ToString)
        End If

        ' Comments
        Dim OutComments As String = Nothing
        Try
            OutComments = InRoot.SelectSingleNode("Comments").InnerText.Replace("/*", String.Empty).Replace("*/", String.Empty)
        Catch ex As Exception
            OutComments = String.Empty
        End Try
        Block = My.Resources.SqlComment.Replace("{RootProgramName}", RoutineName(RootName)).Replace("{Comments}", OutComments).Replace("{Schema}", SchemaName(RootName))

        ' Create
        If Not genmode = eMode.test Then
            Dim SqlCreate As String = String.Empty
            Select Case oType
                Case SquealerObjectType.eType.StoredProcedure
                    SqlCreate = My.Resources.SqlCreateProcedure
                Case SquealerObjectType.eType.ScalarFunction, SquealerObjectType.eType.InlineTableFunction, SquealerObjectType.eType.MultiStatementTableFunction
                    SqlCreate = My.Resources.SqlCreateFunction
                Case SquealerObjectType.eType.View
                    SqlCreate = My.Resources.SqlCreateView
            End Select
            If genmode = eMode.alter Then
                SqlCreate = SqlCreate.Replace("create", "alter")
            End If
            Block &= SqlCreate.Replace("{RootProgramName}", RoutineName(RootName)).Replace("{Schema}", SchemaName(RootName))
        End If

        ' Parameters
        Dim InParameters As DataTable = GetParameters(InXml)
        Dim ParameterCount As Int32 = 0
        Dim DeclareList As New ArrayList
        Dim SetList As New ArrayList
        Dim OutputParameters As String = String.Empty
        Dim ErrorLogParameters As String = String.Empty

        For Each Parameter As DataRow In InParameters.Select()

            Dim def As String = Nothing

            If genmode = eMode.test Then

                def = "declare @" & Parameter.Item("Name").ToString & " " & Parameter.Item("Type").ToString
                If Parameter.Item("DefaultValue").ToString = String.Empty Then
                    def = def & " = "
                Else
                    def = def & " = " & Parameter.Item("DefaultValue").ToString
                End If
                def = def & ";"
                If Not Parameter.Item("Comments").ToString = String.Empty Then
                    def = def & " -- " & Parameter.Item("Comments").ToString
                End If

            Else

                'def = ""
                If ParameterCount = 0 Then '< InParameters.Rows.Count Then
                    def = ""
                Else
                    def = ","
                End If

                ' Write parameters as actual parameters.
                ParameterCount += 1
                def = def & "@" & Parameter.Item("Name").ToString & " " & Parameter.Item("Type").ToString
                If Not Parameter.Item("DefaultValue").ToString = String.Empty Then
                    def = def & " = " & Parameter.Item("DefaultValue").ToString
                End If
                If Parameter.Item("Output").ToString = Boolean.TrueString Then
                    def = def & " output"
                End If
                If Not Parameter.Item("Comments").ToString = String.Empty Then
                    def = def & " -- " & Parameter.Item("Comments").ToString
                End If
                ' Write out error logging section.
                If Not (Parameter.Item("Type").ToString.ToLower.Contains("max") OrElse Parameter.Item("Name").ToString.ToLower.Contains(" readonly")) Then
                    ErrorLogParameters &= vbCrLf & My.Resources.SqlEndProcedure2.Replace("{ErrorParameterNumber}", ParameterCount.ToString).Replace("{ErrorParameterName}", Parameter.Item("Name").ToString)
                End If

            End If

            DeclareList.Add(def)

        Next
        For Each s As String In DeclareList
            Block = Block & vbCrLf & s
        Next
        For Each s As String In SetList
            Block = Block & vbCrLf & s
        Next


        ' Table (View)
        If oType = SquealerObjectType.eType.View Then

            Dim InColumns As DataTable = GetColumns(InXml)

            If InColumns.Rows.Count > 0 AndAlso Not genmode = eMode.test Then

                Block &= vbCrLf & "(" & vbCrLf

                Dim ColumnCount As Integer = 0

                For Each Column As DataRow In InColumns.Select()

                    Dim c As String = String.Empty
                    If ColumnCount > 0 Then
                        c = ","
                    End If

                    ColumnCount += 1

                    c &= String.Format("[{0}]", Column.Item("Name").ToString)
                    If Not Column.Item("Comments").ToString = String.Empty Then
                        c = c & " -- " & Column.Item("Comments").ToString
                    End If

                    Block &= c & vbCrLf

                Next

                Block &= vbCrLf & ")" & vbCrLf

            End If

        End If



        ' Table (MultiStatementTableFunction)
        If oType = SquealerObjectType.eType.MultiStatementTableFunction Then

            If genmode = eMode.test Then
                Block = Block & My.Resources.SqlTableMultiStatementTableFunctionTest
            Else
                Block = Block & My.Resources.SqlTableMultiStatementTableFunction
            End If

            Dim PrimaryKey As String = String.Empty ' If this never gets filled, then there is no primary key.
            Dim Clustered As Boolean = CBool(DirectCast(InRoot.SelectSingleNode("Table"), Xml.XmlElement).GetAttribute("PrimaryKeyClustered"))

            Dim InColumns As DataTable = GetColumns(InXml)
            Dim ColumnCount As Integer = 0
            For Each Column As DataRow In InColumns.Select()

                Dim c As String = String.Empty
                If ColumnCount > 0 Then
                    c = ","
                End If

                ColumnCount += 1

                c &= String.Format("[{0}]", Column.Item("Name").ToString)
                c &= " " & Column.Item("Type").ToString & " "
                If Not CBool(Column.Item("Nullable")) Then
                    c &= "Not "
                End If
                c &= "null"
                If CBool(Column.Item("Identity")) Then
                    c &= " identity"
                End If
                If Not Column.Item("Comments").ToString = String.Empty Then
                    c = c & " -- " & Column.Item("Comments").ToString
                End If

                Block &= c & vbCrLf

                If Column.Item("IncludeInPrimaryKey").ToString = Boolean.TrueString Then
                    If PrimaryKey.Length > 0 Then
                        PrimaryKey &= ","
                    End If
                    PrimaryKey &= Column.Item("Name").ToString
                End If
            Next

            If PrimaryKey.Length > 0 Then
                Block &= "primary key " & IIf(Clustered, "clustered ", "").ToString & "(" & PrimaryKey & ")"
            End If

        End If

        ' Begin
        Dim BeginBlock As String = Nothing
        Select Case oType
            Case SquealerObjectType.eType.StoredProcedure
                If genmode = eMode.test Then
                    BeginBlock = My.Resources.SqlBeginProcedureTest
                Else
                    BeginBlock = My.Resources.SqlBeginProcedure
                End If
            Case SquealerObjectType.eType.ScalarFunction
                Dim Returns As String = Nothing
                Returns = DirectCast(InRoot.SelectSingleNode("Returns"), Xml.XmlElement).GetAttribute("Type")
                If genmode = eMode.test Then
                    BeginBlock = My.Resources.SqlBeginScalarFunctionTest.Replace("{ReturnDataType}", Returns)
                Else
                    BeginBlock = My.Resources.SqlBeginScalarFunction.Replace("{ReturnDataType}", Returns)
                End If
            Case SquealerObjectType.eType.InlineTableFunction
                If genmode = eMode.test Then
                    BeginBlock = String.Empty
                Else
                    BeginBlock = My.Resources.SqlBeginInlineTableFunction
                End If
            Case SquealerObjectType.eType.MultiStatementTableFunction
                If genmode = eMode.test Then
                    BeginBlock = My.Resources.SqlBeginMultiStatementTableFunctionTest
                Else
                    BeginBlock = My.Resources.SqlBeginMultiStatementTableFunction
                End If
            Case SquealerObjectType.eType.View
                If genmode = eMode.test Then
                    BeginBlock = String.Empty
                Else
                    BeginBlock = My.Resources.SqlBeginView
                End If
        End Select


        Dim obj As New SquealerObject(info.FullName)
        Dim WithOptions As String = obj.WithOptions
        If genmode = eMode.encrypt Then
            If String.IsNullOrWhiteSpace(obj.WithOptions) Then
                WithOptions = "encryption"
            ElseIf Not WithOptions.ToLower.Contains("encryption") Then
                WithOptions = WithOptions & ",encryption"
            End If
        End If

        If String.IsNullOrWhiteSpace(WithOptions) Then
            BeginBlock = BeginBlock.Replace("{WithOptions}", String.Empty)
        Else
            BeginBlock = BeginBlock.Replace("{WithOptions}", "with " & WithOptions)
        End If

        Block &= BeginBlock

        ' Code
        Dim InCode As String = String.Empty
        Try
            InCode = InRoot.SelectSingleNode("Code").InnerText
        Catch ex As Exception
        End Try
        Block &= InCode

        ' End
        Select Case oType
            Case SquealerObjectType.eType.StoredProcedure
                If genmode = eMode.test Then
                    Block &= My.Resources.SqlEndProcedureTest
                Else
                    Block &= My.Resources.SqlEndProcedure1 & ErrorLogParameters & My.Resources.SqlEndProcedure3
                End If
            Case SquealerObjectType.eType.ScalarFunction
                If genmode = eMode.test Then
                    Block = Block & My.Resources.SqlEndScalarFunctionTest
                Else
                    Block = Block & My.Resources.SqlEndScalarFunction
                End If
            Case SquealerObjectType.eType.MultiStatementTableFunction
                If genmode = eMode.test Then
                    Block = Block & My.Resources.SqlEndMultiStatementTableFunctionTest
                Else
                    Block = Block & My.Resources.SqlEndMultiStatementTableFunction
                End If
            Case SquealerObjectType.eType.View
                ' nothing to add
        End Select

        ' Save the block.
        CodeBlocks.Add(Block)

        If Not genmode = eMode.test AndAlso Not genmode = eMode.alter Then

            Block = String.Empty

            ' Grant Execute
            Dim InUsers As DataTable = GetUsers(InXml)
            For Each User As DataRow In InUsers.Select("", "Name asc")
                Dim GrantStatement As String
                If oType = SquealerObjectType.eType.StoredProcedure OrElse oType = SquealerObjectType.eType.ScalarFunction Then
                    GrantStatement = My.Resources.SqlGrantExecute
                Else
                    GrantStatement = My.Resources.SqlGrantSelect
                End If
                Block = Block & vbCrLf & GrantStatement.Replace("{RootProgramName}", RoutineName(RootName)).Replace("{User}", User.Item("Name").ToString).Replace("{Schema}", SchemaName(RootName))
            Next
            If Not Block = String.Empty Then
                CodeBlocks.Add(Block)
            End If

        End If


        ' Post-Code
        If Not genmode = eMode.test Then
            Dim InPostCode As String = String.Empty
            Try
                InPostCode = InRoot.SelectSingleNode("PostCode").InnerText
            Catch ex As Exception
            End Try

            If Not String.IsNullOrWhiteSpace(InPostCode) Then
                InPostCode = vbCrLf & "-- additional code to execute after " & oType.ToString & " is created" & vbCrLf & InPostCode
                CodeBlocks.Add(InPostCode)
            End If

        End If


        ' Now add all the GOs
        ExpandIndividual = String.Empty
        For Each s As String In CodeBlocks
            ExpandIndividual &= s & vbCrLf
            If Not genmode = eMode.test Then
                ExpandIndividual &= "go" & vbCrLf
            End If
        Next

        ' Do string replacements.
        For Each Replacement As DataRow In StringReplacements.Select() '.Select("")
            ExpandIndividual = ExpandIndividual.Replace(Replacement.Item("Original").ToString, Replacement.Item("Replacement").ToString)
        Next
        ExpandIndividual = ExpandIndividual.Replace("{THIS}", RoutineName(RootName))


    End Function

#End Region

#Region " Git "

    Private Function GitChangedFiles(folder As String, gc As String) As List(Of String)
        Dim gitstream As New List(Of String)
        For Each s As String In GitResults(folder, gc)
            gitstream.Add(folder & "\" & s.Substring(3))
        Next

        Return gitstream

    End Function

    Private Function GitResults(folder As String, gc As String) As List(Of String)

        Dim gitstream As New List(Of String)

        Try
            Dim command As String = "cmd.exe"
            Dim arguments As String = "/c " & gc
            Dim ps As New ProcessStartInfo With {
                .WorkingDirectory = folder,
                .FileName = command,
                .Arguments = arguments,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .RedirectStandardError = True,
                .RedirectStandardOutput = True,
                .UseShellExecute = False
            }
            Dim oProcess As New Process() With {.StartInfo = ps}
            oProcess.Start()

            Using oStreamReader As System.IO.StreamReader = oProcess.StandardOutput
                'If oStreamReader.EndOfStream Then
                '    gitstream = ""
                'End If
                While Not oStreamReader.EndOfStream
                    gitstream.Add(oStreamReader.ReadLine())
                End While
            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return gitstream

    End Function


    Private Sub GitCommandDo(folder As String, gc As String, errormessage As String)

        Try
            Dim command As String = "cmd.exe"
            Dim arguments As String = String.Format("/c " & gc)
            Dim ps As New ProcessStartInfo
            ps.WorkingDirectory = folder
            ps.FileName = command
            ps.Arguments = arguments
            ps.WindowStyle = ProcessWindowStyle.Hidden
            Dim oProcess As New Process()
            ps.RedirectStandardOutput = True
            ps.RedirectStandardError = True
            ps.UseShellExecute = False
            oProcess.StartInfo = ps
            oProcess.Start()

            Using oStreamReader As System.IO.StreamReader = oProcess.StandardOutput
                If oStreamReader.EndOfStream Then
                    Textify.Write(errormessage, ConsoleColor.Red)
                End If
                While Not oStreamReader.EndOfStream
                    Console.WriteLine()
                    Textify.Write(oStreamReader.ReadLine(), ConsoleColor.Cyan, Textify.eLineMode.Truncate)
                End While
            End Using

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub


    Private Function CurrentBranch(folder As String, sformat As String) As String

        Dim s As String

        If UserSettings.ShowBranch Then
            Try
                Dim command As String = "cmd.exe"
                Dim arguments As String = String.Format("/c git symbolic-ref HEAD")
                Dim ps As New ProcessStartInfo
                ps.WorkingDirectory = folder
                ps.FileName = command
                ps.Arguments = arguments
                ps.WindowStyle = ProcessWindowStyle.Hidden
                Dim oProcess As New Process()
                ps.RedirectStandardOutput = True
                ps.RedirectStandardError = True
                ps.UseShellExecute = False
                oProcess.StartInfo = ps
                oProcess.Start()

                Dim sOutput As String
                Using oStreamReader As System.IO.StreamReader = oProcess.StandardOutput
                    sOutput = oStreamReader.ReadToEnd()
                End Using

                If String.IsNullOrEmpty(oProcess.StandardError.ReadToEnd) Then
                    s = sformat.Replace("{0}", sOutput.Trim.Replace("refs/heads/", String.Empty))
                Else
                    s = sformat.Replace("{0}", "no git")
                End If

            Catch ex As Exception

                s = sformat.Replace("{0}", "git error!")

            End Try

        Else
            s = String.Empty
        End If

        Return s

    End Function

#End Region

#Region " Misc "

    Private Sub BracketCheck(s As String)
        If s.Contains("["c) OrElse s.Contains("]"c) Then
            Throw New System.Exception("Illegal square bracket character in filename: " & s)
        End If
    End Sub

    Private Function ValidDirectoryStyle(s As String) As Boolean
        Select Case s.ToLower
            Case eDirectoryStyle.compact.ToString
                Return True
            Case eDirectoryStyle.full.ToString
                Return True
            Case eDirectoryStyle.symbolic.ToString
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Function PadRightIfNotEmpty(s As String) As String
        If String.IsNullOrWhiteSpace(s) Then
            Return s
        Else
            Return s & " "
        End If
    End Function

    Private Sub NukeFiles(folder As String, wildcard As String)
        Dim command As String = "cmd.exe"
        Dim arguments As String = String.Format("/c del {0}{1}", wildcard, MyConstants.ObjectFileExtension)
        Textify.Write(String.Format("{0} {1}", command, arguments))
        Dim ps As New ProcessStartInfo
        ps.WorkingDirectory = folder
        ps.FileName = command
        ps.Arguments = arguments
        ps.WindowStyle = ProcessWindowStyle.Hidden
        Process.Start(ps)
        Textify.SayNewLine()
    End Sub

    Private Sub ReadChangeLog()

        Dim sr As New IO.StringReader(My.Resources.ChangeLog.TrimEnd)
        While sr.Peek <> -1
            Dim s As String = sr.ReadLine
            Textify.WriteLine(String.Format(s, ">>>> ", " <<<<", " - "))
        End While
        Textify.SayNewLine()
        Textify.SayNewLine()

    End Sub

    Private Sub AboutInfo()
        With My.Application.Info
            Textify.SayBulletLine(Textify.eBullet.Hash, .Title)
            Textify.SayBulletLine(Textify.eBullet.Hash, .Description)
            Textify.SayBulletLine(Textify.eBullet.Hash, "by " & .CompanyName)
            Textify.SayBulletLine(Textify.eBullet.Hash, .Copyright)
            Textify.SayBulletLine(Textify.eBullet.Hash, """" & .Trademark & """")
            Textify.SayBulletLine(Textify.eBullet.Hash, "v." & .Version.ToString)
        End With
        Textify.SayNewLine()
        Textify.SayBulletLine(Textify.eBullet.Hash, "++++++++++++++++++++++++++++++++++++++++++++++++++++++ #")
        Textify.SayBulletLine(Textify.eBullet.Hash, "Check out my album ""To The Crows"" at www.thehusht.com  #")
        Textify.SayBulletLine(Textify.eBullet.Hash, "New album ""The Law of Gravity"" coming out in 2020!     #")
        Textify.SayBulletLine(Textify.eBullet.Hash, "++++++++++++++++++++++++++++++++++++++++++++++++++++++ #")
        Textify.SayNewLine()
        Textify.SayBulletLine(Textify.eBullet.Hash, "SQL formatting by https://github.com/TaoK/PoorMansTSqlFormatter")
        Textify.SayNewLine()
        Textify.SayBulletLine(Textify.eBullet.Hash, "May the Force be with you.")
        Textify.SayNewLine()
    End Sub


    Private Sub OpenExplorer(ByVal wildcard As String, ByVal WorkingFolder As String)

        Dim files As ObjectModel.ReadOnlyCollection(Of String)

        files = My.Computer.FileSystem.GetFiles(WorkingFolder, FileIO.SearchOption.SearchTopLevelOnly, wildcard)

        If wildcard = "*" & MyConstants.ObjectFileExtension Then
            Process.Start("Explorer.exe", WorkingFolder)
        ElseIf files.Count = 0 Then
            Throw New Exception("File not found.")
        Else
            Process.Start("Explorer.exe", "/select," & WorkingFolder & "\" & My.Computer.FileSystem.GetFileInfo(files(0)).Name)
        End If

    End Sub

    ' Edit one or more files.
    Private Sub FileEdit(filename As String)
        Try
            Process.Start(UserSettings.TextEditor, PadRightIfNotEmpty(UserSettings.TextEditorSwitches) & """" & My.Computer.FileSystem.GetFileInfo(filename).Name & """") ' Surround with quotes for file names with spaces
        Catch ex As Exception
            Textify.SayError(ex.Message)
            Textify.SayBullet(Textify.eBullet.Arrow, UserSettings.TextEditor)
            Textify.SayNewLine()
        End Try
    End Sub

    Private Sub ThrowErrorIfOverFileLimit(limit As Integer, n As Integer, OverrideSafety As Boolean)
        If n > limit AndAlso Not OverrideSafety Then
            Throw New Exception(String.Format("Too many files. Limit {0}, found {1}.", limit.ToString, n.ToString))
        End If
    End Sub

    ' Clear the keyboard input buffer silently.
    Private Sub ClearKeyboard()
        While Console.KeyAvailable
            Console.ReadKey(True)
        End While
    End Sub

    ' Return the schema name from the filename.
    Friend Function SchemaName(ByVal DisplayName As String) As String
        Dim i As Integer = DisplayName.IndexOf("."c)
        If i < 0 Then
            Return "dbo"
        Else
            Return DisplayName.Substring(0, i)
        End If
    End Function

    ' Return the routine name from the filename.
    Friend Function RoutineName(ByVal DisplayName As String) As String
        Dim i As Integer = DisplayName.IndexOf("."c)
        If i < 0 Then
            Return DisplayName
        Else
            Return DisplayName.Substring(i + 1)
        End If
    End Function

#End Region

#Region " Config File "

    ' Grab the default users from the project config file.
    Private Function GetDefaultUsers(ByVal WorkingFolder As String) As DataTable

        Dim DefaultUsers As New DataTable
        With DefaultUsers.Columns
            .Add("Name", GetType(String))
        End With

        Dim Reader As New Xml.XmlDocument
        Reader.Load(WorkingFolder & "\" & MyConstants.ConfigFilename)
        Dim Nodes As Xml.XmlNodeList = Reader.SelectNodes("/Settings/DefaultUsers/User")

        For Each Node As Xml.XmlNode In Nodes
            DefaultUsers.Rows.Add(AttributeDefaultString(Node.Attributes.GetNamedItem("Name"), String.Empty))
        Next

        GetDefaultUsers = DefaultUsers

    End Function

    ' Grab the string replacements from the project config file.
    Private Function GetStringReplacements(ByVal WorkingFolder As String) As DataTable

        Dim StringReplacements As New DataTable
        With StringReplacements.Columns
            .Add("Original", GetType(String))
            .Add("Replacement", GetType(String))
        End With

        Try
            Dim Reader As New Xml.XmlDocument
            Reader.Load(WorkingFolder & "\" & MyConstants.ConfigFilename)
            Dim Nodes As Xml.XmlNodeList = Reader.SelectNodes("/Settings/StringReplacements/String")

            For Each Node As Xml.XmlNode In Nodes
                StringReplacements.Rows.Add(
                    AttributeDefaultString(Node.Attributes.GetNamedItem("Original"), String.Empty),
                    AttributeDefaultString(Node.Attributes.GetNamedItem("Replacement"), String.Empty)
                )
            Next
        Catch ex As Exception
        End Try

        GetStringReplacements = StringReplacements

    End Function

    ' Get the project nickname.
    Private Function GetProjectNickname(ByVal WorkingFolder As String) As String

        Try
            Dim Reader As New Xml.XmlDocument
            Reader.Load(WorkingFolder & "\" & MyConstants.ConfigFilename)
            Dim Node As Xml.XmlNode = Reader.SelectSingleNode("/Settings")
            Dim s As String = Node.Attributes("ProjectName").Value.ToString.Trim()
            If String.IsNullOrWhiteSpace(s) Then
                s = "myproject"
            End If
            If s.Length > 30 Then
                s = s.Substring(0, 30)
            End If
            Return s
        Catch ex As Exception
            Return "myproject"
        End Try

    End Function

#End Region

#Region " Console Output "

    ' Display a notice for a file.
    Private Sub SayFileAction(ByVal notice As String, ByVal path As String, ByVal file As String)
        Textify.SayBullet(Textify.eBullet.Hash, notice & ":")
        Textify.SayBullet(Textify.eBullet.Arrow, path & IIf(file = "", "", "\" & file).ToString)
    End Sub

    Private Sub ShowFile(ByVal path As String, ByVal file As String)

        Dim f As String = path & "\" & file

        SayFileAction("reading", path, file)
        Textify.SayNewLine()

        Try
            Console.WriteLine(My.Computer.FileSystem.ReadAllText(f))
        Catch ex As Exception
            Textify.SayError("File not found.", Textify.eSeverity.info)
        End Try

    End Sub

#End Region

#Region " Star Wars "

    Private Sub BeginStarWarsDay()
        Console.WriteLine(My.Resources.eggMay4)
        Textify.SayNewLine()
        Textify.SayBullet(Textify.eBullet.Hash, "[Help] me, Obi-Wan Kenobi. You're my only hope. [Help]!")
        Textify.SayNewLine()
        'Commands.GetCommand("usetheforce").Visible = True
        MyCommands.FindCommand(eCommandType.usetheforce.ToString).Visible = True
        BadCommandMessage = "I've got a bad feeling about this."
    End Sub

    Private Sub UseTheForce(ByVal k As String)

        Dim fc As ForceCommand = StarWars.ForcePowerFind(k)

        If fc.Found Then
            Textify.SayBullet(Textify.eBullet.Hash, "(You found this one already.)")
            Textify.SayNewLine()
        Else
            fc.Found = True
        End If

        If k = "awaken" Then ' don't use fc.Keyword because it's not in the list
            StarWars.StarWarsDay = True
            BeginStarWarsDay()

        ElseIf fc.Crawl Then
            StarWarsCrawl(StarWars.ForcePowerFind(k).Response)

        ElseIf fc.Keyword = "greedo shot first" Then

            If StarWars.FoundAll Then
                StarWarsCrawl(My.Resources.eggVictory)
            Else
                BadMotivatorInfiniteLoop()
            End If

        ElseIf fc.Keyword = "tie fighter" Then
            Dim fgColor As ConsoleColor = Console.ForegroundColor
            Dim bgColor As ConsoleColor = Console.BackgroundColor
            Dim fight As New GoldLeader(False)
            fight.TryPlay()
            Console.ForegroundColor = fgColor
            Console.BackgroundColor = bgColor
            Console.WriteLine()

        ElseIf fc.Keyword = "error" Then
            Textify.SayError("Would it help if I got out and pushed?", Textify.eSeverity.info)
            Textify.SayNewLine()

        Else
            Textify.SayBullet(Textify.eBullet.Hash, fc.Response)
            Textify.SayNewLine()

        End If

        Textify.SayBullet(Textify.eBullet.Hash, "(Found: " & StarWars.FoundCount & " of " & StarWars.PossibleCount & ")")
        Textify.SayNewLine()

    End Sub

    Private Sub BadMotivatorErrorMessage(ByVal s As String)
        Textify.SayError(s)
        Threading.Thread.Sleep(1500)
    End Sub

    ' Infinite loop.
    Private Sub BadMotivatorInfiniteLoop()
        BadMotivatorErrorMessage("CIRCUIT FAULT.")
        BadMotivatorErrorMessage("DIAGNOSTIC TEST STARTED...")
        BadMotivatorErrorMessage("FAILED.")
        BadMotivatorErrorMessage("CHECK LIQUID SCHWARTZ...")
        BadMotivatorErrorMessage("EMPTY.")
        Textify.SayNewLine()
        ClearKeyboard()
        Console.Write("< 0-9 A-F > ")
        Dim key As Char = Console.ReadKey().KeyChar
        For i As Integer = 1 To 10
            Console.Write("^" & key)
            Threading.Thread.Sleep(100)
        Next
        ClearKeyboard()
        Dim r As New Random
        While True
            Console.Write("  BAD_MOTIVATOR:" & r.Next(0, 65535).ToString & "  ")
            For i As Integer = 1 To r.Next(5, 50)
                If Not Console.KeyAvailable Then
                    Console.Write(Chr(r.Next(8, 255))) ' Start at 8 because 7 is a beep
                    Threading.Thread.Sleep(10)
                End If
            Next
            While Console.KeyAvailable
                Console.WriteLine()
                Console.Write("  [CRAWL IV]")
                If Textify.ErrorAlert.Beep Then
                    Console.Beep()
                End If
                Threading.Thread.Sleep(2000)
                ClearKeyboard()
            End While

        End While
    End Sub

    Private Sub StarWarsCrawl(ByVal crawlText As String)

        ' Intro, then clear the console
        Textify.SayCentered("A long time ago in a galaxy far, far away....", False)
        Threading.Thread.Sleep(5000)
        Textify.SayNewLine(Console.WindowHeight)

        Dim script As New List(Of String)

        ' Double-space the script
        For Each line As String In crawlText.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
            script.Add(line)
            script.Add("")
        Next

        ClearKeyboard()

        Dim milliseconds As Integer = 1000 ' One second between lines
        For Each line As String In script
            Textify.SayCentered(line, False)
            Threading.Thread.Sleep(milliseconds)
            Console.WriteLine()
            If Console.KeyAvailable Then
                milliseconds = 10 ' 1/100 second between lines (keystroke = hurry up)
            End If
        Next

    End Sub

#End Region

#Region " Connection String "

    Private Sub SetConnectionString(workingfolder As String, cs As String)

        Dim f As String = String.Format("{0}\{1}", workingfolder, MyConstants.ConnectionStringFilename)
        Dim entropy As Byte() = {1, 9, 1, 1, 4, 5}
        Dim csbytes As Byte() = System.Text.Encoding.Unicode.GetBytes(cs.Trim)

        Dim encrypted As Byte() = System.Security.Cryptography.ProtectedData.Protect(csbytes, entropy, Security.Cryptography.DataProtectionScope.CurrentUser)
        If My.Computer.FileSystem.FileExists(f) Then
            My.Computer.FileSystem.DeleteFile(f)
        End If
        My.Computer.FileSystem.WriteAllBytes(f, encrypted, False)
        System.IO.File.SetAttributes(f, IO.FileAttributes.Hidden)

        Textify.SayBulletLine(Textify.eBullet.Hash, "OK")
        Textify.SayNewLine()

    End Sub
    Private Sub ForgetConnectionString(workingfolder As String)

        Dim f As String = String.Format("{0}\{1}", workingfolder, MyConstants.ConnectionStringFilename)
        My.Computer.FileSystem.DeleteFile(f)
        Textify.SayBulletLine(Textify.eBullet.Hash, "OK")
        Textify.SayNewLine()

    End Sub

    Private Function GetConnectionString(workingfolder As String) As String

        Dim f As String = String.Format("{0}\{1}", workingfolder, MyConstants.ConnectionStringFilename)

        If Not My.Computer.FileSystem.FileExists(f) Then
            Throw New Exception("Connection string not defined.")
        End If

        Dim entropy As Byte() = {1, 9, 1, 1, 4, 5}
        Dim decrypted As Byte() = System.Security.Cryptography.ProtectedData.Unprotect(My.Computer.FileSystem.ReadAllBytes(f), entropy, Security.Cryptography.DataProtectionScope.CurrentUser)
        GetConnectionString = System.Text.Encoding.Unicode.GetString(decrypted)

    End Function

    Private Sub TestConnectionString(workingfolder As String)

        Dim cs As String = GetConnectionString(workingfolder)

        Textify.SayBullet(Textify.eBullet.Arrow, cs)

        Using DbTest As SqlClient.SqlConnection = New SqlClient.SqlConnection(cs)

            DbTest.Open()

            Dim DbReader As SqlClient.SqlDataReader = New SqlClient.SqlCommand("select @@SERVERNAME,DB_NAME(),@@VERSION,(select count(1) from sys.tables),(select count(1) from sys.objects o where o.type = 'p');", DbTest).ExecuteReader

            While DbReader.Read

                'Dim Result As String = 

                Textify.SayBulletLine(Textify.eBullet.Arrow, DbReader.GetString(2)) ' @@version
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("[{0}].[{1}]", DbReader.GetString(0), DbReader.GetString(1)))
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("{0} table(s)", DbReader.GetInt32(3).ToString))
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("{0} stored procedure(s)", DbReader.GetInt32(4).ToString))
                Textify.SayBulletLine(Textify.eBullet.Hash, "OK")

            End While

            Textify.SayNewLine()

        End Using

    End Sub

#End Region

#Region " Automagic "

    Private Enum eAutoProcType
        [Insert]
        [Update]
        [Delete]
        [Get]
        [List]
    End Enum

    Private Sub Automagic(cs As String, WorkingFolder As String, ReplaceOnly As Boolean, datasourcecomment As Boolean)

        Textify.Write("Reading tables .")

        Dim ProcCount As Integer = 0

        Using DbTables As SqlClient.SqlConnection = New SqlClient.SqlConnection(cs)

            DbTables.Open()

            Dim TableReader As SqlClient.SqlDataReader = New SqlClient.SqlCommand(My.Resources.AutoGetTables, DbTables).ExecuteReader

            Dim spinny As New SpinnyProgress()


            While TableReader.Read

                Dim SchemaName As String = TableReader.GetString(0)
                Dim TableName As String = TableReader.GetString(1)
                Dim TableId As Integer = TableReader.GetInt32(2)
                Dim AutoProcType As eAutoProcType = DirectCast([Enum].Parse(GetType(eAutoProcType), TableReader.GetString(3)), eAutoProcType)

                Dim GenOutputs As New List(Of String)
                Dim GenParameters As New List(Of String)
                Dim GenWhereClause As String = ""
                Dim GenFromColumns As New List(Of String)
                Dim GenValuesColumns As New List(Of String)

                spinny.DoStep()

                Using DbColumns As SqlClient.SqlConnection = New SqlClient.SqlConnection(cs)

                    DbColumns.Open()

                    Dim ColumnReader As SqlClient.SqlDataReader = New SqlClient.SqlCommand(My.Resources.AutoGetColumns.Replace("{TableId}", TableReader("table_id").ToString), DbColumns).ExecuteReader

                    While ColumnReader.Read

                        Dim ColName As String = ColumnReader.GetString(0)
                        Dim ColType As String = ColumnReader.GetString(1).ToLower
                        Dim ColIsIdentity As Boolean = ColumnReader.GetBoolean(2)
                        Dim ColIsRowGuid As Boolean = ColumnReader.GetBoolean(3)
                        Dim ColId As Integer = ColumnReader.GetInt32(4)
                        Dim ColMaxLength As Int16 = ColumnReader.GetInt16(5)
                        Dim ColPrecision As Byte = ColumnReader.GetByte(6)
                        Dim ColScale As Byte = ColumnReader.GetByte(7)
                        Dim ColDefaultValue As String = ColumnReader(8).ToString
                        Dim ColHasDefault As Boolean = ColDefaultValue.Trim.Length > 0
                        Dim ColIsPk As Boolean = ColumnReader.GetBoolean(9)
                        Dim ColIsComputed As Boolean = ColumnReader.GetBoolean(10)
                        Dim ColIsGuidIdentity As Boolean = ColType.Contains("uniqueidentifier") AndAlso (ColDefaultValue.Contains("newid") OrElse ColDefaultValue.Contains("newsequentialid"))


                        ' DEFINE PARAMETERS

                        ' Add scale and precision to column type
                        If (ColType.Contains("char") OrElse ColType.Contains("binary")) Then
                            ColType &= String.Format("({0})", ColMaxLength.ToString.Replace("-1", "max"))
                        ElseIf (ColType.Contains("decimal") OrElse ColType.Contains("numeric")) Then
                            ColType &= String.Format("({0},{1})", ColPrecision.ToString, ColScale.ToString)
                        End If

                        ' Add all parameters (with output IDs) for write procs; only add key parameters for read procs
                        If (AutoProcType = eAutoProcType.Insert OrElse AutoProcType = eAutoProcType.Update) OrElse ((AutoProcType = eAutoProcType.Delete OrElse AutoProcType = eAutoProcType.Get) AndAlso ColIsPk) Then
                            GenParameters.Add(String.Format("<Parameter Name=""{0}"" Type=""{1}"" Output=""{2}"" />", ColName.Replace(" ", "_"), ColType, (AutoProcType = eAutoProcType.Insert AndAlso (ColIsIdentity OrElse ColIsRowGuid))))
                        End If


                        ' DETECT OUTPUT COLUMNS

                        ' Ignore columns with MAX width specification because those could be enormous (ex: file attachments).
                        If AutoProcType = eAutoProcType.Insert AndAlso Not ColMaxLength = -1 Then
                            GenOutputs.Add(String.Format("{0}|{1}", ColName, ColType))
                        End If


                        ' BUILD THE INSERT/UPDATE/SELECT COLUMNS (never build DELETE columns)

                        If (AutoProcType = eAutoProcType.Insert And Not ColIsIdentity And Not ColIsGuidIdentity And Not ColIsComputed) _
                                OrElse (AutoProcType = eAutoProcType.Update And Not ColIsPk And Not ColIsComputed) _
                                OrElse (AutoProcType = eAutoProcType.Get) _
                                OrElse (AutoProcType = eAutoProcType.List) Then

                            Dim c As String = String.Format("[{0}]", ColName)
                            If AutoProcType = eAutoProcType.Update Then
                                c &= String.Format(" = @{0}", ColName.Replace(" ", "_"))
                            End If
                            If (AutoProcType = eAutoProcType.Insert OrElse AutoProcType = eAutoProcType.Update) AndAlso ColHasDefault Then
                                c &= " -- default: " & ColDefaultValue
                            End If
                            GenFromColumns.Add(c)

                            If AutoProcType = eAutoProcType.Insert Then
                                If ColIsRowGuid AndAlso Not ColHasDefault Then
                                    c = "newid()"
                                ElseIf ColHasDefault Then
                                    c = ColDefaultValue
                                Else
                                    c = "@" & ColName.Replace(" ", "_")
                                End If
                                GenValuesColumns.Add(c)
                            End If

                        End If


                        ' BUILD THE WHERE CLAUSE

                        If ColIsPk AndAlso (AutoProcType = eAutoProcType.Update OrElse AutoProcType = eAutoProcType.Delete OrElse AutoProcType = eAutoProcType.Get) Then

                            GenWhereClause &= vbCrLf & vbTab & IIf(GenWhereClause.Length > 0, "and ", "").ToString & String.Format("[{0}] = @{0}", ColName)

                        End If



                    End While

                End Using

                Dim OutputUsers As String = ""

                For Each User As DataRow In GetDefaultUsers(WorkingFolder).Rows
                    OutputUsers &= String.Format("<User Name=""{0}"" />", User.Item("Name"))
                Next


                Dim AutoCode As String = "/***********************************************************************" _
                    & vbCrLf & String.Format("	This code was generated by {0}.", My.Application.Info.Title) _
                    & vbCrLf & "***********************************************************************/"

                If AutoProcType = eAutoProcType.Delete Then
                    AutoCode &= vbCrLf & vbCrLf & "-- do you need an UPDATE statement to create an audit log entry?"
                End If

                Dim GenOutputsString As String() = {"", "", ""}

                If GenOutputs.Count > 0 Then

                    For i = 0 To GenOutputs.Count - 1
                        GenOutputsString(0) &= vbCrLf & vbTab & IIf(i = 0, "", ",").ToString & String.Format("[{0}] {1}", GenOutputs(i).Split("|"c)(0), GenOutputs(i).Split("|"c)(1))
                        GenOutputsString(1) &= vbCrLf & vbTab & IIf(i = 0, "", ",").ToString & String.Format("inserted.[{0}]", GenOutputs(i).Split("|"c)(0))
                        GenOutputsString(2) &= vbCrLf & vbTab & IIf(i = 0, "", ",").ToString & String.Format("@{0} = o.[{1}]", GenOutputs(i).Split("|"c)(0).Replace(" ", "_"), GenOutputs(i).Split("|"c)(0))
                    Next

                    AutoCode &= vbCrLf & vbCrLf & "declare @Outputs as table" & vbCrLf & "(" & GenOutputsString(0) & vbCrLf & ");"
                    GenOutputsString(1) = "output" & GenOutputsString(1) & vbCrLf & "into" & vbCrLf & vbTab & "@outputs"
                    GenOutputsString(2) = "select" & GenOutputsString(2) & vbCrLf & "from" & vbCrLf & vbTab & "@outputs o;"

                End If

                AutoCode &= vbCrLf

                If AutoProcType = eAutoProcType.Get OrElse AutoProcType = eAutoProcType.List Then
                    AutoCode &= vbCrLf & "select"
                Else
                    AutoCode &= vbCrLf & AutoProcType.ToString.ToLower
                End If

                If AutoProcType = eAutoProcType.Insert OrElse AutoProcType = eAutoProcType.Update OrElse AutoProcType = eAutoProcType.Delete Then
                    AutoCode &= vbCrLf & vbTab & String.Format("[{0}].[{1}]", SchemaName, TableName)
                End If

                Select Case AutoProcType
                    Case eAutoProcType.Insert
                        AutoCode &= vbCrLf & "("
                    Case eAutoProcType.Update
                        AutoCode &= vbCrLf & "set"
                End Select

                Dim ValidOutput As Boolean = True

                If AutoProcType = eAutoProcType.Update AndAlso GenFromColumns.Count = 0 Then
                    ValidOutput = False
                End If


                For i = 0 To GenFromColumns.Count - 1
                    AutoCode &= vbCrLf & vbTab & IIf(i = 0, "", ",").ToString & GenFromColumns(i)
                Next

                If AutoProcType = eAutoProcType.Insert Then
                    AutoCode &= vbCrLf & ")" & vbCrLf & GenOutputsString(1) & vbCrLf & "values" & vbCrLf & "("
                End If

                For i = 0 To GenValuesColumns.Count - 1
                    AutoCode &= vbCrLf & vbTab & IIf(i = 0, "", ",").ToString & GenValuesColumns(i)
                Next


                If AutoProcType = eAutoProcType.Insert Then
                    AutoCode &= vbCrLf & ");" & vbCrLf & vbCrLf & GenOutputsString(2)
                End If

                'AutoCode &= GenFromClause

                Select Case AutoProcType
                    Case eAutoProcType.Get
                        AutoCode &= vbCrLf & "from" & vbCrLf & vbTab & String.Format("[{0}].[{1}]", SchemaName, TableName)
                    Case eAutoProcType.List
                        AutoCode &= vbCrLf & "from" & vbCrLf & vbTab & String.Format("[{0}].[{1}]", SchemaName, TableName)
                End Select

                If AutoProcType = eAutoProcType.List Then
                    AutoCode &= vbCrLf & "--order by" & vbCrLf & "--" & vbTab & "?"
                End If

                If Not String.IsNullOrWhiteSpace(GenWhereClause) Then
                    AutoCode &= vbCrLf & "where" & GenWhereClause
                End If

                Dim GenParametersString As String = ""
                For Each s As String In GenParameters
                    GenParametersString &= s
                Next


                If ValidOutput Then

                    Dim filename As String = String.Format("{0}\{1}.{5}{2}{3}{4}", WorkingFolder, SchemaName, TableName, AutoProcType, MyConstants.ObjectFileExtension, MyConstants.AutocreateFilename)

                    If Not ReplaceOnly OrElse My.Computer.FileSystem.FileExists(filename) Then

                        Dim comments As String = String.Format("Basic {0} for [{1}].[{2}]", AutoProcType.ToString.ToUpper, SchemaName, TableName)
                        If datasourcecomment Then
                            comments &= String.Format(vbCrLf & "Generated from [{0}].[{1}] on {2}", DbTables.DataSource, DbTables.Database, Now)
                        End If

                        My.Computer.FileSystem.WriteAllText(filename,
                                My.Resources.AutoProcTemplate _
                                    .Replace("{Comments}", comments) _
                                    .Replace("{Parameters}", GenParametersString) _
                                    .Replace("{Code}", AutoCode) _
                                    .Replace("{Users}", OutputUsers) _
                                , False) ' Overwrite

                        RepairXmlFile(False, filename, False)

                        ProcCount += 1

                    End If

                End If


                If Console.KeyAvailable() Then
                    Throw New System.Exception("Keyboard interrupt.")
                End If

            End While

        End Using

        Textify.Write(" done.")

        Textify.SayBullet(Textify.eBullet.Hash, ProcCount.ToString & " files generated.")
        Textify.SayNewLine()

    End Sub

    Public Class SpinnyProgress

        Const TicksPerSecond = 10000000 ' 10 million

        Private _Index As Integer = 0
        Private _SymbolString As String
        Private _Symbols As New List(Of Char)
        Private _AnimationsPerSecond As Integer
        Private _LastTick As Long = DateTime.Now.Ticks

        Public Sub New()
            Me.New("/—\|", 4)
        End Sub

        Public Sub New(s As String)
            Me.New(s, 4)
        End Sub

        Public Sub New(AnimationsPerSecond As Integer)
            Me.New("/—\|", AnimationsPerSecond)
        End Sub

        Public Sub New(s As String, AnimationsPerSecond As Integer)
            _Symbols.AddRange(s.ToCharArray)
            _AnimationsPerSecond = AnimationsPerSecond
            Console.Write(CurrentSymbol)
        End Sub

        Private Function CurrentSymbol() As String

            If Console.CursorLeft > Console.BufferWidth - 2 Then
                Return "."
            Else
                Return _Symbols(_Index)
            End If

        End Function

        Public Sub DoStep()

            Console.Write(Chr(8) & ".")

            If DateTime.Now.Ticks - _LastTick > TicksPerSecond / _AnimationsPerSecond Then
                _LastTick = DateTime.Now.Ticks
                _Index += 1
                If _Index >= _Symbols.Count Then
                    _Index = 0
                End If
            End If

            Console.Write(CurrentSymbol)

        End Sub

    End Class

#End Region

#Region " Version Check "

    Private Sub CheckS3(silent As Boolean)

        Try
            Dim client As Net.WebClient = New Net.WebClient()
            Using reader As New IO.StreamReader(client.OpenRead(s3VersionText))

                Dim av As New Version(reader.ReadToEnd)

                If My.Application.Info.Version.CompareTo(av) < 0 Then
                    Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("{0} version {1} is available. Use {2} -{3} to download.", My.Application.Info.ProductName, av.ToString, eCommandType.update.ToString.ToUpper, MyCommands.FindCommand(eCommandType.update.ToString).Options.Items(0).Keyword.ToUpper), New Textify.ColorScheme(ConsoleColor.White, ConsoleColor.DarkBlue))
                    Console.BackgroundColor = ConsoleColor.Black
                    Textify.SayBulletLine(Textify.eBullet.Arrow, s3ZipFile)
                ElseIf My.Application.Info.Version.CompareTo(av) = 0 AndAlso Not silent Then
                    Textify.SayBullet(Textify.eBullet.Hash, String.Format("You have the latest version of {0}.", My.Application.Info.ProductName))
                ElseIf My.Application.Info.Version.CompareTo(av) > 0 AndAlso Not silent Then
                    Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("Your version: {0}", My.Application.Info.Version.ToString))
                    Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("Public version: {0}", av.ToString))
                End If

            End Using

        Catch ex As Exception
            If Not silent Then
                Textify.SayError(ex.Message)
            End If
        End Try

        Console.WriteLine()

    End Sub

#End Region

End Module
