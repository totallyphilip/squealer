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
        If _Blasts < 10 Then
            blaster.Play(My.Resources.BlasterFiring, AudioPlayMode.Background)
        End If
        Select Case _Blasts
            Case 1
                lblHint.Visible = True
            Case 2
                lblHint.Visible = False
                txtLeaderboardCs.Visible = True
            Case 3
                lblLeaderboard.Visible = True
            Case 4
                Dim f As New TempFileHandler
                f.Writeline(My.Resources.LeaderboardCreate)
                f.Show(txtTextEditorProgram.Text)
            Case 10
                blaster.Play(My.Resources.DroidScream, AudioPlayMode.Background)
        End Select
    End Sub

End Class