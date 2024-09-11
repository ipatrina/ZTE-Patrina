<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class License
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(License))
        Me.BtnLicense = New System.Windows.Forms.Button()
        Me.TxtLicense = New System.Windows.Forms.TextBox()
        Me.BgwLicense = New System.ComponentModel.BackgroundWorker()
        Me.SuspendLayout()
        '
        'BtnLicense
        '
        Me.BtnLicense.BackColor = System.Drawing.Color.SteelBlue
        Me.BtnLicense.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.BtnLicense.ForeColor = System.Drawing.Color.White
        Me.BtnLicense.Location = New System.Drawing.Point(117, 156)
        Me.BtnLicense.Name = "BtnLicense"
        Me.BtnLicense.Size = New System.Drawing.Size(110, 35)
        Me.BtnLicense.TabIndex = 201
        Me.BtnLicense.Text = "退出 (&Q)"
        Me.BtnLicense.UseVisualStyleBackColor = False
        '
        'TxtLicense
        '
        Me.TxtLicense.Font = New System.Drawing.Font("微软雅黑", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.TxtLicense.Location = New System.Drawing.Point(12, 9)
        Me.TxtLicense.MaxLength = 65535
        Me.TxtLicense.Multiline = True
        Me.TxtLicense.Name = "TxtLicense"
        Me.TxtLicense.ReadOnly = True
        Me.TxtLicense.Size = New System.Drawing.Size(320, 140)
        Me.TxtLicense.TabIndex = 101
        Me.TxtLicense.Text = "正在请求License..."
        '
        'BgwLicense
        '
        Me.BgwLicense.WorkerReportsProgress = True
        '
        'License
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.DeepPink
        Me.ClientSize = New System.Drawing.Size(344, 201)
        Me.Controls.Add(Me.BtnLicense)
        Me.Controls.Add(Me.TxtLicense)
        Me.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "License"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "License"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnLicense As Button
    Friend WithEvents TxtLicense As TextBox
    Friend WithEvents BgwLicense As System.ComponentModel.BackgroundWorker
End Class
