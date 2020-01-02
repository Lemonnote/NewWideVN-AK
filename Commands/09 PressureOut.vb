<Command("Pressure Out", "Temp |0-99|"),
 TranslateCommand("zh-TW", "主缸排壓", "排壓溫度 |0-99|"),
 Description("99=MAX 0=MIN"),
 TranslateDescription("zh-TW", "99度=最高,0度=最小")>
Public NotInheritable Class Command09
  Inherits MarshalByRefObject
  Implements ACCommand

  Public Enum S09
    Off
  End Enum

  Public Wait As New Timer

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      .PressureOutTemp = param(1) * 10
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S09.Off
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
      Return State <> S09.Off
    End Get
  End Property

  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S09
  Public Property State() As S09
    Get
      Return state_
    End Get
    Private Set(ByVal value As S09)
      state_ = value
    End Set
  End Property
#End Region
End Class

#Region "Class Instance"
Partial Public Class ControlCode
  Public ReadOnly Command09 As New Command09(Me)
End Class
#End Region
