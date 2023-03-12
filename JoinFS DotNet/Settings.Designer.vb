<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Settings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        DarkMode = New CheckBox()
        Button1 = New Button()
        ComboBox1 = New ComboBox()
        Label1 = New Label()
        EnhancedLogs = New CheckBox()
        Label2 = New Label()
        TextBox1 = New TextBox()
        CheckBox1 = New CheckBox()
        Label3 = New Label()
        ComboBox2 = New ComboBox()
        CheckBox2 = New CheckBox()
        CheckBox3 = New CheckBox()
        SuspendLayout()
        ' 
        ' DarkMode
        ' 
        DarkMode.AutoSize = True
        DarkMode.Location = New Point(8, 12)
        DarkMode.Name = "DarkMode"
        DarkMode.Size = New Size(84, 19)
        DarkMode.TabIndex = 0
        DarkMode.Text = "Dark Mode"
        DarkMode.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Dock = DockStyle.Bottom
        Button1.Location = New Point(0, 184)
        Button1.Name = "Button1"
        Button1.Size = New Size(229, 23)
        Button1.TabIndex = 1
        Button1.Text = "Apply"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' ComboBox1
        ' 
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.FormattingEnabled = True
        ComboBox1.Items.AddRange(New Object() {"P3Dv5", "MSFS"})
        ComboBox1.Location = New Point(85, 97)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(137, 23)
        ComboBox1.TabIndex = 2
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(6, 100)
        Label1.Name = "Label1"
        Label1.Size = New Size(58, 15)
        Label1.TabIndex = 3
        Label1.Text = "Simulator"' 
        ' EnhancedLogs
        ' 
        EnhancedLogs.AutoSize = True
        EnhancedLogs.Location = New Point(122, 33)
        EnhancedLogs.Name = "EnhancedLogs"
        EnhancedLogs.Size = New Size(106, 19)
        EnhancedLogs.TabIndex = 4
        EnhancedLogs.Text = "Enhanced Logs"
        EnhancedLogs.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(6, 129)
        Label2.Name = "Label2"
        Label2.Size = New Size(74, 15)
        Label2.TabIndex = 5
        Label2.Text = "SimBrief UN:"' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(86, 126)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(137, 23)
        TextBox1.TabIndex = 6
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(122, 12)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(78, 19)
        CheckBox1.TabIndex = 7
        CheckBox1.Text = "Save Logs"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(6, 157)
        Label3.Name = "Label3"
        Label3.Size = New Size(53, 15)
        Label3.TabIndex = 8
        Label3.Text = "Updates:"' 
        ' ComboBox2
        ' 
        ComboBox2.FormattingEnabled = True
        ComboBox2.Items.AddRange(New Object() {"Very Slow", "Slow", "Normal", "Fast"})
        ComboBox2.Location = New Point(85, 154)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(138, 23)
        ComboBox2.TabIndex = 9
        ComboBox2.Text = "Normal"' 
        ' CheckBox2
        ' 
        CheckBox2.AutoSize = True
        CheckBox2.Location = New Point(8, 33)
        CheckBox2.Name = "CheckBox2"
        CheckBox2.Size = New Size(83, 19)
        CheckBox2.TabIndex = 10
        CheckBox2.Text = "Beta Mode"
        CheckBox2.UseVisualStyleBackColor = True
        ' 
        ' CheckBox3
        ' 
        CheckBox3.AutoSize = True
        CheckBox3.Location = New Point(8, 58)
        CheckBox3.Name = "CheckBox3"
        CheckBox3.Size = New Size(100, 19)
        CheckBox3.TabIndex = 11
        CheckBox3.Text = "Auto Connect"
        CheckBox3.UseVisualStyleBackColor = True
        ' 
        ' Settings
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(229, 207)
        Controls.Add(CheckBox3)
        Controls.Add(CheckBox2)
        Controls.Add(ComboBox2)
        Controls.Add(Label3)
        Controls.Add(CheckBox1)
        Controls.Add(TextBox1)
        Controls.Add(Label2)
        Controls.Add(EnhancedLogs)
        Controls.Add(Label1)
        Controls.Add(ComboBox1)
        Controls.Add(Button1)
        Controls.Add(DarkMode)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "Settings"
        Text = "Join FS Settings"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents DarkMode As CheckBox
    Friend WithEvents Button1 As Button
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents EnhancedLogs As CheckBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Label3 As Label
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents CheckBox3 As CheckBox
End Class
