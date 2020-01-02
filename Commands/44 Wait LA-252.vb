<Command("Wait LA-252", , , , "15", CommandType.Standard),
TranslateCommand("zh-TW", "等待染料備藥"),
Description("Wait LA-252"),
TranslateDescription("zh-TW", "等待染料備藥")>
Public NotInheritable Class Command44
    Inherits MarshalByRefObject                       'Inheritsg是繼承Windows Form的應用程式要繼承System.Windows.Forms.Form，可先參考物件導向程式設計相關書籍
    Implements ACCommand

    Public Enum S44
        Off
        WaitAuto
        WaitLA252
        DispenseWaitResponse
    Ready
    End Enum
    Public StateString As String
    Public CallAlarm As Boolean


    Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
        With ControlCode
      .Command03.Cancel() : .Command04.Cancel() : .Command05.Cancel()
      .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel() : .Command14.Cancel()
      .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
      .Command33.Cancel() : .Command01.Cancel()
      .Command51.Cancel() : .Command52.Cancel() : .Command54.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()

            .SPCConnectError = False
      State = S44.WaitAuto
        End With
    End Function

    Public Function Run() As Boolean Implements ACCommand.Run
        With ControlCode
            Select Case State
                Case S44.Off
                    CallAlarm = False
                    StateString = ""

                Case S44.WaitAuto
                    StateString = If(.Language = LanguageValue.ZhTw, "系統手動中", "Interlocked not In auto")
                    If Not .IO.SystemAuto Then Exit Select
                    State = S44.WaitLA252

        Case S44.WaitLA252

          If .Parameters.CallFor252AddDye <> 0 And .DState = "205" And .CallLA252.IsOn Then
            .Call252AddDye = True
          End If

          If .Wait252Scheduled Then
            StateString = If(.Language = LanguageValue.ZhTw, "等待LA-252化料", "Wait LA-252 Dissolving")
          ElseIf .Call252AddDye Then
            StateString = If(.Language = LanguageValue.ZhTw, "等待LA-252加藥", "Wait LA-252 Adding")
          ElseIf Not .LA252Ready Then
            StateString = If(.Language = LanguageValue.ZhTw, "等待LA-252備藥中", "Wait LA-252 Ready")
          ElseIf .LA252Ready Then
            State = S44.DispenseWaitResponse
          End If


        Case S44.DispenseWaitResponse
          StateString = If(.Language = LanguageValue.ZhTw, "藥缸沒水，LA-252輸送異常", "Side Tank Empty, LA-252 Dispense Error")
          CallAlarm = True
          .LA252Ready = False
          If (.Parameters.DyeEnable = 1 And .BTankLowLevel) Or .IO.CallAck Then
            .TankBReady = True
            CallAlarm = False
            State = S44.Ready
          End If
          If .Parameters.DyeEnable = 2 And .CTankLowLevel Then
            .TankCReady = True
            CallAlarm = False
            State = S44.Ready
          End If

        Case S44.Ready
          .DyeCallOff = 0
          .DyeTank = 0
          State = S44.Off

      End Select

        End With
    End Function

    Public Sub Cancel() Implements ACCommand.Cancel
        State = S44.Off
    End Sub

    Public Sub ParametersChanged(ByVal ParamArray param() As Integer) Implements ACCommand.ParametersChanged

    End Sub


#Region " Standard Definitions "

    Private ReadOnly ControlCode As ControlCode
    Public Sub New(ByVal controlCode As ControlCode)
        Me.ControlCode = controlCode
    End Sub
    Friend ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
        Get
            Return State <> S44.Off
        End Get
    End Property
  Public ReadOnly Property IsCall() As Boolean
    Get
      Return (State = S44.DispenseWaitResponse) And CallAlarm
    End Get
  End Property
    Public ReadOnly Property IsReady() As Boolean
        Get
            Return (State = S44.Ready)
        End Get
    End Property
    <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S44
    Public Property State() As S44          'Property	屬性名稱() As 傳回值型別
        Get                                 'Get
            Return state_                   '屬性名稱 = 私有資料成員        '讀取私有資料成員的值
        End Get                             'End Get
        Private Set(ByVal value As S44)     'Set(ByVal Value As傳回值型別)
            state_ = value                  '私有資料成員 = Value          '設定私有資料成員的值
        End Set                             'End Set
    End Property
    'Property score() As Integer
    '    Get
    '        score = a           '讀取私有資料成員a的值
    '    End Get
    '
    '    Set(ByVal Value As Integer)
    '        a = Value           '設定私有資料成員a的值
    '    End Set
    'End Property

#End Region

End Class

#Region " Class Instance "

Partial Public Class ControlCode
    Public ReadOnly Command44 As New Command44(Me)
End Class

#End Region
