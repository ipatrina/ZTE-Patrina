<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Indiv
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Indiv))
        Me.BtnIndiv = New System.Windows.Forms.Button()
        Me.TxtIndiv = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'BtnIndiv
        '
        Me.BtnIndiv.BackColor = System.Drawing.Color.SteelBlue
        Me.BtnIndiv.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.BtnIndiv.ForeColor = System.Drawing.Color.White
        Me.BtnIndiv.Location = New System.Drawing.Point(117, 156)
        Me.BtnIndiv.Name = "BtnIndiv"
        Me.BtnIndiv.Size = New System.Drawing.Size(110, 35)
        Me.BtnIndiv.TabIndex = 201
        Me.BtnIndiv.Text = "确定 (&K)"
        Me.BtnIndiv.UseVisualStyleBackColor = False
        '
        'TxtIndiv
        '
        Me.TxtIndiv.Font = New System.Drawing.Font("微软雅黑", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.TxtIndiv.Location = New System.Drawing.Point(12, 9)
        Me.TxtIndiv.MaxLength = 65535
        Me.TxtIndiv.Multiline = True
        Me.TxtIndiv.Name = "TxtIndiv"
        Me.TxtIndiv.Size = New System.Drawing.Size(320, 140)
        Me.TxtIndiv.TabIndex = 101
        '
        'Indiv
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.DeepPink
        Me.ClientSize = New System.Drawing.Size(344, 201)
        Me.Controls.Add(Me.BtnIndiv)
        Me.Controls.Add(Me.TxtIndiv)
        Me.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Indiv"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Indiv密钥 (ASCII)"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnIndiv As Button
    Friend WithEvents TxtIndiv As TextBox
End Class
