<Command("Wait LA-301P", , , , "15", CommandType.Standard),
TranslateCommand("zh-TW", "等待粉體助劑備藥"),
Description("Wait LA-303"),
TranslateDescription("zh-TW", "等待粉體助劑備藥")>
Public NotInheritable Class Command46
  Inherits MarshalByRefObject                       'Inheritsg是繼承Windows Form的應用程式要繼承System.Windows.Forms.Form，可先參考物件導向程式設計相關書籍
  Implements ACCommand

  Public Enum S46
    Off
    WaitAuto
    WaitLA302
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
      .Command45.Cancel() : .Command46.Cancel()
      .Command01.Cancel() : .Command02.Cancel()

      .SPCConnectError = False

      State = S46.WaitAuto
    End With
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S46.Off
          CallAlarm = False
          StateString = ""

        Case S46.WaitAuto
          StateString = If(.Language = LanguageValue.ZhTw, "系統手動中", "Interlocked not In auto")
          If Not .IO.SystemAuto Then Exit Select
          State = S46.WaitLA302

        Case S46.WaitLA302

          If .RunCallLA302 Then
            If .WaitFor302D Then
              StateString = If(.Language = LanguageValue.ZhTw, "呼叫粉體助劑，請操作LA-301P", "Prepare Powder, Wait LA-301P")
              State = S46.WaitLA302
            Else
              State = S46.DispenseWaitResponse
            End If
          Else
            StateString = If(.Language = LanguageValue.ZhTw, "沒有呼叫粉體助劑", "No Call LA-301P")
            State = S46.WaitLA302
          End If

        Case S46.DispenseWaitResponse
          StateString = If(.Language = LanguageValue.ZhTw, "呼叫粉體助劑，請操作LA-301P", "Prepare Powder, Wait LA-301P")
          .LA302Ready = False
          CallAlarm = True
          If .Parameters.ChemicalEnable = 1 Then
            CallAlarm = False
            .TankBReady = True
            State = S46.Ready
          End If
          If .Parameters.ChemicalEnable = 2 Then
            CallAlarm = False
            .TankCReady = True
            State = S46.Ready
          End If

        Case S46.Ready
          .ChemicalCallOff = 0
          .ChemicalTank = 0
          State = S46.Off

      End Select

    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S46.Off
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
      Return State <> S46.Off
    End Get
  End Property
  Public ReadOnly Property IsCall() As Boolean
    Get
      Return (State = S46.DispenseWaitResponse) And CallAlarm
    End Get
  End Property
  Public ReadOnly Property IsReady() As Boolean
    Get
      Return (State = S46.Ready)
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S46
  Public Property State() As S46          'Property	屬性名稱() As 傳回值型別
    Get                                 'Get
      Return state_                   '屬性名稱 = 私有資料成員        '讀取私有資料成員的值
    End Get                             'End Get
    Private Set(ByVal value As S46)     'Set(ByVal Value As傳回值型別)
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
  Public ReadOnly Command46 As New Command46(Me)
End Class

#End Region
