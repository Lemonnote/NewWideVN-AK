<Command("C Tank Volume Prepare", "Mixer Time |0-30| Type |0-1| Qty% |0-100|", , , "'1+2"),
TranslateCommand("zh-TW", "C藥缸備藥", "攪拌 |0-30| 水源 |0-1| 水量% |0-100|"),
Description("30=MAX 0=UN MIX   1=COLD 0=CIRCULATE 100%=MAX 0%=MIN"),
TranslateDescription("zh-TW", "30分=最高 0分=不攪拌   1=備清水 0=備回水 100%=最高 0%=最小")>
Public NotInheritable Class Command55
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S55
    Off
    CheckReady
    WaitNoAddButtons
    FillToLowLevel
    CheckReady1
    DispenseWaitReady
    DispenseWaitResponse
    Slow
    FillQty
    MixForTime
    WaitMixer
    MixForTime1
    Ready
    Pause
  End Enum
  Public StateString As String

  Public Time, Type, Qty, CallOff As Integer
  Public LaSystem As Boolean
  Public HeatingSet As Integer
    Public WaitTimer As New Timer
    Public 馬達啟動延遲 As New Timer

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
      .Command05.Cancel() : .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel()
      .Command14.Cancel() : .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel()
      .Command32.Cancel() : .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel()
      .Command54.Cancel() : .Command56.Cancel() : .Command57.Cancel() : .Command65.Cancel()
            .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command24.Cancel()


      Time = Maximum(param(1) * 60, 1800) '1800 is 30 *60
      Type = MinMax(param(2), 0, 1)
      Qty = MinMax(param(3) * 10, 0, 1000)
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
      .Command65.Cancel()
      State = S55.CheckReady
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S55.Off
          StateString = ""

        Case S55.CheckReady
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
          State = S55.WaitNoAddButtons

        Case S55.WaitNoAddButtons
          StateString = If(.Language = LanguageValue.ZhTw, "等待藥缸", "Tank C Interlocked")
          State = S55.FillToLowLevel
          WaitTimer.TimeRemaining = 2
          .MessageSTankFilling = True

        Case S55.FillToLowLevel
          If .Parameters.CTankLevelType = 0 Then
            StateString = If(.Language = LanguageValue.ZhTw, "C藥缸進水至低水位", "Tank C Filling to low level")
            If Not .CTankLowLevel Then WaitTimer.TimeRemaining = 2
            If WaitTimer.Finished Then  'NB
              .MessageSTankFilling = False
              .MessageSTankPrepare = True
              State = S55.Slow
              If CallOff > 0 And .Parameters.ChemicalEnable = 1 Then
                .ChemicalCallOff = 0   'Starts the handshake with the host / auto dispenser
                .ChemicalTank = 1
                State = S55.DispenseWaitReady
              End If
            End If
          Else
            StateString = If(.Language = LanguageValue.ZhTw, "C藥缸預先進水", "Tank C Pre Fill") & .IO.TankCLevel \ 10 & "%"
            If .IO.TankCLevel >= .Parameters.SideTankPreFillVolume * 10 Then
              .MessageSTankFilling = False
              .MessageSTankPrepare = True
              State = S55.Slow
              If CallOff > 0 And .Parameters.ChemicalEnable = 1 Then
                .ChemicalCallOff = 0   'Starts the handshake with the host / auto dispenser
                .ChemicalTank = 1
                State = S55.DispenseWaitReady
              End If
            End If
          End If

        Case S55.DispenseWaitReady
          StateString = If(.Language = LanguageValue.ZhTw, "助劑備藥中", "Prepare Tank C")
          'TODO  Add timeout code to switch to manual if no response
          If .ChemicalState = EDispenseState.Ready Then
            'Dispenser is ready so set CallOff number and wait for result
            .ChemicalCallOff = CallOff
            .ChemicalTank = 1
            State = S55.DispenseWaitResponse
          End If
          'Switch to manual if enable parameter is changed or calloff is reset
          If .Parameters.ChemicalEnable <> 1 Then State = S55.Slow
          If CallOff = 0 Then State = S55.Slow

        Case S55.DispenseWaitResponse
          Select Case .ChemicalState
            Case EDispenseState.Complete
              'Everything completed ok so set ready flag and carry on
              .ChemicalCallOff = 0
              State = S55.Slow

            Case EDispenseState.Manual
              'Manual dispenses required so call the operator
              .ChemicalCallOff = 0
              State = S55.Slow

            Case EDispenseState.Error
              'Dispense error call the operator
              .ChemicalCallOff = 0
              State = S55.Slow
          End Select
          'Switch to manual if enable parameter is changed or calloff is reset
          If .Parameters.ChemicalEnable <> 1 Then State = S55.Slow
          If CallOff = 0 Then State = S55.Slow

        Case S55.Slow
          StateString = If(.Language = LanguageValue.ZhTw, "備藥完成，請按備藥OK", "Prepare Tank C")
          If .IO.CTankReady Or (.Parameters.SideTankPrepareConfirm = 1) Then
            .TankCReady = True
            If Qty > 5 Then
              State = S55.FillQty
            Else
              State = S55.MixForTime
            End If
          End If

        Case S55.FillQty
          StateString = If(.Language = LanguageValue.ZhTw, "C藥缸進水至 ", "Filling Tank C to ") & Qty / 10 & "%"
          If .Parent.IsPaused Then
            StateWas = State
            State = S55.Pause
            WaitTimer.Pause()
          End If
          If .IO.TankCLevel > Qty Or .CTankHighLevel Then
            State = S55.MixForTime
          End If

        Case S55.MixForTime
          StateString = If(.Language = LanguageValue.ZhTw, "C藥缸升溫 ", "Tank C heating for ")
          If HeatingSet = 1 Then
            .CTankHeatStartRequest = True
            If .IO.CTankTemperature < .CHeatTemperature Then Exit Select
            .CTankHeatStartRequest = False
          End If
          State = S55.WaitMixer

        Case S55.WaitMixer
          WaitTimer.TimeRemaining = Time
          StateString = If(.Language = LanguageValue.ZhTw, "等待B缸攪拌", "Wait Tank B Mixing")
          If .TankBMix.IsMixingForTime And (.Parameters.CTankMixType = 0 And .Parameters.BTankMixType = 0) Then Exit Select
          State = S55.MixForTime1

        Case S55.MixForTime1
          StateString = If(.Language = LanguageValue.ZhTw, "C藥缸攪拌 ", "Tank C mixing for ") & TimerString(WaitTimer.TimeRemaining)
          If .Parent.IsPaused Then
            StateWas = State
            State = S55.Pause
            WaitTimer.Pause()
          End If
          If WaitTimer.Finished Then
            State = S55.Ready
          End If

        Case S55.Ready
          .TankCReady = True
          State = S55.Off

        Case S55.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停", "Paused") & " " & TimerString(WaitTimer.TimeRemaining)
          If Not .Parent.IsPaused Then
            State = StateWas
            StateWas = S55.Off
            WaitTimer.Restart()
          End If

      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S55.Off
    WaitTimer.Cancel()
  End Sub

  Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

  End Sub

#Region " Standard Definitions "

  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return State <> S55.Off
    End Get
  End Property
  Public ReadOnly Property IsTankInterlocked() As Boolean
    Get
      Return (State = S55.WaitNoAddButtons)
    End Get
  End Property
  Public ReadOnly Property IsFillingFresh() As Boolean
    Get
      Return (Type = 1) And ((State = S55.FillQty) Or (State = S55.FillToLowLevel))
    End Get
  End Property
  Public ReadOnly Property IsFillingCirc() As Boolean
    Get
      Return (Type = 0) And ((State = S55.FillQty) Or (State = S55.FillToLowLevel))
    End Get
  End Property
  Public ReadOnly Property IsSlow() As Boolean
    Get
      Return (State = S55.Slow)
    End Get
  End Property
  Public ReadOnly Property IsMixingForTime() As Boolean
    Get
      Return (State = S55.MixForTime) Or (State = S55.MixForTime1)
    End Get
  End Property
  Public ReadOnly Property IsReady() As Boolean
    Get
      Return (State = S55.Ready)
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S55
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S55
  Public Property State() As S55
    Get
      Return state_
    End Get
    Private Set(ByVal value As S55)
      state_ = value
    End Set
  End Property
  Public Property StateWas() As S55
    Get
      Return statewas_
    End Get
    Private Set(ByVal value As S55)
      statewas_ = value
    End Set
  End Property

#End Region

End Class

#Region " Class Instance "

Partial Public Class ControlCode
  Public ReadOnly Command55 As New Command55(Me)
End Class

#End Region
