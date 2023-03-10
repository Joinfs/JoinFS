Imports JoinFS_DotNet.Functions
Imports JoinFS_DotNet.P3Dv5SimCon
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
Imports Newtonsoft.Json.Linq
Public Class ClientList
    Private Sub ClientList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        listAircraft()
        ClientListTimer.Enabled = True

    End Sub
    Public Shared Function listAircraft()
        ClientList.DataGridView1.Rows.Clear()
        If JoinFsMain.NetTimer.Enabled = False Then
            Dim latitude = My.Settings.SimLatitude
            Dim longitude = My.Settings.SimLongitude
            Dim altitude = My.Settings.SimAltitude
            Dim nickname = My.Settings.Nickname
            Dim va = My.Settings.PreferedAirline
            ClientList.DataGridView1.Rows.Add(New String() {"yes", Nickname, VA, latitude, longitude, altitude})

        Else
            Dim mylat = My.Settings.SimLatitude
            Dim mylong = My.Settings.SimLongitude
            Dim myaltitude = My.Settings.SimAltitude
            Dim mynickname = My.Settings.Nickname
            Dim myva = My.Settings.PreferedAirline
            ClientList.DataGridView1.Rows.Add(New String() {"yes", mynickname, myva, mylat, mylong, myaltitude})
            Console.WriteLine(jsonArray)
            For Each item As JObject In JoinFS_DotNet.Functions.jsonArray
                ' Get the nickname from the item and print it
                Dim nickname As String = item("nickname").ToString()
                Dim va As String = item("va").ToString()
                Dim latitude As Double = Double.Parse(item("latitude").ToString())
                Dim longitude As Double = Double.Parse(item("longitude").ToString())
                Dim altitude As Double = Double.Parse(item("altitude").ToString())
                'rest of the code
                Dim randomID As String = item("randomID").ToString()
                If (My.Settings.RandomID = randomID) Then
                Else
                    Dim distanceAway = distanceToGoCalc(mylat, mylong, latitude, longitude, "N")
                    ClientList.DataGridView1.Rows.Add(New String() {"no", nickname, va, latitude, longitude, altitude, distanceAway.ToString("#0")})
                End If


            Next
            Return "2"
        End If
    End Function

    Private Sub ClientListTimer_Tick(sender As Object, e As EventArgs) Handles ClientListTimer.Tick
        listAircraft()
    End Sub
    Public Shared Function distanceToGoCalc(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double, ByVal unit As Char) As Double
        If lat1 = lat2 And lon1 = lon2 Then
            Return 0
        Else
            Dim theta As Double = lon1 - lon2
            Dim dist As Double = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta))
            dist = Math.Acos(dist)
            dist = rad2deg(dist)
            dist = dist * 60 * 1.1515
            If unit = "K" Then
                dist = dist * 1.609344
            ElseIf unit = "N" Then
                dist = dist * 0.8684
            End If
            Return dist
        End If
    End Function

    Public Shared Function deg2rad(ByVal deg As Double) As Double
        Return (deg * Math.PI / 180.0)
    End Function

    Public Shared Function rad2deg(ByVal rad As Double) As Double
        Return rad / Math.PI * 180.0
    End Function

    Private Sub ClientList_LostFocus(sender As Object, e As EventArgs) Handles MyBase.LostFocus
        ClientListTimer.Enabled = False
    End Sub

    Private Sub ClientList_GotFocus(sender As Object, e As EventArgs) Handles MyBase.GotFocus
        ClientListTimer.Enabled = True
    End Sub

End Class