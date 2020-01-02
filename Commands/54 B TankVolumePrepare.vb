<Command("B Tank Volume Prepare", "MixTime|0-30| Type|0-1| Qty%|0-100| Heat|0-99|℃", , , "'1+2"),
TranslateCommand("zh-TW", "B藥缸備藥", "攪拌|0-30| 水源|0-1| 水量%|0-100| 加熱|0-99|℃"),
Description("Mix Time=0-30 Minutes 0=NoMix, Type 1=COLD 0=CIRCULATE, Qty=0-100%, Heat=0-99℃"),
TranslateDescription("zh-TW", "攪拌時間=0-30分 0=不攪拌, 水源 1=備清水 0=備回水, 水量=0-100%, 加熱=0-99℃")>
Public NotInheritable Class Command54
  Inherits MarshalByRefObject                       'Inheritsg是繼承Windows Form的應用程式要繼承System.Windows.Forms.Form，可先參考物件導向程式設計相關書籍
  Implements ACCommand

  Public Enum S54
    Off
    CheckReady
    WaitNoAddButtons
    PreFill
    CheckReady1
    DispenseWaitReady
    DispenseWaitResponse
    Slow
    FillQty
    WaitMixer
    MixForTime
    Ready
    Pause
  End Enum
  Public StateString As String

  Public Time, Type, Qty, CallOff As Integer
  Public LaSystem As Boolean
  Public WaitTimer As New Timer
  Public 馬達啟動延遲 As New Timer

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
      .Command05.Cancel() : .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel()
      .Command14.Cancel() : .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel()
      .Command32.Cancel() : .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel() : .Command64.Cancel()
      .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command24.Cancel()


      Time = Maximum(param(1) * 60, 1800) '1800 is 30 *60
      Type = MinMax(param(2), 0, 1)
      Qty = MinMax(param(3) * 10, 0, 1000)
      .BHeatTemperature = MinMax(param(4) * 10, 0, 990)
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
      .Command64.Cancel()
      State = S54.CheckReady
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S54.Off
          StateString = ""

        Case S54.CheckReady
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
          State = S54.WaitNoAddButtons

        Case S54.WaitNoAddButtons
          StateString = If(.Language = LanguageValue.ZhTw, "等待藥缸", "Tank B Interlocked")
          State = S54.PreFill
          .MessageSTankFilling = True

        Case S54.PreFill
          StateString = If(.Language = LanguageValue.ZhTw, "B藥缸預先進水", "Tank B Pre Fill") & .IO.TankBLevel \ 10 & "%"
          If .IO.TankBLevel >= .Parameters.SideTankPreFillVolume * 10 Then
            .MessageSTankFilling = False
            .MessageSTankPrepare = True
            State = S54.Slow
            If CallOff > 0 And .Parameters.DyeEnable = 1 Then
              .DyeCallOff = 0   'Starts the handshake with the host / auto dispenser
              .DyeTank = 1
              State = S54.DispenseWaitReady
            End If
          End If

        Case S54.DispenseWaitReady
          StateString = If(.Language = LanguageValue.ZhTw, "染料備藥中", "Prepare Tank B")
          'TODO  Add timeout code to switch to manual if no response
          If .DyeState = EDispenseState.Ready Then
            'Dispenser is ready so set CallOff number and wait for result
            .DyeCallOff = CallOff
            .DyeTank = 1
            State = S54.DispenseWaitResponse
          End If
          'Switch to manual if enable parameter is changed or calloff is reset
          If .Parameters.DyeEnable <> 1 Then State = S54.Slow
          If CallOff = 0 Then State = S54.Slow

        Case S54.DispenseWaitResponse
          Select Case .DyeState
            Case EDispenseState.Complete
              'Everything completed ok so set ready flag and carry on
              .DyeCallOff = 0
              State = S54.Slow

            Case EDispenseState.Manual
              'Manual dispenses required so call the operator
              .DyeCallOff = 0
              State = S54.Slow

            Case EDispenseState.Error
              'Dispense error call the operator
              .DyeCallOff = 0
              State = S54.Slow
          End Select
          'Switch to manual if enable parameter is changed or calloff is reset
          If .Parameters.DyeEnable <> 1 Then State = S54.Slow
          If CallOff = 0 Then State = S54.Slow

        Case S54.Slow
          StateString = If(.Language = LanguageValue.ZhTw, "備藥完成，請按備藥OK", "Prepare Tank B")
          If .IO.BTankReady Or (.Parameters.SideTankPrepareConfirm = 1) Then
            .TankBReady = True
            If Qty > 5 Then
              State = S54.FillQty
            Else
              State = S54.MixForTime
            End If
          End If

        Case S54.FillQty
          StateString = If(.Language = LanguageValue.ZhTw, "B藥缸進水 ", "Filling Tank B to ") & Qty / 10 & "%"
          If .Parent.IsPaused Then
            StateWas = State
            State = S54.Pause
            WaitTimer.Pause()
          End If
          If .IO.TankBLevel > Qty Then
            State = S54.WaitMixer
          End If


        Case S54.WaitMixer
          WaitTimer.TimeRemaining = Time
          StateString = If(.Language = LanguageValue.ZhTw, "等待C缸攪拌", "Wait Tank C Mixing")
          If .TankCMix.IsMixingForTime And (.Parameters.CTankMixType = 0 And .Parameters.BTankMixType = 0) Then Exit Select
          State = S54.MixForTime

        Case S54.MixForTime
          StateString = If(.Language = LanguageValue.ZhTw, "B藥缸攪拌 ", "Tank B mixing for ") & TimerString(WaitTimer.TimeRemaining)
          If .Parent.IsPaused Then
            StateWas = State
            State = S54.Pause
            WaitTimer.Pause()
          End If
          If .BHeatTemperature > 200 Then
            .BTankHeatStartRequest = True
            .CTankHeatStartRequest = False
          End If
          If Not WaitTimer.Finished Then Exit Select
          StateString = If(.Language = LanguageValue.ZhTw, "等待B藥缸加熱 ", "Wait tank B heat")
          If .IO.BTankTemperature < .BHeatTemperature Then Exit Select
          .BTankHeatStartRequest = False
          .CTankHeatStartRequest = False
          State = S54.Ready

        Case S54.Ready
          .TankBReady = True
          State = S54.Off

        Case S54.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停", "Paused") & " " & TimerString(WaitTimer.TimeRemaining)
          If Not .Parent.IsPaused Then
            State = StateWas
            StateWas = S54.Off
            WaitTimer.Restart()
          End If

      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S54.Off
    WaitTimer.Cancel()
  End Sub

  Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged
    With ControlCode
      Time = Maximum(param(1) * 60, 600) '600 is 10 *60
      Type = MinMax(param(2), 0, 1)
      Qty = MinMax(param(3) * 10, 0, 1000)
      .BHeatTemperature = MinMax(param(4) * 10, 0, 990)
    End With
  End Sub

#Region " Standard Definitions "

  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return State <> S54.Off
    End Get
  End Property
  Public ReadOnly Property IsTankInterlocked() As Boolean
    Get
      Return (State = S54.WaitNoAddButtons)
    End Get
  End Property
  Public ReadOnly Property IsFillingFresh() As Boolean
    Get
      Return (Type = 1) And ((State = S54.FillQty) Or (State = S54.PreFill))
    End Get
  End Property
  Public ReadOnly Property IsFillingCirc() As Boolean
    Get
      Return (Type = 0) And ((State = S54.FillQty) Or (State = S54.PreFill))
    End Get
  End Property
  Public ReadOnly Property IsSlow() As Boolean
    Get
      Return (State = S54.Slow)
    End Get
  End Property
  Public ReadOnly Property IsMixingForTime() As Boolean
    Get
      Return (State = S54.MixForTime)
    End Get
  End Property
  Public ReadOnly Property IsReady() As Boolean
    Get
      Return (State = S54.Ready)
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S54
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S54
  Public Property State() As S54
    Get
      Return state_
    End Get
    Private Set(ByVal value As S54)
      state_ = value
    End Set
  End Property
  Public Property StateWas() As S54
    Get
      Return statewas_
    End Get
    Private Set(ByVal value As S54)
      statewas_ = value
    End Set
  End Property
  'End Property

#End Region

End Class

#Region " Class Instance "

Partial Public Class ControlCode
  Public ReadOnly Command54 As New Command54(Me)
End Class

#End Region
