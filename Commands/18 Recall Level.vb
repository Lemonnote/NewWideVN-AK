<Command("RecallLevel", "StableTime|1-60|", , , ),
 TranslateCommand("zh-TW", "計錄水位", "穩定時間|1-60|"),
 Description("Recall the Main Tank Water Level. Wait stable time=1-60 seconds"),
 TranslateDescription("zh-TW", "計錄當前主缸的水位, 等待穩定時間=1-60秒")>
Public NotInheritable Class Command18
    Inherits MarshalByRefObject
    Implements ACCommand

    Public Enum S18
        Off
        WaitStopCirPump
        WaitTimer
        RecollectLevel
        WaitStartPump
        WaitPumpFB
        WaitFinish
    End Enum
    Public StateString As String
    Public Wait, WaitStable As New Timer
    Public WaitStableTime As Integer

    Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
        With ControlCode
            'ToDo 執行此命令時要停止其他命令
            .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel() : .Command05.Cancel()
            .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel() : .Command14.Cancel()
            .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
            .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel() : .Command56.Cancel()
            .Command57.Cancel() : .TemperatureControl.Cancel() : .Command01.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command24.Cancel()

      .TempControlFlag = False
            WaitStableTime = param(1)
            .PumpStopRequest = True                                   'ControlCode.PumpStopRequest = 1
            Wait.TimeRemaining = 1
            State = S18.WaitStopCirPump
        End With
    End Function

    Public Function Run() As Boolean Implements ACCommand.Run
        With ControlCode
            Select Case State
                Case S18.Off
                    StateString = ""

                Case S18.WaitStopCirPump
                    StateString = If(.Language = LanguageValue.ZhTw, "停止主泵", "Stopping pump")
                    If Not Wait.Finished Then Exit Select
                    .PumpOn = False                   '在ControlCode內 impco -->pump off + pumpFB no = error 
                    .PumpStopRequest = False                                    'ControlCode.PumpStopRequest = 1
                    '.FanOn = False
                    WaitStable.TimeRemaining = WaitStableTime        '
                    State = S18.WaitTimer
                Case S18.WaitTimer
                    StateString = If(.Language = LanguageValue.ZhTw, "等待水位穩定 ", "Wait stable time ") & "" & TimerString(WaitStable.TimeRemaining)
                    If Not WaitStable.Finished Then Exit Select
                    State = S18.RecollectLevel

                Case S18.RecollectLevel
                    .RecallLevel = .IO.MainTankLevel
                    Wait.TimeRemaining = 5
                    State = S18.WaitFinish

                Case S18.WaitFinish
                    Dim i As Integer
                    For i = 1 To 50
            If .SetMainTankAnalogInput(i) > 0 And .SetMainTankVolume(i) > 0 And .RecallLevel <= .SetMainTankAnalogInput(i) Then
              .MainTankTargetVolume = .SetMainTankVolume(i)
              Exit For
            End If
                    Next
                    StateString = If(.Language = LanguageValue.ZhTw, "記憶水位=", "Recall level=") & "" & .MainTankTargetVolume & "L"
                    If Not Wait.Finished Then Exit Select
                    Wait.TimeRemaining = 2
                    .PumpStartRequest = True
                    State = S18.WaitStartPump

                Case S18.WaitStartPump
                    StateString = If(.Language = LanguageValue.ZhTw, "啟動主泵", "Start pump")
                    If Not Wait.Finished Then Exit Select
                    .PumpStartRequest = False
                    .PumpOn = True
                    State = S18.WaitPumpFB

                Case S18.WaitPumpFB
                    StateString = If(.Language = LanguageValue.ZhTw, "主泵啟動異常", "Main pump not run")
                    If Not .IO.MainPumpFB Then Exit Select
                    State = S18.Off

            End Select
        End With
    End Function

    Public Sub Cancel() Implements ACCommand.Cancel
        State = S18.Off
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
            Return State <> S18.Off
        End Get
    End Property

    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S18
    Public Property State() As S18
        Get
            Return state_
        End Get
        Private Set(ByVal value As S18)
            state_ = value
        End Set
    End Property
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S18
    Public Property StateWas() As S18
        Get
            Return statewas_
        End Get
        Private Set(ByVal value As S18)
            statewas_ = value
        End Set
    End Property

#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
    Public ReadOnly Command18 As New Command18(Me)
End Class
#End Region
