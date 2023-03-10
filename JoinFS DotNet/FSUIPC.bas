Attribute VB_Name = "FSUIPC"
' ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
'   Visual Basic <-> FSUIPC/WideFS Communication                 Version 1.005
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
'
'   BASIC USAGE GOES LIKE THIS:
'     - call FSUIPC_Initialization() to initialize important vars at beginning
'       of program.
'     - call FSUIPC_Open() to connect with FSUIPC if it's available.
'     - call a series of FSUIPC_Read and/or FSUIPC_Write and/or FSUIPC_WriteS (be careful when
'       combining these in the one transaction, for your own sanity).
'     - call FSUIPC_Process() to actually get/set the data from/to FS.
'     - repeat steps 3 and 4 as necessary
'     - at program termination, call FSUIPC_Close().
'
' ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Option Explicit


' **************************************************************************************
' ***************  EXTERNAL PROCEDURE REFERENCES ***************************************
' **************************************************************************************
Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" _
        (Destination As Any, Source As Any, ByVal Length As Long)
 
Declare Sub ZeroMemory Lib "kernel32" Alias "RtlZeroMemory" _
        (Destination As Any, ByVal Length As Long)

Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" _
        (ByVal hWnd1 As Long, ByVal hWnd2 As Long, ByVal lpsz1 As String, _
        ByVal lpsz2 As String) As Long

Declare Function RegisterWindowMessage& Lib "user32" Alias _
        "RegisterWindowMessageA" (ByVal lpString As String)

Declare Function GetCurrentProcessId& Lib "kernel32" ()

Declare Function GlobalAddAtom% Lib "kernel32" Alias _
        "GlobalAddAtomA" (ByVal lpString As String)

Declare Function CreateFileMapping& Lib "kernel32" Alias _
        "CreateFileMappingA" (ByVal hFile As Long, _
        ByVal lpFileMappingAttributes As Long, ByVal _
        flProtect As Long, ByVal dwMaximumSizeHigh As Long, _
        ByVal dwMaximumSizeLow As Long, ByVal lpName As String)

Declare Function GetLastError& Lib "kernel32" ()

Declare Function MapViewOfFile& Lib "kernel32" (ByVal hFileMappingObject As _
        Long, ByVal dwDesiredAccess As Long, ByVal dwFileOffsetHigh _
        As Long, ByVal dwFileOffsetLow As Long, ByVal _
        dwNumberOfBytesToMap As Long)

Declare Function MapViewOfFileEx& Lib "kernel32" _
        (ByVal hFileMappingObject As _
        Long, ByVal dwDesiredAccess As Long, ByVal dwFileOffsetHigh _
        As Long, ByVal dwFileOffsetLow As Long, ByVal _
        dwNumberOfBytesToMap As Long, lpBaseAddress As Any)

Declare Function GlobalDeleteAtom% Lib "kernel32" (ByVal nAtom As _
        Integer)

Declare Function UnmapViewOfFile& Lib "kernel32" _
        (ByVal lpBaseAddress As Long)

Declare Function CloseHandle& Lib "kernel32" (ByVal hObject As Long)

Declare Function SendMessageTimeout& Lib "user32" Alias _
        "SendMessageTimeoutA" (ByVal hWnd As Long, ByVal Msg As Long, _
        ByVal wParam As Long, ByVal lparam As Long, ByVal fuFlags As _
        Long, ByVal uTimeout As Long, lpdwResult As Long)
        

Public Const SMTO_BLOCK = &H1
Public Const PAGE_READWRITE = 4&
Public Const NO_ERROR = 0
Public Const ERROR_ALREADY_EXISTS = 183&
Public Const SECTION_MAP_WRITE = &H2
Public Const FILE_MAP_WRITE = SECTION_MAP_WRITE


' **************************************************************************************
' ************************** FSUIPC SPECIFICS ******************************************
' **************************************************************************************

' Supported Sims
Public Const SIM_ANY = 0
Public Const SIM_FS98 = 1
Public Const SIM_FS2K = 2
Public Const SIM_CFS2 = 3
Public Const SIM_CFS1 = 4
Public Const SIM_FLY = 5
Public Const SIM_FS2K2 = 6
Public Const SIM_FS2K4 = 7
Public Const SIM_FSX = 8
Public Const SIM_ESP = 9

' Error numbers
Public Const FSUIPC_ERR_OK = 0
Public Const FSUIPC_ERR_OPEN = 1              ' Attempt to Open when already Open
Public Const FSUIPC_ERR_NOFS = 2              ' Cannot link to FSUIPC or WideClient
Public Const FSUIPC_ERR_REGMSG = 3            ' Failed to Register common message with Windows
Public Const FSUIPC_ERR_ATOM = 4              ' Failed to create Atom for mapping filename
Public Const FSUIPC_ERR_MAP = 5               ' Failed to create a file mapping object
Public Const FSUIPC_ERR_VIEW = 6              ' Failed to open a view to the file map
Public Const FSUIPC_ERR_VERSION = 7           ' Incorrect version of FSUIPC, or not FSUIPC
Public Const FSUIPC_ERR_WRONGFS = 8           ' Sim is not version requested
Public Const FSUIPC_ERR_NOTOPEN = 9           ' Call cannot execute, link not Open
Public Const FSUIPC_ERR_NODATA = 10           ' Call cannot execute: no requests accumulated
Public Const FSUIPC_ERR_TIMEOUT = 11          ' IPC timed out all retries
Public Const FSUIPC_ERR_SENDMSG = 12          ' IPC sendmessage failed all retries
Public Const FSUIPC_ERR_DATA = 13             ' IPC request contains bad data
Public Const FSUIPC_ERR_RUNNING = 14          ' Maybe running on WideClient, but FS not running on Server, or wrong FSUIPC
Public Const FSUIPC_ERR_SIZE = 15             ' Read or Write request cannot be added, memory for Process is full


' define the DWORD type for VB
Public Type DWORD
  Byt0 As Byte
  Byt1 As Byte
  Byt2 As Byte
  Byt3 As Byte
End Type


' global variables for the UIPC communication
Public FSUIPC_Version As Long
Public FSUIPC_FS_Version As Long
Public FSUIPC_Lib_Version As Long
Public m_hWnd&              ' FS window handle
Public m_msg&               ' ID of registered window message
Public m_atom As Long       ' global atom containing name of file-mapping object
Public m_hMap&              ' handle of file-mapping object
Public m_pView&             ' pointer to view of file-mapping object
Public m_pNext&



Public Const LIB_VERSION = 2004              ' 2.004
Public Const MAX_SIZE = &H7F00&              ' Largest data (kept below 32k to avoid
                                             ' any possible 16-bit sign problems
                                         
Public Const FS6IPC_MSGNAME1 = "FSASMLIB:IPC"
Public Const FS6IPC_MSGNAME2 = "EFISFSCOM:IPC"
Public Const FS6IPC_MESSAGE_SUCCESS = 1
Public Const FS6IPC_MESSAGE_FAILURE = 0
' IPC message types
Public Const FS6IPC_READSTATEDATA_ID = 1
Public Const FS6IPC_WRITESTATEDATA_ID = 2
Public Const FS6IPC_SPECIALREQUEST_ID = &HABAC


  
' declare the record types for reading and writing comms with UIPC
Public Type FS6IPC_READSTATEDATA_HDR
  dwId As Long
  dwOffset As Long
  nBytes As Long
  pDest As Long
End Type

Public Type FS6IPC_WRITESTATEDATA_HDR
  dwId As Long
  dwOffset As Long
  nBytes As Long
End Type

'--- Stop the Client ---------------------------------------------------------
Sub FSUIPC_Close()

  m_hWnd = 0
  m_msg = 0
  m_pNext& = 0

  If (m_atom <> 0) Then
    GlobalDeleteAtom (m_atom)
    m_atom = 0
  End If
    
  If (m_pView& <> 0) Then
    UnmapViewOfFile (m_pView&)
    m_pView& = 0
  End If
    
  If (m_hMap <> 0) Then
    CloseHandle (m_hMap)
    m_hMap = 0
  End If
    
End Sub

Sub FSUIPC_Initialization()
  m_hWnd = 0
  m_msg = 0
  m_atom = 0
  m_hMap = 0
  m_pView& = 0
  m_pNext& = 0
End Sub


'--- Start the Client --------------------------------------------------------
' returns TRUE if successful, FALSE otherwise,
' if FALSE dwResult contains the "error-code"
Function FSUIPC_Open(dwFSReq As Long, ByRef dwResult As Long) As Boolean
Dim szName As String * 24
Dim fWideFS As Boolean
Dim nTry    As Integer
Dim i       As Integer

  ' initialize vars
  nTry = 0
  fWideFS = False
  i = 0
  
  ' abort if already started
  If (m_pView& <> 0) Then
    dwResult = FSUIPC_ERR_OPEN
    FSUIPC_Open = False
    Exit Function
  End If
    
  FSUIPC_Version = 0
  FSUIPC_FS_Version = 0
  
  ' Connect via FSUIPC, which is known to be FSUIPC's own
  ' and isn't subject to user modification
  m_hWnd = FindWindowEx(0&, 0&, "UIPCMAIN", vbNullString)
  If (m_hWnd = 0) Then
    ' If there's no UIPCMAIN, we may be using WideClient
    ' which only simulates FS98
    m_hWnd = FindWindowEx(0&, 0&, "FS98MAIN", vbNullString)
    fWideFS = True
    If (m_hWnd = 0) Then
      dwResult = FSUIPC_ERR_NOFS
      FSUIPC_Open = False
      Exit Function
    End If
  End If
  
  ' register the window message
  m_msg = RegisterWindowMessage(FS6IPC_MSGNAME1)
  If (m_msg = 0) Then
    dwResult = FSUIPC_ERR_REGMSG
    FSUIPC_Open = False
    Exit Function
  End If
  
  ' create the name of our file-mapping object
  nTry = nTry + 1 ' Ensures a unique string is used in case user closes and reopens
  szName = FS6IPC_MSGNAME1 & ":" & Hex(GetCurrentProcessId) & ":" & Hex(nTry) & Chr(0)
    
  ' stuff the name into a global atom
  m_atom = GlobalAddAtom(ByVal szName)
  If (m_atom = 0) Then
    dwResult = FSUIPC_ERR_ATOM
    FSUIPC_Close
    FSUIPC_Open = False
    Exit Function
  End If
  
  ' create the file-mapping object
                             ' use system paging file
                                         ' security
                                             'protection
                                                                 ' size
                                                                                 ' name
  m_hMap = CreateFileMapping(&HFFFFFFFF, 0&, PAGE_READWRITE, 0&, MAX_SIZE + 256, szName)

  If (m_hMap = Null) Or (GetLastError = ERROR_ALREADY_EXISTS) Then
    dwResult = FSUIPC_ERR_MAP
    FSUIPC_Close
    FSUIPC_Open = False
    Exit Function
  End If
    
  ' get a view of the file-mapping object
  m_pView& = MapViewOfFile(m_hMap, FILE_MAP_WRITE, 0&, 0&, 0&)
  If m_pView& = 0 Then
    dwResult = FSUIPC_ERR_VIEW
    FSUIPC_Close
    FSUIPC_Open = False
    Exit Function
  End If
  
  ' Okay, now determine FSUIPC version AND FS type
  m_pNext& = m_pView&
    
  ' Try up to 5 times with a 100msec rest between each
  ' Note that WideClient returns zeros initially, whilst waiting
  ' for the Server to get the data
  While ((i < 5) And ((FSUIPC_Version = 0) Or (FSUIPC_FS_Version = 0)))
    i = i + 1
    ' Read FSUIPC version
    If (Not FSUIPC_Read(&H3304, 4, VarPtr(FSUIPC_Version), dwResult)) Then
      FSUIPC_Close
      FSUIPC_Open = False
      Exit Function
    End If

    ' and FS version and validity check pattern
    If (Not FSUIPC_Read(&H3308, 4, VarPtr(FSUIPC_FS_Version), dwResult)) Then
      FSUIPC_Close
      FSUIPC_Open = False
      Exit Function
    End If

    ' write our Library version number to a special read-only offset
    ' This is to assist diagnosis from FSUIPC logging
    ' But only do this on first try
    If (i < 2) And (Not FSUIPC_Write(&H330A, 2, VarPtr(FSUIPC_Lib_Version), dwResult)) Then
      FSUIPC_Close
      FSUIPC_Open = False
      Exit Function
    End If

    ' Actually send the request and get the responses ("process")
    If Not (FSUIPC_Process(dwResult)) Then
      FSUIPC_Close
      FSUIPC_Open = False
      Exit Function
    End If

    ' Maybe running on WideClient, and need another try
    Call Sleep(100) ' Give it a chance
  Wend

  ' Only allow running on FSUIPC 1.998e or later
  ' with correct check pattern &HFADE
  If ((FSUIPC_Version < &H19980005) Or ((FSUIPC_FS_Version And &HFFFF0000) <> &HFADE0000)) Then
    If fWideFS Then dwResult = FSUIPC_ERR_RUNNING Else dwResult = FSUIPC_ERR_VERSION
    FSUIPC_Close
    FSUIPC_Open = False
    Exit Function
  End If

  ' grab the FS version number
  FSUIPC_FS_Version = (FSUIPC_FS_Version And &HFFFF&)
  
  ' Optional version-specific request made?  If so and wrong version, return error
  If (dwFSReq <> 0) And (dwFSReq <> FSUIPC_FS_Version) Then
    dwResult = FSUIPC_ERR_WRONGFS
    FSUIPC_Close
    FSUIPC_Open = False
    Exit Function
  End If
  
  dwResult = FSUIPC_ERR_OK
  FSUIPC_Open = True
End Function

'--- Read request ---------------------------------------------------------------
Function FSUIPC_Read(dwOffset As Long, dwSize As Long, pDest As Long, ByRef dwResult As Long) As Boolean
Dim pHdr As FS6IPC_READSTATEDATA_HDR

  ' Check link is open
  If (m_pView& = 0) Then
    dwResult = FSUIPC_ERR_NOTOPEN
    FSUIPC_Read = False
    Exit Function
  End If

  ' Check have space for this request (including terminator)
  If ((((m_pNext&) - (m_pView&)) + 4 + (dwSize + Len(pHdr))) > MAX_SIZE) Then
    dwResult = FSUIPC_ERR_SIZE
    FSUIPC_Read = False
    Exit Function
  End If

  ' Initialise header for read request
  pHdr.dwId = FS6IPC_READSTATEDATA_ID
  pHdr.dwOffset = dwOffset
  pHdr.nBytes = dwSize
  pHdr.pDest = pDest
  CopyMemory ByVal m_pNext&, pHdr, Len(pHdr)

  ' Move pointer past the Record
  m_pNext& = m_pNext& + Len(pHdr)
  If (dwSize <> 0) Then
    ' Zero the reception area, so rubbish won't be returned
    Call ZeroMemory(ByVal m_pNext&, dwSize)
    ' Update the pointer ready for more data
    m_pNext& = m_pNext& + dwSize
  End If
  
  dwResult = FSUIPC_ERR_OK
  FSUIPC_Read = True
End Function

'--- Process read/write ------------------------------------------------------
Function FSUIPC_Process(dwResult As Long) As Boolean
Dim dwError As Long
Dim pdw As Integer
Dim pHdrR As FS6IPC_READSTATEDATA_HDR
Dim pHdrW As FS6IPC_WRITESTATEDATA_HDR
Dim i As Integer

  i = 0

  If (m_pView& = 0) Then
    dwResult = FSUIPC_ERR_NOTOPEN
    FSUIPC_Process = False
    Exit Function
  End If

  If (m_pView& = m_pNext&) Then
    dwResult = FSUIPC_ERR_NODATA
    FSUIPC_Process = False
    Exit Function
  End If

  Call ZeroMemory(ByVal m_pNext&, 4) ' Terminator
  m_pNext& = m_pView&

 ' send the request (allow up to 9 tries)
 While (i < 10) And ((SendMessageTimeout(m_hWnd, m_msg, m_atom, 0&, SMTO_BLOCK, 2000, dwError)) = 0)
 '                         m_hWnd,              // FS6 window handle
 '                         m_msg,               // our registered message id
 '                         m_atom,              // wParam: name of file-mapping object
 '                         0,                   // lParam: offset of request into file-mapping obj
 '                         SMTO_BLOCK,          // halt this thread until we get a response
 '                         2000,                // time out interval
 '                         dwError)) = 0) do begin  // return value
    i = i + 1
    Sleep (100) ' Allow for things to happen
  Wend

  If (i >= 10) Then   ' Failed all tries
    If GetLastError = 0 Then dwResult = FSUIPC_ERR_TIMEOUT Else dwResult = FSUIPC_ERR_SENDMSG
    FSUIPC_Process = False
    Exit Function
  End If

  ' did IPC like the data request?
  If (dwError <> FS6IPC_MESSAGE_SUCCESS) Then
    ' no...
    dwResult = FSUIPC_ERR_DATA
    FSUIPC_Process = False
    Exit Function
  End If

  ' Decode and store results of Read requests
  m_pNext& = m_pView&
  Call CopyMemory(pdw, ByVal m_pNext&, Len(pdw))

  While pdw <> 0
    Select Case pdw
        Case FS6IPC_READSTATEDATA_ID
          ' copy the data we read into the user's buffer
          CopyMemory pHdrR, ByVal m_pNext&, Len(pHdrR)
          m_pNext& = m_pNext& + Len(pHdrR)
          If (pHdrR.pDest <> 0) And (pHdrR.nBytes <> 0) Then
            CopyMemory ByVal pHdrR.pDest, ByVal m_pNext&, pHdrR.nBytes
          End If
          m_pNext& = m_pNext& + pHdrR.nBytes
            
        Case FS6IPC_WRITESTATEDATA_ID
          ' This is a write, so there's no returned data to store
          CopyMemory pHdrW, ByVal m_pNext&, Len(pHdrW)
          m_pNext& = m_pNext& + Len(pHdrW) + pHdrW.nBytes
              
        Case Else
          ' Error! So terminate the scan
          m_pNext& = m_pView&
          Exit Function
          
    End Select
    
    CopyMemory pdw, ByVal m_pNext&, Len(pdw)
  Wend

  m_pNext& = m_pView&
  dwResult = FSUIPC_ERR_OK
  FSUIPC_Process = True
End Function

'--- Write request ------------------------------------------------------
Function FSUIPC_Write(dwOffset As Long, dwSize As Long, pSrce As Long, ByRef dwResult As Long) As Boolean
Dim pHdr As FS6IPC_WRITESTATEDATA_HDR

  ' abort if necessary
  If (m_pView& = 0) Then
    dwResult = FSUIPC_ERR_NOTOPEN
    FSUIPC_Write = False
    Exit Function
  End If

  ' Check have space for this request (including terminator)
  If ((((m_pNext&) - (m_pView&)) + 4 + (dwSize + Len(pHdr))) > MAX_SIZE) Then
    dwResult = FSUIPC_ERR_SIZE
    FSUIPC_Write = False
    Exit Function
  End If

  ' Initialise header for write request
  pHdr.dwId = FS6IPC_WRITESTATEDATA_ID
  pHdr.dwOffset = dwOffset
  pHdr.nBytes = dwSize
  CopyMemory ByVal m_pNext&, pHdr, Len(pHdr)

  ' Move pointer past the record
  m_pNext& = m_pNext& + Len(pHdr)
  If (dwSize <> 0) Then
    ' Copy in the data to be written
    CopyMemory ByVal m_pNext&, ByVal pSrce, dwSize
    ' Update the pointer ready for more data
    m_pNext& = m_pNext& + dwSize
  End If

  dwResult = FSUIPC_ERR_OK
  FSUIPC_Write = True
End Function

'--- Null Terminated String Write request ------------------------------------------------------
Function FSUIPC_WriteS(dwOffset As Long, dwSize As Long, ByVal pSrce As String, ByRef dwResult As Long) As Boolean
Dim pHdr As FS6IPC_WRITESTATEDATA_HDR

  ' abort if necessary
  If (m_pView& = 0) Then
    dwResult = FSUIPC_ERR_NOTOPEN
    FSUIPC_WriteS = False
    Exit Function
  End If

  ' Check have space for this request (including terminator)
  If ((((m_pNext&) - (m_pView&)) + 4 + (dwSize + Len(pHdr))) > MAX_SIZE) Then
    dwResult = FSUIPC_ERR_SIZE
    FSUIPC_WriteS = False
    Exit Function
  End If

  ' Initialise header for write request
  pHdr.dwId = FS6IPC_WRITESTATEDATA_ID
  pHdr.dwOffset = dwOffset
  pHdr.nBytes = dwSize
  CopyMemory ByVal m_pNext&, pHdr, Len(pHdr)

  ' Move pointer past the record
  m_pNext& = m_pNext& + Len(pHdr)
  If (dwSize <> 0) Then
    ' Copy in the data to be written
    CopyMemory ByVal m_pNext&, ByVal pSrce, dwSize
    ' Update the pointer ready for more data
    m_pNext& = m_pNext& + dwSize
  End If

  dwResult = FSUIPC_ERR_OK
  FSUIPC_WriteS = True
End Function


