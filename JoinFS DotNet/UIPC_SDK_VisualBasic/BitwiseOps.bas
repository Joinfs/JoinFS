Attribute VB_Name = "BitwiseOps"
' ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
'   Visual Basic <-> FSUIPC/WideFS Communication                 Version 1.004
'   Copyright © 2000 Chris Brett.  All rights reserved.
'   e-mail: chris@formulate.clara.net
'
'   BITWISE OPERATIONS MODULE
'   used on "FSUIPC_Version" var for version information, otherwise not needed
'
'   PLEASE NOTE:
'     Visual Basic is not my primary programming language.  There may well be
'     syntax or other inefficiencies in this port of the code.  If you find any,
'     please kindly contact me so that I may correct them for future versions.
'     This code was ported to Visual Basic 5, and has not been tested in any
'     other VB version.
' ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



'Enumeration of bit-shifting
Public Enum dcShiftDirection
  LeftShift = -1
  RightShift = 0
End Enum

'Enters:
' lValue as Long
' lNumberOfBitsToShift as Long
' lDirectionToShift as Long
'Returns:
' Long - shifted value
'Purpose:
' Shift the given value by the given number of bits to shift in the given direction.
' Shifting bits to the left acts as a multiplier and to the right divides.

Public Function Shift(ByVal lValue As Long, ByVal lNumberOfBitsToShift As Long, ByVal lDirectionToShift As dcShiftDirection) As Long
Const ksCallname As String = "Shift"
On Error GoTo Procedure_Error
Dim LShift As Long

If lDirectionToShift Then 'shift left
  LShift = lValue * (2 ^ lNumberOfBitsToShift)
Else 'shift right
  LShift = lValue \ (2 ^ lNumberOfBitsToShift)
End If

Procedure_Exit:
  Shift = LShift
  Exit Function

Procedure_Error:
  Err.Raise Err.Number, ksCallname, Err.Description, Err.HelpFile, Err.HelpContext
  Resume Procedure_Exit
End Function



