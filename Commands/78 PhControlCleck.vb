<Command("PhControlCleck", "TIME |0-99| ", , , "'1", ),
TranslateCommand("zh", "PH控制-檢測", "檢測時間 |0-99|  "),
Description(" TIME MAX=99M  0=KEEP"),
TranslateDescription("zh", "時間 最高=99分 0=持續檢測")>
Public NotInheritable Class Command78
  Inherits MarshalByRefObject
  Implements ACCommand
  Public Enum S78
    Off
    Start
    KeepTime
    Pause
    Finish
    Finish_Wash

  End Enum

  Public CheckTime As Integer
  Public Wait, WaitKeepTime As New Timer
  Public StateString As String

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode

      '--------------------------------------------------------------------------------------------------------PH用
      .PhControl.Cancel() : .PhWash.Cancel() : .PhControlFlag = False : .PhCirculateRun.Cancel()
      .Command73.Cancel() : .Command74.Cancel() : .Command75.Cancel() : .Command76.Cancel() : .Command77.Cancel() : .Command79.Cancel() : .Command80.Cancel() : .Command81.Cancel() : .Ph76Time.Cancel()
      '---------------------------------------------------------------------------------------------------------

      CheckTime = MinMax(param(1), 0, 99) * 60
      If CheckTime = 0 Then
        .PhCirRun = True
        State = S78.Finish
      Else
        State = S78.Start
      End If


    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S78.Off
          StateString = ""
          If .Command75.State = Command75.S75.Off And .Command74.State = Command74.S74.Off And
                    .Command76.State = Command76.S76.Off And .Command77.State = Command77.S77.Off And .Command80.State = Command80.S80.Off And Not .Ph76Time.IsOn Then
            .PhControlFlag = False
          End If


        Case S78.Start
          StateString = ""
          .StepNumber = .Parent.StepNumber

          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Or .IO.PhCheckTemp > 1050 Then
            'pause the timer
            WaitKeepTime.Pause()
            State = S78.Pause
          End If
          WaitKeepTime.TimeRemaining = CheckTime
          State = S78.KeepTime


        Case S78.KeepTime
          .PhCirculateRun.Run()
          StateString = If(.Language = LanguageValue.ZhTw, "pH檢測剩餘時間", "Running") & " " & TimerString(WaitKeepTime.TimeRemaining) & " ( pH值 " & (.IO.PhValue / 100) & " ) "
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Or .IO.PhCheckTemp > 1050 Then
            'pause the timer
            WaitKeepTime.Pause()
            State = S78.Pause
          End If

          If WaitKeepTime.Finished Then
            State = S78.Finish
          End If

        Case S78.Pause
          If Not .IO.MainPumpFB Then
            StateString = If(.Language = LanguageValue.ZhTw, "馬達未啟動！", "Not Running")
          End If
          If .IO.PhCheckTemp > 1050 Then
            StateString = If(.Language = LanguageValue.ZhTw, "PH檢測桶溫度過高！", "Hot temp.")
          End If
          'no longer pause restart the timer and go back to the wait state
          If (Not .Parent.IsPaused) And .IO.MainPumpFB And .IO.SystemAuto And .IO.PhCheckTemp < 1050 Then

            If .Parent.StepNumber = .StepNumber Then
              WaitKeepTime.Restart()
              State = S78.Start
            Else
              State = S78.Off
            End If

          End If


        Case S78.Finish
          StateString = ""
          State = S78.Off



      End Select

    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S78.Off
    Wait.Cancel()

  End Sub
  Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged


    CheckTime = MinMax(param(1), 0, 99) * 60
  End Sub
#Region "Standard Definitions"
  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return State <> S78.Off
    End Get
  End Property
  Public ReadOnly Property IsActive() As Boolean
    Get
      Return State > S78.Start
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S78
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S78

  Public Property State() As S78
    Get
      Return state_
    End Get
    Private Set(ByVal value As S78)
      state_ = value
    End Set
  End Property
  Public ReadOnly Property Istest() As Boolean
    Get
      Return (State = S78.Start)
    End Get
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
    Public ReadOnly Command78 As New Command78(Me)
End Class
#End Region
