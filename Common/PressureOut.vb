Public Class PressureOut_
    Public Enum PressureOut
        Pressurised
        Depressurising
        WaitForDepressurising
        Depressurised
    End Enum

    Public Wait As New Timer
    Public C37 As Integer
    Public StartDepressuring As Boolean
    Public Sub Run()
        With ControlCode
      If (.IO.MainTemperature > .PressureOutTemp) Then
        State = PressureOut.Pressurised
      End If

      If (.IO.MainTemperature > .PressureOutTemp) Then
        Wait.TimeRemaining = 5
        C37 = .PressureOutTimes
      Else
        If Wait.Finished And (State = PressureOut.Pressurised) Then
          State = PressureOut.Depressurising
          Wait.TimeRemaining = 2
          C37 = .PressureOutTimes
        ElseIf Wait.Finished And (C37 > 0) And (State = PressureOut.Depressurising) Then
          State = PressureOut.WaitForDepressurising
          C37 -= 1
          Wait.TimeRemainingMs = .Parameters.SetPressureOutOffTime
        ElseIf Wait.Finished And (C37 > 0) And (State = PressureOut.WaitForDepressurising) Then
          State = PressureOut.Depressurising
          Wait.TimeRemainingMs = .Parameters.SetPressureOutOnTime
        ElseIf C37 = 0 Then
          State = PressureOut.Depressurised
        End If
      End If


    End With
  End Sub
#Region "Standard Definitions"

  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As PressureOut
  Public Property State() As PressureOut
    Get
      Return state_
    End Get
    Private Set(ByVal value As PressureOut)
      state_ = value
    End Set
  End Property
  Public ReadOnly Property IsPressurised() As Boolean
    Get
      Return (State = PressureOut.Pressurised)
    End Get
  End Property
  Public ReadOnly Property IsDepressurised() As Boolean
    Get
      Return (State = PressureOut.Depressurised)
    End Get
  End Property
  Public ReadOnly Property IsDepressurising() As Boolean
    Get
      Return (State = PressureOut.Depressurising)
    End Get
  End Property
  Public ReadOnly Property IsWaitForDepressurising() As Boolean
    Get
      Return (State = PressureOut.WaitForDepressurising)
    End Get
  End Property
#End Region
End Class

Partial Class ControlCode
  Public ReadOnly PressureOut As New PressureOut_(Me)
End Class
