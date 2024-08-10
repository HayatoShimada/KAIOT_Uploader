Imports System.Net.NetworkInformation
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class SettingsForm

    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 初期設定値をセット
        TextBoxIp.Text = My.Settings.pFTPadd
        TextBoxDir.Text = My.Settings.pFTPfolder
        TextBoxUser.Text = My.Settings.pFTPuser
        TextBoxPass.Text = My.Settings.pFTPpass
    End Sub


    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        ' キャンセルボタンを押すとダイアログを閉じる
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ButtonCheckConnection_Click(sender As Object, e As EventArgs) Handles ButtonCheckConnection.Click
        ' チェックするFTPサーバーのIPアドレス
        CheckFtpConnection(TextBoxIp.Text, TextBoxUser.Text, TextBoxPass.Text)
    End Sub

    Private Sub CheckFtpConnection(ftpServer As String, ftpUserName As String, ftpPassword As String)
        Try
            ' FTPスクリプトを作成
            Dim ftpCommands As String = String.Join(Environment.NewLine, {
                $"open {ftpServer}",
                ftpUserName,
                ftpPassword,
                "bye"
            })

            ' 一時ファイルにFTPスクリプトを保存
            Dim tempScriptPath As String = IO.Path.Combine(IO.Path.GetTempPath(), "ftpCheckScript.txt")
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

            ' 成功メッセージを検索
            If output.Contains("無効です") Then
                MessageBox.Show("FTPサーバーへの接続に失敗しました。", "通信確認", MessageBoxButtons.OK, MessageBoxIcon.Warning)


            Else
                MessageBox.Show("FTPサーバーに正常に接続できました。", "通信確認", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' 入力されたFTP設定を保存
                My.Settings.pFTPadd = TextBoxIp.Text
                My.Settings.pFTPfolder = TextBoxDir.Text
                My.Settings.pFTPuser = TextBoxUser.Text
                My.Settings.pFTPpass = TextBoxPass.Text

                ' OKボタンを押すとダイアログを閉じる
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If

            ' 一時ファイルを削除
            IO.File.Delete(tempScriptPath)

        Catch ex As Exception
            MessageBox.Show("接続確認中にエラーが発生しました: " & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
