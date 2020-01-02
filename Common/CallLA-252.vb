
Public Class CallLA252_
  Public Enum CallLA252
    off
    CheckReady
    DispenseWaitReady
    DispenseWaitScheduled
    WaitDispenseReady
    Ready
  End Enum
  Public StateString As String

  Public CallLA252Step As Integer
  Public WaitTimer As New Timer

    Public Sub Run()
        With ControlCode
            Select Case State
                Case CallLA252.off
                    StateString = ""
                    If .RunCallLA252 Then
                        CallLA252Step = .Command43.CallOff
                    Else
                        Exit Select
                    End If
                    State = CallLA252.CheckReady

                Case CallLA252.CheckReady
                    StateString = ""
                    If .LA252Ready = True Then
                        State = CallLA252.Ready
                    Else
                        State = CallLA252.DispenseWaitReady
                    End If

                Case CallLA252.DispenseWaitReady
                    'TODO  Add timeout code to switch to manual if no response
                    If .DyeState = EDispenseState.Ready Then
                        'Dispenser is ready so set CallOff number and wait for result
                        .DyeCallOff = CallLA252Step
                        .DyeTank = 1
                        State = CallLA252.DispenseWaitScheduled
                    End If
                    'Switch to manual if enable parameter is changed or calloff is reset
                    If .Parameters.DyeEnable = 0 Then State = CallLA252.Ready
                    If CallLA252Step = 0 Then State = CallLA252.Ready

        Case CallLA252.DispenseWaitScheduled
          Select Case .DyeState
            Case EDispenseState.Ready
              .Wait252Scheduled = False
              State = CallLA252.DispenseWaitScheduled

            Case EDispenseState.Auto
              .Wait252Scheduled = True
              State = CallLA252.DispenseWaitScheduled

            Case EDispenseState.Scheduled
              .Wait252Scheduled = True
              State = CallLA252.DispenseWaitScheduled

            Case EDispenseState.WaitDissolve
              .Wait252Scheduled = False
              .Call252AddDye = True
              State = CallLA252.DispenseWaitScheduled

            Case EDispenseState.Complete
              .Call252AddDye = False
              .Wait252Scheduled = False
              State = CallLA252.WaitDispenseReady

            Case EDispenseState.Error
              .Call252AddDye = False
              .Wait252Scheduled = False
              State = CallLA252.WaitDispenseReady

            Case EDispenseState.Manual
              .Call252AddDye = False
              .Wait252Scheduled = False
              State = CallLA252.WaitDispenseReady

          End Select

                Case CallLA252.WaitDispenseReady
                    If .Parameters.DyeEnable = 1 And .BTankLowLevel Then
                        ' WaitTimer.TimeRemaining = .Parameters.DyeTransferDelayTime
                        State = CallLA252.Ready
                    End If
                    If .Parameters.DyeEnable = 2 And .CTankLowLevel Then
                        ' WaitTimer.TimeRemaining = .Parameters.DyeTransferDelayTime
                        State = CallLA252.Ready
                    End If


                Case CallLA252.Ready
                    If Not WaitTimer.Finished Then Exit Select
                    .LA252Ready = True
                    .RunCallLA252 = False
                    .Call252AddDye = False
                    State = CallLA252.off


            End Select

        End With
    End Sub

#Region " Standard Definitions "
  Public Sub Cancel()
    With ControlCode
      State = CallLA252.off
      CallLA252Step = 0
      .LA252Ready = False
      .RunCallLA252 = False
    End With
  End Sub

  Private ReadOnly ControlCode As ControlCode
  Public Sub New(ByVal controlCode As ControlCode)
    Me.ControlCode = controlCode
  End Sub
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As CallLA252
  Public Property State() As CallLA252
    Get
      Return state_
    End Get
    Private Set(ByVal value As CallLA252)
      state_ = value
    End Set
  End Property
  Public ReadOnly Property IsOn() As Boolean
    Get
      Return (State <> CallLA252.off)
    End Get
  End Property
  Public ReadOnly Property IsReady() As Boolean
    Get
      Return (State = CallLA252.Ready)
    End Get
  End Property

#End Region

End Class

Partial Class ControlCode
  Public ReadOnly CallLA252 As New CallLA252_(Me)
End Class
