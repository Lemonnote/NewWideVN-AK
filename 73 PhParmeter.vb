<Command("PH PARAMETER", "FillLevel |100-15000|"), _
 TranslateCommand("zh-TW", "PH參數設定", "進水量 |100-15000|"), _
 Description("15000=MAX 100=MIN"), _
 TranslateDescription("zh-TW", "15000L=最高,100L=最少")> _
Public NotInheritable Class Command73
    Inherits MarshalByRefObject
    Implements ACCommand

    Public Enum S73
        Off
        Wash
        WashFinish
        Pause
    End Enum

    Public Wait As New Timer
    Public StateString As String

    Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
        With ControlCode
            .PhFillLevel = param(1)             '進水量
            State = S73.wash
        End With
    End Function

    Public Function Run() As Boolean Implements ACCommand.Run
        With ControlCode
            Select Case State

                Case S73.Wash

                    '.PhWash.Run()

                    'If .PhWash.State = PhWash_.PhWash.Finish Then

                    State = S73.WashFinish

                    'End If



                Case S73.WashFinish

                    .PhWash.Cancel()

                    State = S73.Off





            End Select

        End With
    End Function

    Public Sub Cancel() Implements ACCommand.Cancel
        State = S73.Off
        Wait.Cancel()
    End Sub

    'Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

    'End Sub
    Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

        ControlCode.PhFillLevel = param(1)             '進水量
    End Sub
#Region "Standard Definitions"
    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
        Get
            Return State <> S73.Off
        End Get
    End Property

    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S73
    Public Property State() As S73
        Get
            Return state_
        End Get
        Private Set(ByVal value As S73)
            state_ = value
        End Set
    End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
    Public ReadOnly Command73 As New Command73(Me)
End Class
#End Region
