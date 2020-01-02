
'Standard dispense state codes
Public Enum EDispenseState As Integer
  Ready = 101
  Busy = 102
  Auto = 201
  Scheduled = 202
  WaitDissolve = 205
  Complete = 301
  Manual = 302
  [Error] = 309
End Enum