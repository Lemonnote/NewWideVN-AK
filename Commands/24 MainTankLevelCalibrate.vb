<Command("Main Level Cal.", "Qty|0-100| Time|0-99| Volume|0-999| Start|0-9999| End|0-9999|", , , "('4/60)+2"),
TranslateCommand("zh-TW", "主缸水位校正", "水量|0-100|% 時間|0-99| 容量|0-999| 開始|0-9999| 結束|0-9999|"),
Description("水量=0-100%, 容量=0-999L"),
TranslateDescription("zh-TW", "水量=0-100%, 容量=0-999L")>
Public NotInheritable Class Command24
    Inherits MarshalByRefObject                       'Inheritsg是繼承Windows Form的應用程式要繼承System.Windows.Forms.Form，可先參考物件導向程式設計相關書籍
    Implements ACCommand

    Public Enum S24
        Off
        CheckReady
        FillQty
        Add
        WaitStable
        Update
        Pause
    End Enum
    Public StateString As String

    Public Time, Qty, Number, StartVolume, EndVolume, Volume As Integer
    Public WaitTimer As New Timer
    Public CurrentStepNumber As Integer

    Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
      .Command05.Cancel() : .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel()
      .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
      .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel() : .Command54.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel() : .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command45.Cancel() : .Command46.Cancel()

      .TempControlFlag = False

      Qty = MinMax(param(1) * 10, 0, 1000)
      Time = Maximum(param(2), 99)
      Volume = MinMax(param(3), 0, 999)
      StartVolume = MinMax(param(4), 0, 9999)
      EndVolume = MinMax(param(5), 0, 9999)
      Number = 1
      .SetMainTankVolume(1) = StartVolume
      .SetMainTankAnalogInput(1) = 0
      State = S24.CheckReady
    End With
  End Function

    Public Function Run() As Boolean Implements ACCommand.Run
        With ControlCode
            Select Case State
                Case S24.Off
                    StateString = ""

                Case S24.CheckReady
                    If .Parent.IsPaused Then
                        WaitTimer.Pause()
                        StateWas = State
                        State = S24.Pause
                        CurrentStepNumber = .Parent.CurrentStep
                    End If
                    If Not .IO.SystemAuto Then Exit Select
                    Number = Number + 1
                    State = S24.FillQty

                Case S24.FillQty
                    If .Parent.IsPaused Then
                        WaitTimer.Pause()
                        StateWas = State
                        State = S24.Pause
                        CurrentStepNumber = .Parent.CurrentStep
                    End If
          StateString = If(.Language = LanguageValue.ZhTw, "B缸入水到 ", "Filling Tank B to ") & " " & Qty / 10 & "%"
          If .IO.TankBLevel >= Qty Then
                        State = S24.Add
                        WaitTimer.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
                    End If

                Case S24.Add
                    If .Parent.IsPaused Then
                        WaitTimer.Pause()
                        StateWas = State
                        State = S24.Pause
                        CurrentStepNumber = .Parent.CurrentStep
                    End If
          StateString = If(.Language = LanguageValue.ZhTw, "加藥延遲", "Transferring") & " " & TimerString(WaitTimer.TimeRemaining)
          If .IO.TankBLevel >= 50 Then WaitTimer.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
                    If Not WaitTimer.Finished Then Exit Select
                    State = S24.WaitStable
                    WaitTimer.TimeRemaining = Time

                Case S24.WaitStable
                    If .Parent.IsPaused Then
                        WaitTimer.Pause()
                        StateWas = State
                        State = S24.Pause
                        CurrentStepNumber = .Parent.CurrentStep
                    End If
          StateString = If(.Language = LanguageValue.ZhTw, "等待穩定", "Wait stable") & " " & TimerString(WaitTimer.TimeRemaining)
          If Not WaitTimer.Finished Then Exit Select
                    State = S24.Update

                Case S24.Update
                    If .Parent.IsPaused Then
                        WaitTimer.Pause()
                        StateWas = State
                        State = S24.Pause
                        CurrentStepNumber = .Parent.CurrentStep
                    End If
          .SetMainTankAnalogInput(Number) = .IO.MainTankLevel
          .SetMainTankVolume(Number) = .SetMainTankVolume(Number - 1) + Volume
          .SaveMainTankCalibration()
          If .SetMainTankVolume(Number) >= EndVolume Then
            State = S24.Off
          Else
            State = S24.CheckReady
                    End If

                Case S24.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停", "Paused") & " " & TimerString(WaitTimer.TimeRemaining)
          If Not .Parent.IsPaused Then
                        If CurrentStepNumber = .Parent.CurrentStep Then
                            State = StateWas
                            StateWas = S24.Off
                            WaitTimer.Restart()
                        Else
                            State = S24.Off
                            WaitTimer.Cancel()
                        End If
                    End If

            End Select
        End With
    End Function

    Public Sub Cancel() Implements ACCommand.Cancel
        State = S24.Off
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
            Return State <> S24.Off
        End Get
    End Property
    Public ReadOnly Property IsFilling() As Boolean
        Get
            Return State = S24.FillQty
        End Get
    End Property
    Public ReadOnly Property IsAdd() As Boolean
        Get
            Return State = S24.Add
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S24
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S24
    Public Property State() As S24
        Get
            Return state_
        End Get
        Private Set(ByVal value As S24)
            state_ = value
        End Set
    End Property
    Public Property StateWas() As S24
        Get
            Return statewas_
        End Get
        Private Set(ByVal value As S24)
            statewas_ = value
        End Set
    End Property
    'End Property

#End Region

End Class

#Region " Class Instance "

Partial Public Class ControlCode
    Public ReadOnly Command24 As New Command24(Me)
End Class

#End Region
