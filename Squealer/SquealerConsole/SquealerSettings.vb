Public Class SquealerSettings

    Private _Blasts As Integer = 0
    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = My.Resources.PigNose
        UpdateWildcardExample()
        UpdateDirectoryExample()
        Tabs.TabPages.Remove(tabEasterEgg)
        tabWildcards.Text = MyConstants.WildcardAsterisks
        SetEditorAccess()
    End Sub

    Private Sub optUseWildcards_CheckedChanged(sender As Object, e As EventArgs) Handles chkEdgesWild.CheckedChanged, chkSpacesWild.CheckedChanged, txtTryIt.TextChanged
        UpdateWildcardExample()
    End Sub

    Private Sub UpdateWildcardExample()
        txtWildcardExample.Text = Misc.WildcardInterpreter(txtTryIt.Text.Trim, chkSpacesWild.Checked, chkEdgesWild.Checked, False)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim blaster As New Microsoft.VisualBasic.Devices.Audio
        _Blasts += 1
        Const MaxBlasts As Integer = 12
        If _Blasts < MaxBlasts Then
            blaster.Play(My.Resources.BlasterFiring, AudioPlayMode.Background)
        End If
        Select Case _Blasts
            Case 3
                Tabs.TabPages.Add(tabEasterEgg)
            Case 6
                Tabs.SelectedTab = tabEasterEgg

            Case MaxBlasts
                blaster.Play(My.Resources.DroidScream, AudioPlayMode.Background)
                PictureBox1.Visible = False
        End Select
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles btnLeaderboardSql.Click
        Dim f As New TempFileHandler(".sql")
        f.Writeline(My.Resources.LeaderboardCreate)
        f.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnStarwarsHelp.Click
        System.Windows.Forms.MessageBox.Show(My.Resources.HowToLaunchR2)
    End Sub

    Private Sub rbCompact_CheckedChanged(sender As Object, e As EventArgs) Handles rbCompact.CheckedChanged, rbFull.CheckedChanged, rbSymbolic.CheckedChanged
        UpdateDirectoryExample()
    End Sub

    Private Sub UpdateDirectoryExample()

        If rbSymbolic.Checked Then
            txtDirExample.Text = "dbo.MyInlineTableValuedFunction*" _
                & vbCrLf & "dbo.MyMultiStatementTableValuedFunction**" _
                & vbCrLf & "dbo.MyScalarFunction()" _
                & vbCrLf & "dbo.MyStoredProcedure" _
                & vbCrLf & "dbo.MyView+"
        End If

        If rbCompact.Checked Then
            txtDirExample.Text = "if dbo.MyInlineTableValuedFunction" _
                & vbCrLf & "tf dbo.MyMultiStatementTableValuedFunction" _
                & vbCrLf & "fn dbo.MyScalarFunction" _
                & vbCrLf & "p  dbo.MyStoredProcedure" _
                & vbCrLf & "v  dbo.MyView"
        End If

        If rbFull.Checked Then
            txtDirExample.Text = "if [flags] dbo.MyInlineTableValuedFunction" _
                & vbCrLf & "tf [flags] dbo.MyMultiStatementTableValuedFunction" _
                & vbCrLf & "fn [flags] dbo.MyScalarFunction" _
                & vbCrLf & "p  [flags] dbo.MyStoredProcedure" _
                & vbCrLf & "v  [flags] dbo.MyView"
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnEditorDialog.Click
        dlgTextEditor.FileName = txtEditorProgram.Text
        Try
            dlgTextEditor.InitialDirectory = My.Computer.FileSystem.GetFileInfo(txtEditorProgram.Text).DirectoryName
        Catch ex As Exception
        End Try
        If dlgTextEditor.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            txtEditorProgram.Text = dlgTextEditor.FileName
        End If
    End Sub

    Private Sub chkOutputDefaultEditor_CheckedChanged(sender As Object, e As EventArgs) Handles chkOutputDefaultEditor.CheckedChanged, chkConfigDefaultEditor.CheckedChanged, chkSquealerDefaultEditor.CheckedChanged
        SetEditorAccess()
    End Sub

    Private Sub SetEditorAccess()
        txtEditorProgram.Enabled = Not (chkOutputDefaultEditor.Checked AndAlso chkConfigDefaultEditor.Checked AndAlso chkSquealerDefaultEditor.Checked)
        btnEditorDialog.Enabled = txtEditorProgram.Enabled
    End Sub

End Class