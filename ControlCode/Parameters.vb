Public NotInheritable Class Parameters
  Inherits MarshalByRefObject

  '主缸參數
  <Translate("zh", "MT:主缸排水延遲時間秒"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數"), _
   Description("When system runs main tank drain function, it will start counting delay time as main tank low level is off. The system will not stop drain until delay time up."), _
   TranslateDescription("zh", "當主缸執行排水功能，若主缸低水位訊號Low，則開始計算延遲時間，延遲時間到達即結束排水")> Public DrainDelay As Integer = 60

  <Translate("zh", "MT:主缸排水安全時間分"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數"), _
   Description("When system runs main tank drain function, if it runs over set time, the system will cancel function."), _
   TranslateDescription("zh", "當主缸執行排水功能，若超過設定時間則強制結束")> Public SetDrainSafetyTime As Integer = 5

  <Translate("zh", "MT:主缸熱交換器排水時間秒"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數"), _
  Description("主缸升溫時會先開啟熱交換器排水閥，待設定時間到達則將熱交換器排水閥關閉，開啟排冷凝水閥")> Public CondensationDelayTime As Integer = 60

  <Translate("zh", "MT:主缸排壓安全溫度度"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數"), _
  Description("降溫時，當主缸溫度低於設定值，則開始進行點放排壓動作")> Public SetPressureOutTemp As Integer = 85

  <Translate("zh", "MT:主缸安全溫度度"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數"), _
  Description("當主缸溫度高於設定值，則不允許進水、排水、溢流動作")> Public SetSafetyTemp As Integer = 85

  <Translate("zh", "MT:主缸加藥安全溫度度"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數"), _
  Description("當主缸溫度高於設定值，則不允許加藥動作")> Public SetAddSafetyTemp As Integer = 85

    <Translate("zh", "MT:帶布輪啟動延遲時間秒"), Category("Main Tank"), _
     TranslateCategory("zh", "主缸參數"), _
    Description("當主馬達啟動後，即開始計算延遲時間，時間到達時即啟動帶布輪正轉")> Public ReelStartDelayTime As Integer = 15

  <Translate("zh", "MT:昇溫閥種類，0數位，1類比"), Category("Main Tank"), _
   Translate("EN", "MT:Heat Valve Type, 0:OnOff, 1:Analog"), _
   TranslateCategory("zh", "主缸參數")> Public HeatValveType As Integer = 0

  <Translate("zh", "MT:降溫閥種類，0數位，1類比"), Category("Main Tank"), _
   Translate("EN", "MT:Cool Valve Type, 0:OnOff, 1:Analog"), _
   TranslateCategory("zh", "主缸參數")> Public CoolValveType As Integer = 0

  ' <Translate("zh", "MT:流量進水補水時間秒"), Category("Main Tank"), _
  '  TranslateCategory("zh", "主缸參數")> Public FlowmeterRefillDelay As Integer = 20

  <Translate("zh", "MT:動力排水延遲時間秒"), Category("Main Tank"), _
     TranslateCategory("zh", "主缸參數")> Public PowerDrainDelay As Integer = 5

  <Translate("zh", "MT:降溫回收進水0否1是"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數")> Public CoolFillYes0No1 As Integer = 0

  <Translate("zh", "MT:Liter Per Counter"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數")> Public VolumePerCount As Integer = 1

  <Translate("zh", "MT:主缸最小入水量"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數")> Public MainTankFillMinLiter As Integer = 1000

  <Translate("zh", "MT:主缸最大入水量"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數")> Public MainTankFillMaxLiter As Integer = 4000

    <Translate("zh", "MT:主缸溢流水位容量(公升)"), Category("Main Tank"), _
TranslateCategory("zh", "主缸參數")> Public SetMainTankOverflowVolume As Integer = 4000

  <Translate("zh", "MT:主缸高水位容量(公升)"), Category("Main Tank"), _
  TranslateCategory("zh", "主缸參數")> Public SetMainTankHighVolume As Integer = 3000

  <Translate("zh", "MT:主缸中水位容量(公升)"), Category("Main Tank"), _
  TranslateCategory("zh", "主缸參數")> Public SetMainTankMidVolume As Integer = 2000

  ' <Translate("zh", "MT:熱水降溫溫度"), Category("Main Tank"), _
  'TranslateCategory("zh", "主缸參數")> Public SetHotCoolingTemperature As Integer = 120

  '<Translate("zh", "MT:溫水降溫溫度"), Category("Main Tank"), _
  'TranslateCategory("zh", "主缸參數")> Public SetWarmCoolingTemperature As Integer = 100

  '<Translate("zh", "MT:使用熱水降溫"), Category("Main Tank"), _
  'TranslateCategory("zh", "主缸參數")> Public HotCooling As Integer = 0

  <Translate("zh", "MTS:主馬達轉速控制預設參數"), Category("Main Control"), _
   TranslateCategory("zh", "速度參數")> Public 主馬達預設參數 As Integer = 60

  <Translate("zh", "MTS:帶布輪轉速預設參數"), Category("Main Control"), _
     TranslateCategory("zh", "速度參數")> Public 帶布輪預設參數 As Integer = 60

  '<Translate("zh", "MT:主馬達智能控制"), Category("Main Tank"), _
  'TranslateCategory("zh", "主缸參數")> Public 主馬達智能控制 As Integer = 0

  '<Translate("zh", "MT:壓差最大基準值"), Category("Main Tank"), _
  'TranslateCategory("zh", "主缸參數")> Public PressureSet As Integer = 155

  <Translate("zh", "MT:排壓點放次數"), Category("Main Tank"), _
 TranslateCategory("zh", "主缸參數"), _
      Description("Set pressure out on-off times when cooling and reach pressure out temperature."), _
     TranslateDescription("zh", "當降溫且到達排壓設定溫度時, 設定排壓點放次數")> Public SetPressureOutTimes As Integer = 10

  <Translate("zh", "MT:排壓點放開啟時間ms"), Category("Main Tank"), _
TranslateCategory("zh", "主缸參數"), _
TranslateDescription("zh", "當排壓點放時，每次排壓閥開啟的時間，單位是ms")> Public SetPressureOutOnTime As Integer = 1000

  <Translate("zh", "MT:排壓點放關閉時間ms"), Category("Main Tank"), _
TranslateCategory("zh", "主缸參數"), _
TranslateDescription("zh", "當排壓點放時，每次排壓閥關閉的時間，單位是ms")> Public SetPressureOutOffTime As Integer = 1000

    <Translate("zh", "MT:升溫中允許開排壓"), Category("Main Tank"), _
TranslateCategory("zh", "主缸參數"), _
     Description("Set if it open pressure out valve when heating. 0=Disable, 1=Enable."), _
     TranslateDescription("zh", "設定是否允許在升溫中開排壓. 0=不允許, 1=允許.")> Public AllowPressureOutAsHeating As Integer = 0

    <Translate("zh", "MT:主缸低水位容量(公升)"), Category("Main Tank"), _
    TranslateCategory("zh", "主缸參數")> Public SetMainTankLowVolume As Integer = 1000

  <Translate("zh", "MT:主缸類比水位AI值01"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set analog input value for main tank level."), _
   TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput01 As Integer

  <Translate("zh", "MT:主缸類比水位AI值02"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set analog input value for main tank level."), _
   TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput02 As Integer

  <Translate("zh", "MT:主缸類比水位AI值03"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput03 As Integer

  <Translate("zh", "MT:主缸類比水位AI值04"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput04 As Integer

  <Translate("zh", "MT:主缸類比水位AI值05"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput05 As Integer

  <Translate("zh", "MT:主缸類比水位AI值06"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput06 As Integer

  <Translate("zh", "MT:主缸類比水位AI值07"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput07 As Integer

  <Translate("zh", "MT:主缸類比水位AI值08"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput08 As Integer

  <Translate("zh", "MT:主缸類比水位AI值09"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput09 As Integer

  <Translate("zh", "MT:主缸類比水位AI值10"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput10 As Integer

  <Translate("zh", "MT:主缸類比水位AI值11"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput11 As Integer

  <Translate("zh", "MT:主缸類比水位AI值12"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput12 As Integer

  <Translate("zh", "MT:主缸類比水位AI值13"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput13 As Integer

  <Translate("zh", "MT:主缸類比水位AI值14"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput14 As Integer

  <Translate("zh", "MT:主缸類比水位AI值15"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput15 As Integer

  <Translate("zh", "MT:主缸類比水位AI值16"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput16 As Integer

  <Translate("zh", "MT:主缸類比水位AI值17"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput17 As Integer

  <Translate("zh", "MT:主缸類比水位AI值18"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput18 As Integer

  <Translate("zh", "MT:主缸類比水位AI值19"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput19 As Integer

  <Translate("zh", "MT:主缸類比水位AI值20"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput20 As Integer

  <Translate("zh", "MT:主缸類比水位AI值21"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput21 As Integer

  <Translate("zh", "MT:主缸類比水位AI值22"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput22 As Integer

  <Translate("zh", "MT:主缸類比水位AI值23"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput23 As Integer

  <Translate("zh", "MT:主缸類比水位AI值24"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput24 As Integer

  <Translate("zh", "MT:主缸類比水位AI值25"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput25 As Integer

  <Translate("zh", "MT:主缸類比水位AI值26"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput26 As Integer

  <Translate("zh", "MT:主缸類比水位AI值27"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput27 As Integer

  <Translate("zh", "MT:主缸類比水位AI值28"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput28 As Integer

  <Translate("zh", "MT:主缸類比水位AI值29"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput29 As Integer

  <Translate("zh", "MT:主缸類比水位AI值30"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput30 As Integer

  <Translate("zh", "MT:主缸類比水位AI值31"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput31 As Integer

  <Translate("zh", "MT:主缸類比水位AI值32"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput32 As Integer

  <Translate("zh", "MT:主缸類比水位AI值33"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput33 As Integer

  <Translate("zh", "MT:主缸類比水位AI值34"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput34 As Integer

  <Translate("zh", "MT:主缸類比水位AI值35"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput35 As Integer

  <Translate("zh", "MT:主缸類比水位AI值36"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput36 As Integer

  <Translate("zh", "MT:主缸類比水位AI值37"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput37 As Integer

  <Translate("zh", "MT:主缸類比水位AI值38"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput38 As Integer

  <Translate("zh", "MT:主缸類比水位AI值39"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput39 As Integer

  <Translate("zh", "MT:主缸類比水位AI值40"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput40 As Integer

  <Translate("zh", "MT:主缸類比水位AI值41"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput41 As Integer

  <Translate("zh", "MT:主缸類比水位AI值42"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput42 As Integer

  <Translate("zh", "MT:主缸類比水位AI值43"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput43 As Integer

  <Translate("zh", "MT:主缸類比水位AI值44"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput44 As Integer

  <Translate("zh", "MT:主缸類比水位AI值45"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput45 As Integer

  <Translate("zh", "MT:主缸類比水位AI值46"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput46 As Integer

  <Translate("zh", "MT:主缸類比水位AI值47"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput47 As Integer

  <Translate("zh", "MT:主缸類比水位AI值48"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput48 As Integer

  <Translate("zh", "MT:主缸類比水位AI值49"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput49 As Integer

  <Translate("zh", "MT:主缸類比水位AI值50"), Category("Main Tank Calibration"), _
 TranslateCategory("zh", "主缸校正參數"), _
 Description("Set analog input value for main tank level."), _
 TranslateDescription("zh", "設定主缸類比水位的讀取值")> Public SetMainTankAnalogInput50 As Integer

  <Translate("zh", "MT:主缸類比水位設定值01"), Category("Main Tank Calibration"), _
     TranslateCategory("zh", "主缸校正參數"), _
     Description("Set main tank volume by liters"), _
     TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume01 As Integer

  <Translate("zh", "MT:主缸類比水位設定值02"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume02 As Integer

  <Translate("zh", "MT:主缸類比水位設定值03"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume03 As Integer

  <Translate("zh", "MT:主缸類比水位設定值04"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume04 As Integer

  <Translate("zh", "MT:主缸類比水位設定值05"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume05 As Integer

  <Translate("zh", "MT:主缸類比水位設定值06"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume06 As Integer

  <Translate("zh", "MT:主缸類比水位設定值07"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume07 As Integer

  <Translate("zh", "MT:主缸類比水位設定值08"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume08 As Integer

  <Translate("zh", "MT:主缸類比水位設定值09"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume09 As Integer

  <Translate("zh", "MT:主缸類比水位設定值10"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume10 As Integer

  <Translate("zh", "MT:主缸類比水位設定值11"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume11 As Integer

  <Translate("zh", "MT:主缸類比水位設定值12"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume12 As Integer

  <Translate("zh", "MT:主缸類比水位設定值13"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume13 As Integer

  <Translate("zh", "MT:主缸類比水位設定值14"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume14 As Integer

  <Translate("zh", "MT:主缸類比水位設定值15"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume15 As Integer

  <Translate("zh", "MT:主缸類比水位設定值16"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume16 As Integer

  <Translate("zh", "MT:主缸類比水位設定值17"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume17 As Integer

  <Translate("zh", "MT:主缸類比水位設定值18"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume18 As Integer

  <Translate("zh", "MT:主缸類比水位設定值19"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume19 As Integer

  <Translate("zh", "MT:主缸類比水位設定值20"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume20 As Integer

  <Translate("zh", "MT:主缸類比水位設定值21"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume21 As Integer

  <Translate("zh", "MT:主缸類比水位設定值22"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume22 As Integer

  <Translate("zh", "MT:主缸類比水位設定值23"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume23 As Integer

  <Translate("zh", "MT:主缸類比水位設定值24"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume24 As Integer

  <Translate("zh", "MT:主缸類比水位設定值25"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume25 As Integer

  <Translate("zh", "MT:主缸類比水位設定值26"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume26 As Integer

  <Translate("zh", "MT:主缸類比水位設定值27"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume27 As Integer

  <Translate("zh", "MT:主缸類比水位設定值28"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume28 As Integer

  <Translate("zh", "MT:主缸類比水位設定值29"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume29 As Integer

  <Translate("zh", "MT:主缸類比水位設定值30"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume30 As Integer

  <Translate("zh", "MT:主缸類比水位設定值31"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume31 As Integer

  <Translate("zh", "MT:主缸類比水位設定值32"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume32 As Integer

  <Translate("zh", "MT:主缸類比水位設定值33"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume33 As Integer

  <Translate("zh", "MT:主缸類比水位設定值34"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume34 As Integer

  <Translate("zh", "MT:主缸類比水位設定值35"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume35 As Integer

  <Translate("zh", "MT:主缸類比水位設定值36"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume36 As Integer

  <Translate("zh", "MT:主缸類比水位設定值37"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume37 As Integer

  <Translate("zh", "MT:主缸類比水位設定值38"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume38 As Integer

  <Translate("zh", "MT:主缸類比水位設定值39"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume39 As Integer

  <Translate("zh", "MT:主缸類比水位設定值40"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume40 As Integer

  <Translate("zh", "MT:主缸類比水位設定值41"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume41 As Integer

  <Translate("zh", "MT:主缸類比水位設定值42"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume42 As Integer

  <Translate("zh", "MT:主缸類比水位設定值43"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume43 As Integer

  <Translate("zh", "MT:主缸類比水位設定值44"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume44 As Integer

  <Translate("zh", "MT:主缸類比水位設定值45"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume45 As Integer

  <Translate("zh", "MT:主缸類比水位設定值46"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume46 As Integer

  <Translate("zh", "MT:主缸類比水位設定值47"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume47 As Integer

  <Translate("zh", "MT:主缸類比水位設定值48"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume48 As Integer

  <Translate("zh", "MT:主缸類比水位設定值49"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume49 As Integer

  <Translate("zh", "MT:主缸類比水位設定值50"), Category("Main Tank Calibration"), _
   TranslateCategory("zh", "主缸校正參數"), _
   Description("Set main tank volume by liters"), _
   TranslateDescription("zh", "設定主缸水位值，單位是公升")> Public SetMainTankVolume50 As Integer

  <Translate("zh", "MT:類比進水等待水位穩定時間"), Category("Main Tank"), _
   TranslateCategory("zh", "主缸參數")> Public AnalogFillWaitStableTime As Integer

  <Translate("zh", "MT:警報檢查時間"), Category("Main Tank"), _
       TranslateCategory("zh", "主缸參數")> Public 警報檢查時間 As Integer = 10

  '藥缸參數

  <Translate("zh", "ST:BC藥缸0雙馬達1單馬達"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定雙藥缸加藥馬達數量，設定0是雙馬達，設定1是單馬達")> Public BCTank0TwoPump1OnePump As Integer = 0

  <Translate("zh", "ST:Dosing種類0變頻1比例2氣動"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數")> Public DosingKind0Pump1AO2DO As Integer = 2

  <Translate("zh", "B缸加熱溫度(度)"), Category("藥缸參數")> Public SetBTankTemp As Integer = 60
  <Translate("zh", "C缸加熱溫度(度)"), Category("藥缸參數")> Public SetCTankTemp As Integer = 60

    <Translate("zh", "藥缸容量(公升)"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定藥缸的容量，提供流量進水預扣")> Public SetSideTankVolume As Integer = 100

    <Translate("zh", "ST:藥缸Dosing延遲時間"), Category("Side Tank"), _
TranslateCategory("zh", "藥缸參數"), _
Description("設定Dosing延遲時間，Dosing閥依照設定時間開關")> Public DosingDelayTime As Integer = 10


  <Translate("zh", "ST:藥缸加藥延遲時間"), Category("Side Tank"), _
  TranslateCategory("zh", "藥缸參數"), _
 Description("設定洗缸前加藥延遲時間")> Public AddTransferTimeBeforeRinse As Integer = 10

  <Translate("zh", "ST:藥缸循環前洗缸時間"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定循環前，洗缸進水的時間")> Public MixCirculateRinseTime As Integer = 5

  <Translate("zh", "ST:藥缸洗缸循環時間"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定洗缸完後，循環攪拌的時間")> Public AddCirculateTimeAfterRinse As Integer = 5

  <Translate("zh", "ST:藥缸洗缸加藥時間"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定洗缸完後，加藥的延遲時間")> Public AddTransferTimeAfterRinse As Integer = 10

  <Translate("zh", "ST:藥缸加藥洗缸時間"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定加藥完後，洗缸進水的時間")> Public AddTransferRinseTime As Integer = 5

  <Translate("zh", "ST:藥缸排水延遲時間"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定洗缸後排水延遲時間")> Public AddTransferDrainTime As Integer = 15

  <Translate("zh", "ST:藥缸備藥確認1否0是"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定藥缸備藥時是否要按備藥確認鈕，設定值是1時則不需按鈕確認")> Public SideTankPrepareConfirm As Integer = 0

  <Translate("zh", "ST:B缸攪拌類型1馬達0循環"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定B藥缸攪拌類型，1是使用馬達攪拌，0是循環攪拌")> Public BTankMixType As Integer = 0

  <Translate("zh", "ST:C缸攪拌類型1馬達0循環"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
  Description("設定C藥缸攪拌類型，1是使用馬達攪拌，0是循環攪拌")> Public CTankMixType As Integer = 0

    <Translate("zh", "ST:藥缸預先入水容量"), Category("Side Tank"), _
     TranslateCategory("zh", "藥缸參數"), _
     Description("設定備藥前，藥缸預先入水的容量")> Public SideTankPreFillVolume As Integer = 30

  <Translate("zh", "ST:藥缸稀釋加藥控制模式"), Category("Side Tank"), _
   TranslateCategory("zh", "藥缸參數"), _
   Description("稀釋加藥控制模式,0=氣動,1=比例")> Public DiluteAddControlType As Integer = 0

  <Translate("zh", "ST:C缸水位感應器種類"), Category("Side Tank"), _
 TranslateCategory("zh", "藥缸參數"), _
 Description("C缸水位感應器種類,0=數位輸入,1=類比輸入")> Public CTankLevelType As Integer = 1

  <Translate("zh", "ST:B缸高水位類比輸入值"), Category("Side Tank"), _
 TranslateCategory("zh", "藥缸參數"), _
 Description("設定B缸高水位時的類比輸入值，用以傳送到PLC做水位比較")> Public BTankHighLevelAI As Integer

  <Translate("zh", "ST:B缸中水位類比輸入值"), Category("Side Tank"), _
TranslateCategory("zh", "藥缸參數"), _
Description("設定B缸中水位時的類比輸入值，用以傳送到PLC做水位比較")> Public BTankMiddleLevelAI As Integer

  <Translate("zh", "ST:B缸低水位類比輸入值"), Category("Side Tank"), _
TranslateCategory("zh", "藥缸參數"), _
Description("設定B缸低水位時的類比輸入值，用以傳送到PLC做水位比較")> Public BTankLowLevelAI As Integer

  <Translate("zh", "ST:C缸高水位類比輸入值"), Category("Side Tank"), _
TranslateCategory("zh", "藥缸參數"), _
Description("設定C缸高水位時的類比輸入值，用以傳送到PLC做水位比較")> Public CTankHighLevelAI As Integer

  <Translate("zh", "ST:C缸中水位類比輸入值"), Category("Side Tank"), _
TranslateCategory("zh", "藥缸參數"), _
Description("設定C缸中水位時的類比輸入值，用以傳送到PLC做水位比較")> Public CTankMiddleLevelAI As Integer

  <Translate("zh", "ST:C缸低水位類比輸入值"), Category("Side Tank"), _
TranslateCategory("zh", "藥缸參數"), _
Description("設定C缸低水位時的類比輸入值，用以傳送到PLC做水位比較")> Public CTankLowLevelAI As Integer

  <Translate("zh", "ST:藥缸加藥確認"), Category("Side Tank"), _
TranslateCategory("zh", "藥缸參數"), _
Description("藥缸加藥時按確認才開始加藥，0=否 1=是")> Public AcknowledgeForAddition As Integer

  '自動計量系統

  <Translate("zh", "AD:連接助劑自動計量系統"), Category("Auto Dispenser"), _
   TranslateCategory("zh", "自動計量系統"), _
      Description(""), _
  TranslateDescription("zh", "是否連接助劑自動計量系統，0為否，1是連接到B缸，2是連接到C缸")> Public ChemicalEnable As Integer

  <Translate("zh", "AD:連接染料自動輸送系統"), Category("Auto Dispenser"), _
   TranslateCategory("zh", "自動計量系統"), _
      Description(""), _
  TranslateDescription("zh", "是否連接染料自動輸送系統，0為否，1是連接到B缸，2是連接到C缸")> Public DyeEnable As Integer

  <Translate("zh", "AD:呼叫LA-252加藥"), Category("Auto Dispenser"), _
TranslateCategory("zh", "自動計量系統"), _
Description(""), _
TranslateDescription("zh", "執行等待LA-252時，是否呼叫LA-252加藥")> Public CallFor252AddDye As Integer

  <Translate("zh", "AD:助劑輸送延遲時間"), Category("Auto Dispenser"), _
TranslateCategory("zh", "自動計量系統"), _
 Description(""), _
TranslateDescription("zh", "設定助劑輸送系統的延遲時間，當藥缸低水位到達時開始計算延遲時間，時間到即判定輸送完成")> Public ChemicalTransferDelayTime As Integer

  <Translate("zh", "AD:染料輸送延遲時間"), Category("Auto Dispenser"), _
TranslateCategory("zh", "自動計量系統"), _
  Description(""), _
TranslateDescription("zh", "設定染料輸送系統的延遲時間，當藥缸低水位到達時開始計算延遲時間，時間到即判定輸送完成")> Public DyeTransferDelayTime As Integer


  '其他參數

  <Translate("zh", "帶布輪速度換算係數"), Category("System"), _
 TranslateCategory("zh", "系統參數")> Public ReelSpeedConvert As Integer = 165

  <Translate("zh", "蒸氣流量計取樣時間,毫秒"), Category("System"),
 TranslateCategory("zh", "系統參數")> Public SteamFlowmeterSampleTime As Integer = 200


  <Translate("zh", "主馬達保養設定時數"), Category("Maintenance"), _
   TranslateCategory("zh", "維護保養參數"), _
  Description("當主馬達運行時數超過設定值，則顯示主馬達需保養的警報")> Public MainPumpMaintainTime As Integer = 5000

  <Translate("zh", "主馬達運轉時數"), Category("Maintenance"), _
   TranslateCategory("zh", "維護保養參數"), _
  Description("當主馬達實際運行時數，當此數值超過設定值，則顯示主馬達需保養的警報")> Public MainPumpRunTime As Integer

  <Translate("zh", "通訊埠"), Category("System"), _
   TranslateCategory("zh", "系統參數")> Public ComNumber As Integer = 1

  <Translate("zh", "通訊速度"), Category("System"), _
 TranslateCategory("zh", "系統參數")> Public ComBaudRate As Integer = 57600

  <Translate("zh", "允許LA-838本機載入染程"), Category("System"), _
TranslateCategory("zh", "系統參數"), _
 Description("設定是否允許操作員在本機載入染程來運行，0=不允許，1=允許")> Public EnableLoadProgramOnLocal As Integer = 0

  <Translate("zh", "連結LASPC數據庫重新連結"), Category("DispenseOverTime"), _
TranslateCategory("zh", "LASPC連結"), _
Description("如果沒有連線,請按1會重新連線.如果有連線後會自動歸0")> Public ConnectSPCTest As Integer

  <Translate("zh", "連結LASPC數據庫設置"), Category("DispenseOverTime"), _
TranslateCategory("zh", "LASPC連結"), _
Description("如果有使用SPC請按1")> Public ConnectSPCEnable As Integer

  'PH參數
  '<Translate("zh", "入酸量/分鐘"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  'Description("每分鐘加入酸量，多少C.C量，比方輸入60，代表一分鐘加入60CC量")> Public PhAddHacTime As Integer

  '  <Translate("zh", "HAC酸度%"), Category("PH Setup"), _
  '   TranslateCategory("zh", "PH參數"), _
  '  Description("酸的濃度，以%計算")> Public PhConcentration As Integer

  '  <Translate("zh", "PH調整時檢測時間(秒)"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("每段時間檢測PH值")> Public PhAdjustCheckTime As Integer

  '  <Translate("zh", "PH清洗時間(秒)"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("PH清洗管路時間")> Public PhWashTime As Integer

  '  <Translate("zh", "PH迴流桶排水延遲時間(秒)"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("迴流桶排水的延遲時間")> Public CirDrainDelayTime As Integer

  '  <Translate("zh", "PH迴流桶迴水延遲關閉時間(秒)"), Category("PH Setup"), _
  '   TranslateCategory("zh", "PH參數"), _
  '   Description("迴流桶迴水至低水位時，入迴水延遲關閉時間")> Public CirFillDelayTime As Integer

  '  <Translate("zh", "PH啟動冷卻系統溫度(度)"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("PH啟動冷卻系統溫度(度)")> Public PhCoolingTemp As Integer

  '  <Translate("zh", "PH取樣時間(秒)"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("PH取樣時間(最少60秒)")> Public PhSamplingTime As Integer

  '  <Translate("zh", "PH取樣後，延遲等待穩定值(0-30秒)"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("每PH取樣時間後，將等待設定秒確認PH值(最多30秒)")> Public DoublePhSample As Integer

  '  <Translate("zh", "PH偏差時警報(1:是,0:不)"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("PH偏差時警報(1:是,0:不)")> Public PhErrorAlarm As Integer

  '  <Translate("zh", "PH加酸動作異常次數"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("加酸時超過多少次為異常")> Public PhAddError As Integer

  '  <Translate("zh", "PH到達範圍"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("PH到達範圍,1= 0.01PH,10=0.1PH")> Public PhApproach As Integer

  '  <Translate("zh", "PH泵加酸比(1-10)"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("pH泵加酸比,1代表1分鐘=60C.C，10代表1分鐘=600C.C")> Public PhPumpOutRatio As Integer

  '  <Translate("zh", "主缸進水量(L)"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("主缸進水量(L)")> Public MainTankFillLevel As Integer

  '  <Translate("zh", "迴流時，加藥馬達%"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("迴流時，加藥馬達")> Public AddPercent As Integer

  '  <Translate("zh", "起始檢查PH時間"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("一開始時，檢查PH值")> Public StartCheckPh As Integer

  '  <Translate("zh", "PH控制模式"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("時間等分演算 = 0 , PH等分演算 = 1")> Public PhControlMode As Integer

  '  <Translate("zh", "迴水%,開大閥入染機"), Category("PH Setup"), _
  '  TranslateCategory("zh", "PH參數"), _
  '  Description("迴水超過多少%，開大閥加藥")> Public CirculateOpenAdd As Integer
  '******************************************************************************************
  '<Translate("zh", "PressureIn01"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn1 As Integer = 380

  '<Translate("zh", "PressureIn02"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn2 As Integer = 401

  '<Translate("zh", "PressureIn03"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn3 As Integer = 431

  '<Translate("zh", "PressureIn04"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn4 As Integer = 457

  '<Translate("zh", "PressureIn05"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn5 As Integer = 492

  '<Translate("zh", "PressureIn06"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn6 As Integer = 535

  '<Translate("zh", "PressureIn07"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn7 As Integer = 584

  '<Translate("zh", "PressureIn08"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn8 As Integer = 645

  '<Translate("zh", "PressureIn09"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn9 As Integer = 706

  '<Translate("zh", "PressureIn10"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn10 As Integer = 781

  '<Translate("zh", "PressureIn11"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn11 As Integer = 861

  '<Translate("zh", "PressureIn12"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn12 As Integer = 901

  '<Translate("zh", "PressureIn13"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn13 As Integer = 901
  '<Translate("zh", "PressureIn14"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn14 As Integer = 901
  '<Translate("zh", "PressureIn15"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn15 As Integer = 901
  '<Translate("zh", "PressureIn16"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn16 As Integer = 901
  ' <Translate("zh", "PressureIn17"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn17 As Integer = 901
  ' <Translate("zh", "PressureIn18"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn18 As Integer = 901
  ' <Translate("zh", "PressureIn19"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn19 As Integer = 901
  ' <Translate("zh", "PressureIn20"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn20 As Integer = 901
  ' <Translate("zh", "PressureIn21"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn21 As Integer = 901
  ' <Translate("zh", "PressureIn22"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn22 As Integer = 901
  '  <Translate("zh", "PressureIn23"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn23 As Integer = 901
  ' <Translate("zh", "PressureIn24"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn24 As Integer = 901
  ' <Translate("zh", "PressureIn25"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn25 As Integer = 901
  ' <Translate("zh", "PressureIn26"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn26 As Integer = 901
  ' <Translate("zh", "PressureIn27"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn27 As Integer = 901
  ' <Translate("zh", "PressureIn28"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn28 As Integer = 901
  ' <Translate("zh", "PressureIn29"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn29 As Integer = 901
  ' <Translate("zh", "PressureIn30"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn30 As Integer = 901
  ' <Translate("zh", "PressureIn31"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn31 As Integer = 901
  ' <Translate("zh", "PressureIn32"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn32 As Integer = 901
  ' <Translate("zh", "PressureIn33"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn33 As Integer = 901
  ' <Translate("zh", "PressureIn34"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn34 As Integer = 901
  '  <Translate("zh", "PressureIn35"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn35 As Integer = 901
  ' <Translate("zh", "PressureIn36"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn36 As Integer = 901
  '  <Translate("zh", "PressureIn37"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn37 As Integer = 901
  ' <Translate("zh", "PressureIn38"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn38 As Integer = 901
  ' <Translate("zh", "PressureIn39"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn39 As Integer = 901
  ' <Translate("zh", "PressureIn40"), Category("Pressure Setup"), _
  'TranslateCategory("zh", "Pressure")> Public PressureIn40 As Integer = 901

  ' <Translate("zh", "PH:是否已連接PH系統"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("是=1 否=0")> Public ConnectPhSystem As Integer
  '<Translate("zh", "PH:HAC酸度%"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("酸的濃度，以%計算")> Public PhConcentration As Integer = 100
  '<Translate("zh", "PH:是否有回流桶"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("是=1 否=0")> Public PhCirTank As Integer = 1
  '<Translate("zh", "PH:調整時檢測時間(秒)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("每段時間檢測PH值")> Public PhAdjustCheckTime As Integer = 20
  '<Translate("zh", "PH:清洗時間(秒)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("PH清洗管路時間")> Public PhWashTime As Integer = 20
  '<Translate("zh", "PH:迴流桶排水延遲時間(秒)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("迴流桶排水的延遲時間")> Public CirDrainDelayTime As Integer = 20
  '<Translate("zh", "PH:迴流桶迴水延遲關閉時間(秒)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("迴流桶迴水至低水位時，入迴水延遲關閉時間")> Public CirFillDelayTime As Integer = 10
  '<Translate("zh", "PH:啟動冷卻系統溫度(度)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("PH啟動冷卻系統溫度(度)")> Public PhCoolingTemp As Integer = 40
  '<Translate("zh", "PH:pH sensor安全溫度(度)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("超過sensor安全溫度，強制關閉動作)")> Public PH檢測筒溫度過高 As Integer = 100
  '<Translate("zh", "PH:加酸安全溫度度"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("當PH溫度高於設定值，則不允許加酸動作 60C~110C")> Public PH加酸安全溫度 As Integer = 85
  '<Translate("zh", "PH:取樣時間(秒)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("PH取樣時間(最少60秒)")> Public PhSamplingTime As Integer = 60
  '<Translate("zh", "PH:取樣後，延遲等待穩定值(0-60秒)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  'Description("每PH取樣時間後，將等待設定秒確認PH值，管路越遠時間要越長")> Public DoublePhSample As Integer = 30
  ' <Translate("zh", "PH:到達範圍"), Category("PH Setup"), 
  '  TranslateCategory("zh", "PH參數"), _
  'Description("PH到達範圍,1= 0.01PH,10=0.1PH")> Public PhApproach As Integer = 2
  '<Translate("zh", "PH:泵加酸比(60-600)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("pH泵加酸比,1代表1分鐘=1C.C")> Public PhPumpOutRatio As Integer = 300
  '<Translate("zh", "PH:初始主缸進水量(L)"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("主缸進水量(L)，如果染程沒有設定F73的設定水量，則採用預設參數水量")> Public MainTankFillLevel As Integer = 2000
  '<Translate("zh", "PH:起始檢查PH時間"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("一開始時，檢查PH值")> Public StartCheckPh As Integer = 10
  '<Translate("zh", "PH:控制模式"), Category("PH Setup"), _
  'TranslateCategory("zh", "PH參數"), _
  'Description("時間等分演算 = 0 , PH等分演算 = 1")> Public PhControlMode As Integer = 1
  '<Translate("zh", "PH:迴水%,開大閥入染機"), Category("PH Setup"), _
  'TranslateCategory("zh", "PH參數"), _
  'Description("迴水超過多少%，開大閥加藥")> Public CirculateOpenAdd As Integer = 10
  '<Translate("zh", "PH:加酸關閉,循環馬達延遲關閉時間"), Category("PH Setup"), _
  ' TranslateCategory("zh", "PH參數"), _
  ' Description("加酸閥關閉時,為了讓加酸確實入染機,延遲關閉循環馬達時間")> Public DelayCirculatPump As Integer = 4

  '<Translate("zh", "PH:是否啟用持續回流偵測"), Category("PH Setup"), _
  'TranslateCategory("zh", "PH參數"), _
  'Description("F73代表開始,F79洗管代表結束")> Public PhCirRuning As Integer = 0
  '<Translate("zh", "PH:偵測pH酸控長時間無反應提示"), Category("PH Setup"), _
  'TranslateCategory("zh", "PH參數"), _
  'Description("如果pH酸控，長時間無法反應時，將發出警報")> Public PhControlToLongTime As Integer = 600
  '<Translate("zh", "PH:pH回流加藥速度"), Category("PH Setup"), _
  'TranslateCategory("zh", "PH參數")> Public PhDosing As Integer = 600
  '<Translate("zh", "PH:pH自動清洗時間"), Category("PH Setup"), _
  'TranslateCategory("zh", "PH參數")> Public PhAutoWashTime As Integer = 10

  '******************************************************************************************


End Class


Partial Public Class ControlCode
  Public ReadOnly Parameters As New Parameters
End Class

