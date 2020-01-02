<Command("B Dose Parallel", "Time |0-60| Curve |0-9|", , , "'1", CommandType.ParallelCommand),
 TranslateCommand("zh-TW", "平行B加藥", "加藥時間 |0-60| 曲線選擇 |0-9|"),
 TranslateDescription("zh-TW", "4=C缸 5=B缸,0(分)=最小 60(分)=最大 ,最小=0 最大=9")>
Public NotInheritable Class Command66
  Inherits MarshalByRefObject
  Implements ACCommand
  Public StateString As String

  Public Enum S66
    Off
    CheckSafetyTemp
    CheckReady
    PreMix
    WaitStable
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
      .Command56.Cancel()
      .BTankHeatStartRequest = False
      AddTime = Maximum(param(1) * 60, 3600)
      AddCurve = param(2)
      WashTimes = 2
      State = S66.CheckReady
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode


      Select Case State
        Case S66.Off
          StateString = ""

        Case S66.CheckReady

          If .Parent.IsPaused Or Not .IO.MainPumpFB Then
            Timer.Pause()
            State = S66.Pause
          End If
          If .TankBMixOn Then Exit Select
          If .Parameters.AcknowledgeForAddition = 0 Then
            If (.TankBReady Or .IO.BTankReady) And (Not (.Command54.IsOn Or .Command64.IsOn)) Then     '如果有備藥OK及其他沒用到藥缸的
              Timer.TimeRemaining = 10
              State = S66.PreMix
            End If
          Else
            StateString = If(.Language = LanguageValue.ZhTw, "請按確認鈕開始B加藥", "Press CallACK to start B dosing")
            If .IO.CallAck Then '加藥確認=1,須按確認鈕才開始加藥
              Timer.TimeRemaining = 10
              State = S66.PreMix
            End If
            Exit Select
          End If

          'state string stuff.
          If Not .TankBReady Then                             '如果沒備藥OK，將顯示"Tank B not prepared"，有備藥跳步驟
            StateString = If(.Language = LanguageValue.ZhTw, "B缸未備藥", "Tank B not prepared")
          ElseIf .Command54.IsOn Or .Command64.IsOn Then      '如果B缸備藥有使用，顯示"Waiting for Tank B"，不然跳步驟
            StateString = If(.Language = LanguageValue.ZhTw, "等待B缸備藥中", "Waiting for Tank B")
          ElseIf .Command51.IsActive Then                     '如果B稀釋加藥有使用，顯示"Waiting for tank B to dilute"，不然就跳步驟
            StateString = If(.Language = LanguageValue.ZhTw, "等待B缸稀釋加藥中", "Waiting for tank B to dilute")
          ElseIf .Command52.IsActive Or .Command57.IsActive Then ''如果C稀釋加藥有使用，顯示"Waiting for tank B to dilute"，不然就跳步驟
            StateString = If(.Language = LanguageValue.ZhTw, "等待C缸動作", "Waiting for Tank C")
          End If

        Case S66.PreMix
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B藥缸預先攪拌 ", "Tank B pre mixing ") & TimerString(Timer.TimeRemaining)
          If Not Timer.Finished Then Exit Select
          Timer.TimeRemaining = 15
          State = S66.WaitStable

        Case S66.WaitStable
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B藥缸等待穩定 ", "Tank B wait stable ") & TimerString(Timer.TimeRemaining)
          If Not Timer.Finished Then Exit Select
          Timer.TimeRemaining = AddTime                   '將（加藥時間AddTime） 放到 （Timer.TimeRemaining）
          StartLevel = .IO.TankBLevel                     '將（藥缸水位.IO.TankBLevel） 放到 （StartLevel）
          State = S66.Dose

        Case S66.Dose
          StateString = If(.Language = LanguageValue.ZhTw, "B藥缸計量加藥 ", "Tank B dosing ") & TimerString(Timer.TimeRemaining)
          Static delay10 As New DelayTimer
          .BTankHeatStartRequest = False
          DoseOutput = MinMax((((.IO.TankBLevel - SetPoint()) * 10) + 100), 0, 1000)
          DoseON = delay10.Run((DoseOutput > 0), 2)
          If Timer.Finished Then
            If AddTime > 0 Then
              DoseOutput = 1000
              DoseON = True
              Timer.TimeRemaining = .Parameters.DosingDelayTime
              State = S66.WaitDoseFinish
            Else
              DoseOutput = 1000
              DoseON = True
              Timer.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
              State = S66.WaitAddFinish
            End If
          End If
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If

        Case S66.WaitDoseFinish
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B缸Dosing延遲", "Tank B dosing delay ") & TimerString(Timer.TimeRemaining)
          If DoseOffTimer.Finished Then
            DoseOffTimer.TimeRemaining = 4
            If DoseOnTimer.Finished Then
              DoseOnTimer.TimeRemainingMs = 500
            End If
          End If
          DoseON = Not DoseOnTimer.Finished

          If .IO.TankBLevel > 20 Then Timer.TimeRemaining = .Parameters.DosingDelayTime
          If Not Timer.Finished Or .BTankLowLevel Then Exit Select
          DoseOutput = 1000
          DoseON = True
          Timer.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
          State = S66.WaitAddFinish


        Case S66.WaitAddFinish
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B缸加藥延遲", "Tank B transferring ") & TimerString(Timer.TimeRemaining)
          If .BTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
          If Timer.Finished And Not .BTankLowLevel Then
            DoseOutput = 0
            DoseON = False
            State = S66.Rinse1
            Timer.TimeRemaining = .Parameters.AddTransferRinseTime
          End If

        Case S66.Rinse1
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B缸洗缸中", "Tank B rinsing ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddCirculateTimeAfterRinse
            State = S66.MixCir1
          End If

        Case S66.MixCir1
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B缸循環中", "Tank B circulating ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
            DoseOutput = 1000
            State = S66.Add
          End If

        Case S66.Add
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B缸加藥中", "Tank B transferring ") & TimerString(Timer.TimeRemaining)
          If .BTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
          If Timer.Finished And Not .BTankLowLevel Then
            If WashTimes > 1 Then
              WashTimes = WashTimes - 1
              State = S66.Rinse1
              DoseOutput = 0
              DoseON = False
              Timer.TimeRemaining = .Parameters.AddTransferRinseTime
            Else
              Timer.TimeRemaining = .Parameters.MixCirculateRinseTime
              State = S66.Rinse2
              DoseOutput = 0
            End If
          End If

        Case S66.Rinse2
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B缸洗缸中", "Tank B rinsing ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddCirculateTimeAfterRinse
            State = S66.MixCir2
          End If

        Case S66.MixCir2
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B缸循環中", "Tank B circulating ") & TimerString(Timer.TimeRemaining)
          If Timer.Finished Then
            Timer.TimeRemaining = .Parameters.AddTransferDrainTime
            State = S66.Drain
          End If


        Case S66.Drain
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            Timer.Pause()
            StateWas = State
            State = S66.Pause
          End If
          StateString = If(.Language = LanguageValue.ZhTw, "B缸排水", "Tank B draining ") & TimerString(Timer.TimeRemaining)
          If .BTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferDrainTime
          If Timer.Finished And Not .BTankLowLevel Then
            State = S66.Off
            .TankBReady = False
          End If

        Case S66.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停 ", "Paused ") & " " & TimerString(Timer.TimeRemaining)
          'no longer pause restart the timer and go back to the wait state
          If .Parent.CurrentStep <> .Parent.ChangingStep Then
            State = S66.Off
            Timer.Cancel()
          End If
          If (Not .Parent.IsPaused) And .IO.MainPumpFB And .IO.SystemAuto Then
            Timer.Restart()
            State = StateWas
            StateWas = S66.Off
          End If

      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    With ControlCode
      State = S66.Off
      LevelTimer.Cancel()
    End With
  End Sub

  Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

  End Sub


  '<GraphTrace(1, 1200, 2600, 4800, "Blue", "%t%"), Translate("zh", "B設定水位")> _
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
      Return Math.Min(Math.Max(0, StartLevel - CType(transferAmount, Integer)), 2000)
    End Get
  End Property

#Region "Standard Definitions"
  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return State <> S66.Off
    End Get
  End Property
  Public ReadOnly Property IsActive() As Boolean
    Get
      Return State > S66.CheckReady
    End Get
  End Property
  Public ReadOnly Property IsWaitingForPrepare() As Boolean
    Get
      Return (State = S66.CheckReady)
    End Get

  End Property
  'this is for the dosing valve
  Public ReadOnly Property IsDosing() As Boolean
    Get
      Return ((State = S66.Dose) And DoseON) Or (State = S66.WaitAddFinish) Or (State = S66.Add) Or (State = S66.WaitDoseFinish And DoseON)
    End Get
  End Property
  Public ReadOnly Property IsTransfer() As Boolean
    Get
      Return (State = S66.Add) Or (AddTime = 0 And ((State = S66.Dose) Or (State = S66.WaitAddFinish) Or (State = S66.Add) Or (State = S66.WaitDoseFinish)))
    End Get
  End Property
  Public ReadOnly Property IsTransferPump() As Boolean
    Get
      Return (State = S66.Dose) Or (State = S66.WaitAddFinish) Or (State = S66.Add) Or (State = S66.WaitDoseFinish) Or (State = S66.MixCir1) Or (State = S66.MixCir2) Or (State = S66.PreMix)
    End Get
  End Property
  Public ReadOnly Property IsRinsing() As Boolean
    Get
      Return ((State = S66.Rinse1) Or (State = S66.Rinse2))
    End Get
  End Property
  Public ReadOnly Property IsDraining() As Boolean
    Get
      Return (State = S66.Drain)
    End Get
  End Property
  Public ReadOnly Property IsCirculating() As Boolean
    Get
      Return (State = S66.MixCir1) Or (State = S66.MixCir2) Or (State = S66.PreMix) Or (State = S66.Rinse1) Or (State = S66.Rinse2)
    End Get
  End Property
  Public ReadOnly Property IsPaused() As Boolean
    Get
      Return State = S66.Pause
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S66
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S66
  Public Property State() As S66
    Get
      Return state_
    End Get
    Private Set(ByVal value As S66)
      state_ = value
    End Set
  End Property
  Public Property StateWas() As S66
    Get
      Return statewas_
    End Get
    Private Set(ByVal value As S66)
      statewas_ = value
    End Set
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command66 As New Command66(Me)
End Class
#End Region
