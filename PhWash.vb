Public Class PhWash_
    Public Enum PhWash
        off
        Wash1
        Wash2
        Wash3
        Wash4
        Wash5
        Rinse1
        Add
        Rinse2
        Drain
        Finish
    End Enum
    Public StateString As String
    Public Wait, Wait1 As New Timer
    Public C37 As Integer
    Public Sub Run()
        With ControlCode
            'Usual pressure State control this will by default start at pressurized


            Select Case State
                Case PhWash.off
                    StateString = ""
                    State = PhWash.Wash1
                Case PhWash.Wash1
                    Wait.TimeRemaining = .Parameters.PhWashTime
                    State = PhWash.Wash2

                    '-------------------------------------------------------------------------------------------
                    '開PH清洗閥.IO.PhWashFill    開PH定量泵.IO.PhAddPump   
                    '開加藥馬達.IO.BTankAddPump     開總加藥閥.IO.CDosing         開BDosing .IO.BDosing
                Case PhWash.Wash2
                    StateString = If(.Language = LanguageValue.ZhTw, "PH管路清洗中!!", "PH WASH FILLING！！") & " " & TimerString(Wait.TimeRemaining)
                    If Wait.Finished Then

                        State = PhWash.Wash3
                    End If

                    '-------------------------------------------------------------------------------------------
                    '開加藥閥.IO.BTankAddition  
                    '開加藥馬達.IO.BTankAddPump     開總加藥閥.IO.CDosing         開BDosing .IO.BDosing
                Case PhWash.Wash3
                    StateString = If(.Language = LanguageValue.ZhTw, "等待B藥缸低水位!!", "Wait B Tank Low！！")
                    If .IO.BTankLow = False Then
                        ' Wait.TimeRemaining = .Parameters.AddTransferTimeBeforeRinse
                        State = PhWash.Wash4
                    End If
                    '-------------------------------------------------------------------------------------------
                    '開加藥閥.IO.BTankAddition  
                    '開加藥馬達.IO.BTankAddPump     開總加藥閥.IO.CDosing         開BDosing .IO.BDosing
                Case PhWash.Wash4
                    StateString = If(.Language = LanguageValue.ZhTw, "加藥完延遲時間!!", "Add Finish Delay Time！！") & " " & TimerString(Wait.TimeRemaining)
                    If Wait.Finished Then
                        ' Wait.TimeRemaining = .Parameters.AddTransferRinseTime
                        State = PhWash.Rinse1
                    End If
                    '-------------------------------------------------------------------------------------------
                    '開藥桶進水.IO.BTankColdFill  
                Case PhWash.Rinse1
                    StateString = If(.Language = LanguageValue.ZhTw, "B缸洗缸中", "Tank B rinsing ") & TimerString(Wait.TimeRemaining)
                    If Wait.Finished Then
                        ' Wait.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
                        State = PhWash.Add
                    End If
                    '-------------------------------------------------------------------------------------------
                    '開加藥閥.IO.BTankAddition  
                    '開加藥馬達.IO.BTankAddPump     開總加藥閥.IO.CDosing         開BDosing .IO.BDosing
                Case PhWash.Add
                    StateString = If(.Language = LanguageValue.ZhTw, "B缸加藥中", "Tank B transferring ") & TimerString(Wait.TimeRemaining)
                    'If .IO.BTankLow Then Wait.TimeRemaining = .Parameters.AddTransferTimeAfterRinse
                    If Wait.Finished Then
                        '   Wait.TimeRemaining = .Parameters.AddTransferRinseTime
                        State = PhWash.Rinse2

                        '-------------------------------------------------------------
                    End If

                Case PhWash.Rinse2
                    StateString = If(.Language = LanguageValue.ZhTw, "B缸洗缸中", "Tank B rinsing ") & TimerString(Wait.TimeRemaining)
                    If Wait.Finished Then
                        '    Wait.TimeRemaining = .Parameters.AddTransferDrainTime
                        State = PhWash.Drain
                    End If

                Case PhWash.Drain
                    StateString = If(.Language = LanguageValue.ZhTw, "B缸排水", "Tank B draining ") & TimerString(Wait.TimeRemaining)
                    'If .IO.BTankLow Then Wait.TimeRemaining = .Parameters.AddTransferDrainTime
                    If Wait.Finished Then
                        State = PhWash.Finish
                    End If
                Case PhWash.Finish

            End Select

        End With

    End Sub
    Public Sub Cancel()
        State = PhWash.off

    End Sub
#Region "Standard Definitions"

    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As PhWash
    Public Property State() As PhWash
        Get
            Return state_
        End Get
        Private Set(ByVal value As PhWash)
            state_ = value
        End Set
    End Property
    Public ReadOnly Property IsOn() As Boolean
        Get
            Return (State <> PhWash.off)
        End Get
    End Property
    Public ReadOnly Property PhWashFill() As Boolean
        Get
            Return (State = PhWash.Wash2)
        End Get
    End Property
    Public ReadOnly Property IsDosing() As Boolean
        Get
            Return (State = PhWash.Rinse1) Or (State = PhWash.Add) Or (State = PhWash.Wash4) Or (State = PhWash.Wash3)
        End Get
    End Property
    Public ReadOnly Property IsTransfer() As Boolean
        Get
            Return (State = PhWash.Rinse1) Or (State = PhWash.Add) Or (State = PhWash.Wash4) Or (State = PhWash.Wash3)
        End Get
    End Property
    Public ReadOnly Property IsTransferPump() As Boolean
        Get
            Return (State = PhWash.Rinse1) Or (State = PhWash.Add) Or (State = PhWash.Wash4) Or (State = PhWash.Wash3)
        End Get
    End Property
    Public ReadOnly Property IsRinsing() As Boolean
        Get
            Return ((State = PhWash.Rinse1) Or (State = PhWash.Rinse2))
        End Get
    End Property
    Public ReadOnly Property IsDraining() As Boolean
        Get
            Return ((State = PhWash.Rinse2) Or (State = PhWash.Drain))
        End Get
    End Property


    'Public ReadOnly Property IsPressurised() As Boolean
    '    Get
    '        Return (State = PhControl.Pressurised)
    '    End Get
    'End Property
    'Public ReadOnly Property IsDepressurised() As Boolean
    '    Get
    '        Return (State = PhControl.Depressurised)
    '    End Get
    'End Property
    'Public ReadOnly Property IsDepressurising() As Boolean
    '    Get
    '        Return (State = PhControl.Depressurising)
    '    End Get
    'End Property
    'Public ReadOnly Property IsWaitForDepressurising() As Boolean
    '    Get
    '        Return (State = PhControl.WaitForDepressurising)
    '    End Get
    'End Property
#End Region
End Class

Partial Class ControlCode
    Public ReadOnly PhWash As New PhWash_(Me)
End Class
