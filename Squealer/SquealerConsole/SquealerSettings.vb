Public Class SquealerSettings

    Private _Blasts As Integer = 0
    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = My.Resources.PigNose
        UpdateExample()
    End Sub

    Private Sub optUseWildcards_CheckedChanged(sender As Object, e As EventArgs) Handles optUseWildcards.CheckedChanged, optSpacesAreWildcards.CheckedChanged
        UpdateExample()
    End Sub

    Private Sub UpdateExample()
        txtWildcardExample.Text = String.Format("DIR {0}your{1}file{1}search{0}", IIf(optUseWildcards.Checked, "*", "").ToString, IIf(optSpacesAreWildcards.Checked, "*", " ").ToString)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        dlgTextEditor.FileName = txtTextEditorProgram.Text
        Try
            dlgTextEditor.InitialDirectory = My.Computer.FileSystem.GetFileInfo(txtTextEditorProgram.Text).DirectoryName
        Catch ex As Exception
        End Try
        If dlgTextEditor.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            txtTextEditorProgram.Text = dlgTextEditor.FileName
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
            Case 1
                gbGeneral.Controls.Remove(gbStarwars)
                gbGeneral.Visible = False
                gbOutput.Visible = False
                gbFilenames.Visible = False
            Case 2
                gbStarwars.Visible = True
            Case 3
                lblLeaderboard.Visible = True
            Case 4
                txtLeaderboardCs.Visible = True
                lblLeaveBlank.Visible = True
            Case 5
                chkShowLeaderboard.Visible = True
            Case 6
                btnStarwarsHelp.Visible = True
            Case 7
                btnLeaderboardSql.Visible = True

            Case MaxBlasts
                blaster.Play(My.Resources.DroidScream, AudioPlayMode.Background)
                PictureBox1.Visible = False
        End Select
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles btnLeaderboardSql.Click
        Dim f As New TempFileHandler
        f.Writeline(My.Resources.LeaderboardCreate)
        f.Show(txtTextEditorProgram.Text)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnStarwarsHelp.Click
        System.Windows.Forms.MessageBox.Show(My.Resources.HowToLaunchR2)
    End Sub
End Class