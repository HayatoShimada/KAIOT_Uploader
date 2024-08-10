<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingsForm
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBoxPass = New System.Windows.Forms.TextBox()
        Me.TextBoxUser = New System.Windows.Forms.TextBox()
        Me.TextBoxDir = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxIp = New System.Windows.Forms.TextBox()
        Me.ButtonCheckConnection = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(280, 152)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 1
        Me.ButtonCancel.Text = "Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label9.Location = New System.Drawing.Point(12, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(55, 15)
        Me.Label9.TabIndex = 31
        Me.Label9.Text = "通信設定"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 123)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 15)
        Me.Label5.TabIndex = 30
        Me.Label5.Text = "パスワード"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 94)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 15)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "ユーザー名"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(66, 15)
        Me.Label3.TabIndex = 28
        Me.Label3.Text = "画像フォルダ"
        '
        'TextBoxPass
        '
        Me.TextBoxPass.Location = New System.Drawing.Point(83, 123)
        Me.TextBoxPass.Name = "TextBoxPass"
        Me.TextBoxPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxPass.Size = New System.Drawing.Size(353, 23)
        Me.TextBoxPass.TabIndex = 27
        '
        'TextBoxUser
        '
        Me.TextBoxUser.Location = New System.Drawing.Point(83, 94)
        Me.TextBoxUser.Name = "TextBoxUser"
        Me.TextBoxUser.Size = New System.Drawing.Size(353, 23)
        Me.TextBoxUser.TabIndex = 26
        '
        'TextBoxDir
        '
        Me.TextBoxDir.Location = New System.Drawing.Point(83, 65)
        Me.TextBoxDir.Name = "TextBoxDir"
        Me.TextBoxDir.Size = New System.Drawing.Size(353, 23)
        Me.TextBoxDir.TabIndex = 25
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 15)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "FTPアドレス"
        '
        'TextBoxIp
        '
        Me.TextBoxIp.Location = New System.Drawing.Point(83, 36)
        Me.TextBoxIp.Name = "TextBoxIp"
        Me.TextBoxIp.Size = New System.Drawing.Size(353, 23)
        Me.TextBoxIp.TabIndex = 23
        '
        'ButtonCheckConnection
        '
        Me.ButtonCheckConnection.Location = New System.Drawing.Point(361, 152)
        Me.ButtonCheckConnection.Name = "ButtonCheckConnection"
        Me.ButtonCheckConnection.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCheckConnection.TabIndex = 32
        Me.ButtonCheckConnection.Text = "通信確認"
        Me.ButtonCheckConnection.UseVisualStyleBackColor = True
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(454, 189)
        Me.Controls.Add(Me.ButtonCheckConnection)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBoxPass)
        Me.Controls.Add(Me.TextBoxUser)
        Me.Controls.Add(Me.TextBoxDir)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBoxIp)
        Me.Controls.Add(Me.ButtonCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "SettingsForm"
        Me.Text = "SettingsForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBoxPass As TextBox
    Friend WithEvents TextBoxUser As TextBox
    Friend WithEvents TextBoxDir As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxIp As TextBox
    Friend WithEvents ButtonCheckConnection As Button
End Class
