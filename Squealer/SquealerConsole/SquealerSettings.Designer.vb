<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SquealerSettings
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
        Me.optUseWildcards = New System.Windows.Forms.CheckBox()
        Me.optSpacesAreWildcards = New System.Windows.Forms.CheckBox()
        Me.optEditNewFiles = New System.Windows.Forms.CheckBox()
        Me.optShowGitBranch = New System.Windows.Forms.CheckBox()
        Me.rbCompact = New System.Windows.Forms.RadioButton()
        Me.rbFull = New System.Windows.Forms.RadioButton()
        Me.rbSymbolic = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.updnFolderSaves = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtTryIt = New System.Windows.Forms.TextBox()
        Me.txtWildcardExample = New System.Windows.Forms.TextBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.rbTempFile = New System.Windows.Forms.RadioButton()
        Me.rbClipboard = New System.Windows.Forms.RadioButton()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.optDetectOldSquealerObjects = New System.Windows.Forms.CheckBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.dlgTextEditor = New System.Windows.Forms.OpenFileDialog()
        Me.txtLeaderboardCs = New System.Windows.Forms.TextBox()
        Me.lblLeaderboard = New System.Windows.Forms.Label()
        Me.chkShowLeaderboard = New System.Windows.Forms.CheckBox()
        Me.lblLeaveBlank = New System.Windows.Forms.Label()
        Me.btnStarwarsHelp = New System.Windows.Forms.Button()
        Me.btnLeaderboardSql = New System.Windows.Forms.Button()
        Me.Tabs = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.txtDirExample = New System.Windows.Forms.TextBox()
        Me.tabOutput = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.chkConfigDefaultEditor = New System.Windows.Forms.CheckBox()
        Me.chkSquealerDefaultEditor = New System.Windows.Forms.CheckBox()
        Me.chkOutputDefaultEditor = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnEditorDialog = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtEditorProgram = New System.Windows.Forms.TextBox()
        Me.tabEasterEgg = New System.Windows.Forms.TabPage()
        Me.Label9 = New System.Windows.Forms.Label()
        CType(Me.updnFolderSaves, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tabs.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabOutput.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.tabEasterEgg.SuspendLayout()
        Me.SuspendLayout()
        '
        'optBeep
        '
        Me.optBeep.AutoSize = True
        Me.optBeep.Location = New System.Drawing.Point(8, 6)
        Me.optBeep.Name = "optBeep"
        Me.optBeep.Size = New System.Drawing.Size(90, 17)
        Me.optBeep.TabIndex = 1
        Me.optBeep.Text = "Beep on error"
        Me.optBeep.UseVisualStyleBackColor = True
        '
        'optUseWildcards
        '
        Me.optUseWildcards.AutoSize = True
        Me.optUseWildcards.Location = New System.Drawing.Point(9, 39)
        Me.optUseWildcards.Name = "optUseWildcards"
        Me.optUseWildcards.Size = New System.Drawing.Size(135, 17)
        Me.optUseWildcards.TabIndex = 3
        Me.optUseWildcards.Text = "Surround with asterisks"
        Me.optUseWildcards.UseVisualStyleBackColor = True
        '
        'optSpacesAreWildcards
        '
        Me.optSpacesAreWildcards.AutoSize = True
        Me.optSpacesAreWildcards.Location = New System.Drawing.Point(9, 62)
        Me.optSpacesAreWildcards.Name = "optSpacesAreWildcards"
        Me.optSpacesAreWildcards.Size = New System.Drawing.Size(146, 17)
        Me.optSpacesAreWildcards.TabIndex = 5
        Me.optSpacesAreWildcards.Text = "Treat spaces as asterisks"
        Me.optSpacesAreWildcards.UseVisualStyleBackColor = True
        '
        'optEditNewFiles
        '
        Me.optEditNewFiles.AutoSize = True
        Me.optEditNewFiles.Location = New System.Drawing.Point(8, 29)
        Me.optEditNewFiles.Name = "optEditNewFiles"
        Me.optEditNewFiles.Size = New System.Drawing.Size(285, 17)
        Me.optEditNewFiles.TabIndex = 6
        Me.optEditNewFiles.Text = "Automatically run EDIT command after NEW command"
        Me.optEditNewFiles.UseVisualStyleBackColor = True
        '
        'optShowGitBranch
        '
        Me.optShowGitBranch.AutoSize = True
        Me.optShowGitBranch.Location = New System.Drawing.Point(8, 51)
        Me.optShowGitBranch.Name = "optShowGitBranch"
        Me.optShowGitBranch.Size = New System.Drawing.Size(243, 17)
        Me.optShowGitBranch.TabIndex = 9
        Me.optShowGitBranch.Text = "Display the Git branch in the command prompt"
        Me.optShowGitBranch.UseVisualStyleBackColor = True
        '
        'rbCompact
        '
        Me.rbCompact.AutoSize = True
        Me.rbCompact.Location = New System.Drawing.Point(383, 156)
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
        Me.rbFull.Location = New System.Drawing.Point(383, 133)
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
        Me.rbSymbolic.Location = New System.Drawing.Point(383, 179)
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
        Me.Label3.Location = New System.Drawing.Point(8, 252)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Remember up to"
        '
        'updnFolderSaves
        '
        Me.updnFolderSaves.Location = New System.Drawing.Point(99, 250)
        Me.updnFolderSaves.Name = "updnFolderSaves"
        Me.updnFolderSaves.Size = New System.Drawing.Size(55, 20)
        Me.updnFolderSaves.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 90)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(81, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Directory format"
        '
        'txtTryIt
        '
        Me.txtTryIt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTryIt.Location = New System.Drawing.Point(6, 114)
        Me.txtTryIt.Name = "txtTryIt"
        Me.txtTryIt.Size = New System.Drawing.Size(519, 20)
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
        Me.txtWildcardExample.Size = New System.Drawing.Size(519, 20)
        Me.txtWildcardExample.TabIndex = 14
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnOK.Location = New System.Drawing.Point(472, 386)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 18
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'rbTempFile
        '
        Me.rbTempFile.AutoSize = True
        Me.rbTempFile.Location = New System.Drawing.Point(166, 39)
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
        Me.rbClipboard.Location = New System.Drawing.Point(92, 39)
        Me.rbClipboard.Name = "rbClipboard"
        Me.rbClipboard.Size = New System.Drawing.Size(68, 17)
        Me.rbClipboard.TabIndex = 10
        Me.rbClipboard.TabStop = True
        Me.rbClipboard.Text = "clipboard"
        Me.rbClipboard.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 41)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 13)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Send output to"
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
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 385)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(24, 24)
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = False
        '
        'dlgTextEditor
        '
        Me.dlgTextEditor.DefaultExt = "exe"
        Me.dlgTextEditor.FileName = "OpenFileDialog1"
        Me.dlgTextEditor.Filter = "*.exe|*.exe"
        '
        'txtLeaderboardCs
        '
        Me.txtLeaderboardCs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLeaderboardCs.Location = New System.Drawing.Point(167, 6)
        Me.txtLeaderboardCs.Name = "txtLeaderboardCs"
        Me.txtLeaderboardCs.Size = New System.Drawing.Size(358, 20)
        Me.txtLeaderboardCs.TabIndex = 21
        '
        'lblLeaderboard
        '
        Me.lblLeaderboard.AutoSize = True
        Me.lblLeaderboard.Location = New System.Drawing.Point(8, 9)
        Me.lblLeaderboard.Name = "lblLeaderboard"
        Me.lblLeaderboard.Size = New System.Drawing.Size(151, 13)
        Me.lblLeaderboard.TabIndex = 22
        Me.lblLeaderboard.Text = "Leaderboard ConnectionString"
        '
        'chkShowLeaderboard
        '
        Me.chkShowLeaderboard.AutoSize = True
        Me.chkShowLeaderboard.Location = New System.Drawing.Point(11, 60)
        Me.chkShowLeaderboard.Name = "chkShowLeaderboard"
        Me.chkShowLeaderboard.Size = New System.Drawing.Size(134, 17)
        Me.chkShowLeaderboard.TabIndex = 23
        Me.chkShowLeaderboard.Text = "Show scores at startup"
        Me.chkShowLeaderboard.UseVisualStyleBackColor = True
        '
        'lblLeaveBlank
        '
        Me.lblLeaveBlank.AutoSize = True
        Me.lblLeaveBlank.Location = New System.Drawing.Point(164, 29)
        Me.lblLeaveBlank.Name = "lblLeaveBlank"
        Me.lblLeaveBlank.Size = New System.Drawing.Size(176, 13)
        Me.lblLeaveBlank.TabIndex = 26
        Me.lblLeaveBlank.Text = "Leave blank to disable leaderboard."
        '
        'btnStarwarsHelp
        '
        Me.btnStarwarsHelp.Location = New System.Drawing.Point(11, 83)
        Me.btnStarwarsHelp.Name = "btnStarwarsHelp"
        Me.btnStarwarsHelp.Size = New System.Drawing.Size(121, 23)
        Me.btnStarwarsHelp.TabIndex = 25
        Me.btnStarwarsHelp.Text = "Help me Obi Wan"
        Me.btnStarwarsHelp.UseVisualStyleBackColor = True
        '
        'btnLeaderboardSql
        '
        Me.btnLeaderboardSql.Location = New System.Drawing.Point(11, 112)
        Me.btnLeaderboardSql.Name = "btnLeaderboardSql"
        Me.btnLeaderboardSql.Size = New System.Drawing.Size(121, 23)
        Me.btnLeaderboardSql.TabIndex = 24
        Me.btnLeaderboardSql.Text = "TOP SECRET"
        Me.btnLeaderboardSql.UseVisualStyleBackColor = True
        '
        'Tabs
        '
        Me.Tabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tabs.Controls.Add(Me.tabGeneral)
        Me.Tabs.Controls.Add(Me.tabOutput)
        Me.Tabs.Controls.Add(Me.TabPage3)
        Me.Tabs.Controls.Add(Me.TabPage4)
        Me.Tabs.Controls.Add(Me.tabEasterEgg)
        Me.Tabs.Location = New System.Drawing.Point(12, 12)
        Me.Tabs.Name = "Tabs"
        Me.Tabs.SelectedIndex = 0
        Me.Tabs.Size = New System.Drawing.Size(539, 367)
        Me.Tabs.TabIndex = 26
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.Label9)
        Me.tabGeneral.Controls.Add(Me.txtDirExample)
        Me.tabGeneral.Controls.Add(Me.Label3)
        Me.tabGeneral.Controls.Add(Me.updnFolderSaves)
        Me.tabGeneral.Controls.Add(Me.optBeep)
        Me.tabGeneral.Controls.Add(Me.optEditNewFiles)
        Me.tabGeneral.Controls.Add(Me.Label5)
        Me.tabGeneral.Controls.Add(Me.rbFull)
        Me.tabGeneral.Controls.Add(Me.optShowGitBranch)
        Me.tabGeneral.Controls.Add(Me.rbSymbolic)
        Me.tabGeneral.Controls.Add(Me.rbCompact)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(531, 341)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'txtDirExample
        '
        Me.txtDirExample.Location = New System.Drawing.Point(8, 111)
        Me.txtDirExample.Multiline = True
        Me.txtDirExample.Name = "txtDirExample"
        Me.txtDirExample.ReadOnly = True
        Me.txtDirExample.Size = New System.Drawing.Size(369, 107)
        Me.txtDirExample.TabIndex = 15
        '
        'tabOutput
        '
        Me.tabOutput.Controls.Add(Me.rbTempFile)
        Me.tabOutput.Controls.Add(Me.optDetectOldSquealerObjects)
        Me.tabOutput.Controls.Add(Me.rbClipboard)
        Me.tabOutput.Controls.Add(Me.Label6)
        Me.tabOutput.Location = New System.Drawing.Point(4, 22)
        Me.tabOutput.Name = "tabOutput"
        Me.tabOutput.Padding = New System.Windows.Forms.Padding(3)
        Me.tabOutput.Size = New System.Drawing.Size(531, 341)
        Me.tabOutput.TabIndex = 1
        Me.tabOutput.Text = "Output"
        Me.tabOutput.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Label4)
        Me.TabPage3.Controls.Add(Me.Label8)
        Me.TabPage3.Controls.Add(Me.Label1)
        Me.TabPage3.Controls.Add(Me.txtTryIt)
        Me.TabPage3.Controls.Add(Me.txtWildcardExample)
        Me.TabPage3.Controls.Add(Me.optUseWildcards)
        Me.TabPage3.Controls.Add(Me.optSpacesAreWildcards)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(531, 341)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Wildcards"
        Me.TabPage3.UseVisualStyleBackColor = True
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
        Me.TabPage4.Controls.Add(Me.chkConfigDefaultEditor)
        Me.TabPage4.Controls.Add(Me.chkSquealerDefaultEditor)
        Me.TabPage4.Controls.Add(Me.chkOutputDefaultEditor)
        Me.TabPage4.Controls.Add(Me.Label7)
        Me.TabPage4.Controls.Add(Me.btnEditorDialog)
        Me.TabPage4.Controls.Add(Me.Label2)
        Me.TabPage4.Controls.Add(Me.txtEditorProgram)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(531, 341)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Editor"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'chkConfigDefaultEditor
        '
        Me.chkConfigDefaultEditor.AutoSize = True
        Me.chkConfigDefaultEditor.Location = New System.Drawing.Point(6, 86)
        Me.chkConfigDefaultEditor.Name = "chkConfigDefaultEditor"
        Me.chkConfigDefaultEditor.Size = New System.Drawing.Size(72, 17)
        Me.chkConfigDefaultEditor.TabIndex = 7
        Me.chkConfigDefaultEditor.Text = "Config file"
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
        Me.chkOutputDefaultEditor.Size = New System.Drawing.Size(58, 17)
        Me.chkOutputDefaultEditor.TabIndex = 5
        Me.chkOutputDefaultEditor.Text = "Output"
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
        'btnEditorDialog
        '
        Me.btnEditorDialog.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditorDialog.Image = Global.Squealer.My.Resources.Resources.Folder
        Me.btnEditorDialog.Location = New System.Drawing.Point(490, 136)
        Me.btnEditorDialog.Name = "btnEditorDialog"
        Me.btnEditorDialog.Size = New System.Drawing.Size(35, 35)
        Me.btnEditorDialog.TabIndex = 3
        Me.btnEditorDialog.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 124)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Custom text editor:"
        '
        'txtEditorProgram
        '
        Me.txtEditorProgram.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEditorProgram.Location = New System.Drawing.Point(11, 144)
        Me.txtEditorProgram.Name = "txtEditorProgram"
        Me.txtEditorProgram.ReadOnly = True
        Me.txtEditorProgram.Size = New System.Drawing.Size(473, 20)
        Me.txtEditorProgram.TabIndex = 1
        '
        'tabEasterEgg
        '
        Me.tabEasterEgg.Controls.Add(Me.btnStarwarsHelp)
        Me.tabEasterEgg.Controls.Add(Me.lblLeaveBlank)
        Me.tabEasterEgg.Controls.Add(Me.btnLeaderboardSql)
        Me.tabEasterEgg.Controls.Add(Me.chkShowLeaderboard)
        Me.tabEasterEgg.Controls.Add(Me.lblLeaderboard)
        Me.tabEasterEgg.Controls.Add(Me.txtLeaderboardCs)
        Me.tabEasterEgg.Location = New System.Drawing.Point(4, 22)
        Me.tabEasterEgg.Name = "tabEasterEgg"
        Me.tabEasterEgg.Padding = New System.Windows.Forms.Padding(3)
        Me.tabEasterEgg.Size = New System.Drawing.Size(531, 341)
        Me.tabEasterEgg.TabIndex = 4
        Me.tabEasterEgg.Text = "Easter Egg"
        Me.tabEasterEgg.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(160, 252)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "project folders"
        '
        'SquealerSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnOK
        Me.ClientSize = New System.Drawing.Size(563, 421)
        Me.Controls.Add(Me.Tabs)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnOK)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SquealerSettings"
        Me.Text = "Settings"
        CType(Me.updnFolderSaves, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tabs.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabOutput.ResumeLayout(False)
        Me.tabOutput.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.tabEasterEgg.ResumeLayout(False)
        Me.tabEasterEgg.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents optBeep As System.Windows.Forms.CheckBox
    Friend WithEvents optUseWildcards As System.Windows.Forms.CheckBox
    Friend WithEvents optSpacesAreWildcards As System.Windows.Forms.CheckBox
    Friend WithEvents optEditNewFiles As System.Windows.Forms.CheckBox
    Friend WithEvents optShowGitBranch As System.Windows.Forms.CheckBox
    Friend WithEvents rbCompact As System.Windows.Forms.RadioButton
    Friend WithEvents rbFull As System.Windows.Forms.RadioButton
    Friend WithEvents rbSymbolic As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents updnFolderSaves As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtWildcardExample As System.Windows.Forms.TextBox
    Friend WithEvents txtTryIt As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents optDetectOldSquealerObjects As System.Windows.Forms.CheckBox
    Friend WithEvents rbTempFile As System.Windows.Forms.RadioButton
    Friend WithEvents rbClipboard As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dlgTextEditor As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtLeaderboardCs As System.Windows.Forms.TextBox
    Friend WithEvents lblLeaderboard As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents chkShowLeaderboard As System.Windows.Forms.CheckBox
    Friend WithEvents btnLeaderboardSql As System.Windows.Forms.Button
    Friend WithEvents btnStarwarsHelp As System.Windows.Forms.Button
    Friend WithEvents lblLeaveBlank As System.Windows.Forms.Label
    Friend WithEvents Tabs As Windows.Forms.TabControl
    Friend WithEvents tabGeneral As Windows.Forms.TabPage
    Friend WithEvents tabOutput As Windows.Forms.TabPage
    Friend WithEvents TabPage3 As Windows.Forms.TabPage
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents TabPage4 As Windows.Forms.TabPage
    Friend WithEvents txtEditorProgram As Windows.Forms.TextBox
    Friend WithEvents txtDirExample As Windows.Forms.TextBox
    Friend WithEvents chkConfigDefaultEditor As Windows.Forms.CheckBox
    Friend WithEvents chkSquealerDefaultEditor As Windows.Forms.CheckBox
    Friend WithEvents chkOutputDefaultEditor As Windows.Forms.CheckBox
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents btnEditorDialog As Windows.Forms.Button
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents tabEasterEgg As Windows.Forms.TabPage
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents Label9 As Windows.Forms.Label
End Class
