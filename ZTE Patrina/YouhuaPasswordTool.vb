Imports System.Text

Public Class YouhuaPasswordTool
    Private Sub YouhuaPasswordTool_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            Dispose()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BtnDecode_Click(sender As Object, e As EventArgs) Handles BtnDecode.Click
        Try
            Dim _loc_1 As Byte() = Encoding.UTF8.GetBytes(TxtPassword.Text)
            Dim _loc_2 As Byte() = New Byte(_loc_1.Length - 1) {}
            For _loc_3 = 1 To _loc_1.Length
                _loc_2(_loc_3 - 1) = Int(_loc_1(_loc_1.Length - _loc_3)) - 1
            Next
            TxtPassword.Text = Encoding.UTF8.GetString(_loc_2)
        Catch ex As Exception

        End Try
    End Sub
End Class