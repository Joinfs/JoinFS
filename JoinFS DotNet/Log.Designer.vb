<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Log
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
        ClearLog = New Button()
        RichTextBox1 = New RichTextBox()
        Button1 = New Button()
        SuspendLayout()
        ' 
        ' ClearLog
        ' 
        ClearLog.Location = New Point(713, 557)
        ClearLog.Name = "ClearLog"
        ClearLog.Size = New Size(75, 23)
        ClearLog.TabIndex = 1
        ClearLog.Text = "Clear Log"
        ClearLog.UseVisualStyleBackColor = True
        ' 
        ' RichTextBox1
        ' 
        RichTextBox1.BackColor = SystemColors.GrayText
        RichTextBox1.BorderStyle = BorderStyle.FixedSingle
        RichTextBox1.Cursor = Cursors.No
        RichTextBox1.ForeColor = SystemColors.Info
        RichTextBox1.Location = New Point(12, 12)
        RichTextBox1.MaxLength = 214748
        RichTextBox1.Name = "RichTextBox1"
        RichTextBox1.ReadOnly = True
        RichTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical
        RichTextBox1.Size = New Size(776, 539)
        RichTextBox1.TabIndex = 0
        RichTextBox1.Text = "" ' 
        ' Button1
        ' 
        Button1.Location = New Point(632, 557)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 2
        Button1.Text = "Save Log"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Log
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 589)
        Controls.Add(Button1)
        Controls.Add(ClearLog)
        Controls.Add(RichTextBox1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        MaximizeBox = False
        MinimizeBox = False
        Name = "Log"
        RightToLeft = RightToLeft.No
        ShowIcon = False
        Text = "System Log"
        ResumeLayout(False)
    End Sub

    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents ClearLog As Button
    Friend WithEvents Button1 As Button
End Class
