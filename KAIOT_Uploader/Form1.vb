﻿Public Class Form1

    Private temporaryFiles As New List(Of String)() ' ダウンロードしたファイルのパスを保持するリスト

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        CashDelete()

        ListUpdate()
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        CashDelete()

        ListUpdate()

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        CashDelete()

        ListUpdate()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        ' 通信設定フォームをモーダルで表示
        Dim settingsForm As New SettingsForm()

        ' モーダル表示のため、ユーザーが設定を完了するまでメインフォームが表示されない
        If settingsForm.ShowDialog() = DialogResult.OK Then

        Else
            ' キャンセルされた場合、アプリケーションを終了
            Me.Close()
        End If

        TextBox2.Text = My.Settings.pFTPadd
        TextBox3.Text = My.Settings.pFTPfolder
        TextBox4.Text = My.Settings.pFTPuser
        TextBox5.Text = My.Settings.pFTPpass

    End Sub


    Private Sub CashDelete()
        If PictureBox1.Image IsNot Nothing Then
            PictureBox1.Image.Dispose()
            PictureBox1.Image = Nothing

        End If
        temporaryFiles.Clear()

    End Sub
    ' フォームが閉じられるときに一時ファイルを削除する
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CashDelete()

    End Sub

    Private Function ComboBoxSelecter()
        Dim filePath2 As String = ""

        ' ComboBox1, 2の選択に応じて filePath2 の値を設定
        If ComboBox1.SelectedItem Is Nothing Then
            MsgBox("画像種別を選択してください")
            Return filePath2 = "err"

        End If

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

        If ComboBox2.SelectedItem Is Nothing Then
            MsgBox("画像種別を選択してください")
            Return filePath2 = "err"

        End If

        Select Case ComboBox2.Text
            Case "指定なし"
                filePath2 = filePath2 & "_U"
            Case "上型"
                filePath2 = filePath2 & "_U"
            Case "下型"
                filePath2 = filePath2 & "_D"
        End Select

        Return filePath2

    End Function

    ' 画像の追加処理
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ftpServer As String = TextBox2.Text
        Dim targetFolder As String = TextBox3.Text
        Dim filePath1 As String = NumericUpDown1.Value.ToString()
        Dim filePath2 As String
        Dim localFileMulti As String
        Dim imageFiles As String()

        filePath2 = ComboBoxSelecter()

        If filePath2 = "err" Then
            Exit Sub

        End If

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
                If ListBox1.Items.Count = 0 Then
                    itemMaxNum = 0

                ElseIf ListBox1.Items.Count = 1 Then
                    itemMaxNum = 1

                Else
                    For Each item As Object In ListBox1.Items
                        Dim itemNum As Integer
                        Dim itemName As String = item.Replace($"/{targetFolder}/{filePath1}_{filePath2}_", "")
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
                Dim remoteFilePath As String = $"{targetFolder}/{filePath1}_{filePath2}_{itemMaxNum}.jpg"

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


                ListUpdate()

            Next
            CashDelete()


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
        Dim filePath2 As String
        Dim imageFiles As String()
        Dim localFileSingle As String

        filePath2 = ComboBoxSelecter()

        If filePath2 = "err" Then
            Exit Sub

        End If

        Dim ftpUserName As String = TextBox4.Text
        Dim ftpPassword As String = TextBox5.Text

        If ListBox1.SelectedItem Is Nothing Then
            MsgBox("画像を選択してください")
            Exit Sub

        End If

        Dim selectedFile As String = ListBox1.SelectedItem.ToString()


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
                $"delete ""{remoteFilePath}""",
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

            ListUpdate()
            CashDelete()
            MessageBox.Show("ファイルのアップロードが完了しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' 画像のプレビュー機能
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

        If ListBox1.SelectedItem Is Nothing Then
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
        Dim filePath2 As String

        filePath2 = ComboBoxSelecter()

        If filePath2 = "err" Then
            Exit Sub

        End If

        ' selectedFileはフルパスなので、画像名だけに絞る
        selectedFile = selectedFile.Replace($"/{targetFolder}_{filePath1}_{filePath2}_", "")

        ' 一時ファイルのパスを生成
        Dim localTempFile As String = IO.Path.Combine(IO.Path.GetTempPath(), selectedFile)

        Try
            ' FTPスクリプトを生成
            Dim ftpCommands As String = String.Join(Environment.NewLine, {
            $"open {ftpServer}",
            ftpUserName,
            ftpPassword,
            $"cd {targetFolder}",
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
            ' ダウンロードしたファイルのパスをリストに追加して後で削除する
            temporaryFiles.Add(localTempFile)

            ' 以前の画像を解放してから新しい画像を表示
            If PictureBox1.Image IsNot Nothing Then
                PictureBox1.Image.Dispose()
                PictureBox1.Image = Nothing
            End If

            ' ダウンロードした画像をPictureBoxに表示
            PictureBox1.Image = Image.FromFile(localTempFile)

        Catch ex As Exception
            MessageBox.Show("画像のダウンロード中にエラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ' 画像の削除処理
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim ftpServer As String = TextBox2.Text
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


            ListUpdate()

            MessageBox.Show("ファイルを削除しました。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
        Dim filePath2 As String

        filePath2 = ComboBoxSelecter()

        If filePath2 = "err" Then
            Exit Sub

        End If

        Try
            ' FTPコマンドを生成
            Dim ftpCommands As String = String.Join(Environment.NewLine, {
                $"open {ftpServer}",
                ftpUserName,
                ftpPassword,
                $"cd {targetFolder}",
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

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub


End Class
