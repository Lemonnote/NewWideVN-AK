<Command("PhControlAdjust", "TARGET PH |4-9|.|00-99| OPEN  ADD TIME |2-60| ", , , "", ), _
 TranslateCommand("zh-TW", "PH值控制-調整", "PH值設定 |4-9|.|00-99|  加酸時間 |0-60|  "), _
 Description("CONTROL PH MAX=PH 9.9,MIN=PH 4.0  ; ADD TIME MAX=60M MIN=FAST  "), _
 TranslateDescription("zh-TW", "PH控制  最高=PH 9.9,最低=PH 4.0  ; 加酸時間 最高=60分 快速=0 ")> _
Public NotInheritable Class Command77
  Inherits MarshalByRefObject
  Implements ACCommand
  Public Enum S77
    Off
    Start
    KeepTime
    Pause
    Finish
    Finish_Wash
  End Enum

  Public PhTarget, PhOpenTemp, AddTime As Integer
  Public Wait, WaitKeepTime As New Timer, RunBackWait As New Timer
  Public StateString As String

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .Command02.Cancel() : .Command03.Cancel() : .Command04.Cancel() : .Command05.Cancel()
      .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel() : .Command14.Cancel()
      .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
      .Command33.Cancel() : .Command51.Cancel() : .Command52.Cancel() : .Command56.Cancel()
      .Command57.Cancel() : .TemperatureControl.Cancel() : .Command01.Cancel()
      '--------------------------------------------------------------------------------------------------------PH用
      .PhControl.Cancel() : .PhWash.Cancel() : .PhControlFlag = False : .PhCirculateRun.Cancel()
      .Command73.Cancel() : .Command74.Cancel() : .Command75.Cancel() : .Command76.Cancel()
      '---------------------------------------------------------------------------------------------------------

      PhTarget = Maximum(param(1) * 100 + param(2), 999) '60*60
      AddTime = MinMax(param(3), 0, 60)
      State = S77.Start


    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S77.Off
          If .Command75.State = Command75.S75.Off And .Command74.State = Command74.S74.Off And .Command76.State = Command76.S76.Off Then
            .PhControlFlag = False
          End If


        Case S77.Start
          .StepNumber = .Parent.StepNumber
          If .PhControlFlag = False Then
            .PhControlFlag = True
          End If


          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then


            'pause the timer
            WaitKeepTime.Pause()
            State = S77.Pause
          End If


          If .PhControl.OpenKeepTime = True Then
            WaitKeepTime.TimeRemaining = AddTime * 60
            State = S77.KeepTime
          End If

        Case S77.KeepTime
          StateString = If(.Language = LanguageValue.ZhTw, "PH調整剩餘時間", "Running") & " " & TimerString(WaitKeepTime.TimeRemaining)
          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Then


            'pause the timer
            WaitKeepTime.Pause()
            State = S77.Pause
          End If

          If WaitKeepTime.Finished Then
            .PhControl.OpenKeepTime = False
            State = S77.Finish_Wash
          End If

        Case S77.Pause

          'no longer pause restart the timer and go back to the wait state
          If (Not .Parent.IsPaused) And .IO.MainPumpFB And .IO.SystemAuto Then

            If .Parent.StepNumber = .StepNumber Then
              WaitKeepTime.Restart()
              State = S77.Start
            Else
              State = S77.Off
            End If

          End If

        Case S77.Finish_Wash
          If .PhControlFlag = True Then
            .PhControlFlag = False
          End If
          .PhWash.Run()
          If .PhWash.State = PhWash_.PhWash.Finish Then
            State = S77.Finish
          End If

        Case S77.Finish

          State = S77.Off



      End Select

    End With
  End Function

    Public Sub Cancel() Implements ACCommand.Cancel
        State = S77.Off
        Wait.Cancel()

    End Sub
    Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

        PhTarget = Maximum(param(1) * 100 + param(2), 999) '60*60
        AddTime = MinMax(param(3), 0, 60)
    End Sub
#Region "Standard Definitions"
    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
        Get
            Return State <> S77.Off
        End Get
    End Property
    Public ReadOnly Property IsActive() As Boolean
        Get
            Return State > S77.Start
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S77
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S77

    Public Property State() As S77
        Get
            Return state_
        End Get
        Private Set(ByVal value As S77)
            state_ = value
        End Set
    End Property
    Public ReadOnly Property Istest() As Boolean
        Get
            Return (State = S77.Start)
        End Get
    End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
    Public ReadOnly Command77 As New Command77(Me)
End Class
#End Region
