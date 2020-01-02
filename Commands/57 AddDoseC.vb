<Command("Add Dose C", "Time|0-60| Curve|0-9| Hold|0-1|", , , "'1"),
 TranslateCommand("zh-TW", "C計量加藥", "加藥時間|0-60| 曲線選擇|0-9| 持溫|0-1|"),
 Description("Dose Time=0~60(minutes), Curve=0~9, 0=Not hold 1=Hold temperature"),
 TranslateDescription("zh-TW", "加藥時間=0~60(分鐘), 曲線=0~9, 0=不持溫 1=持溫")>
Public NotInheritable Class Command57
  Inherits MarshalByRefObject
  Implements ACCommand
  Public StateString As String

  Public Enum S57
    Off
    CheckSafetyTemp
    CheckReady
    Dose
    WaitDoseFinish
    WaitAddFinish
    Rinse1
    MixCir1
    Add
    Rinse2
    MixCir2
    Drain
    Pause
  End Enum

  Public Timer As New Timer, LevelTimer As New Timer
  Public AddTime, AddCurve As Integer
  Public StartLevel As Integer
  Public DoseOutput As Integer
  Public DoseON As Boolean
  Public HoldOn As Boolean
  Public DoseOnTimer As New Timer
  Public DoseOffTimer As New Timer
  Public 馬達啟動延遲 As New Timer
  Public WashTimes As Integer


  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
      .Command05.Cancel() : .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel()
      .Command14.Cancel() : .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel()
      .Command32.Cancel() : .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel()
      .Command54.Cancel() : .Command55.Cancel() : .Command56.Cancel() : .Command67.Cancel()
      .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command24.Cancel()


      AddTime = Maximum(param(1) * 60, 3600)
      AddCurve = param(2)

      If param(3) = 1 Then
        HoldOn = True
        If .IO.MainTemperature < 400 Then
          .CoolNow = True
          .HeatNow = False
          .TemperatureControl.TempMode = 4
        Else
          .CoolNow = False
          .HeatNow = True
          .TemperatureControl.TempMode = 3
        End If
        .TempControlFlag = True
        .TemperatureControl.CoolingIntegral = .TemperatureControl.Parameters_CoolIntegral
        .TemperatureControl.CoolingMaxGradient = .TemperatureControl.Parameters_CoolMaxGradient
        .TemperatureControl.CoolingPropBand = .TemperatureControl.Parameters_CoolPropBand
        .TemperatureControl.CoolingStepMargin = .TemperatureControl.Parameters_CoolStepMargin
        .HoldTemperature = .IO.MainTemperature
        .TemperatureControl.Start(.IO.MainTemperature, .HoldTemperature, 10)
      Else
        HoldOn = False
      End If
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
      WashTimes = 2
      State = S57.CheckReady
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode


      Select Case State
        Case S57.Off
          StateString = ""

        Case S57.CheckReady
          '****************************************************
          If Not 馬達啟動延遲.Finished Then Exit Select
          If Not .IO.MainPumpFB And .IO.LowLevel Then
            StateString = "馬達未運行"
            Exit Select
          ElseIf Not .IO.LowLevel Then
            StateString = "沒有低水位無法運行"
            Exit Select
          Else
            StateString = ""
            .PumpStartRequest = False
          End If
          '****************************************************
          If .Parameters.AcknowledgeForAddition = 0 Then
            If (.TankCReady Or .IO.CTankReady) And (Not (.Command55.IsOn Or .Command65.IsOn Or .Command51.IsOn Or .Command54.IsOn)) Then
              Timer.TimeRemaining = AddTime
              StartLevel = .IO.TankCLevel
              State = S57.Dose
            End If
          Else
            StateString = If(.Language = LanguageValue.ZhTw, "請按確認鈕開始C加藥", "Press CallACK to start C dosing")
            If .IO.CallAck Then '加藥確認=1,須按確認才開始加藥
              Timer.TimeRemaining = AddTime
              StartLevel = .IO.TankCLevel
              State = S57.Dose
            End If
            Exit Select
          End If

          'state string stuff.
          If Not .TankCReady Then
            StateString = If(.Language = LanguageValue.ZhTw, "C缸未備藥", "Tank C not prepared")
          ElseIf .Command55.IsOn Or .Command65.IsOn Then
            StateString = If(.Language = LanguageValue.ZhTw, "等待C缸備藥中", "Waiting for tank C")
          ElseIf .Command52.IsActive Then
            StateString = If(.Language = LanguageValue.ZhTw, "等待C缸稀釋加藥中", "Waiting for tank C to dilute")
          ElseIf .Command51.IsActive Or .Command56.IsActive Then
            StateString = If(.Language = LanguageValue.ZhTw, "等待B缸動作", "Waiting for Tank B")
          End If

        Case S57.Dose
          StateString = If(.Language = LanguageValue.ZhTw, "C缸計量加藥中 ", "Tank C dosing ") & TimerString(Timer.TimeRemaining)
          Static delay10 As New DelayTimer
          DoseOutput = MinMax((((.IO.TankCLevel - SetPoint()) * 10) + 100), 0, 1000)
          DoseON = delay10.Run((DoseOutput > 0), 2)
          If Timer.Finished Then
            If AddTime > 0 Then
              DoseOutput = 1000
              DoseON = True
              Timer.TimeRemaining = .Parameters.DosingDelayTime
              State = S57.WaitDoseFinish
            Else
              DoseOutput = 1000
              DoseON = True
              Timer.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
              State = S57.WaitAddFinish
            End If
          End If
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S57.Pause
          End If

        Case S57.WaitDoseFinish
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S57.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸Dosing延遲", "Tank C dosing delay ") & TimerString(Timer.TimeRemaining)
          If DoseOffTimer.Finished Then
            DoseOffTimer.TimeRemaining = 4
            If DoseOnTimer.Finished Then
              DoseOnTimer.TimeRemainingMs = 500
            End If
          End If
          DoseON = Not DoseOnTimer.Finished

          If .CTankLowLevel Then Timer.TimeRemaining = .Parameters.DosingDelayTime
          If Not Timer.Finished Or .CTankLowLevel Then Exit Select
          DoseOutput = 1000
          DoseON = True
          Timer.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
          State = S57.WaitAddFinish


        Case S57.WaitAddFinish
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S57.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "等待C缸加藥延遲", "Tank C transferring ") & TimerString(Timer.TimeRemaining)
          If .CTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
          If Timer.Finished And Not .CTankLowLevel Then
            DoseOutput = 0
            DoseON = False
            State = S57.Rinse1
            Timer.TimeRemaining = .Parameters.AddTransferRinseTime
          End If

        Case S57.Rinse1
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S57.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸洗缸中", "Tank C rinsing ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddCirculateTimeAfterRinse
            State = S57.MixCir1
          End If

        Case S57.MixCir1
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S57.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸循環中", "Tank C circulating ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
            DoseOutput = 1000
            State = S57.Add
          End If

        Case S57.Add
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S57.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸加藥中", "Tank C transferring ") & TimerString(Timer.TimeRemaining)
          If .CTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
          If Timer.Finished And Not .CTankLowLevel Then
            If WashTimes > 1 Then
              WashTimes = WashTimes - 1
              Timer.TimeRemaining = .Parameters.AddTransferRinseTime
              DoseOutput = 0
              State = S57.Rinse1
            Else
              Timer.TimeRemaining = .Parameters.MixCirculateRinseTime
              State = S57.Rinse2
              DoseOutput = 0
            End If
          End If

        Case S57.Rinse2
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S57.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸洗缸中", "Tank C rinsing ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddCirculateTimeAfterRinse
            State = S57.MixCir2
          End If

        Case S57.MixCir2
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S57.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸循環中", "Tank C circulating ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddTransferDrainTime
            State = S57.Drain
          End If


        Case S57.Drain
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S57.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸排水中", "Tank C draining ") & TimerString(Timer.TimeRemaining)
          If .CTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferDrainTime
          If Timer.Finished And Not .CTankLowLevel Then
            State = S57.Off
            .TankCReady = False
            .TemperatureControl.Cancel()
            .TempControlFlag = False
          End If

        Case S57.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停 ", "Paused ") & " " & TimerString(Timer.TimeRemaining)
          'no longer pause restart the timer and go back to the wait state
          If (Not .Parent.IsPaused) And .IO.MainPumpFB And .IO.SystemAuto Then
            Timer.Restart()
            State = StateWas
            StateWas = S57.Off
          End If

      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    With ControlCode
      State = S57.Off
      LevelTimer.Cancel()
    End With
  End Sub

  Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

  End Sub


  '=100*(1-(SQRT(1-(A5*A5*A5))))
  '<GraphTrace(1, 1200, 300, 2500, "Blue", "%t%"), Translate("zh", "C設定水位")> _
  Public ReadOnly Property SetPoint() As Integer
    Get
      'If timer has finished, just return 0
      If Timer.Finished Then Return 0

      'Amount we should have transferred so far
      Dim elapsedTime = (AddTime - Timer.TimeRemaining) / AddTime
      Dim timeToGo = 1 - elapsedTime
      Dim linearTerm = elapsedTime
      Dim transferAmount = StartLevel * linearTerm

      'Calculate scaling factor (0-1) for progressive and digressive curves
      If AddCurve > 0 Then
        Dim scalingFactor = (10 - AddCurve) / 10
        'Calculate term for progressive transfer (0-1) if odd curve
        If (AddCurve Mod 2) = 1 Then
          Dim maxOddCurve = 1 - Math.Sqrt(1 - (elapsedTime * elapsedTime * elapsedTime))
          Dim oddTerm = (((9 - AddCurve) * elapsedTime) + ((AddCurve + 1) * maxOddCurve)) / 10
          transferAmount = StartLevel * oddTerm
        Else
          'Calculate term for digressive transfer (0-1) if even curve
          Dim maxEvenCurve = 1 - Math.Sqrt(1 - (timeToGo * timeToGo * timeToGo))
          Dim evenTerm = (((10 - AddCurve) * timeToGo) + (AddCurve * maxEvenCurve)) / 10
          transferAmount = StartLevel * (1 - evenTerm)
        End If
      End If

      'Calculate and limit to 0-1000
      Return Math.Min(Math.Max(0, StartLevel - CType(transferAmount, Integer)), 1000)
    End Get
  End Property

#Region "Standard Definitions"
  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return State <> S57.Off
    End Get
  End Property
  Public ReadOnly Property IsActive() As Boolean
    Get
      Return State > S57.CheckReady
    End Get
  End Property
  Public ReadOnly Property IsWaitingForPrepare() As Boolean
    Get
      Return (State = S57.CheckReady)
    End Get

  End Property
  'this is for the dosing valve
  Public ReadOnly Property IsDosing() As Boolean
    Get
      Return ((State = S57.Dose) And DoseON) Or (State = S57.WaitAddFinish) Or (State = S57.Add) Or (State = S57.WaitDoseFinish And DoseON)
    End Get
  End Property
  Public ReadOnly Property IsTransfer() As Boolean
    Get
      Return (State = S57.Add) Or (AddTime = 0 And ((State = S57.Dose) Or (State = S57.WaitAddFinish) Or (State = S57.Add) Or (State = S57.WaitDoseFinish)))
    End Get
  End Property
  Public ReadOnly Property IsTransferPump() As Boolean
    Get
      Return (State = S57.Dose) Or (State = S57.WaitAddFinish) Or (State = S57.Add) Or (State = S57.WaitDoseFinish) Or (State = S57.MixCir1) Or (State = S57.MixCir2)
    End Get
  End Property
  Public ReadOnly Property IsRinsing() As Boolean
    Get
      Return ((State = S57.Rinse1) Or (State = S57.Rinse2))
    End Get
  End Property
  Public ReadOnly Property IsDraining() As Boolean
    Get
      Return (State = S57.Drain)
    End Get
  End Property
  Public ReadOnly Property IsCirculating() As Boolean
    Get
      Return (State = S57.MixCir1) Or (State = S57.MixCir2) Or (State = S57.Rinse1) Or (State = S57.Rinse2)
    End Get
  End Property
  Public ReadOnly Property IsHolding() As Boolean
    Get
      Return (State <> S57.Off) And HoldOn
    End Get
  End Property
  Public ReadOnly Property IsPaused() As Boolean
    Get
      Return State = S57.Pause
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S57
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S57
  Public Property State() As S57
    Get
      Return state_
    End Get
    Private Set(ByVal value As S57)
      state_ = value
    End Set
  End Property
  Public Property StateWas() As S57
    Get
      Return statewas_
    End Get
    Private Set(ByVal value As S57)
      statewas_ = value
    End Set
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command57 As New Command57(Me)
End Class
#End Region
