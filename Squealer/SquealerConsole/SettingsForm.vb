Public Class SettingsForm

    Private _Blasts As Integer = 0
    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = My.Resources.PigNose
        UpdateWildcardExample()
        UpdateDirectoryExample()
        Tabs.TabPages.Remove(tabEasterEgg)
        tabWildcards.Text = Constants.WildcardAsterisks
        SetEditorAccess()
        chkSquealerDefaultEditor.Text = String.Format("Squealer files (*{0})", Constants.SquealerFileExtension)
        If ddIncrement.SelectedIndex = -1 Then
            ddIncrement.SelectedIndex = ddIncrement.FindString("5")
        End If
    End Sub

    Private Sub optUseWildcards_CheckedChanged(sender As Object, e As EventArgs) Handles chkEdgesWild.CheckedChanged, chkSpacesWild.CheckedChanged, txtTryIt.TextChanged
        UpdateWildcardExample()
    End Sub

    Private Sub UpdateWildcardExample()
        txtWildcardExample.Text = Misc.WildcardInterpreter(txtTryIt.Text.Trim, chkSpacesWild.Checked, chkEdgesWild.Checked, False)
        If txtTryIt.Text.EndsWith(".sqlr") Then
            txtWildcardExample.Text = "<invalid!>"
        End If
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
        System.Windows.Forms.MessageBox.Show(My.Resources.HowToPlay)
    End Sub

    Private Sub rbCompact_CheckedChanged(sender As Object, e As EventArgs) Handles rbCompact.CheckedChanged, rbFull.CheckedChanged, rbSymbolic.CheckedChanged
        UpdateDirectoryExample()
    End Sub
    Private Sub rbOutputStyle_Changed(sender As Object, e As EventArgs) Handles rbDetailed.CheckedChanged, rbPercentage.CheckedChanged
        UpdateProgressExample()
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

    Private Sub UpdateProgressExample()

        If rbDetailed.Checked Then
            txtProgressExample.Text = "1/10 creating [dbo].[Func1], ScalarFunction" _
                & vbCrLf & "2/10 creating [dbo].[Proc1], StoredProcedure" _
                & vbCrLf & "3/10 creating [dbo].[Proc2], StoredProcedure" _
                & vbCrLf & "4/10 creating [dbo].[Proc3], StoredProcedure" _
                & vbCrLf & "5/10 creating [dbo].[Proc4], StoredProcedure" _
                & vbCrLf & "6/10 creating [dbo].[TableFunction1], InlineTableFunction" _
                & vbCrLf & "7/10 creating [dbo].[TableFunction2], MultiStatementTableFunction" _
                & vbCrLf & "8/10 creating [dbo].[View1], View" _
                & vbCrLf & "9/10 creating [dbo].[View2], View" _
                & vbCrLf & "10/10 creating [dbo].[View3], View"
        End If

        If rbPercentage.Checked Then
            txtProgressExample.Text = String.Empty
            Dim v As Integer = CInt(ddIncrement.Items(ddIncrement.SelectedIndex))
            For n As Integer = v To 100 Step v
                txtProgressExample.Text &= vbCrLf & String.Format("{0}%", n.ToString)
            Next
            txtProgressExample.Text = txtProgressExample.Text.Trim
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
        gbTextEditor.Visible = Not (chkOutputDefaultEditor.Checked AndAlso chkConfigDefaultEditor.Checked AndAlso chkSquealerDefaultEditor.Checked)
    End Sub

    Private Sub ddIncrement_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddIncrement.SelectedIndexChanged
        UpdateProgressExample()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        System.Windows.Forms.MessageBox.Show(rbDetailed.Checked.ToString)
    End Sub
End Class