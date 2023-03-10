Attribute VB_Name = "General"
' ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
'   Visual Basic <-> FSUIPC/WideFS Communication                 Version 1.004
'   Copyright © 2000 Chris Brett.  All rights reserved.
'   e-mail: chris@formulate.clara.net
'
'   FUNCTION LIBRARY FOR FSUIPC
'   based on C code supplied by Pete Dowson
'
'   PLEASE NOTE:
'     Visual Basic is not my primary programming language.  There may well be
'     syntax or other inefficiencies in this port of the code.  If you find any,
'     please kindly contact me so that I may correct them for future versions.
'     This code was ported to Visual Basic 5, and has not been tested in any
'     other VB version.
' ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Option Explicit

Public ResultText, SimulationText

Sub Main()
  Dim dwResult As Long
  
  ' set VB "constant" :-) string refs dynamically now ready for result messages
  ResultText = Array("Okay", _
                    "Attempt to Open when already Open", _
                    "Cannot link to FSUIPC or WideClient", _
                    "Failed to Register common message with Windows", _
                    "Failed to create Atom for mapping filename", _
                    "Failed to create a file mapping object", _
                    "Failed to open a view to the file map", _
                    "Incorrect version of FSUIPC, or not FSUIPC", _
                    "Sim is not version requested", _
                    "Call cannot execute, link not Open", _
                    "Call cannot execute: no requests accumulated", _
                    "IPC timed out all retries", _
                    "IPC sendmessage failed all retries", _
                    "IPC request contains bad data", _
                    "Maybe running on WideClient, but FS not running on Server, or wrong FSUIPC", _
                    "Read or Write request cannot be added, memory for Process is full")
  SimulationText = Array("Any", "FS98", "FS2K", "CFS2", "CFS1", "FLY", "FS2002", "FSNEXT", "FSYETANOTHER")

  ' initialize important vars for UIPC comms - call only once!
  FSUIPC_Initialization
  
  ' Try to connect to FSUIPC (or WideFS)
  If FSUIPC_Open(SIM_ANY, dwResult) Then
  
    ' Connected to simulator (visible in the form's caption)
    frmMain.Caption = frmMain.Caption & ResultText(dwResult)
    
    ' Show what simulator we are connected to
    frmMain.lblSim.Caption = SimulationText(FSUIPC_FS_Version)
    
    ' Show version of FSUIPC
    frmMain.lblVer.Caption = Chr(Asc("0") + (&HF And (Shift(FSUIPC_Version, 28, RightShift)))) & _
                               "." & _
                               Chr(Asc("0") + (&HF And (Shift(FSUIPC_Version, 24, RightShift)))) & _
                               Chr(Asc("0") + (&HF And (Shift(FSUIPC_Version, 20, RightShift)))) & _
                               Chr(Asc("0") + (&HF And (Shift(FSUIPC_Version, 16, RightShift))))
                               
    ' lo order word for _version holds build letter
    If (FSUIPC_Version And &HFFFF&) <> 0 Then
      frmMain.lblVer.Caption = frmMain.lblVer.Caption & Chr(Asc("a") + (FSUIPC_Version And &HFF) - 1)
    End If
        
    ' Enable the timer which is actually reading the FS clock time
    frmMain.Timer1.Enabled = True
    
  Else
  
    ' Unable to "Connect"
    frmMain.Caption = frmMain.Caption & "Can't connect: " & ResultText(dwResult)
    
  End If
  
  frmMain.Show
End Sub



