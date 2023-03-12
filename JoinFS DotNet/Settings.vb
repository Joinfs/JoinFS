Public Class Settings
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Set the settings in local area
        My.Settings.DarkMode = DarkMode.Checked
        My.Settings.Simulator = ComboBox1.Text
        My.Settings.EnhancedLogs = EnhancedLogs.Checked
        My.Settings.SimBriefUID = TextBox1.Text
        My.Settings.SaveLogs = CheckBox1.Checked
        My.Settings.LowBandwidth = ComboBox2.Text
        My.Settings.BetaMode = CheckBox2.Checked
        My.Settings.AutoConnect = CheckBox3.Checked
        Functions.saveLog()
        My.Settings.Save()
        ' Update latest information
        Functions.DarkModeToggle()
        Functions.AddLogItem("Updated User Settings")
        Functions.AddLogItem("Dark Mode set to: " + DarkMode.Checked.ToString)
        Functions.AddLogItem("Simulator set to: " + My.Settings.Simulator)
        Functions.BandwidthMode()
        Me.Hide()

    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Text = My.Settings.Simulator
        EnhancedLogs.Checked = My.Settings.EnhancedLogs
        DarkMode.Checked = My.Settings.DarkMode
        CheckBox1.Checked = My.Settings.SaveLogs
        ComboBox2.Text = My.Settings.LowBandwidth
        CheckBox2.Checked = My.Settings.BetaMode
        CheckBox3.Checked = My.Settings.AutoConnect
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged

    End Sub
End Class