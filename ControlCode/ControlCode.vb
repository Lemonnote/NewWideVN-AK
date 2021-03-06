Imports System.IO
Imports System.Data.SqlClient
Public NotInheritable Class ControlCode

  Inherits MarshalByRefObject
  Implements ACControlCode

  Public Parent As ACParent
  Friend Language As LanguageValue
  Public TemperatureControl As New TemperatureControl
  'Public Recipe As New Recipe
  Public 程式執行小時 As Integer
  Public 程式執行分鐘 As Integer
  Public LowLevel As Boolean
  Public MiddleLevel As Boolean
  Public HighLevel As Boolean
  Public OverflowLevel As Boolean
  Public MainTankVolume As Integer
  Public TotalWeight As Integer
  Public LiquidRatio As Integer
  Public TotalWeightWas As Integer
  Public LiquidRatioWas As Integer
  Public ShowTotalVolume As Integer
  Public TargetVolume As Integer
  Public CheckCounter As Integer
  Public CheckCounterTimer As New Timer
  Public ATankFillHot As Boolean
  Public Setpoint As Integer
  Public BTankHighLevel As Boolean
  Public BTankMiddleLevel As Boolean
  Public BTankLowLevel As Boolean
  Public BTankHeatStartRequest As Boolean
  Public TankBReady As Boolean
  Public TankBMixOn As Boolean
  Public CTankHighLevel As Boolean
  Public CTankMiddleLevel As Boolean
  Public CTankLowLevel As Boolean
  Public CTankHeatStartRequest As Boolean
  Public TankCMixOn As Boolean
  Public PumpOn As Boolean
  Public ReelOn As Boolean
  Public TankCReady As Boolean
  Public CoolNow As Boolean
  Public HeatNow As Boolean
  Public TemperatureControlFlag As Boolean
  Public RemoteDisplayError As Boolean
  Public FlashFlag As Boolean
  Public FlashFlag2 As Boolean
  Public PumpSpeed As Integer
  Public Roller1Speed As Integer
  Public Roller2Speed As Integer
  Public Roller3Speed As Integer
  Public Roller4Speed As Integer
  Public PressureInTemp As Integer
  Public PressureOutTemp As Integer
  Public PressureOutTimes As Integer
  Public BAddStop As Boolean
  Public CAddStop As Boolean
  Public TempControlFlag As Boolean
  Public PressureIn As Boolean
  Public HeatValve As Boolean
  Public CoolValve As Boolean
  Public TemperatureControlTimer As New Timer
  Public CoolValveOpenTimer As New Timer
  Public HeatValveOpenTimer As New Timer
  Public TemperatureControlTime As Integer
  Public CoolValveOpenTime As Integer
  Public HeatValveOpenTime As Integer
  Public FlowMeterCount As Integer
  Public FlowMeterCountOn As Boolean
  Public RecallLevel As Integer
  Public MainTankTargetVolume As Integer
  Public MainTankActualVolume As Integer
  Public HoldTemperature As Integer
  Public CallMessage As Boolean
  Public SafeToAdd As Boolean
  Public ReelOnRequest As Boolean
  Public ReelOnWas As Boolean
  Public ReelOnRequest_Timer As New Timer
  Public ReelOffRequest As Boolean
  Public ReelOffWas As Boolean
  Public ReelOffRequest_Timer As New Timer
  Public PumpOffRequest As Boolean
  Public PumpOffWas As Boolean
  Public PumpOffRequest_Timer As New Timer
  Public EntanglementWas As Boolean
  Public EntanglementKeepTimer As New Timer
  Public PumpOnRequest As Boolean
  Public PumpOnWas As Boolean
  Public PumpOnRequest_Timer As New Timer
  Public NozzleGap As String
  Public NozzleSize As String
  Public ColdWaterCooling As Boolean
  Public BHeatTemperature As Integer
  Public CHeatTemperature As Integer
  Public Mimic_啟動進冷水 As Boolean
  Public 啟動排水 As Boolean
  Public SteamFlowmeterSampleTimer As New Timer
  Public SteamUsage As Integer
  Public SetHotCoolingTemperature As Integer = 150
  Public SetWarmCoolingTemperature As Integer = 150
  Public P_PhShowData As Integer = 0
  Public HotCooling As Integer = 0


  '連接SPC資料庫用的變數
  Public SPCConnectTimer As New Timer
  Public SPCConnectError As Boolean
  Public ComputerName As String
  Public SQL連線狀況 As Integer
  Public SQL連線狀況1 As Integer
  Public ProgramStopCleanDatabase As Boolean


  Public cs_DispenseState As String
  Public qs_DispenseState As String
  Public cn_DispenseState As SqlClient.SqlConnection
  Public cmd_DispenseState As SqlClient.SqlCommand
  Public da_DispenseState As SqlClient.SqlDataAdapter
  Public cb_DispenseState As SqlClient.SqlCommandBuilder
  Public ds_DispenseState As DataSet
  Public dt_DispenseState As DataTable

  Public cs_Recipe As String
  Public qs_Recipe As String
  Public cn_Recipe As SqlClient.SqlConnection
  Public cmd_Recipe As SqlClient.SqlCommand
  Public da_Recipe As SqlClient.SqlDataAdapter
  Public cb_Recipe As SqlClient.SqlCommandBuilder
  Public ds_Recipe As DataSet
  Public dt_Recipe As DataTable
  'Public SPCServerName As String = ".\SQLEXPRESS"
  'Public SPCUserName As String = "sa"
  'Public SPCPassword As String = "1234"
  Public SPCServerName As String
  Public SPCUserName As String
  Public SPCPassword As String
  Public StepNumber1(30) As String
  Public ProductCode(30) As String
  Public ProductType(30) As String
  Public Grams(30) As String
  Public DispenseGrams(30) As String
  Public DispenseResult(30) As String

  '染料資料
  Public DyeStepNumber(10) As String
  Public DyeCode(10) As String
  Public DyeGrams(10) As String
  Public DyeDispenseGrams(10) As String
  Public DyeDispenseResult(10) As String
  '助劑資料
  Public ChemicalStepNumber(20) As String
  Public ChemicalCode(20) As String
  Public ChemicalGrams(20) As String
  Public ChemicalDispenseGrams(20) As String
  Public ChemicalDispenseResult(20) As String
  '==========================================================

  '呼叫252跟302用變數
  Public ChemicalEnabled As Integer
  Public ChemicalCallOff As Integer            'We fill in this value - prepare commands
  Public ChemicalTank As Integer               'We fill in this value - prepare commands
  Public ChemicalState As Integer              'This value is filled in by host / auto dispenser
  Public ChemicalProducts As String            'This value is filled in by host / auto dispenser
  Public DyeEnabled As Integer
  Public DyeCallOff As Integer                 'We fill in this value - prepare commands
  Public DyeTank As Integer                    'We fill in this value - prepare commands
  Public DyeState As Integer                   'This value is filled in by host / auto dispenser
  Public DyeProducts As String                 'This value is filled in by host / auto dispenser
  Public Call252AddDye As Boolean
  Public Wait252Scheduled As Boolean
  Public CallFor302D As Boolean
  Public WaitFor302D As Boolean
  Public RunCallLA252 As Boolean
  Public LA252Ready As Boolean
  Public RunCallLA302 As Boolean
  Public LA302Ready As Boolean
  Public UpdatePowderDispenseResult As Boolean

  '======
  Public 粉體步驟 As Integer
  Public 粉體藥桶 As Integer

  '跟SPC溝通用的變數
  Public CCallOff As String
  Public CTank As String
  Public CState As String
  Public CEnabled As String
  Public DCallOff As String
  Public DTank As String
  Public DState As String
  Public DEnabled As String
  Public 粉體呼叫 As String

  Public 工單 As String
  Public 重染 As Integer

  '自動備藥訊息
  Public DyeStepDispensing(12) As Boolean
  Public ChemicalStepDispensing(12) As Boolean
  Public DyeStepReady(12) As Boolean
  Public ChemicalStepReady(12) As Boolean
  Public StopProgram As Boolean
  Public NotAllowLoadProgramAlarmTimer As New Timer



  '布頭訊號用的變數
  Public FabricCycleInput1Times As Integer
  Public FabricCycleInput2Times As Integer
  Public FabricCycleInput3Times As Integer
  Public FabricCycleInput4Times As Integer
  Public FabricCycleInput5Times As Integer
  Public FabricCycleInput6Times As Integer
  Public FabricCycleInput7Times As Integer
  Public FabricCycleInput8Times As Integer
  Public FabricCycle1FirstInput As Boolean
  Public FabricCycle2FirstInput As Boolean
  Public FabricCycle3FirstInput As Boolean
  Public FabricCycle4FirstInput As Boolean
  Public FabricCycle5FirstInput As Boolean
  Public FabricCycle6FirstInput As Boolean
  Public FabricCycle7FirstInput As Boolean
  Public FabricCycle8FirstInput As Boolean
  Public FabricCycleInput1Was As Boolean
  Public FabricCycleInput2Was As Boolean
  Public FabricCycleInput3Was As Boolean
  Public FabricCycleInput4Was As Boolean
  Public FabricCycleInput5Was As Boolean
  Public FabricCycleInput6Was As Boolean
  Public FabricCycleInput7Was As Boolean
  Public FabricCycleInput8Was As Boolean
  Public FabricCycleTime1 As Integer
  Public FabricCycleTime2 As Integer
  Public FabricCycleTime3 As Integer
  Public FabricCycleTime4 As Integer
  Public FabricCycleTime5 As Integer
  Public FabricCycleTime6 As Integer
  Public FabricCycleTime7 As Integer
  Public FabricCycleTime8 As Integer
  Public FabricCycleTimeCount1 As Integer
  Public FabricCycleTimeCount2 As Integer
  Public FabricCycleTimeCount3 As Integer
  Public FabricCycleTimeCount4 As Integer
  Public FabricCycleTimeCount5 As Integer
  Public FabricCycleTimeCount6 As Integer
  Public FabricCycleTimeCount7 As Integer
  Public FabricCycleTimeCount8 As Integer
  Public FabricCycleTimer1 As New TimerUp
  Public FabricCycleTimer2 As New TimerUp
  Public FabricCycleTimer3 As New TimerUp
  Public FabricCycleTimer4 As New TimerUp
  Public FabricCycleTimer5 As New TimerUp
  Public FabricCycleTimer6 As New TimerUp
  Public FabricCycleTimer7 As New TimerUp
  Public FabricCycleTimer8 As New TimerUp
  Public StartCycleTimeRecord As Boolean


  'For mimic
  Public Mimic_BTemperature As Integer

  '檢查溫度控制時，主缸溫度是否有變化，用以判定蒸氣不足或是冷卻水不足
  Public MainTempStopChange As Boolean
  Public CheckMainTempTimer As New Timer

  Public FirstScanDone As Boolean
  Public PumpStartRequest As Boolean
  Public PumpStopRequest As Boolean
  Public btankreadywas As Boolean
  Public ctankreadywas As Boolean
  Public slowflash As New Flasher
  'machine idle time stuf
  Public ProgramStoppedTimer As New TimerUp                       'Program Stopped Timer
  Public ProgramStoppedTime As Integer
  Public CycleTime As Integer
  Public DispenseStep As Integer
  Public DispenseTank As Integer


  'pH control
  Public pHset As Double
  Public MathHacTimes As New TimerUp
  Public MathHacFlag As Boolean
  Public UseHacThisValue As Integer
  Public UseHacAllTotal, UseHacAllTotal2 As Integer

  Public PhToCPump As Boolean 'PH回流專用的加藥泵
  Public PhToAdd As Boolean 'PH回流專用的加藥閥
  Public PhToDrain As Boolean 'PH回流專用的排水閥

  Public test1, test2, test3, test4, test5, test6, test7, test8, test9, test10, test11, test12,
test13, test14, test15, test17, test18, test19, test20, test21, test22, test23, test26, test27, test28, test31, test32, test33, test34, test36 As Double
  Public 補償狀態 As String
  Public test24, test16 As String
  Public test25, test29, test30, test35 As Boolean
  Public PhShowData As String
  Public P11, P22 As Double
  Private DelayAddTime As New Timer
  Private DelayAddHac As Boolean
  Public PH再檢測 As Integer
  Public PH檢測_短時間內不在檢測 As Boolean
  Public PH再檢測時間 As New Timer
  Public PhCirRun As Boolean
  Public 已經確保120秒檢測完成, 短時間內重新執行, 是否縮短檢測時間 As Boolean
  Public 加酸時間 As New Timer
  Public 加酸次數 As Integer
  Public 補酸狀態分析 As Integer
  Public pH加酸長時間無動作計時 As New Timer
  Public pH加酸長時間無動作flag As Boolean
  Public 記錄pH之前變化值 As Short
  Public 主程式完成pH未完成計時 As New Timer
  Public 主程式完成pH未完成flag As Boolean
  Public pHSensor超過安全溫度計時 As New Timer
  Public pHSensor超過安全溫度flag As Boolean
  Public pHSensor超過安全溫度警報flag As Boolean
  Public F76專用旗標 As Boolean
  Public PhFillLevel As Integer
  Public PhControlFlag As Boolean
  Public PhAutoWashTimer As New Timer
  Public PhAutoWashBDrainTimer As New Timer
  Public PhAutoWash As Boolean
  Public PhAutoWashWas As Boolean
  Public PhDosingAdjustTimer As New Timer
  Public StepNumber As Integer
  'pH參數
  Public ConnectPhSystem As Integer = 0
  Public PhConcentration As Integer = 100
  Public PhCirTank As Integer = 1
  Public PhAdjustCheckTime As Integer = 20
  Public PhWashTime As Integer = 20
  Public CirDrainDelayTime As Integer = 20
  Public CirFillDelayTime As Integer = 10
  Public PhCoolingTemp As Integer = 40
  Public PH檢測筒溫度過高 As Integer = 100
  Public PH加酸安全溫度 As Integer = 85
  Public PhSamplingTime As Integer = 60
  Public DoublePhSample As Integer = 30
  Public PhApproach As Integer = 2
  Public PhPumpOutRatio As Integer = 300
  Public MainTankFillLevel As Integer = 2000
  Public StartCheckPh As Integer = 10
  Public PhControlMode As Integer = 1
  Public CirculateOpenAdd As Integer = 10
  Public DelayCirculatPump As Integer = 4
  Public PhCirRuning As Integer = 0
  Public PhControlToLongTime As Integer = 600
  Public PhDosing As Integer = 600
  Public PhAutoWashTime As Integer = 10


  '主缸類比水位校正
  Public SetMainTankAnalogInput(50) As Integer
  Public SetMainTankVolume(50) As Integer

  'BC tank 
  Public BDosingOn As Boolean
  Public BAllinOn As Boolean
  Public BCirMixOn As Boolean
  Public BInjOn As Boolean
  Public BPumpOn As Boolean
  'Public BHeatTemperature As Integer
  Public B_ManualMixing As Boolean
  Public CDosingOn As Boolean
  Public CAllinOn As Boolean
  Public CCirMixOn As Boolean
  Public CInjOn As Boolean
  Public CPumpOn As Boolean

  Public TotalVolume As Integer
  Public TotalVolumeWas As Integer

  Public Sub New(ByVal parent As ACParent)
    Me.Parent = parent
    Select Case parent.CultureName
      Case "zh-TW" : Language = LanguageValue.ZhTw
      Case "zh" : Language = LanguageValue.ZhTw
      Case "zh-CN" : Language = LanguageValue.ZhCn
      Case Else : Language = LanguageValue.English
    End Select
  End Sub

  Public Sub StartUp() Implements ACControlCode.StartUp
    If True Then  ' Set to True to start in debug mode
      ' Parent.Mode = Mode.Debug
      St.Load()
      Dim readText As String = My.Computer.FileSystem.ReadAllText("Setup.ini")
      Dim objStreamReader As StreamReader
      Dim strLine As String
      Dim i As Integer = 0

      'Pass the file path and the file name to the StreamReader constructor.
      objStreamReader = New StreamReader("Setup.ini")

      'Read the first line of text.
      strLine = objStreamReader.ReadLine

      'Continue to read until you reach the end of the file.
      Do While Not strLine Is Nothing
        i = i + 1

        If i = 1 Then
          SPCServerName = strLine
        ElseIf i = 2 Then
          SPCUserName = strLine
        ElseIf i = 3 Then
          SPCPassword = strLine
        End If
        'Write the line to the Console window.
        Console.WriteLine(strLine)
        'Read the next line.
        strLine = objStreamReader.ReadLine
      Loop

    End If
  End Sub

  Public Sub ShutDown() Implements ACControlCode.ShutDown
  End Sub

  Public Sub Run() Implements ACControlCode.Run

    'calculate main tank and side tank level 
    Dim LevelNumber As Integer
    For LevelNumber = 1 To 50
      If SetMainTankVolume(LevelNumber) > 0 And SetMainTankAnalogInput(LevelNumber) > 0 And IO.MainTankLevel <= SetMainTankAnalogInput(LevelNumber) Then
        MainTankActualVolume = Minimum(SetMainTankVolume(LevelNumber - 1) + CInt((SetMainTankVolume(LevelNumber) - SetMainTankVolume(LevelNumber - 1)) / (SetMainTankAnalogInput(LevelNumber) - SetMainTankAnalogInput(LevelNumber - 1))) * (IO.MainTankLevel - SetMainTankAnalogInput(LevelNumber - 1)), 0)
        Exit For
      End If
    Next


    If SteamFlowmeterSampleTimer.Finished Then
      SteamFlowmeterSampleTimer.TimeRemainingMs = Parameters.SteamFlowmeterSampleTime
      SteamUsage = IO.SteamFlowmeter + SteamUsage
    End If

    'machine idle time stuff
    If Not FirstScanDone Then ProgramStoppedTimer.Start()

    'Program running state changes
    Static WasProgramRunning As Boolean
    If Parent.IsProgramRunning Then            'A Program is running
      Static ProgramRunTimer As New TimerUp  ' program run timer
      CycleTime = ProgramRunTimer.TimeElapsed
      If Not WasProgramRunning Then     'A Program has just started
        ProgramStoppedTime = ProgramStoppedTimer.TimeElapsed
        ProgramStoppedTimer.Pause()
        ProgramRunTimer.Start()
      End If
    Else
      If WasProgramRunning Then         'A program has just finished
        ProgramStoppedTimer.Start()
        TemperatureControl.Cancel()
      End If
      CycleTime = 0                     'No Program is running
      '  TankBReady = False
      '  TankCReady = False
    End If
    WasProgramRunning = Parent.IsProgramRunning
    程式執行分鐘 = (CycleTime - 程式執行小時 * 3600) \ 60
    程式執行小時 = CycleTime \ 3600

    Dim halt = Parent.IsPaused  ' Or IO_EStop_PB Or (Not ReadInputs_Succeeded)
    Dim NHalt = Not halt
    If TempControlFlag = True Then TemperatureControl.Run(IO.MainTemperature)
    TemperatureControl.CheckErrorsAndMakeAlarms(Me)
    Setpoint = TemperatureControl.TempSetpoint  ' keep a copy to show on graph

    If TempControlFlag = False Then TemperatureControl.Cancel()

    If halt Or (Not IO.SystemAuto) Then TemperatureControl.pidPause()

    'On/Off昇降溫閥控制
    If (Command01.IsOn Or Command56.IsHolding Or Command57.IsHolding) And NHalt Then
      CoolValve = Not TemperatureControlTimer.Finished And Not CoolValveOpenTimer.Finished And CoolNow
      HeatValve = Not TemperatureControlTimer.Finished And Not HeatValveOpenTimer.Finished And HeatNow And (Parameters.HeatValveType = 0)

      If CoolNow Then
        TemperatureControlTime = 10
        CoolValveOpenTime = Math.Min(Math.Max((TemperatureControl.Output * 10 \ 1000), 0), 10)
        If TemperatureControlTimer.Finished Then
          TemperatureControlTimer.TimeRemaining = TemperatureControlTime
          If CoolValveOpenTimer.Finished Then
            CoolValveOpenTimer.TimeRemaining = CoolValveOpenTime
          End If
        End If
      ElseIf HeatNow And Parameters.HeatValveType = 0 Then
        TemperatureControlTime = 10
        HeatValveOpenTime = Math.Min(Math.Max((TemperatureControl.Output * 10 \ 1000), 0), 10)
        If TemperatureControlTimer.Finished Then
          TemperatureControlTimer.TimeRemaining = TemperatureControlTime
          If HeatValveOpenTimer.Finished Then
            HeatValveOpenTimer.TimeRemaining = HeatValveOpenTime
          End If
        End If
      End If
    End If

    '流量計計算
    If IO.PulseFB And FlowMeterCountOn = False Then
      FlowMeterCount = FlowMeterCount + 1
      FlowMeterCountOn = True
    End If
    If Not IO.PulseFB Then FlowMeterCountOn = False

    'set up a flasher
    slowflash.Flash(FlashFlag, 800)

    'set up a flasher2
    slowflash.Flash(FlashFlag2, 1600)

    'run pressure out control when cooling to open up the pressure out when temp becomes ok
    'run the pressure control 
    'set the pressureouttemp to the parameter on boot
    If Not FirstScanDone Then
      'pressure out temp
      If Parameters.SetPressureOutTemp > 0 Then
        PressureOutTemp = Parameters.SetPressureOutTemp * 10
        PressureInTemp = Parameters.SetPressureOutTemp * 10
      Else
        PressureOutTemp = 850   '預設排壓溫度為85
        PressureInTemp = 850    '預設加壓溫度為85
      End If
      If PressureOutTemp >= 950 Then PressureOutTemp = 950
      If PressureInTemp >= 950 Then PressureInTemp = 950
    End If
    PressureOutTimes = Parameters.SetPressureOutTimes
    If PressureOutTimes <= 10 Then PressureOutTimes = 10

    If HeatNow And IO.MainTemperature > PressureInTemp Then
      PressureIn = True
    ElseIf Not HeatNow Then
      PressureIn = False
    End If

    PressureOut.Run()

    'manual pump pushbuttons
    'If IO.MainPumpOnPB Then
    '  PumpOn = True
    'End If
    'If IO.MainPumpOffPB Then
    '  PumpOn = False
    'End If

    ' Make run and halt(=pause) push-buttons work
    Parent.PressButtons(IO.RemoteRun, IO.RemoteHalt, StopProgram, False, False)

    Alarms.HighTempNoFill = NHalt And (IO.ColdFill Or IO.HotFill) And IO.MainTemperature > (Parameters.SetSafetyTemp * 10)
    Alarms.HighTempNoAdd = NHalt And (IO.BTankAddition Or IO.CTankAddition Or IO.BDosing) And IO.MainTemperature > (Parameters.SetAddSafetyTemp * 10)
    Alarms.HighTempNoDrain = NHalt And IO.Drain And IO.MainTemperature > (Parameters.SetSafetyTemp * 10)
    Alarms.TerminalError = RemoteDisplayError
    Alarms.PlcError = IO.PlcFault And Parent.Mode <> Mode.Debug  ' no plc error in debug mode
    '  Alarms.Pt1Open = (io.MainTemperature > Parameters.pt1Highlimit)
    '  Alarms.Pt1Short = (io.MainTemperature < Parameters.pt1lowlimit)
    Alarms.InsufficientSteam = MainTempStopChange And HeatNow
    Alarms.CoolingNotEnough = MainTempStopChange And CoolNow
    Alarms.MainPumpError = (HeatNow Or CoolNow) And Not IO.MainPumpFB

    '  Static AddPumpErrorAlarmDelay As New DelayTimer
    '      Alarms.AddPumpError = AddPumpErrorAlarmDelay.Run(IO.BTankAddPump And Not IO.BPumpFB, 3)

    Static delay2 As New DelayTimer
    Alarms.MainPumpOverload = delay2.Run(IO.MainPumpOverload, 3)
    Alarms.AddMotorOverload = IO.BPumpOverload
    Alarms.ManualOperation = Not IO.SystemAuto

    'Alarms.MainElectricFanError = IO.FanError
    Alarms.FabricStop = IO.Entanglement
    Alarms.FabricStopWas = EntanglementWas And Not IO.Entanglement

    If IO.Entanglement Then
      EntanglementWas = True
    ElseIf (Not IO.Entanglement And EntanglementKeepTimer.Finished) Or IO.CallAck Then
      EntanglementWas = False
    End If

    If Not EntanglementWas Then EntanglementKeepTimer.TimeRemaining = 30

    Alarms.STankNotReady = Command56.IsWaitingForPrepare Or Command57.IsWaitingForPrepare Or
                               Command66.IsWaitingForPrepare Or Command67.IsWaitingForPrepare
    '溫度異常警報
    Alarms.Pt1Open = IO.MainTemperature < 50
    Alarms.Pt1Short = IO.MainTemperature > 1400

    'calculate side tank level 
    Dim i As Integer
    For i = 0 To 50
      If (SetMainTankVolume(i) > 0 And SetMainTankAnalogInput(i) > 0) Or i = 0 Then
        If IO.MainTankLevel >= SetMainTankAnalogInput(i) Then
          MainTankVolume = SetMainTankVolume(i)
        End If
      End If
    Next i

    LowLevel = IO.LowLevel
    MiddleLevel = IO.MiddleLevel
    HighLevel = IO.HighLevel
    OverflowLevel = IO.OverflowLevel
    BTankLowLevel = IO.BTankLow Or IO.TankBLevel >= 50                 '(IO.TankBLevel >= 50)
    BTankMiddleLevel = IO.BTankMiddle Or IO.TankBLevel >= 500           '(IO.TankBLevel >= 500)
    BTankHighLevel = IO.BTankHigh Or IO.TankBLevel >= 1000            '(IO.TankBLevel >= 1000)
    CTankLowLevel = IO.CTankLow Or IO.TankCLevel >= 50
    CTankMiddleLevel = IO.CTankMiddle Or IO.TankCLevel >= 500
    CTankHighLevel = IO.CTankHigh Or IO.TankCLevel >= 1000

    'tank ready pushbuttons
    '  If IO.BTankReady And Not btankreadywas And Not TankBReady Then
    ' TankBReady = Not TankBReady
    '     End If
    ' btankreadywas = IO.BTankReady
    If Not BTankLowLevel Then TankBReady = False
    ' If IO.CTankReady And Not ctankreadywas And Not TankCReady Then
    ' TankCReady = Not TankCReady
    ' End If
    ' ctankreadywas = IO.CTankReady
    If Not CTankLowLevel Then TankCReady = False

    '停止熱水降溫
    If CoolNow And IO.MainTemperature < SetHotCoolingTemperature * 10 Then
      ColdWaterCooling = True
    ElseIf Not CoolNow Then
      ColdWaterCooling = False
    End If

    'digital outputs 
    IO.Heat = NHalt And TemperatureControl.IsHeating And IO.MainPumpFB And ((HeatNow And Parameters.HeatValveType = 1 And IO.MainTemperature < Setpoint + 20 And IO.TemperatureControl > 0) Or HeatValve)
    If HotCooling = 1 Then
      IO.Cool = False
    Else
      IO.Cool = IO.ColdWaterCooling
    End If
    If SetHotCoolingTemperature > 0 And CoolNow Then
      If IO.MainTemperature >= SetHotCoolingTemperature * 10 And Not ColdWaterCooling Then
        IO.HotWaterCooling = NHalt And TemperatureControl.IsCooling And IO.MainPumpFB And CoolValve
      Else
        IO.ColdWaterCooling = NHalt And (TemperatureControl.IsCooling And IO.MainPumpFB And (CoolValve Or (CoolNow And Parameters.CoolValveType = 1))) Or Command12.IsCoolFill Or
                  Command11.IsCoolFill Or Command13.IsCoolFill Or Command16.IsCoolFill
        IO.HotWaterCooling = False
      End If
    Else
      IO.ColdWaterCooling = NHalt And (TemperatureControl.IsCooling And IO.MainPumpFB And (CoolValve Or (CoolNow And Parameters.CoolValveType = 1))) Or Command12.IsCoolFill Or
                Command11.IsCoolFill Or Command13.IsCoolFill Or Command16.IsCoolFill
      IO.HotWaterCooling = False
    End If

    If Not CoolNow Then
      IO.HotWaterCooling = False
      IO.ColdWaterCooling = False
    End If

    '升溫時開熱交換器排水閥，熱交換器排水延遲時間到達則改開排冷凝水閥
    Static HxDrainDelay As New DelayTimer

    IO.CondenserDrain = NHalt And HxDrainDelay.Run(HeatNow, Parameters.CondensationDelayTime)
    IO.HxDrain = Not (IO.CondenserDrain Or IO.CoolDrain Or CoolNow Or Command11.IsOn Or Command12.IsOn Or
                      Command13.IsOn Or Command16.IsOn)
    IO.CoolDrain = NHalt And CoolNow
    IO.PressureIn = NHalt And HeatNow And Not IO.PressureSw And PressureIn And
                        IO.MainTemperature < 1200
    IO.PressureOut = (PressureOut.IsDepressurising Or PressureOut.IsDepressurised) And Not PressureIn And Not (HeatNow And Parameters.AllowPressureOutAsHeating * 10 < IO.MainTemperature)
    IO.Overflow = NHalt And (Command11.IsDrainingToLowLevel Or Command12.IsRinsing Or
                                 Command14.IsOverflowDrain Or Command16.IsOverFlowDrain)
    IO.ColdFill = NHalt And (Command03.IsFillCold Or Command04.IsFillCold Or Command11.IsFillCold Or
                             Command12.IsFillCold Or Command13.IsFillCold Or Command19.IsFillCold Or Command17.IsFillCold)
    IO.HotFill = NHalt And (Command03.IsFillHot Or Command04.IsFillHot Or Command11.IsFillHot Or
                        Command12.IsFillHot Or Command13.IsFillHot Or Command19.IsFillHot Or Command17.IsFillHot)
    IO.Fill3 = NHalt And (Command04.IsFill3 Or Command17.IsFill3 Or Command03.IsFill3 Or Command19.IsFill3)
    IO.Drain = NHalt And (Command11.IsDrainingToLowLevel Or Command11.IsDrainingEmpty Or
                      Command13.IsDrainToMiddleLevel Or Command14.IsColdDrain Or Command32.IsDrain Or 啟動排水)
    IO.HotDrain = NHalt And (Command11.IsDrainingToLowLevel Or Command11.IsDrainingEmpty Or
                         Command14.IsHotDrain Or Command32.IsHotDrain)
    IO.PowerDrain = NHalt And Command32.IsPowerDrain


    IO.PumpOn = PumpStartRequest Or PumpOnRequest
    IO.PumpOff = PumpStopRequest Or PumpOffRequest

    IO.BTankDrain = (NHalt And (Command51.IsDraining Or Command56.IsDraining Or Command61.IsDraining Or Command66.IsDraining Or Command58.IsDraining Or
                                Command41.IsBTankDrain Or Command43.IsBTankDrain))

    IO.BDosing = (NHalt And (Parameters.BCTank0TwoPump1OnePump = 0 And BDosingOn) Or (Parameters.BCTank0TwoPump1OnePump = 1 And (BDosingOn Or CDosingOn)))
    IO.BTankAddPump = ((Parameters.BCTank0TwoPump1OnePump = 0 And BPumpOn) Or (Parameters.BCTank0TwoPump1OnePump = 1 And (BPumpOn Or CPumpOn)))
    IO.BTankMixCir2 = (NHalt And (Command54.IsMixingForTime Or TankBMix.IsMixingForTime Or Command56.IsCirculating Or
                      Command66.IsCirculating Or Command51.IsMixing Or Command61.IsMixing Or B_ManualMixing Or Command58.IsCirculating Or Command51.IsRinsing Or Command61.IsRinsing))
    IO.BTankColdFill = (NHalt And (Command54.IsFillingFresh Or Command64.IsFillingFresh Or Command58.IsRinsing Or
                       Command51.IsRinsing Or Command56.IsRinsing Or Command61.IsRinsing Or Command66.IsRinsing Or Command24.IsFilling))
    IO.BTankCirculate2 = NHalt And (Command54.IsFillingCirc Or Command64.IsFillingCirc Or Command51.IsFillingCirc Or
                                    Command61.IsFillingCirc)
    IO.BTankAddition = ((Parameters.BCTank0TwoPump1OnePump = 0 And BAllinOn) Or (Parameters.BCTank0TwoPump1OnePump = 1 And BInjOn))
    'IO.BTankOkLamp = ((Command54.IsSlow And FlashFlag) Or (Command64.IsSlow And FlashFlag) Or _
    '                 (Command51.IsWaitingForPrepare And FlashFlag) Or (Command61.IsWaitingForPrepare And FlashFlag) Or TankBReady) And BTankLowLevel
    IO.BTankHeat = NHalt And BTankHeatStartRequest And BTankLowLevel And (IO.BTankTemperature < BHeatTemperature) And IO.TankBLevel > 300

    '加藥溫度保護
    SafeToAdd = IO.MainTemperature < Parameters.SetAddSafetyTemp * 10

    IO.CTankDrain = NHalt And (Command52.IsDraining Or Command57.IsDraining Or Command62.IsDraining Or Command67.IsDraining Or
                               Command41.IsCTankDrain Or Command43.IsCTankDrain)
    IO.CDosing = (Parameters.BCTank0TwoPump1OnePump = 0 And CDosingOn) Or
                     (Parameters.BCTank0TwoPump1OnePump = 1 And (BAllinOn Or CAllinOn))
    IO.CTankAddPump = Parameters.BCTank0TwoPump1OnePump = 0 And CPumpOn
    IO.CTankMixCir2 = NHalt And (Command55.IsMixingForTime Or TankCMix.IsMixingForTime Or Command57.IsCirculating Or Command67.IsCirculating Or Command52.IsMixing Or Command62.IsMixing Or Command52.IsRinsing Or Command62.IsRinsing)
    IO.CTankColdFill = NHalt And (Command55.IsFillingFresh Or Command65.IsFillingFresh Or
                                      Command52.IsRinsing Or Command57.IsRinsing Or Command62.IsRinsing Or Command67.IsRinsing)
    IO.CTankCirculate2 = NHalt And (Command55.IsFillingCirc) Or (Command65.IsFillingCirc Or Command52.IsFillingCirc Or Command62.IsFillingCirc)
    IO.CTankAddition = (Parameters.BCTank0TwoPump1OnePump = 0 And CAllinOn) Or
                              (Parameters.BCTank0TwoPump1OnePump = 1 And CInjOn)
    'IO.BCTankToATank = NHalt And (Command58.IsTransfer Or Command59.IsTransfer)

    'B TANK outputs +++++++++++++++++++++++++++++++
    BDosingOn = (NHalt And (Command56.IsDosing Or Command66.IsDosing))
    BAllinOn = (NHalt And (Command51.IsTransfer Or Command56.IsTransfer Or Command61.IsTransfer Or Command66.IsTransfer Or Command24.IsAdd))
    BCirMixOn = NHalt And (Command54.IsMixingForTime Or TankBMix.IsMixingForTime Or Command56.IsCirculating Or Command66.IsCirculating)
    BInjOn = BDosingOn Or BAllinOn Or (BCirMixOn And Parameters.BTankMixType = 0)
    BPumpOn = (NHalt And (((Command54.IsMixingForTime Or TankBMix.IsMixingForTime) And Parameters.BTankMixType = 0) Or
              Command51.IsTransfer Or Command51.IsMixing Or Command56.IsTransferPump Or Command58.IsTransferPump Or
              Command61.IsTransfer Or Command61.IsMixing Or Command66.IsTransferPump Or B_ManualMixing Or Command24.IsAdd))

    'C TANK outputs +++++++++++++++++++++++++++++++
    CDosingOn = (NHalt And Command57.IsDosing Or Command67.IsDosing)
    CAllinOn = (NHalt And (Command52.IsTransfer Or Command57.IsTransfer Or Command62.IsTransfer Or Command67.IsTransfer Or Command52.IsTransfer Or Command62.IsTransfer))
    CCirMixOn = NHalt And (Command55.IsMixingForTime Or TankCMix.IsMixingForTime)
    CInjOn = CDosingOn Or CAllinOn Or (CCirMixOn And Parameters.CTankMixType = 0)
    CPumpOn = (NHalt And (((Command55.IsMixingForTime Or TankCMix.IsMixingForTime) And
                        Parameters.CTankMixType = 0) Or
                       Command52.IsTransfer Or
                       Command52.IsMixing Or
                       Command57.IsTransferPump Or
                       Command62.IsTransfer Or
                       Command62.IsMixing Or
                      Command67.IsTransferPump))


    Static delay4 As New DelayTimer
    IO.AlarmLamp = delay4.Run(Alarms.MainPumpError Or IO.Entanglement, 5) Or Command05.IsCalling Or Command20.IsCalling Or Command31.IsCalling Or
               Command33.IsCalling Or CallMessage Or Command54.IsSlow Or Command55.IsSlow Or Command64.IsSlow Or Command65.IsSlow Or
               Command34.IsCalling Or Command35.IsCalling Or Command36.IsCalling
    ' IO.SampleLamp = Command05.IsCalling Or Command20.IsCalling Or Command31.IsCalling Or Command33.IsCalling Or _
    '               Alarms.STankNotReady Or CallMessage Or Command34.IsCalling Or Command35.IsCalling Or Command36.IsCalling
    IO.SampleLamp = Command20.IsCalling

    'IO.C6AddLamp = NHalt
    ' IO.SoftFill = NHalt
    'analog outputs +++++++++++++++++++++++++++++++
    'Analog Heat/Cool
    If SetHotCoolingTemperature > 0 And IO.MainTemperature > SetHotCoolingTemperature * 10 And Not ColdWaterCooling And CoolNow Then
      IO.TemperatureControl = 0
    Else
      If NHalt And (TemperatureControl.IsCooling And Command01.Gradient = 0 And IO.MainPumpFB And TempControlFlag) Then
        IO.TemperatureControl = 1000
      ElseIf NHalt And (TemperatureControl.IsHeating Or TemperatureControl.IsCooling) And IO.MainPumpFB And TempControlFlag Then
        IO.TemperatureControl = CType(TemperatureControl.Output, Short)
      Else
        IO.TemperatureControl = 0
      End If
    End If

    'pump speed
    If PumpOn Or PumpStartRequest Then
      IO.PumpSpeedControl = CType(PumpSpeed * 10, Short)
    Else
      IO.PumpSpeedControl = 0
    End If
    'roller speed
    '帶布輪啟動跟停止
    Static ReelStartDelay As New DelayTimer
    ReelOn = ReelStartDelay.Run(IO.MainPumpFB, Parameters.ReelStartDelayTime) And Not IO.Entanglement
    If ReelOn Then
      IO.Reel1SpeedControl = CType(Roller1Speed * 10, Short)
      IO.Reel2SpeedControl = CType(Roller2Speed * 10, Short)
      IO.Reel3SpeedControl = CType(Roller3Speed * 10, Short)
      IO.Reel4SpeedControl = CType(Roller4Speed * 10, Short)
    Else
      IO.Reel1SpeedControl = 0
      IO.Reel2SpeedControl = 0
      IO.Reel3SpeedControl = 0
      IO.Reel4SpeedControl = 0
    End If

    If ReelOn And Not ReelOnWas Then
      ReelOnRequest_Timer.TimeRemaining = 2
      ReelOnWas = True
      ReelOffWas = False
    ElseIf Not ReelOn And Not ReelOffWas Then
      ReelOffRequest_Timer.TimeRemaining = 2
      ReelOnWas = False
      ReelOffWas = True
    End If
    ReelOnRequest = ReelOn And Not ReelOnRequest_Timer.Finished
    ReelOffRequest = (Not ReelOn And Not ReelOffRequest_Timer.Finished) Or IO.PumpOff

    '纏車時停止主泵
    '   If IO.Entanglement And Not PumpOffWas Then
    ' PumpOffRequest_Timer.TimeRemaining = 2
    ' PumpOffWas = True
    ' ElseIf IO.MainPumpFB Then
    ' PumpOffWas = False
    ' End If
    ' PumpOffRequest = IO.Entanglement And Not PumpOffRequest_Timer.Finished

    '沒有纏車時啟動主泵
    ' If IO.Entanglement Then
    ' EntanglementWas = True
    ' ElseIf IO.MainPumpFB Then
    ' EntanglementWas = False
    ' End If
    ' If Not IO.Entanglement And EntanglementWas And Not PumpOnWas Then
    ' PumpOnRequest_Timer.TimeRemaining = 2
    ' PumpOnWas = True
    ' ElseIf Not IO.MainPumpFB Then
    ' PumpOnWas = False
    ' End If
    ' PumpOnRequest = EntanglementWas And Not PumpOnRequest_Timer.Finished


    'b dosing pump output
    'if 0 inverter on dosing pump run at full speed. if 1 just an on off pump
    If Parameters.DosingKind0Pump1AO2DO = 0 Or Parameters.DosingKind0Pump1AO2DO = 1 Then
      If NHalt And (Command51.IsTransfer Or Command61.IsTransfer) And Parameters.DiluteAddControlType = 1 Then
        IO.BDosingOutput = 1000
      ElseIf NHalt And Command56.IsOn Then
        IO.BDosingOutput = CShort(MinMax(Command56.DoseOutput, 0, 1000))
      ElseIf NHalt And Command66.IsOn Then
        IO.BDosingOutput = CShort(MinMax(Command66.DoseOutput, 0, 1000))
      Else
        IO.BDosingOutput = 0
      End If
    Else
      IO.BDosingOutput = 0
    End If
    'c dosing pump output
    'if 0 inverter on dosing pump run at full speed. if 1 just an on off pump
    If Parameters.DosingKind0Pump1AO2DO = 0 Or Parameters.DosingKind0Pump1AO2DO = 1 Then
      If NHalt And (Command52.IsTransfer Or Command62.IsTransfer) And Parameters.DiluteAddControlType = 1 Then
        IO.CDosingOutput = 1000
      ElseIf NHalt And Command57.IsOn Then
        IO.CDosingOutput = CShort(MinMax(Command57.DoseOutput, 0, 1000))
      ElseIf NHalt And Command67.IsOn Then
        IO.CDosingOutput = CShort(MinMax(Command67.DoseOutput, 0, 1000))
      Else
        IO.CDosingOutput = 0
      End If
    Else
      IO.CDosingOutput = 0
    End If
    '***********************PH功能*************************************************************

    '-----------------------------------------------------
    '如果在十秒內,重複叫PhControl ,將縮短初期PH檢測時間
    '但是條件是第一次PhControl最少要執行過60秒以上,才可以下次重新執行PhControl時,縮短初期檢測時間


    If PhControl.IsOn = True And PhCirculateRun.IsOn = True And PH檢測_短時間內不在檢測 = False Then
      PH再檢測 = My.Computer.Clock.TickCount
      PH檢測_短時間內不在檢測 = True
    End If

    If (My.Computer.Clock.TickCount - PH再檢測) > 60000 And PH檢測_短時間內不在檢測 = True Then
      已經確保120秒檢測完成 = True
      是否縮短檢測時間 = False
      PH再檢測時間.TimeRemaining = 0
      短時間內重新執行 = False
    End If


    If PhControl.IsOn = False And 已經確保120秒檢測完成 = True And 短時間內重新執行 = False Then
      短時間內重新執行 = True
      PH再檢測時間.TimeRemaining = 10
      是否縮短檢測時間 = True
      PH檢測_短時間內不在檢測 = False
    End If

    If 短時間內重新執行 = True And PH再檢測時間.Finished Then
      是否縮短檢測時間 = False
    End If



    '================================================================
    '-----------------------計算使用PH量---------------------------------------
    If Not IO.PhAddHacOut Then
      加酸時間.TimeRemaining = 0
    ElseIf IO.PhAddHacOut And 加酸時間.Finished Then
      加酸時間.TimeRemaining = 1
      加酸次數 = 加酸次數 + 1
    End If
    If Not PhControl.IsOn Then
      加酸次數 = 0
    End If
    '-----------------------顯示mimic---------------------------------------
    test1 = IO.PhValue / 100
    test2 = PhControl.C75Gradient / 100
    test3 = 加酸次數
    test4 = PhControl.Wait1.TimeRemaining
    test5 = PhControl.PhAddError
    test6 = PhControl.ExpectPh / 100
    test7 = PhControl.CountHacVolume
    test8 = PhCheck.Wait.TimeRemaining
    test9 = PhControl.RegisterI(2)
    test10 = CType(PhControl.TotalAddHac2, Integer)
    test11 = PhControl.CalculateTmepRange
    test12 = PhControl.Wait10Second.TimeRemaining
    test13 = PhControl.WaitAddHac.TimeRemaining
    test14 = PhControl.RegisterI(10)
    test15 = PhControl.ExpectPh2

    test19 = PhCheck.DelayWait.TimeRemaining
    test20 = PhCheck.S12 / 100
    test21 = PhConcentration
    test22 = PhControl.PhFillLevel
    test23 = PhPumpOutRatio
    test25 = PhControl.AddOverError
    test26 = PhControl.AddOverTimes
    test27 = PhCheckError.PhOverAddTimes
    test28 = PhCheckError.Wait.TimeRemaining
    test29 = PhCheckError.StopAddPH
    test30 = PhCheckError.StopAddPH2
    test31 = PhControl.MathNeverOpenValue

    test32 = PhControl.次數
    test33 = PhControl.微調次數
    test34 = pH加酸長時間無動作計時.TimeRemaining
    補酸狀態分析 = PhControl.減酸比率
    test35 = PhControl.W0微量補酸
    test36 = 主程式完成pH未完成計時.TimeRemaining

    If P_PhShowData = 0 Then
      PhShowData = "PhShowClose"
    Else
      PhShowData = "PhShowOpen"
    End If



    If PhCheck.State = PhCheck_.PhCheck.Check1 Then
      test16 = "Check1"
    ElseIf PhCheck.State = PhCheck_.PhCheck.Check2 Then
      test16 = "Check2"
    ElseIf PhCheck.State = PhCheck_.PhCheck.DelayCheck1 Then
      test16 = "DelayCheck1"
    ElseIf PhCheck.State = PhCheck_.PhCheck.DelayCheck2 Then
      test16 = "DelayCheck2"
    ElseIf PhCheck.State = PhCheck_.PhCheck.Finish Then
      test16 = "Finish"
    ElseIf PhCheck.State = PhCheck_.PhCheck.off Then
      test16 = "off"
    End If

    If PhControl.State = PhControl_.PhControl.AddHacError Then
      test24 = "AddHacError"
    ElseIf PhControl.State = PhControl_.PhControl.Alarm_TmepHigh Then
      test24 = "Alarm_TmepHigh"
    ElseIf PhControl.State = PhControl_.PhControl.AlarmPhHigh Then
      test24 = "AlarmPhHigh"
    ElseIf PhControl.State = PhControl_.PhControl.AllAddHac Then
      test24 = "AllAddHac"
    ElseIf PhControl.State = PhControl_.PhControl.CheckPhValue Then
      test24 = "CheckPhValue"
    ElseIf PhControl.State = PhControl_.PhControl.Divider Then
      test24 = "Divider"
    ElseIf PhControl.State = PhControl_.PhControl.DownloadParameter Then
      test24 = "DownloadParameter"
    ElseIf PhControl.State = PhControl_.PhControl.Finished Then
      test24 = "Finished"
    ElseIf PhControl.State = PhControl_.PhControl.MathAddHac2 Then
      test24 = "MathAddHac2"
    ElseIf PhControl.State = PhControl_.PhControl.MathAddHac3 Then
      test24 = "MathAddHac3"
    ElseIf PhControl.State = PhControl_.PhControl.MathAddHac4 Then
      test24 = "MathAddHac4"
    ElseIf PhControl.State = PhControl_.PhControl.Off Then
      test24 = "Off"
    ElseIf PhControl.State = PhControl_.PhControl.Pause Then
      test24 = "Pause"
    ElseIf PhControl.State = PhControl_.PhControl.WaitKeepTime Then
      test24 = "WaitKeepTime"
    ElseIf PhControl.State = PhControl_.PhControl.WaitTempArrival Then
      test24 = "WaitTempArrival"
    ElseIf PhControl.State = PhControl_.PhControl.WaitTempFinish Then
      test24 = "WaitTempFinish"
    End If
    '-------------------------PH測試-----------------------------------------------
    If PhControlFlag = True Then

      PhControl.Run()                           'PH控制

      PhCirculateRun.Run()                      '跑迴流桶動作

      '-----------------------顯示PH加藥量-----------------------------------
      If IO.PhAddHacOut And MathHacFlag = False Then
        MathHacTimes.Stop()

        MathHacTimes.Start()
        MathHacFlag = True
      End If

      If MathHacFlag = True And IO.PhAddHacOut = False Then
        UseHacThisValue = MathHacTimes.TimeElapsed
        'MathHacTimes.Stop()
        'UseHacThisValue = MathHacTimes.TimeElapsed
        MathHacFlag = False
        UseHacAllTotal = UseHacAllTotal + UseHacThisValue

        If IO.PhValue < 450 Then
          UseHacAllTotal2 = UseHacAllTotal2 + UseHacThisValue
        End If

      End If
      '-----------------PH控制加酸時，實際低於PH450時，開始計算每次加酸量，當 實際加酸總量 > 目標加酸總量 
      If UseHacAllTotal2 > PhControl.TotalAddHac4 Then                        '20525---9005 1801 2020 404

        If (UseHacAllTotal2 - PhControl.TotalAddHac4) > 1000 Then

          Alarms.MessageAddHacError = True

        End If

      End If
    ElseIf PhControlFlag = False Then
      MathHacTimes.Stop()
    End If
    '------------------------------------------------------------------------------
    If PhControlFlag = False Then
      PhControl.Cancel()                      '結束PH控制
      If Command78.State = Command78.S78.KeepTime Then
        PhCirculateRun.Run()                 '結束迴流桶動作
      Else
        If PhCirRun And PhCirRuning = 1 And Parent.IsProgramRunning Then
          PhCirculateRun.Run()                 '結束迴流桶動作
        Else
          PhCirculateRun.Cancel()                 '結束迴流桶動作
        End If

      End If

      PhControl.Wait.TimeRemaining = 0
      PhControl.ExpectPh = 0
      UseHacAllTotal = 0 '
      UseHacAllTotal2 = 0
      Alarms.MessageAddHacError = False
      PhControl.Wait1.TimeRemaining = 0
      PhControl.CalculateTmepRange = 0
      PhControl.TotalAddHac2 = 0

    End If



    '-----------------------------------------------------------------------------
    If PhControl.CheckPhRun = True And PhControl.IsOn Then
      PhCheck.Run()
      PhCheckError.Run()
    End If
    '-----------------------------------------------------------------------------
    If PhControl.CheckPhRun = False Or Not PhControl.IsOn Then
      PhCheckError.Cancel()
      PhCheck.Cancel()
      PhCheck.DelayWait.TimeRemaining = 0
      PhCheck.Wait.TimeRemaining = 0
      PhCheck.Wait1.TimeRemaining = 0
      PhCheck.X = 0
      PhCheck.X2 = 0
      PhCheck.S12 = 0
      PhCheckError.PhOverAddTimes = 0
    End If
    '---------------------------------------------------主要是關閉加藥閥時,需要延遲關閉循環馬達,讓加酸打入染機
    If IO.PhAddHacOut = True Then
      If Not Command81.IsOn Then
        DelayAddTime.TimeRemaining = DelayCirculatPump       '加酸關閉,循環馬達延遲關閉時間
        DelayAddHac = True
      End If

    End If

    If IO.PhAddHacOut = False And DelayAddTime.Finished Then
      DelayAddHac = False
    End If
    '-----------------------------------------------------------------------------

    Alarms.HighTempNoAddHac = PhControl.IsOn And IO.MainPumpFB And IO.MainTemperature > (PH加酸安全溫度 * 10) And Not PhCheckError.StopAddPH And Not PhCheckError.StopAddPH2
    'PH加酸閥
    IO.PhAddHacOut = NHalt And (Alarms.MessageAddHacError = False And (PhControl.PhAddHacOut And IO.MainPumpFB And IO.MainTemperature < (PH加酸安全溫度 * 10)) _
                      And IO.PhCheckTemp <= PH檢測筒溫度過高 * 10) Or Command81.Istest     'PH加酸閥
    'PH入染機
    'IO.PhInToMachine = NHalt And ( _
    '                             (PhControl.PhInToMachine And IO.MainTemperature < (Parameters.PH加酸安全溫度 * 10)) Or _
    '                             (PhCirculateRun.PhInToMachine And IO.MainTemperature < (Parameters.PH加酸安全溫度 * 10)) Or _
    '                             DelayAddHac = True _
    '                             ) _
    '                          And Parameters.PhCirTank = 1 And IO.PhCheckTemp <= Parameters.PH檢測筒溫度過高 * 10 'PH入染機閥

    'PH入迴水
    IO.PhFillCirculate = (
                          (PhCirculateRun.PhFillCirculate2 Or PhCirculateRun.PhFillCirculate3) _
                          And IO.MainPumpFB And IO.PhCheckTemp <= PH檢測筒溫度過高 * 10
                          )  'PH迴水閥

    'PH定量泵
    ' IO.PhAddPump = NHalt And ((Alarms.MessageAddHacError = False And IO.MainPumpFB And (PhControl.PhAddPump And IO.MainTemperature < Parameters.PH加酸安全溫度 * 10)) _
    '                           And Not PhCheckError.StopAddPH And Not PhCheckError.StopAddPH2 And IO.PhCheckTemp <= Parameters.PH檢測筒溫度過高 * 10) Or Command81.Istest 'PH定量馬達

    'PH循環泵
    ' IO.PhCirculatePump = ((PhCirculateRun.PHTankAddPump) Or PhWash.PhCirculatePump Or DelayAddHac = True) _
    '                       And IO.MainPumpFB And IO.MainTemperature < (Parameters.PH加酸安全溫度 * 10) _
    '                       And Parameters.PhCirTank = 1 And IO.PhCheckTemp <= Parameters.PH檢測筒溫度過高 * 10

    'PH冷卻
    IO.PhCool = NHalt And ((PhCirculateRun.IsOn Or (Parent.IsProgramRunning And Not PhControlFlag)) And (IO.PhCheckTemp > PhCoolingTemp * 10))              'PH冷卻

    If (IO.ColdFill Or IO.HotFill Or IO.Drain Or IO.HotDrain) And Not PhAutoWashWas Then
      PhControl.Cancel() : PhWash.Cancel() : Command73.Cancel() : Command75.Cancel() : Command74.Cancel() : Command77.Cancel() : Command78.Cancel() : Command79.Cancel() : Command76.Cancel() : Command80.Cancel() : Command81.Cancel() : Ph76Time.Cancel()
      F76專用旗標 = False
      PhAutoWashTimer.TimeRemaining = PhAutoWashTime
      PhAutoWash = True
      PhAutoWashWas = True
    ElseIf IO.PhFillCirculate Then
      PhAutoWash = False
      PhAutoWashWas = False
    End If

    'PH入清水
    IO.PhFill = PhAutoWash And Not PhAutoWashTimer.Finished

    'PH排水
    IO.PhDrain = PhAutoWash And Not PhAutoWashTimer.Finished
    '=======================藥缸回流桶控制==========================================================

    PhToAdd = (PhCirculateRun.CtankToMachine And IO.MainTemperature < (PH加酸安全溫度 * 10)) And PhCirTank = 0
    PhToCPump = PhCirculateRun.CAddPump And IO.MainTemperature < (PH加酸安全溫度 * 10) And PhCirTank = 0
    PhToDrain = PhWash.PhNoCirTank And PhCirTank = 0
    '----------------------------------------------------------------------------
    '=======================加酸長時間無動作警報====================================================

    If PhControl.IsOn Then

      If Math.Abs(記錄pH之前變化值 - IO.PhValue) > 10 Then
        pH加酸長時間無動作計時.TimeRemaining = PhControlToLongTime
        pH加酸長時間無動作flag = False
        記錄pH之前變化值 = IO.PhValue
      End If

      If pH加酸長時間無動作計時.Finished Then
        pH加酸長時間無動作flag = True                       '連接PLC  開警報
      End If
    Else
      pH加酸長時間無動作計時.TimeRemaining = PhControlToLongTime
      pH加酸長時間無動作flag = False
      記錄pH之前變化值 = 0
    End If
    '--------------------------加酸過長警報，按確認鈕，重新記時
    If (pH加酸長時間無動作flag = True And IO.CallAck) Then
      pH加酸長時間無動作計時.TimeRemaining = PhControlToLongTime
      pH加酸長時間無動作flag = False
      記錄pH之前變化值 = 0
    End If

    If PhControl.State = PhControl_.PhControl.Pause Or PhControl.State = PhControl_.PhControl.Alarm_TmepHigh Or PhControl.State = PhControl_.PhControl.Off Or
    PhControl.State = PhControl_.PhControl.CheckPhValue Or PhControl.State = PhControl_.PhControl.Finished Or PhControl.State = PhControl_.PhControl.WaitTempFinish Or
    PhControl.State = PhControl_.PhControl.WaitTempArrival Then
      pH加酸長時間無動作計時.TimeRemaining = PhControlToLongTime
      pH加酸長時間無動作flag = False
      記錄pH之前變化值 = 0
    End If

    Alarms.LongtimeToHac = pH加酸長時間無動作flag = True
    '=======================主程式結束後，pH尚未完成將執行警報====================================================
    If Command01.IsOn And (Command74.IsOn Or Command75.IsOn) Then
      主程式完成pH未完成計時.TimeRemaining = 600
      主程式完成pH未完成flag = False

    ElseIf Not Command01.IsOn And (Command74.IsOn Or Command75.IsOn) Then
      If 主程式完成pH未完成計時.Finished Then
        主程式完成pH未完成flag = True                         '連接PLC  開警報
      End If

    ElseIf Not (Command74.IsOn Or Command75.IsOn) Then
      主程式完成pH未完成計時.TimeRemaining = 600
      主程式完成pH未完成flag = False

    End If

    If 主程式完成pH未完成flag = True And IO.CallAck Then
      主程式完成pH未完成計時.TimeRemaining = 600
      主程式完成pH未完成flag = False
    End If
    Alarms.pHstillNoFinishControl = 主程式完成pH未完成flag = True
    '=======================pH Sensor超過安全溫度====================================================
    If NHalt And PhControl.IsOn And IO.PhCheckTemp <= (PH檢測筒溫度過高 * 10) Then
      pHSensor超過安全溫度計時.TimeRemaining = 5
      pHSensor超過安全溫度flag = False
      pHSensor超過安全溫度警報flag = False
    ElseIf NHalt And PhControl.IsOn And IO.PhCheckTemp > (PH檢測筒溫度過高 * 10) Then
      PhControl.Cancel()
      PhCirculateRun.Cancel()
      If pHSensor超過安全溫度計時.Finished Then
        pHSensor超過安全溫度flag = True
        pHSensor超過安全溫度警報flag = True

      End If
    End If


    '----------------警報後，按完確認鈕後，依然會顯示Alarm,但是會消除警報
    If NHalt And pHSensor超過安全溫度flag = True And IO.CallAck Then
      pHSensor超過安全溫度計時.TimeRemaining = 125
      pHSensor超過安全溫度flag = True
      pHSensor超過安全溫度警報flag = False
      PhControl.Cancel()
      PhCirculateRun.Cancel()
    End If

    '-----------------顯示Alarm資訊
    Alarms.pHSensorHinhTemp = NHalt And (Command73.IsOn Or Command74.IsOn Or Command75.IsOn Or Command76.IsOn Or Command77.IsOn Or Command78.IsOn Or Command80.IsOn Or Ph76Time.IsOn) And pHSensor超過安全溫度flag = True

    '----------------不執行控制PH，將消除警報、消除Alarm顯示
    If Not PhControl.IsOn And Not Command73.IsOn And Not Command74.IsOn And Not Command75.IsOn And Not Command76.IsOn And Not Command77.IsOn And Not Command78.IsOn And Not Command80.IsOn And Not Ph76Time.IsOn Then
      pHSensor超過安全溫度計時.TimeRemaining = 5
      pHSensor超過安全溫度flag = False
      pHSensor超過安全溫度警報flag = False
    End If
    '******************************************************************************************



    '參數轉換至IO
    IO.SetSafetyTempToPlc = Parameters.SetSafetyTemp * 10
    IO.SetAddSafetyTempToPlc = Parameters.SetAddSafetyTemp * 10
    IO.AddFinishDelayToPlc = Parameters.AddTransferTimeBeforeRinse * 10
    IO.PowerDrainDelayToPlc = Parameters.PowerDrainDelay * 10
    IO.CondensationDelayTimeToPlc = Parameters.CondensationDelayTime * 10

    'DownLoad Parameter to LB60B
    FirstScanDone = True

    'BC缸呼叫平行攪拌功能
    TankBMix.Run()
    TankCMix.Run()

    '藥缸功能執行時，啟動溫度控制
    If (Command51.IsOn Or Command52.IsOn Or Command54.IsOn Or Command55.IsOn Or Command56.IsOn Or Command57.IsOn Or
        Command64.IsOn Or Command65.IsOn Or Command66.IsOn Or Command67.IsOn) And Not Command01.IsOn And TempControlFlag Then
      If IO.MainTemperature > TemperatureControl.TempFinalTemp Then
        CoolNow = True
        HeatNow = False
      Else
        CoolNow = False
        HeatNow = True
      End If
    End If

    '溫度控制時，檢查溫度是否有變化
    If Command01.IsOn And TempControlFlag And CheckMainTempTimer.Finished Then
      CheckMainTempTimer.TimeRemaining = 60
      If (HeatNow And (Setpoint - IO.MainTemperature) > 30 And Command01.Gradient > 0) Or
      (HeatNow And (Setpoint - IO.MainTemperature) > 30 And Command01.IsHolding) Then
        MainTempStopChange = True
      Else
        MainTempStopChange = False
      End If

      If (CoolNow And (IO.MainTemperature - Setpoint) > 30 And Command01.Gradient > 0) Or
      (CoolNow And (IO.MainTemperature - Setpoint) > 30 And Command01.IsHolding) Then
        MainTempStopChange = True
      Else
        MainTempStopChange = False
      End If

    ElseIf Not Command01.IsOn Then
      MainTempStopChange = False
    End If

    'For mimic
    Mimic_BTemperature = CType(IO.BTankTemperature / 10, Short)

    '計算布速
    If IO.MainPumpFB Then
      If IO.FabricCycleInput1 And Not FabricCycleInput1Was Then
        FabricCycleTime1 = Maximum(FabricCycleTimer1.TimeElapsed, 999)
        FabricCycleTimer1.Start()
        FabricCycleInput1Was = Not FabricCycleInput1Was
      ElseIf FabricCycleTimer1.TimeElapsed > 900 Then
        FabricCycleTime1 = 0
        FabricCycleTimer1.Start()
      End If
      FabricCycleInput1Was = IO.FabricCycleInput1
      If IO.FabricCycleInput2 And Not FabricCycleInput2Was Then
        FabricCycleTime2 = Maximum(FabricCycleTimer2.TimeElapsed, 999)
        FabricCycleTimer2.Start()
        FabricCycleInput2Was = Not FabricCycleInput2Was
      ElseIf FabricCycleTimer2.TimeElapsed > 900 Then
        FabricCycleTime2 = 0
        FabricCycleTimer2.Start()
      End If
      FabricCycleInput2Was = IO.FabricCycleInput2
      If IO.FabricCycleInput3 And Not FabricCycleInput3Was Then
        FabricCycleTime3 = Maximum(FabricCycleTimer3.TimeElapsed, 999)
        FabricCycleTimer3.Start()
        FabricCycleInput3Was = Not FabricCycleInput3Was
      ElseIf FabricCycleTimer3.TimeElapsed > 900 Then
        FabricCycleTime3 = 0
        FabricCycleTimer3.Start()
      End If
      FabricCycleInput3Was = IO.FabricCycleInput3
      If IO.FabricCycleInput4 And Not FabricCycleInput4Was Then
        FabricCycleTime4 = Maximum(FabricCycleTimer4.TimeElapsed, 999)
        FabricCycleTimer4.Start()
        FabricCycleInput4Was = Not FabricCycleInput4Was
      ElseIf FabricCycleTimer4.TimeElapsed > 900 Then
        FabricCycleTime4 = 0
        FabricCycleTimer4.Start()
      End If
      FabricCycleInput4Was = IO.FabricCycleInput4
      If IO.FabricCycleInput5 And Not FabricCycleInput5Was Then
        FabricCycleTime5 = Maximum(FabricCycleTimer5.TimeElapsed, 999)
        FabricCycleTimer5.Start()
        FabricCycleInput5Was = Not FabricCycleInput5Was
      ElseIf FabricCycleTimer5.TimeElapsed > 900 Then
        FabricCycleTime5 = 0
        FabricCycleTimer5.Start()
      End If
      FabricCycleInput5Was = IO.FabricCycleInput5
      If IO.FabricCycleInput6 And Not FabricCycleInput6Was Then
        FabricCycleTime6 = Maximum(FabricCycleTimer6.TimeElapsed, 999)
        FabricCycleTimer6.Start()
        FabricCycleInput6Was = Not FabricCycleInput6Was
      ElseIf FabricCycleTimer6.TimeElapsed > 900 Then
        FabricCycleTime6 = 0
        FabricCycleTimer6.Start()
      End If
      FabricCycleInput6Was = IO.FabricCycleInput6
      If IO.FabricCycleInput7 And Not FabricCycleInput7Was Then
        FabricCycleTime7 = Maximum(FabricCycleTimer7.TimeElapsed, 999)
        FabricCycleTimer7.Start()
        FabricCycleInput7Was = Not FabricCycleInput7Was
      ElseIf FabricCycleTimer7.TimeElapsed > 900 Then
        FabricCycleTime7 = 0
        FabricCycleTimer7.Start()
      End If
      FabricCycleInput7Was = IO.FabricCycleInput7
      If IO.FabricCycleInput8 And Not FabricCycleInput8Was Then
        FabricCycleTime8 = Maximum(FabricCycleTimer8.TimeElapsed, 999)
        FabricCycleTimer8.Start()
        FabricCycleInput8Was = Not FabricCycleInput8Was
      ElseIf FabricCycleTimer8.TimeElapsed > 900 Then
        FabricCycleTime8 = 0
        FabricCycleTimer8.Start()
      End If
      FabricCycleInput8Was = IO.FabricCycleInput8
    Else
      FabricCycleTime1 = 0
      FabricCycleTime2 = 0
      FabricCycleTime3 = 0
      FabricCycleTime4 = 0
    End If
    FabricCycleTimeCount1 = FabricCycleTimer1.TimeElapsed
    FabricCycleTimeCount2 = FabricCycleTimer2.TimeElapsed
    FabricCycleTimeCount3 = FabricCycleTimer3.TimeElapsed
    FabricCycleTimeCount4 = FabricCycleTimer4.TimeElapsed

    'B缸C缸高中低水位的類比值傳送到PLC，用來做比較
    IO.BTankHighAIToPlc = Parameters.BTankHighLevelAI
    IO.BTankMiddleAIToPlc = Parameters.BTankMiddleLevelAI
    IO.BTankLowAIToPlc = Parameters.BTankLowLevelAI
    IO.CTankHighAIToPlc = Parameters.CTankHighLevelAI
    IO.CTankMiddleAIToPlc = Parameters.CTankMiddleLevelAI
    IO.CTankLowAIToPlc = Parameters.CTankLowLevelAI

    If Not Command24.IsOn Then
      TransferMainTankCalibration()
    End If

    '噴嘴間隙大小按鈕動作
    If IO.NozzleGapPB1 Then
      IO.NozzleGapLamp1 = True
      IO.NozzleGapLamp2 = False
      IO.NozzleGapLamp3 = False
      IO.NozzleGapLamp4 = False
      NozzleGap = "1"
    ElseIf IO.NozzleGapPB2 Then
      IO.NozzleGapLamp1 = False
      IO.NozzleGapLamp2 = True
      IO.NozzleGapLamp3 = False
      IO.NozzleGapLamp4 = False
      NozzleGap = "2"
    ElseIf IO.NozzleGapPB3 Then
      IO.NozzleGapLamp1 = False
      IO.NozzleGapLamp2 = False
      IO.NozzleGapLamp3 = True
      IO.NozzleGapLamp4 = False
      NozzleGap = "3"
    ElseIf IO.NozzleGapPB4 Then
      IO.NozzleGapLamp1 = False
      IO.NozzleGapLamp2 = False
      IO.NozzleGapLamp3 = False
      IO.NozzleGapLamp4 = True
      NozzleGap = "4"
    End If
    If IO.NozzleSizePB1 Then
      IO.NozzleSizeLamp1 = True
      IO.NozzleSizeLamp2 = False
      IO.NozzleSizeLamp3 = False
      NozzleSize = "1"
    ElseIf IO.NozzleSizePB2 Then
      IO.NozzleSizeLamp1 = False
      IO.NozzleSizeLamp2 = True
      IO.NozzleSizeLamp3 = False
      NozzleSize = "2"
    ElseIf IO.NozzleSizePB3 Then
      IO.NozzleSizeLamp1 = False
      IO.NozzleSizeLamp2 = False
      IO.NozzleSizeLamp3 = True
      NozzleSize = "3"
    End If

    CallLA252.Run()
    CallLA302.Run()
    Dim dyelot = Parent.Job
    If Not dyelot Is Nothing Then
      Dim f = Parent.Job.IndexOf("@")
      If f <> -1 Then
        工單 = dyelot.Substring(0, f)
        Integer.TryParse(dyelot.Substring(f + 1), 重染)
      Else
        工單 = dyelot
        重染 = 0
      End If

      If (工單.Length > 2 And SPCConnectTimer.Finished And Not SPCConnectError And Parameters.ConnectSPCEnable = 1) Or Parameters.ConnectSPCTest = 1 Then
        ConnectBDC()
        DispenseState()
        SPCConnectTimer.TimeRemaining = 5
      Else

      End If

      If (工單.Length < 3 Or 工單 = System.Environment.MachineName) And Parameters.EnableLoadProgramOnLocal = 0 And Parent.IsProgramRunning Then
        StopProgram = True
        NotAllowLoadProgramAlarmTimer.TimeRemaining = 5
      Else
        StopProgram = False
      End If


    End If


    If RunCallLA302 Then
      Dim j As Integer
      For j = 0 To 15
        If ProductType(j) = "3" Then
          If StepNumber1(j) = Command45.CallOff.ToString Then
            CallFor302D = True
          End If
        End If
      Next
    Else
      CallFor302D = False
    End If


    If Not NotAllowLoadProgramAlarmTimer.Finished Then
      Alarms.NotAllowLoadProgramFromLocal = True
    Else
      Alarms.NotAllowLoadProgramFromLocal = False
    End If

    If Not Parent.IsProgramRunning And ProgramStopCleanDatabase And Parameters.ConnectSPCEnable = 1 Then
      DispenseState()
    End If

  End Sub



  Public Function ReadInputs(ByVal dinp() As Boolean, ByVal aninp() As Short, ByVal temp() As Short) As Boolean Implements ACControlCode.ReadInputs
    Return IO.ReadInputs(dinp, aninp, temp)
  End Function

  Public Sub WriteOutputs(ByVal dout() As Boolean, ByVal anout() As Short) Implements ACControlCode.WriteOutputs
    IO.WriteOutputs(dout, anout)
  End Sub

  <ScreenButton("主缸", 1, ButtonImage.Vessel), ScreenButton("SideTank", 2, ButtonImage.SideVessel), ScreenButton("Recipe", 3, ButtonImage.Thermometer), ScreenButton("Calibration", 4, ButtonImage.Information)>
  Public Sub DrawScreen(ByVal screen As Integer, ByVal row() As String) Implements ACControlCode.DrawScreen
    Dim maximumRows As Integer = 24

    Select Case screen
      Case 1
        'Screen 1
        If FlashFlag Then
          row(1) = " "
        ElseIf Parent.IsProgramRunning Then
          row(1) = If(Language = LanguageValue.ZhTw, "程式執行中", "Program is running")
        ElseIf Parent.IsPaused Then
          row(1) = If(Language = LanguageValue.ZhTw, "程式暫停", "Program is paused")
        Else
          row(1) = If(Language = LanguageValue.ZhTw, "待機", "Idle")
        End If
        row(2) = If(Language = LanguageValue.ZhTw, "現在時間", "Current Time") & ":" & CurrentTime
        row(3) = If(Language = LanguageValue.ZhTw, "運行時間", "Run Time") & ":" & 程式執行小時.ToString("00") & ":" & 程式執行分鐘.ToString("00")
        row(4) = If(Language = LanguageValue.ZhTw, "實際:", "Actual:") & IO.MainTemperature / 10 & "C/" & If(Language = LanguageValue.ZhTw, "設定:", "SetPoint:") & Setpoint / 10 & "C"
        row(5) = If(Language = LanguageValue.ZhTw, "目標:", "Target:") & TemperatureControl.TempFinalTemp / 10 & "C/" & If(Language = LanguageValue.ZhTw, "保溫:", "Hold:") & TimerString(Command01.Wait.TimeRemaining)
        row(6) = If(Language = LanguageValue.ZhTw, "溫控閥開度:", "TempControlOutput:") & IO.TemperatureControl / 10 & "%"
        row(7) = "-----------------------------------------------------"
        If IO.MainPumpFB Then
          row(8) = If(Language = LanguageValue.ZhTw, "主馬達:啟動", "MainPump:On") & "/" & If(Language = LanguageValue.ZhTw, "速度:", "Speed:") & IO.PumpSpeedControl * 1800 / 1000 & "rpm"
        Else
          row(8) = If(Language = LanguageValue.ZhTw, "主馬達:停止", "MainPump:Off")
        End If
        row(9) = If(Language = LanguageValue.ZhTw, "布輪:", "Reel:") & IO.Reel1SpeedControl * 800 / 1000 & "rpm /"
        row(10) = If(Language = LanguageValue.ZhTw, "布速1:", "Cycle1:") & FabricCycleTime1 & "/" & FabricCycleTime2 & "/" & FabricCycleTime3 & "/" & FabricCycleTime4 & "sec"
        row(11) = If(Language = LanguageValue.ZhTw, "布速5:", "Cycle5:") & FabricCycleTime5 & "/" & FabricCycleTime6 & "/" & FabricCycleTime7 & "/" & FabricCycleTime8 & "sec"
        row(12) = "噴嘴間隙: " & NozzleGap & "/噴嘴大小: " & NozzleSize
        If HighLevel Then
          row(13) = If(Language = LanguageValue.ZhTw, "主缸水位:高水位", "MainTankLevel  :High") & "/" & MainTankActualVolume & "L"
        ElseIf MiddleLevel Then
          row(13) = If(Language = LanguageValue.ZhTw, "主缸水位:中水位", "MainTankLevel  :Middle") & "/" & MainTankActualVolume & "L"
        ElseIf LowLevel Then
          row(13) = If(Language = LanguageValue.ZhTw, "主缸水位:低水位", "MainTankLevel  :Low") & "/" & MainTankActualVolume & "L"
        Else
          row(13) = If(Language = LanguageValue.ZhTw, "主缸水位:無", "MainTankLevel  :None") & "/" & MainTankActualVolume & "L"
        End If
        If PhCirculateRun.IsOn Then
          If PhControl.IsOn Then
            If PhControl.ExpectPh <= 200 Then
              row(14) = If(Language = LanguageValue.ZhTw, "pH 實際值 : ", "pH value") & (IO.PhValue / 100).ToString("0.00")
            Else
              Dim TestPH As Integer
              TestPH = CType((IO.PhValue + PhControl.ExpectPh) / 2, Integer)
              If IO.PhValue > PhControl.ExpectPh Then
                row(14) = If(Language = LanguageValue.ZhTw, "pH 實際值 : ", "pH value") & (IO.PhValue / 100).ToString("0.00")
              Else
                row(14) = If(Language = LanguageValue.ZhTw, "pH 實際值 : ", "pH value") & (TestPH / 100).ToString("0.00")
              End If
            End If
          Else
            row(14) = If(Language = LanguageValue.ZhTw, "pH 實際值 : ", "pH value") & (IO.PhValue / 100).ToString("0.00")
          End If
        Else
          row(14) = If(Language = LanguageValue.ZhTw, "pH 實際值 : ", "pH value") & (IO.PhValue / 100).ToString("0.00")
        End If
        If Command01.IsPaused Then
          row(17) = If(Language = LanguageValue.ZhTw, "        暫停", "        Pause")
        ElseIf TemperatureControl.IsHeating And Not Command01.IsHolding Then
          row(17) = If(Language = LanguageValue.ZhTw, "        升溫中", "        Heating")
        ElseIf TemperatureControl.IsCooling And Not Command01.IsHolding Then
          If ColdWaterCooling Then
            row(17) = If(Language = LanguageValue.ZhTw, "        冷水降溫中", "        ColdWaterCooling")
          Else
            row(17) = If(Language = LanguageValue.ZhTw, "        熱水降溫中", "        HotWaterCooling")
          End If
        ElseIf Command01.IsHolding Then
          row(17) = If(Language = LanguageValue.ZhTw, "        持溫中", "        Holding")
        ElseIf TemperatureControl.IsHeating And Setpoint - IO.MainTemperature > 20 Then
          row(17) = If(Language = LanguageValue.ZhTw, "        蒸氣不足", "        SteamNotEnough")
        ElseIf TemperatureControl.IsCooling And IO.MainTemperature - Setpoint > 20 Then
          row(17) = If(Language = LanguageValue.ZhTw, "        冷卻水不足", "        CoolingWaterNotEnough")
        ElseIf IO.HotFill And IO.ColdFill Then
          row(17) = If(Language = LanguageValue.ZhTw, "        進熱水+冷水", "        Fill Hot+Cold")
        ElseIf IO.ColdFill Then
          row(17) = If(Language = LanguageValue.ZhTw, "        進冷水", "        Fill Cold")
        ElseIf IO.HotFill Then
          row(17) = If(Language = LanguageValue.ZhTw, "        進熱水", "        FillHot")
        End If
        If MessageCallOperator Then
          row(18) = If(Language = LanguageValue.ZhTw, "        呼叫操作員", "        CallOperator")
        ElseIf MessageTakeSample Then
          row(18) = If(Language = LanguageValue.ZhTw, "        呼叫取樣", "        Sample")
        ElseIf MessageLoadFabric Then
          row(18) = If(Language = LanguageValue.ZhTw, "        入布", "        Load")
        ElseIf MessageUnloadFiber Then
          row(18) = If(Language = LanguageValue.ZhTw, "        出布", "        Unload")
        End If
        If Alarms.ManualOperation Then
          row(19) = If(Language = LanguageValue.ZhTw, "        系統手動中", "        Manual")
        ElseIf IO.SystemAuto Then
          row(19) = If(Language = LanguageValue.ZhTw, "        系統自動中", "        Auto")
        End If
        row(20) = If(Language = LanguageValue.ZhTw, "電量", "Power") & ": " & IO.瓦時計 & "Kwhr" & "/ " &
                           If(Language = LanguageValue.ZhTw, "蒸氣", "Steam") & ": " & SteamUsage / 100 & "m3"

      Case 2
        'Screen 2 - side tank

        row(1) = If(Language = LanguageValue.ZhTw, "B 缸訊息", "B Tank Message")
        If BTankHighLevel Then
          row(2) = If(Language = LanguageValue.ZhTw, "B缸水位:", "B Tank Level:") & IO.TankBLevel / 10 &
          If(Language = LanguageValue.ZhTw, "% ----->高水位", "% ----->High")
        ElseIf BTankMiddleLevel Then
          row(2) = If(Language = LanguageValue.ZhTw, "B缸水位:", "B Tank Level:") & IO.TankBLevel / 10 &
          If(Language = LanguageValue.ZhTw, "% ----->中水位", "% ----->Middle")
        ElseIf BTankLowLevel Then
          row(2) = If(Language = LanguageValue.ZhTw, "B缸水位:", "B Tank Level:") & IO.TankBLevel / 10 &
          If(Language = LanguageValue.ZhTw, "% ----->低水位", "% ----->Low")
        Else
          row(2) = If(Language = LanguageValue.ZhTw, "B缸水位:", "B Tank Level:") & IO.TankBLevel / 10 &
          If(Language = LanguageValue.ZhTw, "% ----->無", "% ----->None")
        End If
        '加藥目標水位
        If Command56.IsOn Then
          row(3) = If(Language = LanguageValue.ZhTw, "目標水位:", "Target Level:") & Command56.SetPoint / 10 & "%"
        ElseIf Command66.IsOn Then
          row(3) = If(Language = LanguageValue.ZhTw, "目標水位:", "Target Level:") & Command66.SetPoint / 10 & "%"
        Else
          row(3) = If(Language = LanguageValue.ZhTw, "目標水位:", "Target Level:")
        End If      '進水中
        If IO.BTankColdFill Then '備水顯示程式還沒完成
          row(4) = If(Language = LanguageValue.ZhTw, "B缸備水中", "B Tank Filling")
        ElseIf Command56.IsWaitingForPrepare Or Command66.IsWaitingForPrepare Then
          row(4) = If(Language = LanguageValue.ZhTw, "等待B備藥OK", "Wait B Tank Ready")
        ElseIf IO.BTankCirculate2 Then
          row(4) = If(Language = LanguageValue.ZhTw, "B缸備迴水中", "B Tank Circulating")
        ElseIf IO.BTankMixCir2 Then
          row(4) = If(Language = LanguageValue.ZhTw, "B缸攪拌中", "B Tank Mixing")
        End If
        '顯示攪拌時間
        If Command54.IsOn Then
          row(5) = Command54.StateString
        ElseIf TankBMixOn Then
          row(5) = TankBMix.StateString
          '備藥OK
          '加藥中，顯示時間
        ElseIf Command56.IsOn Then
          row(5) = Command56.StateString
        ElseIf Command51.IsOn Then
          row(5) = Command51.StateString
        ElseIf Command61.IsOn Then
          row(5) = Command61.StateString
        ElseIf Command66.IsOn Then
          row(5) = Command66.StateString
        End If

        row(7) = If(Language = LanguageValue.ZhTw, "C 缸訊息", "C Tank Message")
        If CTankHighLevel Then
          row(8) = If(Language = LanguageValue.ZhTw, "C缸水位:", "C Tank Level:") & IO.TankCLevel / 10 &
          If(Language = LanguageValue.ZhTw, "% ----->高水位", "% ----->High")
        ElseIf CTankMiddleLevel Then
          row(8) = If(Language = LanguageValue.ZhTw, "C缸水位:", "C Tank Level:") & IO.TankCLevel / 10 &
          If(Language = LanguageValue.ZhTw, "% ----->中水位", "% ----->Middle")
        ElseIf CTankLowLevel Then
          row(8) = If(Language = LanguageValue.ZhTw, "C缸水位:", "C Tank Level:") & IO.TankCLevel / 10 &
          If(Language = LanguageValue.ZhTw, "% ----->低水位", "% ----->Low")
        Else
          row(8) = If(Language = LanguageValue.ZhTw, "C缸水位:", "C Tank Level:") & IO.TankCLevel / 10 &
          If(Language = LanguageValue.ZhTw, "% ----->無", "% ----->None")
        End If
        '加藥目標水位
        If Command57.IsOn Then
          row(9) = If(Language = LanguageValue.ZhTw, "目標水位:", "Target Level:") & Command57.SetPoint / 10 & "%"
        ElseIf Command67.IsOn Then
          row(9) = If(Language = LanguageValue.ZhTw, "目標水位:", "Target Level:") & Command67.SetPoint / 10 & "%"
        Else
          row(9) = If(Language = LanguageValue.ZhTw, "目標水位:", "Target Level:")
        End If
        '進水中
        If IO.CTankColdFill Then '備水顯示程式還沒完成
          row(10) = If(Language = LanguageValue.ZhTw, "C缸備水中", "C Tank Filling")
        ElseIf Command57.IsWaitingForPrepare Or Command67.IsWaitingForPrepare Then
          row(10) = If(Language = LanguageValue.ZhTw, "等待C備藥OK", "Wait C Tank Ready")
        ElseIf IO.CTankCirculate2 Then
          row(10) = If(Language = LanguageValue.ZhTw, "C缸備迴水中", "C Tank Circulating")
        ElseIf IO.CTankMixCir2 Then
          row(10) = If(Language = LanguageValue.ZhTw, "C缸攪拌中", "C Tank Mixing")
        End If
        '顯示攪拌時間
        If Command55.IsOn Then
          row(11) = Command55.StateString
        ElseIf TankCMixOn Then
          row(11) = TankCMix.StateString
          '加藥中，顯示時間
        ElseIf Command57.IsOn Then
          row(11) = Command57.StateString
        ElseIf Command52.IsOn Then
          row(11) = Command52.StateString
        ElseIf Command62.IsOn Then
          row(11) = Command62.StateString
        End If

      Case 3  '配方訊息 

        row(1) = "領料單號: " & Parent.Job

        If DyeState = 202 Or DyeState = 201 Then
          row(2) = "步驟" & DyeCallOff & " " & "染料輸送中: " & TimerString(CallLA252.WaitTimer.TimeRemaining)
        Else
          row(2) = ""
        End If

        If DyeStepReady(1) Or
           DyeStepReady(2) Or
           DyeStepReady(3) Or
           DyeStepReady(4) Or
           DyeStepReady(5) Or
           DyeStepReady(6) Or
           DyeStepReady(7) Or
           DyeStepReady(8) Or
           DyeStepReady(9) Or
           DyeStepReady(10) Or
           DyeStepReady(11) Or
           DyeStepReady(12) Then
          row(3) = "步驟" & If(DyeStepReady(1), "1 ", "") &
          If(DyeStepReady(2), "2 ", "") &
          If(DyeStepReady(3), "3 ", "") &
          If(DyeStepReady(4), "4 ", "") &
          If(DyeStepReady(5), "5 ", "") &
          If(DyeStepReady(6), "6 ", "") &
          If(DyeStepReady(7), "7 ", "") &
          If(DyeStepReady(8), "8 ", "") &
          If(DyeStepReady(9), "9 ", "") &
          If(DyeStepReady(10), "10 ", "") &
          If(DyeStepReady(11), "11 ", "") &
          If(DyeStepReady(12), "12 ", "") &
          "染料計量完成"
        Else
          row(3) = ""
        End If

        '顯示LA-302F狀態

        If ChemicalState = 201 Then
          row(4) = "步驟" & ChemicalCallOff & " " & "助劑呼叫中"
        ElseIf ChemicalState = 202 Then
          row(4) = "步驟" & ChemicalCallOff & " " & "助劑輸送中: " & TimerString(CallLA302.WaitTimer.TimeRemaining)
        Else
          row(4) = ""
        End If

        If ChemicalStepReady(1) Or
           ChemicalStepReady(2) Or
           ChemicalStepReady(3) Or
           ChemicalStepReady(4) Or
           ChemicalStepReady(5) Or
           ChemicalStepReady(6) Or
           ChemicalStepReady(7) Or
           ChemicalStepReady(8) Or
           ChemicalStepReady(9) Or
           ChemicalStepReady(10) Or
           ChemicalStepReady(11) Or
           ChemicalStepReady(12) Then
          row(3) = "步驟" & If(ChemicalStepReady(1), "1 ", "") &
          If(ChemicalStepReady(2), "2 ", "") &
          If(ChemicalStepReady(3), "3 ", "") &
          If(ChemicalStepReady(4), "4 ", "") &
          If(ChemicalStepReady(5), "5 ", "") &
          If(ChemicalStepReady(6), "6 ", "") &
          If(ChemicalStepReady(7), "7 ", "") &
          If(ChemicalStepReady(8), "8 ", "") &
          If(ChemicalStepReady(9), "9 ", "") &
          If(ChemicalStepReady(10), "10 ", "") &
          If(ChemicalStepReady(11), "11 ", "") &
          If(ChemicalStepReady(12), "12 ", "") &
          "助劑計量完成"
        Else
          row(5) = ""
        End If


        For i As Integer = 0 To 15
          If ProductCode(i) IsNot Nothing Then
            If DispenseResult(i) = "301" Then
              row(i + 6) = StepNumber1(i) & ": " & ProductCode(i) & ": " & Grams(i) & "/" & DispenseGrams(i) & " 正常"
            ElseIf DispenseResult(i) = "302" Then
              row(i + 6) = StepNumber1(i) & ": " & ProductCode(i) & ": " & Grams(i) & "/" & DispenseGrams(i) & " 手動"
            ElseIf DispenseResult(i) = "309" Then
              row(i + 6) = StepNumber1(i) & ": " & ProductCode(i) & ": " & Grams(i) & "/" & DispenseGrams(i) & " 異常"
            Else
              row(i + 6) = StepNumber1(i) & ": " & ProductCode(i) & ": " & Grams(i) & "/" & DispenseGrams(i)
            End If
          Else
            row(i + 6) = ""
          End If

        Next

      Case 4
        'Screen 4 - Display Aninp Calibrate real Value

        If Language = LanguageValue.English Then
          row(1) = "AI1 Raw:" & IO.RealAI1
          row(2) = "Low In :" & IO.Parameters_AI1MinValue & "      High In  :" & IO.Parameters_AI1MaxValue
          row(3) = "Low Set :" & IO.Parameters_AI1SetMinValue & "     High Set  :" & IO.Parameters_AI1SetMaxValue
          row(4) = "AI2 Raw:" & IO.RealAI2
          row(5) = "Low In :" & IO.Parameters_AI2MinValue & "      High In  :" & IO.Parameters_AI2MaxValue
          row(6) = "Low Set :" & IO.Parameters_AI2SetMinValue & "     High Set  :" & IO.Parameters_AI2SetMaxValue
          row(7) = "AI3 Raw:" & IO.RealAI3
          row(8) = "Low In :" & IO.Parameters_AI3MinValue & "         High In :" & IO.Parameters_AI3MaxValue
          row(9) = "Low Set :" & IO.Parameters_AI3SetMinValue & "        High Set :" & IO.Parameters_AI3SetMaxValue
          row(10) = "AI4 Raw:" & IO.RealAI4
          row(11) = "Low In  :" & IO.Parameters_AI4MinValue & "      High In  :" & IO.Parameters_AI4MaxValue
          row(12) = "Low Set  :" & IO.Parameters_AI4SetMinValue & "     High Set  :" & IO.Parameters_AI4SetMaxValue
          row(13) = "AI5 Raw:" & IO.RealAI5
          row(14) = "Low In  :" & IO.Parameters_AI5MinValue & "      High In  :" & IO.Parameters_AI5MaxValue
          row(15) = "Low Set  :" & IO.Parameters_AI5SetMinValue & "     High Set  :" & IO.Parameters_AI5SetMaxValue
          row(16) = "AI6 Raw:" & IO.RealAI6
          row(17) = "Low In  :" & IO.Parameters_AI6MinValue & "      High In  :" & IO.Parameters_AI6MaxValue
          row(18) = "Low Set  :" & IO.Parameters_AI6SetMinValue & "     High Set  :" & IO.Parameters_AI6SetMaxValue
        ElseIf Language = LanguageValue.ZhTw Then
          row(1) = "實際第一組類比讀值:" & IO.RealAI1
          row(2) = "取低值 :" & IO.Parameters_AI1MinValue & "      取高值  :" & IO.Parameters_AI1MaxValue
          row(3) = "設低值 :" & IO.Parameters_AI1SetMinValue & "      設高值  :" & IO.Parameters_AI1SetMaxValue
          row(4) = "實際第二組類比讀值:" & IO.RealAI2
          row(5) = "取低值 :" & IO.Parameters_AI2MinValue & "      取高值  :" & IO.Parameters_AI2MaxValue
          row(6) = "設低值 :" & IO.Parameters_AI2SetMinValue & "      設高值  :" & IO.Parameters_AI2SetMaxValue
          row(7) = "實際第三組類比讀值:" & IO.RealAI3
          row(8) = "取低值 :" & IO.Parameters_AI3MinValue & "         取高值 :" & IO.Parameters_AI3MaxValue
          row(9) = "設低值 :" & IO.Parameters_AI3SetMinValue & "         設高值 :" & IO.Parameters_AI3SetMaxValue
          row(10) = "實際第四組類比讀值:" & IO.RealAI4
          row(11) = "取低值  :" & IO.Parameters_AI4MinValue & "      取高值  :" & IO.Parameters_AI4MaxValue
          row(12) = "設低值  :" & IO.Parameters_AI4SetMinValue & "      設高值  :" & IO.Parameters_AI4SetMaxValue
          row(13) = "實際第五組類比讀值:" & IO.RealAI5
          row(14) = "取低值  :" & IO.Parameters_AI5MinValue & "      取高值  :" & IO.Parameters_AI5MaxValue
          row(15) = "設低值  :" & IO.Parameters_AI5SetMinValue & "      設高值  :" & IO.Parameters_AI5SetMaxValue
          row(16) = "實際第六組類比讀值:" & IO.RealAI6
          row(17) = "取低值  :" & IO.Parameters_AI6MinValue & "      取高值  :" & IO.Parameters_AI6MaxValue
          row(18) = "設低值  :" & IO.Parameters_AI6SetMinValue & "      設高值  :" & IO.Parameters_AI6SetMaxValue
        End If


    End Select
  End Sub

  Public Sub ProgramStart() Implements ACControlCode.ProgramStart
    SteamUsage = 0
    'Reset dispense variables
    ChemicalCallOff = 0
    ChemicalState = 0
    ChemicalTank = 0
    ChemicalProducts = ""
    DyeCallOff = 0
    DyeState = 0
    DyeTank = 0
    DyeProducts = ""

    PressureOutTemp = Parameters.SetPressureOutTemp * 10
    PressureInTemp = Parameters.SetPressureOutTemp * 10

  End Sub

  Public Sub ProgramStop() Implements ACControlCode.ProgramStop
    SteamUsage = 0
    Command01.Cancel() : Command02.Cancel() : Command04.Cancel() : Command05.Cancel()
    Command11.Cancel() : Command12.Cancel() : Command13.Cancel() : Command14.Cancel()
    Command16.Cancel() : Command20.Cancel() : Command31.Cancel() : Command32.Cancel()
    Command33.Cancel() : Command51.Cancel() : Command52.Cancel() : Command54.Cancel()
    Command55.Cancel() : Command56.Cancel() : Command57.Cancel() : Command64.Cancel()
    Command65.Cancel() : TemperatureControl.Cancel()
    Command03.Cancel()
    Command61.Cancel() : Command62.Cancel() : Command66.Cancel() : Command67.Cancel()
    TankBMix.Cancel() : TankCMix.Cancel()
    Command18.Cancel() : Command19.Cancel()
    RecallLevel = 0
    TemperatureControlFlag = False
    HeatNow = False
    CoolNow = False
    PumpStartRequest = False
    PumpStopRequest = False
    PumpOn = False
    BTankHeatStartRequest = False
    CTankHeatStartRequest = False

    'Reset dispense variables
    ChemicalCallOff = 0
    ChemicalState = 0
    ChemicalTank = 0
    ChemicalProducts = ""
    DyeCallOff = 0
    DyeState = 0
    DyeTank = 0
    DyeProducts = ""

    RunCallLA252 = False
    RunCallLA302 = False
    CallLA252.Cancel()
    CallLA302.Cancel()
    LA252Ready = False
    LA302Ready = False
    Call252AddDye = False
    Wait252Scheduled = False
    CallFor302D = False
    WaitFor302D = False

    '清除計量狀態的陣列
    Array.Clear(DyeStepDispensing, 0, 12)
    Array.Clear(DyeStepReady, 0, 12)
    Array.Clear(ChemicalStepDispensing, 0, 12)
    Array.Clear(ChemicalStepReady, 0, 12)

    '清除配方的陣列
    Array.Clear(StepNumber1, 0, 30)
    Array.Clear(ProductCode, 0, 30)
    Array.Clear(ProductType, 0, 30)
    Array.Clear(Grams, 0, 30)
    Array.Clear(DispenseGrams, 0, 30)
    Array.Clear(DispenseResult, 0, 30)
    '清除染料的陣列
    Array.Clear(DyeStepNumber, 0, 10)
    Array.Clear(DyeCode, 0, 10)
    Array.Clear(DyeGrams, 0, 10)
    Array.Clear(DyeDispenseGrams, 0, 10)
    Array.Clear(DyeDispenseResult, 0, 10)
    '清除助劑的陣列
    Array.Clear(ChemicalStepNumber, 0, 20)
    Array.Clear(ChemicalCode, 0, 20)
    Array.Clear(ChemicalGrams, 0, 20)
    Array.Clear(ChemicalDispenseGrams, 0, 20)
    Array.Clear(ChemicalDispenseResult, 0, 20)

    '染程停止時清除資料庫資料
    ProgramStopCleanDatabase = True


  End Sub

  <GraphTrace(0, 1500, 5000, 9800, "Red", "%t℃"), Translate("zh", "主缸溫度"),
  GraphLabel("20℃", 200), GraphLabel("60℃", 600), GraphLabel("100℃", 1000), GraphLabel("130℃", 1300)>
  Public ReadOnly Property Temperature() As Integer
    Get
      Return (IO.MainTemperature)
    End Get
  End Property


  <GraphTrace(1, 1500, 5000, 9800, "Blue", "%t℃"), Translate("zh", "設定溫度")>
  Public ReadOnly Property SetTemperature() As Integer
    Get
      Return (Setpoint)
    End Get
  End Property

  <GraphTrace(0, 1200, 300, 2500, "Green", "%t%"), Translate("zh", "C缸水位"),
     GraphLabel("C Level0%", 0), GraphLabel("50%", 500), GraphLabel("100%", 1000)>
  Public ReadOnly Property CTankLevel() As Integer
    Get
      Return (IO.TankCLevel)
    End Get
  End Property

  <GraphTrace(1, 1200, 300, 2500, "Blue", "%t%"), Translate("zh", "C設定水位")>
  Public ReadOnly Property CSetLevel() As Integer
    Get
      If Command57.IsOn Then
        Return Command57.SetPoint
      ElseIf Command67.IsOn Then
        Return Command67.SetPoint
      Else
        Return 0
      End If
    End Get
  End Property

  <GraphTrace(0, 1200, 2600, 4800, "Green", "%t%"), Translate("zh", "B缸水位"),
    GraphLabel("B Level0%", 0), GraphLabel("50%", 500), GraphLabel("100%", 1000)>
  Public ReadOnly Property BTankLevel() As Integer
    Get
      Return (IO.TankBLevel)
    End Get
  End Property

  <GraphTrace(1, 1200, 2600, 4800, "Blue", "%t%"), Translate("zh", "B設定水位")>
  Public ReadOnly Property BSetLevel() As Integer
    Get
      If Command56.IsOn Then
        Return Command56.SetPoint
      ElseIf Command66.IsOn Then
        Return Command66.SetPoint
      Else
        Return 0
      End If
    End Get
  End Property

  <GraphTrace(0, 4000, 5000, 9800, "Green", "0L"), Translate("zh", "主缸水位")>
  Public ReadOnly Property MainTankLiter() As Integer
    Get
      Return MainTankVolume
    End Get
  End Property


  Public ReadOnly Property CurrentTime() As String
    Get
      Return Date.Now.ToLongTimeString()
    End Get
  End Property

  Public ReadOnly Property Status() As String
    Get
      If Parent.Signal <> "" And FlashFlag Then Return Parent.Signal
      If Command61.IsOn Then
        Return Command61.StateString
      ElseIf Command62.IsOn Then
        Return Command62.StateString
      ElseIf Command64.IsOn Then
        Return Command64.StateString
      ElseIf Command65.IsOn Then
        Return Command65.StateString
      ElseIf Command66.IsOn Then
        Return Command66.StateString
      ElseIf Command67.IsOn Then
        Return Command67.StateString
      ElseIf Command24.IsOn Then
        Return Command24.StateString
      ElseIf Command01.IsOn Then
        If Command61.IsOn And FlashFlag Then
            Return Command61.StateString
          ElseIf Command62.IsOn And FlashFlag Then
            Return Command62.StateString
          ElseIf Command64.IsOn And FlashFlag Then
            Return Command64.StateString
          ElseIf Command65.IsOn And FlashFlag Then
            Return Command65.StateString
          ElseIf Command66.IsOn And FlashFlag Then
            Return Command66.StateString
          ElseIf Command67.IsOn And FlashFlag Then
            Return Command67.StateString
          Else
            Return Command01.StateString
          End If
        ElseIf Command02.IsOn Then
          Return Command02.StateString
        ElseIf Command03.IsOn Then
          Return Command03.StateString
        ElseIf Command04.IsOn Then
          Return Command04.StateString
        ElseIf Command05.IsOn Then
          Return Command05.StateString
        ElseIf Command11.IsOn Then
          Return Command11.StateString
        ElseIf Command12.IsOn Then
          Return Command12.StateString
        ElseIf Command13.IsOn Then
          Return Command13.StateString
        ElseIf Command14.IsOn Then
          Return Command14.StateString
        ElseIf Command16.IsOn Then
          Return Command16.StateString
        ElseIf Command17.IsOn Then
          Return Command17.StateString
        ElseIf Command18.IsOn Then
          Return Command18.StateString
        ElseIf Command19.IsOn Then
          Return Command19.StateString
        ElseIf Command20.IsOn Then
          Return Command20.StateString
        ElseIf Command31.IsOn Then
          Return Command31.StateString
        ElseIf Command33.IsOn Then
          Return Command33.StateString
        ElseIf Command58.IsOn Then
          Return Command58.StateString
        ElseIf Command32.IsOn Then
          If Command64.IsOn And Command65.IsOn Then
            If Command64.IsOn And FlashFlag And Not FlashFlag2 Then
              Return Command64.StateString
            ElseIf Command65.IsOn And FlashFlag And FlashFlag2 Then
              Return Command65.StateString
            Else
              Return Command32.StateString
            End If
          End If
          If Command64.IsOn Or Command65.IsOn Then
            If Command64.IsOn And FlashFlag Then
              Return Command64.StateString
            ElseIf Command65.IsOn And FlashFlag Then
              Return Command65.StateString
            Else
              Return Command32.StateString
            End If
          End If
          Return Command32.StateString
        ElseIf Command51.IsOn Then
          Return Command51.StateString
        ElseIf Command52.IsOn Then
          Return Command52.StateString
        ElseIf Command54.IsOn Then
          If Command61.IsOn Or Command65.IsOn Or Command67.IsActive Then
            If Command61.IsOn And FlashFlag Then
              Return Command61.StateString
            ElseIf Command65.IsOn And FlashFlag Then
              Return Command65.StateString
            ElseIf Command67.IsActive And FlashFlag Then
              Return Command67.StateString
            Else
              Return Command54.StateString
            End If
          Else
            Return Command54.StateString
          End If

        ElseIf Command55.IsOn Then
          If Command64.IsOn Or Command61.IsActive Or Command67.IsActive Then
            If Command64.IsOn And FlashFlag Then
              Return Command64.StateString
            ElseIf Command61.IsOn And FlashFlag Then
              Return Command61.StateString
            ElseIf Command67.IsActive And FlashFlag Then
              Return Command67.StateString
            Else
              Return Command55.StateString
            End If
          Else
            Return Command55.StateString
          End If

        ElseIf Command56.IsOn Then
          If Command65.IsOn Then
            If Command65.IsOn And FlashFlag Then
              Return Command65.StateString
            Else
              Return Command56.StateString
            End If
          Else
            Return Command56.StateString
          End If

        ElseIf Command57.IsOn Then
          If Command64.IsOn Then
            If Command64.IsOn And FlashFlag Then
              Return Command64.StateString
            Else
              Return Command57.StateString
            End If
          Else
            Return Command57.StateString
          End If
        ElseIf Not Parent.IsProgramRunning Then
          Return If(Language = LanguageValue.ZhTw, "待機: ", "Idle: ") & TimerString(ProgramStoppedTimer.TimeElapsed)
        ElseIf Command41.IsOn Then
          Return Command41.StateString
        ElseIf Command42.IsOn Then
          Return Command42.StateString
        ElseIf Command43.IsOn Then
          Return Command43.StateString
        ElseIf Command44.IsOn Then
          Return Command44.StateString
        ElseIf Command45.IsOn Then
          Return Command45.StateString
        ElseIf Command46.IsOn Then
          Return Command46.StateString
      End If
      Return ""
    End Get
  End Property

  Private Sub TransferMainTankCalibration()
    SetMainTankAnalogInput(1) = Parameters.SetMainTankAnalogInput01
    SetMainTankAnalogInput(2) = Parameters.SetMainTankAnalogInput02
    SetMainTankAnalogInput(3) = Parameters.SetMainTankAnalogInput03
    SetMainTankAnalogInput(4) = Parameters.SetMainTankAnalogInput04
    SetMainTankAnalogInput(5) = Parameters.SetMainTankAnalogInput05
    SetMainTankAnalogInput(6) = Parameters.SetMainTankAnalogInput06
    SetMainTankAnalogInput(7) = Parameters.SetMainTankAnalogInput07
    SetMainTankAnalogInput(8) = Parameters.SetMainTankAnalogInput08
    SetMainTankAnalogInput(9) = Parameters.SetMainTankAnalogInput09
    SetMainTankAnalogInput(10) = Parameters.SetMainTankAnalogInput10
    SetMainTankAnalogInput(11) = Parameters.SetMainTankAnalogInput11
    SetMainTankAnalogInput(12) = Parameters.SetMainTankAnalogInput12
    SetMainTankAnalogInput(13) = Parameters.SetMainTankAnalogInput13
    SetMainTankAnalogInput(14) = Parameters.SetMainTankAnalogInput14
    SetMainTankAnalogInput(15) = Parameters.SetMainTankAnalogInput15
    SetMainTankAnalogInput(16) = Parameters.SetMainTankAnalogInput16
    SetMainTankAnalogInput(17) = Parameters.SetMainTankAnalogInput17
    SetMainTankAnalogInput(18) = Parameters.SetMainTankAnalogInput18
    SetMainTankAnalogInput(19) = Parameters.SetMainTankAnalogInput19
    SetMainTankAnalogInput(20) = Parameters.SetMainTankAnalogInput20
    SetMainTankAnalogInput(21) = Parameters.SetMainTankAnalogInput21
    SetMainTankAnalogInput(22) = Parameters.SetMainTankAnalogInput22
    SetMainTankAnalogInput(23) = Parameters.SetMainTankAnalogInput23
    SetMainTankAnalogInput(24) = Parameters.SetMainTankAnalogInput24
    SetMainTankAnalogInput(25) = Parameters.SetMainTankAnalogInput25
    SetMainTankAnalogInput(26) = Parameters.SetMainTankAnalogInput26
    SetMainTankAnalogInput(27) = Parameters.SetMainTankAnalogInput27
    SetMainTankAnalogInput(28) = Parameters.SetMainTankAnalogInput28
    SetMainTankAnalogInput(29) = Parameters.SetMainTankAnalogInput29
    SetMainTankAnalogInput(30) = Parameters.SetMainTankAnalogInput30
    SetMainTankAnalogInput(31) = Parameters.SetMainTankAnalogInput31
    SetMainTankAnalogInput(32) = Parameters.SetMainTankAnalogInput32
    SetMainTankAnalogInput(33) = Parameters.SetMainTankAnalogInput33
    SetMainTankAnalogInput(34) = Parameters.SetMainTankAnalogInput34
    SetMainTankAnalogInput(35) = Parameters.SetMainTankAnalogInput35
    SetMainTankAnalogInput(36) = Parameters.SetMainTankAnalogInput36
    SetMainTankAnalogInput(37) = Parameters.SetMainTankAnalogInput37
    SetMainTankAnalogInput(38) = Parameters.SetMainTankAnalogInput38
    SetMainTankAnalogInput(39) = Parameters.SetMainTankAnalogInput39
    SetMainTankAnalogInput(40) = Parameters.SetMainTankAnalogInput40
    SetMainTankAnalogInput(41) = Parameters.SetMainTankAnalogInput41
    SetMainTankAnalogInput(42) = Parameters.SetMainTankAnalogInput42
    SetMainTankAnalogInput(43) = Parameters.SetMainTankAnalogInput43
    SetMainTankAnalogInput(44) = Parameters.SetMainTankAnalogInput44
    SetMainTankAnalogInput(45) = Parameters.SetMainTankAnalogInput45
    SetMainTankAnalogInput(46) = Parameters.SetMainTankAnalogInput46
    SetMainTankAnalogInput(47) = Parameters.SetMainTankAnalogInput47
    SetMainTankAnalogInput(48) = Parameters.SetMainTankAnalogInput48
    SetMainTankAnalogInput(49) = Parameters.SetMainTankAnalogInput49
    SetMainTankAnalogInput(50) = Parameters.SetMainTankAnalogInput50

    SetMainTankVolume(1) = Parameters.SetMainTankVolume01
    SetMainTankVolume(2) = Parameters.SetMainTankVolume02
    SetMainTankVolume(3) = Parameters.SetMainTankVolume03
    SetMainTankVolume(4) = Parameters.SetMainTankVolume04
    SetMainTankVolume(5) = Parameters.SetMainTankVolume05
    SetMainTankVolume(6) = Parameters.SetMainTankVolume06
    SetMainTankVolume(7) = Parameters.SetMainTankVolume07
    SetMainTankVolume(8) = Parameters.SetMainTankVolume08
    SetMainTankVolume(9) = Parameters.SetMainTankVolume09
    SetMainTankVolume(10) = Parameters.SetMainTankVolume10
    SetMainTankVolume(11) = Parameters.SetMainTankVolume11
    SetMainTankVolume(12) = Parameters.SetMainTankVolume12
    SetMainTankVolume(13) = Parameters.SetMainTankVolume13
    SetMainTankVolume(14) = Parameters.SetMainTankVolume14
    SetMainTankVolume(15) = Parameters.SetMainTankVolume15
    SetMainTankVolume(16) = Parameters.SetMainTankVolume16
    SetMainTankVolume(17) = Parameters.SetMainTankVolume17
    SetMainTankVolume(18) = Parameters.SetMainTankVolume18
    SetMainTankVolume(19) = Parameters.SetMainTankVolume19
    SetMainTankVolume(20) = Parameters.SetMainTankVolume20
    SetMainTankVolume(21) = Parameters.SetMainTankVolume21
    SetMainTankVolume(22) = Parameters.SetMainTankVolume22
    SetMainTankVolume(23) = Parameters.SetMainTankVolume23
    SetMainTankVolume(24) = Parameters.SetMainTankVolume24
    SetMainTankVolume(25) = Parameters.SetMainTankVolume25
    SetMainTankVolume(26) = Parameters.SetMainTankVolume26
    SetMainTankVolume(27) = Parameters.SetMainTankVolume27
    SetMainTankVolume(28) = Parameters.SetMainTankVolume28
    SetMainTankVolume(29) = Parameters.SetMainTankVolume29
    SetMainTankVolume(30) = Parameters.SetMainTankVolume30
    SetMainTankVolume(31) = Parameters.SetMainTankVolume31
    SetMainTankVolume(32) = Parameters.SetMainTankVolume32
    SetMainTankVolume(33) = Parameters.SetMainTankVolume33
    SetMainTankVolume(34) = Parameters.SetMainTankVolume34
    SetMainTankVolume(35) = Parameters.SetMainTankVolume35
    SetMainTankVolume(36) = Parameters.SetMainTankVolume36
    SetMainTankVolume(37) = Parameters.SetMainTankVolume37
    SetMainTankVolume(38) = Parameters.SetMainTankVolume38
    SetMainTankVolume(39) = Parameters.SetMainTankVolume39
    SetMainTankVolume(40) = Parameters.SetMainTankVolume40
    SetMainTankVolume(41) = Parameters.SetMainTankVolume41
    SetMainTankVolume(42) = Parameters.SetMainTankVolume42
    SetMainTankVolume(43) = Parameters.SetMainTankVolume43
    SetMainTankVolume(44) = Parameters.SetMainTankVolume44
    SetMainTankVolume(45) = Parameters.SetMainTankVolume45
    SetMainTankVolume(46) = Parameters.SetMainTankVolume46
    SetMainTankVolume(47) = Parameters.SetMainTankVolume47
    SetMainTankVolume(48) = Parameters.SetMainTankVolume48
    SetMainTankVolume(49) = Parameters.SetMainTankVolume49
    SetMainTankVolume(50) = Parameters.SetMainTankVolume50

  End Sub

  Public Sub SaveMainTankCalibration()
    Parameters.SetMainTankAnalogInput01 = SetMainTankAnalogInput(1)
    Parameters.SetMainTankAnalogInput02 = SetMainTankAnalogInput(2)
    Parameters.SetMainTankAnalogInput03 = SetMainTankAnalogInput(3)
    Parameters.SetMainTankAnalogInput04 = SetMainTankAnalogInput(4)
    Parameters.SetMainTankAnalogInput05 = SetMainTankAnalogInput(5)
    Parameters.SetMainTankAnalogInput06 = SetMainTankAnalogInput(6)
    Parameters.SetMainTankAnalogInput07 = SetMainTankAnalogInput(7)
    Parameters.SetMainTankAnalogInput08 = SetMainTankAnalogInput(8)
    Parameters.SetMainTankAnalogInput09 = SetMainTankAnalogInput(9)
    Parameters.SetMainTankAnalogInput10 = SetMainTankAnalogInput(10)
    Parameters.SetMainTankAnalogInput11 = SetMainTankAnalogInput(11)
    Parameters.SetMainTankAnalogInput12 = SetMainTankAnalogInput(12)
    Parameters.SetMainTankAnalogInput13 = SetMainTankAnalogInput(13)
    Parameters.SetMainTankAnalogInput14 = SetMainTankAnalogInput(14)
    Parameters.SetMainTankAnalogInput15 = SetMainTankAnalogInput(15)
    Parameters.SetMainTankAnalogInput16 = SetMainTankAnalogInput(16)
    Parameters.SetMainTankAnalogInput17 = SetMainTankAnalogInput(17)
    Parameters.SetMainTankAnalogInput18 = SetMainTankAnalogInput(18)
    Parameters.SetMainTankAnalogInput19 = SetMainTankAnalogInput(19)
    Parameters.SetMainTankAnalogInput20 = SetMainTankAnalogInput(20)
    Parameters.SetMainTankAnalogInput21 = SetMainTankAnalogInput(21)
    Parameters.SetMainTankAnalogInput22 = SetMainTankAnalogInput(22)
    Parameters.SetMainTankAnalogInput23 = SetMainTankAnalogInput(23)
    Parameters.SetMainTankAnalogInput24 = SetMainTankAnalogInput(24)
    Parameters.SetMainTankAnalogInput25 = SetMainTankAnalogInput(25)
    Parameters.SetMainTankAnalogInput26 = SetMainTankAnalogInput(26)
    Parameters.SetMainTankAnalogInput27 = SetMainTankAnalogInput(27)
    Parameters.SetMainTankAnalogInput28 = SetMainTankAnalogInput(28)
    Parameters.SetMainTankAnalogInput29 = SetMainTankAnalogInput(29)
    Parameters.SetMainTankAnalogInput30 = SetMainTankAnalogInput(30)
    Parameters.SetMainTankAnalogInput31 = SetMainTankAnalogInput(31)
    Parameters.SetMainTankAnalogInput32 = SetMainTankAnalogInput(32)
    Parameters.SetMainTankAnalogInput33 = SetMainTankAnalogInput(33)
    Parameters.SetMainTankAnalogInput34 = SetMainTankAnalogInput(34)
    Parameters.SetMainTankAnalogInput35 = SetMainTankAnalogInput(35)
    Parameters.SetMainTankAnalogInput36 = SetMainTankAnalogInput(36)
    Parameters.SetMainTankAnalogInput37 = SetMainTankAnalogInput(37)
    Parameters.SetMainTankAnalogInput38 = SetMainTankAnalogInput(38)
    Parameters.SetMainTankAnalogInput39 = SetMainTankAnalogInput(39)
    Parameters.SetMainTankAnalogInput40 = SetMainTankAnalogInput(40)
    Parameters.SetMainTankAnalogInput41 = SetMainTankAnalogInput(41)
    Parameters.SetMainTankAnalogInput42 = SetMainTankAnalogInput(42)
    Parameters.SetMainTankAnalogInput43 = SetMainTankAnalogInput(43)
    Parameters.SetMainTankAnalogInput44 = SetMainTankAnalogInput(44)
    Parameters.SetMainTankAnalogInput45 = SetMainTankAnalogInput(45)
    Parameters.SetMainTankAnalogInput46 = SetMainTankAnalogInput(46)
    Parameters.SetMainTankAnalogInput47 = SetMainTankAnalogInput(47)
    Parameters.SetMainTankAnalogInput48 = SetMainTankAnalogInput(48)
    Parameters.SetMainTankAnalogInput49 = SetMainTankAnalogInput(49)
    Parameters.SetMainTankAnalogInput50 = SetMainTankAnalogInput(50)

    Parameters.SetMainTankVolume01 = SetMainTankVolume(1)
    Parameters.SetMainTankVolume02 = SetMainTankVolume(2)
    Parameters.SetMainTankVolume03 = SetMainTankVolume(3)
    Parameters.SetMainTankVolume04 = SetMainTankVolume(4)
    Parameters.SetMainTankVolume05 = SetMainTankVolume(5)
    Parameters.SetMainTankVolume06 = SetMainTankVolume(6)
    Parameters.SetMainTankVolume07 = SetMainTankVolume(7)
    Parameters.SetMainTankVolume08 = SetMainTankVolume(8)
    Parameters.SetMainTankVolume09 = SetMainTankVolume(9)
    Parameters.SetMainTankVolume10 = SetMainTankVolume(10)
    Parameters.SetMainTankVolume11 = SetMainTankVolume(11)
    Parameters.SetMainTankVolume12 = SetMainTankVolume(12)
    Parameters.SetMainTankVolume13 = SetMainTankVolume(13)
    Parameters.SetMainTankVolume14 = SetMainTankVolume(14)
    Parameters.SetMainTankVolume15 = SetMainTankVolume(15)
    Parameters.SetMainTankVolume16 = SetMainTankVolume(16)
    Parameters.SetMainTankVolume17 = SetMainTankVolume(17)
    Parameters.SetMainTankVolume18 = SetMainTankVolume(18)
    Parameters.SetMainTankVolume19 = SetMainTankVolume(19)
    Parameters.SetMainTankVolume20 = SetMainTankVolume(20)
    Parameters.SetMainTankVolume21 = SetMainTankVolume(21)
    Parameters.SetMainTankVolume22 = SetMainTankVolume(22)
    Parameters.SetMainTankVolume23 = SetMainTankVolume(23)
    Parameters.SetMainTankVolume24 = SetMainTankVolume(24)
    Parameters.SetMainTankVolume25 = SetMainTankVolume(25)
    Parameters.SetMainTankVolume26 = SetMainTankVolume(26)
    Parameters.SetMainTankVolume27 = SetMainTankVolume(27)
    Parameters.SetMainTankVolume28 = SetMainTankVolume(28)
    Parameters.SetMainTankVolume29 = SetMainTankVolume(29)
    Parameters.SetMainTankVolume30 = SetMainTankVolume(30)
    Parameters.SetMainTankVolume31 = SetMainTankVolume(31)
    Parameters.SetMainTankVolume32 = SetMainTankVolume(32)
    Parameters.SetMainTankVolume33 = SetMainTankVolume(33)
    Parameters.SetMainTankVolume34 = SetMainTankVolume(34)
    Parameters.SetMainTankVolume35 = SetMainTankVolume(35)
    Parameters.SetMainTankVolume36 = SetMainTankVolume(36)
    Parameters.SetMainTankVolume37 = SetMainTankVolume(37)
    Parameters.SetMainTankVolume38 = SetMainTankVolume(38)
    Parameters.SetMainTankVolume39 = SetMainTankVolume(39)
    Parameters.SetMainTankVolume40 = SetMainTankVolume(40)
    Parameters.SetMainTankVolume41 = SetMainTankVolume(41)
    Parameters.SetMainTankVolume42 = SetMainTankVolume(42)
    Parameters.SetMainTankVolume43 = SetMainTankVolume(43)
    Parameters.SetMainTankVolume44 = SetMainTankVolume(44)
    Parameters.SetMainTankVolume45 = SetMainTankVolume(45)
    Parameters.SetMainTankVolume46 = SetMainTankVolume(46)
    Parameters.SetMainTankVolume47 = SetMainTankVolume(47)
    Parameters.SetMainTankVolume48 = SetMainTankVolume(48)
    Parameters.SetMainTankVolume49 = SetMainTankVolume(49)
    Parameters.SetMainTankVolume50 = SetMainTankVolume(50)

  End Sub


  '  Private Sub GetRecipeData()
  'Get recipe data... data should be in the following format in the column RecipeProducts
  '  step,product1Code,product1Name,grams;step,product2Code,product2Name,grams;step,product3Code,product3Name,grams etc...
  '   Try
  'Get the current (running) dyelot from the local database (state=2) 
  'Dim dt As System.Data.DataTable = Parent.DbGetDataTable("SELECT * FROM Dyelots WHERE State=2")
  '    If dt IsNot Nothing AndAlso dt.Rows.Count = 1 Then
  '     Recipe.Load(dt.Rows(0))
  '   End If
  ' Catch ex As Exception
  '   Parent.LogException(ex)
  ' End Try
  'End Sub

  '  Private Function GetProductsFromString(ByVal productString As String) As String()
  '    Try
  '  Dim products() As String
  '      products = productString.Split("|".ToCharArray)
  '      Return products
  '
  '    Catch ex As Exception
  '      Parent.LogException(ex)
  '    End Try
  '    Return Nothing
  '  End Function
  Public Sub ConnectBDC()
    Try
      Parameters.ConnectSPCTest = 0
      SPCConnectError = True
      cs_Recipe = "data source=" & SPCServerName & ";initial catalog=BatchDyeingCentral;user id= " & SPCUserName & ";password=" & SPCPassword & ""
      qs_Recipe = "SELECT Dyelot, ReDye, StepNumber, ProductCode, ProductType, Grams, DispenseGrams, DispenseResult FROM DyelotsBulkedRecipe WHERE (Dyelot = '" & 工單 & "') AND (ReDye = '" & 重染 & "') AND (ProductCode <> '') ORDER BY StepNumber"

      '1.SqlConnection
      cn_Recipe = New SqlClient.SqlConnection(cs_Recipe)
      cn_Recipe.Open()
      '2.SqlCommand
      cmd_Recipe = New SqlClient.SqlCommand(qs_Recipe, cn_Recipe)
      '3.SqlDataAdapter
      da_Recipe = New SqlClient.SqlDataAdapter(cmd_Recipe)
      '4.SqlCommandBuilder
      cb_Recipe = New SqlClient.SqlCommandBuilder(da_Recipe)
      '5.建立DataSet類別或DataTable類別
      ds_Recipe = New DataSet()
      '6.使用Fill方法填入DataTable
      da_Recipe.Fill(ds_Recipe, "Recipe")
      dt_Recipe = ds_Recipe.Tables("Recipe")
      If dt_Recipe.Rows.Count > 0 Then
        Dim i As Integer
        Dim j As Integer = 0
        Dim k As Integer = 0
        For i = 0 To dt_Recipe.Rows.Count - 1
          StepNumber1(i) = dt_Recipe.Rows(i).Item("StepNumber").ToString
          ProductCode(i) = dt_Recipe.Rows(i).Item("ProductCode").ToString.Trim
          ProductType(i) = dt_Recipe.Rows(i).Item("ProductType").ToString
          Grams(i) = dt_Recipe.Rows(i).Item("Grams").ToString
          DispenseGrams(i) = dt_Recipe.Rows(i).Item("DispenseGrams").ToString
          DispenseResult(i) = dt_Recipe.Rows(i).Item("DispenseResult").ToString



          If ProductType(i) = "1" Then
            DyeStepNumber(j) = StepNumber1(i)
            DyeCode(j) = ProductCode(i).Trim
            DyeGrams(j) = Grams(i)
            DyeDispenseGrams(j) = DispenseGrams(i)
            DyeDispenseResult(j) = DispenseResult(i)
            j = j + 1
          Else
            ChemicalStepNumber(k) = StepNumber1(i)
            ChemicalCode(k) = ProductCode(i)
            ChemicalGrams(k) = Grams(i)
            ChemicalDispenseGrams(k) = DispenseGrams(i)
            ChemicalDispenseResult(k) = DispenseResult(i)
            k = k + 1
          End If
        Next

      End If
      SPCConnectError = False

      cn_Recipe.Close()

    Catch ex As Exception
      'Ignore errors
    End Try
  End Sub

  Public Sub DispenseState()
    Try
      'ComputerName = System.Environment.MachineName
      ComputerName = System.Environment.MachineName
      cs_DispenseState = "data source=" & SPCServerName & ";initial catalog=BatchDyeingCentral;user id= " & SPCUserName & ";password=" & SPCPassword & ""
      qs_DispenseState = "SELECT Name, DispenseDyelot, DispenseReDye, ChemicalCallOff, ChemicalState, ChemicalTank, ChemicalEnabled, DyeCallOff, DyeState, DyeTank, DyeEnabled FROM Machines WHERE (Name = '" & ComputerName & "') "

      '1.SqlConnection
      cn_DispenseState = New SqlClient.SqlConnection(cs_DispenseState)
      cn_DispenseState.Open()
      '2.SqlCommand
      cmd_DispenseState = New SqlClient.SqlCommand(qs_DispenseState, cn_DispenseState)
      '3.SqlDataAdapter
      da_DispenseState = New SqlClient.SqlDataAdapter(cmd_DispenseState)
      '4.SqlCommandBuilder
      cb_DispenseState = New SqlClient.SqlCommandBuilder(da_DispenseState)
      '5.建立DataSet類別或DataTable類別
      ds_DispenseState = New DataSet()
      '6.使用Fill方法填入DataTable
      da_DispenseState.Fill(ds_DispenseState, "DispenseState")
      dt_DispenseState = ds_DispenseState.Tables("DispenseState")
      CCallOff = dt_DispenseState.Rows(0).Item("ChemicalCallOff").ToString
      CTank = dt_DispenseState.Rows(0).Item("ChemicalTank").ToString
      CState = dt_DispenseState.Rows(0).Item("ChemicalState").ToString
      CEnabled = dt_DispenseState.Rows(0).Item("ChemicalEnabled").ToString
      DCallOff = dt_DispenseState.Rows(0).Item("DyeCallOff").ToString
      DTank = dt_DispenseState.Rows(0).Item("DyeTank").ToString
      DState = dt_DispenseState.Rows(0).Item("DyeState").ToString
      DEnabled = dt_DispenseState.Rows(0).Item("DyeEnabled").ToString

      '助劑呼叫的規則

      If ProgramStopCleanDatabase Then
        cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET DispenseDyelot=null, DispenseReDye=0 " &
                                             ", ChemicalCallOff = 0, ChemicalTank=0, ChemicalState='101', DyeCallOff =0, DyeTank=0, DyeState='101'" &
                                             " WHERE (Name = '" & ComputerName & "')", cn_DispenseState)
        cmd_DispenseState.ExecuteNonQuery()
        ProgramStopCleanDatabase = False
      Else
        If ChemicalCallOff = 0 And ChemicalTank = 0 And Not CallFor302D And ((CState = "") Or (CState = "101") Or (CState = "301") Or (CState = "302") Or (CState = "309") Or (CState = "202") Or (CState = "201") Or ChemicalState = 101) Then
          ChemicalState = 101
          cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET DispenseDyelot='" & 工單 & "', DispenseReDye='" & 重染 &
                                             "', ChemicalCallOff = " & ChemicalCallOff & ",CheckPowderRb = 0, ChemicalTank=" & ChemicalTank & ", ChemicalState=" & ChemicalState &
                                             " WHERE (Name = '" & ComputerName & "')", cn_DispenseState)
          cmd_DispenseState.ExecuteNonQuery()

        ElseIf ChemicalCallOff = 0 And ChemicalTank = 0 And CState = "101" And CEnabled = "1" And CallFor302D Then
          If CallFor302D And Not UpdatePowderDispenseResult Then
            cmd_DispenseState = New SqlClient.SqlCommand("UPDATE DyelotsBulkedRecipe SET DispenseResult=Null WHERE Dyelot='" & 工單 & "'AND ReDye='" & 重染 &
                                             "'AND StepNumber = " & Command45.CallOff.ToString & " AND ProductType= 3", cn_DispenseState)

            cmd_DispenseState.ExecuteNonQuery()
            UpdatePowderDispenseResult = True
          End If
        ElseIf ChemicalCallOff <> 0 And ChemicalTank <> 0 And CState = "101" And CEnabled = "1" Then
          ChemicalState = 201
          cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET DispenseDyelot='" & 工單 & "', DispenseReDye='" & 重染 &
                                             "', ChemicalCallOff = " & ChemicalCallOff & ", ChemicalTank=" & ChemicalTank & ", ChemicalState=" & ChemicalState &
                                             " WHERE (Name = '" & ComputerName & "')", cn_DispenseState)
          cmd_DispenseState.ExecuteNonQuery()
        ElseIf ChemicalCallOff <> 0 And ChemicalTank <> 0 And (CState = "202" Or (WaitFor302D)) And CEnabled = "1" Then
          ChemicalState = 202
          For i As Integer = 0 To 15
            If ChemicalCode(i) IsNot Nothing Then
              If ChemicalStepNumber(i) = ChemicalCallOff.ToString Then
                If ChemicalDispenseResult(i) = "309" Or ChemicalState = 309 Then
                  ChemicalState = 309
                ElseIf ChemicalDispenseResult(i) = "302" Or ChemicalState = 302 Then
                  ChemicalState = 302
                ElseIf ChemicalDispenseResult(i) = "301" Or ChemicalState = 301 Then
                  ChemicalState = 301
                End If
              End If
            End If
          Next
          If ChemicalState = 301 Or ChemicalState = 302 Or ChemicalState = 309 Then
            cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET DispenseDyelot='" & 工單 & "', DispenseReDye='" & 重染 &
                                             "', ChemicalCallOff = " & ChemicalCallOff & ", ChemicalTank=" & ChemicalTank & ", ChemicalState=" & ChemicalState &
                                             " WHERE (Name = '" & ComputerName & "')", cn_DispenseState)
            cmd_DispenseState.ExecuteNonQuery()
          End If
        ElseIf ChemicalCallOff <> 0 And ChemicalTank <> 0 And CState = "301" And CEnabled = "1" Then
          ChemicalState = 301
        ElseIf ChemicalCallOff <> 0 And ChemicalTank <> 0 And CState = "302" And CEnabled = "1" Then
          ChemicalState = 302
          ' If 粉體呼叫 <> "0" Or 粉體呼叫 <> "" Then
          ' cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET CheckPowderRb = '1' WHERE Name = 'NC'", cn_DispenseState)
          ' cmd_DispenseState.ExecuteNonQuery()
          ' End If

        ElseIf ChemicalCallOff <> 0 And ChemicalTank <> 0 And CState = "309" And CEnabled = "1" Then
          ChemicalState = 309
        End If
      End If

      '染料呼叫的規則
      If DyeCallOff = 0 And DyeTank = 0 And ((DState = "") Or (DState = "101") Or (DState = "301") Or (DState = "302") Or (DState = "309") Or (DState = "201") Or (DState = "202") Or (DState = "203") Or (DState = "205") Or (DState = "207")) Then
        DyeState = 101
        cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET DispenseDyelot='" & 工單 & "', DispenseReDye='" & 重染 &
                                             "', DyeCallOff = " & DyeCallOff & ", DyeTank=" & DyeTank & ", DyeState=" & DyeState &
                                             " WHERE (Name = '" & ComputerName & "')", cn_DispenseState)
        cmd_DispenseState.ExecuteNonQuery()
      ElseIf DyeCallOff <> 0 And DyeTank <> 0 And DState = "101" And DEnabled = "1" Then
        DyeState = 201
        cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET DispenseDyelot='" & 工單 & "', DispenseReDye='" & 重染 &
                                             "', DyeCallOff = " & DyeCallOff & ", DyeTank=" & DyeTank & ", DyeState=" & DyeState &
                                             " WHERE (Name = '" & ComputerName & "')", cn_DispenseState)
        cmd_DispenseState.ExecuteNonQuery()
      ElseIf DyeCallOff <> 0 And DyeTank <> 0 And DState = "202" And DEnabled = "1" Then
        DyeState = 202
        ' If Call252AddDye Then
        ' DyeState = 207
        ' cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET DispenseDyelot='" & 工單 & "', DispenseReDye='" & 重染 & _
        '                                              "', DyeCallOff = " & DyeCallOff & ", DyeTank=" & DyeTank & ", DyeState=" & DyeState & _
        '                                              " WHERE (Name = '" & ComputerName & "')", cn_DispenseState)
        ' cmd_DispenseState.ExecuteNonQuery()
        ' End If
      ElseIf DyeCallOff <> 0 And DyeTank <> 0 And (DState = "205" Or DyeState = 205) And DEnabled = "1" And Command44.IsOn Then
        DyeState = 205
        If Call252AddDye Then
          DyeState = 207
          cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET DispenseDyelot='" & 工單 & "', DispenseReDye='" & 重染 &
                                             "', DyeCallOff = " & DyeCallOff & ", DyeTank=" & DyeTank & ", DyeState=" & DyeState &
                                             " WHERE (Name = '" & ComputerName & "')", cn_DispenseState)
          cmd_DispenseState.ExecuteNonQuery()
        End If
      ElseIf DyeCallOff <> 0 And DyeTank <> 0 And (DState = "102" And DyeState = 207) And DEnabled = "1" And Command44.IsOn Then
        DyeState = 207
        If Call252AddDye Then
          DyeState = 207
          cmd_DispenseState = New SqlClient.SqlCommand("UPDATE Machines SET DispenseDyelot='" & 工單 & "', DispenseReDye='" & 重染 &
                                             "', DyeCallOff = " & DyeCallOff & ", DyeTank=" & DyeTank & ", DyeState=" & DyeState &
                                             " WHERE (Name = '" & ComputerName & "')", cn_DispenseState)
          cmd_DispenseState.ExecuteNonQuery()
        End If
      ElseIf DyeCallOff <> 0 And DyeTank <> 0 And DState = "301" And DEnabled = "1" Then
        DyeState = 301
      ElseIf DyeCallOff <> 0 And DyeTank <> 0 And DState = "302" And DEnabled = "1" Then
        DyeState = 302
      ElseIf DyeCallOff <> 0 And DyeTank <> 0 And DState = "309" And DEnabled = "1" Then
        DyeState = 309
      End If

      cn_DispenseState.Close()

      'SQL連線狀況 = My.Computer.Clock.TickCount - SQL連線狀況1

      'SQL連線狀況1 = My.Computer.Clock.TickCount

    Catch ex As Exception
      'Ignore errors
    End Try
  End Sub

  Public Sub 藥缸清洗()

  End Sub
End Class

Public Enum LanguageValue
  English
  ZhTw
  ZhCn
End Enum