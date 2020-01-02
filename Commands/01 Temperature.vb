<Command("Temperature", "SetTemp |0-145|℃ Gradient |0-9|.|0-9| HoldTime |0-999|", "('2*10)+'3", "'1", "'4"),
 TranslateCommand("zh-TW", "溫度控制", "目標溫度|0-145|℃,斜率|0-9|.|0-9|,持溫|0-999|分")>
Public NotInheritable Class Command01
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S01
    Off
    Start
    Run
    Hold
    Complete
    Pause
  End Enum

  Public Wait As New Timer
  Public TargetTemp As Integer
  Public Gradient As Integer
  Public StateString As String
  Public HoldTime As Integer
  Public HoldTimeWas As Integer
  Public 馬達啟動延遲 As New Timer
  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      'cancels for all other forground functions
      .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel() : .Command05.Cancel()
      .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel() : .Command14.Cancel()
      .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
      .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel() : .Command56.Cancel() : .Command54.Cancel() : .Command55.Cancel()
      .Command57.Cancel() : .TemperatureControl.Cancel() : .Command01.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command45.Cancel() : .Command46.Cancel()
      .Command24.Cancel()


      .BTankHeatStartRequest = False
      .PumpStopRequest = False
      TargetTemp = Maximum(param(1) * 10, 1500)
      Gradient = param(2) * 10 + param(3)
      HoldTime = 60 * param(4)
      Wait.TimeRemaining = HoldTime
      Wait.Pause()

      'Check Temperature mode - change during TPHold if necessary
      '.TemperatureControl.TempMode = 0
      'If .TemperatureControl.Parameters_HeatCoolModeChange = 1 Then .TemperatureControl.TempMode = 2
      'If .TemperatureControl.Parameters_HeatCoolModeChange = 2 Then .TemperatureControl.TempMode = 2
      '****************************************************
      .PumpStartRequest = False
      If .IO.LowLevel And Not .IO.MainPumpFB Then
        .PumpStartRequest = True
        .PumpStopRequest = False
        .PumpOn = True
        .PumpSpeed = 100

      End If
      馬達啟動延遲.TimeRemaining = 2
      '****************************************************
      State = S01.Start

    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S01.Off
          StateString = ""

        Case S01.Start
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
          StateString = ""
          .TempControlFlag = True
          .TemperatureControl.CoolingIntegral = .TemperatureControl.Parameters_CoolIntegral
          .TemperatureControl.CoolingMaxGradient = .TemperatureControl.Parameters_CoolMaxGradient
          .TemperatureControl.CoolingPropBand = .TemperatureControl.Parameters_CoolPropBand
          .TemperatureControl.CoolingStepMargin = .TemperatureControl.Parameters_CoolStepMargin
          .TemperatureControl.Start(.IO.MainTemperature, TargetTemp, Gradient)
          If .IO.MainTemperature > TargetTemp Then
            .CoolNow = True
            .HeatNow = False
            .TemperatureControl.TempMode = 4
          Else
            .CoolNow = False
            .HeatNow = True
            .TemperatureControl.TempMode = 3
          End If
          State = S01.Run

        Case S01.Run

          StateString = ""
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Wait.Pause()
            StateWas = State
            State = S01.Pause
          End If
          If .HeatNow And .IO.MainTemperature < TargetTemp Then Exit Select
          If .CoolNow And .IO.MainTemperature > TargetTemp Then Exit Select
          Wait.Restart()
          State = S01.Hold

        Case S01.Hold
          StateString = "持溫時間" & " " & TimerString(Wait.TimeRemaining)
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            .TemperatureControl.Cancel()
            .TemperatureControl.Start(.IO.MainTemperature, TargetTemp, Gradient)
            Wait.Pause()
            StateWas = State
            State = S01.Pause
            HoldTimeWas = HoldTime
          End If
          If Not Wait.Finished Then Exit Select
          .TemperatureControl.Cancel()
          .TemperatureControlFlag = False
          State = S01.Complete

        Case S01.Complete
          .HeatNow = False
          .CoolNow = False
          State = S01.Off

        Case S01.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停 ", "Paused ") & " " & TimerString(Wait.TimeRemaining)
          If (Not .Parent.IsPaused) And .IO.MainPumpFB And .IO.SystemAuto Then
            .TemperatureControl.Cancel()
            .TemperatureControl.Start(.IO.MainTemperature, TargetTemp, Gradient)
            State = StateWas
            StateWas = S01.Off
            If State = S01.Run Then
              Wait.TimeRemaining = HoldTime
              Wait.Pause()
            ElseIf State = S01.Hold Then
              If ((HoldTime - HoldTimeWas) + Wait.TimeRemaining) > 0 Then
                Wait.TimeRemaining = (HoldTime - HoldTimeWas) + Wait.TimeRemaining
              Else
                Wait.Restart()
              End If
            End If
          End If
      End Select
    End With
  End Function


  Public Sub Cancel() Implements ACCommand.Cancel
    State = S01.Complete
    Wait.Cancel()
  End Sub

  Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged
    TargetTemp = Maximum(param(1) * 10, 1500)
    Gradient = param(2) * 10 + param(3)
    HoldTime = 60 * param(4)
  End Sub



#Region "Standard Definitions"
  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return State <> S01.Off
    End Get
  End Property

  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S01
  Public Property State() As S01
    Get
      Return state_
    End Get
    Private Set(ByVal value As S01)
      state_ = value
    End Set
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S01
  Public Property StateWas() As S01
    Get
      Return statewas_
    End Get
    Private Set(ByVal value As S01)
      statewas_ = value
    End Set
  End Property


  Public ReadOnly Property IsRamping() As Boolean
    Get
      Return (State = S01.Run)
    End Get
  End Property
  Public ReadOnly Property IsHolding() As Boolean
    Get
      Return (State = S01.Hold)
    End Get
  End Property
  Public ReadOnly Property IsPaused() As Boolean
    Get
      Return (State = S01.Pause)
    End Get
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command01 As New Command01(Me)
End Class
#End Region
