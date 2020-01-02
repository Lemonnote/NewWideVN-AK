'<Command("PhControlTime", "TARGET PH |4-9|.|00-99| OPEN TEMP |25-140| ADD TIME |2-999| ", , , "", CommandType.ParallelCommand), _
'TranslateCommand("zh", "PH平行-時間", "PH值設定 |4-9|.|00-99| 起始溫度 |25-140| 加酸時間 |2-999|  "), _
'Description("CONTROL PH MAX=PH 9.9,MIN=PH 4.0 ; OPEN TEMP. MAX=140C,MIN=25C ; ADD TIME MAX=999M MIN=2M  "), _
'TranslateDescription("zh", "PH控制  最高=PH 9.9,最低=PH 4.0 ; 起始溫度 最高=140度,最低=25度 ; 加酸時間 最高=999分 最低=2分 ")> _
Public NotInheritable Class Command76
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S76
    Off
    Start
    Finish_Wash
    Pause
    Finish
  End Enum

  Public PhTarget, PhOpenTemp, AddTime As Integer
  Public Wait As New Timer, RunBackWait As New Timer
  Public StateString As String

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      '--------------------------------------------------------------------------------------------------------PH用

      .PhControl.Cancel() : .PhWash.Cancel() : .Command73.Cancel() : .Command75.Cancel() : .Command74.Cancel() : .Command77.Cancel() : .Command78.Cancel() : .Command79.Cancel() : .Command80.Cancel() : .Command81.Cancel() : .Ph76Time.Cancel()
      '---------------------------------------------------------------------------------------------------------
      .F76專用旗標 = False
      PhTarget = Maximum(param(1) * 100 + param(2), 999) '60*60
      PhOpenTemp = MinMax(param(3), 9, 140)
      AddTime = MinMax(param(4), 1, 999)
      'Wait.TimeRemaining = AddTime * 60
      State = S76.Start


    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S76.Off
          StateString = ""
          If .Command75.State = Command75.S75.Off And .Command74.State = Command74.S74.Off And .Command77.State = Command77.S77.Off And .Command78.State = Command78.S78.Off And _
          .Command80.State = Command80.S80.Off And Not .Ph76Time.IsOn Then
            .PhControlFlag = False
          End If



        Case S76.Start

          StateString = ""
          .F76專用旗標 = True
          .Ph76Time.Run()
          State = S76.Off



      End Select

    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S76.Off
    Wait.Cancel()

  End Sub
  Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

    PhTarget = Maximum(param(1) * 100 + param(2), 999) '60*60
    PhOpenTemp = MinMax(param(3), 25, 140)
    AddTime = MinMax(param(4), 2, 60)
  End Sub
#Region "Standard Definitions"
  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return State <> S76.Off
    End Get
  End Property
  Public ReadOnly Property IsActive() As Boolean
    Get
      Return State > S76.Start
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S76
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S76

  Public Property State() As S76
    Get
      Return state_
    End Get
    Private Set(ByVal value As S76)
      state_ = value
    End Set
  End Property
  Public ReadOnly Property Istest() As Boolean
    Get
      Return (State = S76.Start)
    End Get
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
    Public ReadOnly Command76 As New Command76(Me)
End Class
#End Region
