<Command("Unload", , , , "5"),
 TranslateCommand("zh-TW", "出布")>
Public NotInheritable Class Command33
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S33
    Off
    WaitAuto
    WaitCallAck
    WaitTime
    WaitCallAck2
  End Enum

    Public Wait As New Timer
    Public StateString As String
    Public 馬達啟動延遲 As New Timer
  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command01.Cancel() : .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel()
      .Command05.Cancel() : .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel()
      .Command14.Cancel() : .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel()
      .Command32.Cancel() : .Command51.Cancel() : .Command52.Cancel() : .Command54.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel() : .Command64.Cancel()
      .Command65.Cancel() : .TemperatureControl.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command45.Cancel() : .Command46.Cancel()
      .Command24.Cancel()


      .TempControlFlag = False
      ' .IO.CallLamp = True
            .MessageUnloadFiber = True
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
      State = S33.WaitAuto
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S33.Off
          .MessageUnloadFiber = False

                Case S33.WaitAuto
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
          StateString = If(.Language = LanguageValue.ZhTw, "出布", "Unload")
          If Not .IO.SystemAuto Then Exit Select
                    State = S33.WaitCallAck


        Case S33.WaitCallAck
          StateString = If(.Language = LanguageValue.ZhTw, "出布", "Unload")
          If Not .IO.CallAck Then Exit Select
          Wait.TimeRemaining = 2
          State = S33.WaitTime

        Case S33.WaitTime
          StateString = If(.Language = LanguageValue.ZhTw, "出布", "Unload")
          If Not Wait.Finished Then Exit Select
          State = S33.WaitCallAck2

        Case S33.WaitCallAck2
          StateString = If(.Language = LanguageValue.ZhTw, "出布", "Unload")
          If Not .IO.CallAck Then Exit Select
          '    .IO.CallLamp = False
          .MessageUnloadFiber = False
          State = S33.Off
      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S33.Off
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
      Return State <> S33.Off
    End Get
  End Property
  Public ReadOnly Property IsCalling() As Boolean
    Get
      Return (State = S33.WaitCallAck)
    End Get
  End Property

  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S33
  Public Property State() As S33
    Get
      Return state_
    End Get
    Private Set(ByVal value As S33)
      state_ = value
    End Set
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command33 As New Command33(Me)
End Class
#End Region
