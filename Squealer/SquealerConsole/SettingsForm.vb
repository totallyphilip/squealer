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
        Const MaxBlasts As Integer = 7
        Select Case _Blasts
            Case 3
                Tabs.TabPages.Add(tabEasterEgg)
                Tabs.SelectTab(tabEasterEgg)
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
    Private Sub rbOutputStyle_Changed(sender As Object, e As EventArgs) Handles rbDetailed.CheckedChanged, rbPercentage.CheckedChanged, rbDetailed.Click, rbPercentage.Click
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

        txtProgressExample.Text = String.Empty

        If rbDetailed.Checked Then
            Dim r As New Random
            Dim total As Integer = r.Next(5, 50)
            For n As Integer = 1 To total
                txtProgressExample.Text &= vbCrLf & String.Format("{0}/{1} creating [dbo].[File{0}], ", n, total)
                Dim e As SquealerObjectType.eType = DirectCast(r.Next(1, 5), SquealerObjectType.eType)
                txtProgressExample.Text &= e.ToString
            Next
        End If

        If rbPercentage.Checked Then
            Dim r As Integer = New Random().Next(20, 500)
            Dim v As Integer = CInt(ddIncrement.Items(ddIncrement.SelectedIndex))
            For n As Integer = v To 100 Step v
                txtProgressExample.Text &= vbCrLf & String.Format("{0}% ({1}/{2})", n, Math.Floor((n / 100) * r), r)
            Next
        End If

        txtProgressExample.Text = txtProgressExample.Text.Trim

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
        rbPercentage.Checked = True
        UpdateProgressExample()
    End Sub

End Class