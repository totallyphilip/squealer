Imports System.Windows.Forms

Public Class GitFlags

    Private _ShowUncommitted As Boolean = False
    Public Property ShowUncommitted As Boolean
        Get
            Return _ShowUncommitted
        End Get
        Set(value As Boolean)
            _ShowUncommitted = value
        End Set
    End Property

    Private _ShowDeleted As Boolean = False
    Public Property ShowDeleted As Boolean
        Get
            Return _ShowDeleted And _ShowUncommitted
        End Get
        Set(value As Boolean)
            _ShowDeleted = value
        End Set
    End Property

    Private _ShowHistory As Boolean = False
    Public Property ShowHistory As Boolean
        Get
            Return _ShowHistory
        End Get
        Set(value As Boolean)
            _ShowHistory = value
        End Set
    End Property

    Public ReadOnly Property GitEnabled As Boolean
        Get
            Return _ShowUncommitted
        End Get
    End Property

End Class

Module Main

#Region " All The Definitions "

    Private MyCommands As New CommandCatalog.CommandDefinitionList
    Private MySettings As New Settings(True)
    Private MyFileHashes As New FileHashCollection

    Private Class BatchParametersClass

        Public Enum eOutputMode
            normal
            flags
            encrypt
            test
            alter
            convert
            permanent
            [string]
        End Enum

        Private _OutputMode As eOutputMode = eOutputMode.normal
        Public Property OutputMode As eOutputMode
            Get
                Return _OutputMode
            End Get
            Set(value As eOutputMode)
                _OutputMode = value
            End Set
        End Property

    End Class

#End Region

#Region " Enums "

    Private Enum eFileAction
        directory
        edit
        generate
        fix
        compare
        delete
        checkout
    End Enum

    Private Enum eCommandType
        [about]
        [browse]
        hash
        [checkout]
        [clear]
        [compare]
        copypath
        [config]
        [connection]
        [delete]
        [directory]
        download
        [edit]
        eztool
        [exit]
        [fix]
        [generate]
        [help]
        [helpall]
        [list]
        [make]
        [nerfherder]
        [new]
        [open]
        release
        [reverse]
        [setting]
        pewpew
        [test]
        [use]
    End Enum

#End Region

#Region " Folders "

    ' Set a new working folder and remember it for later.
    Private Sub ChangeFolder(ByVal newpath As String, ByRef ProjectFolder As String)

        My.Computer.FileSystem.CurrentDirectory = newpath ' this will throw an error if the path is not valid
        ProjectFolder = newpath
        RememberFolder(newpath)
        Textify.SayBulletLine(Textify.eBullet.Arrow, newpath)
        Textify.SayNewLine()

        ' Temporary code to rename existing connection strings 4/3/2019
        Dim oldcs As String = newpath & "\.Squealer_cs"
        Try
            If My.Computer.FileSystem.FileExists(oldcs) Then
                My.Computer.FileSystem.RenameFile(oldcs, Constants.ConnectionStringFilename)
            End If
        Catch ex As Exception
            ' suppress errors
        End Try

    End Sub

    Private Function FolderCollection() As List(Of String)

        Dim folders As New List(Of String)
        Dim unsplit As String = MySettings.RecentProjectFolders
        If Not String.IsNullOrWhiteSpace(unsplit) Then
            folders.AddRange(unsplit.Split(New Char() {"|"c}))
        End If
        While folders.Count > MySettings.ProjectFoldersLimit
            folders.RemoveAt(folders.Count - 1)
        End While
        Return folders

    End Function

    Private Function InvalidFolderIndex() As Integer

        Dim f As String = FolderCollection().Find(Function(x) Not My.Computer.FileSystem.DirectoryExists(x))
        If String.IsNullOrEmpty(f) Then ' couldn't find any bad directories
            f = FolderCollection().Find(Function(x) My.Computer.FileSystem.GetFiles(x, FileIO.SearchOption.SearchTopLevelOnly, "*" & Constants.SquealerFileExtension).Count = 0)
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
            Textify.WriteLine("All folders contain *" & Constants.SquealerFileExtension)
        Else
            While InvalidFolderIndex() > -1
                Dim i As Integer = InvalidFolderIndex()
                Textify.WriteLine("Remove: " & FolderCollection(i), ConsoleColor.Red)
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
            Throw New Exception("No saved project folders.")
        Else

            Dim farray(folders.Count - 1, 3) As String

            For i As Integer = 0 To folders.Count - 1
                farray(i, 0) = i.ToString
                farray(i, 1) = Misc.ProjectName(folders(i))
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
                Dim s As String = FolderCollection.Find(Function(x) ProjectName(x).ToLower.StartsWith(NewFolder.ToLower))
                If String.IsNullOrEmpty(s) Then
                    s = FolderCollection.Find(Function(x) ProjectName(x).ToLower.Contains(NewFolder.ToLower))
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
            MySettings.RecentProjectFolders = FolderString(folders)
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
        MySettings.RecentProjectFolders = FolderString(folders)

    End Sub

#End Region

#Region " Main Functions "

    ' Main module. Start here.
    Sub Main()

        My.Logging.WriteLog("Startup.")

        DefineCommands()

        ' Increase input buffer size.
        Console.SetIn(New IO.StreamReader(Console.OpenStandardInput(8192)))

        ' Restore the previous window size
        ResetWindowSize()

        Textify.SayCentered(Constants.HomePage, True)
        Textify.SayCentered(My.Application.Info.Copyright, True)
        Console.WriteLine()

        Dim ver As New Version(My.Configger.LoadSetting(NameOf(MySettings.LastVersionNumberExecuted), "0.0.0.0"))
        If My.Application.Info.Version.CompareTo(ver) > 0 Then ' Are we running this version for the first time?
            DisplayAboutInfo(False, False)
            My.Configger.SaveSetting(NameOf(MySettings.LastVersionNumberExecuted), My.Application.Info.Version.ToString)
        ElseIf Not MySettings.LastVersionCheckDate.Date = DateTime.Now.Date Then ' Is a newer version available?
            My.Configger.SaveSetting(NameOf(MySettings.LastVersionCheckDate), DateTime.Now)
            Dim v As New VersionCheck
            v.DisplayVersionCheckResults(MySettings.MediaSourceUrl, MySettings.IsDefaultMediaSource)
        End If

        ' Main process
        Console.WriteLine()
        If Misc.IsStarWarsDay() Then
            Console.WriteLine("May the Fourth be with you! (easter egg revealed - see HELP)")
            Console.WriteLine()
        End If
        If MySettings.ShowLeaderboardAtStartup Then
            ShowLeaderboard(10)
        End If
        GetLatestEz()

        Textify.SayBulletLine(Textify.eBullet.Hash, "Type HELP to get started.")
        Console.WriteLine()

        ' Restore the previous working folder
        Dim WorkingFolder As String = MySettings.LastProjectFolder
        If My.Computer.FileSystem.DirectoryExists(WorkingFolder) Then
            ChangeFolder(WorkingFolder, WorkingFolder)
        End If

        HandleUserInput(WorkingFolder)

        SaveWindowSize()

        ' Save the current working folder for next time
        MySettings.LastProjectFolder = WorkingFolder

        ' Delete any settings that weren't referenced
        My.Configger.PruneSettings()

        My.Logging.WriteLog("Shutdown.")

    End Sub

    Private Sub GetLatestEz()
        Dim v As New VersionCheck
        v.DownloadLatestEzBinary(MySettings.MediaSourceUrl & EzBinFilename(), EzBinPath) ' always get latest binary
    End Sub

    Private Function FilesToProcess(ByVal ProjectFolder As String, ByVal Wildcard As String, SearchText As String, usedialog As Boolean, filter As SquealerObjectTypeCollection, ignoreCase As Boolean, FindExact As Boolean, hasPrePostCode As Boolean, gf As GitFlags, DifferentOnly As Boolean) As List(Of String)

        If DifferentOnly AndAlso MyFileHashes.Items.Count = 0 Then
            Throw New ArgumentException(String.Format("Cannot use -DIFF before {0} is executed (see {1} {0})", eCommandType.hash.ToString.ToUpper, eCommandType.help.ToString.ToUpper))
        End If

        Wildcard = Wildcard.Replace("[", "").Replace("]", "")

        Dim plaincolor As New Textify.ColorScheme(ConsoleColor.Gray, ConsoleColor.Black)
        Dim highlightcolor As New Textify.ColorScheme(ConsoleColor.Cyan, ConsoleColor.Black)
        Dim gitcolor As New Textify.ColorScheme(ConsoleColor.Red, ConsoleColor.Black)

        Textify.SayBullet(Textify.eBullet.Hash, "")
        Textify.Write("finding", plaincolor)

        Dim comma As String = Nothing


        If filter.AllSelected Then
            Textify.Write(" all", highlightcolor)
            If gf.GitEnabled Then
                Textify.Write(" uncommitted case-sensitive", gitcolor)
            End If
        Else

            If gf.GitEnabled Then
                Textify.Write(" uncommitted case-sensitive", gitcolor)
            End If

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
        If hasPrePostCode Then
            Textify.Write(" with ", plaincolor)
            Textify.Write("pre/post code", highlightcolor)
        End If
        If Not String.IsNullOrEmpty(SearchText) Then
            Textify.Write(" containing ", plaincolor)
            Textify.Write("""" & SearchText & """" & IIf(ignoreCase, "", "(case-sensitive)").ToString, highlightcolor)
        End If

        If DifferentOnly Then
            Textify.Write(" different than", plaincolor)
            Textify.Write(String.Format(" {1} ({0})", MyFileHashes.Project, MyFileHashes.Branch), highlightcolor)
        End If

        Textify.Write(" matching", plaincolor)

        comma = ""

        For Each s As String In Wildcard.Split((New Char() {"|"c}))
            If s.ToLower.Contains(Constants.SquealerFileExtension.ToLower) Then
                Console.WriteLine()
                Throw New ArgumentException(s.Trim & " search term contains explicit reference To " & Constants.SquealerFileExtension)
            End If
            s = Misc.WildcardInterpreter(s.Trim, MySettings.WildcardBehavior, FindExact)
            Textify.Write(comma & " " & s, highlightcolor)
            comma = ", "

            If gf.GitEnabled Then

                If Not String.IsNullOrEmpty(SearchText) Then
                    gf.ShowDeleted = False ' there will not be any deleted files that contain search text
                End If

                Dim FoundFiles As New List(Of String)

                FoundFiles.AddRange(GitShell.ChangedFiles(ProjectFolder, "git status -s", s, gf.ShowDeleted).FindAll(Function(x) x.ToLower Like s.ToLower))

                ' Remove any results that don't contain the search text
                If gf.GitEnabled AndAlso Not String.IsNullOrEmpty(SearchText) Then
                    If ignoreCase Then
                        FoundFiles.RemoveAll(Function(x) Not My.Computer.FileSystem.ReadAllText(x).ToLower.Contains(SearchText.ToLower))
                    Else
                        FoundFiles.RemoveAll(Function(x) Not My.Computer.FileSystem.ReadAllText(x).Contains(SearchText))
                    End If
                End If

                EverythingIncludingDuplicates.AddRange(FoundFiles)

            ElseIf String.IsNullOrEmpty(SearchText) Then
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
            DistinctFiles.RemoveAll(Function(x) Not pickedfiles.Exists(Function(y) y.ToLower = x.ToLower))
        End If

        ' Remove any results that don't match the requested object types
        For Each t As SquealerObjectType In filter.Items.Where(Function(x) Not x.Selected)
            DistinctFiles.RemoveAll(Function(x) SquealerObjectType.Eval(XmlGetObjectType(x)) = t.LongType)
        Next

        ' Remove any results that don't have pre/post code
        If hasPrePostCode Then
            DistinctFiles.RemoveAll(Function(x) Not PrePostCodeExists(x))
        End If

        ' Remove any results that are the same as the baseline.
        If DifferentOnly Then
            DistinctFiles.RemoveAll(Function(x) MyFileHashes.MatchExists(x))
        End If

        Return DistinctFiles

    End Function

    ' Enumerate through files in the working folder and take some action on them.
    Private Sub ProcessFiles(ByVal FileListing As List(Of String), ByVal Action As eFileAction, bp As BatchParametersClass, ByVal TargetFileType As SquealerObjectType.eType, git As GitFlags)

        Dim FileCount As Integer = 0
        Dim SkippedFiles As Integer = 0
        Dim GeneratedOutput As String = String.Empty

        If bp.OutputMode = BatchParametersClass.eOutputMode.string Then
            Console.Write(eCommandType.directory.ToString.ToLower & " - x ")
        Else

            If MySettings.DirectoryStyleSelected = Settings.DirectoryStyle.Full Then
                Textify.Write("Type Flags ")
            ElseIf MySettings.DirectoryStyleSelected = Settings.DirectoryStyle.Compact Then
                Textify.Write("   ")
            ElseIf MySettings.DirectoryStyleSelected = Settings.DirectoryStyle.Symbolic Then
                Textify.Write(" ")
            End If

            Textify.WriteLine("FileName")

            If MySettings.DirectoryStyleSelected = Settings.DirectoryStyle.Full Then
                Textify.Write("---- ----- ")
            ElseIf MySettings.DirectoryStyleSelected = Settings.DirectoryStyle.Compact Then
                Textify.Write("-- ")
            End If

            Textify.WriteToEol("-"c)
            Console.WriteLine()

        End If


        Dim NextPercentageStep As Integer = MySettings.OutputPercentageIncrement


        For Each FileName As String In FileListing

            If Console.KeyAvailable() Then
                Throw New System.Exception("Keyboard interrupt.")
            End If

            BracketCheck(FileName)

            Dim info As IO.FileInfo = My.Computer.FileSystem.GetFileInfo(FileName)

            Dim obj As New SquealerObject(FileName)


            If bp.OutputMode = BatchParametersClass.eOutputMode.string Then
                If FileCount > 0 Then
                    Console.Write("|")
                End If
                Console.Write(info.Name.Replace(Constants.SquealerFileExtension, ""))
            Else
                Dim fg As ConsoleColor = ConsoleColor.Gray




                If obj.Type.LongType = SquealerObjectType.eType.Invalid Then
                    fg = ConsoleColor.Red
                End If


                If MySettings.DirectoryStyleSelected = Settings.DirectoryStyle.Full Then
                    Textify.Write(" " & obj.Type.ShortType.ToString.PadRight(4) & obj.FlagsSummary)
                ElseIf MySettings.DirectoryStyleSelected = Settings.DirectoryStyle.Compact Then
                    Textify.Write(obj.Type.ShortType.ToString.PadRight(2))
                    If String.IsNullOrWhiteSpace(obj.FlagsSummary) Then
                        Textify.Write(" ")
                    Else
                        Textify.Write("*")
                    End If
                ElseIf MySettings.DirectoryStyleSelected = Settings.DirectoryStyle.Symbolic Then
                    If String.IsNullOrWhiteSpace(obj.FlagsSummary) Then
                        Textify.Write(" ")
                    Else
                        Textify.Write("*")
                    End If
                End If


                Textify.Write(info.Name.Replace(Constants.SquealerFileExtension, ""), fg)

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

                If MySettings.DirectoryStyleSelected = Settings.DirectoryStyle.Symbolic Then
                    Textify.Write(symbol, ConsoleColor.Green)
                End If

            End If

            Try

                Dim gitstatuscode As String = String.Empty
                If git.ShowUncommitted Then
                    gitstatuscode = " " & GitShell.FileSearch(info.DirectoryName, "git status -s ", info.Name)(0).Replace(info.Name, "").TrimStart
                End If

                Select Case Action
                    Case eFileAction.directory
                        If bp.OutputMode = BatchParametersClass.eOutputMode.flags AndAlso obj.FlagsList.Count > 0 Then
                            Console.WriteLine()
                            Console.WriteLine("           {")
                            For Each s As String In obj.FlagsList
                                Console.WriteLine("             " & s)
                            Next
                            Console.Write("           }")
                        End If
                    Case eFileAction.fix
                        If bp.OutputMode = BatchParametersClass.eOutputMode.normal Then
                            If RepairXmlFile(False, info.FullName) Then
                                Textify.Write(String.Format(" ... {0}", eCommandType.fix.ToString.ToUpper), ConsoleColor.Green)
                            Else
                                SkippedFiles += 1
                            End If
                        Else
                            If ConvertXmlFile(info.FullName, TargetFileType) Then
                                Textify.Write(String.Format(" ... {0}", BatchParametersClass.eOutputMode.convert.ToString.ToUpper))
                            Else
                                SkippedFiles += 1
                            End If
                        End If

                    Case eFileAction.edit
                        EditFile(info.FullName)
                    Case eFileAction.checkout
                        GitShell.DisplayResults(info.DirectoryName, "git checkout -- " & info.Name, " (oops, wut happened)")
                    Case eFileAction.generate
                        GeneratedOutput &= ExpandIndividual(info, GetStringReplacements(My.Computer.FileSystem.GetFileInfo(FileListing(0)).DirectoryName), bp, FileCount + 1, FileListing.Count, MySettings.OutputStepStyleSelected = Settings.OutputStepStyle.Detailed)

                        If MySettings.OutputStepStyleSelected = Settings.OutputStepStyle.Percentage Then
                            Dim CurrentPercentage As Double = ((FileCount + 1) / FileListing.Count) * 100
                            If CurrentPercentage >= NextPercentageStep OrElse (FileCount = 0 AndAlso CurrentPercentage < 1) Then
                                While NextPercentageStep <= CurrentPercentage
                                    NextPercentageStep += MySettings.OutputPercentageIncrement
                                End While
                                GeneratedOutput &= vbCrLf & String.Format("print '{0}% ({1}/{2})';", NextPercentageStep - MySettings.OutputPercentageIncrement, FileCount + 1, FileListing.Count) & vbCrLf
                            End If
                        End If

                    Case eFileAction.compare
                        Dim RootName As String = info.Name.Replace(Constants.SquealerFileExtension, "")
                        GeneratedOutput &= String.Format("insert #CodeToDrop ([Type], [Schema], [Name]) values ('{0}','{1}','{2}');", obj.Type.GeneralType, SchemaName(RootName), RoutineName(RootName)) & vbCrLf
                    Case eFileAction.delete
                        Dim trashcan As FileIO.RecycleOption = FileIO.RecycleOption.SendToRecycleBin
                        If bp.OutputMode = BatchParametersClass.eOutputMode.permanent Then
                            trashcan = FileIO.RecycleOption.DeletePermanently
                        End If
                        My.Computer.FileSystem.DeleteFile(info.FullName, FileIO.UIOption.OnlyErrorDialogs, trashcan)
                End Select
                If Not bp.OutputMode = BatchParametersClass.eOutputMode.string Then

                    If git.ShowUncommitted Then
                        'Try
                        ' This will fail if we just now deleted the file
                        'Textify.Write(" " & GitResults(info.DirectoryName, "git status -s ", info.Name)(0).Replace(info.Name, "").TrimStart, ConsoleColor.Red)
                        Textify.Write(gitstatuscode, ConsoleColor.Red)
                        'Catch ex As Exception
                        'End Try
                    End If
                    If git.ShowHistory Then
                        GitShell.DisplayResults(info.DirectoryName, "git log --pretty=format:""%h (%cr) %s"" " & info.Name, " (no history)")
                    End If

                    Console.WriteLine()

                End If
                FileCount += 1
            Catch ex As Exception
                Textify.WriteLine(" ... FAILED!", ConsoleColor.Red)
                Throw New Exception(ex.Message)
            End Try

        Next

        If FileCount > 0 Then
            If bp.OutputMode = BatchParametersClass.eOutputMode.string Then
                Textify.SayNewLine()
            End If
            Textify.SayNewLine()
        End If

        Dim SummaryLine As String = "{4}/{0} files ({3} skipped) (action:{1}, mode:{2})"
        If SkippedFiles = 0 Then
            SummaryLine = "{0} files (action:{1}, mode:{2})"
        End If

        Textify.SayBulletLine(Textify.eBullet.Hash, String.Format(SummaryLine, FileCount.ToString, Action.ToString, bp.OutputMode.ToString, SkippedFiles.ToString, (FileCount - SkippedFiles).ToString))


        If (Action = eFileAction.generate OrElse Action = eFileAction.compare) AndAlso FileCount > 0 Then

            If Action = eFileAction.compare Then
                GeneratedOutput = My.Resources.CompareObjects.Replace("{RoutineList}", GeneratedOutput).Replace("{ExcludeFilename}", Constants.AutocreateFilename)
            ElseIf Not bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                If MySettings.DetectDeprecatedSquealerObjects Then
                    GeneratedOutput = My.Resources._TopScript & GeneratedOutput
                End If
                If MySettings.EnableEzObjects Then
                    GeneratedOutput = EzText(False).Replace("{Schema}", MySettings.EzSchema) & GeneratedOutput
                End If
            End If

            If MySettings.OutputToClipboard Then
                Console.WriteLine()
                Textify.SayBulletLine(Textify.eBullet.Hash, "Output copied to Windows clipboard.")
                Clipboard.SetText(GeneratedOutput)
            Else
                Dim tempfile As New TempFileHandler(".sql")
                tempfile.Writeline(GeneratedOutput)
                tempfile.Show()
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

        ' show ez script only
        cmd = New CommandCatalog.CommandDefinition({eCommandType.eztool.ToString}, {"Display the EZ script from hidden options."}, CommandCatalog.eCommandCategory.other)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("encrypt;convert .sql to .bin.new"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("extract;convert .bin or embedded resource file to .sql"))
        cmd.Visible = False
        MyCommands.Items.Add(cmd)

        ' open folder
        cmd = New CommandCatalog.CommandDefinition({eCommandType.open.ToString}, {"Open folder {options}.", "This folder path will be saved for quick access. See " & eCommandType.list.ToString.ToUpper & " command. Omit path to open folder dialog."}, CommandCatalog.eCommandCategory.folder, "<path>", False)
        cmd.Examples.Add("% " & Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        cmd.IgnoreSwitches = True
        MyCommands.Items.Add(cmd)

        ' list folders
        cmd = New CommandCatalog.CommandDefinition({eCommandType.list.ToString, "l"}, {"List or remove project folders.", "Set maximum list size in General tab of settings. Removing folders from the list does NOT delete folders from the filesystem."}, CommandCatalog.eCommandCategory.folder, "project folder number", False)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("trim;remove all invalid or non-Squealer folders"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("remove;remove a specific project folder"))
        cmd.Examples.Add("% -trim -- clean up list")
        cmd.Examples.Add("% 3 -remove -- remove project folder 3 from list")
        MyCommands.Items.Add(cmd)

        ' use folder
        cmd = New CommandCatalog.CommandDefinition({eCommandType.use.ToString}, {"Reopen a saved folder.", "See " & eCommandType.list.ToString.ToUpper & " command."}, CommandCatalog.eCommandCategory.folder, "<project name or folder number>", True)
        cmd.Examples.Add("% 3")
        cmd.Examples.Add("% northwind")
        MyCommands.Items.Add(cmd)

        ' browse
        cmd = New CommandCatalog.CommandDefinition({eCommandType.browse.ToString, "b"}, {"Open file browser.", "Browse files in the current project folder. If {options} is specified, only matching files will be displayed."}, CommandCatalog.eCommandCategory.folder, CommandCatalog.CommandDefinition.WildcardText, False)
        cmd.Examples.Add("% *employee*")
        MyCommands.Items.Add(cmd)

        ' copy path
        cmd = New CommandCatalog.CommandDefinition({eCommandType.copypath.ToString, "cp"}, {"Copy working folder path to clipboard.", "Copies the full path of the current working folder into the Windows clipboard.."}, CommandCatalog.eCommandCategory.folder)
        MyCommands.Items.Add(cmd)

        ' checkout
        cmd = New CommandCatalog.CommandDefinition({eCommandType.checkout.ToString, "undo"}, {"Git checkout.", "Checkout objects from Git and discard local changes."}, CommandCatalog.eCommandCategory.file, True, True)
        MyCommands.Items.Add(cmd)

        ' dir
        cmd = New CommandCatalog.CommandDefinition({eCommandType.directory.ToString, "dir"}, {"Directory.", String.Format("List {0} objects in the current working folder.", My.Application.Info.ProductName)}, CommandCatalog.eCommandCategory.file, False, True)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("h;show git history"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("f;show flags"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("str;string output"))
        cmd.Examples.Add("% -cs dbo.* / Han shot first -- find all dbo.* files containing "" Han shot first"" (with leading space and capital H)")
        cmd.Examples.Add("% -p -v /Solo -- find all stored procedures and views containing ""Solo"" (or ""solo"" or ""SOLO"" or ""soLO"")")
        MyCommands.Items.Add(cmd)

        ' new file
        cmd = New CommandCatalog.CommandDefinition({eCommandType.new.ToString}, {String.Format("Create a new {0} file.", My.Application.Info.ProductName), "Default schema is ""dbo""."}, CommandCatalog.eCommandCategory.file, CommandCatalog.CommandDefinition.FilenameText, True)
        For Each s As String In New SquealerObjectTypeCollection().ObjectTypesOptionString(False).Split((New Char() {"|"c}))
            cmd.Options.Items.Add(New CommandCatalog.CommandSwitch(s, s.StartsWith("p")))
        Next
        cmd.Examples.Add("% AddEmployee -- create new stored procedure dbo.AddEmployee")
        cmd.Examples.Add("% -v myschema.Employees -- create new view myschema.Employees")
        MyCommands.Items.Add(cmd)

        ' edit files
        cmd = New CommandCatalog.CommandDefinition({eCommandType.edit.ToString, "e"}, {String.Format("Edit {0} files.", My.Application.Info.ProductName), String.Format("Uses your configured text editor. See {0} command.", eCommandType.setting.ToString.ToUpper)}, CommandCatalog.eCommandCategory.file, False, True)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("all;override file limit"))
        cmd.Examples.Add("% dbo.AddEmployee")
        cmd.Examples.Add("% dbo.*")
        MyCommands.Items.Add(cmd)

        ' fix files
        cmd = New CommandCatalog.CommandDefinition({eCommandType.fix.ToString}, {String.Format("Rewrite {0} files (DESTRUCTIVE).", My.Application.Info.ProductName), String.Format("Files will be validated and reformatted to {0} specifications. Optionally convert files to a different type.", My.Application.Info.ProductName)}, CommandCatalog.eCommandCategory.file, False, True)
        opt = New CommandCatalog.CommandSwitch("c;convert to")
        For Each s As String In New SquealerObjectTypeCollection().ObjectTypesOptionString(False).Split((New Char() {"|"c}))
            opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption(s))
        Next
        cmd.Options.Items.Add(opt)
        cmd.Examples.Add("% dbo.*")
        cmd.Examples.Add("% -c:p * -- convert everything to stored procedures")
        cmd.Examples.Add("% -v -p -c:if * -- convert views and stored procedures to inline table-valued functions")
        MyCommands.Items.Add(cmd)

        ' generate
        cmd = New CommandCatalog.CommandDefinition({eCommandType.generate.ToString, "gen"}, {"Generate SQL Server CREATE or ALTER scripts.", String.Format("Output is written to a temp file and opened with your configured text editor. See {0} command.", eCommandType.setting.ToString.ToUpper)}, CommandCatalog.eCommandCategory.file, False, True)
        opt = New CommandCatalog.CommandSwitch("m;output mode")
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption("alt;alter, do not drop original"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption("t;test script, limit 1 object"))
        opt.Options.Items.Add(New CommandCatalog.CommandSwitchOption("e;with encryption"))
        cmd.Options.Items.Add(opt)
        cmd.Examples.Add("% dbo.*")
        cmd.Examples.Add("% -m:alt -v dbo.* -- generate ALTER scripts for dbo.* VIEW objects")
        cmd.Examples.Add(String.Format("% -diff * -- generate files that have changed (see {0} command)", eCommandType.hash.ToString.ToUpper))
        MyCommands.Items.Add(cmd)

        ' baseline
        cmd = New CommandCatalog.CommandDefinition({eCommandType.hash.ToString}, {String.Format("Calculate the hash values for all {0} files.", My.Application.Info.ProductName), String.Format("This is useful when working with source control such as Git. For example, {0} your files, then check out a different branch, then {1} only files that are different from the {0}. The hash values are kept in memory only; nothing is written to disk.", eCommandType.hash.ToString.ToUpper, eCommandType.generate.ToString.ToUpper)}, CommandCatalog.eCommandCategory.file)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("i;information display"))
        MyCommands.Items.Add(cmd)

        ' compare
        cmd = New CommandCatalog.CommandDefinition({eCommandType.compare.ToString}, {String.Format("Compare {0} with SQL Server.", My.Application.Info.ProductName), String.Format("This generates a T-SQL query to discover any SQL Server objects that are not in {0}, and any {0} objects that are not in SQL Server.", My.Application.Info.ProductName)}, CommandCatalog.eCommandCategory.file, False, True)
        MyCommands.Items.Add(cmd)

        ' delete
        cmd = New CommandCatalog.CommandDefinition({eCommandType.delete.ToString, "del"}, {String.Format("Delete {0} files.", My.Application.Info.ProductName), "Objects will be sent to the Recycle Bin by default."}, CommandCatalog.eCommandCategory.file, True, True)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("e;permanently erase"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("all;override file limit"))
        cmd.Examples.Add("% dbo.AddEmployee")
        cmd.Examples.Add("% dbo.*")
        MyCommands.Items.Add(cmd)


        ' make
        cmd = New CommandCatalog.CommandDefinition({eCommandType.make.ToString}, {String.Format("Automatically create {0} files.", My.Application.Info.ProductName), "Create default INSERT, UPDATE, SELECT, and DELETE files for the target database. Define the target database with the " & eCommandType.connection.ToString.ToUpper & " command."}, CommandCatalog.eCommandCategory.file)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch(String.Format("r;replace existing {0} objects only", My.Application.Info.ProductName)))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("nocomment;omit data source and timestamp from comment section"))
        MyCommands.Items.Add(cmd)

        ' reverse engineer
        cmd = New CommandCatalog.CommandDefinition({eCommandType.reverse.ToString}, {String.Format("Reverse engineer SQL objects.", My.Application.Info.ProductName), "Reverse engineer existing SQL Server procs, views, and functions from the target database. Define the target database with the " & eCommandType.connection.ToString.ToUpper & " command. Duplicate filenames will not be overwritten. Results are NOT GUARANTEED and require manual review and edits."}, CommandCatalog.eCommandCategory.file)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("clean;attempt to clean up imported code previously generated by " & My.Application.Info.ProductName))
        MyCommands.Items.Add(cmd)


        ' help
        cmd = New CommandCatalog.CommandDefinition({eCommandType.help.ToString, "h"}, {"{command} {options} for help.", "Use {command} alone for list of commands."}, CommandCatalog.eCommandCategory.other, "<command>", False)
        cmd.Examples.Add("% " & eCommandType.generate.ToString)
        MyCommands.Items.Add(cmd)

        ' helpall
        cmd = New CommandCatalog.CommandDefinition({eCommandType.helpall.ToString}, {"Show all help including hidden commands."}, CommandCatalog.eCommandCategory.other)
        cmd.Visible = False
        MyCommands.Items.Add(cmd)


        ' config
        cmd = New CommandCatalog.CommandDefinition({eCommandType.config.ToString, "c"}, {"Edit " & Constants.ConfigFilename & ".", "This file configures how " & My.Application.Info.ProductName & " operates in your current working folder."}, CommandCatalog.eCommandCategory.other)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("new;create new default file"))
        MyCommands.Items.Add(cmd)

        ' setting
        cmd = New CommandCatalog.CommandDefinition({eCommandType.setting.ToString, "set"}, {"Display application settings."}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)

        ' connection string
        cmd = New CommandCatalog.CommandDefinition({eCommandType.connection.ToString, "cs"}, {"Define the SQL Server connection string.", String.Format("The connection string is encrypted for the current local user and current working folder, and is required for some {0} commands. If you are using version control, you should add ""{1}"" to your ignore list.", My.Application.Info.ProductName, Constants.ConnectionStringFilename)}, CommandCatalog.eCommandCategory.other, "<connectionstring>", False)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("e;edit current connection string"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("t;test connection", True))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("set;encrypt and save the connection string"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("show;display the connection string"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("forget;discard the saved connection string"))
        cmd.Examples.Add("% -set " & Constants.DefaultConnectionString)
        MyCommands.Items.Add(cmd)

        ' cls
        cmd = New CommandCatalog.CommandDefinition({eCommandType.clear.ToString, "cls"}, {"Clear the console."}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)

        ' about
        cmd = New CommandCatalog.CommandDefinition({eCommandType.about.ToString}, {"Check for updates and display program information."}, CommandCatalog.eCommandCategory.other)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("whatsnew;display what's new"))
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("changelog;display full change log"))
        MyCommands.Items.Add(cmd)

        ' download
        cmd = New CommandCatalog.CommandDefinition({eCommandType.download.ToString}, {String.Format("Download the latest version of {0}.", My.Application.Info.ProductName)}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)

        ' exit
        cmd = New CommandCatalog.CommandDefinition({eCommandType.exit.ToString, "x"}, {"Quit."}, CommandCatalog.eCommandCategory.other)
        MyCommands.Items.Add(cmd)

        ' star wars
        cmd = New CommandCatalog.CommandDefinition({eCommandType.pewpew.ToString, "pew"}, {"I've got a bad feeling about this.", "Jump in an X-Wing and blow something up!"}, CommandCatalog.eCommandCategory.other)
        cmd.Options.Items.Add(New CommandCatalog.CommandSwitch("top;display leaderboard"))
        cmd.Visible = Misc.IsStarWarsDay()
        MyCommands.Items.Add(cmd)

        ' test 
        cmd = New CommandCatalog.CommandDefinition({eCommandType.test.ToString}, {"Hidden command. Debugging/testing only."}, CommandCatalog.eCommandCategory.other)
        cmd.Visible = False
        MyCommands.Items.Add(cmd)

        ' release
        cmd = New CommandCatalog.CommandDefinition({eCommandType.release.ToString}, {String.Format("Create files for {0} release.", My.Application.Info.Version)}, CommandCatalog.eCommandCategory.other)
        cmd.Visible = False
        MyCommands.Items.Add(cmd)

    End Sub

    Private Function StringInList(l As List(Of String), s As String) As Boolean
        Return l.Exists(Function(x) x.ToLower = s.ToLower)
    End Function


    ' The main command interface loop.
    Private Sub HandleUserInput(ByRef WorkingFolder As String)

        Dim MySwitches As New List(Of String)
        Dim UserInput As String = Nothing
        Dim RawUserInput As String = String.Empty

        Dim MyCommand As CommandCatalog.CommandDefinition = MyCommands.FindCommand(eCommandType.nerfherder.ToString)
        Dim SwitchesValidated As Boolean = True
        Dim MySearchText As String = String.Empty
        Dim ObjectTypeFilter As New SquealerObjectTypeCollection
        Dim FirstLoop As Boolean = True



        While Not (MyCommand IsNot Nothing AndAlso MyCommand.Keyword = eCommandType.exit.ToString AndAlso SwitchesValidated AndAlso String.IsNullOrEmpty(UserInput))

            Try

                If MyCommand IsNot Nothing AndAlso MyCommand.Keyword = eCommandType.nerfherder.ToString AndAlso FirstLoop Then

                    ' do nothing


                ElseIf Not SwitchesValidated Then

                    Throw New Exception("Invalid command switch.")


                ElseIf MyCommand IsNot Nothing AndAlso MyCommand.ParameterRequired AndAlso String.IsNullOrEmpty(UserInput) Then

                    Throw New Exception("Required parameter is missing.")


                ElseIf MyCommand IsNot Nothing AndAlso String.IsNullOrEmpty(MyCommand.ParameterDefinition) AndAlso Not String.IsNullOrEmpty(UserInput) Then

                    Throw New Exception("Unexpected command parameter.")


                ElseIf MyCommand Is Nothing Then

                    Throw New System.Exception(Constants.BadCommandMessage)


                ElseIf MyCommand.Keyword = eCommandType.about.ToString AndAlso SwitchesValidated Then

                    DisplayAboutInfo(StringInList(MySwitches, "whatsnew"), StringInList(MySwitches, "changelog"))



                ElseIf MyCommand.Keyword = eCommandType.clear.ToString Then

                    Console.Clear()



                ElseIf MyCommand.Keyword = eCommandType.copypath.ToString AndAlso String.IsNullOrEmpty(UserInput) Then

                    Clipboard.SetText(WorkingFolder)
                    Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("Copied: {0}", WorkingFolder))
                    Console.WriteLine()


                ElseIf MyCommand.Keyword = eCommandType.download.ToString AndAlso String.IsNullOrEmpty(UserInput) Then

                    Dim v As New VersionCheck
                    v.DownloadLatestInstaller(MySettings.MediaSourceUrl)


                ElseIf MyCommand.Keyword = eCommandType.eztool.ToString AndAlso StringInList(MySwitches, "encrypt") Then

                    EzConvertSqlToNewBin()


                ElseIf MyCommand.Keyword = eCommandType.eztool.ToString AndAlso StringInList(MySwitches, "extract") Then

                    EzExtractSqlToFile()


                ElseIf MyCommand.Keyword = eCommandType.eztool.ToString AndAlso MySwitches.Count = 0 Then

                    Dim f As New TempFileHandler("sql")
                    f.Writeline(EzText(False).Replace("{Schema}", MySettings.EzSchema))
                    f.Show()



                ElseIf MyCommand.Keyword = eCommandType.hash.ToString AndAlso StringInList(MySwitches, "i") AndAlso String.IsNullOrWhiteSpace(UserInput) Then

                    Textify.WriteLine("Latest baseline:", ConsoleColor.White)
                    Console.WriteLine()
                    Console.WriteLine(String.Format("Last snapshot at {0}", MyFileHashes.CacheDate.ToString))
                    Console.WriteLine(String.Format("{0} files", MyFileHashes.Items.Count.ToString))
                    Console.WriteLine(String.Format("project: {0}, branch: {1}", MyFileHashes.Project, MyFileHashes.Branch))
                    Console.WriteLine()


                ElseIf MyCommand.Keyword = eCommandType.hash.ToString AndAlso String.IsNullOrWhiteSpace(UserInput) Then

                    Console.Write("Calculating hashes..")

                    Dim spinny As New SpinCursor()

                    Dim i As Integer = 0

                    MyFileHashes.Reset(Misc.ProjectName(WorkingFolder), GitShell.CurrentBranch(WorkingFolder))
                    For Each f As String In My.Computer.FileSystem.GetFiles(WorkingFolder, FileIO.SearchOption.SearchTopLevelOnly, "*" & Constants.SquealerFileExtension)
                        i += 1
                        MyFileHashes.AddItem(f)
                        spinny.Animate()
                    Next

                    Console.WriteLine()
                    Console.WriteLine(String.Format("{0} files hashed.", i.ToString))
                    Console.WriteLine()
                    Textify.WriteLine("New baseline stored in memory.", ConsoleColor.White)
                    Console.WriteLine()

                    'foooooooooooooooooooooo




                ElseIf MyCommand.Keyword = eCommandType.[config].ToString Then

                    ' Try to make a new file
                    If StringInList(MySwitches, "new") Then
                        If My.Computer.FileSystem.FileExists(WorkingFolder & "\" & Constants.ConfigFilename) Then
                            Throw New Exception("Config file already exists.")
                        Else
                            My.Computer.FileSystem.WriteAllText(WorkingFolder & "\" & Constants.ConfigFilename, My.Resources.UserConfig, False)
                            SayFileAction("config file created", WorkingFolder, Constants.ConfigFilename)
                            Textify.SayNewLine()
                        End If
                        Textify.SayNewLine()
                    End If

                    ' Now edit 
                    OpenInTextEditor(Constants.ConfigFilename, WorkingFolder)


                ElseIf MyCommand.Keyword = eCommandType.[delete].ToString _
                    OrElse MyCommand.Keyword = eCommandType.directory.ToString _
                    OrElse MyCommand.Keyword = eCommandType.checkout.ToString _
                    OrElse MyCommand.Keyword = eCommandType.[generate].ToString _
                    OrElse MyCommand.Keyword = eCommandType.edit.ToString _
                    OrElse MyCommand.Keyword = eCommandType.fix.ToString _
                    OrElse MyCommand.Keyword = eCommandType.compare.ToString Then


                    Dim FileLimit As Integer = Integer.MaxValue
                    Dim action As eFileAction = eFileAction.directory
                    Dim bp As New BatchParametersClass
                    Dim targetftype As SquealerObjectType.eType = SquealerObjectType.eType.Invalid ' for object conversion only
                    Dim gf As New GitFlags()
                    gf.ShowUncommitted = StringInList(MySwitches, "u")

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
                            bp.OutputMode = BatchParametersClass.eOutputMode.permanent
                        End If

                        FileLimit = 20



                    ElseIf MyCommand.Keyword = eCommandType.checkout.ToString Then

                        action = eFileAction.checkout
                        gf.ShowUncommitted = True
                        gf.ShowDeleted = True


                        'FileLimit = 20




                    ElseIf MyCommand.Keyword = eCommandType.directory.ToString Then

                        If gf.ShowUncommitted Then
                            gf.ShowDeleted = True
                        End If
                        If StringInList(MySwitches, "h") Then
                            gf.ShowHistory = True
                        End If
                        If StringInList(MySwitches, "f") Then
                            bp.OutputMode = BatchParametersClass.eOutputMode.flags
                        ElseIf StringInList(MySwitches, "str") Then
                            bp.OutputMode = BatchParametersClass.eOutputMode.string
                        End If


                    ElseIf MyCommand.Keyword = eCommandType.edit.ToString Then

                        action = eFileAction.edit

                        FileLimit = 10


                    ElseIf MyCommand.Keyword = eCommandType.generate.ToString Then

                        action = eFileAction.generate

                        If StringInList(MySwitches, "m:t") Then
                            bp.OutputMode = BatchParametersClass.eOutputMode.test
                            FileLimit = 1
                        ElseIf StringInList(MySwitches, "m:e") Then
                            bp.OutputMode = BatchParametersClass.eOutputMode.encrypt
                        ElseIf StringInList(MySwitches, "m:alt") Then
                            bp.OutputMode = BatchParametersClass.eOutputMode.alter
                        End If


                    ElseIf MyCommand.Keyword = eCommandType.fix.ToString Then

                        action = eFileAction.fix

                        Dim convertswitch As String = MySwitches.Find(Function(x) x.Split(New Char() {":"c})(0).ToLower = "c")
                        If Not String.IsNullOrWhiteSpace(convertswitch) Then
                            targetftype = SquealerObjectType.Eval(convertswitch.Split(New Char() {":"c})(1))
                        End If

                        If Not targetftype = SquealerObjectType.eType.Invalid Then
                            bp.OutputMode = BatchParametersClass.eOutputMode.convert
                        End If

                    ElseIf MyCommand.Keyword = eCommandType.compare.ToString Then

                        action = eFileAction.compare

                    End If




                    Dim ignoreCase As Boolean = Not StringInList(MySwitches, "cs")
                    Dim findexact As Boolean = StringInList(MySwitches, "x")
                    Dim ignorefilelimit As Boolean = StringInList(MySwitches, "all")
                    Dim findPrePost As Boolean = StringInList(MySwitches, "code")

                    Dim SelectedFiles As List(Of String) = FilesToProcess(WorkingFolder, UserInput, MySearchText, usedialog, ObjectTypeFilter, ignoreCase, findexact, findPrePost, gf, StringInList(MySwitches, "diff"))

                    ThrowErrorIfOverFileLimit(FileLimit, SelectedFiles.Count, ignorefilelimit)

                    ProcessFiles(SelectedFiles, action, bp, targetftype, gf)







                ElseIf MyCommand.Keyword = eCommandType.[help].ToString Then

                    If String.IsNullOrEmpty(UserInput) Then
                        MyCommands.ShowHelpCatalog(False)
                    Else
                        Dim HelpWithCommand As CommandCatalog.CommandDefinition = MyCommands.FindCommand(UserInput)

                        If HelpWithCommand IsNot Nothing Then
                            HelpWithCommand.ShowHelp()
                        Else
                            Throw New Exception("Unknown command.")
                        End If
                    End If


                ElseIf MyCommand.Keyword = eCommandType.[helpall].ToString AndAlso String.IsNullOrWhiteSpace(UserInput) Then

                    MyCommands.ShowHelpCatalog(True)



                ElseIf MyCommand.Keyword = eCommandType.[list].ToString AndAlso StringInList(MySwitches, "remove") AndAlso Not String.IsNullOrEmpty(UserInput) Then

                    ForgetFolder(UserInput)
                    Textify.SayNewLine()




                ElseIf MyCommand.Keyword = eCommandType.[list].ToString AndAlso StringInList(MySwitches, "trim") AndAlso String.IsNullOrEmpty(UserInput) Then

                    AutoRemoveFolders()
                    Textify.SayNewLine()



                ElseIf MyCommand.Keyword = eCommandType.[list].ToString AndAlso MySwitches.Count = 0 AndAlso String.IsNullOrEmpty(UserInput) Then

                    ListFolders(WorkingFolder)





                ElseIf MyCommand.Keyword = eCommandType.[new].ToString Then

                    Dim filetype As SquealerObjectType.eType = SquealerObjectType.eType.StoredProcedure
                    If ObjectTypeFilter.SelectedCount > 0 Then
                        filetype = ObjectTypeFilter.Items.Find(Function(x) x.Selected).LongType
                    End If

                    BracketCheck(UserInput)

                    Dim f As String = CreateNewFile(WorkingFolder, filetype, UserInput)

                    If MySettings.AutoEditNewFiles AndAlso Not String.IsNullOrEmpty(f) Then
                        EditFile(f)
                    End If


                ElseIf MyCommand.Keyword = eCommandType.[open].ToString Then

                    If String.IsNullOrWhiteSpace(UserInput) Then
                        Dim f As New System.Windows.Forms.FolderBrowserDialog
                        f.ShowDialog()
                        UserInput = f.SelectedPath
                        Textify.SayBulletLine(Textify.eBullet.Carat, UserInput)
                    End If
                    ChangeFolder(UserInput, WorkingFolder)




                ElseIf MyCommand.Keyword = eCommandType.setting.ToString Then

                    Dim w As Boolean = MySettings.LockWindowSize
                    MySettings.Show()
                    If MySettings.LockWindowSize AndAlso Not w Then
                        SaveWindowSize()
                    End If


                ElseIf MyCommand.Keyword = eCommandType.browse.ToString Then

                    'OpenExplorer(Misc.WildcardInterpreter(UserInput, MySettings.WildcardBehavior, False), WorkingFolder)


                    Dim SelectedFiles As List(Of String) = OpenExplorer(Misc.WildcardInterpreter(UserInput, MySettings.WildcardBehavior, False), WorkingFolder) '  FilesToProcess(WorkingFolder, UserInput, MySearchText, True, ObjectTypeFilter, True, False, False, False, New GitFlags)

                    If SelectedFiles.Count > 0 Then
                        ProcessFiles(SelectedFiles, eFileAction.edit, New BatchParametersClass, SquealerObjectType.eType.Invalid, New GitFlags)
                    End If






                ElseIf MyCommand.Keyword = eCommandType.[use].ToString Then

                    LoadFolder(UserInput, WorkingFolder)


                ElseIf MyCommand.Keyword = eCommandType.pewpew.ToString Then

                    If StringInList(MySwitches, "top") Then
                        ShowLeaderboard(20)
                    Else
                        Dim fgColor As ConsoleColor = Console.ForegroundColor
                        Dim bgColor As ConsoleColor = Console.BackgroundColor
                        Dim fight As New GoldLeader(False)
                        fight.TryPlay(MySettings.LeaderboardConnectionString)
                        Console.ForegroundColor = fgColor
                        Console.BackgroundColor = bgColor
                        Console.WriteLine()
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
                        cs = Constants.DefaultConnectionString
                    End Try
                    cs = Microsoft.VisualBasic.Interaction.InputBox("Connection String", "", cs)
                    If Not String.IsNullOrWhiteSpace(cs) Then
                        SetConnectionString(WorkingFolder, cs)
                        Textify.SayBulletLine(Textify.eBullet.Arrow, cs)
                        Textify.SayNewLine()
                    End If



                ElseIf MyCommand.Keyword = eCommandType.make.ToString Then


                    Automagic(GetConnectionString(WorkingFolder), WorkingFolder, StringInList(MySwitches, "r"), Not StringInList(MySwitches, "nocomment"))




                ElseIf MyCommand.Keyword = eCommandType.reverse.ToString Then

                    ReverseEngineer(GetConnectionString(WorkingFolder), WorkingFolder, StringInList(MySwitches, "clean"))





                ElseIf MyCommand.Keyword = eCommandType.release.ToString Then

                    Dim v As New VersionCheck
                    v.CreateMetadata(MySettings.MediaSourceUrl)



                ElseIf MyCommand.Keyword = "test" Then 'footest

                    For Each f As String In My.Computer.FileSystem.GetFiles(WorkingFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.sqlr")
                        If Not MyFileHashes.MatchExists(f) Then
                            Console.Write(f.Remove(0, f.LastIndexOf("\")))
                            Console.WriteLine(" ... DIFFERENT")
                        End If

                    Next


                Else
                    Throw New System.Exception(Constants.BadCommandMessage)
                End If

            Catch ex As Exception

                Textify.SayError(ex.Message)

                If MyCommand Is Nothing OrElse MyCommand.Keyword = eCommandType.nerfherder.ToString Then
                    Textify.SayBulletLine(Textify.eBullet.Hash, "Try: HELP")
                Else
                    Textify.SayBulletLine(Textify.eBullet.Hash, "Try: HELP " & MyCommand.Keyword.ToUpper)
                End If

                Textify.SayNewLine()

                My.Logging.WriteLog("Command error: " & RawUserInput)
                My.Logging.WriteLog(ex.Message & vbCrLf & ex.StackTrace)


            End Try

            FirstLoop = False

            Console.Title = Misc.TitleText(MySettings.ShowProjectNameInTitleBar, MySettings.ShowProjectDirectoryInTitleBar, WorkingFolder)

            UserInput = String.Empty
            While String.IsNullOrWhiteSpace(UserInput)

                If Not My.Computer.FileSystem.DirectoryExists(WorkingFolder) Then
                    Textify.SayError(WorkingFolder, Textify.eSeverity.error, True)
                    Console.WriteLine()
                End If

                If MySettings.ShowProjectNameInCommandPrompt Then
                    Textify.Write(String.Format("[{0}] ", Misc.ProjectName(WorkingFolder)), ConsoleColor.DarkYellow)
                End If
                If MySettings.ShowGitBranch Then
                    Dim s As String = GitShell.CurrentBranch(WorkingFolder)
                    Dim c As ConsoleColor = ConsoleColor.DarkGreen
                    If s = GitShell.GitErrorMessage Then
                        c = ConsoleColor.Red
                    End If
                    Textify.Write(String.Format("({0}) ", s), c)
                End If
                Textify.Write("> ", ConsoleColor.DarkYellow)
                ClearKeyboard()

                Dim KA As New KeepAlive
                If MySettings.KeepScreenAlive Then
                    KA.KeepMonitorActive()
                End If
                UserInput = Console.ReadLine
                If MySettings.LockWindowSize Then
                    ResetWindowSize()
                End If
                KA.RestoreMonitorSettings()
                Textify.SayNewLine()

            End While

            RawUserInput = UserInput

            ' Separate command text from search text
            If UserInput.Contains("/") Then
                Dim n As Integer = UserInput.IndexOf("/")
                MySearchText = UserInput.Substring(n + 1)
                UserInput = UserInput.Substring(0, n)
            Else
                MySearchText = String.Empty
            End If


            Dim keyword As String = UserInput.Trim.Split(New Char() {" "c})(0) 'get the first solid word
            MyCommand = MyCommands.FindCommand(keyword)
            Dim SplitInput As New List(Of String)
            If MyCommand IsNot Nothing Then
                SplitInput.AddRange(UserInput.Trim.Split(New Char() {" "c}))
            End If
            UserInput = String.Empty
            MySwitches.Clear()

            ' Go through each piece of the user command and pull out all the switches.
            While SplitInput.Count > 0

                Dim rawinput As String = SplitInput(0)

                If rawinput.StartsWith("-") AndAlso Not MyCommand.IgnoreSwitches Then ' -Ex:Opt:JUNK

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
                MyCommand = MyCommands.FindCommand(eCommandType.nerfherder.ToString) 'this is a dummy command just so the command object is not Nothing
            Else
                'Dim keyword As String = UserInput.Split(New Char() {" "c})(0)
                keyword = UserInput.Split(New Char() {" "c})(0)
                'MyCommand = MyCommands.FindCommand(keyword)
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

    Private Function PrePostCodeExists(FileName As String) As Boolean

        Dim InputXml As Xml.XmlDocument = New Xml.XmlDocument

        InputXml.Load(FileName)

        Dim InRoot As Xml.XmlElement = DirectCast(InputXml.SelectSingleNode(My.Application.Info.ProductName), Xml.XmlElement)

        Dim HasCode As Boolean = False

        Try
            If Not String.IsNullOrWhiteSpace(InRoot.SelectSingleNode("PreCode").InnerText) Then
                HasCode = True
            End If
        Catch ex As Exception
        End Try
        Try
            If Not String.IsNullOrWhiteSpace(InRoot.SelectSingleNode("PostCode").InnerText) Then
                HasCode = True
            End If
        Catch ex As Exception
        End Try

        Return HasCode

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
    Private Function FixedXml(ByVal ApplyDefaultUsers As Boolean, fqfn As String) As Xml.XmlDocument
        Dim obj As New SquealerObject(fqfn)
        Return FixedXml(ApplyDefaultUsers, fqfn, obj)
    End Function

    ' Load and clean up the XML using the specified target file type.
    Private Function FixedXml(ByVal ApplyDefaultUsers As Boolean, fqfn As String, ByVal obj As SquealerObject) As Xml.XmlDocument

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
        Catch ex As Exception
            InCode = String.Empty
        End Try

        If InCode.Trim = String.Empty Then
            InCode = "/***********************************************************************" _
                & vbCrLf & vbTab & "Comments." _
                & vbCrLf & "***********************************************************************/" _
                & vbCrLf & vbCrLf
            Select Case obj.Type.LongType
                Case SquealerObjectType.eType.InlineTableFunction
                    InCode &= "select 'hello world! love, ``this``' as [MyColumn]"
                Case SquealerObjectType.eType.MultiStatementTableFunction
                    InCode &= "insert @TableValue select 'hello world! love, ``this``'"
                Case SquealerObjectType.eType.ScalarFunction
                    InCode &= "set @Result = 'hello world! love, ``this``'"
                Case SquealerObjectType.eType.StoredProcedure
                    InCode &= "select 'hello world! love, ``this``'" _
                        & vbCrLf _
                        & vbCrLf _
                        & vbCrLf _
                        & "--optional (see https://docs.microsoft.com/en-us/sql/t-sql/language-elements/return-transact-sql?view=sql-server-ver15)" _
                        & vbCrLf _
                        & "--set @Squealer_ReturnValue = [ integer_expression ]"
                Case SquealerObjectType.eType.View
                    InCode &= "select 'hello world! love, ``this``' as hello"
            End Select



        End If

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
            OutUsers.AppendChild(OutputXml.CreateComment(" <User Name=""MyUser""/> "))
        Else
            For Each User As DataRow In InUsers.Select("", "Name asc")
                Dim OutUser As Xml.XmlElement = OutputXml.CreateElement("User")
                OutUser.SetAttribute("Name", User.Item("Name").ToString.Trim)
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


    ' Fix a root file and replace the original.
    Private Function ConvertXmlFile(fqfn As String, ByVal oType As SquealerObjectType.eType) As Boolean

        Dim obj As New SquealerObject(fqfn)
        obj.Type.LongType = oType
        Dim NewXml As Xml.XmlDocument = FixedXml(False, fqfn, obj) ' Fix it.

        Return IsXmlReplaced(fqfn, NewXml)

    End Function

    ' Fix a root file and replace the original.
    Private Function RepairXmlFile(ByVal IsNew As Boolean, fqfn As String) As Boolean
        Dim NewXml As Xml.XmlDocument = FixedXml(IsNew, fqfn) ' Fix it.
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
        Return CreateNewFile(WorkingFolder, FileType, filename, Nothing, Nothing, Nothing)
    End Function

    Private Function CreateNewFile(ByVal WorkingFolder As String, ByVal FileType As SquealerObjectType.eType, ByVal filename As String, Parameters As ParameterCollection, definition As String, userlist As List(Of String)) As String

        Dim Template As String = String.Empty
        Select Case FileType
            Case SquealerObjectType.eType.InlineTableFunction
                Template = My.Resources.IF_Template
            Case SquealerObjectType.eType.MultiStatementTableFunction
                Template = My.Resources.TF_Template
            Case SquealerObjectType.eType.ScalarFunction
                Template = My.Resources.FN_Template
            Case SquealerObjectType.eType.StoredProcedure
                Template = My.Resources.P_Template
            Case SquealerObjectType.eType.View
                Template = My.Resources.V_Template
        End Select
        Template = Template.Replace("{RootType}", FileType.ToString).Replace("{THIS}", Constants.MyThis)

        Dim IsNew As Boolean = True


        If Parameters Is Nothing Then
            Template = Template.Replace("{ReturnDataType}", "varchar(100)")
        Else

            IsNew = False

            If FileType = SquealerObjectType.eType.ScalarFunction Then
                Template = Template.Replace("{ReturnDataType}", Parameters.ReturnType.Type)
            End If

            Dim parms As String = String.Empty
            For Each p As ParameterClass In Parameters.Items
                parms &= String.Format("<Parameter Name=""{0}"" Type=""{1}"" Output=""{2}"" />", p.Name, p.Type, p.IsOutput.ToString)
            Next
            Template = Template.Replace("<!--Parameters-->", parms)
        End If

        If userlist IsNot Nothing Then

            IsNew = False

            Dim users As String = String.Empty
            For Each s As String In userlist
                users &= String.Format("<User Name="" {0}""/>", s)
            Next
            Template = Template.Replace("<Users/>", String.Format("<Users>{0}</Users>", users))

        End If

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
        Dim fqTarget As String = WorkingFolder & "\" & filename & Constants.SquealerFileExtension

        If definition IsNot Nothing Then
            Template = Template.Replace("<Code/>", String.Format("<Code><![CDATA[{0}]]></Code>", definition))
        End If

        ' Careful not to overwrite existing file.
        If My.Computer.FileSystem.FileExists(fqTarget) Then
            If IsNew Then
                Textify.SayError("File already exists.")
            End If
            CreateNewFile = String.Empty
        Else
            My.Computer.FileSystem.WriteAllText(fqTarget, Template, False, System.Text.Encoding.ASCII)
            RepairXmlFile(IsNew, fqTarget)
            If IsNew Then
                Textify.SayBullet(Textify.eBullet.Hash, "OK")
                Textify.WriteLine(" (" & filename & ")")
            End If
            CreateNewFile = fqTarget
        End If

        If IsNew Then
            Textify.SayNewLine()
        End If

    End Function


#End Region

#Region " Proc Generation "

    ' Expand one root file.
    Private Function ExpandIndividual(info As IO.FileInfo, StringReplacements As DataTable, bp As BatchParametersClass, cur As Integer, tot As Integer, printsteps As Boolean) As String

        Dim oType As SquealerObjectType.eType = SquealerObjectType.Eval(XmlGetObjectType(info.FullName))
        Dim RootName As String = info.Name.Replace(Constants.SquealerFileExtension, "")

        Dim InXml As Xml.XmlDocument = FixedXml(False, info.FullName)
        Dim InRoot As Xml.XmlElement = DirectCast(InXml.SelectSingleNode(My.Application.Info.ProductName), Xml.XmlElement)
        Dim Block As String = Nothing

        Dim CodeBlocks As New List(Of String)


        ' Pre-Code
        If Not bp.OutputMode = BatchParametersClass.eOutputMode.test Then
            Dim InPreCode As String = ""
            If printsteps Then
                InPreCode = String.Format("print '{2}/{3} creating {0}, {1}'", Constants.MyThis, oType.ToString, cur, tot) & vbCrLf & "go" & vbCrLf
            End If
            Try
                InPreCode &= InRoot.SelectSingleNode("PreCode").InnerText
            Catch ex As Exception
            End Try

            If Not String.IsNullOrWhiteSpace(InPreCode) Then
                InPreCode = vbCrLf & "-- additional code to execute after " & oType.ToString & " is created" & vbCrLf & InPreCode
                CodeBlocks.Add(InPreCode)
            End If

        End If


        ' Drop 
        If Not bp.OutputMode = BatchParametersClass.eOutputMode.test AndAlso Not bp.OutputMode = BatchParametersClass.eOutputMode.alter Then
            CodeBlocks.Add(My.Resources.DropAny.Replace("{RootProgramName}", RoutineName(RootName)).Replace("{Schema}", SchemaName(RootName)).ToString)
        End If

        ' Comments
        Dim OutComments As String = Nothing
        Try
            OutComments = InRoot.SelectSingleNode("Comments").InnerText.Replace("/*", String.Empty).Replace("*/", String.Empty)
        Catch ex As Exception
            OutComments = String.Empty
        End Try
        Block = My.Resources.Comment.Replace("{RootProgramName}", RoutineName(RootName)).Replace("{Comments}", OutComments).Replace("{Schema}", SchemaName(RootName))

        ' Create
        If Not bp.OutputMode = BatchParametersClass.eOutputMode.test Then
            Dim SqlCreate As String = String.Empty
            Select Case oType
                Case SquealerObjectType.eType.StoredProcedure
                    SqlCreate = My.Resources.P_Create
                Case SquealerObjectType.eType.ScalarFunction, SquealerObjectType.eType.InlineTableFunction, SquealerObjectType.eType.MultiStatementTableFunction
                    SqlCreate = My.Resources.FN_Create
                Case SquealerObjectType.eType.View
                    SqlCreate = My.Resources.V_Create
            End Select
            If bp.OutputMode = BatchParametersClass.eOutputMode.alter Then
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

            If bp.OutputMode = BatchParametersClass.eOutputMode.test Then

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
                If ParameterCount = 0 Then '<InParameters.Rows.Count Then
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
                If (Parameter.Item("Type").ToString.ToLower.Contains("max") OrElse Parameter.Item("Name").ToString.ToLower.Contains(" readonly")) Then
                    Dim whynot As String = vbCrLf & vbTab & vbTab & String.Format("--parameter @{0} cannot be logged due to its 'max' or 'readonly' definition", Parameter.Item("Name").ToString)
                    ErrorLogParameters &= whynot
                Else
                    ErrorLogParameters &= vbCrLf & My.Resources.P_ErrorParameter.Replace("{ErrorParameterNumber}", ParameterCount.ToString).Replace("{ErrorParameterName}", Parameter.Item("Name").ToString)
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

            If InColumns.Rows.Count > 0 AndAlso Not bp.OutputMode = BatchParametersClass.eOutputMode.test Then

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

            If bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                Block = Block & My.Resources.TF_TableTest
            Else
                Block = Block & My.Resources.TF_Table
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
                If bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                    BeginBlock = My.Resources.P_BeginTest
                Else
                    BeginBlock = My.Resources.P_Begin
                End If
            Case SquealerObjectType.eType.ScalarFunction
                Dim Returns As String = Nothing
                Returns = DirectCast(InRoot.SelectSingleNode("Returns"), Xml.XmlElement).GetAttribute("Type")
                If bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                    BeginBlock = My.Resources.FN_BeginTest.Replace("{ReturnDataType}", Returns)
                Else
                    BeginBlock = My.Resources.FN_Begin.Replace("{ReturnDataType}", Returns)
                End If
            Case SquealerObjectType.eType.InlineTableFunction
                If bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                    BeginBlock = String.Empty
                Else
                    BeginBlock = My.Resources.IF_Begin
                End If
            Case SquealerObjectType.eType.MultiStatementTableFunction
                If bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                    BeginBlock = My.Resources.Tf_BeginTest
                Else
                    BeginBlock = My.Resources.Tf_Begin
                End If
            Case SquealerObjectType.eType.View
                If bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                    BeginBlock = String.Empty
                Else
                    BeginBlock = My.Resources.V_Begin
                End If
        End Select


        Dim obj As New SquealerObject(info.FullName)
        Dim WithOptions As String = obj.WithOptions
        If bp.OutputMode = BatchParametersClass.eOutputMode.encrypt Then
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
                If bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                    Block &= My.Resources.P_EndTest
                Else
                    Block &= My.Resources.P_End.Replace("{Parameters}", ErrorLogParameters) '& ErrorLogParameters & My.Resources.SqlEndProcedure3
                End If
            Case SquealerObjectType.eType.ScalarFunction
                If bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                    Block = Block & My.Resources.FN_EndTest
                Else
                    Block = Block & My.Resources.FN_End
                End If
            Case SquealerObjectType.eType.MultiStatementTableFunction
                If bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                    Block = Block & My.Resources.TF_EndTest
                Else
                    Block = Block & My.Resources.TF_End
                End If
            Case SquealerObjectType.eType.View
                ' nothing to add
        End Select

        ' Save the block.
        CodeBlocks.Add(Block)

        If Not bp.OutputMode = BatchParametersClass.eOutputMode.test AndAlso Not bp.OutputMode = BatchParametersClass.eOutputMode.alter Then

            Block = String.Empty

            ' Grant Execute
            Dim InUsers As DataTable = GetUsers(InXml)

            If InUsers.Rows.Count > 0 Then
                Block = String.Format("if object_id('``this``','{0}') is not null", SquealerObjectType.ToShortType(oType))
                Block &= vbCrLf & "begin"
                For Each User As DataRow In InUsers.Select("", "Name asc")
                    Dim GrantStatement As String
                    If oType = SquealerObjectType.eType.StoredProcedure OrElse oType = SquealerObjectType.eType.ScalarFunction Then
                        GrantStatement = My.Resources.GrantExecute
                    Else
                        GrantStatement = My.Resources.GrantSelect
                    End If
                    Block = Block & vbCrLf & GrantStatement.Replace("{RootProgramName}", RoutineName(RootName)).Replace("{User}", User.Item("Name").ToString).Replace("{Schema}", SchemaName(RootName))
                Next
                Block &= vbCrLf & "end" _
                    & vbCrLf & String.Format("else print 'Permissions not granted on {0}.'", Constants.MyThis)
            End If

            If Not Block = String.Empty Then
                CodeBlocks.Add(Block)
            End If

        End If


        ' Post-Code
        If Not bp.OutputMode = BatchParametersClass.eOutputMode.test Then
            Dim InPostCode As String = String.Empty
            Try
                InPostCode = InRoot.SelectSingleNode("PostCode").InnerText
            Catch ex As Exception
            End Try

            If Not String.IsNullOrWhiteSpace(InPostCode) Then

                InPostCode = vbCrLf & "-- additional code to execute after " & oType.ToString & " is created" _
                    & vbCrLf & String.Format("if object_id('``this``','{0}') is not null", SquealerObjectType.ToShortType(oType)) _
                    & vbCrLf & "begin" _
                    & vbCrLf & InPostCode _
                    & vbCrLf & "end" _
                    & vbCrLf & "else print 'PostCode not executed.'"

                CodeBlocks.Add(InPostCode)
            End If

        End If


        ' Now add all the GOs
        ExpandIndividual = String.Empty
        For Each s As String In CodeBlocks
            ExpandIndividual &= s & vbCrLf
            If Not bp.OutputMode = BatchParametersClass.eOutputMode.test Then
                ExpandIndividual &= "go" & vbCrLf
            End If
        Next



        ' Add top/bottom markers
        ExpandIndividual =
            SpitDashes(String.Format("[{0}].[{1}]", SchemaName(RootName), RoutineName(RootName)), "<BOF>") _
            & vbCrLf & ExpandIndividual & vbCr _
            & SpitDashes(String.Format("[{0}].[{1}]", SchemaName(RootName), RoutineName(RootName)), "<EOF>") & vbCrLf & vbCrLf

        ' Do string replacements.
        ExpandIndividual = ExpandIndividual.Replace(Constants.MyThis, String.Format("[{0}].[{1}]", SchemaName(RootName), RoutineName(RootName)))
        For Each Replacement As DataRow In StringReplacements.Select() '.Select("")
            ExpandIndividual = ExpandIndividual.Replace(Replacement.Item("Original").ToString, Replacement.Item("Replacement").ToString)
        Next


    End Function

#End Region

#Region " Ez "

    Private Function EzSqlPath() As String
        Return My.Configger.AppDataFolder & "\ezscript.sql"
    End Function

    Private Function EzBinFilename() As String
        Return "ezscript.bin"
    End Function

    Private Function EzBinPath() As String
        Return My.Configger.AppDataFolder & "\" & EzBinFilename()
    End Function
    Private Function EzNewBinPath() As String
        Return EzBinPath() & ".new"
    End Function

    Private Function EzText(TraceIt As Boolean) As String
        Dim s As String
        Try
            Try
                ' attempt to read plain text file from disk because local customization takes precedence
                s = My.Computer.FileSystem.ReadAllText(EzSqlPath)
                If TraceIt Then
                    Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("reading {0}", EzSqlPath))
                End If
            Catch ex As Exception
                ' attempt to read encrypted file from disk
                s = Misc.DecryptedString(My.Computer.FileSystem.ReadAllBytes(EzBinPath))
                If TraceIt Then
                    Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("reading {0}", EzBinPath))
                End If
            End Try
        Catch ex As Exception
            ' default text
            s = My.Resources.EzObjects
            If TraceIt Then
                Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("reading resource file"))
            End If
        End Try
        Return vbCrLf & s.Trim & vbCrLf
    End Function

    Private Sub EzExtractSqlToFile()
        My.Computer.FileSystem.WriteAllText(EzSqlPath, EzText(True), False)
        Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("writing {0}", EzSqlPath), 0, New Textify.ColorScheme(ConsoleColor.Cyan))
        Console.WriteLine()
    End Sub

    Private Sub EzConvertSqlToNewBin()
        My.Computer.FileSystem.WriteAllBytes(EzNewBinPath, Misc.EncryptedBytes(EzText(True)), False)
        Textify.SayBulletLine(Textify.eBullet.Hash, String.Format("writing {0}", EzNewBinPath), 0, New Textify.ColorScheme(ConsoleColor.Cyan))
        Console.WriteLine()
    End Sub

#End Region

#Region " Misc "

    Private Sub ResetWindowSize()
        Try
            ' This fails if the console was in full-screen mode at previous exit
            Console.SetWindowSize(My.Configger.LoadSetting("WindowWidth", 130), My.Configger.LoadSetting("WindowHeight", 30))
        Catch ex As Exception
        End Try
        Console.BufferWidth = Console.WindowWidth
    End Sub

    Private Sub SaveWindowSize()
        My.Configger.SaveSetting("WindowWidth", Console.WindowWidth)
        My.Configger.SaveSetting("WindowHeight", Console.WindowHeight)
    End Sub

    Private Sub ShowLeaderboard(topN As Integer)
        Textify.WriteLine("Retrieving scores...")
        Console.WriteLine()
        Dim lb As New AsciiEngine.Leaderboard
        lb.SqlConnectionString = MySettings.LeaderboardConnectionString
        lb.SqlLoadScores(topN)
        Dim i As Integer = 0
        For Each s As AsciiEngine.Leaderboard.Score In lb.Items
            i += 1
            Textify.SayCentered(String.Format("|  {0}  {1}  |", s.Signature, s.Points.ToString("00000#"), i.ToString("0#")))
        Next
        Console.WriteLine()
    End Sub


    Private Function SpitDashes(s As String, marker As String) As String
        Return New String("-"c, 5) & " " & s & " " & New String("-"c, 100 - s.Length) & " " & marker
    End Function

    Private Sub BracketCheck(s As String)
        If s.Contains("["c) OrElse s.Contains("]"c) Then
            Throw New System.Exception("Illegal square bracket character in filename: " & s)
        End If
    End Sub

    Private Sub DisplayAboutInfo(ShowWhatsNew As Boolean, ShowFullHistory As Boolean)

        Console.WriteLine(String.Format("{0} v.{1}", My.Application.Info.Title, My.Application.Info.Version))
        Console.WriteLine(My.Application.Info.Copyright)
        Console.WriteLine()

        Dim v As New VersionCheck
        v.DisplayVersionCheckResults(MySettings.MediaSourceUrl, MySettings.IsDefaultMediaSource)
        Console.WriteLine()

        If ShowWhatsNew Then
            Textify.WriteLine("New in this release:", New Textify.ColorScheme(ConsoleColor.Cyan))
            Console.WriteLine()
            Console.WriteLine(VersionCheck.WhatsNew)
            Console.WriteLine()
        End If

        If ShowFullHistory Then
            Textify.WriteLine("Change log:", New Textify.ColorScheme(ConsoleColor.Cyan))
            Console.WriteLine()
            Console.WriteLine(VersionCheck.ChangeLog)
            Console.WriteLine()
        End If

    End Sub

    Private Function OpenExplorer(ByVal wildcard As String, ByVal WorkingFolder As String) As List(Of String)
        Dim f As New OpenFileDialog
        f.InitialDirectory = WorkingFolder
        f.FileName = wildcard
        f.Multiselect = True
        Dim s As New List(Of String)
        If Not f.ShowDialog() = DialogResult.Cancel Then
            s.AddRange(f.FileNames)
        End If
        Return s
    End Function

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
        Reader.Load(WorkingFolder & "\" & Constants.ConfigFilename)
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
            Reader.Load(WorkingFolder & "\" & Constants.ConfigFilename)
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



#End Region

#Region " Console Output "

    ' Display a notice for a file.
    Private Sub SayFileAction(ByVal notice As String, ByVal path As String, ByVal file As String)
        Textify.SayBullet(Textify.eBullet.Hash, notice & ":")
        Textify.SayBullet(Textify.eBullet.Arrow, path & IIf(file = "", "", "\" & file).ToString)
    End Sub

#End Region

#Region " Connection String "

    Private Sub SetConnectionString(workingfolder As String, cs As String)

        Dim f As String = String.Format("{0}\{1}", workingfolder, Constants.ConnectionStringFilename)

        If My.Computer.FileSystem.FileExists(f) Then
            My.Computer.FileSystem.DeleteFile(f)
        End If
        My.Computer.FileSystem.WriteAllBytes(f, Misc.EncryptedBytes(cs), False)
        System.IO.File.SetAttributes(f, IO.FileAttributes.Hidden)

        Textify.SayBulletLine(Textify.eBullet.Hash, "OK")
        Textify.SayNewLine()

    End Sub
    Private Sub ForgetConnectionString(workingfolder As String)

        Dim f As String = String.Format("{0}\{1}", workingfolder, Constants.ConnectionStringFilename)
        My.Computer.FileSystem.DeleteFile(f)
        Textify.SayBulletLine(Textify.eBullet.Hash, "OK")
        Textify.SayNewLine()

    End Sub

    Private Function GetConnectionString(workingfolder As String) As String

        Dim f As String = String.Format("{0}\{1}", workingfolder, Constants.ConnectionStringFilename)

        If Not My.Computer.FileSystem.FileExists(f) Then
            Throw New Exception("Connection string not defined.")
        End If

        GetConnectionString = Misc.DecryptedString(My.Computer.FileSystem.ReadAllBytes(f))

    End Function

    Private Sub TestConnectionString(workingfolder As String)

        Dim cs As String = GetConnectionString(workingfolder)

        Textify.SayBullet(Textify.eBullet.Arrow, cs)

        Using DbTest As SqlClient.SqlConnection = New SqlClient.SqlConnection(cs)

            DbTest.Open()

            Dim DbReader As SqlClient.SqlDataReader = New SqlClient.SqlCommand("select @@SERVERNAME,DB_NAME(),@@VERSION" _
                & ",(select count(1) from sys.tables)" _
                & ",(select count(1) from sys.objects o where o.type = 'p')" _
                & ",(select count(1) from sys.objects o where o.type = 'fn')" _
                & ",(select count(1) from sys.objects o where o.type = 'if')" _
                & ",(select count(1) from sys.objects o where o.type = 'tf')" _
                & ",(select count(1) from sys.objects o where o.type = 'v')" _
                & ";", DbTest).ExecuteReader

            While DbReader.Read

                Textify.SayBulletLine(Textify.eBullet.Arrow, DbReader.GetString(2)) ' @@version
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("[{0}].[{1}]", DbReader.GetString(0), DbReader.GetString(1)))
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("{0} table(s)", DbReader.GetInt32(3).ToString))
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("{0} stored procedure(s)", DbReader.GetInt32(4).ToString))
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("{0} scalar function(s)", DbReader.GetInt32(5).ToString))
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("{0} inline table-valued function(s)", DbReader.GetInt32(6).ToString))
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("{0} multi-statement table-valued function(s)", DbReader.GetInt32(7).ToString))
                Textify.SayBulletLine(Textify.eBullet.Arrow, String.Format("{0} views(s)", DbReader.GetInt32(8).ToString))
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

        Textify.Write("Reading tables..")

        Dim ProcCount As Integer = 0

        Using DbTables As SqlClient.SqlConnection = New SqlClient.SqlConnection(cs)

            DbTables.Open()

            Dim TableReader As SqlClient.SqlDataReader = New SqlClient.SqlCommand(My.Resources.AutoGetTables, DbTables).ExecuteReader

            Dim spinny As New SpinCursor()


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

                spinny.Animate()

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

                    Dim filename As String = String.Format("{0}\{1}.{5}{2}{3}{4}", WorkingFolder, SchemaName, TableName, AutoProcType, Constants.SquealerFileExtension, Constants.AutocreateFilename)

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

                        RepairXmlFile(False, filename)

                        ProcCount += 1

                    End If

                End If


                If Console.KeyAvailable() Then
                    Throw New System.Exception("Keyboard interrupt.")
                End If

            End While

            spinny.Finish()

        End Using

        Textify.SayNewLine()
        Textify.SayBullet(Textify.eBullet.Hash, String.Format("{0} files automatically created.", ProcCount.ToString))
        Textify.SayNewLine(2)

    End Sub

    Private Sub ReverseEngineer(cs As String, WorkingFolder As String, DoCleanup As Boolean)

        Console.Write("Reading procs, views, functions..")

        Dim ProcCount As Integer = 0
        Dim SkippedCount As Integer = 0
        Dim tempfile As New TempFileHandler(".txt")

        Using DbObjects As SqlClient.SqlConnection = New SqlClient.SqlConnection(cs)

            DbObjects.Open()

            Dim ObjectReader As SqlClient.SqlDataReader = New SqlClient.SqlCommand(My.Resources.ObjectList, DbObjects).ExecuteReader

            Dim spinny As New SpinCursor()

            While ObjectReader.Read ' loop thru procs, views, functions

                Dim ParameterList As New ParameterCollection
                Dim UserList As New List(Of String)

                Dim ObjectName As String = ObjectReader.GetString(0)
                Dim ObjectType As String = ObjectReader.GetString(1).ToLower
                Dim ObjectDefinition As String = ObjectReader.GetString(2)
                Dim ObjectId As Integer = ObjectReader.GetInt32(3)

                Dim filetype As SquealerObjectType.eType = SquealerObjectType.Eval(ObjectType)

                Using DbParameters As SqlClient.SqlConnection = New SqlClient.SqlConnection(cs)

                    DbParameters.Open()

                    Dim ParameterReader As SqlClient.SqlDataReader = New SqlClient.SqlCommand(My.Resources.ObjectParameters.Replace("@ObjectId", ObjectId.ToString), DbParameters).ExecuteReader

                    While ParameterReader.Read ' loop thru parameters

                        Dim ParameterName As String = ParameterReader.GetString(0)
                        Dim ParameterType As String = ParameterReader.GetString(1)
                        Dim IsOutput As Boolean = ParameterReader.GetBoolean(2)
                        Dim MaxLength As Int16 = ParameterReader.GetInt16(3)

                        ParameterList.Add(New ParameterClass(ParameterName, ParameterType, MaxLength, IsOutput))

                    End While

                End Using

                Using DbUsers As SqlClient.SqlConnection = New SqlClient.SqlConnection(cs)

                    DbUsers.Open()

                    Dim UserReader As SqlClient.SqlDataReader = New SqlClient.SqlCommand(My.Resources.ObjectPermissions.Replace("@ObjectId", ObjectId.ToString), DbUsers).ExecuteReader

                    While UserReader.Read ' loop thru user permissions granted

                        Dim UserName As String = UserReader.GetString(0)

                        UserList.Add(UserName)

                    End While

                End Using

                If DoCleanup Then

                    ' delete head
                    Try
                        Dim s As String = "create " & SquealerObjectType.EvalSimpleType(filetype)
                        ObjectDefinition = ObjectDefinition.Remove(0, ObjectDefinition.ToLower.IndexOf(s) + s.Length + 1)
                    Catch ex As Exception
                    End Try

                    ' delete tail
                    Try
                        Dim s As String = String.Empty
                        Select Case filetype
                            Case SquealerObjectType.eType.StoredProcedure
                                s = "YOUR CODE ENDS HERE."
                            Case SquealerObjectType.eType.ScalarFunction
                                s = "Return the function result."
                        End Select
                        If Not String.IsNullOrEmpty(s) Then
                            ObjectDefinition = ObjectDefinition.Substring(0, ObjectDefinition.IndexOf(s))
                        End If
                    Catch ex As Exception
                    End Try

                    ' delete everything between parameters and beginning of code
                    Try

                        Dim s As String = String.Empty
                        Dim s2 As String = String.Empty
                        Select Case filetype
                            Case SquealerObjectType.eType.StoredProcedure
                                s = "Begin the transaction. Start the TRY..CATCH wrapper."
                                s2 = "YOUR CODE BEGINS HERE."
                            Case SquealerObjectType.eType.ScalarFunction
                                s = "returns"
                                s2 = "declare @Result " & ParameterList.ReturnType.Type
                        End Select
                        If Not String.IsNullOrEmpty(s) Then
                            Dim startpos As Integer = ObjectDefinition.IndexOf(s)
                            Dim charcount As Integer = ObjectDefinition.IndexOf(s2) + s2.Length - startpos
                            ObjectDefinition = ObjectDefinition.Remove(startpos, charcount)
                        End If
                    Catch ex As Exception
                    End Try

                End If

                ObjectDefinition = "-- reverse engineered on " & Now.ToString & vbCrLf & vbCrLf & ObjectDefinition

                Dim f As String = CreateNewFile(WorkingFolder, filetype, ObjectName, ParameterList, ObjectDefinition, UserList)
                If f = String.Empty Then
                    SkippedCount += 1
                    tempfile.Writeline(String.Format("{0} -- duplicate ({1})", ObjectName, filetype.ToString))
                Else
                    ProcCount += 1
                    tempfile.Writeline(ObjectName & " -- OK")
                End If
                spinny.Animate()

                If Console.KeyAvailable() Then
                    Throw New System.Exception("Keyboard interrupt.")
                End If

            End While

            spinny.Finish()

        End Using

        Textify.SayNewLine()
        Textify.SayBullet(Textify.eBullet.Hash, String.Format("{0} files reverse engineered; {1} skipped (duplicate filename).", ProcCount.ToString, SkippedCount.ToString))
        Textify.SayNewLine()
        Textify.SayBullet(Textify.eBullet.Hash, "Results not guaranteed!", 0, New Textify.ColorScheme(ConsoleColor.Yellow))
        Textify.SayNewLine(2)

        tempfile.Show()

    End Sub


#End Region

#Region " Text Editor "

    ' Edit one or more files.
    Private Sub OpenInTextEditor(filename As String, path As String)
        EditFile(path & "\" & filename)
    End Sub
    Private Sub EditFile(filename As String)
        If (filename.EndsWith(".sql") AndAlso Not MySettings.OpenWithDefault.SqlFiles) _
            OrElse (filename.EndsWith(Constants.ConfigFilename) AndAlso Not MySettings.OpenWithDefault.ConfigFiles) _
            OrElse (filename.EndsWith(Constants.SquealerFileExtension) AndAlso Not MySettings.OpenWithDefault.SquealerFiles) Then
            EasyShell.StartProcess(MySettings.TextEditorPath, filename)
        Else
            EasyShell.StartProcess(filename)
        End If
    End Sub

#End Region

End Module
