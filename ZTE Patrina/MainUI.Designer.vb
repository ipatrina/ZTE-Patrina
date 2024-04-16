<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainUI
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainUI))
        Me.TxtMain = New System.Windows.Forms.TextBox()
        Me.BtnOpen = New System.Windows.Forms.Button()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.BtnCopy = New System.Windows.Forms.Button()
        Me.LblVersion = New System.Windows.Forms.Label()
        Me.OfdCfg = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'TxtMain
        '
        Me.TxtMain.Font = New System.Drawing.Font("微软雅黑", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.TxtMain.Location = New System.Drawing.Point(12, 46)
        Me.TxtMain.MaxLength = 2147483647
        Me.TxtMain.Multiline = True
        Me.TxtMain.Name = "TxtMain"
        Me.TxtMain.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TxtMain.Size = New System.Drawing.Size(760, 503)
        Me.TxtMain.TabIndex = 901
        '
        'BtnOpen
        '
        Me.BtnOpen.BackColor = System.Drawing.Color.Orchid
        Me.BtnOpen.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.BtnOpen.ForeColor = System.Drawing.Color.White
        Me.BtnOpen.Location = New System.Drawing.Point(128, 8)
        Me.BtnOpen.Name = "BtnOpen"
        Me.BtnOpen.Size = New System.Drawing.Size(110, 32)
        Me.BtnOpen.TabIndex = 111
        Me.BtnOpen.Text = "打开 (&O)"
        Me.BtnOpen.UseVisualStyleBackColor = False
        '
        'BtnSave
        '
        Me.BtnSave.BackColor = System.Drawing.Color.MediumVioletRed
        Me.BtnSave.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.BtnSave.ForeColor = System.Drawing.Color.White
        Me.BtnSave.Location = New System.Drawing.Point(12, 8)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(110, 32)
        Me.BtnSave.TabIndex = 101
        Me.BtnSave.Text = "保存 (&S)"
        Me.BtnSave.UseVisualStyleBackColor = False
        '
        'BtnCopy
        '
        Me.BtnCopy.BackColor = System.Drawing.Color.DeepPink
        Me.BtnCopy.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.BtnCopy.ForeColor = System.Drawing.Color.White
        Me.BtnCopy.Location = New System.Drawing.Point(662, 8)
        Me.BtnCopy.Name = "BtnCopy"
        Me.BtnCopy.Size = New System.Drawing.Size(110, 32)
        Me.BtnCopy.TabIndex = 501
        Me.BtnCopy.Text = "复制 (&C)"
        Me.BtnCopy.UseVisualStyleBackColor = False
        '
        'LblVersion
        '
        Me.LblVersion.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.LblVersion.ForeColor = System.Drawing.Color.Ivory
        Me.LblVersion.Location = New System.Drawing.Point(244, 9)
        Me.LblVersion.Name = "LblVersion"
        Me.LblVersion.Size = New System.Drawing.Size(412, 31)
        Me.LblVersion.TabIndex = 301
        Me.LblVersion.Text = "ZTE Patrina 版本: 1.0.0 (1970/01)"
        Me.LblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'OfdCfg
        '
        Me.OfdCfg.Filter = "所有文件|*.*"
        '
        'MainUI
        '
        Me.AllowDrop = True
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.HotPink
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.LblVersion)
        Me.Controls.Add(Me.BtnCopy)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.BtnOpen)
        Me.Controls.Add(Me.TxtMain)
        Me.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(5)
        Me.MaximizeBox = False
        Me.Name = "MainUI"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ZTE Patrina"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtMain As TextBox
    Friend WithEvents BtnOpen As Button
    Friend WithEvents BtnSave As Button
    Friend WithEvents BtnCopy As Button
    Friend WithEvents LblVersion As Label
    Friend WithEvents OfdCfg As OpenFileDialog
End Class
