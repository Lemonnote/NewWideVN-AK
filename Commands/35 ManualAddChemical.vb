<Command("Manual Add Chemical", , , , "2"),
 TranslateCommand("zh-TW", "呼叫手動加助劑"),
 Description("Manual Add Chemical"),
 TranslateDescription("zh-TW", "呼叫手動加助劑")>
Public NotInheritable Class Command35
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S35
    Off
    WaitCallAck
    WaitTime
    WaitCallAck2
  End Enum

  Public Wait As New Timer
  Public StateString As String
  Public 馬達啟動延遲 As New Timer

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      'cancels for all other forground functions
      .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
      .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel() : .Command14.Cancel()
      .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
      .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel() : .Command54.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel()
      .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command24.Cancel()

      '.IO.CallLamp = True
      .MessageCallOperator = True
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
      State = S35.WaitCallAck
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S35.Off
          StateString = ""
          .MessageCallOperator = False

        Case S35.WaitCallAck
          '****************************************************
          If Not 馬達啟動延遲.Finished Then Exit Select
          If Not .IO.MainPumpFB And .IO.LowLevel Then
            StateString = If(.Language = LanguageValue.ZhTw, "馬達未運行", "Main Pump Not Run")
            Exit Select
          ElseIf Not .IO.LowLevel Then
            StateString = If(.Language = LanguageValue.ZhTw, "沒有低水位來啟動馬達", "No Low Level To Start Pump")
            Exit Select
          Else
            StateString = ""
            .PumpStartRequest = False
          End If
          '****************************************************
          '呼叫的訊息如下列
          '1.Msk Scouring Agent
          '2.Msk Anti Crease
          '3:Msk(Sequesier)
          '4:Msk Soda Ash
          '5:Msk(Coustic)
          '6:Msk Soda Bicarbonat
          '7:Msk(Peroxide)
          '8:Msk Peroxide killer
          '9:Msk(Cuka)
          '10:Msk(Garam)
          '11:Msk(Sabun)
          '12:Msk(Cat)
          '13:Msk(Optical)
          '14:Msk(Hydrosulphite)
          '15:Msk(Sabun & Caustic)
          '16:Msk Cuka & Anti Crease
          '17:Msk(Alkali)
          '18:Msk(Auxiliaries)
          '19:Msk Fixing Agent
          '20:Cek(pH)
          '21:Cek(Air / Bersih)
          '22:Cek Sisa H202
          If .FlashFlag2 Then
            StateString = If(.Language = LanguageValue.ZhTw, "呼叫手動加染料", "Call Manual Add Dyes")
          Else
            StateString = If(.Language = LanguageValue.ZhTw, "按下確認鈕停止警報", "Wait Call Ack To Stop Alarm")
          End If

          If Not .IO.CallAck Then Exit Select
          Wait.TimeRemaining = 2
          State = S35.WaitTime

        Case S35.WaitTime
          StateString = If(.Language = LanguageValue.ZhTw, "呼叫手動加染料", "Call Manual Add Dyes")
          If Not Wait.Finished Then Exit Select
          State = S35.WaitCallAck2

        Case S35.WaitCallAck2
          If .FlashFlag2 Then
            StateString = If(.Language = LanguageValue.ZhTw, "呼叫手動加染料", "Call Manual Add Dyes")
          Else
            StateString = If(.Language = LanguageValue.ZhTw, "按下確認鈕到下一步", "Wait Call Ack To Next Step")
          End If
          If Not .IO.CallAck Then Exit Select
          '.IO.CallLamp = False
          .MessageCallOperator = False
          State = S35.Off
      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S35.Off
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
      Return State <> S35.Off
    End Get
  End Property
  Public ReadOnly Property IsCalling() As Boolean
    Get
      Return (State = S35.WaitCallAck)
    End Get
  End Property

  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S35
  Public Property State() As S35
    Get
      Return state_
    End Get
    Private Set(ByVal value As S35)
      state_ = value
    End Set
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command35 As New Command35(Me)
End Class
#End Region
