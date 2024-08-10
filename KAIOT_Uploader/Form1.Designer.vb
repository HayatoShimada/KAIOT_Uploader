<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(Form1))
        Label1 = New Label()
        TextBox1 = New TextBox()
        Button1 = New Button()
        TextBox2 = New TextBox()
        Label2 = New Label()
        TextBox3 = New TextBox()
        TextBox4 = New TextBox()
        TextBox5 = New TextBox()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Button2 = New Button()
        Label6 = New Label()
        Label7 = New Label()
        ComboBox1 = New ComboBox()
        NumericUpDown1 = New NumericUpDown()
        Button3 = New Button()
        ListBox1 = New ListBox()
        PictureBox1 = New PictureBox()
        Label9 = New Label()
        RichTextBox1 = New RichTextBox()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(16, 307)
        Label1.Name = "Label1"
        Label1.Size = New Size(65, 15)
        Label1.TabIndex = 0
        Label1.Text = "画像ファイル"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(87, 307)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(386, 23)
        TextBox1.TabIndex = 1
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(479, 307)
        Button1.Name = "Button1"
        Button1.Size = New Size(101, 23)
        Button1.TabIndex = 2
        Button1.Text = "参照"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(87, 397)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(386, 23)
        TextBox2.TabIndex = 3
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(16, 397)
        Label2.Name = "Label2"
        Label2.Size = New Size(61, 15)
        Label2.TabIndex = 4
        Label2.Text = "FTPアドレス"
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(87, 426)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(386, 23)
        TextBox3.TabIndex = 5
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(87, 455)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(386, 23)
        TextBox4.TabIndex = 6
        ' 
        ' TextBox5
        ' 
        TextBox5.Location = New Point(87, 484)
        TextBox5.Name = "TextBox5"
        TextBox5.PasswordChar = "*"c
        TextBox5.Size = New Size(386, 23)
        TextBox5.TabIndex = 7
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(16, 426)
        Label3.Name = "Label3"
        Label3.Size = New Size(66, 15)
        Label3.TabIndex = 8
        Label3.Text = "画像フォルダ"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(16, 455)
        Label4.Name = "Label4"
        Label4.Size = New Size(55, 15)
        Label4.TabIndex = 9
        Label4.Text = "ユーザー名"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(16, 484)
        Label5.Name = "Label5"
        Label5.Size = New Size(51, 15)
        Label5.TabIndex = 10
        Label5.Text = "パスワード"
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(480, 336)
        Button2.Name = "Button2"
        Button2.Size = New Size(101, 23)
        Button2.TabIndex = 11
        Button2.Text = "アップロード"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(16, 12)
        Label6.Name = "Label6"
        Label6.Size = New Size(54, 15)
        Label6.TabIndex = 12
        Label6.Text = "パターンID"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(16, 41)
        Label7.Name = "Label7"
        Label7.Size = New Size(55, 15)
        Label7.TabIndex = 14
        Label7.Text = "作業位置"
        ' 
        ' ComboBox1
        ' 
        ComboBox1.FormattingEnabled = True
        ComboBox1.Items.AddRange(New Object() {"型替", "造型", "中子", "注湯", "解枠"})
        ComboBox1.Location = New Point(87, 41)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(121, 23)
        ComboBox1.TabIndex = 15
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Location = New Point(87, 12)
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Size(120, 23)
        NumericUpDown1.TabIndex = 16
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(225, 12)
        Button3.Name = "Button3"
        Button3.Size = New Size(248, 23)
        Button3.TabIndex = 18
        Button3.Text = "画像リスト検索"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' ListBox1
        ' 
        ListBox1.FormattingEnabled = True
        ListBox1.ItemHeight = 15
        ListBox1.Location = New Point(16, 72)
        ListBox1.Name = "ListBox1"
        ListBox1.Size = New Size(457, 229)
        ListBox1.TabIndex = 19
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        PictureBox1.Location = New Point(480, 72)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(424, 229)
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.TabIndex = 20
        PictureBox1.TabStop = False
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(16, 370)
        Label9.Name = "Label9"
        Label9.Size = New Size(55, 15)
        Label9.TabIndex = 22
        Label9.Text = "通信設定"
        ' 
        ' RichTextBox1
        ' 
        RichTextBox1.Location = New Point(481, 370)
        RichTextBox1.Name = "RichTextBox1"
        RichTextBox1.Size = New Size(419, 137)
        RichTextBox1.TabIndex = 23
        RichTextBox1.Text = ""
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(912, 528)
        Controls.Add(RichTextBox1)
        Controls.Add(Label9)
        Controls.Add(PictureBox1)
        Controls.Add(ListBox1)
        Controls.Add(Button3)
        Controls.Add(NumericUpDown1)
        Controls.Add(ComboBox1)
        Controls.Add(Label7)
        Controls.Add(Label6)
        Controls.Add(Button2)
        Controls.Add(Label5)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(TextBox5)
        Controls.Add(TextBox4)
        Controls.Add(TextBox3)
        Controls.Add(Label2)
        Controls.Add(TextBox2)
        Controls.Add(Button1)
        Controls.Add(TextBox1)
        Controls.Add(Label1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "Form1"
        Text = "KAIOT Uploader"
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents Button3 As Button
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label9 As Label
    Friend WithEvents RichTextBox1 As RichTextBox
End Class
