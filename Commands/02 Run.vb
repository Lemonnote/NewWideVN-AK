<Command("Run", "Time |1-99| mins", , , "'1"),
 TranslateCommand("zh-TW", "運轉時間", "運轉時間 |1-99|分")>
Public NotInheritable Class Command02
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S02
    Off
    WaitTime
    Pause
  End Enum
  Public StateString As String
  Public Wait As New Timer
  Public RunTime As Integer
  Public RunTimeWas As Integer
    Public 馬達啟動延遲 As New Timer
  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      'cancels for all other forground functions
      .Command03.Cancel() : .Command04.Cancel() : .Command05.Cancel()
      .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel() : .Command14.Cancel()
      .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
      .Command33.Cancel() : .Command01.Cancel()
      .Command51.Cancel() : .Command52.Cancel() : .Command54.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command45.Cancel() : .Command46.Cancel()
      .Command24.Cancel()

      .TemperatureControlFlag = True
      RunTime = 60 * param(1)
            Wait.TimeRemaining = RunTime
            '****************************************************
            .PumpStartRequest = False
      .PumpStopRequest = False
      If .IO.LowLevel And Not .IO.MainPumpFB Then
        .PumpStartRequest = True
        .PumpStopRequest = False
        .PumpOn = True
        .PumpSpeed = 100

      End If
      馬達啟動延遲.TimeRemaining = 2
            '****************************************************
      State = S02.WaitTime
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S02.Off
          StateString = ""

                Case S02.WaitTime
                    '****************************************************
                    If Not 馬達啟動延遲.Finished Then Exit Select
                    If Not .IO.MainPumpFB And .IO.LowLevel Then
            StateString = If(.Language = LanguageValue.ZhTw, "馬達未運行", "Main Pump Not Run")
            Exit Select
                    ElseIf Not .IO.LowLevel Then
            StateString = If(.Language = LanguageValue.ZhTw, "沒有低水位來啟動馬達", "No Low Level To Start Pump")
            Exit Select
                    Else
                        StateString = ""
                        .PumpStartRequest = False
                    End If
                    '****************************************************
                    StateString = If(.Language = LanguageValue.ZhTw, "運轉循環時間", "Running") & " " & TimerString(Wait.TimeRemaining)
                    'this pauses the command
                    If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
                        'pause the timer
                        Wait.Pause()
                        StateWas = State
                        State = S02.Pause
                        RunTimeWas = RunTime
                    End If
                    If Not Wait.Finished Then Exit Select
                    State = S02.Off

        Case S02.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停", "Paused") & " " & TimerString(Wait.TimeRemaining)
          'no longer pause restart the timer and go back to the wait state
          If (Not .Parent.IsPaused) And .IO.MainPumpFB And .IO.SystemAuto Then
            State = StateWas
            StateWas = S02.Off
            If RunTimeWas = RunTime Then
              Wait.Restart()
            Else
              Wait.TimeRemaining = RunTime
            End If
          End If
      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S02.Off
    Wait.Cancel()
  End Sub

  Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged
    RunTime = 60 * param(1)
  End Sub


#Region "Standard Definitions"
  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return State <> S02.Off
    End Get
  End Property

  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S02
  Public Property State() As S02
    Get
      Return state_
    End Get
    Private Set(ByVal value As S02)
      state_ = value
    End Set
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S02
  Public Property StateWas() As S02
    Get
      Return statewas_
    End Get
    Private Set(ByVal value As S02)
      statewas_ = value
    End Set
  End Property


#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command02 As New Command02(Me)
End Class
#End Region
