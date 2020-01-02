<Command("Call LA-301P", "Call off |0-99|", , , "1", CommandType.Standard),
TranslateCommand("zh-TW", "呼叫粉體助劑備藥", "呼叫粉體助劑備藥 |0-99|"),
Description("0-99 Call off"),
TranslateDescription("zh-TW", "0-99 呼叫粉體助劑備藥")>
Public NotInheritable Class Command45
  Inherits MarshalByRefObject                       'Inheritsg是繼承Windows Form的應用程式要繼承System.Windows.Forms.Form，可先參考物件導向程式設計相關書籍
  Implements ACCommand

  Public Enum S45
    Off
    CheckLevel
    WaitAuto
    Ready
  End Enum

  Public StateString As String
  Public CallOff As Integer
  Public WaitTimer As New Timer
  Public BTankDrain As Boolean
  Public CTankDrain As Boolean

  Public Function Start(ByVal ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With ControlCode
      'cancels for all other forground functions
      .Command03.Cancel() : .Command04.Cancel() : .Command05.Cancel()
      .Command11.Cancel() : .Command12.Cancel() : .Command13.Cancel() : .Command14.Cancel()
      .Command16.Cancel() : .Command20.Cancel() : .Command31.Cancel() : .Command32.Cancel()
      .Command33.Cancel() : .Command01.Cancel()
      .Command51.Cancel() : .Command52.Cancel() : .Command54.Cancel()
      .Command55.Cancel() : .Command56.Cancel() : .Command57.Cancel()
      .Command41.Cancel() : .Command42.Cancel() : .Command43.Cancel() : .Command44.Cancel()
      .Command45.Cancel() : .Command46.Cancel()


      .ChemicalCallOff = 0   'Starts the handshake with the host / auto dispenser
      .ChemicalTank = 0
      .RunCallLA302 = False
      .CallLA302.Cancel()

      CallOff = param(1)
      .SPCConnectError = False
      .UpdatePowderDispenseResult = False
      WaitTimer.TimeRemaining = 0
      BTankDrain = False
      CTankDrain = False
      State = S45.CheckLevel
    End With
  End Function


  Public Function Run() As Boolean Implements ACCommand.Run
    With ControlCode
      Select Case State
        Case S45.Off
          BTankDrain = False
          CTankDrain = False
          StateString = ""

        Case S45.CheckLevel
          If .Parameters.ChemicalEnable = 1 Then
            StateString = If(.Language = LanguageValue.ZhTw, "B缸有水, 排水中", "B Tank Not Empty, Draining")
            BTankDrain = True
            If .BTankLowLevel Then WaitTimer.TimeRemaining = .Parameters.AddTransferDrainTime
            If WaitTimer.Finished Then
              BTankDrain = False
              WaitTimer.TimeRemaining = 3
              State = S45.WaitAuto
            End If
          End If
          If .Parameters.ChemicalEnable = 2 Then
            StateString = If(.Language = LanguageValue.ZhTw, "C缸有水, 排水中", "C Tank Not Empty, Draining")
            CTankDrain = True
            If .CTankLowLevel Then WaitTimer.TimeRemaining = .Parameters.AddTransferDrainTime
            If WaitTimer.Finished Then
              CTankDrain = False
              WaitTimer.TimeRemaining = 3
              State = S45.WaitAuto
            End If
          End If

        Case S45.WaitAuto
          .ChemicalCallOff = 0   'Starts the handshake with the host / auto dispenser
          .ChemicalTank = 0
          StateString = If(.Language = LanguageValue.ZhTw, "呼叫助劑備藥", "Call For Chemicals")
          If Not .IO.SystemAuto Then Exit Select
          If Not WaitTimer.Finished Then Exit Select
          If Not .ChemicalState = 101 Then Exit Select
          State = S45.Ready

        Case S45.Ready
          .CallFor302D = True
          .粉體步驟 = CallOff
          .粉體藥桶 = 1
          .ChemicalCallOff = 0   'Starts the handshake with the host / auto dispenser
          .ChemicalTank = 0
          .RunCallLA302 = True
          State = S45.Off


      End Select
    End With
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = S45.Off
    WaitTimer.Cancel()
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
      Return State <> S45.Off
    End Get
  End Property
  Public ReadOnly Property IsBTankDrain() As Boolean
    Get
      Return (State = S45.CheckLevel) And (BTankDrain)
    End Get
  End Property
  Public ReadOnly Property IsCTankDrain() As Boolean
    Get
      Return (State = S45.CheckLevel) And (CTankDrain)
    End Get
  End Property
  Public ReadOnly Property IsNotReady() As Boolean
    Get
      Return (State = S45.CheckLevel)
    End Get
  End Property
  Public ReadOnly Property IsNotEmpty() As Boolean
    Get
      Return (State = S45.CheckLevel)
    End Get
  End Property
  <EditorBrowsable(EditorBrowsableState.Advanced)> Private state_ As S45
  Public Property State() As S45          'Property	屬性名稱() As 傳回值型別
    Get                                 'Get
      Return state_                   '屬性名稱 = 私有資料成員        '讀取私有資料成員的值
    End Get                             'End Get
    Private Set(ByVal value As S45)     'Set(ByVal Value As傳回值型別)
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
  Public ReadOnly Command45 As New Command45(Me)
End Class

#End Region
