<Command("Wash B Tank", , , , "2"),
 TranslateCommand("zh-TW", "B缸清洗"),
 Description("Wash B Tank"),
 TranslateDescription("zh-TW", "B缸清洗")>
Public NotInheritable Class Command58
    Inherits MarshalByRefObject
    Implements ACCommand
    Public StateString As String

    Public Enum S58
        Off
        Rinse1
        Rinse11
        MixCir
        Add
        Rinse2
        Drain
    End Enum

    Public Timer As New Timer
    Public 馬達啟動延遲 As New Timer

    Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
        With ControlCode
            .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
            .Command05.Cancel() : .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel()
            .Command14.Cancel() : .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel()
            .Command32.Cancel() : .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel()
            .Command54.Cancel() : .Command55.Cancel() : .Command57.Cancel() : .Command66.Cancel()
            .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
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

            State = S58.Rinse1
        End With
    End Function

    Public Function Run() As Boolean Implements ACCommand.Run
        With ControlCode


            Select Case State
                Case S58.Off
                    StateString = ""


                Case S58.Rinse1

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
                    Timer.TimeRemaining = .Parameters.AddTransferRinseTime
                    State = S58.Rinse11

                Case S58.Rinse11

                    StateString = If(.Language = LanguageValue.ZhTw, "B缸洗缸中", "Tank B rinsing ") & TimerString(Timer.TimeRemaining)
                    If Timer.Finished Then
                        Timer.TimeRemaining = .Parameters.AddCirculateTimeAfterRinse
                        State = S58.MixCir
                    End If

                Case S58.MixCir
                    StateString = If(.Language = LanguageValue.ZhTw, "B缸循環中", "Tank B circulating ") & TimerString(Timer.TimeRemaining)
                    If Timer.Finished Then
                        Timer.TimeRemaining = .Parameters.AddTransferDrainTime
                        State = S58.Add
                    End If

                Case S58.Add
                    StateString = If(.Language = LanguageValue.ZhTw, "B缸排水中", "Tank B draining ") & TimerString(Timer.TimeRemaining)
                    If .BTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
                    If Timer.Finished Then
                        Timer.TimeRemaining = .Parameters.AddTransferRinseTime
                        State = S58.Rinse2
                    End If

                Case S58.Rinse2
                    StateString = If(.Language = LanguageValue.ZhTw, "B缸洗缸中", "Tank B rinsing ") & TimerString(Timer.TimeRemaining)
                    If Timer.Finished Then
                        Timer.TimeRemaining = .Parameters.AddTransferDrainTime
                        State = S58.Drain
                    End If

                Case S58.Drain
                    StateString = If(.Language = LanguageValue.ZhTw, "B缸排水", "Tank B draining ") & TimerString(Timer.TimeRemaining)
                    If .BTankLowLevel Then Timer.TimeRemaining = .Parameters.AddTransferDrainTime
                    If Timer.Finished Then
                        State = S58.Off
                        .TankBReady = False
                        .TemperatureControl.Cancel()
                        .TempControlFlag = False
                    End If

            End Select
        End With
    End Function

    Public Sub Cancel() Implements ACCommand.Cancel
        With ControlCode
            State = S58.Off
        End With
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
            Return State <> S58.Off
        End Get
    End Property
    Public ReadOnly Property IsRinsing() As Boolean
        Get
            Return ((State = S58.Rinse1) Or (State = S58.Rinse2))
        End Get
    End Property
    Public ReadOnly Property IsDraining() As Boolean
        Get
            Return ((State = S58.Rinse2) Or (State = S58.Drain) Or (State = S58.Add))
        End Get
    End Property
    Public ReadOnly Property IsCirculating() As Boolean
        Get
            Return (State = S58.MixCir) Or (State = S58.Rinse1)
        End Get
    End Property
    Public ReadOnly Property IsTransferPump() As Boolean
        Get
            Return (State = S58.MixCir)
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S58
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S58
    Public Property State() As S58
        Get
            Return state_
        End Get
        Private Set(ByVal value As S58)
            state_ = value
        End Set
    End Property
    Public Property StateWas() As S58
        Get
            Return statewas_
        End Get
        Private Set(ByVal value As S58)
            statewas_ = value
        End Set
    End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
    Public ReadOnly Command58 As New Command58(Me)
End Class
#End Region
