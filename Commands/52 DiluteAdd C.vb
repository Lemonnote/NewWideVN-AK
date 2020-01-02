<Command("Dilute Add C", "Time |0-60|", , , "'1"),
 TranslateCommand("zh-TW", "C缸稀釋加藥", "加藥時間 |0-60|"),
 Description("MAX=60,0=OPERTOR CONTROL"),
 TranslateDescription("zh-TW", "最高=60分,0=操作員控制")>
Public NotInheritable Class Command52
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S52
    Off
    WaitNoAddButtons
    WaitTempSafe
    WaitSystemAuto
    WaitHighLevel
    WaitTankReady
    WaitAddTimeHold
    WaitAddTimeRunback
    WaitAddTimeTranfer
    WaitNotTankLowLevel
    WaitWashSTank
    WaitMixCir1
    WaitNotTankLowLevel2
    WaitWashSTank2
    WaitMixCir2
    WaitAddFinish3
    Pause
  End Enum

  Public Tank, AddTime As Integer
  Public Wait As New Timer, RunBackWait As New Timer
  Public StateString As String
  Public 馬達啟動延遲 As New Timer

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
      .Command05.Cancel() : .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel()
      .Command14.Cancel() : .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel()
      .Command32.Cancel() : .Command33.Cancel() : .Command51.Cancel() : .Command54.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel()
      .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command24.Cancel()

      AddTime = Maximum(param(1) * 60, 3600) '60*60

      .CoolNow = False
      .HeatNow = True
      .TemperatureControl.TempMode = 3
      .TempControlFlag = True
      .TemperatureControl.CoolingIntegral = .TemperatureControl.Parameters_CoolIntegral
      .TemperatureControl.CoolingMaxGradient = .TemperatureControl.Parameters_CoolMaxGradient
      .TemperatureControl.CoolingPropBand = .TemperatureControl.Parameters_CoolPropBand
      .TemperatureControl.CoolingStepMargin = .TemperatureControl.Parameters_CoolStepMargin
      .HoldTemperature = .IO.MainTemperature
      .TemperatureControl.Start(.IO.MainTemperature, .HoldTemperature, 10)
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
      State = S52.WaitNoAddButtons

    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      ' Run this command
      Select Case State
        'make sure we are not using the tank or the other tank
        Case S52.WaitNoAddButtons
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
          StateString = If(.Language = LanguageValue.ZhTw, "等待藥缸", "Tank C Interlocked")
          'need to look at this

          State = S52.WaitTempSafe

          'check that the temp is ok
        Case S52.WaitTempSafe
          StateString = If(.Language = LanguageValue.ZhTw, "溫度異常", "Interlocked Temperature")
          If .IO.MainTemperature >= .Parameters.SetAddSafetyTemp * 10 Then
            .Alarms.HighTempNoAdd = True
            Exit Select
          End If
          .Alarms.HighTempNoAdd = False

          State = S52.WaitSystemAuto

          'check that we are in auto
        Case S52.WaitSystemAuto
          StateString = If(.Language = LanguageValue.ZhTw, "系統手動中", "Interlocked not In auto")
          If Not .IO.SystemAuto Then Exit Select

          .MessageSTankDiluteAddingNow = True
          State = S52.WaitHighLevel

        Case S52.WaitHighLevel
          StateString = If(.Language = LanguageValue.ZhTw, "C缸迴水至高水位", "Filling Tank C to high level")
          If Not .CTankHighLevel Then Exit Select
          Wait.TimeRemaining = 2
          State = S52.WaitTankReady

          'mix the tank and wait for ready
        Case S52.WaitTankReady
          StateString = If(.Language = LanguageValue.ZhTw, "準備備藥", "Prepare Tank C")
          If .IO.CTankReady Then
            .TankCReady = True
            If AddTime > 0 Then
              Wait.TimeRemaining = AddTime
              State = S52.WaitAddTimeHold
            Else
              State = S52.WaitNotTankLowLevel
            End If
          End If

          'if there is a time recirculate the tank to the machine for that time
        Case S52.WaitAddTimeHold
          StateString = If(.Language = LanguageValue.ZhTw, "C缸循環加藥中", "Diluting C ") & TimerString(Wait.TimeRemaining)
          'check to see if we have circulated the tank for the total mix time
          If Wait.Finished Then
            State = S52.WaitNotTankLowLevel
          End If

          'if the level gets to high turn off the run back 
          If .CTankHighLevel Then
            State = S52.WaitAddTimeTranfer
            RunBackWait.TimeRemaining = 2
          End If

          'if level gets to low turn off the transfer
          If Not .IO.TankCLevel > 600 Then
            State = S52.WaitAddTimeRunback
            RunBackWait.TimeRemaining = 2
          End If
          'pause if halted or pump not runnng
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            StateWas = State
            State = S52.Pause
            Wait.Pause()
          End If

        Case S52.WaitAddTimeRunback
          StateString = If(.Language = LanguageValue.ZhTw, "C缸循環加藥中", "Diluting C ") & TimerString(Wait.TimeRemaining)
          'check to see if we have circulated the tank for the total mix time
          If Wait.Finished Then
            State = S52.WaitNotTankLowLevel
          End If

          'if the level gets to high turn off the run back 
          If .CTankHighLevel Then
            State = S52.WaitAddTimeTranfer
            RunBackWait.TimeRemaining = 2
          End If

          If Not .IO.TankCLevel > 600 Then RunBackWait.TimeRemaining = 2
          If .IO.TankCLevel > 600 And RunBackWait.Finished Then
            State = S52.WaitAddTimeHold
          End If
          'pause if halted or pump not runnng
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            StateWas = State
            State = S52.Pause
            Wait.Pause()
          End If

        Case S52.WaitAddTimeTranfer
          StateString = If(.Language = LanguageValue.ZhTw, "C缸循環加藥中", "Diluting C ") & TimerString(Wait.TimeRemaining)
          If Wait.Finished Then
            State = S52.WaitNotTankLowLevel
          End If

          If .CTankHighLevel Then RunBackWait.TimeRemaining = 2
          If Not .CTankHighLevel And RunBackWait.Finished Then
            State = S52.WaitAddTimeHold
          End If

          If Not .IO.TankCLevel > 600 Then
            State = S52.WaitAddTimeRunback
            RunBackWait.TimeRemaining = 2
          End If
          'pause if halted or pump not runnng
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then
            StateWas = State
            State = S52.Pause
            Wait.Pause()
          End If

          'ok recirculate time is done transfer it to the machine
        Case S52.WaitNotTankLowLevel
          StateString = If(.Language = LanguageValue.ZhTw, "C缸加藥中", "Transferring C ") & TimerString(Wait.TimeRemaining)
          If .CTankLowLevel Then Wait.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
          If Wait.Finished And Not .CTankLowLevel Then
            Wait.TimeRemaining = .Parameters.AddTransferRinseTime
            State = S52.WaitWashSTank
          End If

          'rinse the tank
        Case S52.WaitWashSTank
          StateString = If(.Language = LanguageValue.ZhTw, "C缸洗缸", "Rinsing C ") & TimerString(Wait.TimeRemaining)
          If Not Wait.Finished Then Exit Select
          Wait.TimeRemaining = .Parameters.AddCirculateTimeAfterRinse
          State = S52.WaitMixCir1

        Case S52.WaitMixCir1
          StateString = If(.Language = LanguageValue.ZhTw, "C缸循環中", "Tank C circulating ") & TimerString(Wait.TimeRemaining)
          If Wait.Finished Then
            Wait.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
            State = S52.WaitNotTankLowLevel2
          End If

          'transfer the rinse
        Case S52.WaitNotTankLowLevel2
          StateString = If(.Language = LanguageValue.ZhTw, "C缸加藥中", "Transferring C ") & TimerString(Wait.TimeRemaining)
          If .CTankLowLevel Then Wait.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
          If Wait.Finished And Not .CTankLowLevel Then
            Wait.TimeRemaining = .Parameters.MixCirculateRinseTime
            State = S52.WaitWashSTank2
          End If

          'rinse the tank to drain
        Case S52.WaitWashSTank2
          StateString = If(.Language = LanguageValue.ZhTw, "C缸洗缸", "Rinsing C ") & TimerString(Wait.TimeRemaining)
          If Not Wait.Finished Then Exit Select
          Wait.TimeRemaining = .Parameters.AddCirculateTimeAfterRinse
          State = S52.WaitMixCir2

        Case S52.WaitMixCir2
          StateString = If(.Language = LanguageValue.ZhTw, "C缸循環中", "Tank C circulating ") & TimerString(Wait.TimeRemaining)
          If Wait.Finished Then
            Wait.TimeRemaining = .Parameters.AddTransferDrainTime
            State = S52.WaitAddFinish3
          End If


          'drain the tank
        Case S52.WaitAddFinish3
          StateString = If(.Language = LanguageValue.ZhTw, "C缸排水", "Draining C ") & TimerString(Wait.TimeRemaining)
          If .CTankLowLevel Then Wait.TimeRemaining = .Parameters.AddTransferDrainTime
          If Wait.Finished And Not .CTankLowLevel Then
            .MessageSTankDiluteAddingNow = False
            .TemperatureControl.Cancel()
            .TempControlFlag = False
            State = S52.Off
            .TankCReady = False 'set the ready to false
          End If

        Case S52.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停 ", "Paused ") & " " & TimerString(Wait.TimeRemaining)
          'no longer pause restart the timer and go back to the wait state
          If (Not .Parent.IsPaused) And .IO.MainPumpFB And .IO.SystemAuto Then
            State = StateWas
            StateWas = S52.Off
            Wait.Restart()
          End If

      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S52.Off
    Wait.Cancel()
    RunBackWait.Cancel()
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
      Return State <> S52.Off
    End Get
  End Property

  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S52
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S52
  Public Property State() As S52
    Get
      Return state_
    End Get
    Private Set(ByVal value As S52)
      state_ = value
    End Set
  End Property
  Public Property StateWas() As S52
    Get
      Return statewas_
    End Get
    Private Set(ByVal value As S52)
      statewas_ = value
    End Set
  End Property

  Public ReadOnly Property IsTankInterlocked() As Boolean
    Get
      Return ((State = S52.WaitNoAddButtons) Or (State = S52.WaitTempSafe) Or (State = S52.WaitSystemAuto))
    End Get
  End Property
  Public ReadOnly Property IsActive() As Boolean
    Get
      Return State > S52.WaitNoAddButtons
    End Get
  End Property
  Public ReadOnly Property IsFillingCirc() As Boolean
    Get
      Return ((State = S52.WaitHighLevel) Or (State = S52.WaitAddTimeHold) Or (State = S52.WaitAddTimeRunback))
    End Get
  End Property
  Public ReadOnly Property IsWaitingForPrepare() As Boolean
    Get
      Return (State = S52.WaitTankReady)
    End Get
  End Property

  Public ReadOnly Property IsTransfer() As Boolean
    Get
      Return ((State = S52.WaitAddTimeHold) Or (State = S52.WaitAddTimeTranfer) Or (State = S52.WaitNotTankLowLevel) Or _
              (State = S52.WaitNotTankLowLevel2))
    End Get
  End Property
  Public ReadOnly Property IsMixing() As Boolean
    Get
      Return ((State = S52.WaitTankReady) Or (State = S52.WaitAddTimeRunback) Or (State = S52.WaitAddTimeHold) Or (State = S52.WaitAddTimeTranfer) Or (State = S52.WaitMixCir1) Or (State = S52.WaitMixCir2) Or (State = S52.WaitHighLevel) Or (State = S52.WaitWashSTank) Or (State = S52.WaitWashSTank2))
    End Get
  End Property
  Public ReadOnly Property IsCirculating() As Boolean
    Get
      Return ((State = S52.WaitTankReady) Or (State = S52.WaitAddTimeRunback) Or (State = S52.WaitAddTimeHold) Or (State = S52.WaitAddTimeTranfer) Or (State = S52.WaitMixCir1) Or (State = S52.WaitMixCir2) Or (State = S52.WaitWashSTank) Or (State = S52.WaitWashSTank2))
    End Get
  End Property
  Public ReadOnly Property IsRinsing() As Boolean
    Get
      Return ((State = S52.WaitWashSTank) Or (State = S52.WaitWashSTank2))
    End Get
  End Property
  Public ReadOnly Property IsDraining() As Boolean
    Get
      Return (State = S52.WaitAddFinish3)
    End Get
  End Property
  Public ReadOnly Property IsPaused() As Boolean
    Get
      Return State = S52.Pause
    End Get
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command52 As New Command52(Me)
End Class
#End Region
