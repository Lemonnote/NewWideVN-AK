'<Command("PhControlTemp", "TARGET PH |4-9|.|00-99|", , , "", CommandType.ParallelCommand), _
'TranslateCommand("zh", "PH平行-溫度", "PH值設定 |4-9|.|00-99|"), _
'Description("MAX=999,MIN=400 CONTROL"), _
'TranslateDescription("zh", "最高=999,最低=400")> _
Public NotInheritable Class Command75
    Inherits MarshalByRefObject
    Implements ACCommand

    Public Enum S75
        Off
        Start
        Finish_Wash
        Pause
    End Enum

    Public PhTarget As Integer
    Public Wait As New Timer, RunBackWait As New Timer
    Public StateString As String

    Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
        With ControlCode
            '--------------------------------------------------------------------------------------------------------PH用
            .PhControl.Cancel() : .PhWash.Cancel() : .Command73.Cancel() : .Command76.Cancel() : .Command74.Cancel() : .Command77.Cancel() : .Command78.Cancel() : .Command79.Cancel() : .Command80.Cancel() : .Command81.Cancel() : .Ph76Time.Cancel()
            '---------------------------------------------------------------------------------------------------------
            .F76專用旗標 = False
            PhTarget = Maximum(param(1) * 100 + param(2), 999) '60*60
            State = S75.Start


        End With
    End Function

    Public Function Run() As Boolean Implements ACCommand.Run
        With ControlCode
            Select Case State
                Case S75.Off
                    StateString = ""
                    If .Command74.State = Command74.S74.Off And .Command76.State = Command76.S76.Off And .Command77.State = Command77.S77.Off And .Command78.State = Command78.S78.Off And _
                    .Command80.State = Command80.S80.Off And Not .Ph76Time.IsOn Then
                        .PhControlFlag = False
                    End If


                Case S75.Start
                    StateString = ""
                    .StepNumber = .Parent.StepNumber

                    .PhControlFlag = True

                    If .Parent.IsPaused Or Not .IO.MainPumpFB Or Not .IO.SystemAuto Or .IO.PhCheckTemp > 1050 Then


                        'pause the timer
                        Wait.Pause()
                        State = S75.Pause
                    End If

                    If .PhControl.State = PhControl_.PhControl.Finished Then
                        State = S75.Finish_Wash
                    End If

                Case S75.Finish_Wash
                    StateString = ""
                    .PhControlFlag = False
                    '.PhWash.Run()
                    'If .PhWash.State = PhWash_.PhWash.Finish Then
                    State = S75.Off
                    'End If


                Case S75.Pause
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
                            State = S75.Start
                        Else
                            State = S75.Off
                        End If

                    End If
            End Select

        End With
    End Function

    Public Sub Cancel() Implements ACCommand.Cancel
        State = S75.Off
        Wait.Cancel()

    End Sub
    Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

        PhTarget = Maximum(param(1) * 100 + param(2), 999) '60*60
    End Sub
#Region "Standard Definitions"
    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
        Get
            Return State <> S75.Off
        End Get
    End Property
    Public ReadOnly Property IsActive() As Boolean
        Get
            Return State > S75.Start
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S75
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S75

    Public Property State() As S75
        Get
            Return state_
        End Get
        Private Set(ByVal value As S75)
            state_ = value
        End Set
    End Property
    Public ReadOnly Property Istest() As Boolean
        Get
            Return (State = S75.Start)
        End Get
    End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
    Public ReadOnly Command75 As New Command75(Me)
End Class
#End Region
