<Command("C Dose Parallel", "Time |0-60| Curve |0-9|", , , "'1", CommandType.ParallelCommand),
 TranslateCommand("zh-TW", "平行C加藥", "加藥時間 |0-60| 曲線選擇 |0-9|"),
 TranslateDescription("zh-TW", "4=C缸 5=B缸,0(分)=最小 60(分)=最大 ,最小=0 最大=9")>
Public NotInheritable Class Command67
  Inherits MarshalByRefObject
  Implements ACCommand
  Public StateString As String

  Public Enum S67
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
  Public DoseOnTimer As New Timer
  Public DoseOffTimer As New Timer
  Public WashTimes As Integer


  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command67.Cancel()
      AddTime = Maximum(param(1) * 60, 3600)
      AddCurve = param(2)
      WashTimes = 2
      State = S67.CheckReady
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode


      Select Case State
        Case S67.Off
          StateString = ""

        Case S67.CheckReady

          If .Parent.IsPaused Or Not .IO.MainPumpFB Then
            Timer.Pause()
            State = S67.Pause
          End If
          If .TankCMixOn Then Exit Select
          If .Parameters.AcknowledgeForAddition = 0 Then
            If (.TankCReady Or .IO.CTankReady) And (Not (.Command65.IsOn Or .Command55.IsOn Or .Command56.IsOn Or .Command66.IsOn)) Then
              Timer.TimeRemaining = AddTime
              StartLevel = .IO.TankCLevel
              State = S67.Dose
            End If
          Else
            StateString = If(.Language = LanguageValue.ZhTw, "請按確認鈕開始C加藥", "Press CallACK to start C dosing")
            If .IO.CallAck Then '加藥確認=1,須按呼叫確認才開始加藥
              Timer.TimeRemaining = AddTime
              StartLevel = .IO.TankCLevel
              State = S67.Dose
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

        Case S67.Dose
          StateString = If(.Language = LanguageValue.ZhTw, "C缸計量加藥中 ", "Tank C dosing ") & TimerString(Timer.TimeRemaining)
          Static delay10 As New DelayTimer
          DoseOutput = MinMax((((.IO.TankCLevel - SetPoint()) * 10) + 100), 0, 1000)
          DoseON = delay10.Run((DoseOutput > 0), 2)
          If Timer.Finished Then
            If AddTime > 0 Then
              DoseOutput = 1000
              DoseON = True
              Timer.TimeRemaining = .Parameters.DosingDelayTime
              State = S67.WaitDoseFinish
            Else
              DoseOutput = 1000
              DoseON = True
              Timer.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
              State = S67.WaitAddFinish
            End If
          End If
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S67.Pause
          End If

        Case S67.WaitDoseFinish
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S67.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸Dosing延遲", "Tank C dosing delay ") & TimerString(Timer.TimeRemaining)
          If DoseOffTimer.Finished Then
            DoseOffTimer.TimeRemaining = 4
            If DoseOnTimer.Finished Then
              DoseOnTimer.TimeRemainingMs = 500
            End If
          End If
          DoseON = Not DoseOnTimer.Finished

          If .IO.TankCLevel > 20 Then Timer.TimeRemaining = .Parameters.DosingDelayTime
          If Not Timer.Finished Or .CTankLowLevel Then Exit Select
          DoseOutput = 1000
          DoseON = True
          Timer.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
          State = S67.WaitAddFinish


        Case S67.WaitAddFinish
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S67.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "等待C缸加藥延遲", "Tank C transferring ") & TimerString(Timer.TimeRemaining)
          If .CTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
          If Timer.Finished And Not .CTankLowLevel Then
            DoseOutput = 0
            DoseON = False
            State = S67.Rinse1
            Timer.TimeRemaining = .Parameters.AddTransferRinseTime
          End If

        Case S67.Rinse1
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S67.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸洗缸中", "Tank C rinsing ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddCirculateTimeAfterRinse
            State = S67.MixCir1
          End If

        Case S67.MixCir1
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S67.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸循環中", "Tank C circulating ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
            DoseOutput = 1000
            State = S67.Add
          End If

        Case S67.Add
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S67.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸加藥中", "Tank C transferring ") & TimerString(Timer.TimeRemaining)
          If .CTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
          If Timer.Finished And Not .CTankLowLevel Then
            If WashTimes > 2 Then
              WashTimes = WashTimes - 1
              DoseOutput = 0
              DoseON = False
              State = S67.Rinse1
              Timer.TimeRemaining = .Parameters.AddTransferRinseTime
            Else
              Timer.TimeRemaining = .Parameters.MixCirculateRinseTime
              State = S67.Rinse2
              DoseOutput = 0
            End If

          End If

        Case S67.Rinse2
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S67.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸洗缸中", "Tank C rinsing ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddCirculateTimeAfterRinse
            State = S67.MixCir2
          End If

        Case S67.MixCir2
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S67.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸循環中", "Tank C circulating ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddTransferDrainTime
            State = S67.Drain
          End If


        Case S67.Drain
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S67.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "C缸排水中", "Tank C draining ") & TimerString(Timer.TimeRemaining)
          If .CTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferDrainTime
          If Timer.Finished And Not .CTankLowLevel Then
            State = S67.Off
            .TankCReady = False
          End If

        Case S67.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停 ", "Paused ") & " " & TimerString(Timer.TimeRemaining)
          'no longer pause restart the timer and go back to the wait state
          If .Parent.CurrentStep <> .Parent.ChangingStep Then
            State = S67.Off
            Timer.Cancel()
          End If
          If (Not .Parent.IsPaused) And .IO.MainPumpFB And .IO.SystemAuto Then
            Timer.Restart()
            State = StateWas
            StateWas = S67.Off
          End If

      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    With ControlCode
      State = S67.Off
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
      Return State <> S67.Off
    End Get
  End Property
  Public ReadOnly Property IsActive() As Boolean
    Get
      Return State > S67.CheckReady
    End Get
  End Property
  Public ReadOnly Property IsWaitingForPrepare() As Boolean
    Get
      Return (State = S67.CheckReady)
    End Get

  End Property
  'this is for the dosing valve
  Public ReadOnly Property IsDosing() As Boolean
    Get
      Return ((State = S67.Dose) And DoseON) Or (State = S67.WaitAddFinish) Or (State = S67.Add) Or (State = S67.WaitDoseFinish And DoseON)
    End Get
  End Property
  Public ReadOnly Property IsTransfer() As Boolean
    Get
      Return (State = S67.Add) Or (AddTime = 0 And ((State = S67.Dose) Or (State = S67.WaitAddFinish) Or (State = S67.Add) Or (State = S67.WaitDoseFinish)))
    End Get
  End Property
  Public ReadOnly Property IsTransferPump() As Boolean
    Get
      Return (State = S67.Dose) Or (State = S67.WaitAddFinish) Or (State = S67.Add) Or (State = S67.WaitDoseFinish) Or (State = S67.MixCir1) Or (State = S67.MixCir2)
    End Get
  End Property
  Public ReadOnly Property IsRinsing() As Boolean
    Get
      Return ((State = S67.Rinse1) Or (State = S67.Rinse2))
    End Get
  End Property
  Public ReadOnly Property IsDraining() As Boolean
    Get
      Return (State = S67.Drain)
    End Get
  End Property
  Public ReadOnly Property IsCirculating() As Boolean
    Get
      Return (State = S67.MixCir1) Or (State = S67.MixCir2) Or (State = S67.Rinse1) Or (State = S67.Rinse2)
    End Get
  End Property
  Public ReadOnly Property IsPaused() As Boolean
    Get
      Return State = S67.Pause
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S67
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S67
  Public Property State() As S67
    Get
      Return state_
    End Get
    Private Set(ByVal value As S67)
      state_ = value
    End Set
  End Property
  Public Property StateWas() As S67
    Get
      Return statewas_
    End Get
    Private Set(ByVal value As S67)
      statewas_ = value
    End Set
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command67 As New Command67(Me)
End Class
#End Region
