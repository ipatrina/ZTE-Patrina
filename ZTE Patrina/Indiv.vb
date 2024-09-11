Public Class Indiv

    Private Sub BtnIndiv_Click(sender As Object, e As EventArgs) Handles BtnIndiv.Click
        Try
            MainUI.IndivKey = TxtIndiv.Text.Trim()
            Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Indiv_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            Dispose()
        Catch ex As Exception
            Dispose()
        End Try
    End Sub

    Private Sub Indiv_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AcceptButton = BtnIndiv
            TxtIndiv.Text = MainUI.IndivKey
        Catch ex As Exception

        End Try
    End Sub

End Class