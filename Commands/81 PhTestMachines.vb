'<Command("PhTestMachine", "TopH PH |1-9999| ", , , "'3", ), _
' TranslateCommand("zh", "校正pH吐出量-工程師用", "PH吐出量時間 |1-9999|  "), _
' Description("CONTROL PH MAX= 9999S ,MIN= 1S "), _
' TranslateDescription("zh", "吐出量時間 最大=9999秒 最小=1秒 ")> _
Public NotInheritable Class Command81
    Inherits MarshalByRefObject
    Implements ACCommand

    Public Enum S80
        Off
        Start
        Process
        Process1
        Process2
        Finish_Wash
        Pause
        Finish
    End Enum

    Public PhTarget, PhOpenTemp, AddTime As Integer
    Public Wait As New Timer, RunBackWait As New Timer
    Public StateString As String

    Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
        With ControlCode


      '--------------------------------------------------------------------------------------------------------PH用

      .PhControl.Cancel() : .PhWash.Cancel() : .Command73.Cancel() : .Command75.Cancel() : .Command74.Cancel() : .Command77.Cancel() : .Command78.Cancel() : .Command79.Cancel() : .Command76.Cancel() : .Command80.Cancel() : .Ph76Time.Cancel()
            '---------------------------------------------------------------------------------------------------------

            PhTarget = param(1)

            State = S80.Start



        End With
    End Function

    Public Function Run() As Boolean Implements ACCommand.Run
        With ControlCode
            Select Case State
                Case S80.Off
                    StateString = ""

                Case S80.Start
                    StateString = "請放量杯於pH設備吐出管，放置後請按 確認鈕 "
                    If .IO.CallAck Then
                        Wait.TimeRemaining = 10
                        State = S80.Process

                    End If


                Case S80.Process
                    StateString = "pH定量馬達計量開始"
                    Wait.TimeRemaining = PhTarget
                    State = S80.Process1

                Case S80.Process1
                    StateString = "pH定量馬達送料中 倒數：" & TimerString(Wait.TimeRemaining)
                    If Wait.Finished Then
                        State = S80.Process2
                    End If


                Case S80.Process2
                    StateString = "結束後 確認鈕 "
                    If .IO.CallAck Then
                        State = S80.Off
                    End If




            End Select

        End With
    End Function

    Public Sub Cancel() Implements ACCommand.Cancel
        State = S80.Off
        Wait.Cancel()

    End Sub
    Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

        PhTarget = Maximum(param(1) * 100 + param(2), 999) '60*60
        PhOpenTemp = 20
        AddTime = MinMax(param(3), 1, 60)

    End Sub
#Region "Standard Definitions"
    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
        Get
            Return State <> S80.Off
        End Get
    End Property
    Public ReadOnly Property IsActive() As Boolean
        Get
            Return State > S80.Start
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S80
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private statewas_ As S80

    Public Property State() As S80
        Get
            Return state_
        End Get
        Private Set(ByVal value As S80)
            state_ = value
        End Set
    End Property
    Public ReadOnly Property Istest() As Boolean
        Get
            Return (State = S80.Process1)
        End Get
    End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
    Public ReadOnly Command81 As New Command81(Me)
End Class
#End Region
