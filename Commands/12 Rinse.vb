<Command("Rinse", "Type |0-3| Time |1-99|", , "60", "'2"),
 TranslateCommand("zh-TW", "溢流水洗", "水源選擇 |0-3| 水洗時間 |0-99|"),
 Description("0=COLD 1=HOT 2=COLD+HOT 3=A Tank   99=MAX 1=MIN"),
 TranslateDescription("zh-TW", "0=冷水 1=熱水 2=冷+熱水 3=降溫水,99=最高,0=最小")>
Public NotInheritable Class Command12
    Inherits MarshalByRefObject
    Implements ACCommand

    Public Enum S12
        Off
        WaitAuto
        WaitTempSafe
        WaitMiddleLevel
        WaitTime3
        WaitMiddleLevel2
        WaitMiddleLevel3
        WaitTime4
        WaitMainPumpFB
        WaitHighLevel
        DrainToMiddleLevel
        WaitTime5
        Pause
    End Enum

    Public Wait As New Timer
    Public RinseTime As Integer
    Public WaterType As Integer
    Public CoolFill As Boolean
    Public StateString As String

    Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
        With ControlCode
            'cancels for all other forground functions
            .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
            .Command05.Cancel() : .Command11.Cancel() : .Command13.Cancel() : .Command14.Cancel()
            .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
            .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel() : .Command54.Cancel()
            .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel() : .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command45.Cancel() : .Command46.Cancel()
      .Command24.Cancel()


      .TempControlFlag = False
            WaterType = MinMax(param(1), 0, 3)
            RinseTime = param(2)
            State = S12.WaitAuto
            If WaterType = 3 AndAlso .Parameters.CoolFillYes0No1 = 1 Then
                CoolFill = True
            Else
                CoolFill = False
            End If
        End With
    End Function

    Public Function Run() As Boolean Implements ACCommand.Run
        With ControlCode
            Select Case State

                Case S12.Off
                    StateString = ""

                Case S12.WaitAuto
                    StateString = If(.Language = LanguageValue.ZhTw, "系統手動中", "System Manual")
                    If Not .IO.SystemAuto Then Exit Select
                    State = S12.WaitTempSafe

                Case S12.WaitTempSafe
                    StateString = If(.Language = LanguageValue.ZhTw, "溫度異常", "Interlocked Temperature")
                    If .IO.MainTemperature >= .Parameters.SetSafetyTemp * 10 Then
                        .Alarms.HighTempNoFill = True
                        Exit Select
                    End If

                    .Alarms.HighTempNoFill = False
                    'If .WaterType = 0 Then
                    ' .IO.HotFill = False
                    ' .IO.ColdFill = True
                    ' If .Parameters.CoolFillYes0No1 > 0 Then
                    ' .IO.Cool = False
                    ' Else
                    ' .IO.Cool = True
                    ' End If
                    ' ElseIf .WaterType = 1 Then
                    ' .IO.ColdFill = False
                    ' .IO.HotFill = True
                    ' .IO.Cool = False
                    ' Else
                    ' .IO.ColdFill = True
                    '.IO.HotFill = True
                    '.IO.Cool = False
                    'End If
                    State = S12.WaitMiddleLevel

                Case S12.WaitMiddleLevel
                    StateString = If(.Language = LanguageValue.ZhTw, "主缸進水至中水位", "Filling to middle level ")
                    If Not .MiddleLevel Then Exit Select
                    Wait.TimeRemaining = 2
                    State = S12.WaitTime3

                Case S12.WaitTime3
                    If Not Wait.Finished Then Exit Select
                    State = S12.WaitMiddleLevel2

                Case S12.WaitMiddleLevel2
                    StateString = If(.Language = LanguageValue.ZhTw, "主缸進水至中水位", "Filling to middle level ")
                    If Not .MiddleLevel Then Exit Select
                    State = S12.WaitMiddleLevel3

                Case S12.WaitMiddleLevel3
                    StateString = If(.Language = LanguageValue.ZhTw, "主缸進水至中水位", "Filling to middle level ")
          'If Not .LowLevel Then Exit Select
                    '.IO.PumpSpeedControl = CType(.PumpSpeed * 10, Short)
                    .PumpStartRequest = True
                    Wait.TimeRemaining = 1
                    State = S12.WaitTime4

                Case S12.WaitTime4
                    If Not Wait.Finished Then Exit Select
                    .PumpStartRequest = False
                    .PumpOn = True
                    State = S12.WaitMainPumpFB

                Case S12.WaitMainPumpFB
                    StateString = If(.Language = LanguageValue.ZhTw, "主泵沒有運行", "Main pump not running")
                    If Not .IO.MainPumpFB Then Exit Select
                    State = S12.WaitHighLevel

                Case S12.WaitHighLevel
                    StateString = If(.Language = LanguageValue.ZhTw, "主缸進水至溢流水位", "Filling to overflow level ")
                    If Not .OverflowLevel Then Exit Select
                    ' .OverRinse.Start()
                    Wait.TimeRemaining = 60 * RinseTime
                    State = S12.DrainToMiddleLevel

                Case S12.DrainToMiddleLevel
                    If .Parent.IsPaused Or Not .IO.MainPumpFB Then
                        StateWas = State
                        State = S12.Pause
                        Wait.Pause()
                    End If
                    StateString = If(.Language = LanguageValue.ZhTw, "主缸排水至高水位", "Draining to high level ") & TimerString(Wait.TimeRemaining)
                    If Wait.Finished Then
                        State = S12.Off
                    End If
                    If Not .HighLevel Then State = S12.WaitTime5

                Case S12.WaitTime5
                    If .Parent.IsPaused Or Not .IO.MainPumpFB Then
                        StateWas = State
                        State = S12.Pause
                        Wait.Pause()
                    End If
                    StateString = If(.Language = LanguageValue.ZhTw, "主缸溢流水洗中", "Rinsing") & TimerString(Wait.TimeRemaining)
                    If .OverflowLevel Then State = S12.DrainToMiddleLevel

                    If Wait.Finished Then
                        State = S12.Off
                    End If

                Case S12.Pause
                    StateString = If(.Language = LanguageValue.ZhTw, "暫停", "Paused") & " " & TimerString(Wait.TimeRemaining)
                    If Not .Parent.IsPaused And .IO.MainPumpFB Then
                        State = StateWas
                        StateWas = S12.Off
                        Wait.Restart()
                    End If


            End Select
        End With
    End Function

    Public Sub Cancel() Implements ACCommand.Cancel
        State = S12.Off
        CoolFill = False
        Wait.Cancel()
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
            Return State <> S12.Off
        End Get
    End Property

    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S12
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S12

    Public Property State() As S12
        Get
            Return state_
        End Get
        Private Set(ByVal value As S12)
            state_ = value
        End Set
    End Property
    Public Property StateWas() As S12
        Get
            Return statewas_
        End Get
        Private Set(ByVal value As S12)
            statewas_ = value
        End Set
    End Property
    Public ReadOnly Property IsFillHot() As Boolean
        Get
            Return (WaterType = 1 Or WaterType = 2) AndAlso ((State = S12.WaitMiddleLevel) Or (State = S12.WaitTime3) Or (State = S12.WaitMiddleLevel2) Or _
                    (State = S12.WaitMiddleLevel3) Or (State = S12.WaitTime4) Or (State = S12.WaitHighLevel) Or (State = S12.WaitTime5))
        End Get
    End Property
    Public ReadOnly Property IsFillCold() As Boolean
        Get
            Return (WaterType = 0 Or WaterType = 2) AndAlso ((State = S12.WaitMiddleLevel) Or (State = S12.WaitTime3) Or (State = S12.WaitMiddleLevel2) Or _
                    (State = S12.WaitMiddleLevel3) Or (State = S12.WaitTime4) Or (State = S12.WaitHighLevel) Or (State = S12.WaitTime5))
        End Get
    End Property
    Public ReadOnly Property IsCoolFill() As Boolean
        Get
            Return CoolFill AndAlso ((State = S12.WaitMiddleLevel) Or (State = S12.WaitTime3) Or (State = S12.WaitMiddleLevel2) Or _
                    (State = S12.WaitMiddleLevel3) Or (State = S12.WaitTime4) Or (State = S12.WaitHighLevel) Or (State = S12.WaitTime5))
        End Get
    End Property
    Public ReadOnly Property IsRinsing() As Boolean
        Get
            Return (State = S12.DrainToMiddleLevel) Or (State = S12.WaitTime5)
        End Get
    End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command12 As New Command12(Me)
End Class
#End Region
