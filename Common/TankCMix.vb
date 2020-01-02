Public Class TankCMix_
  Public Enum TankCMix
    Off
    MixForTime
    WaitMixer
    MixForTime1
    Ready
    Pause
  End Enum

  Public Wait As New Timer
  Public StateString As String
  Public Time As Integer

  Public Sub Run()
    With ControlCode
      Select Case State
        Case TankCMix.Off
          StateString = ""
          If .TankCMixOn Then
            Time = .Command65.Time
          Else
            Exit Select
          End If
          State = TankCMix.MixForTime

        Case TankCMix.MixForTime
          StateString = If(.Language = LanguageValue.ZhTw, "C藥缸升溫 ", "Tank C heating for ")
          If .Command65.HeatingSet = 1 Then
            .CTankHeatStartRequest = True
            If .IO.CTankTemperature < .CHeatTemperature Then Exit Select
            .CTankHeatStartRequest = False
          End If
          State = TankCMix.WaitMixer

        Case TankCMix.WaitMixer
          Wait.TimeRemaining = Time
          StateString = If(.Language = LanguageValue.ZhTw, "等待B缸攪拌", "Wait Tank B Mixing")
          If (.TankBMix.IsMixingForTime Or .Command54.IsMixingForTime) And (.Parameters.CTankMixType = 0 And .Parameters.BTankMixType = 0) Then Exit Select
          State = TankCMix.MixForTime1

        Case TankCMix.MixForTime1
          StateString = If(.Language = LanguageValue.ZhTw, "C藥缸攪拌 ", "Tank C mixing for ") & TimerString(Wait.TimeRemaining)
          If .Parent.IsPaused Then
            State = TankCMix.Pause
            Wait.Pause()
          End If
          If Wait.Finished Then
            State = TankCMix.Ready
          End If

        Case TankCMix.Ready
          .TankCReady = True
          .TankCMixOn = False
          State = TankCMix.Off

        Case TankCMix.Pause
          StateString = If(.Language = LanguageValue.ZhTw, "暫停", "Paused") & " " & TimerString(Wait.TimeRemaining)
          If .Parent.CurrentStep <> .Parent.ChangingStep Then
            State = TankCMix.Off
            Wait.Cancel()
          End If

          If Not .Parent.IsPaused Then
            State = TankCMix.MixForTime1
            Wait.Restart()
          End If

      End Select

    End With
  End Sub

#Region "Standard Definitions"
  Public Sub Cancel()
    With ControlCode
      State = TankCMix.Off
      .TankCMixOn = False
      Time = 0
      Wait.Cancel()
    End With
  End Sub

  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As TankCMix
  Public Property State() As TankCMix
    Get
      Return state_
    End Get
    Private Set(ByVal value As TankCMix)
      state_ = value
    End Set
  End Property
  Public ReadOnly Property IsMixingForTime() As Boolean
    Get
      Return (State = TankCMix.MixForTime1)
    End Get
  End Property

#End Region
End Class

Partial Class ControlCode
  Public ReadOnly TankCMix As New TankCMix_(Me)
End Class
