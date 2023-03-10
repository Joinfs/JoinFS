'Copyright (c) Lockheed Martin Corporation.  All rights reserved. 
'
' VB.NET Managed Data Request sample
'
' Click on Connect to try and connect to a running version of Prepar3D
' Click on Request Data any number of times
' Click on Disconnect to close the connection, and then you should
' be able to click on Connect and restart the process
'

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

' Add these two statements to all SimConnect clients
Imports LockheedMartin.Prepar3D.SimConnect
Imports System.Runtime.InteropServices

Public Class ManagedDataRequest

    Dim p3d_simconnect As SimConnect

    ' User-defined win32 event
    Private Const WM_USER_SIMCONNECT As Integer = &H402

    Enum StructDefinitions

        Struct1 = 1

    End Enum
    Enum DataRequests

        Request_1 = 1

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

    End Structure
    ' Output text line number

    Dim line

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        p3d_simconnect = Nothing
        line = 0
        SetButtons(True, False, False)

    End Sub

    Private Sub SetButtons(ByVal bConnect As Boolean, ByVal bRequest As Boolean, ByVal bDisconnect As Boolean)

        ButtonConnect.Enabled = bConnect
        ButtonRequest.Enabled = bRequest
        ButtonDisconnect.Enabled = bDisconnect

    End Sub


    Private Sub ButtonConnect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonConnect.Click

        If p3d_simconnect Is Nothing Then

            Try
                p3d_simconnect = New SimConnect(" VB Managed Data Request", Me.Handle, WM_USER_SIMCONNECT, Nothing, 0)
                initDataRequest()
                DisplayText("Connection request sent ...")
                SetButtons(False, True, True)

            Catch ex As Exception
                DisplayText("Failed to connect")
            End Try

        Else
            DisplayText("Error - try again")
            closeConnection()
        End If

    End Sub
    Private Sub initDataRequest()

        ' Set up all the SimConnect related data definitions and event handlers

        ' listen to connect and quit msgs
        AddHandler p3d_simconnect.OnRecvOpen, New SimConnect.RecvOpenEventHandler(AddressOf p3d_simconnect_OnRecvOpen)
        AddHandler p3d_simconnect.OnRecvQuit, New SimConnect.RecvQuitEventHandler(AddressOf p3d_simconnect_OnRecvQuit)

        ' listen to exceptions
        AddHandler p3d_simconnect.OnRecvException, New SimConnect.RecvExceptionEventHandler(AddressOf p3d_simconnect_OnRecvException)

        ' define a data structure, note the last parameter, datumID must be different for each item

        p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "Title", "", LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_DATATYPE.STRING256, 0, 0)
        p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "Plane Latitude", "degrees", LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_DATATYPE.FLOAT64, 0, 1)
        p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "Plane Longitude", "degrees", LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_DATATYPE.FLOAT64, 0, 2)
        p3d_simconnect.AddToDataDefinition(StructDefinitions.Struct1, "Plane Altitude", "feet", LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_DATATYPE.FLOAT64, 0, 3)

        ' IMPORTANT: register it with the simconnect managed wrapper marshaller
        ' if you skip this step, you will only receive an int in the .dwData field.

        p3d_simconnect.RegisterDataDefineStruct(Of Struct1)(StructDefinitions.Struct1)

        ' catch a simobject data request
        AddHandler p3d_simconnect.OnRecvSimobjectDataBytype, New SimConnect.RecvSimobjectDataBytypeEventHandler(AddressOf p3d_simconnect_OnRecvSimobjectDataBytype)

    End Sub
    Private Sub p3d_simconnect_OnRecvOpen(ByVal sender As SimConnect, ByVal data As SIMCONNECT_RECV_OPEN)

        DisplayText("Connected to Prepar3D")

    End Sub

    ' The case where the user closes Prepar3D
    Private Sub p3d_simconnect_OnRecvQuit(ByVal sender As SimConnect, ByVal data As LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_RECV)

        DisplayText("Prepar3D has exited")
        closeConnection()
        SetButtons(True, False, False)

    End Sub
    Private Sub p3d_simconnect_OnRecvException(ByVal sender As SimConnect, ByVal data As LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_RECV_EXCEPTION)

        DisplayText("Exception received: " + data.dwException.ToString())

    End Sub

    ' The case where the user closes the client
    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed

        closeConnection()

    End Sub
    Private Sub p3d_simconnect_OnRecvSimobjectDataBytype(ByVal sender As SimConnect, ByVal data As SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE)

        Select Case data.dwRequestID
            Case DataRequests.Request_1
                ' Cast the data input to the correct structure type
                Dim s1 As Struct1 = CType(data.dwData(0), Struct1)

                DisplayText("Title: " + s1.title)
                DisplayText("LLA:   " + s1.latitude.ToString("##0.00") + "  " + s1.longitude.ToString("##0.00") + "  " + s1.altitude.ToString("#####0.0"))

            Case Else
                DisplayText("Unknown request ID: " + data.dwRequestID.ToString())
        End Select

    End Sub


    Private Sub closeConnection()

        If p3d_simconnect IsNot Nothing Then
            p3d_simconnect.Dispose()
            p3d_simconnect = Nothing
            DisplayText("Connection closed")
        End If

    End Sub
    Private Sub ButtonDisconnect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonDisconnect.Click

        closeConnection()
        SetButtons(True, False, False)

    End Sub

    Private Sub ButtonRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonRequest.Click

        p3d_simconnect.RequestDataOnSimObjectType(DataRequests.Request_1, StructDefinitions.Struct1, 0, LockheedMartin.Prepar3D.SimConnect.SIMCONNECT_SIMOBJECT_TYPE.USER)
        DisplayText("Data request sent ...")

    End Sub

    Private Sub DisplayText(ByVal text As String)

        Dim output As String

        output = line.ToString() + ": " + text + Environment.NewLine + RichResponses.Text

        RichResponses.Text = output

        line += 1

    End Sub
    ' Simconnect client will send a win32 message when there is 
    ' a packet to process. ReceiveMessage must be called to
    ' trigger the events. This model keeps simconnect processing on the main thread.
    Protected Overrides Sub DefWndProc(ByRef m As Message)

        If m.Msg = WM_USER_SIMCONNECT Then

            If p3d_simconnect IsNot Nothing Then
                p3d_simconnect.ReceiveMessage()

            End If
        Else
            MyBase.DefWndProc(m)
        End If

    End Sub

End Class