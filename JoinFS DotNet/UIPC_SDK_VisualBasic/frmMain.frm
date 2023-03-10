VERSION 5.00
Begin VB.Form frmMain 
   Caption         =   "UIPC Hello: "
   ClientHeight    =   1890
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4095
   LinkTopic       =   "Form1"
   ScaleHeight     =   1890
   ScaleWidth      =   4095
   StartUpPosition =   2  'CenterScreen
   Begin VB.Timer Timer1 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   3480
      Top             =   1320
   End
   Begin VB.CommandButton butOK 
      Caption         =   "OK"
      Height          =   375
      Left            =   1440
      TabIndex        =   6
      Top             =   1320
      Width           =   1215
   End
   Begin VB.Label lblClock 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "N/A"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   3495
      TabIndex        =   5
      Top             =   960
      Width           =   360
   End
   Begin VB.Label lblVer 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "N/A"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   3495
      TabIndex        =   4
      Top             =   600
      Width           =   360
   End
   Begin VB.Label lblSim 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      Caption         =   "N/A"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   3495
      TabIndex        =   3
      Top             =   240
      Width           =   360
   End
   Begin VB.Label Label3 
      AutoSize        =   -1  'True
      Caption         =   "FS Clock:"
      Height          =   195
      Left            =   120
      TabIndex        =   2
      Top             =   960
      Width           =   690
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      Caption         =   "FSUIPC Version:"
      Height          =   195
      Left            =   120
      TabIndex        =   1
      Top             =   600
      Width           =   1185
   End
   Begin VB.Label Label1 
      AutoSize        =   -1  'True
      Caption         =   "Sim is:"
      Height          =   195
      Left            =   120
      TabIndex        =   0
      Top             =   240
      Width           =   450
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
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

Private Sub butOK_Click()
  Unload frmMain
End Sub

Private Sub Form_Unload(Cancel As Integer)
  ' close connection to UIPC
  FSUIPC_Close
End Sub

Private Sub Timer1_Timer()
Dim dwResult As Long
Dim auiTime() As Byte
ReDim auiTime(3)
  ' As an example of retrieving data, we will get the FS clock time.
  ' If we wanted additional reads/writes at the same time, we could put them here
  If FSUIPC_Read(&H238, 3, VarPtr(auiTime(1)), dwResult) Then
    ' "Read" proceeded without any problems
    If FSUIPC_Process(dwResult) Then
      ' "Process" proceeded without any problems
      lblClock.Caption = Format(auiTime(1), "00") & ":" & _
                         Format(auiTime(2), "00") & ":" & _
                         Format(auiTime(3), "00")
    Else
      ' Unable to "Process"
      lblClock.Caption = "Processing: " & ResultText(dwResult)
    End If
  Else
    ' Unable to "Read"
    lblClock.Caption = "Reading: " & ResultText(dwResult)
  End If
End Sub
