<Command("Power Drain", "Pump Delay |0-60| Drain Type |0-1| Speed |1-100|%", , "30", "5"),
 TranslateCommand("zh-TW", "動力排水 ", "啟動延遲 |0-60| 水管|0-1| 運轉速度 |1-100|%"),
 Description("MAX=60s,MIN=0s 1=Light Dirty 0=Heavy Dirty   MAX=100% ,MIN=1%"),
 TranslateDescription("zh-TW", "60(秒)=最大,0(秒)=最小   1=排清汙 0=排重汙   最高=100% ,最小=1%")>
Public NotInheritable Class Command32
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S32
    Off
    WaitAuto
    WaitTime
    WaitTempSafe
    WaitTime4
    WaitLowLevel
    WaitTime5
    WaitMainPumpFB
    WaitNotDrainLevel
    WaitTime6
    WaitTime7
    WaitNotDrainLevel2
    WaitTime8
  End Enum

  Public Wait As New Timer

  Public PumpDelayTime As Integer
  Public DrainType As Integer
  Public StateString As String
  Public DrainSafetyTimer As New Timer

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
      .Command05.Cancel() : .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel()
      .Command14.Cancel() : .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel()
      .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel() : .Command54.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel() : .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command45.Cancel() : .Command46.Cancel()
      .Command24.Cancel()

      .TempControlFlag = False
      PumpDelayTime = MinMax(param(1), 0, 60)
      DrainType = MinMax(param(2), 0, 1)
      '.IO.PumpSpeedControl = CType(param(3) * 10, Short)
      .PumpSpeed = MinMax(param(3), 1, 100)
      .PumpStopRequest = True
      Wait.TimeRemaining = 1
      State = S32.WaitAuto
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S32.Off
          StateString = ""

        Case S32.WaitAuto
          StateString = If(.Language = LanguageValue.ZhTw, "系統手動中", "System Manual")
          If Not .IO.SystemAuto Then Exit Select
          State = S32.WaitTime

        Case S32.WaitTime
          StateString = If(.Language = LanguageValue.ZhTw, "停止主泵", "Stopping pump ")

          If Not Wait.Finished Then Exit Select
          .PumpStopRequest = False
          .IO.PumpSpeedControl = 0
          .PumpOn = False
          State = S32.WaitTempSafe

        Case S32.WaitTempSafe
          StateString = If(.Language = LanguageValue.ZhTw, "溫度異常", "Interlocked Temperature")
          If .IO.MainTemperature >= .Parameters.SetSafetyTemp * 10 Then
            .Alarms.HighTempNoFill = True
            Exit Select
          End If

          .Alarms.HighTempNoDrain = False
          'If DrainType = 0 Then
          ' .IO.PowerDrain = True
          ' Else
          ' .IO.PowerHotDrain = True
          ' End If
          Wait.TimeRemaining = PumpDelayTime
          DrainSafetyTimer.TimeRemaining = .Parameters.SetDrainSafetyTime * 60
          State = S32.WaitTime4

        Case S32.WaitTime4
          StateString = If(.Language = LanguageValue.ZhTw, "停止主泵", "Stopping pump ")
          If Not Wait.Finished Then Exit Select
          State = S32.WaitLowLevel

        Case S32.WaitLowLevel
          StateString = If(.Language = LanguageValue.ZhTw, "水位不足", "Level to low ")
          If Not .LowLevel Then
            State = S32.WaitNotDrainLevel2
          Else
            .IO.PumpSpeedControl = CType(.PumpSpeed * 10, Short)
            .PumpStartRequest = True
            Wait.TimeRemaining = 1
            State = S32.WaitTime5
          End If

        Case S32.WaitTime5
          StateString = If(.Language = LanguageValue.ZhTw, "開始主泵", "Starting pump ")
          If Not Wait.Finished Then Exit Select
          .PumpStartRequest = False
          .PumpOn = True
          State = S32.WaitMainPumpFB

        Case S32.WaitMainPumpFB
          StateString = If(.Language = LanguageValue.ZhTw, "主泵沒有運行", "Main pump not running")
          If Not .IO.MainPumpFB Then Exit Select
          State = S32.WaitNotDrainLevel

        Case S32.WaitNotDrainLevel
          StateString = If(.Language = LanguageValue.ZhTw, "主缸排水至低水位", "Draining to low level")
          If DrainSafetyTimer.Finished Then
            State = S32.Off
          End If
          If .LowLevel Then Exit Select
          Wait.TimeRemaining = .Parameters.PowerDrainDelay
          State = S32.WaitTime6

        Case S32.WaitTime6
          StateString = If(.Language = LanguageValue.ZhTw, "主缸排水至低水位", "Draining to low level ")
          If Not Wait.Finished Then Exit Select
          .IO.PumpSpeedControl = 0
          .PumpStopRequest = True
          Wait.TimeRemaining = 1
          State = S32.WaitTime7

        Case S32.WaitTime7
          StateString = If(.Language = LanguageValue.ZhTw, "停止主泵", "Stopping pump ")
          If Not Wait.Finished Then Exit Select
          .PumpStopRequest = False
          .PumpOn = False
          State = S32.WaitNotDrainLevel2

        Case S32.WaitNotDrainLevel2
          StateString = If(.Language = LanguageValue.ZhTw, "主缸排水至低水位", "Draining to drain level ")
          If DrainSafetyTimer.Finished Then
            State = S32.Off
          End If
          If .LowLevel Then Exit Select
          Wait.TimeRemaining = .Parameters.DrainDelay
          State = S32.WaitTime8

        Case S32.WaitTime8
          StateString = If(.Language = LanguageValue.ZhTw, "主缸排水延遲", "Draining delay ") & TimerString(Wait.TimeRemaining)
          If Not Wait.Finished Then Exit Select
          State = S32.Off
      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S32.Off
    Wait.Cancel()
  End Sub

  Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

  End Sub

#Region "Standard Definitions"
  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return State <> S32.Off
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S32
  Public Property State() As S32
    Get
      Return state_
    End Get
    Private Set(ByVal value As S32)
      state_ = value
    End Set
  End Property
  Public ReadOnly Property IsPowerDrain() As Boolean
    Get
      Return (DrainType <> 1) AndAlso ((State = S32.WaitTime4) Or (State = S32.WaitLowLevel) Or (State = S32.WaitTime5) Or _
                   (State = S32.WaitNotDrainLevel) Or (State = S32.WaitTime6) Or (State = S32.WaitTime7) Or (State = S32.WaitNotDrainLevel2) Or _
                   (State = S32.WaitTime8) Or (State = S32.WaitMainPumpFB))
    End Get
  End Property
  Public ReadOnly Property IsPowerHotDrain() As Boolean
    Get
      Return (DrainType <> 0) AndAlso ((State = S32.WaitTime4) Or (State = S32.WaitLowLevel) Or (State = S32.WaitTime5) Or _
                   (State = S32.WaitNotDrainLevel) Or (State = S32.WaitTime6) Or (State = S32.WaitTime7) Or (State = S32.WaitNotDrainLevel2) Or _
                   (State = S32.WaitTime8) Or (State = S32.WaitMainPumpFB))
    End Get
  End Property
  Public ReadOnly Property IsHotDrain() As Boolean
    Get
      Return DrainType <> 0 AndAlso ((State = S32.WaitNotDrainLevel2) Or (State = S32.WaitTime6) Or _
                                       (State = S32.WaitTime7) Or (State = S32.WaitTime8))
    End Get
  End Property
  Public ReadOnly Property IsDrain() As Boolean
    Get
      Return (DrainType <> 1) AndAlso ((State = S32.WaitNotDrainLevel2) Or (State = S32.WaitTime6) Or _
                                      (State = S32.WaitTime7) Or (State = S32.WaitTime8))
    End Get
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command32 As New Command32(Me)
End Class
#End Region
