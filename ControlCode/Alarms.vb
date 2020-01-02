Partial Class ControlCode
    <Translate("zh", "準備入布")> Public MessageLoadFabric As Boolean
    <Translate("zh", "準備出布")> Public MessageUnloadFiber As Boolean
    <Translate("zh", "呼叫操作員")> Public MessageCallOperator As Boolean
    <Translate("zh", "取樣對色")> Public MessageTakeSample As Boolean
    <Translate("zh", "藥缸備水中")> Public MessageSTankFilling As Boolean
    <Translate("zh", "藥缸請備藥")> Public MessageSTankPrepare As Boolean
    <Translate("zh", "藥缸攪拌中")> Public MessageSTankMixing As Boolean
    <Translate("zh", "主缸昇溫中")> Public MessageHeatingNow As Boolean
    <Translate("zh", "主缸持溫中")> Public MessageHoldingNow As Boolean
    <Translate("zh", "主缸降溫中")> Public MessageCoolingNow As Boolean
    <Translate("zh", "藥缸稀釋加藥中")> Public MessageSTankDiluteAddingNow As Boolean
    <Translate("zh", "藥缸加藥中")> Public MessageSTankAddingNow As Boolean
    <Translate("zh", "藥缸dos加藥中")> Public MessageSTankDosingNow As Boolean
    <Translate("zh", "系統暫停")> Public MessageSystemPause As Boolean
    <Translate("zh", "程式結束")> Public MessageProgramFinish As Boolean

End Class

Public NotInheritable Class Alarms
  Inherits MarshalByRefObject

  <Translate("zh", "系統手動操作中")> Public ManualOperation As Boolean
  <Translate("zh", "主排風故障")> Public MainElectricFanError As Boolean
  <Translate("zh", "溫度過高無法進水")> Public HighTempNoFill As Boolean
  <Translate("zh", "溫度過高無法加藥")> Public HighTempNoAdd As Boolean
  <Translate("zh", "溫度過高無法排水")> Public HighTempNoDrain As Boolean
  <Translate("zh", "纏車")> Public FabricStop As Boolean
  <Translate("zh", "等待備藥OK")> Public STankNotReady As Boolean
  <Translate("zh", "布輪加藥馬達過載")> Public AddMotorOverload As Boolean
  <Translate("zh", "主馬達異常")> Public MainPumpError As Boolean
  <Translate("zh", "主馬達過載")> Public MainPumpOverload As Boolean
  <Translate("zh", "終端顯示器異常")> Public TerminalError As Boolean
  <Translate("zh", "Plc異常")> Public PlcError As Boolean
  <Translate("zh", "蒸氣不足")> Public InsufficientSteam As Boolean
  <Translate("zh", "Pt1斷路")> Public Pt1Open As Boolean
  <Translate("zh", "Pt1短路")> Public Pt1Short As Boolean
  <Translate("zh", "A缸溫度異常")> Public ATempError As Boolean
  <Translate("zh", "冷卻水不足")> Public CoolingNotEnough As Boolean
  <Translate("zh", "進水完成")> Public FillFinish As Boolean
  <Translate("zh", "排水完成")> Public DrainFinish As Boolean
  <Translate("zh", "A缸捕水異常")> Public ATankFillError As Boolean
  <Translate("zh", "主缸進水異常")> Public MainTankFillError As Boolean
  <Translate("zh", "偵測到纏車訊號")> Public FabricStopWas As Boolean
  <Translate("zh", "不允許本機端載入染程")> Public NotAllowLoadProgramFromLocal As Boolean
  '---------------------------------------------------------------------------------------------
  <Translate("zh", "溫度過高無法加酸")> Public HighTempNoAddHac As Boolean
  <Translate("zh", "停止加酸，加酸總量超過目標量")> Public MessageAddHacError As Boolean
  <Translate("zh", "酸控無反應，請注意！")> Public LongtimeToHac As Boolean
  <Translate("zh", "主程序已結束，pH程序尚未完成，請注意！")> Public pHstillNoFinishControl As Boolean
  <Translate("zh", "pH Sensor超過安全溫度，請檢查管路！")> Public pHSensorHinhTemp As Boolean
  '---------------------------------------------------------------------------------------------
  <Translate("zh", "水位超過10%")> Public LevelOver10Percent As Boolean
  <Translate("zh", "水位超過15%")> Public LevelOver15Percent As Boolean
  <Translate("zh", "水位超過20%")> Public LevelOver20Percent As Boolean
  <Translate("zh", "水位不足10%")> Public LevelShort10Percent As Boolean
  <Translate("zh", "水位不足15%")> Public LevelShort15Percent As Boolean
  <Translate("zh", "水位不足20%")> Public LevelShort20Percent As Boolean
  <Translate("zh", "酸度超過0‧3")> Public pHOver03 As Boolean
  <Translate("zh", "酸度超過0‧6")> Public pHOver06 As Boolean
  <Translate("zh", "酸度超過0‧9")> Public pHOver09 As Boolean
  <Translate("zh", "酸度不足0‧3")> Public pHShort03 As Boolean
  <Translate("zh", "酸度不足0‧6")> Public pHShort06 As Boolean
  <Translate("zh", "酸度不足0‧9")> Public pHShort09 As Boolean
  <Translate("zh", "升溫超過3度")> Public HeatOver3Degree As Boolean
  <Translate("zh", "升溫超過5度")> Public HeatOver5Degree As Boolean
  <Translate("zh", "升溫超過10度")> Public HeatOver10Degree As Boolean
  <Translate("zh", "升溫不足3度")> Public HeatShort3Degree As Boolean
  <Translate("zh", "升溫不足5度")> Public HeatShort5Degree As Boolean
  <Translate("zh", "升溫不足10度")> Public HeatShort10Degree As Boolean
  <Translate("zh", "降溫超過3度")> Public CoolOver3Degree As Boolean
  <Translate("zh", "降溫超過5度")> Public CoolOver5Degree As Boolean
  <Translate("zh", "降溫超過10度")> Public CoolOver10Degree As Boolean
  <Translate("zh", "降溫不足3度")> Public CoolShort3Degree As Boolean
  <Translate("zh", "降溫不足5度")> Public CoolShort5Degree As Boolean
  <Translate("zh", "降溫不足10度")> Public CoolShort10Degree As Boolean
  <Translate("zh", "噴壓超過0‧1")> Public NozzlePressureOver01 As Boolean
  <Translate("zh", "噴壓超過0‧15")> Public NozzlePressureOver015 As Boolean
  <Translate("zh", "噴壓超過0‧2")> Public NozzlePressureOver02 As Boolean
  <Translate("zh", "噴壓不足0‧1")> Public NozzlePressureShort01 As Boolean
  <Translate("zh", "噴壓不足0‧15")> Public NozzlePressureShort015 As Boolean
  <Translate("zh", "噴壓不足0‧2")> Public NozzlePressureShort02 As Boolean
End Class

Partial Public Class ControlCode
  Public ReadOnly Alarms As New Alarms
End Class
