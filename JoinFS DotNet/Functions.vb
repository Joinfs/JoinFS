Imports JoinFS_DotNet.P3Dv5SimCon
Imports JoinFS_DotNet.MSFSSimCon

Imports System
Imports Microsoft.VisualBasic.FileIO.FileSystem
Imports System.IO.StreamWriter
Imports Microsoft.VisualBasic.Logging
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Net.Mime
Imports System.Net.Http
Imports Newtonsoft.Json
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Environment
Imports System.Configuration
Imports System.Xml
Imports System.Security.Cryptography
Imports Newtonsoft.Json.Linq
Imports System.Net.Http.Headers

Public Class MainVariables
    Public Shared ReadOnly versionText As String = "v4.0.0a"
    Public Shared ReadOnly supportLink As String = "https://joinfs.gg/discord"
    Public Shared ReadOnly bugLink As String = "https://github.com/Joinfs/JoinFS/issues/new"
    Public Shared ReadOnly downloadLink As String = "https://github.com/Joinfs/JoinFS/releases"
    Public Shared ReadOnly apiURL As String = "http://home.eb-it.uk:8080/JoinFS/api/client.php"
End Class
Public Class Functions
    Private Const V As Boolean = True
    Public Shared jsonArray As New JArray
    Public Shared vaJsonArray As New JArray
    Public Shared Async Function connectToSimulator() As Task
        ' Check what simulator and connect to the correct simulator function.
        If (My.Settings.Simulator = "P3Dv5") Then
            If (connectSimulator() = True) Then
                AddLogItem("Connected to P3Dv5 Simulator")
                Return
            Else
                Await API("Stop")
                AddLogItem("Failed to connect to simulator")
                Return
            End If
        End If
    End Function
    Public Shared Function AddLogItem(text As String)
        Log.RichTextBox1.AppendText(text + " at: " + System.DateTime.Now.ToString("HH:mm:ss") + vbCrLf)
        Return "logged"
    End Function
    Public Shared Function saveLog()
        Dim file As System.IO.StreamWriter
        Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
        If My.Settings.SaveLogs = True Then
            '  save logs to local C drive.
            AddLogItem("Log Saved to Data Drive")
            If Not Directory.Exists(appData + "\JoinFS") Then
                Directory.CreateDirectory(appData + "\JoinFS\")
            End If
            file = My.Computer.FileSystem.OpenTextFileWriter(appData + "\JoinFS\joinfsLog-" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".txt", True)
            file.WriteLine(Log.RichTextBox1.Text)
            file.Close()
        Else
            AddLogItem("Log Cannot be saved as disabled in File > Settings")
        End If
        AddLogItem("Deleting all but 5 latest logs from %appdata%\JoinFS\")
        DeleteOldestFiles(appData + "\JoinFS\", 5, "joinfsLog-*.txt")

        'First, create an XML document and add a root element
        Dim xmlDoc As New XmlDocument()
        Dim xmlRoot As XmlElement = xmlDoc.CreateElement("Settings")
        xmlDoc.AppendChild(xmlRoot)

        'Next, loop through each setting in My.Settings and add it to the XML document
        For Each setting As SettingsProperty In My.Settings.Properties
            Dim xmlElement As XmlElement = xmlDoc.CreateElement(setting.Name)
            xmlElement.InnerText = My.Settings(setting.Name).ToString()
            xmlRoot.AppendChild(xmlElement)
            AddLogItem("Saved XML Element: " + setting.Name + " with value: " + xmlElement.InnerText.ToString())
        Next

        'Save the XML document to a file
        xmlDoc.Save(appData + "\JoinFS\settings.xml")
        AddLogItem("Saved Settings to Settings.XML file.")
        Return Nothing
    End Function
    Public Shared Function DeleteOldestFiles(folderPath As String, filesToKeep As Integer, Optional searchPattern As String = "*.*")
        'This function delets old files from the main directory for cleaning up logs.
        Dim folder As New DirectoryInfo(folderPath)
        Dim files = folder.GetFiles(searchPattern).OrderByDescending(Function(fi) fi.CreationTime)

        For Each file In files.Skip(filesToKeep)
            file.Delete()
        Next
    End Function
    Public Shared Function RandomString()
        ' This creates a random string of characters to be used for the random code at launch and to be sent as a client identifer to the server, so we know not to duplicate the same aircraft.
        Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim r As New Random
        Dim sb As New StringBuilder
        For i As Integer = 1 To 12
            Dim idx As Integer = r.Next(0, 35)
            sb.Append(s.Substring(idx, 1))
        Next
        Return sb.ToString()
    End Function
    Public Shared Function getSettings()
        ' This function loads the settings.xml when opening the system.
        'Load the XML file into an XML document
        Dim xmlDoc As New XmlDocument()
        Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
        If My.Computer.FileSystem.FileExists(appData + "\JoinFS\settings.xml") Then

            xmlDoc.Load(appData + "\JoinFS\settings.xml")

            'Loop through each child element of the root element and set the corresponding setting in My.Settings
            For Each xmlElement As XmlElement In xmlDoc.DocumentElement.ChildNodes
                Dim settingName As String = xmlElement.Name
                Dim settingValue As String = xmlElement.InnerText
                My.Settings(settingName) = settingValue
            Next

            'Save the changes to My.Settings
            My.Settings.Save()
            DarkModeToggle()
            JoinFsMain.NickName.Text = My.Settings.Nickname
            JoinFsMain.VAList.Text = My.Settings.PreferedAirline
            Return Nothing
        Else
            JoinFsMain.VAList.Text = "GLOBAL"
            My.Settings.Nickname = "Change Me!"
            JoinFsMain.NickName.Text = My.Settings.Nickname
            My.Settings.PreferedAirline = JoinFsMain.VAList.Text
            AddLogItem("No settings file, saving currentl log to create the settings file")
            saveLog()
            Return Nothing
        End If
    End Function

    Public Shared Function BandwidthMode()
        ' This function serves as a way to limit the bandwith for those who have slow/metered connections. 
        Dim mode = My.Settings.LowBandwidth
        If mode = "Normal" Then
            AddLogItem("Bandwidth Mode changed to: Normal, Updates ever 1/2 Second.")
            JoinFsMain.SimTimer.Interval = "500"
            JoinFsMain.NetTimer.Interval = "500"
            ClientList.ClientListTimer.Interval = "500"
        ElseIf mode = "Fast" Then
            AddLogItem("Bandwidth Mode changed to: Fast, Updates ever 200 miliseconds.")
            JoinFsMain.SimTimer.Interval = "200"
            JoinFsMain.NetTimer.Interval = "200"
            ClientList.ClientListTimer.Interval = "200"
        ElseIf mode = "Slow" Then
            AddLogItem("Bandwidth Mode changed to: Slow, Updates ever 1 Second.")
            JoinFsMain.SimTimer.Interval = "1000"
            JoinFsMain.NetTimer.Interval = "1000"
            ClientList.ClientListTimer.Interval = "1000"
        ElseIf mode = "Very Slow" Then
            AddLogItem("Bandwidth Mode changed to: Very Slow, Updates ever 5 Seconds.")
            JoinFsMain.SimTimer.Interval = "5000"
            JoinFsMain.NetTimer.Interval = "5000"
            ClientList.ClientListTimer.Interval = "5000"

        End If


    End Function

    Public Shared Async Function CloseJoinFS() As Task
        ' Close JoinFS system, disconnect from the network, and save logs.
        Await API("Stop")
        AddLogItem("JoinFS System Closed")
        saveLog()
        End

    End Function
    Public Shared Function DarkModeToggle()
        Try
            If (My.Settings.DarkMode = True) Then
                JoinFsMain.MenuStrip1.BackColor = Color.FromArgb(64, 64, 64)
                JoinFsMain.MenuStrip1.ForeColor = Color.White
                JoinFsMain.BackColor = Color.FromArgb(64, 64, 64)
                JoinFsMain.ForeColor = Color.White
                JoinFsMain.StatusStrip1.BackColor = Color.DarkGray
                JoinFsMain.StatusStrip1.ForeColor = Color.White
                JoinFsMain.NickName.BackColor = Color.DarkGray
                JoinFsMain.NickName.ForeColor = Color.White
                JoinFsMain.VAList.BackColor = Color.DarkGray
                JoinFsMain.VAList.ForeColor = Color.White
                Functions.AddLogItem("Set Theme to Dark Mode")
            Else
                JoinFsMain.MenuStrip1.BackColor = Color.Empty
                JoinFsMain.MenuStrip1.ForeColor = Color.Black
                JoinFsMain.BackColor = Color.Empty
                JoinFsMain.ForeColor = Color.White
                JoinFsMain.StatusStrip1.BackColor = Color.Empty
                JoinFsMain.StatusStrip1.ForeColor = Color.Black
                JoinFsMain.NickName.BackColor = Color.Empty
                JoinFsMain.NickName.ForeColor = Color.Black
                JoinFsMain.VAList.BackColor = Color.Empty
                JoinFsMain.VAList.ForeColor = Color.Black
                Functions.AddLogItem("Set Theme to Normal Mode")
            End If
            Return True
        Catch Ex As Exception
            Return False
            MessageBox.Show(Ex.Message)
        End Try
    End Function
    Public Shared Async Function NetworkManager() As Task
        If (JoinFsMain.NetTimer.Enabled = False) Then
            JoinFsMain.NetTimer.Enabled = True
            JoinFsMain.Button4.BackColor = Color.Orange
            AddLogItem("Network Connected")
        Else
            Await API("Stop")
            JoinFsMain.NetTimer.Enabled = False
            JoinFsMain.Button4.BackColor = Color.Red
            AddLogItem("Network Disconnected")
        End If
        Return
    End Function
    Public Shared Async Function API(method As String) As Task(Of String)
        ' API REQUESTS Check what method it is.

        If method = "POST" Then
            Dim client As New HttpClient()
            Dim uri = MainVariables.apiURL
            Dim nickname = My.Settings.Nickname
            If nickname = "" Then
                nickname = "NickNamenotset"
            End If
            Dim va = My.Settings.PreferedAirline
            Dim data As New Dictionary(Of String, String) From {
    {"nickname", nickname},
    {"va", va},
                {"SimBriefUID", My.Settings.SimBriefUID},
    {"longitude", My.Settings.SimLongitude},
    {"latitude", My.Settings.SimLatitude},
    {"altitude", My.Settings.SimAltitude},
    {"title", My.Settings.SimPlaneTitle},
    {"speed", My.Settings.PlaneGSpeed},
            {"aspeed", My.Settings.PlaneIndicatedAirSpeed},
                    {"pitch", My.Settings.PlanePitch},
                        {"bank", My.Settings.PlaneBank},
            {"simulator", My.Settings.Simulator},
    {"heading", My.Settings.SimHeading},
        {"RandomID", My.Settings.RandomID},
    {"active", "True"}
}
            Dim data2 As String = JsonConvert.SerializeObject(data)
            If My.Settings.EnhancedLogs = True Then
                AddLogItem("Sending to API Server: " + data2)
            End If

            Try
                Dim content = New StringContent(data2, System.Text.Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(uri, content)
                Dim status = response.StatusCode.ToString()
                If My.Settings.EnhancedLogs = True Then
                    AddLogItem("API Response Status Code: " + status)
                End If
                If (status = "NotFound") Then
                    JoinFsMain.NetTimer.Enabled = False
                    JoinFsMain.Button4.BackColor = Color.Red
                    client.Dispose()
                    response.Dispose()
                    content.Dispose()
                    Return "404"
                ElseIf (status = "ServerError") Then
                    JoinFsMain.NetTimer.Enabled = False
                    JoinFsMain.Button4.BackColor = Color.Red
                    Return "500"
                ElseIf (status = "OK") Then
                    If JoinFsMain.Button4.BackColor = Color.Green Then
                    Else
                        JoinFsMain.Button4.BackColor = Color.Green
                    End If
                    If My.Settings.EnhancedLogs = True Then
                        AddLogItem("Response:" + response.ToString())
                    End If
                    client.Dispose()
                    response.Dispose()
                    content.Dispose()
                    Return "200"
                Else
                    client.Dispose()
                    response.Dispose()
                    content.Dispose()
                    JoinFsMain.Button4.BackColor = Color.Red
                    JoinFsMain.NetTimer.Enabled = False
                    Return "100"
                End If

            Catch ex As Exception
                AddLogItem("Error in API POST: " + ex.Message)
                JoinFsMain.NetTimer.Enabled = False
                JoinFsMain.Button4.BackColor = Color.Red
            End Try
        ElseIf method = "GET" Then
            RetrieveJsonData()
        ElseIf method = "Stop" Then
            Dim nickname = My.Settings.Nickname
            If nickname = "" Then
                nickname = "NickNamenotset"
            End If
            Dim va = My.Settings.PreferedAirline
            Dim uri = MainVariables.apiURL
            Dim data As New Dictionary(Of String, String) From {
    {"nickname", nickname},
    {"va", va},
            {"SimBriefUID", My.Settings.SimBriefUID},
    {"longitude", My.Settings.SimLongitude},
    {"latitude", My.Settings.SimLatitude},
    {"altitude", My.Settings.SimAltitude},
    {"title", My.Settings.SimPlaneTitle},
        {"speed", My.Settings.PlaneGSpeed},
        {"aspeed", My.Settings.PlaneIndicatedAirSpeed},
                {"pitch", My.Settings.PlanePitch},
                        {"bank", My.Settings.PlaneBank},
        {"simulator", My.Settings.Simulator},
        {"heading", My.Settings.SimHeading},
                {"RandomID", My.Settings.RandomID},
    {"active", "False"}
}
            Dim data2 As String = JsonConvert.SerializeObject(data)
            AddLogItem("Sending Disconnect to API Server.")
            Try
                Dim client As New HttpClient()
                Dim content = New StringContent(data2, System.Text.Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(uri, content)
                Dim status = response.StatusCode.ToString()
                If My.Settings.EnhancedLogs = True Then
                    AddLogItem("API Response Status Code: " + status)
                End If
                If (status = "NotFound") Then
                    JoinFsMain.NetTimer.Enabled = False
                    JoinFsMain.Button4.BackColor = Color.Red
                    client.Dispose()
                    response.Dispose()
                    content.Dispose()
                    Return "404"
                ElseIf (status = "ServerError") Then
                    JoinFsMain.NetTimer.Enabled = False
                    JoinFsMain.Button4.BackColor = Color.Red
                    Return "500"
                ElseIf (status = "OK") Then
                    If My.Settings.EnhancedLogs = True Then
                        AddLogItem("Response:" + response.ToString())
                    End If
                    client.Dispose()
                    response.Dispose()
                    content.Dispose()
                    JoinFsMain.NetTimer.Enabled = False
                    JoinFsMain.Button4.BackColor = Color.Red
                    Return "200"
                Else
                    client.Dispose()
                    response.Dispose()
                    content.Dispose()
                    JoinFsMain.Button4.BackColor = Color.Red
                    JoinFsMain.NetTimer.Enabled = False
                    Return "100"
                End If
                Return "200"
            Catch ex As Exception
                AddLogItem("Error in API POST: " + ex.Message)
                JoinFsMain.NetTimer.Enabled = False
                JoinFsMain.Button4.BackColor = Color.Red
                Return "500"
            End Try
        End If
        Return Nothing
    End Function
    Public Shared Sub RetrieveJsonData()
        ' Try
        Dim client As New HttpClient()
            Dim uri = MainVariables.apiURL
            Dim queryString As String = "?data=clients&va=" & My.Settings.PreferedAirline.ToString()
            Dim completeUrl As String = uri & queryString
            client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", APISecurity.getApiJWT())
            Dim response As HttpResponseMessage = client.GetAsync(completeUrl).Result
            If response.IsSuccessStatusCode Then
                'Read the response content as a string
                Dim responseContent As String = response.Content.ReadAsStringAsync().Result
                '//AddLogItem(responseContent)
                'Parse the JSON response string into a .NET object
                jsonArray = JArray.Parse(responseContent)
                ' 
                For Each item As JObject In jsonArray
                ' Get the nickname from the item and print it
                If item("randomID").ToString() = My.Settings.RandomID Then
                    ' DO NOT ADD OWN AIRCRAFT

                Else
                    Dim alt = item("altitude").ToString()
                    Dim lat = item("latitude").ToString()
                    Dim lng = item("longitude").ToString()
                    Dim speed = item("airspeed").ToString()
                    Dim pitch = item("pitch").ToString()
                    Dim bank = item("bank").ToString()
                    Dim heading = item("heading").ToString()

                    Dim callsign As String = item("nickname").ToString()
                    callsign = callsign.Substring(0, Math.Min(callsign.ToString().Length, 5))
                    AddLogItem(speed)
                    Dim ground = 1
                    If speed > 180 Then
                        ground = 1
                    End If

                    addOtherAircraft(lat, lng, alt, pitch, bank, heading, speed, ground, callsign)
                    'AddLogItem("test 1")
                    ' Get the image from the url on the database

                End If
                Next

                'Loop through the result array in the JSON object and add each value to the result array
            ElseIf response.StatusCode = "401" Then
                MessageBox.Show("Your Client version is currently invalid, please update!")
                End
                AddLogItem("Error Getting Json Content of Client List")
                'API call was unsuccessful, handle the error here
            End If

        ' Catch ex As Exception
        'AddLogItem(" test: " + ex.Message)
        ' End Try
    End Sub
    Public Shared Sub RetriveVAList()
        Try
            Dim VaClient As New HttpClient()
            Dim uri = MainVariables.apiURL
            Dim queryString As String = "?data=valist"
            Dim completeUrl As String = uri & queryString
            VaClient.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", APISecurity.getApiJWT())
            Dim response2 As HttpResponseMessage = VaClient.GetAsync(completeUrl).Result
            If response2.IsSuccessStatusCode Then
                Dim responseContent As String = response2.Content.ReadAsStringAsync().Result
                vaJsonArray = JArray.Parse(responseContent)
            ElseIf response2.StatusCode = "401" Then
                MessageBox.Show("Your Client version is currently invalid, please update!")
                End

            End If
        Catch ex As Exception
            AddLogItem(ex.Message)
            Console.WriteLine(ex.Message)
        End Try
    End Sub
End Class
