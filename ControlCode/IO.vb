Public NotInheritable Class IO
    Inherits MarshalByRefObject

  <IO(IOType.Dinp, 1), Translate("zh-TW", "流量計脈衝訊號")> Public PulseFB As Boolean
  <IO(IOType.Dinp, 2), Translate("zh-TW", "系統自動")> Public SystemAuto As Boolean
  <IO(IOType.Dinp, 3), Translate("zh-TW", "呼叫確認")> Public CallAck As Boolean
  <IO(IOType.Dinp, 4), Translate("zh-TW", "壓力訊號")> Public PressureSw As Boolean
  <IO(IOType.Dinp, 5), Translate("zh-TW", "主馬達訊號")> Public MainPumpFB As Boolean
  <IO(IOType.Dinp, 6), Translate("zh-TW", "纏車")> Public Entanglement As Boolean
  <IO(IOType.Dinp, 7), Translate("zh-TW", "主缸低水位")> Public LowLevel As Boolean
  <IO(IOType.Dinp, 8), Translate("zh-TW", "主缸中水位")> Public MiddleLevel As Boolean
  <IO(IOType.Dinp, 9), Translate("zh-TW", "主缸高水位")> Public HighLevel As Boolean
  <IO(IOType.Dinp, 10), Translate("zh-TW", "主缸溢流水位")> Public OverflowLevel As Boolean
  <IO(IOType.Dinp, 11), Translate("zh-TW", "B高水位")> Public BTankHigh As Boolean
  <IO(IOType.Dinp, 12), Translate("zh-TW", "B中水位")> Public BTankMiddle As Boolean
  <IO(IOType.Dinp, 13), Translate("zh-TW", "B低水位")> Public BTankLow As Boolean
  <IO(IOType.Dinp, 14), Translate("zh-TW", "B備藥完成")> Public BTankReady As Boolean
  <IO(IOType.Dinp, 15), Translate("zh-TW", "C高水位")> Public CTankHigh As Boolean
  <IO(IOType.Dinp, 16), Translate("zh-TW", "C中水位")> Public CTankMiddle As Boolean
  <IO(IOType.Dinp, 17), Translate("zh-TW", "C低水位")> Public CTankLow As Boolean
  <IO(IOType.Dinp, 18), Translate("zh-TW", "C備藥完成")> Public CTankReady As Boolean
  <IO(IOType.Dinp, 19), Translate("zh-TW", "主馬達過載")> Public MainPumpOverload As Boolean
  <IO(IOType.Dinp, 20), Translate("zh-TW", "B馬達過載")> Public BPumpOverload As Boolean
  <IO(IOType.Dinp, 22), Translate("zh-TW", "布頭訊號1")> Public FabricCycleInput1 As Boolean
  <IO(IOType.Dinp, 23), Translate("zh-TW", "布頭訊號2")> Public FabricCycleInput2 As Boolean
  <IO(IOType.Dinp, 24), Translate("zh-TW", "布頭訊號3")> Public FabricCycleInput3 As Boolean
  <IO(IOType.Dinp, 25), Translate("zh-TW", "布頭訊號4")> Public FabricCycleInput4 As Boolean
  <IO(IOType.Dinp, 26), Translate("zh-TW", "噴嘴大小按鈕1")> Public NozzleSizePB1 As Boolean
  <IO(IOType.Dinp, 27), Translate("zh-TW", "噴嘴大小按鈕2")> Public NozzleSizePB2 As Boolean
  <IO(IOType.Dinp, 28), Translate("zh-TW", "噴嘴大小按鈕3")> Public NozzleSizePB3 As Boolean
  <IO(IOType.Dinp, 29), Translate("zh-TW", "噴嘴間隙按鈕1")> Public NozzleGapPB1 As Boolean
  <IO(IOType.Dinp, 30), Translate("zh-TW", "噴嘴間隙按鈕2")> Public NozzleGapPB2 As Boolean
  <IO(IOType.Dinp, 31), Translate("zh-TW", "噴嘴間隙按鈕3")> Public NozzleGapPB3 As Boolean
  <IO(IOType.Dinp, 32), Translate("zh-TW", "噴嘴間隙按鈕4")> Public NozzleGapPB4 As Boolean
  <IO(IOType.Dinp, 33), Translate("zh-TW", "布頭訊號5")> Public FabricCycleInput5 As Boolean
  <IO(IOType.Dinp, 34), Translate("zh-TW", "布頭訊號6")> Public FabricCycleInput6 As Boolean
  <IO(IOType.Dinp, 35), Translate("zh-TW", "布頭訊號7")> Public FabricCycleInput7 As Boolean
  <IO(IOType.Dinp, 36), Translate("zh-TW", "布頭訊號8")> Public FabricCycleInput8 As Boolean
  Public RemoteRun As Boolean
  Public RemoteHalt As Boolean
  Public RemoteUp As Boolean
  Public RemoteDown As Boolean

  '***********************PH功能*************************************************************
  '<IO(IOType.Dinp, 73), Translate("zh-TW", "PH自動")> Public PhAuto As Boolean
  '<IO(IOType.Dinp, 74), Translate("zh-TW", "PH呼叫確認")> Public PhCallAck As Boolean
  '<IO(IOType.Dinp, 75), Translate("zh-TW", "PH循環桶高水位")> Public PhMixTankHighLevel As Boolean
  '<IO(IOType.Dinp, 76), Translate("zh-TW", "PH循環桶低水位")> Public PhMixTankLowLevel As Boolean
  '<IO(IOType.Dinp, 77), Translate("zh-TW", "PH手動檢測")> Public PhManulCheck As Boolean
  '<IO(IOType.Dinp, 78), Translate("zh-TW", "PH手動清洗")> Public PhManulWash As Boolean
  Public PhMixTankHighLevel As Boolean
  Public PhMixTankLowLevel As Boolean
  Public PhCallAck As Boolean

  <IO(IOType.Aninp, 1, , , "%t%%"), Translate("zh-TW", "藥缸B水位")> Public TankBLevel As Short
  <IO(IOType.Aninp, 2, , , "%t%%"), Translate("zh-TW", "藥缸C水位")> Public TankCLevel As Short
  <IO(IOType.Aninp, 3, , , "%t%%"), Translate("zh-TW", "主缸水位")> Public MainTankLevel As Short
  '<IO(IOType.Aninp, 4, , , "0"), Translate("zh-TW", "主泵速度")> Public MainPumpSpeed As Short
  '<IO(IOType.Aninp, 5, , , "0"), Translate("zh-TW", "布輪速度")> Public ReelSpeed As Short
  ' <IO(IOType.Aninp, 6, , , "0"), Translate("zh-TW", "布輪2速度")> Public Reel2Speed As Short
  <IO(IOType.Aninp, 5, , , "%2tm3"), Translate("zh-TW", "蒸氣流量計")> Public SteamFlowmeter As Short
  <IO(IOType.Aninp, 6, , , "%2tpH"), Translate("zh-TW", "PH值")> Public PhValue As Short

  <IO(IOType.Temp, 1, , , "%tC"), Translate("zh-TW", "主缸溫度")> Public MainTemperature As Short
  <IO(IOType.Temp, 2, , , "%tC"), Translate("zh-TW", "B缸溫度")> Public BTankTemperature As Short
  <IO(IOType.Temp, 3, , , "%tC"), Translate("zh-TW", "pH溫度")> Public PhCheckTemp As Short
  Public CTankTemperature As Short


  <IO(IOType.Counter, 1)> Public 瓦時計 As Integer
  <IO(IOType.Counter, 2)> Public 蒸氣流量計 As Integer

  <IO(IOType.Anout, 1), Translate("zh-TW", "比例式昇降溫")> Public TemperatureControl As Short
  <IO(IOType.Anout, 2), Translate("zh-TW", "B加藥控制")> Public BDosingOutput As Short
  <IO(IOType.Anout, 3), Translate("zh-TW", "C加藥控制")> Public CDosingOutput As Short
  <IO(IOType.Anout, 4), Translate("zh-TW", "主泵速度控制")> Public PumpSpeedControl As Short
  <IO(IOType.Anout, 5), Translate("zh-TW", "帶布輪1控制")> Public Reel1SpeedControl As Short
  <IO(IOType.Anout, 6), Translate("zh-TW", "帶布輪2控制")> Public Reel2SpeedControl As Short
  <IO(IOType.Anout, 7), Translate("zh-TW", "帶布輪3控制")> Public Reel3SpeedControl As Short
  <IO(IOType.Anout, 8), Translate("zh-TW", "帶布輪4控制")> Public Reel4SpeedControl As Short
  <IO(IOType.Anout, 9), Translate("zh-TW", "主缸安全溫度")> Public SetSafetyTempToPlc As Integer
  <IO(IOType.Anout, 10), Translate("zh-TW", "加藥安全溫度")> Public SetAddSafetyTempToPlc As Integer
  <IO(IOType.Anout, 11), Translate("zh-TW", "藥缸加藥延遲時間")> Public AddFinishDelayToPlc As Integer
  <IO(IOType.Anout, 12), Translate("zh-TW", "動力排水延遲時間")> Public PowerDrainDelayToPlc As Integer
  <IO(IOType.Anout, 13), Translate("zh-TW", "熱交換器排水延遲")> Public CondensationDelayTimeToPlc As Integer
  <IO(IOType.Anout, 14), Translate("zh-TW", "B缸高水位類比值")> Public BTankHighAIToPlc As Integer
  <IO(IOType.Anout, 15), Translate("zh-TW", "B缸中水位類比值")> Public BTankMiddleAIToPlc As Integer
  <IO(IOType.Anout, 16), Translate("zh-TW", "B缸低水位類比值")> Public BTankLowAIToPlc As Integer
  <IO(IOType.Anout, 17), Translate("zh-TW", "C缸高水位類比值")> Public CTankHighAIToPlc As Integer
  <IO(IOType.Anout, 18), Translate("zh-TW", "C缸中水位類比值")> Public CTankMiddleAIToPlc As Integer
  <IO(IOType.Anout, 19), Translate("zh-TW", "C缸低水位類比值")> Public CTankLowAIToPlc As Integer

  <IO(IOType.Dout, 1), Translate("zh-TW", "昇溫")> Public Heat As Boolean
  <IO(IOType.Dout, 2), Translate("zh-TW", "降溫")> Public Cool As Boolean
  <IO(IOType.Dout, 3), Translate("zh-TW", "熱交換器排水")> Public HxDrain As Boolean
  <IO(IOType.Dout, 4), Translate("zh-TW", "排冷凝水")> Public CondenserDrain As Boolean
  <IO(IOType.Dout, 5), Translate("zh-TW", "冷卻排水")> Public CoolDrain As Boolean
  <IO(IOType.Dout, 6), Translate("zh-TW", "加壓")> Public PressureIn As Boolean
  <IO(IOType.Dout, 7), Translate("zh-TW", "排壓")> Public PressureOut As Boolean
  <IO(IOType.Dout, 8), Translate("zh-TW", "溢流")> Public Overflow As Boolean
  <IO(IOType.Dout, 9), Translate("zh-TW", "進冷水")> Public ColdFill As Boolean
  <IO(IOType.Dout, 10), Translate("zh-TW", "進熱水")> Public HotFill As Boolean
  <IO(IOType.Dout, 11), Translate("zh-TW", "排清水")> Public HotDrain As Boolean
  <IO(IOType.Dout, 12), Translate("zh-TW", "排廢水")> Public Drain As Boolean
  <IO(IOType.Dout, 13), Translate("zh-TW", "主馬達啟動")> Public PumpOn As Boolean
  <IO(IOType.Dout, 14), Translate("zh-TW", "主馬達停止")> Public PumpOff As Boolean
  <IO(IOType.Dout, 15), Translate("zh-TW", "B進冷水")> Public BTankColdFill As Boolean
  <IO(IOType.Dout, 16), Translate("zh-TW", "B進迴水")> Public BTankCirculate2 As Boolean
  <IO(IOType.Dout, 17), Translate("zh-TW", "B排水")> Public BTankDrain As Boolean
  <IO(IOType.Dout, 18), Translate("zh-TW", "C Dosing")> Public CDosing As Boolean
  <IO(IOType.Dout, 19), Translate("zh-TW", "B加藥馬達")> Public BTankAddPump As Boolean
  <IO(IOType.Dout, 20), Translate("zh-TW", "B循環攪拌")> Public BTankMixCir2 As Boolean
  <IO(IOType.Dout, 21), Translate("zh-TW", "呼叫")> Public AlarmLamp As Boolean
  <IO(IOType.Dout, 22), Translate("zh-TW", "取樣")> Public SampleLamp As Boolean
  <IO(IOType.Dout, 23), Translate("zh-TW", "C進冷水")> Public CTankColdFill As Boolean
  <IO(IOType.Dout, 24), Translate("zh-TW", "C進迴水")> Public CTankCirculate2 As Boolean
  <IO(IOType.Dout, 25), Translate("zh-TW", "C排水")> Public CTankDrain As Boolean
  <IO(IOType.Dout, 26), Translate("zh-TW", "C加藥")> Public CTankAddition As Boolean
  <IO(IOType.Dout, 27), Translate("zh-TW", "C循環攪拌")> Public CTankMixCir2 As Boolean
  <IO(IOType.Dout, 28), Translate("zh-TW", "B加藥")> Public BTankAddition As Boolean
  <IO(IOType.Dout, 29), Translate("zh-TW", "B備藥完成燈")> Public BTankOkLamp As Boolean
  <IO(IOType.Dout, 30), Translate("zh-TW", "動力排水")> Public PowerDrain As Boolean
  <IO(IOType.Dout, 31), Translate("zh-TW", "C加藥馬達")> Public CTankAddPump As Boolean
  <IO(IOType.Dout, 32), Translate("zh-TW", "B Dosing")> Public BDosing As Boolean
  <IO(IOType.Dout, 33), Translate("zh-TW", "噴嘴大小燈1")> Public NozzleSizeLamp1 As Boolean
  <IO(IOType.Dout, 34), Translate("zh-TW", "噴嘴大小燈2")> Public NozzleSizeLamp2 As Boolean
  <IO(IOType.Dout, 35), Translate("zh-TW", "噴嘴大小燈3")> Public NozzleSizeLamp3 As Boolean
  <IO(IOType.Dout, 36), Translate("zh-TW", "噴嘴間隙燈1")> Public NozzleGapLamp1 As Boolean
  <IO(IOType.Dout, 37), Translate("zh-TW", "噴嘴間隙燈2")> Public NozzleGapLamp2 As Boolean
  <IO(IOType.Dout, 38), Translate("zh-TW", "噴嘴間隙燈3")> Public NozzleGapLamp3 As Boolean
  <IO(IOType.Dout, 39), Translate("zh-TW", "噴嘴間隙燈4")> Public NozzleGapLamp4 As Boolean
  <IO(IOType.Dout, 40), Translate("zh-TW", "pH回流")> Public PhFillCirculate As Boolean
  <IO(IOType.Dout, 41), Translate("zh-TW", "pH排水")> Public PhDrain As Boolean
  <IO(IOType.Dout, 42), Translate("zh-TW", "pH冷卻入水")> Public PhCool As Boolean
  <IO(IOType.Dout, 43), Translate("zh-TW", "pH入水")> Public PhFill As Boolean
  <IO(IOType.Dout, 44), Translate("zh-TW", "pH加酸閥")> Public PhAddHacOut As Boolean
  <IO(IOType.Dout, 45), Translate("zh-TW", "熱水降溫")> Public HotWaterCooling As Boolean
  <IO(IOType.Dout, 46), Translate("zh-TW", "冷水降溫")> Public ColdWaterCooling As Boolean
  <IO(IOType.Dout, 47), Translate("zh-TW", "進水3")> Public Fill3 As Boolean
  <IO(IOType.Dout, 48), Translate("zh-TW", "B缸加熱")> Public BTankHeat As Boolean
  <IO(IOType.Dout, 53), Translate("zh-TW", "測試")> Public Testing As Boolean
  <IO(IOType.Dout, 54), Translate("zh-TW", "高速計數器重置")> Public HCounterReset As Boolean


  <Translate("zh-TW", "AI1實際值(唯讀)"), Category("AI1校正參數"), DefaultValue(1000)> Public Parameters_AI1RawValueForRead As Short
  <Translate("zh-TW", "AI1 (取低值)"), Category("AI1校正參數"), DefaultValue(1000)> Public Parameters_AI1MinValue As Short
  <Translate("zh-TW", "AI1 (取高值)"), Category("AI1校正參數"), DefaultValue(1000)> Public Parameters_AI1MaxValue As Short
  <Translate("zh-TW", "AI1 (設低值)"), Category("AI1校正參數"), DefaultValue(1000)> Public Parameters_AI1SetMinValue As Short
  <Translate("zh-TW", "AI1 (設高值)"), Category("AI1校正參數"), DefaultValue(1000)> Public Parameters_AI1SetMaxValue As Short

  <Translate("zh-TW", "AI2實際值(唯讀)"), Category("AI2校正參數"), DefaultValue(1000)> Public Parameters_AI2RawValueForRead As Short
  <Translate("zh-TW", "AI2 (取低值)"), Category("AI2校正參數"), DefaultValue(1000)> Public Parameters_AI2MinValue As Short
  <Translate("zh-TW", "AI2 (取高值)"), Category("AI2校正參數"), DefaultValue(1000)> Public Parameters_AI2MaxValue As Short
  <Translate("zh-TW", "AI2 (設低值)"), Category("AI2校正參數"), DefaultValue(1000)> Public Parameters_AI2SetMinValue As Short
  <Translate("zh-TW", "AI2 (設高值)"), Category("AI2校正參數"), DefaultValue(1000)> Public Parameters_AI2SetMaxValue As Short

  <Translate("zh-TW", "AI3實際值(唯讀)"), Category("AI3校正參數"), DefaultValue(1000)> Public Parameters_AI3RawValueForRead As Short
  <Translate("zh-TW", "AI3 (取低值)"), Category("AI3校正參數"), DefaultValue(1000)> Public Parameters_AI3MinValue As Short
  <Translate("zh-TW", "AI3 (取高值)"), Category("AI3校正參數"), DefaultValue(1000)> Public Parameters_AI3MaxValue As Short
  <Translate("zh-TW", "AI3 (設低值)"), Category("AI3校正參數"), DefaultValue(1000)> Public Parameters_AI3SetMinValue As Short
  <Translate("zh-TW", "AI3 (設高值)"), Category("AI3校正參數"), DefaultValue(1000)> Public Parameters_AI3SetMaxValue As Short

  <Translate("zh-TW", "AI4實際值(唯讀)"), Category("AI4校正參數"), DefaultValue(1000)> Public Parameters_AI4RawValueForRead As Short
  <Translate("zh-TW", "AI4 (取低值)"), Category("AI4校正參數"), DefaultValue(1000)> Public Parameters_AI4MinValue As Short
  <Translate("zh-TW", "AI4 (取高值)"), Category("AI4校正參數"), DefaultValue(1000)> Public Parameters_AI4MaxValue As Short
  <Translate("zh-TW", "AI4 (設低值)"), Category("AI4校正參數"), DefaultValue(1000)> Public Parameters_AI4SetMinValue As Short
  <Translate("zh-TW", "AI4 (設高值)"), Category("AI4校正參數"), DefaultValue(1000)> Public Parameters_AI4SetMaxValue As Short

  <Translate("zh-TW", "AI5實際值(唯讀)"), Category("AI5校正參數"), DefaultValue(1000)> Public Parameters_AI5RawValueForRead As Short
  <Translate("zh-TW", "AI5 (取低值)"), Category("AI5校正參數"), DefaultValue(1000)> Public Parameters_AI5MinValue As Short
  <Translate("zh-TW", "AI5 (取高值)"), Category("AI5校正參數"), DefaultValue(1000)> Public Parameters_AI5MaxValue As Short
  <Translate("zh-TW", "AI5 (設低值)"), Category("AI5校正參數"), DefaultValue(1000)> Public Parameters_AI5SetMinValue As Short
  <Translate("zh-TW", "AI5 (設高值)"), Category("AI5校正參數"), DefaultValue(1000)> Public Parameters_AI5SetMaxValue As Short

  <Translate("zh-TW", "AI6實際值(唯讀)"), Category("AI6校正參數"), DefaultValue(1000)> Public Parameters_AI6RawValueForRead As Short
  <Translate("zh-TW", "AI6 (取低值)"), Category("AI6校正參數"), DefaultValue(1000)> Public Parameters_AI6MinValue As Short
  <Translate("zh-TW", "AI6 (取高值)"), Category("AI6校正參數"), DefaultValue(1000)> Public Parameters_AI6MaxValue As Short
  <Translate("zh-TW", "AI6 (設低值)"), Category("AI6校正參數"), DefaultValue(1000)> Public Parameters_AI6SetMinValue As Short
  <Translate("zh-TW", "AI6 (設高值)"), Category("AI6校正參數"), DefaultValue(1000)> Public Parameters_AI6SetMaxValue As Short

  <Translate("zh-TW", "實際AI1讀值"), Category("校正參數")> Public RealAI1 As Short
  <Translate("zh-TW", "實際AI2讀值"), Category("校正參數")> Public RealAI2 As Short
  <Translate("zh-TW", "實際AI3讀值"), Category("校正參數")> Public RealAI3 As Short
  <Translate("zh-TW", "實際AI4讀值"), Category("校正參數")> Public RealAI4 As Short
  <Translate("zh-TW", "實際AI5讀值"), Category("校正參數")> Public RealAI5 As Short
  <Translate("zh-TW", "實際AI6讀值"), Category("校正參數")> Public RealAI6 As Short


  <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Function ReadInputs(ByVal dinp() As Boolean, ByVal aninp() As Short, ByVal temp() As Short) As Boolean
    CheckForSerialPortParametersChanged()
    If Plc.Read(1, "WM400", dinp) = Ports.LA60B.Result.OK Then
      ReadInputs = True
      PlcTimeout.TimeRemaining = 1 ' must have a problem for at least 1 second before triggering the alarm
    End If
    Static ai(13) As UShort
    If Plc.Read(1, "D300", ai) = Ports.LA60B.Result.OK Then
      ' Copy temperatures directly
      temp(1) = CType(ai(1), Short)
      temp(2) = CType(ai(2), Short)
      temp(3) = CType(ai(3), Short)
      aninp(1) = CType(ai(4), Short)
      aninp(2) = CType(ai(5), Short)
      aninp(3) = CType(ai(6), Short)
      aninp(4) = CType(ai(7), Short)
      aninp(5) = CType(ai(8), Short)
      aninp(6) = CType(ai(9), Short)
      Parameters_AI1RawValueForRead = CType(ai(4), Short)
      Parameters_AI2RawValueForRead = CType(ai(5), Short)
      Parameters_AI3RawValueForRead = CType(ai(6), Short)
      Parameters_AI4RawValueForRead = CType(ai(7), Short)
      Parameters_AI5RawValueForRead = CType(ai(8), Short)
      Parameters_AI6RawValueForRead = CType(ai(9), Short)

      Dim value1, value2, value3, value4, value5, value6 As Double
      Dim Parameter_Max, Parameter_Min, Parameter_SetMax, Parameter_SetMin As Short


      For i = 0 To 5
        Select Case i
          Case 0
            Parameter_Min = Parameters_AI1MinValue
            Parameter_Max = Parameters_AI1MaxValue
            Parameter_SetMin = Parameters_AI1SetMinValue
            Parameter_SetMax = Parameters_AI1SetMaxValue
          Case 1
            Parameter_Min = Parameters_AI2MinValue
            Parameter_Max = Parameters_AI2MaxValue
            Parameter_SetMin = Parameters_AI2SetMinValue
            Parameter_SetMax = Parameters_AI2SetMaxValue
          Case 2
            Parameter_Min = Parameters_AI3MinValue
            Parameter_Max = Parameters_AI3MaxValue
            Parameter_SetMin = Parameters_AI3SetMinValue
            Parameter_SetMax = Parameters_AI3SetMaxValue
          Case 3
            Parameter_Min = Parameters_AI4MinValue
            Parameter_Max = Parameters_AI4MaxValue
            Parameter_SetMin = Parameters_AI4SetMinValue
            Parameter_SetMax = Parameters_AI4SetMaxValue
          Case 4
            Parameter_Min = Parameters_AI5MinValue
            Parameter_Max = Parameters_AI5MaxValue
            Parameter_SetMin = Parameters_AI5SetMinValue
            Parameter_SetMax = Parameters_AI5SetMaxValue
          Case 5
            Parameter_Min = Parameters_AI6MinValue
            Parameter_Max = Parameters_AI6MaxValue
            Parameter_SetMin = Parameters_AI6SetMinValue
            Parameter_SetMax = Parameters_AI6SetMaxValue
        End Select


        value1 = Parameter_Max - Parameter_Min

        value2 = (ai(4 + i)) - Parameter_Min

        value3 = value2 / value1

        value4 = Parameter_SetMax - Parameter_SetMin

        value5 = value4 * value3

        value6 = value5 + Parameter_SetMin

        aninp(1 + i) = CType(value6, Short)

        'RealPhValue = CType(ai(5 + i), Short)

        Select Case i
          Case 0
            RealAI1 = CType(ai(4), Short)
          Case 1
            RealAI2 = CType(ai(5), Short)
          Case 2
            RealAI3 = CType(ai(6), Short)
          Case 3
            RealAI4 = CType(ai(7), Short)
          Case 4
            RealAI5 = CType(ai(8), Short)
          Case 5
            RealAI6 = CType(ai(9), Short)
        End Select
      Next



      ' Copy counters directly
      瓦時計 = ai(10)
      蒸氣流量計 = ai(11)
    End If
  End Function

    <EditorBrowsable(EditorBrowsableState.Advanced)> _
    Public Sub WriteOutputs(ByVal dout() As Boolean, ByVal anout() As Short)
        Static watchdogDout(128) As Boolean
        Array.Copy(dout, watchdogDout, dout.Length)
        watchdogDout(128) = (Date.UtcNow.Millisecond < 500)  ' alternate the last output to keep the plc happy
        ' M = Internal Relays, W = access as words
        Plc.Write(1, "WM272", watchdogDout, Ports.WriteMode.Optimised)

        ' Rescale: 100.0% = 255
        For i = 1 To 6
            anout(i) = CType((anout(i) * 255) \ 1000, Short)
        Next i
    For i = 7 To 19
      anout(i) = CType(anout(i), Short)
    Next i
        Plc.Write(1, "D400", anout, Ports.WriteMode.Optimised)
    End Sub

    Private ReadOnly ControlCode As ControlCode
    <EditorBrowsable(EditorBrowsableState.Advanced)> Public Plc As Ports.LA60B
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub

    Private Sub CheckForSerialPortParametersChanged()
        If LastComNumber <> ControlCode.Parameters.ComNumber OrElse LastComBaudRate <> ControlCode.Parameters.ComBaudRate Then
            ReOpenSerialPort()
        End If
    End Sub
    Private LastComNumber, LastComBaudRate As Integer
    Private Sub ReOpenSerialPort()
        If Plc IsNot Nothing Then DirectCast(Plc, IDisposable).Dispose() : Plc = Nothing
        LastComNumber = ControlCode.Parameters.ComNumber
        LastComBaudRate = ControlCode.Parameters.ComBaudRate
        Plc = New Ports.LA60B(New Ports.SerialPort("COM" & LastComNumber.ToString, LastComBaudRate, _
                                                   System.IO.Ports.Parity.Even, 7, System.IO.Ports.StopBits.One))
    End Sub

    Private PlcTimeout As New Timer ' if no communications for 1 second, then make the fault below true
    Friend ReadOnly Property PlcFault() As Boolean
        Get
            Return PlcTimeout.Finished
        End Get
    End Property
End Class

Partial Public Class ControlCode
  Public ReadOnly IO As New IO(Me)
End Class
