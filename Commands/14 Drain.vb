<Command("Drain", "Drain Type |0-1|", , "30", "5"),
 TranslateCommand("zh-TW", "主缸排水", "水管選擇 |0-2|"),
 Description("2=Light+Heavy,1=Drain Heavy Dirty, 0=Drain Light Dirty"),
 TranslateDescription("zh-TW", "2=清汙+重汙,1=排清汙 0=排重汙")>
Public NotInheritable Class Command14
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S14
    Off
    WaitAuto
    WaitTime
    WaitTempSafe
    WaitDrain
    WaitTime5
    WaitNotEntanglement2
    WaitTime6
  End Enum

  Public Wait As New Timer
  Public StateString As String
  Public DrainSafetyTimer As New Timer

  Public DrainType As Integer
  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
      .Command05.Cancel() : .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel()
      .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
      .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel() : .Command54.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel() : .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command45.Cancel() : .Command46.Cancel()
      .Command24.Cancel()

      .TempControlFlag = False
      DrainType = MinMax(param(1), 0, 1)
      .PumpStopRequest = True
      Wait.TimeRemaining = 1
      State = S14.WaitAuto
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S14.Off
          StateString = ""

        Case S14.WaitAuto
          StateString = If(.Language = LanguageValue.ZhTw, "系統手動中", "System Manual")
          If Not .IO.SystemAuto Then Exit Select
          State = S14.WaitTime

        Case S14.WaitTime
          If Not Wait.Finished Then Exit Select
          .PumpStopRequest = False
          '.IO.PumpSpeedControl = 0
          .PumpOn = False
          State = S14.WaitTempSafe

        Case S14.WaitTempSafe
          StateString = If(.Language = LanguageValue.ZhTw, "溫度異常", "Interlocked Temperature")
          If .IO.MainTemperature >= .Parameters.SetSafetyTemp * 10 Then
            .Alarms.HighTempNoFill = True
            Exit Select
          End If

          .Alarms.HighTempNoDrain = False
          ' .IO.Overflow = True
          ' If DrainType = 0 Then
          ' .IO.Drain = True
          ' ElseIf DrainType = 1 Then
          ' .IO.HotDrain = True
          ' Else
          ' .IO.HotDrain = True
          ' .IO.Drain = True
          ' End If

          'Wait.TimeRemaining = 600 ' after 10 minutes we maybe have a stuck RrainLevel, so go on anyway
          DrainSafetyTimer.TimeRemaining = .Parameters.SetDrainSafetyTime * 60
          State = S14.WaitDrain

        Case S14.WaitDrain
          StateString = If(.Language = LanguageValue.ZhTw, "排水", "Draining ")
          If DrainSafetyTimer.Finished Then
            State = S14.Off
          End If
          If .LowLevel Then Exit Select 'And Not Wait.Finished 

          ' .IO.Overflow = False
          State = S14.WaitNotEntanglement2

        Case S14.WaitNotEntanglement2
          StateString = If(.Language = LanguageValue.ZhTw, "等待", "Waiting for not entangled2 ")
          If .LowLevel Then Exit Select
          Wait.TimeRemaining = .Parameters.DrainDelay
          State = S14.WaitTime6

        Case S14.WaitTime6
          StateString = If(.Language = LanguageValue.ZhTw, "排水延遲", "Draining ") & TimerString(Wait.TimeRemaining)
          If Not Wait.Finished Then Exit Select
          State = S14.Off
      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S14.Off
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
      Return State <> S14.Off
    End Get
  End Property

  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S14
  Public Property State() As S14
    Get
      Return state_
    End Get
    Private Set(ByVal value As S14)
      state_ = value
    End Set
  End Property
  Public ReadOnly Property IsHotDrain() As Boolean
    Get
      Return (DrainType <> 0) AndAlso ((State = S14.WaitDrain) Or (State = S14.WaitNotEntanglement2) Or (State = S14.WaitTime6))
    End Get
  End Property
  Public ReadOnly Property IsColdDrain() As Boolean
    Get
      Return (DrainType <> 1) AndAlso ((State = S14.WaitDrain) Or (State = S14.WaitNotEntanglement2) Or (State = S14.WaitTime6))
    End Get
  End Property
  Public ReadOnly Property IsOverflowDrain() As Boolean
    Get
      Return (State = S14.WaitDrain)
    End Get
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command14 As New Command14(Me)
End Class
#End Region
