Public Class PhControl_

    Public Enum PhControl
        Off
        DownloadParameter
        Alarm_TmepHigh
        AlarmPhHigh
        Divider
        AddHacError
        MathAddHac
        MathAddHac1
        MathAddHac2
        MathAddHac3
        MathAddHac4
        AllAddHac
        CheckPhValue
        Pause
        WaitTempFinish
        Finished
        WaitTempArrival
        WaitKeepTime
    End Enum

    Public Wait As New Timer
    Public Wait1, WaitAllAdd3min As New Timer
    Public WaitPhSample As New Timer
    Public WaitCheckPH As New Timer
    Public Wait10Second As New Timer
    Public WaitAddHac As New Timer
    Public TmepTime As New Timer
    Public WaitCheckTime As New Timer
    Public C37 As Integer
    Public StateString As String
    Public CalculateTmep As Integer                     '溫度差
    Public TotalAddHac As Integer                        'HAC總量
    Public TotalAddHac2 As Integer                        'HAC總量
    Public TotalAddHac3 As Integer                        'HAC總量
    Public TotalAddHac4 As Integer                        'HAC總量
    Public C01TargetTemp As Integer                     '到達溫度
    Public C01Gradient As Integer                       '斜率
    Public C75Gradient As Integer                       'PH目標值
    Public C76AddTime As Integer                        '加的時間
    Public C76OpenTemp As Integer                       '起始溫度
    Public C77KeepTime As Integer
    Public PhConcentration As Double                  '濃度
    Public PhPumpOutRatio As Double                    'PH泵加酸比
    Public PhFillLevel As Double                        '進水量
    Public PhFillLevel2 As Integer                         '進水量
    Public PhRangeValue, PhRangeValue2, PhRangeValue3, PhRangeValue4, PhRangeValue5, PhRangeValue6, ExpectPh, ExpectPh2 As Integer
    Public PhSamplingTime As Integer                    'PH取樣時間
    Public FirstPhValue, FirstTemp As Integer                      '初始讀取PH實際值
    Public Register(10) As Integer                      '暫存器
    Public RegisterD(12) As Double                       '暫存器
    Public RegisterI(21) As Integer                     '暫存器
    Public PhAddError As Integer                        '
    Public CalculateTmepRange As Integer                '加酸錯誤演算溫度差的總數
    Public OpenHacValve As Boolean                      '開加藥閥
    Public CountHacVolume As Integer                    '計算HAC加的總量
    Public PauseCode As Integer                             '
    'Public X As Integer
    Private PerPhValue(1000) As Integer
    Private FreeTime(1000) As Integer
    Private TotalValue(1000) As Integer
    Private X, MathNeverOpenValue As Integer
    Public CheckPhRun, OpenKeepTime As Boolean



    Public Sub Run()
        With ControlCode
            Select Case State
                Case PhControl.Off
                    Wait.TimeRemaining = 0
                    WaitCheckTime.TimeRemaining = .Parameters.StartCheckPh
                    State = PhControl.DownloadParameter
                    '======================================================================================載入參數
                Case PhControl.DownloadParameter
                    CheckPhRun = False
                    StateString = If(.Language = LanguageValue.ZhTw, "確認目前PH值，檢查時間剩餘 " & WaitCheckTime.TimeRemaining & " 秒", "Check PH " & WaitCheckTime.TimeRemaining & " S")
                    '----------------------------------------------------------'等待PH確定值
                    If Not WaitCheckTime.Finished Then Exit Select
                    '-----------------------------------------------------------'等待藥桶低水位
                    StateString = If(.Language = LanguageValue.ZhTw, "等待B桶低水位", "Waiting for B tank low level")
                    If Not .IO.BTankLow Then Exit Select
                    '------------------------------------------------------------------
                    StateString = ""
                    For i = 0 To 10
                        Register(i) = 0
                    Next
                    For i = 0 To 12
                        RegisterD(i) = 0
                    Next
                    For i = 0 To 21
                        RegisterI(i) = 0
                    Next

                    For i = 0 To 1000
                        PerPhValue(i) = 0
                        FreeTime(i) = 0
                        TotalValue(i) = 0
                    Next

                    .UseHacAllTotal = 0
                    CountHacVolume = 0                                              '統計加HAC總量 歸零

                    PhAddError = 0

                    '---------------------------------------------------------------------------------------------------------------------------------------
                    If .Command75.State = Command75.S75.Start Then

                        If Not .Setpoint > 10 Then Exit Select

                        C01TargetTemp = .Command01.TargetTemp                                   '到達溫度
                        C01Gradient = .Command01.Gradient                                       '斜率
                        C75Gradient = .Command75.PhTarget                                       'PH目標值

                        If .IO.MainTemperature > C01TargetTemp Then
                            '------------------------------實際溫度 > 設定溫度
                            StateString = If(.Language = LanguageValue.ZhTw, "實際溫度高於設定溫度", "MainTemperature is high than TargetTemperature")
                            State = PhControl.DownloadParameter

                            Exit Select

                        Else
                            '------------------------------實際溫度 < 設定溫度
                            StateString = ""
                            If .IO.PhValue < C75Gradient Then
                                State = PhControl.AlarmPhHigh
                            Else

                                State = PhControl.Divider
                            End If

                        End If



                        '---------------------------------------------------------------------------------------------------------------------------------------
                    ElseIf .Command74.State = Command74.S74.Start Then

                        C01Gradient = 0

                        C75Gradient = .Command74.PhTarget                                       'PH目標值

                        State = PhControl.Divider
                        '---------------------------------------------------------------------------------------------------------------------------------------
                    ElseIf .Command76.State = Command76.S76.Start Then

                        C75Gradient = .Command76.PhTarget                                       'PH目標值
                        C76AddTime = .Command76.AddTime
                        C76OpenTemp = .Command76.PhOpenTemp * 10

                        State = PhControl.WaitTempArrival
                        '---------------------------------------------------------------------------------------------------------------------------------------
                    ElseIf .Command77.State = Command77.S77.Start Then

                        C75Gradient = .Command77.PhTarget                                       'PH目標值
                        OpenKeepTime = False

                        State = PhControl.WaitTempArrival

                        '---------------------------------------------------------------------------------------------------------------------------------------
                    Else

                        Exit Select
                        '---------------------------------------------------------------------------------------------------------------------------------------
                    End If

                    '====================================================================================
                Case PhControl.WaitTempArrival

                    If .IO.MainTemperature <= C76OpenTemp Then
                        StateString = If(.Language = LanguageValue.ZhTw, "等待溫度" & (C76OpenTemp / 10) & "度，才執行PH控制", "Wait for" & (C76OpenTemp / 10) & "C，then PH control")
                        State = PhControl.WaitTempArrival
                        Exit Select
                    Else
                        State = PhControl.Divider
                    End If



                    '====================================================================================超過偈實際值低於PH設定值會提示警告
                Case PhControl.AlarmPhHigh
                    StateString = If(.Language = LanguageValue.ZhTw, "PH實際值低於PH設定值", "PH Value Error")
                    If .IO.PhValue > (C75Gradient + .Parameters.PhApproach) Then
                        State = PhControl.Divider
                    End If
                    '====================================================================================區分"配合斜率"，計量加HAC  或  直加HAC
                Case PhControl.Divider

                    '-----------------------------------------------------------------------載入參數(濃度、PH泵加酸比、進水量)
                    PhConcentration = CType((.Parameters.PhConcentration / 100), Double)   '濃度
                    PhPumpOutRatio = CType((.Parameters.PhPumpOutRatio * 60) / 60, Double) 'PH泵加酸比

                    '---------------------------此部份如果 進水量 濃度 PH泵加酸比 不得為0,如果為0將修改為1
                    If .PhFillLevel = 0 Then

                        PhFillLevel = CType(.Parameters.MainTankFillLevel / 2000, Double)      '進水量
                    Else
                        PhFillLevel = CType(.PhFillLevel / 2000, Double)                       '進水量
                    End If

                    If PhPumpOutRatio = 0 Then
                        PhPumpOutRatio = 1
                    End If
                    If PhConcentration = 0 Then
                        PhConcentration = 1
                    End If

                    WaitAllAdd3min.TimeRemaining = 0
                    '-----------------------------------------------------------------------如果 實際PH值 已經低於 PH目標值 時，跳到PH確認部分 PhControl.CheckPhValue

                    If .IO.PhValue <= (C75Gradient + .Parameters.PhApproach) Then           'PH實際值 < = PH設定值

                        WaitCheckPH.TimeRemaining = .Parameters.PhSamplingTime              'PH取樣時間
                        State = PhControl.CheckPhValue
                        Exit Select
                    End If
                    '------------------------------------
                    StateString = ""

                    TotalAddHac = St.GetAmount(.IO.PhValue, C75Gradient)               '加酸所需要總量 =（ 現在PH值 , PH目標值 ）

                    FirstPhValue = .IO.PhValue

                    TotalAddHac2 = CType(((TotalAddHac * PhFillLevel) / PhPumpOutRatio) / PhConcentration, Integer)
                    '加酸總量 （加成後確認） =  ((( 實際PH - 設定PH ) ＊ 進水量比率 ) / PH泵加酸比率 / 濃度比率 )
                    If C75Gradient < 450 Then
                        TotalAddHac3 = St.GetAmount(450, C75Gradient)               '加酸所需要總量 =（ 現在PH值 , PH目標值 ）

                        TotalAddHac4 = CType(((TotalAddHac3 * PhFillLevel) / PhPumpOutRatio) / PhConcentration, Integer)
                    End If



                    Wait.TimeRemaining = TotalAddHac2


                    '------------------------------------------------------區分 (1)有斜率 就計量加藥 (2)沒有斜率 就直接加藥

                    If (.Command01.Gradient <> 0 And .Command01.IsOn = True) Then
                        '================================================================================================ 'PH控制模式 = 0
                        If .Parameters.PhControlMode = 0 Then
                            If (.Command75.State = Command75.S75.Start) Then


                                CalculateTmep = C01TargetTemp - .IO.MainTemperature
                                '演算溫度200  = 設定溫度1000  -  實際溫度800

                                RegisterD(0) = CType(CalculateTmep / C01Gradient, Double)
                                '升溫部分所需要的總時間 = ( 設定溫度-目前溫度 ) / 斜率

                                CalculateTmepRange = CType(RegisterD(0) * 60, Integer)

                                '---------------------------------------------------------------升溫所需要的時間   >   加酸的時間 （計量控制加HAC）
                                '比方 加酸總量是800  斜率總秒數 1000       
                                RegisterD(1) = CType(TotalAddHac2 / CalculateTmepRange, Double)             ' 比率 4/5 =  加酸總量是800 / 升溫部分所需要的總時間 1000

                                RegisterI(2) = CalculateTmepRange \ 10                                  '分段加藥次數 100 = 斜率總秒數 1000 / 10秒

                                RegisterI(3) = CType(RegisterD(1) * 10, Integer)                      '如果每10秒, 加酸佔的秒數 8秒 = 比率 4/5 * 10秒

                                RegisterI(4) = .Parameters.PhSamplingTime \ 10                                      '第 6 次 做取樣 = 取樣時間 60秒 / 10秒

                                RegisterI(5) = 1
                                '取樣倍數，當第6次加藥結束後，會做取樣動作，然後RegisterI(5) + 1，RegisterI(5) = 2，然後(RegisterI(4) * RegisterI(5))， 6 * 2 = 12
                                '第12次才在取樣。

                                RegisterI(7) = .IO.PhValue                                      '取得第一次的PH值  .IO.PhValue

                                FirstTemp = .Setpoint

                                RegisterI(10) = 0

                                RegisterI(14) = 1

                                State = PhControl.MathAddHac

                                '------------------------------------------------------------------------------------------------------------------------
                            ElseIf (.Command76.State = Command76.S76.Start) Then

                                RegisterD(0) = CType(C76AddTime, Double)
                                '升溫部分所需要的總時間 = ( 設定加酸的時間 ) 

                                RegisterI(21) = RegisterI(21) + (RegisterI(10) * 10)

                                CalculateTmepRange = CType(RegisterD(0) * 60, Integer)

                                CalculateTmepRange = CalculateTmepRange - (RegisterI(21))

                                '---------------------------------------------------------------升溫所需要的時間   >   加酸的時間 （計量控制加HAC）

                                '比方 加酸總量是800  斜率總秒數 1000       
                                RegisterD(1) = CType(TotalAddHac2 / CalculateTmepRange, Double)             ' 比率 4/5 =  加酸總量是800 / 升溫部分所需要的總時間 1000

                                RegisterI(2) = CalculateTmepRange \ 10                                  '分段加藥次數 100 = 斜率總秒數 1000 / 10秒

                                RegisterI(3) = CType(RegisterD(1) * 10, Integer)                      '如果每10秒, 加酸佔的秒數 8秒 = 比率 4/5 * 10秒

                                RegisterI(4) = .Parameters.PhSamplingTime \ 10                                      '第 6 次 做取樣 = 取樣時間 60秒 / 10秒

                                RegisterI(5) = 1
                                '取樣倍數，當第6次加藥結束後，會做取樣動作，然後RegisterI(5) + 1，RegisterI(5) = 2，然後(RegisterI(4) * RegisterI(5))， 6 * 2 = 12
                                '第12次才在取樣。

                                RegisterI(7) = .IO.PhValue                                      '取得第一次的PH值  .IO.PhValue

                                FirstTemp = .Setpoint

                                RegisterI(10) = 0

                                RegisterI(14) = 1

                                State = PhControl.MathAddHac
                            End If
                            '============================================================================================PH控制模式 = 1
                        ElseIf .Parameters.PhControlMode = 1 Then
                            CheckPhRun = False
                            '-----------------------------------------------------------------.Parameters.PhControlMode = 1---F01溫度控制,F75跟溫度跑PH
                            If (.Command75.State = Command75.S75.Start) Then
                                CalculateTmep = C01TargetTemp - .IO.MainTemperature
                                '演算溫度200  = 設定溫度1000  -  實際溫度800

                                RegisterD(0) = CType(CalculateTmep / C01Gradient, Double)
                                '升溫部分所需要的總時間 = ( 設定溫度-目前溫度 ) / 斜率
                            ElseIf (.Command76.State = Command76.S76.Start) Then
                                RegisterD(0) = C76AddTime

                            ElseIf (.Command77.State = Command77.S77.Start) Then

                                If C76AddTime = 0 Then
                                    State = PhControl.AllAddHac
                                    Exit Select
                                Else
                                    RegisterD(0) = C76AddTime
                                End If



                            End If


                            CalculateTmepRange = CType(RegisterD(0) * 60, Integer)
                            '升溫部分所需要的總時間 秒


                            '-------------------------------------------------------------------升溫部分所需要的總時間(秒)  <   加酸總量 
                            If CalculateTmepRange < TotalAddHac2 Then

                                Register(3) = TotalAddHac2 - .Parameters.PhSamplingTime
                                WaitAllAdd3min.TimeRemaining = 60
                                State = PhControl.AllAddHac

                                Exit Select
                            End If
                            '-------------------------------------------------------------------
                            Dim Y, Y2, Z As Integer



                            X = FirstPhValue - C75Gradient

                            Z = FirstPhValue

                            For i = 1 To X

                                Y = St.GetAmount(Z, Z - 1)

                                Y2 = CType(((Y * PhFillLevel) / PhPumpOutRatio) / PhConcentration, Integer)

                                PerPhValue(i) = Y2

                                Z = Z - 1

                            Next
                            '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                            Dim G1, G2, G3, G4, G5, G6, G7, G8 As Integer
                            Dim X2 As Integer

                            G1 = St.GetAmount(FirstPhValue, C75Gradient)

                            G2 = CType(((G1 * PhFillLevel) / PhPumpOutRatio) / PhConcentration, Integer)
                            '加酸總量
                            '%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

                            For i = 1 To X

                                G3 = G3 + PerPhValue(i)
                                'G3 = 加酸總量
                            Next

                            G4 = CalculateTmepRange - G3
                            '空餘秒數 = 升溫部分所需要的總時間 秒 - 加酸總量
                            G7 = CalculateTmepRange

                            X2 = X

123:
                            G5 = CType(G7 / X2, Integer)

                            For i = 1 To X2

                                FreeTime(i) = G5 - PerPhValue(i)

                                If FreeTime(i) < 0 Then
                                    FreeTime(i) = 0
                                End If

                                G6 = G6 + FreeTime(i)
                            Next

                            If G6 > G4 Then

                                X2 = X2 - 1

                                G7 = G7 - PerPhValue(X)

                                G6 = 0

                                GoTo 123

                            End If

                            For i = 1 To X

                                TotalValue(i) = PerPhValue(i) + FreeTime(i)
                                G8 = G8 + TotalValue(i)
                            Next

                            RegisterI(1) = 1        '每次減依次PH值

                            Wait1.TimeRemaining = G8

                            MathNeverOpenValue = 0

                            State = PhControl.MathAddHac2



                        End If
                        '-------------------------------------------------------------------------------------------------------------------------
                    Else



                        Register(3) = TotalAddHac2 - .Parameters.PhSamplingTime


                        State = PhControl.AllAddHac
                    End If

                    '************************************************************************************************************************計量加HAC 2
                Case PhControl.MathAddHac2


                    ExpectPh = FirstPhValue - RegisterI(1)
                    'PH設定值 = 最初讀取PH值 - 每次減依次PH值
                    '6.99     =     7.00     -  0.01

                    CheckPhRun = True

                    If RegisterI(1) >= X Then
                        State = PhControl.CheckPhValue
                        Exit Select
                    End If

                    WaitAddHac.TimeRemaining = PerPhValue(RegisterI(1))
                    '每階段加酸時間      =   每階段加酸量(每次減依次PH值)

                    OpenHacValve = True
                    '開啟加酸閥

                    Wait10Second.TimeRemaining = PerPhValue(RegisterI(1)) + FreeTime(RegisterI(1))
                    '每階段PH設定值時間       =   每階段加酸量(每次減依次PH值) +  空於時間(每次減依次PH值)

                    '-----------此部分是如果三次都沒開閥,則開一秒鐘的閥---------------------------------
                    If WaitAddHac.TimeRemaining = 0 Then
                        MathNeverOpenValue += 1
                        If MathNeverOpenValue > 3 Then
                            MathNeverOpenValue = 0
                            WaitAddHac.TimeRemaining = 1
                        End If
                    End If

                    State = PhControl.MathAddHac3

                    Exit Select
                    '==============================================================================================================詳細計量加HAC 2
                Case PhControl.MathAddHac3

                    If Not WaitAddHac.Finished Then
                        If .IO.PhValue < ExpectPh - .Parameters.PhApproach Or .IO.PhValue < C75Gradient Then
                            OpenHacValve = False
                        Else
                            OpenHacValve = True
                        End If
                    End If


                    State = PhControl.MathAddHac4
                    Exit Select
                    '==============================================================================================================詳細計量加HAC 2
                Case PhControl.MathAddHac4

                    If WaitAddHac.Finished Then                         '開閥 HAC 比方說7秒

                        OpenHacValve = False                            '關閉 HAC

                        If Wait10Second.Finished Then                   '等待10秒時間

                            State = PhControl.MathAddHac2

                            RegisterI(1) = RegisterI(1) + 1

                            Exit Select

                        End If
                        State = PhControl.MathAddHac3
                        Exit Select
                    End If
                    State = PhControl.MathAddHac3
                    Exit Select
                    '==============================================================================================================詳細計量加HAC
                Case PhControl.MathAddHac1

                    '--------------------------------------------------------------------------升溫時，系統暫停-----------------------------
                    If .Parent.IsPaused Or .Command01.State = Command01.S01.Pause Or Not .IO.MainPumpFB Then

                        State = PhControl.Pause
                        Exit Select
                    End If
                    '------------------------------------------------------------------------------------------------------------------------

                    If WaitAddHac.Finished Then                         '開閥 HAC 比方說7秒

                        OpenHacValve = False                            '關閉 HAC



                        If Wait10Second.Finished Then                   '等待10秒時間

                            State = PhControl.MathAddHac

                        End If

                    End If

                    '************************************************************************************************************************計量加HAC
                Case PhControl.MathAddHac

                    '------------------------------------------
                    RegisterD(0) = RegisterD(0)       ' 升溫部分所需要的總時間(分)  = ( 設定溫度-目前溫度 ) / 斜率
                    RegisterD(1) = RegisterD(1)       '比率 4/5 =  加酸總量是800 / 升溫部分所需要的總時間（秒） 1000
                    RegisterD(2) = RegisterD(2)       '預設設定溫度
                    RegisterI(2) = RegisterI(2)       '分段加藥次數 100 = 斜率總秒數 1000 / 10秒
                    RegisterI(3) = RegisterI(3)       '如果每10秒, 加酸佔的秒數 8秒 = 比率 4/5 * 10秒
                    RegisterI(4) = RegisterI(4)       '第 6 次 做取樣 = 取樣時間 60秒 / 10秒
                    RegisterI(5) = RegisterI(5)       ''取樣倍數，當第6次加藥結束後，會做取樣動作，然後RegisterI(5) + 1，RegisterI(5) = 2，
                    RegisterI(6) = RegisterI(6)       '此次 60秒共花了多少HAC (48秒)  = 次數(6)  * 如果每10秒, 加酸佔的秒數(8)
                    RegisterI(7) = RegisterI(7)       '取得第一次的PH值  .IO.PhValue
                    RegisterI(8) = RegisterI(8)       '此次 60秒共花了多少HAC (48秒)  = 次數(6)  * 如果每10秒, 加酸佔的秒數(8)
                    RegisterI(9) = RegisterI(9)       'RegisterI(9) 所加的量 = RegisterI(8)是目前的PH量 + RegisterI(6)已經加HAC量
                    RegisterI(10) = RegisterI(10)     '
                    RegisterI(11) = RegisterI(11)     '
                    RegisterI(12) = RegisterI(12)     '
                    '------------------------------------------

                    PauseCode = 1

                    If .IO.PhValue > (C75Gradient + .Parameters.PhApproach) Then                   ''PH實際值 < = PH設定值  
                        '--------------------------------------------------------------------------升溫時，系統暫停-----------------------------
                        If .Parent.IsPaused Or .Command01.State = Command01.S01.Pause Or Not .IO.MainPumpFB Then

                            State = PhControl.Pause
                            Exit Select
                        End If
                        '------------------------------------------------------------------------------------------------------------
                        If .Command75.State = Command75.S75.Start Then
                            '--------------------------------------------------------------------持溫時將直接加酸
                            If .Command01.State = Command01.S01.Hold Then

                                State = PhControl.AllAddHac

                                Exit Select
                            End If
                            '----------------------------------------------------------------------溫度比較
                            If RegisterI(10) = 6 * RegisterI(14) Then


                                RegisterI(15) = FirstTemp + (C01Gradient * RegisterI(14))


                                RegisterI(17) = FirstTemp + (C01Gradient * (RegisterI(14)) + 30)


                                If .IO.MainTemperature < RegisterI(15) Then         '主溫度小於設定溫度,暫停加HAC
                                    RegisterI(16) = RegisterI(16) + 1
                                    Exit Select
                                ElseIf .IO.MainTemperature > RegisterI(17) Then     '主溫度大於設定溫度,從新計算

                                    State = PhControl.Divider
                                    Exit Select
                                Else
                                    RegisterI(14) = RegisterI(14) + 1
                                End If

                            End If
                        End If

                        '---------------------------------------------------------------------------------------------------------

                        '---------------------------------------------------升溫所需要的時間 > 加酸的時間(計量加藥) or 升溫所需要的時間 < 加酸的時間(直接全數加酸)
                        If CalculateTmepRange > TotalAddHac2 Then
                            '-----------------------------------------------升溫所需要的時間   >   加酸的時間  (計量加藥)


                            RegisterI(2) = RegisterI(2) - 1                         '每次加完一段  則 -1次

                            RegisterI(10) = RegisterI(10) + 1

                            RegisterD(2) = CType((FirstTemp + (C01Gradient / 6) * RegisterI(10)) - 5, Integer)

                            '---------------------------------------------------------------次數低於1以下,則全開加藥
                            If RegisterI(2) <= 1 Then
                                Register(3) = TotalAddHac2 - .Parameters.PhSamplingTime
                                State = PhControl.AllAddHac

                            Else
                                '-----------------------------------------------------------次數高於1以上,則計量加藥
                                Wait10Second.TimeRemaining = 10

                                WaitAddHac.TimeRemaining = RegisterI(3)

                                RegisterI(6) = (RegisterI(10) - 1) * RegisterI(3)
                                '此次 60秒共花了多少HAC (48秒)  = 次數(6)  * 如果每10秒, 加酸佔的秒數(8)

                                RegisterI(12) = CType(((RegisterI(6) * PhConcentration) * PhPumpOutRatio) / PhFillLevel, Integer)


                                RegisterI(13) = CType(((.Parameters.DoublePhSample * PhConcentration) * PhPumpOutRatio) / PhFillLevel, Integer)
                                '再次延遲PH等待值(加權)

                                RegisterI(8) = St.GetAmount(1000, RegisterI(7))
                                ''RegisterI(8)是目前的PH量 = ( 1000 ,  RegisterI(7)上次讀取PH值 )

                                RegisterI(9) = RegisterI(8) + RegisterI(12)
                                ''RegisterI(9) 所加的量 = RegisterI(8)是目前的PH量 + RegisterI(6)已經加HAC量

                                RegisterI(11) = RegisterI(9) - RegisterI(13)
                                ''  預定PH值 =   RegisterI(9) 所加的量 - 穩定PH延遲時間

                                ExpectPh2 = St.GetAmount1(RegisterI(11))
                                '此部分是所加的酸，目前應該到達的PH值

                                If ExpectPh2 >= FirstPhValue Then

                                    ExpectPh2 = FirstPhValue


                                End If




                                '-----------------------------------------------------------PH檢測動作
                                If RegisterI(10) = ((RegisterI(4) * RegisterI(5)) + 1) Then
                                    ' 如果 第12次 =  固定每6次取樣  *  2次

                                    RegisterI(5) = RegisterI(5) + 1
                                    '每次執行自動 + 1


                                    ExpectPh = St.GetAmount1(RegisterI(11))

                                    If ExpectPh >= FirstPhValue Then
                                        ExpectPh = FirstPhValue
                                    End If
                                    '此部分是所加的酸，目前應該到達的PH值


                                    If .IO.PhValue > ExpectPh Then


                                        PhAddError = PhAddError + 1


                                        If .Parameters.PhAddError <= PhAddError Then


                                            State = PhControl.AddHacError


                                        Else


                                            RegisterI(18) = St.GetAmount(.IO.PhValue, ExpectPh)     '算需要設定PH值，尚未到達PH差值。

                                            RegisterD(11) = RegisterI(18) / RegisterI(4)

                                            RegisterD(12) = RegisterI(4)


                                        End If



                                    Else
                                        PhAddError = 0

                                    End If
                                Else


                                End If
                                If RegisterD(12) > 0 Then
                                    RegisterD(12) = RegisterD(12) - 1
                                    WaitAddHac.TimeRemaining = CType(WaitAddHac.TimeRemaining + RegisterD(11), Integer)
                                    If WaitAddHac.TimeRemaining > 10 Then
                                        WaitAddHac.TimeRemaining = 10
                                    End If
                                End If


                                '-----------------------------------------------------------
                                If .IO.PhValue < ExpectPh2 - .Parameters.PhApproach Then        'PH實際值 7.2 < 7.5（ PH設定值 - PH範圍值 )


                                    OpenHacValve = False

                                Else

                                    OpenHacValve = True

                                End If



                                CountHacVolume = CountHacVolume + WaitAddHac.TimeRemaining

                                State = PhControl.MathAddHac1


                            End If
                        Else
                            '-----------------------------------------------升溫所需要的時間   <   加酸的時間  (直接全數加酸)
                            Register(3) = TotalAddHac2 - .Parameters.PhSamplingTime
                            State = PhControl.AllAddHac

                        End If

                    Else
                        State = PhControl.CheckPhValue
                        Wait.TimeRemaining = 0
                        WaitPhSample.TimeRemaining = 0
                        WaitCheckPH.TimeRemaining = .Parameters.PhSamplingTime

                    End If
                    '===================================================================================================================================直加HAC
                Case PhControl.AllAddHac

                    If WaitAllAdd3min.Finished Then
                        State = PhControl.Divider
                        Exit Select
                    End If

                    PauseCode = 2

                    If .IO.PhValue > (C75Gradient + .Parameters.PhApproach) Then                   ''PH實際值 < = PH設定值
                        '--------------------------------------------------------------------------升溫時，系統暫停-----------------------------
                        If .Parent.IsPaused Or .Command01.State = Command01.S01.Pause Or Not .IO.MainPumpFB Then
                            Wait.Pause()
                            State = PhControl.Pause
                            Exit Select
                        End If
                        '------------------------------------------------------------------------------------------------------------------------
                        StateString = If(.Language = LanguageValue.ZhTw, "加酸剩餘", "ADD HAC") & " " & Wait.TimeRemaining & " C.C "

                        '----------------------------------------------------此部分，是在運算加酸後，大概PH值會落在多少值，預估PH直


                        PhRangeValue = TotalAddHac2 - Wait.TimeRemaining


                        PhRangeValue2 = CType(((PhRangeValue * PhConcentration) * PhPumpOutRatio) / PhFillLevel, Integer)
                        '預設值使用量  =        目前所加酸的量    濃度               PH泵加酸比         水量

                        PhRangeValue3 = St.GetAmount(1000, FirstPhValue)

                        PhRangeValue4 = PhRangeValue3 + PhRangeValue2
                        '預計猜測到達PH值  = 基準點PH值 +  加酸的量

                        PhRangeValue5 = PhRangeValue4 - .Parameters.DoublePhSample
                        '預計確定到達PH直  = 預計猜測到達PH值 - 延遲等待PH穩定值

                        ExpectPh = St.GetAmount1(PhRangeValue5)
                        '此部分是所加的酸，目前應該到達的PH值


                        '--------------------------------------------------------------------------

                        If Wait.TimeRemaining = Register(3) Then


                            Register(3) = Register(3) - .Parameters.PhSamplingTime


                            If .IO.PhValue < ExpectPh And FirstPhValue > .IO.PhValue Then
                                PhAddError = 0


                            Else
                                If .Parameters.PhAddError = PhAddError Then


                                    State = PhControl.AddHacError


                                Else
                                    PhAddError = PhAddError + 1

                                    State = PhControl.Divider
                                End If


                            End If

                        End If


                        If Not Wait.Finished Then Exit Select
                        State = PhControl.CheckPhValue
                        Wait.TimeRemaining = 0
                        WaitPhSample.TimeRemaining = 0
                        WaitCheckPH.TimeRemaining = .Parameters.PhSamplingTime


                    Else
                        State = PhControl.CheckPhValue
                        Wait.TimeRemaining = 0
                        WaitPhSample.TimeRemaining = 0
                        WaitCheckPH.TimeRemaining = .Parameters.PhSamplingTime

                    End If

                    '==================================================================================================================================檢測PH值
                Case PhControl.CheckPhValue

                    PauseCode = 0

                    StateString = If(.Language = LanguageValue.ZhTw, "檢測PH值 ,等待", "ChECK PH VALVE") & WaitCheckPH.TimeRemaining & "秒"
                    If Not WaitCheckPH.Finished Then Exit Select
                    If .IO.PhValue <= (C75Gradient + .Parameters.PhApproach) Then
                        StateString = If(.Language = LanguageValue.ZhTw, "PH值控制 - 目前 ", "PH CONTROL  ") & (.IO.PhValue / 100) & " PH "
                        WaitCheckPH.TimeRemaining = .Parameters.PhSamplingTime
                        State = PhControl.WaitTempFinish

                    Else
                        State = PhControl.Divider

                    End If
                    '=====================================================================================================================================
                Case PhControl.WaitTempFinish
                    If .Command01.State <> Command01.S01.Off Then

                        StateString = If(.Language = LanguageValue.ZhTw, "PH值控制 - 目前 ", "PH CONTROL  ") & (.IO.PhValue / 100) & " PH "

                        If Not WaitCheckPH.Finished Then Exit Select

                        If .IO.PhValue <= (C75Gradient + .Parameters.PhApproach) Then


                            State = PhControl.WaitTempFinish

                        Else
                            State = PhControl.Divider

                        End If
                    ElseIf .Command77.State = Command77.S77.Start Or .Command77.State = Command77.S77.KeepTime Then
                        OpenKeepTime = True
                        State = PhControl.Divider
                        'ElseIf .Command01.State = Command01.S01.Off Then
                        '    State = PhControl.Finished
                    End If

                    '=====================================================================================================================================
                Case PhControl.Finished

                    '=====================================================================================================================================
                Case PhControl.AddHacError
                    StateString = If(.Language = LanguageValue.ZhTw, "加酸動作異常，請檢查設備！！", "ADD HAC ERROR")
                    If .IO.PhCallAck = True Then
                        State = PhControl.Divider
                    End If

                    '=====================================================================================================================================
                Case PhControl.Pause

                    StateString = If(.Language = LanguageValue.ZhTw, "PH加酸控制暫停", "Paused") & " " & TimerString(Wait.TimeRemaining)
                    'no longer pause restart the timer and go back to the wait state
                    If (Not .Parent.IsPaused) And .IO.MainPumpFB Then



                        If PauseCode = 1 Then
                            State = PhControl.Divider

                        ElseIf PauseCode = 2 Then
                            Wait.Restart()
                            State = PhControl.AllAddHac

                        End If
                    End If
                    '=====================================================================================================================================



            End Select


        End With

    End Sub
    Public Sub Cancel()
        State = PhControl.Off

    End Sub
#Region "Standard Definitions"

    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    Friend ReadOnly Property IsOn() As Boolean
        Get
            Return State <> PhControl.Off
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As PhControl
    Public Property State() As PhControl
        Get
            Return state_
        End Get
        Private Set(ByVal value As PhControl)
            state_ = value
        End Set
    End Property

    Public ReadOnly Property PhInToMachine() As Boolean     'PH入染機閥
        Get
            Return (State = PhControl.AllAddHac) Or (State = PhControl.MathAddHac1 And OpenHacValve = True) Or ((State = PhControl.MathAddHac3 Or State = PhControl.MathAddHac4) And OpenHacValve = True)
        End Get
    End Property

    Public ReadOnly Property PhAddHacOut() As Boolean       '加酸閥
        Get
            Return (State = PhControl.AllAddHac) Or (State = PhControl.MathAddHac1 And OpenHacValve = True) Or ((State = PhControl.MathAddHac3 Or State = PhControl.MathAddHac4) And OpenHacValve = True)
        End Get
    End Property

    Public ReadOnly Property PhAddPump() As Boolean     '加酸定量馬達
        Get
            Return (State = PhControl.AllAddHac) Or (State = PhControl.MathAddHac1 And OpenHacValve = True) Or ((State = PhControl.MathAddHac3 Or State = PhControl.MathAddHac4) And OpenHacValve = True)
        End Get
    End Property

#End Region
End Class

Partial Class ControlCode
    Public ReadOnly PhControl As New PhControl_(Me)
End Class
