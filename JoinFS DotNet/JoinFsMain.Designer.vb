<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class JoinFsMain
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
        components = New ComponentModel.Container()
        Dim resources As ComponentModel.ComponentResourceManager = New ComponentModel.ComponentResourceManager(GetType(JoinFsMain))
        MenuStrip1 = New MenuStrip()
        FileToolStripMenuItem = New ToolStripMenuItem()
        SettingsToolStripMenuItem = New ToolStripMenuItem()
        ToolStripSeparator1 = New ToolStripSeparator()
        ExitToolStripMenuItem = New ToolStripMenuItem()
        ViewToolStripMenuItem = New ToolStripMenuItem()
        SystemLogsToolStripMenuItem = New ToolStripMenuItem()
        HelpToolStripMenuItem = New ToolStripMenuItem()
        SupportToolStripMenuItem = New ToolStripMenuItem()
        ReportBugsToolStripMenuItem = New ToolStripMenuItem()
        AboutToolStripMenuItem = New ToolStripMenuItem()
        NickName = New TextBox()
        Button3 = New Button()
        Button4 = New Button()
        VAList = New ComboBox()
        StatusStrip1 = New StatusStrip()
        ConnectedText = New ToolStripStatusLabel()
        SimTimer = New Timer(components)
        NetTimer = New Timer(components)
        PictureBox1 = New PictureBox()
        ClientListToolStripMenuItem = New ToolStripMenuItem()
        MenuStrip1.SuspendLayout()
        StatusStrip1.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.BackColor = SystemColors.Control
        MenuStrip1.Items.AddRange(New ToolStripItem() {FileToolStripMenuItem, ViewToolStripMenuItem, HelpToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(227, 24)
        MenuStrip1.TabIndex = 0
        MenuStrip1.Text = "MenuStrip1"' 
        ' FileToolStripMenuItem
        ' 
        FileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SettingsToolStripMenuItem, ToolStripSeparator1, ExitToolStripMenuItem})
        FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        FileToolStripMenuItem.Size = New Size(37, 20)
        FileToolStripMenuItem.Text = "File"' 
        ' SettingsToolStripMenuItem
        ' 
        SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        SettingsToolStripMenuItem.Size = New Size(116, 22)
        SettingsToolStripMenuItem.Text = "Settings"' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(113, 6)
        ' 
        ' ExitToolStripMenuItem
        ' 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        ExitToolStripMenuItem.Size = New Size(116, 22)
        ExitToolStripMenuItem.Text = "Exit"' 
        ' ViewToolStripMenuItem
        ' 
        ViewToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SystemLogsToolStripMenuItem, ClientListToolStripMenuItem})
        ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        ViewToolStripMenuItem.Size = New Size(44, 20)
        ViewToolStripMenuItem.Text = "View"' 
        ' SystemLogsToolStripMenuItem
        ' 
        SystemLogsToolStripMenuItem.Name = "SystemLogsToolStripMenuItem"
        SystemLogsToolStripMenuItem.Size = New Size(180, 22)
        SystemLogsToolStripMenuItem.Text = "System Logs"' 
        ' HelpToolStripMenuItem
        ' 
        HelpToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {SupportToolStripMenuItem, ReportBugsToolStripMenuItem, AboutToolStripMenuItem})
        HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        HelpToolStripMenuItem.Size = New Size(44, 20)
        HelpToolStripMenuItem.Text = "Help"' 
        ' SupportToolStripMenuItem
        ' 
        SupportToolStripMenuItem.Name = "SupportToolStripMenuItem"
        SupportToolStripMenuItem.Size = New Size(138, 22)
        SupportToolStripMenuItem.Text = "Support"' 
        ' ReportBugsToolStripMenuItem
        ' 
        ReportBugsToolStripMenuItem.Name = "ReportBugsToolStripMenuItem"
        ReportBugsToolStripMenuItem.Size = New Size(138, 22)
        ReportBugsToolStripMenuItem.Text = "Report Bugs"' 
        ' AboutToolStripMenuItem
        ' 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        AboutToolStripMenuItem.Size = New Size(138, 22)
        AboutToolStripMenuItem.Text = "About"' 
        ' NickName
        ' 
        NickName.Location = New Point(12, 27)
        NickName.Name = "NickName"
        NickName.Size = New Size(97, 23)
        NickName.TabIndex = 1
        ' 
        ' Button3
        ' 
        Button3.BackColor = Color.Red
        Button3.ForeColor = SystemColors.Control
        Button3.Location = New Point(12, 112)
        Button3.Name = "Button3"
        Button3.Size = New Size(88, 23)
        Button3.TabIndex = 4
        Button3.Text = "Simulator"
        Button3.UseVisualStyleBackColor = False
        ' 
        ' Button4
        ' 
        Button4.BackColor = Color.Red
        Button4.ForeColor = SystemColors.ButtonHighlight
        Button4.Location = New Point(106, 112)
        Button4.Name = "Button4"
        Button4.Size = New Size(107, 23)
        Button4.TabIndex = 5
        Button4.Text = "Network"
        Button4.UseVisualStyleBackColor = False
        ' 
        ' VAList
        ' 
        VAList.DropDownStyle = ComboBoxStyle.DropDownList
        VAList.FormattingEnabled = True
        VAList.Items.AddRange(New Object() {"GLOBAL", "VirginXL"})
        VAList.Location = New Point(115, 27)
        VAList.Name = "VAList"
        VAList.Size = New Size(98, 23)
        VAList.TabIndex = 6
        ' 
        ' StatusStrip1
        ' 
        StatusStrip1.Items.AddRange(New ToolStripItem() {ConnectedText})
        StatusStrip1.Location = New Point(0, 145)
        StatusStrip1.Name = "StatusStrip1"
        StatusStrip1.Size = New Size(227, 22)
        StatusStrip1.TabIndex = 8
        StatusStrip1.Text = "StatusStrip1"' 
        ' ConnectedText
        ' 
        ConnectedText.Name = "ConnectedText"
        ConnectedText.Size = New Size(86, 17)
        ConnectedText.Text = "ConnectedText"' 
        ' SimTimer
        ' 
        SimTimer.Interval = 500
        ' 
        ' NetTimer
        ' 
        NetTimer.Interval = 500
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Location = New Point(12, 56)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(201, 50)
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.TabIndex = 9
        PictureBox1.TabStop = False
        ' 
        ' ClientListToolStripMenuItem
        ' 
        ClientListToolStripMenuItem.Name = "ClientListToolStripMenuItem"
        ClientListToolStripMenuItem.Size = New Size(180, 22)
        ClientListToolStripMenuItem.Text = "Client List"' 
        ' JoinFsMain
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = SystemColors.Control
        ClientSize = New Size(227, 167)
        Controls.Add(PictureBox1)
        Controls.Add(StatusStrip1)
        Controls.Add(VAList)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(NickName)
        Controls.Add(MenuStrip1)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MainMenuStrip = MenuStrip1
        MaximizeBox = False
        MaximumSize = New Size(263, 226)
        MinimumSize = New Size(243, 206)
        Name = "JoinFsMain"
        StartPosition = FormStartPosition.WindowsDefaultBounds
        Text = "JoinFS"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        StatusStrip1.ResumeLayout(False)
        StatusStrip1.PerformLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NickName As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents VAList As ComboBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ConnectedText As ToolStripStatusLabel
    Friend WithEvents SettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SupportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReportBugsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SimTimer As Timer
    Friend WithEvents NetTimer As Timer
    Friend WithEvents SystemLogsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClientListToolStripMenuItem As ToolStripMenuItem
End Class
