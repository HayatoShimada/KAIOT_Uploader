Imports System.Net
Imports System.Net.Http

Public Class Form1

    Public sImageName As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.Text = My.Settings.pFTPadd
        TextBox3.Text = My.Settings.pFTPfolder
        TextBox4.Text = My.Settings.pFTPuser
        TextBox5.Text = My.Settings.pFTPpass
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using ofd_IO As New OpenFileDialog
            With ofd_IO
                .Title = "画像選択"
                .Filter = "jpgファイル|*.jpg"
            End With
            If ofd_IO.ShowDialog() = DialogResult.OK Then
                sImageName = ofd_IO.FileName
                TextBox1.Text = sImageName
            End If
        End Using
    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim ftpServer As String = TextBox2.Text
        Dim targetFolder As String = TextBox3.Text
        Dim filePath1 As String = NumericUpDown1.Value.ToString()
        Dim filePath2 As String = "0"

        ' ComboBox1 の選択に応じて filePath2 の値を設定
        Select Case ComboBox1.Text
            Case "型替"
                filePath2 = "0"
            Case "造型"
                filePath2 = "1"
            Case "中子"
                filePath2 = "2"
            Case "注湯"
                filePath2 = "3"
            Case "解枠"
                filePath2 = "4"
        End Select

        Dim fileName As String = TextBox1.Text
        Dim ftpFullPath As String = $"{ftpServer}/{targetFolder}/{filePath1}/{filePath2}/{IO.Path.GetFileName(fileName)}"

        Dim ftpUserName As String = TextBox4.Text
        Dim ftpPassword As String = TextBox5.Text

        Try
            ' HttpClientの初期化
            Using client As New HttpClient()

                ' 認証情報の設定
                Dim byteArray As Byte() = System.Text.Encoding.ASCII.GetBytes($"{ftpUserName}:{ftpPassword}")
                client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray))

                ' ファイルの内容を読み込み
                Dim fileContent As ByteArrayContent
                Using fileStream As New IO.FileStream(fileName, IO.FileMode.Open, IO.FileAccess.Read)
                    fileContent = New ByteArrayContent(New IO.BinaryReader(fileStream).ReadBytes(CInt(fileStream.Length)))
                End Using

                ' FTPサーバーにファイルをアップロード
                Dim response As HttpResponseMessage = Await client.PutAsync(ftpFullPath, fileContent)
                response.EnsureSuccessStatusCode()

                MessageBox.Show("ファイルのアップロードが完了しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
