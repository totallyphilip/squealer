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
        Me.gbGeneral = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.updnFolderSaves = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.gbFilenames = New System.Windows.Forms.GroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.txtWildcardExample = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.gbOutput = New System.Windows.Forms.GroupBox()
        Me.rbTempFile = New System.Windows.Forms.RadioButton()
        Me.rbClipboard = New System.Windows.Forms.RadioButton()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.optDetectOldSquealerObjects = New System.Windows.Forms.CheckBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.dlgTextEditor = New System.Windows.Forms.OpenFileDialog()
        Me.txtLeaderboardCs = New System.Windows.Forms.TextBox()
        Me.lblLeaderboard = New System.Windows.Forms.Label()
        Me.chkShowLeaderboard = New System.Windows.Forms.CheckBox()
        Me.gbStarwars = New System.Windows.Forms.GroupBox()
        Me.lblLeaveBlank = New System.Windows.Forms.Label()
        Me.btnStarwarsHelp = New System.Windows.Forms.Button()
        Me.btnLeaderboardSql = New System.Windows.Forms.Button()
        Me.gbGeneral.SuspendLayout()
        CType(Me.updnFolderSaves, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbFilenames.SuspendLayout()
        Me.gbOutput.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbStarwars.SuspendLayout()
        Me.SuspendLayout()
        '
        'optBeep
        '
        Me.optBeep.AutoSize = True
        Me.optBeep.Location = New System.Drawing.Point(6, 19)
        Me.optBeep.Name = "optBeep"
        Me.optBeep.Size = New System.Drawing.Size(90, 17)
        Me.optBeep.TabIndex = 1
        Me.optBeep.Text = "Beep on error"
        Me.optBeep.UseVisualStyleBackColor = True
        '
        'optUseWildcards
        '
        Me.optUseWildcards.AutoSize = True
        Me.optUseWildcards.Location = New System.Drawing.Point(6, 19)
        Me.optUseWildcards.Name = "optUseWildcards"
        Me.optUseWildcards.Size = New System.Drawing.Size(135, 17)
        Me.optUseWildcards.TabIndex = 3
        Me.optUseWildcards.Text = "Surround with asterisks"
        Me.optUseWildcards.UseVisualStyleBackColor = True
        '
        'optSpacesAreWildcards
        '
        Me.optSpacesAreWildcards.AutoSize = True
        Me.optSpacesAreWildcards.Location = New System.Drawing.Point(6, 42)
        Me.optSpacesAreWildcards.Name = "optSpacesAreWildcards"
        Me.optSpacesAreWildcards.Size = New System.Drawing.Size(146, 17)
        Me.optSpacesAreWildcards.TabIndex = 5
        Me.optSpacesAreWildcards.Text = "Treat spaces as asterisks"
        Me.optSpacesAreWildcards.UseVisualStyleBackColor = True
        '
        'optEditNewFiles
        '
        Me.optEditNewFiles.AutoSize = True
        Me.optEditNewFiles.Location = New System.Drawing.Point(6, 42)
        Me.optEditNewFiles.Name = "optEditNewFiles"
        Me.optEditNewFiles.Size = New System.Drawing.Size(285, 17)
        Me.optEditNewFiles.TabIndex = 6
        Me.optEditNewFiles.Text = "Automatically run EDIT command after NEW command"
        Me.optEditNewFiles.UseVisualStyleBackColor = True
        '
        'optShowGitBranch
        '
        Me.optShowGitBranch.AutoSize = True
        Me.optShowGitBranch.Location = New System.Drawing.Point(6, 65)
        Me.optShowGitBranch.Name = "optShowGitBranch"
        Me.optShowGitBranch.Size = New System.Drawing.Size(243, 17)
        Me.optShowGitBranch.TabIndex = 9
        Me.optShowGitBranch.Text = "Display the Git branch in the command prompt"
        Me.optShowGitBranch.UseVisualStyleBackColor = True
        '
        'rbCompact
        '
        Me.rbCompact.AutoSize = True
        Me.rbCompact.Location = New System.Drawing.Point(94, 89)
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
        Me.rbFull.Location = New System.Drawing.Point(166, 89)
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
        Me.rbSymbolic.Location = New System.Drawing.Point(210, 89)
        Me.rbSymbolic.Name = "rbSymbolic"
        Me.rbSymbolic.Size = New System.Drawing.Size(65, 17)
        Me.rbSymbolic.TabIndex = 13
        Me.rbSymbolic.TabStop = True
        Me.rbSymbolic.Text = "symbolic"
        Me.rbSymbolic.UseVisualStyleBackColor = True
        '
        'gbGeneral
        '
        Me.gbGeneral.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbGeneral.Controls.Add(Me.Label3)
        Me.gbGeneral.Controls.Add(Me.updnFolderSaves)
        Me.gbGeneral.Controls.Add(Me.Label5)
        Me.gbGeneral.Controls.Add(Me.optShowGitBranch)
        Me.gbGeneral.Controls.Add(Me.optEditNewFiles)
        Me.gbGeneral.Controls.Add(Me.rbFull)
        Me.gbGeneral.Controls.Add(Me.optBeep)
        Me.gbGeneral.Controls.Add(Me.rbSymbolic)
        Me.gbGeneral.Controls.Add(Me.rbCompact)
        Me.gbGeneral.Location = New System.Drawing.Point(12, 12)
        Me.gbGeneral.Name = "gbGeneral"
        Me.gbGeneral.Size = New System.Drawing.Size(596, 164)
        Me.gbGeneral.TabIndex = 14
        Me.gbGeneral.TabStop = False
        Me.gbGeneral.Text = "General"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 123)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(151, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Number of folders to remember"
        '
        'updnFolderSaves
        '
        Me.updnFolderSaves.Location = New System.Drawing.Point(166, 121)
        Me.updnFolderSaves.Name = "updnFolderSaves"
        Me.updnFolderSaves.Size = New System.Drawing.Size(55, 20)
        Me.updnFolderSaves.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 91)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(81, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Directory format"
        '
        'gbFilenames
        '
        Me.gbFilenames.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbFilenames.Controls.Add(Me.TextBox1)
        Me.gbFilenames.Controls.Add(Me.optSpacesAreWildcards)
        Me.gbFilenames.Controls.Add(Me.optUseWildcards)
        Me.gbFilenames.Controls.Add(Me.txtWildcardExample)
        Me.gbFilenames.Controls.Add(Me.Label4)
        Me.gbFilenames.Location = New System.Drawing.Point(12, 278)
        Me.gbFilenames.Name = "gbFilenames"
        Me.gbFilenames.Size = New System.Drawing.Size(596, 73)
        Me.gbFilenames.TabIndex = 15
        Me.gbFilenames.TabStop = False
        Me.gbFilenames.Text = "Filename matching"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(208, 29)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(124, 20)
        Me.TextBox1.TabIndex = 16
        Me.TextBox1.Text = "DIR your file search"
        '
        'txtWildcardExample
        '
        Me.txtWildcardExample.Location = New System.Drawing.Point(357, 29)
        Me.txtWildcardExample.Name = "txtWildcardExample"
        Me.txtWildcardExample.ReadOnly = True
        Me.txtWildcardExample.Size = New System.Drawing.Size(124, 20)
        Me.txtWildcardExample.TabIndex = 14
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(338, 32)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(13, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "="
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnOK.Location = New System.Drawing.Point(533, 453)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 18
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'gbOutput
        '
        Me.gbOutput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbOutput.Controls.Add(Me.rbTempFile)
        Me.gbOutput.Controls.Add(Me.rbClipboard)
        Me.gbOutput.Controls.Add(Me.Label6)
        Me.gbOutput.Controls.Add(Me.optDetectOldSquealerObjects)
        Me.gbOutput.Location = New System.Drawing.Point(12, 182)
        Me.gbOutput.Name = "gbOutput"
        Me.gbOutput.Size = New System.Drawing.Size(596, 90)
        Me.gbOutput.TabIndex = 19
        Me.gbOutput.TabStop = False
        Me.gbOutput.Text = "Proc/Function/View output"
        '
        'rbTempFile
        '
        Me.rbTempFile.AutoSize = True
        Me.rbTempFile.Location = New System.Drawing.Point(168, 42)
        Me.rbTempFile.Name = "rbTempFile"
        Me.rbTempFile.Size = New System.Drawing.Size(113, 17)
        Me.rbTempFile.TabIndex = 11
        Me.rbTempFile.TabStop = True
        Me.rbTempFile.Text = "text editor temp file"
        Me.rbTempFile.UseVisualStyleBackColor = True
        '
        'rbClipboard
        '
        Me.rbClipboard.AutoSize = True
        Me.rbClipboard.Location = New System.Drawing.Point(94, 42)
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
        Me.Label6.Location = New System.Drawing.Point(6, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 13)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Send output to"
        '
        'optDetectOldSquealerObjects
        '
        Me.optDetectOldSquealerObjects.AutoSize = True
        Me.optDetectOldSquealerObjects.Location = New System.Drawing.Point(6, 19)
        Me.optDetectOldSquealerObjects.Name = "optDetectOldSquealerObjects"
        Me.optDetectOldSquealerObjects.Size = New System.Drawing.Size(197, 17)
        Me.optDetectOldSquealerObjects.TabIndex = 8
        Me.optDetectOldSquealerObjects.Text = "Detect deprecated Squealer objects"
        Me.optDetectOldSquealerObjects.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Squealer.My.Resources.Resources.RebelAlliance
        Me.PictureBox1.Location = New System.Drawing.Point(11, 452)
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
        Me.txtLeaderboardCs.Location = New System.Drawing.Point(163, 19)
        Me.txtLeaderboardCs.Name = "txtLeaderboardCs"
        Me.txtLeaderboardCs.Size = New System.Drawing.Size(427, 20)
        Me.txtLeaderboardCs.TabIndex = 21
        Me.txtLeaderboardCs.Visible = False
        '
        'lblLeaderboard
        '
        Me.lblLeaderboard.AutoSize = True
        Me.lblLeaderboard.Location = New System.Drawing.Point(6, 23)
        Me.lblLeaderboard.Name = "lblLeaderboard"
        Me.lblLeaderboard.Size = New System.Drawing.Size(151, 13)
        Me.lblLeaderboard.TabIndex = 22
        Me.lblLeaderboard.Text = "Leaderboard ConnectionString"
        Me.lblLeaderboard.Visible = False
        '
        'chkShowLeaderboard
        '
        Me.chkShowLeaderboard.AutoSize = True
        Me.chkShowLeaderboard.Location = New System.Drawing.Point(6, 65)
        Me.chkShowLeaderboard.Name = "chkShowLeaderboard"
        Me.chkShowLeaderboard.Size = New System.Drawing.Size(134, 17)
        Me.chkShowLeaderboard.TabIndex = 23
        Me.chkShowLeaderboard.Text = "Show scores at startup"
        Me.chkShowLeaderboard.UseVisualStyleBackColor = True
        Me.chkShowLeaderboard.Visible = False
        '
        'gbStarwars
        '
        Me.gbStarwars.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbStarwars.Controls.Add(Me.lblLeaveBlank)
        Me.gbStarwars.Controls.Add(Me.btnStarwarsHelp)
        Me.gbStarwars.Controls.Add(Me.btnLeaderboardSql)
        Me.gbStarwars.Controls.Add(Me.chkShowLeaderboard)
        Me.gbStarwars.Controls.Add(Me.lblLeaderboard)
        Me.gbStarwars.Controls.Add(Me.txtLeaderboardCs)
        Me.gbStarwars.Location = New System.Drawing.Point(12, 12)
        Me.gbStarwars.Name = "gbStarwars"
        Me.gbStarwars.Size = New System.Drawing.Size(596, 158)
        Me.gbStarwars.TabIndex = 24
        Me.gbStarwars.TabStop = False
        Me.gbStarwars.Text = "May the Force be with you!"
        Me.gbStarwars.Visible = False
        '
        'lblLeaveBlank
        '
        Me.lblLeaveBlank.AutoSize = True
        Me.lblLeaveBlank.Location = New System.Drawing.Point(163, 45)
        Me.lblLeaveBlank.Name = "lblLeaveBlank"
        Me.lblLeaveBlank.Size = New System.Drawing.Size(176, 13)
        Me.lblLeaveBlank.TabIndex = 26
        Me.lblLeaveBlank.Text = "Leave blank to disable leaderboard."
        Me.lblLeaveBlank.Visible = False
        '
        'btnStarwarsHelp
        '
        Me.btnStarwarsHelp.Location = New System.Drawing.Point(6, 88)
        Me.btnStarwarsHelp.Name = "btnStarwarsHelp"
        Me.btnStarwarsHelp.Size = New System.Drawing.Size(121, 23)
        Me.btnStarwarsHelp.TabIndex = 25
        Me.btnStarwarsHelp.Text = "Help me Obi Wan"
        Me.btnStarwarsHelp.UseVisualStyleBackColor = True
        Me.btnStarwarsHelp.Visible = False
        '
        'btnLeaderboardSql
        '
        Me.btnLeaderboardSql.Location = New System.Drawing.Point(6, 117)
        Me.btnLeaderboardSql.Name = "btnLeaderboardSql"
        Me.btnLeaderboardSql.Size = New System.Drawing.Size(121, 23)
        Me.btnLeaderboardSql.TabIndex = 24
        Me.btnLeaderboardSql.Text = "TOP SECRET"
        Me.btnLeaderboardSql.UseVisualStyleBackColor = True
        Me.btnLeaderboardSql.Visible = False
        '
        'SquealerSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnOK
        Me.ClientSize = New System.Drawing.Size(620, 488)
        Me.Controls.Add(Me.gbStarwars)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.gbOutput)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.gbFilenames)
        Me.Controls.Add(Me.gbGeneral)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SquealerSettings"
        Me.Text = "Settings"
        Me.gbGeneral.ResumeLayout(False)
        Me.gbGeneral.PerformLayout()
        CType(Me.updnFolderSaves, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbFilenames.ResumeLayout(False)
        Me.gbFilenames.PerformLayout()
        Me.gbOutput.ResumeLayout(False)
        Me.gbOutput.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbStarwars.ResumeLayout(False)
        Me.gbStarwars.PerformLayout()
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
    Friend WithEvents gbGeneral As System.Windows.Forms.GroupBox
    Friend WithEvents gbFilenames As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents updnFolderSaves As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtWildcardExample As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents gbOutput As System.Windows.Forms.GroupBox
    Friend WithEvents optDetectOldSquealerObjects As System.Windows.Forms.CheckBox
    Friend WithEvents rbTempFile As System.Windows.Forms.RadioButton
    Friend WithEvents rbClipboard As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dlgTextEditor As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtLeaderboardCs As System.Windows.Forms.TextBox
    Friend WithEvents lblLeaderboard As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents chkShowLeaderboard As System.Windows.Forms.CheckBox
    Friend WithEvents gbStarwars As System.Windows.Forms.GroupBox
    Friend WithEvents btnLeaderboardSql As System.Windows.Forms.Button
    Friend WithEvents btnStarwarsHelp As System.Windows.Forms.Button
    Friend WithEvents lblLeaveBlank As System.Windows.Forms.Label
End Class
