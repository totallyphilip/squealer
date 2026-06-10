<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SettingsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.optBeep = New System.Windows.Forms.CheckBox()
        Me.chkEdgesWild = New System.Windows.Forms.CheckBox()
        Me.chkSpacesWild = New System.Windows.Forms.CheckBox()
        Me.optEditNewFiles = New System.Windows.Forms.CheckBox()
        Me.optShowGitBranch = New System.Windows.Forms.CheckBox()
        Me.rbCompact = New System.Windows.Forms.RadioButton()
        Me.rbFull = New System.Windows.Forms.RadioButton()
        Me.rbSymbolic = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.updnFolderSaves = New System.Windows.Forms.NumericUpDown()
        Me.txtTryIt = New System.Windows.Forms.TextBox()
        Me.txtWildcardExample = New System.Windows.Forms.TextBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.rbTempFile = New System.Windows.Forms.RadioButton()
        Me.rbClipboard = New System.Windows.Forms.RadioButton()
        Me.optDetectOldSquealerObjects = New System.Windows.Forms.CheckBox()
        Me.dlgTextEditor = New System.Windows.Forms.OpenFileDialog()
        Me.Tabs = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.chkAutoCompressGit = New System.Windows.Forms.CheckBox()
        Me.chkTrackFailedItems = New System.Windows.Forms.CheckBox()
        Me.chkLockWindowSize = New System.Windows.Forms.CheckBox()
        Me.chkKeepScreenOn = New System.Windows.Forms.CheckBox()
        Me.chkShowProjectNameInCommandPrompt = New System.Windows.Forms.CheckBox()
        Me.chkShowProjectDirectoryInTitleBar = New System.Windows.Forms.CheckBox()
        Me.chkShowProjectNameInTitleBar = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkAlwaysShowSymbols = New System.Windows.Forms.CheckBox()
        Me.txtDirExample = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tabOutput = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.rbAlter = New System.Windows.Forms.RadioButton()
        Me.rbDrop = New System.Windows.Forms.RadioButton()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtProgressExample = New System.Windows.Forms.TextBox()
        Me.ddIncrement = New System.Windows.Forms.ComboBox()
        Me.rbPercentage = New System.Windows.Forms.RadioButton()
        Me.rbDetailed = New System.Windows.Forms.RadioButton()
        Me.tabWildcards = New System.Windows.Forms.TabPage()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.gbTextEditor = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtEditorProgram = New System.Windows.Forms.TextBox()
        Me.btnEditorDialog = New System.Windows.Forms.Button()
        Me.chkConfigDefaultEditor = New System.Windows.Forms.CheckBox()
        Me.chkSquealerDefaultEditor = New System.Windows.Forms.CheckBox()
        Me.chkOutputDefaultEditor = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        CType(Me.updnFolderSaves, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tabs.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tabOutput.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.tabWildcards.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.gbTextEditor.SuspendLayout()
        Me.SuspendLayout()
        '
        'optBeep
        '
        Me.optBeep.AutoSize = True
        Me.optBeep.Location = New System.Drawing.Point(6, 6)
        Me.optBeep.Name = "optBeep"
        Me.optBeep.Size = New System.Drawing.Size(90, 17)
        Me.optBeep.TabIndex = 1
        Me.optBeep.Text = "Beep on error"
        Me.optBeep.UseVisualStyleBackColor = True
        '
        'chkEdgesWild
        '
        Me.chkEdgesWild.AutoSize = True
        Me.chkEdgesWild.Location = New System.Drawing.Point(9, 39)
        Me.chkEdgesWild.Name = "chkEdgesWild"
        Me.chkEdgesWild.Size = New System.Drawing.Size(135, 17)
        Me.chkEdgesWild.TabIndex = 3
        Me.chkEdgesWild.Text = "Surround with asterisks"
        Me.chkEdgesWild.UseVisualStyleBackColor = True
        '
        'chkSpacesWild
        '
        Me.chkSpacesWild.AutoSize = True
        Me.chkSpacesWild.Location = New System.Drawing.Point(9, 62)
        Me.chkSpacesWild.Name = "chkSpacesWild"
        Me.chkSpacesWild.Size = New System.Drawing.Size(146, 17)
        Me.chkSpacesWild.TabIndex = 5
        Me.chkSpacesWild.Text = "Treat spaces as asterisks"
        Me.chkSpacesWild.UseVisualStyleBackColor = True
        '
        'optEditNewFiles
        '
        Me.optEditNewFiles.AutoSize = True
        Me.optEditNewFiles.Location = New System.Drawing.Point(6, 27)
        Me.optEditNewFiles.Name = "optEditNewFiles"
        Me.optEditNewFiles.Size = New System.Drawing.Size(214, 17)
        Me.optEditNewFiles.TabIndex = 6
        Me.optEditNewFiles.Text = "Automatically open NEW files for editing"
        Me.optEditNewFiles.UseVisualStyleBackColor = True
        '
        'optShowGitBranch
        '
        Me.optShowGitBranch.AutoSize = True
        Me.optShowGitBranch.Location = New System.Drawing.Point(6, 48)
        Me.optShowGitBranch.Name = "optShowGitBranch"
        Me.optShowGitBranch.Size = New System.Drawing.Size(243, 17)
        Me.optShowGitBranch.TabIndex = 9
        Me.optShowGitBranch.Text = "Display the Git branch in the command prompt"
        Me.optShowGitBranch.UseVisualStyleBackColor = True
        '
        'rbCompact
        '
        Me.rbCompact.AutoSize = True
        Me.rbCompact.Location = New System.Drawing.Point(50, 19)
        Me.rbCompact.Name = "rbCompact"
        Me.rbCompact.Size = New System.Drawing.Size(66, 17)
        Me.rbCompact.TabIndex = 10
        Me.rbCompact.TabStop = True
        Me.rbCompact.Text = "compact"
        Me.rbCompact.UseVisualStyleBackColor = True
        '
        'rbFull
        '
        Me.rbFull.AutoSize = True
        Me.rbFull.Location = New System.Drawing.Point(6, 19)
        Me.rbFull.Name = "rbFull"
        Me.rbFull.Size = New System.Drawing.Size(38, 17)
        Me.rbFull.TabIndex = 12
        Me.rbFull.TabStop = True
        Me.rbFull.Text = "full"
        Me.rbFull.UseVisualStyleBackColor = True
        '
        'rbSymbolic
        '
        Me.rbSymbolic.AutoSize = True
        Me.rbSymbolic.Location = New System.Drawing.Point(122, 19)
        Me.rbSymbolic.Name = "rbSymbolic"
        Me.rbSymbolic.Size = New System.Drawing.Size(65, 17)
        Me.rbSymbolic.TabIndex = 13
        Me.rbSymbolic.TabStop = True
        Me.rbSymbolic.Text = "symbolic"
        Me.rbSymbolic.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 235)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Remember up to"
        '
        'updnFolderSaves
        '
        Me.updnFolderSaves.Location = New System.Drawing.Point(97, 233)
        Me.updnFolderSaves.Name = "updnFolderSaves"
        Me.updnFolderSaves.Size = New System.Drawing.Size(55, 20)
        Me.updnFolderSaves.TabIndex = 0
        '
        'txtTryIt
        '
        Me.txtTryIt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTryIt.Location = New System.Drawing.Point(6, 114)
        Me.txtTryIt.Name = "txtTryIt"
        Me.txtTryIt.Size = New System.Drawing.Size(466, 20)
        Me.txtTryIt.TabIndex = 16
        Me.txtTryIt.Text = "type any file name"
        '
        'txtWildcardExample
        '
        Me.txtWildcardExample.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtWildcardExample.Location = New System.Drawing.Point(6, 164)
        Me.txtWildcardExample.Name = "txtWildcardExample"
        Me.txtWildcardExample.ReadOnly = True
        Me.txtWildcardExample.Size = New System.Drawing.Size(466, 20)
        Me.txtWildcardExample.TabIndex = 14
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnOK.Location = New System.Drawing.Point(352, 588)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 18
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'rbTempFile
        '
        Me.rbTempFile.AutoSize = True
        Me.rbTempFile.Location = New System.Drawing.Point(86, 19)
        Me.rbTempFile.Name = "rbTempFile"
        Me.rbTempFile.Size = New System.Drawing.Size(64, 17)
        Me.rbTempFile.TabIndex = 11
        Me.rbTempFile.TabStop = True
        Me.rbTempFile.Text = "temp file"
        Me.rbTempFile.UseVisualStyleBackColor = True
        '
        'rbClipboard
        '
        Me.rbClipboard.AutoSize = True
        Me.rbClipboard.Location = New System.Drawing.Point(11, 19)
        Me.rbClipboard.Name = "rbClipboard"
        Me.rbClipboard.Size = New System.Drawing.Size(68, 17)
        Me.rbClipboard.TabIndex = 10
        Me.rbClipboard.TabStop = True
        Me.rbClipboard.Text = "clipboard"
        Me.rbClipboard.UseVisualStyleBackColor = True
        '
        'optDetectOldSquealerObjects
        '
        Me.optDetectOldSquealerObjects.AutoSize = True
        Me.optDetectOldSquealerObjects.Location = New System.Drawing.Point(8, 16)
        Me.optDetectOldSquealerObjects.Name = "optDetectOldSquealerObjects"
        Me.optDetectOldSquealerObjects.Size = New System.Drawing.Size(197, 17)
        Me.optDetectOldSquealerObjects.TabIndex = 8
        Me.optDetectOldSquealerObjects.Text = "Detect deprecated Squealer objects"
        Me.optDetectOldSquealerObjects.UseVisualStyleBackColor = True
        '
        'dlgTextEditor
        '
        Me.dlgTextEditor.DefaultExt = "exe"
        Me.dlgTextEditor.FileName = "OpenFileDialog1"
        Me.dlgTextEditor.Filter = "*.exe|*.exe"
        '
        'Tabs
        '
        Me.Tabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tabs.Controls.Add(Me.tabGeneral)
        Me.Tabs.Controls.Add(Me.tabOutput)
        Me.Tabs.Controls.Add(Me.tabWildcards)
        Me.Tabs.Controls.Add(Me.TabPage4)
        Me.Tabs.Location = New System.Drawing.Point(12, 12)
        Me.Tabs.Name = "Tabs"
        Me.Tabs.SelectedIndex = 0
        Me.Tabs.Size = New System.Drawing.Size(419, 568)
        Me.Tabs.TabIndex = 26
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.chkAutoCompressGit)
        Me.tabGeneral.Controls.Add(Me.chkTrackFailedItems)
        Me.tabGeneral.Controls.Add(Me.chkLockWindowSize)
        Me.tabGeneral.Controls.Add(Me.chkKeepScreenOn)
        Me.tabGeneral.Controls.Add(Me.chkShowProjectNameInCommandPrompt)
        Me.tabGeneral.Controls.Add(Me.chkShowProjectDirectoryInTitleBar)
        Me.tabGeneral.Controls.Add(Me.chkShowProjectNameInTitleBar)
        Me.tabGeneral.Controls.Add(Me.GroupBox1)
        Me.tabGeneral.Controls.Add(Me.Label9)
        Me.tabGeneral.Controls.Add(Me.Label3)
        Me.tabGeneral.Controls.Add(Me.updnFolderSaves)
        Me.tabGeneral.Controls.Add(Me.optBeep)
        Me.tabGeneral.Controls.Add(Me.optEditNewFiles)
        Me.tabGeneral.Controls.Add(Me.optShowGitBranch)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(411, 542)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'chkAutoCompressGit
        '
        Me.chkAutoCompressGit.AutoSize = True
        Me.chkAutoCompressGit.Location = New System.Drawing.Point(6, 197)
        Me.chkAutoCompressGit.Name = "chkAutoCompressGit"
        Me.chkAutoCompressGit.Size = New System.Drawing.Size(152, 17)
        Me.chkAutoCompressGit.TabIndex = 24
        Me.chkAutoCompressGit.Text = "Automatically compress Git"
        Me.chkAutoCompressGit.UseVisualStyleBackColor = True
        '
        'chkTrackFailedItems
        '
        Me.chkTrackFailedItems.AutoSize = True
        Me.chkTrackFailedItems.Location = New System.Drawing.Point(6, 174)
        Me.chkTrackFailedItems.Name = "chkTrackFailedItems"
        Me.chkTrackFailedItems.Size = New System.Drawing.Size(109, 17)
        Me.chkTrackFailedItems.TabIndex = 23
        Me.chkTrackFailedItems.Text = "Track failed items"
        Me.chkTrackFailedItems.UseVisualStyleBackColor = True
        '
        'chkLockWindowSize
        '
        Me.chkLockWindowSize.AutoSize = True
        Me.chkLockWindowSize.Location = New System.Drawing.Point(6, 152)
        Me.chkLockWindowSize.Margin = New System.Windows.Forms.Padding(2)
        Me.chkLockWindowSize.Name = "chkLockWindowSize"
        Me.chkLockWindowSize.Size = New System.Drawing.Size(110, 17)
        Me.chkLockWindowSize.TabIndex = 22
        Me.chkLockWindowSize.Text = "Lock window size"
        Me.chkLockWindowSize.UseVisualStyleBackColor = True
        '
        'chkKeepScreenOn
        '
        Me.chkKeepScreenOn.AutoSize = True
        Me.chkKeepScreenOn.Location = New System.Drawing.Point(6, 110)
        Me.chkKeepScreenOn.Name = "chkKeepScreenOn"
        Me.chkKeepScreenOn.Size = New System.Drawing.Size(101, 17)
        Me.chkKeepScreenOn.TabIndex = 21
        Me.chkKeepScreenOn.Text = "Keep screen on"
        Me.chkKeepScreenOn.UseVisualStyleBackColor = True
        '
        'chkShowProjectNameInCommandPrompt
        '
        Me.chkShowProjectNameInCommandPrompt.AutoSize = True
        Me.chkShowProjectNameInCommandPrompt.Location = New System.Drawing.Point(6, 69)
        Me.chkShowProjectNameInCommandPrompt.Name = "chkShowProjectNameInCommandPrompt"
        Me.chkShowProjectNameInCommandPrompt.Size = New System.Drawing.Size(255, 17)
        Me.chkShowProjectNameInCommandPrompt.TabIndex = 20
        Me.chkShowProjectNameInCommandPrompt.Text = "Display the project name in the command prompt"
        Me.chkShowProjectNameInCommandPrompt.UseVisualStyleBackColor = True
        '
        'chkShowProjectDirectoryInTitleBar
        '
        Me.chkShowProjectDirectoryInTitleBar.AutoSize = True
        Me.chkShowProjectDirectoryInTitleBar.Location = New System.Drawing.Point(6, 131)
        Me.chkShowProjectDirectoryInTitleBar.Name = "chkShowProjectDirectoryInTitleBar"
        Me.chkShowProjectDirectoryInTitleBar.Size = New System.Drawing.Size(208, 17)
        Me.chkShowProjectDirectoryInTitleBar.TabIndex = 19
        Me.chkShowProjectDirectoryInTitleBar.Text = "Display the project folder in the title bar"
        Me.chkShowProjectDirectoryInTitleBar.UseVisualStyleBackColor = True
        '
        'chkShowProjectNameInTitleBar
        '
        Me.chkShowProjectNameInTitleBar.AutoSize = True
        Me.chkShowProjectNameInTitleBar.Location = New System.Drawing.Point(6, 89)
        Me.chkShowProjectNameInTitleBar.Name = "chkShowProjectNameInTitleBar"
        Me.chkShowProjectNameInTitleBar.Size = New System.Drawing.Size(208, 17)
        Me.chkShowProjectNameInTitleBar.TabIndex = 18
        Me.chkShowProjectNameInTitleBar.Text = "Display the project name in the title bar"
        Me.chkShowProjectNameInTitleBar.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.chkAlwaysShowSymbols)
        Me.GroupBox1.Controls.Add(Me.rbCompact)
        Me.GroupBox1.Controls.Add(Me.rbSymbolic)
        Me.GroupBox1.Controls.Add(Me.txtDirExample)
        Me.GroupBox1.Controls.Add(Me.rbFull)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 278)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(396, 258)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Directory format"
        '
        'chkAlwaysShowSymbols
        '
        Me.chkAlwaysShowSymbols.AutoSize = True
        Me.chkAlwaysShowSymbols.Location = New System.Drawing.Point(194, 19)
        Me.chkAlwaysShowSymbols.Name = "chkAlwaysShowSymbols"
        Me.chkAlwaysShowSymbols.Size = New System.Drawing.Size(126, 17)
        Me.chkAlwaysShowSymbols.TabIndex = 16
        Me.chkAlwaysShowSymbols.Text = "always show symbols"
        Me.chkAlwaysShowSymbols.UseVisualStyleBackColor = True
        '
        'txtDirExample
        '
        Me.txtDirExample.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDirExample.Location = New System.Drawing.Point(6, 42)
        Me.txtDirExample.Multiline = True
        Me.txtDirExample.Name = "txtDirExample"
        Me.txtDirExample.ReadOnly = True
        Me.txtDirExample.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDirExample.Size = New System.Drawing.Size(384, 210)
        Me.txtDirExample.TabIndex = 15
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(158, 235)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "project folders"
        '
        'tabOutput
        '
        Me.tabOutput.Controls.Add(Me.GroupBox4)
        Me.tabOutput.Controls.Add(Me.GroupBox3)
        Me.tabOutput.Controls.Add(Me.GroupBox2)
        Me.tabOutput.Controls.Add(Me.optDetectOldSquealerObjects)
        Me.tabOutput.Location = New System.Drawing.Point(4, 22)
        Me.tabOutput.Name = "tabOutput"
        Me.tabOutput.Padding = New System.Windows.Forms.Padding(3)
        Me.tabOutput.Size = New System.Drawing.Size(411, 542)
        Me.tabOutput.TabIndex = 1
        Me.tabOutput.Text = "Output"
        Me.tabOutput.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.rbAlter)
        Me.GroupBox4.Controls.Add(Me.rbDrop)
        Me.GroupBox4.Location = New System.Drawing.Point(6, 92)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(399, 47)
        Me.GroupBox4.TabIndex = 25
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Default build mode"
        '
        'rbAlter
        '
        Me.rbAlter.AutoSize = True
        Me.rbAlter.Location = New System.Drawing.Point(11, 19)
        Me.rbAlter.Name = "rbAlter"
        Me.rbAlter.Size = New System.Drawing.Size(45, 17)
        Me.rbAlter.TabIndex = 10
        Me.rbAlter.TabStop = True
        Me.rbAlter.Text = "alter"
        Me.rbAlter.UseVisualStyleBackColor = True
        '
        'rbDrop
        '
        Me.rbDrop.AutoSize = True
        Me.rbDrop.Location = New System.Drawing.Point(62, 19)
        Me.rbDrop.Name = "rbDrop"
        Me.rbDrop.Size = New System.Drawing.Size(81, 17)
        Me.rbDrop.TabIndex = 11
        Me.rbDrop.TabStop = True
        Me.rbDrop.Text = "drop/create"
        Me.rbDrop.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.rbClipboard)
        Me.GroupBox3.Controls.Add(Me.rbTempFile)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 39)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(399, 47)
        Me.GroupBox3.TabIndex = 24
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Send output to"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.txtProgressExample)
        Me.GroupBox2.Controls.Add(Me.ddIncrement)
        Me.GroupBox2.Controls.Add(Me.rbPercentage)
        Me.GroupBox2.Controls.Add(Me.rbDetailed)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 145)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(399, 394)
        Me.GroupBox2.TabIndex = 23
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Progress display"
        '
        'txtProgressExample
        '
        Me.txtProgressExample.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtProgressExample.Location = New System.Drawing.Point(6, 46)
        Me.txtProgressExample.Multiline = True
        Me.txtProgressExample.Name = "txtProgressExample"
        Me.txtProgressExample.ReadOnly = True
        Me.txtProgressExample.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtProgressExample.Size = New System.Drawing.Size(387, 342)
        Me.txtProgressExample.TabIndex = 20
        '
        'ddIncrement
        '
        Me.ddIncrement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddIncrement.FormattingEnabled = True
        Me.ddIncrement.Items.AddRange(New Object() {"5", "10", "20", "25"})
        Me.ddIncrement.Location = New System.Drawing.Point(159, 15)
        Me.ddIncrement.Name = "ddIncrement"
        Me.ddIncrement.Size = New System.Drawing.Size(42, 21)
        Me.ddIncrement.TabIndex = 22
        '
        'rbPercentage
        '
        Me.rbPercentage.AutoSize = True
        Me.rbPercentage.Location = New System.Drawing.Point(74, 19)
        Me.rbPercentage.Name = "rbPercentage"
        Me.rbPercentage.Size = New System.Drawing.Size(79, 17)
        Me.rbPercentage.TabIndex = 16
        Me.rbPercentage.TabStop = True
        Me.rbPercentage.Text = "percentage"
        Me.rbPercentage.UseVisualStyleBackColor = True
        '
        'rbDetailed
        '
        Me.rbDetailed.AutoSize = True
        Me.rbDetailed.Location = New System.Drawing.Point(6, 19)
        Me.rbDetailed.Name = "rbDetailed"
        Me.rbDetailed.Size = New System.Drawing.Size(62, 17)
        Me.rbDetailed.TabIndex = 17
        Me.rbDetailed.TabStop = True
        Me.rbDetailed.Text = "detailed"
        Me.rbDetailed.UseVisualStyleBackColor = True
        '
        'tabWildcards
        '
        Me.tabWildcards.Controls.Add(Me.Label4)
        Me.tabWildcards.Controls.Add(Me.Label8)
        Me.tabWildcards.Controls.Add(Me.Label1)
        Me.tabWildcards.Controls.Add(Me.txtTryIt)
        Me.tabWildcards.Controls.Add(Me.txtWildcardExample)
        Me.tabWildcards.Controls.Add(Me.chkEdgesWild)
        Me.tabWildcards.Controls.Add(Me.chkSpacesWild)
        Me.tabWildcards.Location = New System.Drawing.Point(4, 22)
        Me.tabWildcards.Name = "tabWildcards"
        Me.tabWildcards.Padding = New System.Windows.Forms.Padding(3)
        Me.tabWildcards.Size = New System.Drawing.Size(411, 542)
        Me.tabWildcards.TabIndex = 2
        Me.tabWildcards.Text = "RUNTIME"
        Me.tabWildcards.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 147)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 13)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "Becomes:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 97)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(33, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Try it!"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(146, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "How filename input is treated:"
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.gbTextEditor)
        Me.TabPage4.Controls.Add(Me.chkConfigDefaultEditor)
        Me.TabPage4.Controls.Add(Me.chkSquealerDefaultEditor)
        Me.TabPage4.Controls.Add(Me.chkOutputDefaultEditor)
        Me.TabPage4.Controls.Add(Me.Label7)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(411, 542)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Editor"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'gbTextEditor
        '
        Me.gbTextEditor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbTextEditor.Controls.Add(Me.Label2)
        Me.gbTextEditor.Controls.Add(Me.txtEditorProgram)
        Me.gbTextEditor.Controls.Add(Me.btnEditorDialog)
        Me.gbTextEditor.Location = New System.Drawing.Point(6, 124)
        Me.gbTextEditor.Name = "gbTextEditor"
        Me.gbTextEditor.Size = New System.Drawing.Size(401, 98)
        Me.gbTextEditor.TabIndex = 8
        Me.gbTextEditor.TabStop = False
        Me.gbTextEditor.Text = "Text editor"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(300, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Select a text editor to use for any file type not checked above."
        '
        'txtEditorProgram
        '
        Me.txtEditorProgram.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEditorProgram.Location = New System.Drawing.Point(6, 58)
        Me.txtEditorProgram.Name = "txtEditorProgram"
        Me.txtEditorProgram.Size = New System.Drawing.Size(348, 20)
        Me.txtEditorProgram.TabIndex = 1
        '
        'btnEditorDialog
        '
        Me.btnEditorDialog.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditorDialog.Image = Global.Squealer.My.Resources.Resources.Folder
        Me.btnEditorDialog.Location = New System.Drawing.Point(360, 50)
        Me.btnEditorDialog.Name = "btnEditorDialog"
        Me.btnEditorDialog.Size = New System.Drawing.Size(35, 35)
        Me.btnEditorDialog.TabIndex = 3
        Me.btnEditorDialog.UseVisualStyleBackColor = True
        '
        'chkConfigDefaultEditor
        '
        Me.chkConfigDefaultEditor.AutoSize = True
        Me.chkConfigDefaultEditor.Location = New System.Drawing.Point(6, 86)
        Me.chkConfigDefaultEditor.Name = "chkConfigDefaultEditor"
        Me.chkConfigDefaultEditor.Size = New System.Drawing.Size(117, 17)
        Me.chkConfigDefaultEditor.TabIndex = 7
        Me.chkConfigDefaultEditor.Text = "Config file (*.config)"
        Me.chkConfigDefaultEditor.UseVisualStyleBackColor = True
        '
        'chkSquealerDefaultEditor
        '
        Me.chkSquealerDefaultEditor.AutoSize = True
        Me.chkSquealerDefaultEditor.Location = New System.Drawing.Point(6, 63)
        Me.chkSquealerDefaultEditor.Name = "chkSquealerDefaultEditor"
        Me.chkSquealerDefaultEditor.Size = New System.Drawing.Size(89, 17)
        Me.chkSquealerDefaultEditor.TabIndex = 6
        Me.chkSquealerDefaultEditor.Text = "Squealer files"
        Me.chkSquealerDefaultEditor.UseVisualStyleBackColor = True
        '
        'chkOutputDefaultEditor
        '
        Me.chkOutputDefaultEditor.AutoSize = True
        Me.chkOutputDefaultEditor.Location = New System.Drawing.Point(6, 40)
        Me.chkOutputDefaultEditor.Name = "chkOutputDefaultEditor"
        Me.chkOutputDefaultEditor.Size = New System.Drawing.Size(108, 17)
        Me.chkOutputDefaultEditor.TabIndex = 5
        Me.chkOutputDefaultEditor.Text = "Output files (*.sql)"
        Me.chkOutputDefaultEditor.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(204, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Use default Windows application to open:"
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnOK
        Me.ClientSize = New System.Drawing.Size(443, 623)
        Me.Controls.Add(Me.Tabs)
        Me.Controls.Add(Me.btnOK)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SettingsForm"
        Me.Text = "Settings"
        CType(Me.updnFolderSaves, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tabs.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabOutput.ResumeLayout(False)
        Me.tabOutput.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.tabWildcards.ResumeLayout(False)
        Me.tabWildcards.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.gbTextEditor.ResumeLayout(False)
        Me.gbTextEditor.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents optBeep As System.Windows.Forms.CheckBox
    Friend WithEvents chkEdgesWild As System.Windows.Forms.CheckBox
    Friend WithEvents chkSpacesWild As System.Windows.Forms.CheckBox
    Friend WithEvents optEditNewFiles As System.Windows.Forms.CheckBox
    Friend WithEvents optShowGitBranch As System.Windows.Forms.CheckBox
    Friend WithEvents rbCompact As System.Windows.Forms.RadioButton
    Friend WithEvents rbFull As System.Windows.Forms.RadioButton
    Friend WithEvents rbSymbolic As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents updnFolderSaves As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtWildcardExample As System.Windows.Forms.TextBox
    Friend WithEvents txtTryIt As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents optDetectOldSquealerObjects As System.Windows.Forms.CheckBox
    Friend WithEvents rbTempFile As System.Windows.Forms.RadioButton
    Friend WithEvents rbClipboard As System.Windows.Forms.RadioButton
    Friend WithEvents dlgTextEditor As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Tabs As Windows.Forms.TabControl
    Friend WithEvents tabGeneral As Windows.Forms.TabPage
    Friend WithEvents tabOutput As Windows.Forms.TabPage
    Friend WithEvents tabWildcards As Windows.Forms.TabPage
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents TabPage4 As Windows.Forms.TabPage
    Friend WithEvents txtEditorProgram As Windows.Forms.TextBox
    Friend WithEvents txtDirExample As Windows.Forms.TextBox
    Friend WithEvents chkConfigDefaultEditor As Windows.Forms.CheckBox
    Friend WithEvents chkSquealerDefaultEditor As Windows.Forms.CheckBox
    Friend WithEvents chkOutputDefaultEditor As Windows.Forms.CheckBox
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents btnEditorDialog As Windows.Forms.Button
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents Label9 As Windows.Forms.Label
    Friend WithEvents gbTextEditor As Windows.Forms.GroupBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtProgressExample As Windows.Forms.TextBox
    Friend WithEvents rbDetailed As Windows.Forms.RadioButton
    Friend WithEvents rbPercentage As Windows.Forms.RadioButton
    Friend WithEvents ddIncrement As Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As Windows.Forms.GroupBox
    Friend WithEvents chkShowProjectDirectoryInTitleBar As Windows.Forms.CheckBox
    Friend WithEvents chkShowProjectNameInTitleBar As Windows.Forms.CheckBox
    Friend WithEvents chkShowProjectNameInCommandPrompt As Windows.Forms.CheckBox
    Friend WithEvents chkKeepScreenOn As Windows.Forms.CheckBox
    Friend WithEvents chkLockWindowSize As Windows.Forms.CheckBox
    Friend WithEvents chkTrackFailedItems As Windows.Forms.CheckBox
    Friend WithEvents chkAlwaysShowSymbols As Windows.Forms.CheckBox
    Friend WithEvents chkAutoCompressGit As Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As Windows.Forms.GroupBox
    Friend WithEvents rbAlter As Windows.Forms.RadioButton
    Friend WithEvents rbDrop As Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As Windows.Forms.GroupBox
End Class
