Imports System.Net

Public Class Form1

    Public imageFiles As String()

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
                .Multiselect = True ' 複数選択を許可
            End With
            If ofd_IO.ShowDialog() = DialogResult.OK Then
                imageFiles = ofd_IO.FileNames
                TextBox1.Text = String.Join(", ", imageFiles)
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

        Dim ftpUserName As String = TextBox4.Text
        Dim ftpPassword As String = TextBox5.Text

        Try
            ' 画像ファイルごとに処理を実行
            For i As Integer = 0 To imageFiles.Length - 1
                Dim originalFileName As String = imageFiles(i)
                Dim newFileName As String

                ' ファイル名に番号が既に割り振られているか確認
                Dim fileNameWithoutExtension As String = IO.Path.GetFileNameWithoutExtension(originalFileName)
                If IsNumeric(fileNameWithoutExtension) Then
                    newFileName = fileNameWithoutExtension & ".jpg"
                Else
                    newFileName = i.ToString() & ".jpg" ' 番号を割り振り
                End If

                ' アップロード先のパスを作成
                Dim ftpFullPath As String = $"{ftpServer}/{targetFolder}/{filePath1}/{filePath2}/{newFileName}"

                ' 既存のファイルがあるかチェック
                If Await FileExistsOnFtp(ftpFullPath, ftpUserName, ftpPassword) Then
                    Dim result = MessageBox.Show($"ファイル {newFileName} は既に存在します。上書きしますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)
                    If result = DialogResult.Cancel Then
                        Exit For
                    ElseIf result = DialogResult.No Then
                        Continue For ' 次のファイルへ
                    End If
                    ' Yesの場合は上書き処理を続ける
                End If

                ' FTPサーバーにファイルをアップロード
                Await UploadFileToFtp(ftpFullPath, originalFileName, ftpUserName, ftpPassword)
            Next

            MessageBox.Show("ファイルのアップロードが完了しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' FTPサーバーに指定したファイルが存在するか確認する関数
    Private Async Function FileExistsOnFtp(ftpFullPath As String, ftpUserName As String, ftpPassword As String) As Task(Of Boolean)
        Try
            Dim request = CType(WebRequest.Create(ftpFullPath), FtpWebRequest)
            request.Credentials = New NetworkCredential(ftpUserName, ftpPassword)
            request.Method = WebRequestMethods.Ftp.GetFileSize

            Using response = CType(Await request.GetResponseAsync(), FtpWebResponse)
                Return response.StatusCode = FtpStatusCode.FileStatus
            End Using
        Catch ex As WebException
            If CType(ex.Response, FtpWebResponse).StatusCode = FtpStatusCode.ActionNotTakenFileUnavailable Then
                Return False
            Else
                Throw
            End If
        End Try
    End Function

    ' FTPサーバーにファイルをアップロードする関数
    Private Async Function UploadFileToFtp(ftpFullPath As String, localFilePath As String, ftpUserName As String, ftpPassword As String) As Task
        Dim request = CType(WebRequest.Create(ftpFullPath), FtpWebRequest)
        request.Credentials = New NetworkCredential(ftpUserName, ftpPassword)
        request.Method = WebRequestMethods.Ftp.UploadFile

        ' ファイルの内容を読み込んでアップロード
        Using fileStream As New IO.FileStream(localFilePath, IO.FileMode.Open, IO.FileAccess.Read)
            Using requestStream = Await request.GetRequestStreamAsync()
                Await fileStream.CopyToAsync(requestStream)
            End Using
        End Using

        ' アップロード結果を確認
        Using response = CType(Await request.GetResponseAsync(), FtpWebResponse)
            If response.StatusCode <> FtpStatusCode.ClosingData Then
                Throw New Exception($"FTPアップロードに失敗しました: {response.StatusDescription}")
            End If
        End Using
    End Function

End Class
