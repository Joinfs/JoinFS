'Option Strict On
Option Explicit On
Imports System.Drawing.Drawing2D
Imports JoinFS_DotNet.P3Dv5SimCon
Imports JoinFS_DotNet.Functions
Imports JoinFS_DotNet.MainVariables
Imports JoinFS_DotNet.JoinFsMain
Imports System.ComponentModel
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports Microsoft.FlightSimulator.SimConnect
Imports Newtonsoft.Json
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Text.Json.Nodes
Imports Newtonsoft.Json.Linq
Imports System.Text.RegularExpressions

Public Class P3Dv5SimCon
    Public Shared p3d_simconnect As SimConnect = Nothing
    Private Const WM_USER_SIMCONNECT As Integer = &H402

    Enum StructDefinitions

        Struct1 = 1
        SIMULATOR_NAME

    End Enum
    Enum DataRequests

        Request_1 = 1
        SIMULATOR_NAME

    End Enum
    Enum DATA_REQUESTS
        SIMULATOR_NAME
    End Enum

    Enum Definitions
        SIMULATOR_NAME
    End Enum
    ' This is how you declare a data structure so that simconnect knows how to fill it/read it.

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi, Pack:=1)>
    Structure Struct1
        ' This is how you declare a fixed size string
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)>
        Public title As String
        Public latitude As Double
        Public longitude As Double
        Public altitude As Double
        Public heading As Double
        Public GroundSpeed As Double
        Public IndicatedAirSpeed As Double
        Public TrueAirSpeed As Double
        Public Pitch As Double
        Public Bank As Double
    End Structure
    ' Output text line number

    Dim line
    Public Sub New()
        ' Add any initialization after the InitializeComponent() call.
        p3d_simconnect = Nothing
        line = 0
    End Sub
    Public Class Settings
        Public Shared ReadOnly Simulator As String = My.Settings.Simulator

    End Class

    Public Shared Sub closeConnection()
        If p3d_simconnect IsNot Nothing Then
            p3d_simconnect.Dispose()
            p3d_simconnect = Nothing
            JoinFsMain.ConnectedText.Text = "Connection to Simulator Closed"
            ' Set the simulator button to red, and cancel the timer.
            JoinFsMain.SimTimer.Enabled = False
            JoinFsMain.Button3.BackColor = Color.Red
            AddLogItem("Simulator Connection has been closed.")
        End If

    End Sub
    Public Shared Function connectSimulator() As Boolean
        REM Open
        AddLogItem("Function called: Connect Simulator")
        Try
            p3d_simconnect.RequestDataOnSimObjectType(DataRequests.Request_1, StructDefinitions.Struct1, 0, SIMCONNECT_SIMOBJECT_TYPE.USER)
            Return True

        Catch ex As Exception
            REM Failed to connect 
            MessageBox.Show("Unable to connect to the P3Dv5 Simulator")
            AddLogItem("Unable to connect to simulator due to error: " + ex.Message + " at line: 82 P3Dv5SimCon.vb")
            Return False

        End Try

        REM Close
    End Function

    Public Shared Sub p3d_simconnect_OnRecvSimobjectDataBytype(ByVal sender As SimConnect, ByVal data As SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE)
        Select Case data.dwRequestID
            Case DataRequests.Request_1
                ' Cast the data input to the correct structure type
                Dim s1 As Struct1 = CType(data.dwData(0), Struct1)
                Dim headingInDegrees As Double = s1.heading
                ''DisplayText("title: " + s1.title)
                My.Settings.SimLatitude = s1.latitude.ToString("##0.00000000")
                My.Settings.SimLongitude = s1.longitude.ToString("##0.00000000")
                My.Settings.SimAltitude = s1.altitude.ToString("#####0")
                My.Settings.SimPlaneTitle = s1.title
                My.Settings.SimHeading = s1.heading.ToString("##0")
                My.Settings.PlaneGSpeed = s1.GroundSpeed.ToString("#0")
                My.Settings.PlaneIndicatedAirSpeed = s1.IndicatedAirSpeed.ToString("#0")
                My.Settings.PlaneTrueAirSpeed = s1.TrueAirSpeed.ToString("#0")
                My.Settings.PlaneBank = s1.Bank.ToString("#0.00")
                My.Settings.PlanePitch = s1.Pitch.ToString("#0.00")
                ' Dim name As String = System.Text.Encoding.ASCII.GetString(data.dwData(0), 0, data.dwDefineCount)

                ' Display the simulator name in a message box
                ' MessageBox.Show("Simulator Name: " & name)
                If My.Settings.EnhancedLogs = True Then
                    AddLogItem("|LA| " + s1.latitude.ToString("##0.00000000") + " |LN| " + s1.longitude.ToString("##0.00000000") + " |A| " + s1.altitude.ToString("#####0") + " |H| " + s1.heading.ToString("##0"))
                    AddLogItem("|GS| " + My.Settings.PlaneGSpeed + " |IAS| " + My.Settings.PlaneIndicatedAirSpeed + " |TS| " + My.Settings.PlaneTrueAirSpeed)
                End If
                JoinFsMain.Button3.BackColor = Color.Green
                JoinFsMain.ConnectedText.Text = "|LA| " + s1.latitude.ToString("##0.00000000") + " |LN| " + s1.longitude.ToString("##0.00000000") + " |A| " + s1.altitude.ToString("#####0") + " |H| " + s1.heading.ToString("##0")
            Case Else
                ' AddLogItem("Unknown request ID: " + data.dwRequestID.ToString())
        End Select

    End Sub
    Private Sub simconnect_OnRecvSimobjectData(ByVal sender As SimConnect, ByVal data As SIMCONNECT_RECV_SIMOBJECT_DATA)
        Select Case data.dwRequestID
            Case DATA_REQUESTS.SIMULATOR_NAME
                ' Get the simulator name from the received data
                Dim name As String = System.Text.Encoding.ASCII.GetString(data.dwData(0), 0, data.dwDefineCount)

                ' Display the simulator name in a message box
                MessageBox.Show("Simulator Name: " & name)
        End Select
    End Sub
    Public Shared Sub p3d_simconnect_OnRecvQuit(ByVal sender As SimConnect, ByVal data As SIMCONNECT_RECV)
        AddLogItem("SimObject Quit")
        JoinFsMain.ConnectedText.Text = "Prepar3D has exited"
        AddLogItem("P3Dv5 has exited, disconnecting from simulator.")
        closeConnection()
        JoinFsMain.Button3.BackColor = Color.Red
        JoinFsMain.SimTimer.Enabled = False
    End Sub
    Public Shared Sub p3d_simconnect_OnRecvException(ByVal sender As SimConnect, ByVal data As SIMCONNECT_RECV_EXCEPTION)
        AddLogItem("SimObject Recieved Exception")
        JoinFsMain.ConnectedText.Text = "Exception received: " + data.dwException.ToString()
        AddLogItem("Exception recieved : " + data.dwException.ToString())
        AddLogItem(data.ToString)
        JoinFsMain.Button3.BackColor = Color.Orange
    End Sub
    Public Shared Sub p3d_simconnect_OnRecvOpen(ByVal sender As SimConnect, ByVal data As SIMCONNECT_RECV_OPEN)
        If My.Settings.EnhancedLogs = True Then
            AddLogItem("SimObject Started - Decoding")
        End If
        Try
            If My.Settings.EnhancedLogs = True Then
                AddLogItem("Recieved SimConnect Open: - Getting Initial Data")
                initDataRequest()
            End If
        Catch ex As Exception
            JoinFsMain.SimTimer.Enabled = False
            JoinFsMain.Button3.BackColor = Color.Red
            JoinFsMain.ConnectedText.Text = "Error Occured"
            AddLogItem("ERROR: SimConnect Open error: " + ex.Message)
        End Try
    End Sub
    Public Shared Sub initDataRequest()
        If My.Settings.EnhancedLogs = True Then
            AddLogItem("Sub Called: initDataRequest")
        End If
        Try
            ' Set up all the SimConnect related data definitions and event handlers
            ' listen to connect and quit msgs
            ' define a data structure, note the last parameter, datumID must be different for each item
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "title", "", SIMCONNECT_DATATYPE.STRING256, 0, 0)
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, 1)
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, 2)
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0, 3)
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "PLANE HEADING DEGREES TRUE", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, 4)
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "GROUND VELOCITY", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0, 5)
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "AIRSPEED INDICATED", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0, 6)
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "AIRSPEED TRUE", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0, 7)
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "ATTITUDE INDICATOR PITCH DEGREES", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, 8)
            p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "ATTITUDE INDICATOR BANK DEGREES", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, 9)
            p3d_simconnect.AddToDataDefinition(Definitions.SIMULATOR_NAME, "Title", "", SIMCONNECT_DATATYPE.STRING256, 0.0, SimConnect.SIMCONNECT_UNUSED)
            p3d_simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.SIMULATOR_NAME, Definitions.SIMULATOR_NAME, 0, SIMCONNECT_SIMOBJECT_TYPE.USER)

            ' IMPORTANT: register it with the simconnect managed wrapper marshaller
            ' if you skip this step, you will only receive an int in the .dwData field.
            p3d_simconnect.RegisterDataDefineStruct(Of Struct1)(StructDefinitions.Struct1)
            ' catch a simobject data request
            AddHandler p3d_simconnect.OnRecvSimobjectDataBytype, New SimConnect.RecvSimobjectDataBytypeEventHandler(AddressOf p3d_simconnect_OnRecvSimobjectDataBytype)

            If My.Settings.EnhancedLogs = True Then
                AddLogItem("Listening for simobjects")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            JoinFsMain.SimTimer.Enabled = False
            JoinFsMain.Button3.BackColor = Color.Red
            JoinFsMain.ConnectedText.Text = "Error Occured"
            AddLogItem("An Error occured during initDataRequest: " + ex.Message)
        End Try
    End Sub
    Public Shared Function getAircraftLocation() As String
        If My.Settings.EnhancedLogs = True Then
            AddLogItem("Function: getAircraftLocation executed")
        End If
        ' set the variable for returning
        Dim Location As String = ""
        ' Connect to the simulator for the simConnect Data
        If p3d_simconnect IsNot Nothing Then
            Try
                JoinFsMain.ConnectedText.Text = "Connected to simulator."
                JoinFsMain.Button3.BackColor = Color.Green
                Return Location
            Catch ex As Exception
                JoinFsMain.ConnectedText.Text = "Failed to connect"
                ' Set the simulator button to red, and cancel the timer.
                JoinFsMain.SimTimer.Enabled = False
                JoinFsMain.Button3.BackColor = Color.Red
                MessageBox.Show("Failed to connect to simulator.")
                AddLogItem("Failed to connect to simulator during get aircraft location: " + ex.Message)
                Return "False"
            End Try
        Else
            JoinFsMain.ConnectedText.Text = "Failed to connect"
        End If
        ' Now Subscribe to the users location
        ' Return the location to the method calling the function
        Return Location
    End Function
    Public Structure SIMCONNECT_DATA_INITPOSITION
        ' This is how you declare a fixed size string
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=256)>
        Public Latitude As Double
        Public Longitude As Double
        Public Altitude As Double
        Public Pitch As Double
        Public Bank As Double
        Public Heading As Double
        Public OnGround As Integer ' 1 = on ground, 0 = in air
        Public Airspeed As Double ' knots
    End Structure
    Enum AI_AIRCRAFT_TYPE
        A320 = 0
        B747 = 1
        B737 = 2
    End Enum
    Private Shared Function IsFLAIModel(title As String) As Boolean
        ' Check if the aircraft title matches an FLAI model
        ' This example assumes that the FLAI model title format is "Aircraft Type Airline (Registration | Year | Livery)"
        Dim regex As New Regex("^[A-Za-z0-9\s]+ \([A-Za-z0-9\s]+ \| [A-Za-z0-9\s]+ \| [A-Za-z0-9\s]+\)$")
        Return regex.IsMatch(title)
    End Function
    Public Shared Function addOtherAircraft(lat As Integer, longitude As Integer, alt As Integer, pitch As Integer, bank As Integer, heading As Integer, speed As Integer, ground As Integer, callsign As String)
        Dim initData As Microsoft.FlightSimulator.SimConnect.SIMCONNECT_DATA_INITPOSITION
        initData.Latitude = lat ' degrees
        initData.Longitude = longitude ' degrees
        initData.Altitude = alt ' feet
        initData.Pitch = pitch ' degrees
        initData.Bank = bank ' degrees
        initData.Heading = heading ' degrees (will be overridden by the AI system)
        initData.OnGround = ground ' in air
        initData.Airspeed = speed ' knots (or whatever value you prefer)

        Dim aircraftType As AI_AIRCRAFT_TYPE = AI_AIRCRAFT_TYPE.A320
        If My.Settings.BetaMode = True Then

            p3d_simconnect.AICreateNonATCAircraft("Lockheed Martin F-35A Lightning II", callsign, initData, aircraftType) ' Thanks to Mattia1513 for the code
        End If

        Return Nothing

    End Function




End Class
