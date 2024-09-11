Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading

Public Class License

    Public Licensed As Boolean = False
    Public LicenseMessage As String = ""

    Private Sub BtnLicense_Click(sender As Object, e As EventArgs) Handles BtnLicense.Click
        Try
            Dispose()
            End
        Catch ex As Exception
            Dispose()
            End
        End Try
    End Sub

    Private Sub License_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            If Not Licensed Then
                Dispose()
                End
            End If
        Catch ex As Exception
            Dispose()
            End
        End Try
    End Sub

    Private Sub License_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AcceptButton = BtnLicense
            TxtLicense.Select(0, 0)
            BgwLicense.RunWorkerAsync()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BgwLicense_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BgwLicense.DoWork
        Try
            Dim _loc_2 As String = (Time() & GetRndString(20)).Substring(0, 16)
            Dim _loc_3 As Byte() = Encoding.UTF8.GetBytes(_loc_2)
            Dim httpReq As System.Net.HttpWebRequest
            Dim httpResp As System.Net.HttpWebResponse
            Dim httpURL As New System.Uri("[license_server]/v1/?app=ztepatrina&version=" & MainUI.VersionStrings(0) & "." & MainUI.VersionStrings(1) & "." & MainUI.VersionStrings(2) & "&machineid=" & MainUI.BytesToHex(New MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(MainUI.GetMachineID()))).ToLower() & "&salt=" & _loc_2)
            httpReq = CType(WebRequest.Create(httpURL), HttpWebRequest)
            httpReq.Method = "GET"
            httpReq.AllowAutoRedirect = True
            httpReq.Timeout = 10000
            httpReq.KeepAlive = True
            httpReq.ServicePoint.ConnectionLimit = Integer.MaxValue
            httpResp = CType(httpReq.GetResponse(), HttpWebResponse)
            Dim MachineID As Byte() = New BinaryReader(httpResp.GetResponseStream).ReadBytes(16)

            If MainUI.GetMachineID().Length < 4 Then
                LicenseMessage = "Machine ID获取失败。"
            ElseIf MachineID.Length < 16 Then
                LicenseMessage = "License信息无效。"
            Else
                For _loc_1 As Integer = 0 To 15
                    MainUI.MachineID(_loc_1) = MachineID(_loc_1) Xor MainUI.MachineID(_loc_1) Xor _loc_3(_loc_1)
                Next

                Try
                    If MainUI.MachineID(7) * MainUI.MachineID(6) + MainUI.MachineID(5) + MainUI.MachineID(4) + MainUI.MachineID(14) - MainUI.MachineID(13) = MainUI.MachineID(12) Then
                        If MainUI.MachineID(14) * MainUI.MachineID(5) - MainUI.MachineID(7) = MainUI.MachineID(15) Then
                            Licensed = True
                            LicenseMessage = "License授权成功。"
                            BgwLicense.ReportProgress(100)
                            Thread.Sleep(500)
                        End If
                    End If
                Catch ex As Exception

                End Try
            End If

            If Not Licensed Then
                LicenseMessage = "License未授权。"
            End If
        Catch ex As Exception
            LicenseMessage = "License信息获取失败。"
        End Try
    End Sub

    Private Sub BgwLicense_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BgwLicense.ProgressChanged
        Try
            If Licensed Then
                TxtLicense.Text = LicenseMessage
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BgwLicense_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BgwLicense.RunWorkerCompleted
        Try
            If Licensed Then
                Dispose()
            Else
                TxtLicense.Text = LicenseMessage
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function GetRndString(lngNum As Long) As String
        GetRndString = ""
        Dim i As Long
        Dim intLength As Integer
        Const STRINGSOURCE = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        intLength = Len(STRINGSOURCE) - 1

        Randomize()
        For i = 1 To lngNum
            GetRndString &= Mid(STRINGSOURCE, Int(Rnd() * intLength + 1), 1)
        Next
    End Function

    Public Function Time() As Long
        Return (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds
    End Function

End Class