Public Class PhCirculateRun_
    Public Enum PhCirculateRun
        Off
        Run
        Run1
        Run2
        Run3
        Run4
        Wait
        Wait1
        test
    End Enum

    Public Wait, WaitLevel As New Timer
    Public C37, DoseOutput As Integer

    Public Sub Run()
        With ControlCode
            Select Case State
                '------------------------------------------------------起始動作
                Case PhCirculateRun.Off
                    DoseOutput = 0
                    State = PhCirculateRun.Run

                    '--------------------------------------------------判斷目前動作
                Case PhCirculateRun.Run

                    If .IO.BTankHigh Then
                        WaitLevel.TimeRemaining = 30
                        State = PhCirculateRun.Run3                     '如有B缸高水位，跳 PhCirculateRun.Run3
                    ElseIf .IO.BTankMiddle Or .IO.TankBLevel > (.Parameters.CirculateOpenAdd * 10) Then
                        WaitLevel.TimeRemaining = 30
                        State = PhCirculateRun.Run4                     '如有B缸高水位，跳 PhCirculateRun.Run3
                    ElseIf .IO.BTankLow Then
                        State = PhCirculateRun.Run1                     '如有B缸低水位，跳 PhCirculateRun.Run1 
                    Else
                        State = PhCirculateRun.Run2                     '如沒B缸低水位，跳 PhCirculateRun.Run2 
                    End If

                    '---------------------------------------------------等待低水位消失（開馬達、下料閥、加藥閥、開迴水、開大加藥閥）
                Case PhCirculateRun.Run4
                    '-------------------------------------------設定藥缸馬達AO轉速
                    If .Parameters.AddPercent = 0 Then
                        DoseOutput = 50
                    Else
                        DoseOutput = .Parameters.AddPercent
                    End If
                    '-------------------------------------------

                    If WaitLevel.Finished Then

                        State = PhCirculateRun.Run
                    Else
                        If .IO.BTankLow = False Then
                            State = PhCirculateRun.Run2
                        Else
                            Exit Select
                        End If


                    End If
                    '---------------------------------------------------等待低水位消失（開馬達、下料閥、加藥閥）
                Case PhCirculateRun.Run3
                    '-------------------------------------------設定藥缸馬達AO轉速
                    If .Parameters.AddPercent = 0 Then
                        DoseOutput = 50
                    Else
                        DoseOutput = .Parameters.AddPercent
                    End If
                    '-------------------------------------------
                    If Not WaitLevel.Finished Then Exit Select

                    State = PhCirculateRun.Run
                    '---------------------------------------------------等待低水位消失（開馬達、下料閥、加藥閥、開迴水）
                Case PhCirculateRun.Run1
                    '-------------------------------------------設定藥缸馬達AO轉速
                    If .Parameters.AddPercent = 0 Then
                        DoseOutput = 50
                    Else
                        DoseOutput = .Parameters.AddPercent
                    End If
                    '-------------------------------------------
                    If .IO.BTankHigh = True Then
                        State = PhCirculateRun.Run
                    ElseIf .IO.BTankMiddle = True Or .IO.TankBLevel > (.Parameters.CirculateOpenAdd * 10) Then
                        State = PhCirculateRun.Run
                    ElseIf .IO.BTankLow = False Then
                        State = PhCirculateRun.Run2
                    End If

                    '---------------------------------------------------等待低水位到達(開迴水)
                Case PhCirculateRun.Run2
                    DoseOutput = 0
                    If .IO.BTankLow Then

                        Wait.TimeRemaining = .Parameters.CirFillDelayTime
                        State = PhCirculateRun.Wait
                    End If

                    '---------------------------------------------------等待低水位到達(開迴水)
                Case PhCirculateRun.Wait
                    DoseOutput = 0
                    If Wait.Finished Then
                        State = PhCirculateRun.Run
                    End If

                    '------------------------------------------------------

            End Select


        End With

    End Sub
    Public Sub Cancel()
        State = PhCirculateRun.Off

    End Sub
#Region "Standard Definitions"

    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    Friend ReadOnly Property IsOn() As Boolean
        Get
            Return State <> PhCirculateRun.Off
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As PhCirculateRun
    Public Property State() As PhCirculateRun
        Get
            Return state_
        End Get
        Private Set(ByVal value As PhCirculateRun)
            state_ = value
        End Set
    End Property

    Public ReadOnly Property BTankAddPump() As Boolean          'B藥缸馬達
        Get
            Return (State = PhCirculateRun.Run1) Or (State = PhCirculateRun.Run3) Or (State = PhCirculateRun.Run4)
        End Get
    End Property
    Public ReadOnly Property CDosing() As Boolean          '大加藥閥
        Get
            Return (State = PhCirculateRun.Run3) Or (State = PhCirculateRun.Run4)
        End Get
    End Property

    Public ReadOnly Property PhInToMachine() As Boolean               '入染機
        Get
            Return (State = PhCirculateRun.Run1) Or (State = PhCirculateRun.Run3) Or (State = PhCirculateRun.Run4)
        End Get
    End Property

    Public ReadOnly Property PhFillCirculate() As Boolean         'PH迴水
        Get
            Return (State = PhCirculateRun.Run2) Or (State = PhCirculateRun.Wait) Or (State = PhCirculateRun.Run1) Or (State = PhCirculateRun.Run4)
        End Get
    End Property

    Public ReadOnly Property PhFillCirculate2() As Boolean         'PH迴水
        Get
            Return (State = PhCirculateRun.Run2) Or (State = PhCirculateRun.Wait) Or (State = PhCirculateRun.Run1) Or (State = PhCirculateRun.Run3) Or (State = PhCirculateRun.Run4)
        End Get
    End Property

    Public ReadOnly Property BTankAddition() As Boolean         'B缸下料閥
        Get
            Return (State = PhCirculateRun.Run1) Or (State = PhCirculateRun.Run3) Or (State = PhCirculateRun.Run4)
        End Get
    End Property

#End Region
End Class

Partial Class ControlCode
    Public ReadOnly PhCirculateRun As New PhCirculateRun_(Me)
End Class
