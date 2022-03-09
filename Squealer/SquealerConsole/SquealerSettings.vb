Public Class SquealerSettings
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
End Class