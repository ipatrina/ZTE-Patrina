Imports System.IO
Imports System.IO.Compression
Imports System.Management
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text

Public Class MainUI

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Shared Function AppendMenu(hMenu As IntPtr, uFlags As MenuFlags, uIDNewItem As Int32, lpNewItem As String) As Boolean
    End Function

    <DllImport("user32.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function GetSystemMenu(hWnd As IntPtr, Optional bRevert As Boolean = False) As IntPtr
    End Function

    <Flags()>
    Public Enum MenuFlags As Integer
        MF_BYPOSITION = 1024
        MF_REMOVE = 4096
        MF_SEPARATOR = 2048
        MF_STRING = 0
    End Enum

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = 274 Then
            If m.WParam.ToInt32 = &H1FFE Then
                YouhuaPasswordTool.ShowDialog()
            ElseIf m.WParam.ToInt32 = &H1FFF Then
                MsgBox("ZTE Patrina" & vbCrLf & vbCrLf & "中兴ONT配置文件实用工具" & vbCrLf & vbCrLf & "软件版本：" & VersionStrings(0) & "." & VersionStrings(1) & "." & VersionStrings(2) & vbCrLf & "更新时间：20" & VersionStrings(3).Substring(0, 2) & "年" & Int(VersionStrings(3).Substring(2, 2)) & "月" & vbCrLf & vbCrLf & "Copyright © 2020-2024 版权所有", vbInformation, "关于")
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Public Shared IndivKey As String = ""
    Public Shared MachineID As Byte() = New Byte() {}
    Public Shared SaveBuffer As Byte() = New Byte() {}
    Public Shared SavePath As String = ""
    Public Shared VersionStrings As String() = Application.ProductVersion.ToString.Split(".")

    Public Function BinToBytes(param1 As String) As Byte()
        Dim _loc_1 As Integer = param1.Length / 8
        Dim _loc_2 As Byte() = New Byte(_loc_1 - 1) {}
        For _loc_3 As Integer = 0 To _loc_1 - 1
            _loc_2(_loc_3) = Convert.ToByte(param1.Substring(8 * _loc_3, 8), 2)
        Next
        Return _loc_2
    End Function

    Private Sub BtnCopy_Click(sender As Object, e As EventArgs) Handles BtnCopy.Click
        Try
            If TxtMain.TextLength > 0 Then
                Clipboard.SetText(TxtMain.Text)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BtnOpen_Click(sender As Object, e As EventArgs) Handles BtnOpen.Click
        Try
            TxtMain.Clear()
            If OfdCfg.ShowDialog = DialogResult.OK Then
                LoadConfig(OfdCfg.FileName)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function BytesToBin(param1() As Byte) As String
        Dim _loc_1 As New StringBuilder
        For Each _loc_2 In param1
            _loc_1.Append(Convert.ToString(_loc_2, 2).PadLeft(8, "0"))
        Next
        Return _loc_1.ToString
    End Function

    Public Function BytesToHex(param1 As Byte()) As String
        Return BitConverter.ToString(param1).Replace("-", "").ToUpper
    End Function

    Private Sub BtnIndiv_Click(sender As Object, e As EventArgs) Handles BtnIndiv.Click
        Try
            Indiv.ShowDialog()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Try
            If SavePath.Length > 0 And SaveBuffer.Length > 0 Then
                My.Computer.FileSystem.WriteAllBytes(SavePath, SaveBuffer, False)
                BtnSave.Enabled = False
                TxtMain.Text = "[ 提示 ] 配置文件已保存！" & vbCrLf & SavePath
                SaveBuffer = New Byte() {}
                SavePath = ""
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function DecryptAES(Input As Byte(), Key As Byte(), IV As Byte()) As Byte()
        Try
            Dim HeaderSize As Integer = 12
            Dim DecryptSize As Integer = Input.Length - HeaderSize
            DecryptSize -= DecryptSize Mod 16
            Dim DataBuffer As Byte() = New Byte(DecryptSize - 1) {}
            Array.Copy(Input, HeaderSize, DataBuffer, 0, DecryptSize)
            Dim Decryptor As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create("AES")
            Decryptor.BlockSize = 128
            Decryptor.KeySize = 256
            Decryptor.Key = Key
            Decryptor.IV = IV
            Decryptor.Mode = 1
            Decryptor.Padding = PaddingMode.None
            Return Decryptor.CreateDecryptor().TransformFinalBlock(DataBuffer, 0, DataBuffer.Length)
        Catch ex As Exception
            Return New Byte() {}
        End Try
    End Function

    Private Function DecryptCRC(Input As Byte()) As Byte()
        Dim FileStream As Byte() = New Byte() {}
        Try
            Dim Pointer As Integer = 0
            Dim HeaderSize As Integer = 12
            Do
                If Pointer < 0 Then Exit Do
                Dim DataSize As Integer = Int(Input(Pointer + 3)) + Int(Input(Pointer + 2)) * 256 + Int(Input(Pointer + 1)) * 65536 + Int(Input(Pointer + 0)) * 16777216
                Dim BufferSize As Integer = Int(Input(Pointer + 7)) + Int(Input(Pointer + 6)) * 256 + Int(Input(Pointer + 5)) * 65536 + Int(Input(Pointer + 4)) * 16777216
                Dim Offset As Integer = Int(Input(Pointer + 11)) + Int(Input(Pointer + 10)) * 256 + Int(Input(Pointer + 9)) * 65536 + Int(Input(Pointer + 8)) * 16777216
                Dim DeflateFlagSize As Integer = 2
                Dim DataBuffer As Byte() = New Byte(BufferSize - DeflateFlagSize - 1) {}
                Array.Copy(Input, Pointer + HeaderSize + DeflateFlagSize, DataBuffer, 0, BufferSize - DeflateFlagSize)
                Dim DataBufferStream As New MemoryStream(DataBuffer)
                Dim DecompressStream As New DeflateStream(DataBufferStream, CompressionMode.Decompress)
                Dim UncompressedStream As New MemoryStream()
                DecompressStream.CopyTo(UncompressedStream)
                Dim StreamBuffer As Byte() = UncompressedStream.GetBuffer()
                DataBufferStream.Close()
                DecompressStream.Close()
                UncompressedStream.Close()
                Dim FileBuffer As Byte() = New Byte(FileStream.Length + StreamBuffer.Length - 1) {}
                Array.Copy(FileStream, 0, FileBuffer, 0, FileStream.Length)
                Array.Copy(StreamBuffer, 0, FileBuffer, FileStream.Length, StreamBuffer.Length)
                FileStream = FileBuffer
                Pointer = Offset - 60
            Loop
            Return FileStream
        Catch ex As Exception
            Return FileStream
        End Try
    End Function

    Private Function DecryptYouhua(Input As Byte()) As Byte()
        Try
            Dim DataBuffer As Byte() = New Byte(Input.Length - 1) {}
            For _loc_1 = 0 To Input.Length - 1
                If Int(Input(_loc_1)) = &H0 Then
                    DataBuffer(_loc_1) = &HFF
                Else
                    DataBuffer(_loc_1) = Int(Input(_loc_1)) - 1
                End If
            Next
            Return DataBuffer
        Catch ex As Exception
            Return New Byte() {}
        End Try
    End Function

    Public Function GetAESCBCEncryIV(Password As String, PasswordLength As Integer) As Byte()
        Try
            Dim _loc_1 As Byte() = New SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(Password.Substring(0, If(Password.Length > PasswordLength, PasswordLength, Password.Length))))
            Dim _loc_2 As Byte() = New Byte(15) {}
            Array.Copy(_loc_1, _loc_2, 16)
            Return _loc_2
        Catch ex As Exception
            Return New Byte() {}
        End Try
    End Function

    Public Function GetAESCBCEncryKey(Password As String, PasswordLength As Integer) As Byte()
        Try
            Return New SHA256CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(Password.Substring(0, If(Password.Length > PasswordLength, PasswordLength, Password.Length))))
        Catch ex As Exception
            Return New Byte() {}
        End Try
    End Function

    Public Function GetMachineID() As String
        Try
            Dim _loc_1 As New StringBuilder()
            For Each _loc_2 As ManagementObject In New ManagementObjectSearcher("SELECT * FROM Win32_BIOS").Get()
                Dim _loc_3 As String = Convert.ToString(_loc_2("Manufacturer"))
                Dim _loc_4 As String = Convert.ToString(_loc_2("SerialNumber"))
                If _loc_3.Length > 1 And _loc_4.Length > 1 Then
                    _loc_1.Append(_loc_3)
                    _loc_1.Append(vbLf)
                    _loc_1.Append(_loc_4)
                    Return _loc_1.ToString()
                Else
                    Return ""
                End If
            Next
            Return ""
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Sub LoadConfig(Input As String)
        Try
            If My.Computer.FileSystem.FileExists(Input) Then
                TxtMain.Clear()
                BtnSave.Enabled = True
                Dim InputBuffer As Byte() = My.Computer.FileSystem.ReadAllBytes(Input)
                Dim HeaderSize As Integer = 60
                Do
                    If InputBuffer.Length <= HeaderSize Then Exit Do
                    If InputBuffer(0) = &H3C Then
                        Exit Do
                    ElseIf InputBuffer(0) = 1 And InputBuffer(1) = 2 And InputBuffer(2) = 3 And InputBuffer(3) = 4 Then
                        Dim DataBuffer As Byte() = New Byte(InputBuffer.Length - HeaderSize - 1) {}
                        Array.Copy(InputBuffer, HeaderSize, DataBuffer, 0, InputBuffer.Length - HeaderSize)
                        If InputBuffer(7) = &H0 Then
                            InputBuffer = DecryptCRC(DataBuffer)
                        ElseIf InputBuffer(7) = 4 Then
                            Dim DecryptAESKey As String = "8cc72b05705d5c46f412af8cbed55aad"
                            Dim DecryptAESIV As String = "667b02a85c61c786def4521b060265e8"
                            If IndivKey.Length > 0 Then
                                Dim _loc_1 As Byte() = New Byte(32) {}
                                Dim _loc_2 As Byte() = Encoding.UTF8.GetBytes(IndivKey)
                                Array.Copy(_loc_2, 0, _loc_1, 0, IndivKey.Length)
                                DecryptAESKey = BytesToHex(New System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(_loc_1)).ToLower()
                            End If
                            InputBuffer = DecryptAES(DataBuffer, GetAESCBCEncryKey(DecryptAESKey, 31), GetAESCBCEncryIV(DecryptAESIV, 31))
                        Else
                            Dim DecryptAESKey As String = "PON_Dkey"
                            Dim DecryptAESIV As String = "PON_DIV"
                            If IndivKey.Length > 64 Then
                                Dim _loc_11 As Byte() = Encoding.UTF8.GetBytes(IndivKey.Substring(0, 64))
                                Dim _loc_13 As Byte() = New Byte(35) {}
                                Dim _loc_14 As Byte() = New Byte(35) {}
                                Dim _loc_16 As Integer = 0
                                Dim _loc_17 As Integer = 0
                                For _loc_15 As Integer = 0 To 63
                                    If _loc_15 - 5 <= 15 And _loc_15 - 5 >= 0 Then
                                        _loc_13(_loc_17) = _loc_11(_loc_15) + 3
                                        _loc_17 += 1
                                    End If
                                    If _loc_15 - 7 <= 31 And _loc_15 - 7 >= 0 Then
                                        _loc_14(_loc_16) = _loc_11(_loc_15) + 1
                                        _loc_16 += 1
                                    End If
                                Next
                                DecryptAESKey = Encoding.UTF8.GetString(_loc_13).TrimEnd(Chr(0)) & IndivKey.Substring(64)
                                DecryptAESIV = Encoding.UTF8.GetString(_loc_14)
                            End If
                            InputBuffer = DecryptAES(DataBuffer, GetAESCBCEncryKey(DecryptAESKey, 256), GetAESCBCEncryIV(DecryptAESIV, 256))
                            If IndivKey.Length > 64 Then Exit Do
                        End If
                    ElseIf InputBuffer(0) = &H99 And InputBuffer(1) = &H99 And InputBuffer(2) = &H99 And InputBuffer(3) = &H99 And InputBuffer(4) = &H44 And InputBuffer(5) = &H44 And InputBuffer(6) = &H44 And InputBuffer(7) = &H44 And InputBuffer(8) = &H55 And InputBuffer(9) = &H55 And InputBuffer(10) = &H55 And InputBuffer(11) = &H55 And InputBuffer(12) = &HAA And InputBuffer(13) = &HAA And InputBuffer(14) = &HAA And InputBuffer(15) = &HAA Then
                        Dim HeaderOffset As Integer = 0
                        For BufferPointer = 15 To InputBuffer.Length - 12
                            If InputBuffer(BufferPointer) = 1 And InputBuffer(BufferPointer + 1) = 2 And InputBuffer(BufferPointer + 2) = 3 And InputBuffer(BufferPointer + 3) = 4 Then
                                HeaderOffset = BufferPointer
                                Exit For
                            End If
                        Next
                        If HeaderOffset = 0 Then
                            InputBuffer = New Byte() {}
                        Else
                            Dim UntaggedBuffer As Byte() = New Byte(InputBuffer.Length - HeaderOffset - 1) {}
                            Array.Copy(InputBuffer, HeaderOffset, UntaggedBuffer, 0, InputBuffer.Length - HeaderOffset)
                            InputBuffer = UntaggedBuffer
                        End If
                    ElseIf InputBuffer(0) = &H3D Then
                        InputBuffer = DecryptYouhua(InputBuffer)
                        Exit Do
                    Else
                        InputBuffer = New Byte() {}
                    End If
                Loop
                If InputBuffer.Length > 60 Then
                    SaveBuffer = InputBuffer
                    SavePath = Path.GetDirectoryName(Input) & "\de_" & Path.GetFileName(Input)
                    TxtMain.Text = Encoding.UTF8.GetString(SaveBuffer).Replace(Chr(10), vbCrLf)
                Else
                    TxtMain.Text = "[ 提示 ] 配置文件打开失败！"
                End If
            End If
        Catch ex As Exception
            TxtMain.Text = "[ 提示 ] 配置文件加载失败！"
        End Try
    End Sub

    Private Sub MainUI_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Try
            LoadConfig(e.Data.GetData(DataFormats.FileDrop)(0))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub MainUI_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        Try
            If e.Data.GetDataPresent(DataFormats.FileDrop) = True Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.None
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub MainUI_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            LblVersion.Text = "ZTE Patrina 版本: " & VersionStrings(0) & "." & VersionStrings(1) & "." & VersionStrings(2) & " (20" & VersionStrings(3).Substring(0, 2) & "." & VersionStrings(3).Substring(2, 2) & ")"

            If LblVersion.Font.Size > 12 Then
                MsgBox("DPI合规性检测不通过！" & vbCrLf & "请更换计算机或调整显示字体为标准大小后运行本软件。", vbCritical, "ZTE Patrina - 系统错误")
                End
            End If

            AppendMenu(GetSystemMenu(Handle), MenuFlags.MF_SEPARATOR, &H1FFE, "SEPARATOR")
            'AppendMenu(GetSystemMenu(Handle), MenuFlags.MF_STRING, &H1FFE, "友华通信PON终端密码实用工具")
            AppendMenu(GetSystemMenu(Handle), MenuFlags.MF_STRING, &H1FFF, "关于(&A)")
        Catch ex As Exception

        End Try
    End Sub

    Public Function HexToBytes(param1 As String) As Byte()
        Return Enumerable.Range(0, param1.Length).Where(Function(x) x Mod 2 = 0).[Select](Function(x) Convert.ToByte(param1.Substring(x, 2), 16)).ToArray()
    End Function

End Class
