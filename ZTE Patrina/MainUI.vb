Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text

Public Class MainUI
    <Flags()>
    Public Enum MenuFlags As Integer
        MF_BYPOSITION = 1024
        MF_REMOVE = 4096
        MF_SEPARATOR = 2048
        MF_STRING = 0
    End Enum

    <DllImport("user32.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Shared Function GetSystemMenu(ByVal hWnd As IntPtr, Optional ByVal bRevert As Boolean = False) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Shared Function AppendMenu(ByVal hMenu As IntPtr, ByVal uFlags As MenuFlags, ByVal uIDNewItem As Int32, ByVal lpNewItem As String) As Boolean
    End Function

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = 274 Then
            If m.WParam.ToInt32 = &H1FFF Then
                YouhuaPasswordTool.ShowDialog()
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Public SaveBuffer As Byte() = New Byte() {}
    Public SavePath As String = ""

    Private Sub MainUI_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim VersionStrings As String() = Application.ProductVersion.ToString.Split(".")
            LblVersion.Text = "ZTE Patrina 版本: " & VersionStrings(0) & "." & VersionStrings(1) & "." & VersionStrings(2) & " (20" & VersionStrings(3).Substring(0, 2) & "/" & VersionStrings(3).Substring(2, 2) & ")"

            AppendMenu(GetSystemMenu(Handle), MenuFlags.MF_SEPARATOR, &H1FFF, "SEPARATOR")
            AppendMenu(GetSystemMenu(Handle), MenuFlags.MF_STRING, &H1FFF, "友华通信PON终端密码实用工具")
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

    Private Sub MainUI_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Try
            LoadConfig(e.Data.GetData(DataFormats.FileDrop)(0))
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

    Private Sub BtnOpen_Click(sender As Object, e As EventArgs) Handles BtnOpen.Click
        Try
            TxtMain.Clear()
            If OfdCfg.ShowDialog = DialogResult.OK Then
                LoadConfig(OfdCfg.FileName)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BtnCopy_Click(sender As Object, e As EventArgs) Handles BtnCopy.Click
        Try
            If TxtMain.TextLength > 0 Then
                Clipboard.SetText(TxtMain.Text)
            End If
        Catch ex As Exception

        End Try
    End Sub

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
                    ElseIf InputBuffer(0) = &H1 And InputBuffer(1) = &H2 And InputBuffer(2) = &H3 And InputBuffer(3) = &H4 Then
                        Dim DataBuffer As Byte() = New Byte(InputBuffer.Length - HeaderSize - 1) {}
                        Array.Copy(InputBuffer, HeaderSize, DataBuffer, 0, InputBuffer.Length - HeaderSize)
                        If InputBuffer(7) = &H0 Then
                            InputBuffer = DecryptCRC(DataBuffer)
                        ElseIf InputBuffer(7) = &H4 Then
                            InputBuffer = DecryptAES(DataBuffer, HexToBytes("be504c2b39905e230f0f39f452ee1500adff8a049a0ed491d497cf239e1704cb"), HexToBytes("789c426ecd22944b2ae4c46dc50636d3"))
                        Else
                            InputBuffer = DecryptAES(DataBuffer, HexToBytes("01f22e4aa10662fdd5fd4562b93c8a41557c5e84e9d0722d6c2babf6021fb300"), HexToBytes("b342153b91b4019e0fbb85ab5783eb1d"))
                        End If
                    ElseIf InputBuffer(0) = &H99 And InputBuffer(1) = &H99 And InputBuffer(2) = &H99 And InputBuffer(3) = &H99 And InputBuffer(4) = &H44 And InputBuffer(5) = &H44 And InputBuffer(6) = &H44 And InputBuffer(7) = &H44 And InputBuffer(8) = &H55 And InputBuffer(9) = &H55 And InputBuffer(10) = &H55 And InputBuffer(11) = &H55 And InputBuffer(12) = &HAA And InputBuffer(13) = &HAA And InputBuffer(14) = &HAA And InputBuffer(15) = &HAA Then
                        Dim HeaderOffset As Integer = 0
                        For BufferPointer = 15 To InputBuffer.Length - 12
                            If InputBuffer(BufferPointer) = &H1 And InputBuffer(BufferPointer + 1) = &H2 And InputBuffer(BufferPointer + 2) = &H3 And InputBuffer(BufferPointer + 3) = &H4 Then
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

    Private Function DecryptAES(Input As Byte(), Key As Byte(), IV As Byte()) As Byte()
        Try
            Dim HeaderSize As Integer = 12
            Dim DecryptSize As Integer = Input.Length - HeaderSize
            DecryptSize -= (DecryptSize Mod 16)
            Dim DataBuffer As Byte() = New Byte(DecryptSize - 1) {}
            Array.Copy(Input, HeaderSize, DataBuffer, 0, DecryptSize)
            Dim Decryptor As System.Security.Cryptography.Aes = System.Security.Cryptography.Aes.Create("AES")
            Decryptor.BlockSize = 128
            Decryptor.KeySize = 256
            Decryptor.Key = Key
            Decryptor.IV = IV
            Decryptor.Mode = CipherMode.CBC
            Decryptor.Padding = PaddingMode.None
            Return Decryptor.CreateDecryptor().TransformFinalBlock(DataBuffer, 0, DataBuffer.Length)
        Catch ex As Exception
            Return New Byte() {}
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

    Public Function HexToBytes(param1 As String) As Byte()
        Return Enumerable.Range(0, param1.Length).Where(Function(x) x Mod 2 = 0).[Select](Function(x) Convert.ToByte(param1.Substring(x, 2), 16)).ToArray()
    End Function

    Public Function BytesToHex(param1 As Byte()) As String
        Return BitConverter.ToString(param1).Replace("-", "").ToUpper
    End Function

    Public Function BytesToBin(param1() As Byte) As String
        Dim _loc_1 As New StringBuilder
        For Each _loc_2 In param1
            _loc_1.Append(Convert.ToString(_loc_2, 2).PadLeft(8, "0"))
        Next
        Return _loc_1.ToString
    End Function

    Public Function BinToBytes(param1 As String) As Byte()
        Dim _loc_1 As Integer = param1.Length / 8
        Dim _loc_2 As Byte() = New Byte(_loc_1 - 1) {}
        For _loc_3 As Integer = 0 To _loc_1 - 1
            _loc_2(_loc_3) = Convert.ToByte(param1.Substring(8 * _loc_3, 8), 2)
        Next
        Return _loc_2
    End Function
End Class
