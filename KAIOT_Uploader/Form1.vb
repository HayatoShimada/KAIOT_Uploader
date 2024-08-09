Imports System.Diagnostics
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
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
            RichTextBox1.Clear() ' 結果表示用のリッチテキストボックスをクリア

            ' 画像ファイルごとにFTPアップロードを実行
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
                Dim remoteFilePath As String = $"{targetFolder}/{filePath1}/{filePath2}/{newFileName}"

                ' FTPスクリプトを作成
                Dim ftpCommands As String = String.Join(Environment.NewLine, {
                    $"open {ftpServer}",
                    ftpUserName,
                    ftpPassword,
                    $"put ""{originalFileName}"" ""{remoteFilePath}""",
                    "bye"
                })

                ' 一時ファイルにFTPスクリプトを保存
                Dim tempScriptPath As String = IO.Path.Combine(IO.Path.GetTempPath(), "ftpScript.txt")
                IO.File.WriteAllText(tempScriptPath, ftpCommands)

                ' コマンドプロンプトでftpコマンドを実行
                Dim startInfo As New ProcessStartInfo("cmd.exe") With {
                    .RedirectStandardInput = True,
                    .RedirectStandardOutput = True,
                    .RedirectStandardError = True,
                    .UseShellExecute = False,
                    .CreateNoWindow = True
                }

                Dim process As Process = Process.Start(startInfo)
                Using writer As IO.StreamWriter = process.StandardInput
                    If writer.BaseStream.CanWrite Then
                        writer.WriteLine($"ftp -s:""{tempScriptPath}""")
                    End If
                End Using

                process.WaitForExit()

                ' 結果を取得してリッチテキストボックスに表示
                Dim output As String = process.StandardOutput.ReadToEnd()
                RichTextBox1.AppendText(output & Environment.NewLine)

                ' 一時ファイルを削除
                IO.File.Delete(tempScriptPath)
            Next

            MessageBox.Show("ファイルのアップロードが完了しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' FTPサーバーの情報
        Dim ftpServer As String = TextBox2.Text
        Dim ftpUserName As String = TextBox4.Text
        Dim ftpPassword As String = TextBox5.Text
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

        Try
            ' FTPコマンドを生成
            Dim ftpCommands As String = String.Join(Environment.NewLine, {
                $"open {ftpServer}",
                ftpUserName,
                ftpPassword,
                $"cd {targetFolder}/{filePath1}/{filePath2}",
                "ls *.jpg",  ' .jpgファイルのみをリスト
                "bye"
            })

            ' 一時ファイルにFTPスクリプトを保存
            Dim tempScriptPath As String = IO.Path.Combine(IO.Path.GetTempPath(), "ftpListScript.txt")
            IO.File.WriteAllText(tempScriptPath, ftpCommands)

            ' コマンドプロンプトでftpコマンドを実行
            Dim startInfo As New ProcessStartInfo("cmd.exe") With {
                .RedirectStandardInput = True,
                .RedirectStandardOutput = True,
                .RedirectStandardError = True,
                .UseShellExecute = False,
                .CreateNoWindow = True
            }

            Dim process As Process = Process.Start(startInfo)
            Using writer As IO.StreamWriter = process.StandardInput
                If writer.BaseStream.CanWrite Then
                    writer.WriteLine($"ftp -s:""{tempScriptPath}""")
                End If
            End Using

            process.WaitForExit()

            ' 結果を取得
            Dim output As String = process.StandardOutput.ReadToEnd()

            ' リストボックスをクリアしてから結果を追加
            ListBox1.Items.Clear()
            For Each line As String In output.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                If line.EndsWith(".jpg") Then
                    ListBox1.Items.Add(line)
                End If
            Next

            ' 一時ファイルを削除
            IO.File.Delete(tempScriptPath)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        ' 選択された画像ファイルを取得
        Dim selectedFile As String = ListBox1.SelectedItem.ToString()

        ' FTPサーバーの情報
        Dim ftpServer As String = TextBox2.Text
        Dim ftpUserName As String = TextBox4.Text
        Dim ftpPassword As String = TextBox5.Text
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


        ' ローカルにダウンロードするパスを設定
        Dim localTempFile As String = IO.Path.Combine(IO.Path.GetTempPath(), selectedFile)

        Try
            ' FTPスクリプトを生成
            Dim ftpCommands As String = String.Join(Environment.NewLine, {
                $"open {ftpServer}",
                ftpUserName,
                ftpPassword,
                $"cd {targetFolder}/{filePath1}/{filePath2}",
                $"get {selectedFile} {localTempFile}", ' ファイルをダウンロード
                "bye"
            })

            ' 一時ファイルにFTPスクリプトを保存
            Dim tempScriptPath As String = IO.Path.Combine(IO.Path.GetTempPath(), "ftpDownloadScript.txt")
            IO.File.WriteAllText(tempScriptPath, ftpCommands)

            ' コマンドプロンプトでftpコマンドを実行
            Dim startInfo As New ProcessStartInfo("cmd.exe") With {
                .RedirectStandardInput = True,
                .RedirectStandardOutput = True,
                .RedirectStandardError = True,
                .UseShellExecute = False,
                .CreateNoWindow = True
            }

            Dim process As Process = Process.Start(startInfo)
            Using writer As IO.StreamWriter = process.StandardInput
                If writer.BaseStream.CanWrite Then
                    writer.WriteLine($"ftp -s:""{tempScriptPath}""")
                End If
            End Using

            process.WaitForExit()

            ' ダウンロードした画像をPictureBoxに表示
            PictureBox1.Image = Image.FromFile(localTempFile)

            ' 一時ファイルを削除
            IO.File.Delete(tempScriptPath)
        Catch ex As Exception
            MessageBox.Show("画像のダウンロード中にエラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class
