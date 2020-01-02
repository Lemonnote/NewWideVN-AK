Public Class TankBMix_
    Public Enum TankBMix
        Off
        WaitMixer
        MixForTime
        Ready
        Pause
    End Enum

    Public Wait As New Timer
    Public StateString As String
    Public Time As Integer

    Public Sub Run()
        With ControlCode
            Select Case State
                Case TankBMix.Off
                    StateString = ""
                    If .TankBMixOn Then
                        Time = .Command65.Time
                    Else
                        Exit Select
                    End If
                    State = TankBMix.WaitMixer


                Case TankBMix.WaitMixer
                    Wait.TimeRemaining = .Command64.Time
                    StateString = If(.Language = LanguageValue.ZhTw, "等待C缸攪拌", "Wait Tank C Mixing")
                    If (.TankCMix.IsMixingForTime Or .Command55.IsMixingForTime) And (.Parameters.CTankMixType = 0 And .Parameters.BTankMixType = 0) Then Exit Select
                    State = TankBMix.MixForTime

                Case TankBMix.MixForTime
                    StateString = If(.Language = LanguageValue.ZhTw, "B藥缸攪拌 ", "Tank B mixing for ") & TimerString(Wait.TimeRemaining)
                    If .Parent.IsPaused Then
                        State = TankBMix.Pause
                        Wait.Pause()
                    End If
          If Not Wait.Finished Then Exit Select
          StateString = If(.Language = LanguageValue.ZhTw, "等待B藥缸加熱 ", "Wait tank B heat")
          If .IO.BTankTemperature < .BHeatTemperature Then Exit Select
          .BTankHeatStartRequest = False
          State = TankBMix.Ready


        Case TankBMix.Ready
          .TankBReady = True
          .TankBMixOn = False
          State = TankBMix.Off

        Case TankBMix.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停", "Paused") & " " & TimerString(Wait.TimeRemaining)
          If .Parent.CurrentStep <> .Parent.ChangingStep Then
            State = TankBMix.Off
            Wait.Cancel()
          End If

          If Not .Parent.IsPaused Then
            State = TankBMix.MixForTime
            Wait.Restart()
          End If

      End Select

        End With
    End Sub
#Region "Standard Definitions"
    Public Sub Cancel()
        With ControlCode
            State = TankBMix.Off
            .TankBMixOn = False
            Time = 0
            Wait.Cancel()
        End With
    End Sub

    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As TankBMix
    Public Property State() As TankBMix
        Get
            Return state_
        End Get
        Private Set(ByVal value As TankBMix)
            state_ = value
        End Set
    End Property
    Public ReadOnly Property IsMixingForTime() As Boolean
        Get
            Return (State = TankBMix.MixForTime)
        End Get
    End Property

#End Region
End Class

Partial Class ControlCode
  Public ReadOnly TankBMix As New TankBMix_(Me)
End Class
