
Public Class CallLA302_
    Public Enum CallLA302
        off
        CheckReady
        DispenseWaitReady
        DispenseWaitScheduled
        WaitDispenseReady
        Ready
    End Enum
    Public StateString As String

    Public CallLA302Step As Integer
    Public WaitTimer As New Timer

  Public Sub Run()
    With ControlCode
      Select Case State
        Case CallLA302.off
          StateString = ""
          If .CallFor302D Then
            CallLA302Step = .Command45.CallOff
          ElseIf .RunCallLA302 Then
            CallLA302Step = .Command41.CallOff
          Else
            Exit Select
          End If

          State = CallLA302.CheckReady

        Case CallLA302.CheckReady
          StateString = ""
          If .LA302Ready = True Then
            State = CallLA302.Ready
          ElseIf .CallFor302D Then
            .WaitFor302D = True
            State = CallLA302.DispenseWaitReady
          Else
            State = CallLA302.DispenseWaitReady
          End If

        Case CallLA302.DispenseWaitReady
          'TODO  Add timeout code to switch to manual if no response
          If .ChemicalState = EDispenseState.Ready Then
            'Dispenser is ready so set CallOff number and wait for result
            .ChemicalCallOff = CallLA302Step
            .ChemicalTank = 1
            State = CallLA302.DispenseWaitScheduled
          End If
          'Switch to manual if enable parameter is changed or calloff is reset
          If .Parameters.ChemicalEnable = 0 Then State = CallLA302.Ready
          If CallLA302Step = 0 Then State = CallLA302.Ready

        Case CallLA302.DispenseWaitScheduled
          Select Case .ChemicalState
            Case EDispenseState.Complete
              State = CallLA302.WaitDispenseReady
            Case EDispenseState.Error
              State = CallLA302.WaitDispenseReady
            Case EDispenseState.Manual
              State = CallLA302.WaitDispenseReady
          End Select

        Case CallLA302.WaitDispenseReady
          If .WaitFor302D Then
            If .Parameters.ChemicalEnable = 1 Then
              State = CallLA302.Ready
            End If
            If .Parameters.ChemicalEnable = 2 Then
              State = CallLA302.Ready
            End If
          Else
            If .Parameters.ChemicalEnable = 1 And .BTankLowLevel Then
              ' WaitTimer.TimeRemaining = .Parameters.ChemicalTransferDelayTime
              State = CallLA302.Ready

            End If
            If .Parameters.ChemicalEnable = 2 And .CTankLowLevel Then
              ' WaitTimer.TimeRemaining = .Parameters.ChemicalTransferDelayTime
              State = CallLA302.Ready
            End If
          End If



        Case CallLA302.Ready
          If Not WaitTimer.Finished Then Exit Select
          .LA302Ready = True
          .RunCallLA302 = False
          .WaitFor302D = False
          .UpdatePowderDispenseResult = False
          State = CallLA302.off


      End Select

    End With
  End Sub

#Region " Standard Definitions "
    Public Sub Cancel()
        With ControlCode
            State = CallLA302.off
            CallLA302Step = 0
            .LA302Ready = False
            .RunCallLA302 = False
        End With
    End Sub

    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As CallLA302
    Public Property State() As CallLA302
        Get
            Return state_
        End Get
        Private Set(ByVal value As CallLA302)
            state_ = value
        End Set
    End Property
    Public ReadOnly Property IsOn() As Boolean
        Get
            Return (State <> CallLA302.off)
        End Get
    End Property
    Public ReadOnly Property IsReady() As Boolean
        Get
            Return (State = CallLA302.Ready)
        End Get
    End Property

#End Region

End Class

Partial Class ControlCode
    Public ReadOnly CallLA302 As New CallLA302_(Me)
End Class
