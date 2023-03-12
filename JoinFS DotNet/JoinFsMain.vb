
Imports System.Drawing.Drawing2D
Imports JoinFS_DotNet.P3Dv5SimCon
Imports JoinFS_DotNet.Functions
Imports JoinFS_DotNet.MainVariables
Imports System.ComponentModel
Imports System.IO
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
' Imports LockheedMartin.Prepar3D.SimConnect
Imports Microsoft.FlightSimulator.SimConnect
Imports System.Runtime.InteropServices
Imports System.Environment
Imports System.Xml
Imports Newtonsoft.Json.Linq
Imports System.Runtime.CompilerServices

Public Class JoinFsMain
    Private Const WM_USER_SIMCONNECT As Integer = &H402
    Private Async Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        AddLogItem("User chose the Exit Menu from the File Menu")
        Await CloseJoinFS()
    End Sub
    Private Sub ReportBugsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportBugsToolStripMenuItem.Click
        ' Open the report bugs via github link
        AddLogItem("Report Bugs Clicked in Menu Bar")
        Process.Start("explorer.exe", MainVariables.bugLink)
    End Sub

    Private Sub JoinFsMain_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
        If Not Directory.Exists(appData + "\JoinFS") Then
            Directory.CreateDirectory(appData + "\JoinFS")
        End If
        updateVAList()
        AddLogItem("Loaded JoinFS Version 4.0.0")
        ConnectedText.Text = "Disconnected"
        If (My.Settings.Nickname.Length > 0) Then
            NickName.Text = My.Settings.Nickname
        End If
        If (My.Settings.PreferedAirline.Length > 0) Then
            VAList.Text = My.Settings.PreferedAirline
        Else
            VAList.Text = "GLOBAL"
        End If
        Functions.DarkModeToggle()
        If VAList.Text = "VirginXL" Then
            PictureBox1.Image = My.Resources.VirginXL
        End If
        getSettings()
        BandwidthMode()
        ' Get a random id to make sure we don't generate ourselves within the client once we load network aircraft.
        My.Settings.RandomID = RandomString()

    End Sub
    Public Shared Function updateVAList()
        JoinFsMain.VAList.Items.Clear()
        JoinFsMain.VAList.Items.Add("GLOBAL")
        RetriveVAList()
        For Each item As JObject In JoinFS_DotNet.Functions.vaJsonArray
            ' Get the nickname from the item and print it
            JoinFsMain.VAList.Items.Add(item("vaname").ToString())
        Next
    End Function
    Private Sub SupportToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles SupportToolStripMenuItem.Click
        Process.Start("explorer.exe", MainVariables.supportLink)
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If (Button3.BackColor = Color.Red) Then
            If p3d_simconnect Is Nothing Then
                If (My.Settings.Simulator = "P3Dv5" Or My.Settings.Simulator = "MSFS" Or My.Settings.Simulator = "") Then
                    Button3.BackColor = Color.Orange
                    ConnectedText.Text = "Connecting to simulator.."
                    Try
                        p3d_simconnect = New SimConnect(" VB Managed Data Request", Me.Handle, WM_USER_SIMCONNECT, Nothing, 0)
                        AddLogItem("Successful Connection to Simulator via P3Dv5 SimConnect")
                        initDataRequest()
                        AddHandler p3d_simconnect.OnRecvOpen, New SimConnect.RecvOpenEventHandler(AddressOf p3d_simconnect_OnRecvOpen)
                        AddHandler p3d_simconnect.OnRecvQuit, New SimConnect.RecvQuitEventHandler(AddressOf p3d_simconnect_OnRecvQuit)
                        If My.Settings.EnhancedLogs = True Then
                            AddLogItem("Added Handler for open/quit messages")
                        End If
                        ' listen to exceptions
                        AddHandler p3d_simconnect.OnRecvException, New SimConnect.RecvExceptionEventHandler(AddressOf p3d_simconnect_OnRecvException)
                        If My.Settings.EnhancedLogs = True Then
                            AddLogItem("Added Handler for exceptions")
                        End If
                        ConnectedText.Text = "Simulator Connected"
                        Button3.BackColor = Color.Green
                        SimTimer.Enabled = True
                    Catch ex As Exception
                        AddLogItem("Error connecting to P3Dv5 Simulator " + ex.Message)
                        ConnectedText.Text = "Simulator Connection Failed see View > Logs"
                        Button3.BackColor = Color.Red
                        Console.Write(ex.Message)
                    End Try
                Else
                    closeConnection()
                End If


            Else
                ' We failed to connect, show the color as red.
                ConnectedText.Text = "Simulator Connection Failed see View > Logs"
                Button3.BackColor = Color.Red
            End If
        Else
            AddLogItem("Disconnected from simulator")
            Await API("Stop")
            ConnectedText.Text = "Simulator Disconnected"
            SimTimer.Enabled = False
            closeConnection()
            Button3.BackColor = Color.Red
        End If
    End Sub

    Private Sub SimTimer_Tick(sender As Object, e As EventArgs) Handles SimTimer.Tick
        ' Run a script to get the latest location of the aircraft that is handled by the p3d simulator
        If My.Settings.Simulator = "P3Dv5" Then
            p3d_simconnect.RequestDataOnSimObjectType(DataRequests.Request_1, StructDefinitions.Struct1, 0, SIMCONNECT_SIMOBJECT_TYPE.USER)
        Else
            AddLogItem("FATAL ERROR: Cancelled Simulator Timer as Simulator wasn't selected or invalid.")
            SimTimer.Enabled = False
            Button3.BackColor = Color.Red
        End If


    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        Settings.ShowDialog()
        AddLogItem("Settings Window shown")
    End Sub

    Private Async Sub JoinFsMain_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
        Await CloseJoinFS()
    End Sub

    Private Sub SystemLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SystemLogsToolStripMenuItem.Click
        Log.Show()
    End Sub
    Protected Overrides Sub DefWndProc(ByRef m As Message)

        If m.Msg = WM_USER_SIMCONNECT Then

            If p3d_simconnect IsNot Nothing Then
                p3d_simconnect.ReceiveMessage()

            End If
        Else
            MyBase.DefWndProc(m)
        End If

    End Sub

    Private Sub NetTimer_Tick(sender As Object, e As EventArgs) Handles NetTimer.Tick
        ' Do We have the simulator connected? if so we can start the network timer, if not cancel out.
        If SimTimer.Enabled = True Then
            Dim timer = API("POST")
            Dim clientUsers = API("GET")
        Else
            AddLogItem("Cancelled Network Timer as Simulator was disconnected")
            NetTimer.Enabled = False
            Button4.BackColor = Color.Red
        End If
    End Sub

    Private Async Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        addOtherAircraft()
        Await NetworkManager()
    End Sub

    Private Async Sub VAList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles VAList.SelectedIndexChanged
        My.Settings.PreferedAirline = VAList.Text
        My.Settings.Save()
        For Each item As JObject In JoinFS_DotNet.Functions.VajsonArray
            ' Get the nickname from the item and print it
            Dim selectedva = item("vaname").ToString()
            If VAList.Text = "GLOBAL" Then
                PictureBox1.Image = My.Resources.joinfs64

            ElseIf VAList.Text = selectedva Then
                If item("valogo").ToString = "" Then
                    PictureBox1.Image = My.Resources.joinfs64
                Else
                    Dim httpClient As New System.Net.Http.HttpClient()
                    Dim imageBytes As Byte() = Await httpClient.GetByteArrayAsync(item("valogo").ToString())
                    Dim ms As New IO.MemoryStream(imageBytes)
                    PictureBox1.Image = New System.Drawing.Bitmap(ms)
                End If
                ' Get the image from the url on the database

            End If
        Next
    End Sub

    Private Sub NickName_TextChanged(sender As Object, e As EventArgs) Handles NickName.TextChanged
        My.Settings.Nickname = NickName.Text
        My.Settings.Save()
        AddLogItem("Nickanme set to: " + My.Settings.Nickname)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.Show()
    End Sub

    Private Sub ClientListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClientListToolStripMenuItem.Click
        ClientList.Show()
    End Sub
End Class
