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

End Class