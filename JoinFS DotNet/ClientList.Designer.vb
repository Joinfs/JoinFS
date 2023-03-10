<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ClientList
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
        components = New ComponentModel.Container()
        DataGridView1 = New DataGridView()
        Current = New DataGridViewTextBoxColumn()
        Nickname = New DataGridViewTextBoxColumn()
        VA = New DataGridViewTextBoxColumn()
        Latitude = New DataGridViewTextBoxColumn()
        Longitude = New DataGridViewTextBoxColumn()
        Altitude = New DataGridViewTextBoxColumn()
        Distance = New DataGridViewTextBoxColumn()
        ClientListTimer = New Timer(components)
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.Columns.AddRange(New DataGridViewColumn() {Current, Nickname, VA, Latitude, Longitude, Altitude, Distance})
        DataGridView1.Cursor = Cursors.IBeam
        DataGridView1.Dock = DockStyle.Fill
        DataGridView1.EditMode = DataGridViewEditMode.EditOnEnter
        DataGridView1.Location = New Point(0, 0)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.ReadOnly = True
        DataGridView1.RowTemplate.Height = 25
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.ShowCellErrors = False
        DataGridView1.ShowCellToolTips = False
        DataGridView1.ShowEditingIcon = False
        DataGridView1.ShowRowErrors = False
        DataGridView1.Size = New Size(1047, 511)
        DataGridView1.TabIndex = 0
        DataGridView1.UseWaitCursor = True
        ' 
        ' Current
        ' 
        Current.HeaderText = "Current"
        Current.Name = "Current"
        Current.ReadOnly = True
        Current.Width = 50
        ' 
        ' Nickname
        ' 
        Nickname.HeaderText = "Nickname"
        Nickname.Name = "Nickname"
        Nickname.ReadOnly = True
        Nickname.Width = 200
        ' 
        ' VA
        ' 
        VA.HeaderText = "Virtual Airline"
        VA.Name = "VA"
        VA.ReadOnly = True
        VA.Width = 200
        ' 
        ' Latitude
        ' 
        Latitude.HeaderText = "Latitude"
        Latitude.Name = "Latitude"
        Latitude.ReadOnly = True
        ' 
        ' Longitude
        ' 
        Longitude.HeaderText = "Longitude"
        Longitude.Name = "Longitude"
        Longitude.ReadOnly = True
        ' 
        ' Altitude
        ' 
        Altitude.HeaderText = "Altitude"
        Altitude.Name = "Altitude"
        Altitude.ReadOnly = True
        ' 
        ' Distance
        ' 
        Distance.HeaderText = "Distance (NM)"
        Distance.Name = "Distance"
        Distance.ReadOnly = True
        ' 
        ' ClientListTimer
        ' 
        ClientListTimer.Interval = 2000
        ' 
        ' ClientList
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1047, 511)
        Controls.Add(DataGridView1)
        MaximizeBox = False
        MaximumSize = New Size(1063, 9999)
        MinimumSize = New Size(1063, 100)
        Name = "ClientList"
        Text = "Client List"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents ClientListTimer As Timer
    Friend WithEvents Current As DataGridViewTextBoxColumn
    Friend WithEvents Nickname As DataGridViewTextBoxColumn
    Friend WithEvents VA As DataGridViewTextBoxColumn
    Friend WithEvents Latitude As DataGridViewTextBoxColumn
    Friend WithEvents Longitude As DataGridViewTextBoxColumn
    Friend WithEvents Altitude As DataGridViewTextBoxColumn
    Friend WithEvents Distance As DataGridViewTextBoxColumn
End Class
