Public Class Ph76Time_
    Public Enum Ph76Time
        Off
        Start
        Finish_Wash
        Pause
        Finish
    End Enum
    Public StateString As String
    Public Wait, Wait1 As New Timer
    Public Sub Run()
        With ControlCode
            'Usual pressure State control this will by default start at pressurized

            Select Case State
                Case Ph76Time.Off
                    StateString = ""
                    Wait.TimeRemaining = .Command76.AddTime * 60
                    State = Ph76Time.Start


                Case Ph76Time.Start
                    StateString = ""
                    .StepNumber = .Parent.StepNumber
                    If .PhControlFlag = False Then
                        .PhControlFlag = True
                    End If

                    'If .PhControl.開啟PH控制旗標 = False Then
                    '    Wait.TimeRemaining = .Command76.AddTime * 60
                    'End If

          If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Or .IO.PhCheckTemp > 1050 Then
            'pause the timer
            Wait.Pause()
            State = Ph76Time.Pause
          End If

                    If .PhControl.State = PhControl_.PhControl.Finished Then
                        State = Ph76Time.Finish_Wash
                    End If

          If Wait.Finished Then

            If .Command01.IsOn And .CoolNow Then
              State = Ph76Time.Finish_Wash

            ElseIf .IO.MainTemperature > .PH加酸安全溫度 * 10 Then
              State = Ph76Time.Finish_Wash

            ElseIf Not .Command01.IsOn Then
              State = Ph76Time.Finish_Wash

            End If

          ElseIf .IO.MainTemperature > .PH加酸安全溫度 * 10 Then
            State = Ph76Time.Finish_Wash

                    End If
                Case Ph76Time.Pause
                    If Not .IO.MainPumpFB Then
                        StateString = If(.Language = LanguageValue.ZhTw, "馬達未啟動！", "Not Running")
                    End If
                    If .IO.PhCheckTemp > 1050 Then
                        StateString = If(.Language = LanguageValue.ZhTw, "PH檢測桶溫度過高！", "Hot temp.")
                    End If
                    'no longer pause restart the timer and go back to the wait state
                    If (Not .Parent.IsPaused) And .IO.MainPumpFB And .IO.SystemAuto And .IO.PhCheckTemp < 1050 Then
                        If .Parent.StepNumber = .StepNumber Then
                            Wait.Restart()
                            State = Ph76Time.Start
                        Else
                            State = Ph76Time.Off
                        End If
                    End If

                Case Ph76Time.Finish_Wash
                    StateString = ""
                    If .PhControlFlag = True Then
                        .PhControlFlag = False
                    End If
                    State = Ph76Time.Finish

                Case Ph76Time.Finish
                    StateString = ""
                    .F76專用旗標 = False
                    State = Ph76Time.Off
            End Select
        End With

    End Sub
    Public Sub Cancel()
        State = Ph76Time.off
        Wait.TimeRemaining = 0
    End Sub
#Region "Standard Definitions"

    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As Ph76Time
    Public Property State() As Ph76Time
        Get
            Return state_
        End Get
        Private Set(ByVal value As Ph76Time)
            state_ = value
        End Set
    End Property
    Public ReadOnly Property IsOn() As Boolean
        Get
            Return (State <> Ph76Time.off)
        End Get
    End Property


#End Region
End Class

Partial Class ControlCode
    Public ReadOnly Ph76Time As New Ph76Time_(Me)
End Class
