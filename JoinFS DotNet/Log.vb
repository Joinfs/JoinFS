Public Class Log
    Private Sub Log_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Functions.AddLogItem("Log Shown")
    End Sub

    Private Sub ClearLog_Click(sender As Object, e As EventArgs) Handles ClearLog.Click
        RichTextBox1.Text = "Log Cleared!"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Functions.saveLog()
    End Sub
End Class