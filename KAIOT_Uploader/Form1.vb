Imports System.Diagnostics
Imports System.Net

Public Class Form1

    Private temporaryFiles As New List(Of String)() ' ダウンロードしたファイルのパスを保持するリスト

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ListUpdate()
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        ListUpdate()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.Text = My.Settings.pFTPadd
        TextBox3.Text = My.Settings.pFTPfolder
        TextBox4.Text = My.Settings.pFTPuser
        TextBox5.Text = My.Settings.pFTPpass
        ComboBox1.SelectedIndex = 0
    End Sub

    ' フォームが閉じられるときに一時ファイルを削除する
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        For Each file In temporaryFiles
            If IO.File.Exists(file) Then
                Try
                    IO.File.Delete(file)
                Catch ex As Exception
                    ' ファイル削除に失敗した場合の処理（ログなどに記録しておくとよい）
                End Try
            End If
        Next
    End Sub


    ' 画像の追加処理
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ftpServer As String = TextBox2.Text
        Dim targetFolder As String = TextBox3.Text
        Dim filePath1 As String = NumericUpDown1.Value.ToString()
        Dim filePath2 As String = "0"
        Dim localFileMulti As String
        Dim imageFiles As String()

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


        ListUpdate()

        Using ofd_IO As New OpenFileDialog
            With ofd_IO
                .Title = "画像選択"
                .Filter = "jpgファイル|*.jpg"
                .Multiselect = True ' 複数選択を許可
            End With
            If ofd_IO.ShowDialog() = DialogResult.OK Then
                imageFiles = ofd_IO.FileNames
                localFileMulti = String.Join(", ", imageFiles)
            Else
                Exit Sub
            End If
        End Using


        Try
            RichTextBox1.Clear() ' 結果表示用のリッチテキストボックスをクリア

            ' 画像ファイルごとにFTPアップロードを実行
            For i As Integer = 0 To imageFiles.Length - 1
                Dim fullpath As String = imageFiles(i)
                Dim originalFileName As String = System.IO.Path.GetFileName(fullpath)
                Dim itemMaxNum As Integer

                ' ListBox1のアイテムから追加する画像の名前を決定する。
                If Not ListBox1.Items.Count = 0 Then
                    itemMaxNum = 0

                ElseIf ListBox1.Items.Count = 1 Then
                    itemMaxNum = 1

                Else
                    For Each item As Object In ListBox1.Items
                        Dim itemNum As Integer
                        Dim itemName As String = item.Replace($"/{targetFolder}/{filePath1}/{filePath2}/", "")
                        itemName = itemName.Replace(".jpg", "")

                        ' ListBox1のアイテム名の中で一番大きい数字をアップロード画像の名前とする。
                        If IsNumeric(itemName) Then
                            itemNum = itemName
                            If itemMaxNum < itemNum Then
                                itemMaxNum = itemNum

                            End If
                        Else
                            Continue For
                        End If
                    Next

                    itemMaxNum += 1

                End If


                ' アップロード先のパスを作成
                Dim remoteFilePath As String = $"{targetFolder}/{filePath1}/{filePath2}/{itemMaxNum}.jpg"

                ' FTPスクリプトを作成
                Dim ftpCommands As String = String.Join(Environment.NewLine, {
                    $"open {ftpServer}",
                    ftpUserName,
                    ftpPassword,
                    $"put ""{imageFiles(i)}"" ""{remoteFilePath}""",
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

                ListUpdate()

            Next


            MessageBox.Show("ファイルのアップロードが完了しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' 画像の更新処理
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim ftpServer As String = TextBox2.Text
        Dim targetFolder As String = TextBox3.Text
        Dim filePath1 As String = NumericUpDown1.Value.ToString()
        Dim filePath2 As String = "0"
        Dim imageFiles As String()
        Dim localFileSingle As String

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

        If ListBox1.SelectedItem Is Nothing Then
            MsgBox("画像を選択してください")
            Exit Sub

        End If

        Dim selectedFile As String = ListBox1.SelectedItem.ToString()

        ListUpdate()

        Using ofd_IO As New OpenFileDialog
            With ofd_IO
                .Title = "画像選択"
                .Filter = "jpgファイル|*.jpg"
                .Multiselect = False ' 複数選択不可
            End With
            If ofd_IO.ShowDialog() = DialogResult.OK Then
                imageFiles = ofd_IO.FileNames
                localFileSingle = String.Join(", ", imageFiles)
            Else
                Exit Sub

            End If
        End Using


        Try
            RichTextBox1.Clear() ' 結果表示用のリッチテキストボックスをクリア

            ' アップロード先のパスを作成
            Dim remoteFilePath As String = selectedFile

            ' FTPスクリプトを作成
            Dim ftpCommands As String = String.Join(Environment.NewLine, {
                $"open {ftpServer}",
                ftpUserName,
                ftpPassword,
                $"put ""{localFileSingle}"" ""{remoteFilePath}""",
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

            ListUpdate()



            MessageBox.Show("ファイルのアップロードが完了しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' 画像のプレビュー機能
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

        If ListBox1.SelectedItems Is Nothing Then
            Exit Sub
        End If

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

        ' selectedFileはフルパスなので、画像名だけに絞る
        selectedFile = selectedFile.Replace($"/{targetFolder}/{filePath1}/{filePath2}/", "")

        ' ローカルにダウンロードするパスをプロジェクトのルートディレクトリに設定
        Dim localTempFile As String = IO.Path.Combine(Application.StartupPath, selectedFile)

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
            Dim output As String = process.StandardOutput.ReadToEnd()

            ' cmdの結果をリッチテキストに表示
            RichTextBox1.AppendText(output & Environment.NewLine)
            ' ダウンロードしたファイルのパスをリストに追加後から消す為
            temporaryFiles.Add(localTempFile)

            ' ダウンロードした画像をPictureBoxに表示
            PictureBox1.Image = Image.FromFile(localTempFile)

            ' ダウンロードした画像をフォトアプリで表示
            ' Process.Start(New ProcessStartInfo(localTempFile) With {.UseShellExecute = True})



        Catch ex As Exception
            MessageBox.Show("画像のダウンロード中にエラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    ' 画像の削除処理
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim ftpServer As String = TextBox2.Text
        Dim targetFolder As String = TextBox3.Text
        Dim filePath1 As String = NumericUpDown1.Value.ToString()
        Dim filePath2 As String = "0"
        Dim imageFiles As String()

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


        If ListBox1.SelectedItem Is Nothing Then
            MsgBox("画像を選択してください")
            Exit Sub

        End If

        Dim selectedFile As String = ListBox1.SelectedItem.ToString()

        Try
            RichTextBox1.Clear() ' 結果表示用のリッチテキストボックスをクリア


            ' アップロード先のパスを作成
            Dim remoteFilePath As String = selectedFile

            ' FTPスクリプトを作成
            Dim ftpCommands As String = String.Join(Environment.NewLine, {
                $"open {ftpServer}",
                ftpUserName,
                ftpPassword,
                $"delete ""{remoteFilePath}""",
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

            ListUpdate()



            MessageBox.Show("ファイルのアップロードが完了しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub ListUpdate()

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
                    If Not line.StartsWith("ls") Then
                        ListBox1.Items.Add(line)

                    End If
                End If
            Next

            RichTextBox1.AppendText(output & Environment.NewLine)


            ' 一時ファイルを削除
            IO.File.Delete(tempScriptPath)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

End Class
