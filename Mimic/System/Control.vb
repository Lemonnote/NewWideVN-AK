#If ADAPTIVECONTROLNAMESPACE Or TARGET <> "library" Then
Namespace AdaptiveControl
#End If


Public Enum Units
  Seconds
  Minutes
  Temperature
  TemperatureTenths
End Enum

<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
Public NotInheritable Class UnitsAttribute : Inherits Attribute
  Private ReadOnly units_ As Units
  Public Sub New(ByVal value As Units)
    units_ = value
  End Sub
  Public ReadOnly Property Units() As Units
    Get
      Return units_
    End Get
  End Property
End Class

<AttributeUsage(AttributeTargets.All, AllowMultiple:=True)> _
Public NotInheritable Class TranslateAttribute : Inherits Attribute
  Private ReadOnly language_, translation_ As String
  Public Sub New(ByVal language As String, ByVal translation As String)
    language_ = language : translation_ = translation
  End Sub
  Public ReadOnly Property Language() As String
    Get
      Return language_
    End Get
  End Property
  Public ReadOnly Property Translation() As String
    Get
      Return translation_
    End Get
  End Property
End Class

  <AttributeUsage(AttributeTargets.All, AllowMultiple:=True)> _
  Public NotInheritable Class TranslateDescriptionAttribute : Inherits Attribute
    Private ReadOnly language_, description_ As String
    Public Sub New(ByVal language As String, ByVal description As String)
      language_ = language : description_ = description
    End Sub
    Public ReadOnly Property Language() As String
      Get
        Return language_
      End Get
    End Property
    Public ReadOnly Property Description() As String
      Get
        Return description_
      End Get
    End Property
  End Class

  <AttributeUsage(AttributeTargets.All, AllowMultiple:=True)> _
  Public NotInheritable Class TranslateCategoryAttribute : Inherits Attribute
    Private ReadOnly language_, category_ As String
    Public Sub New(ByVal language As String, ByVal category As String)
      language_ = language : category_ = category
    End Sub
    Public ReadOnly Property Language() As String
      Get
        Return language_
      End Get
    End Property
    Public ReadOnly Property Category() As String
      Get
        Return category_
      End Get
    End Property
  End Class

<AttributeUsage(AttributeTargets.Class, AllowMultiple:=True)> _
Public NotInheritable Class TranslateCommandAttribute : Inherits Attribute
  Private ReadOnly language_, longName_, parameters_ As String
  Public Sub New(ByVal language As String, ByVal longName As String, Optional ByVal parameters As String = "")
    language_ = language : longName_ = longName : parameters_ = parameters
  End Sub
  Public ReadOnly Property Language() As String
    Get
      Return language_
    End Get
  End Property
  Public ReadOnly Property LongName() As String
    Get
      Return longName_
    End Get
  End Property
  Public ReadOnly Property Parameters() As String
    Get
      Return parameters_
    End Get
  End Property
End Class


Public NotInheritable Class PersistAttribute : Inherits Attribute
End Class

#If TARGET = "library" Then
' A user-written command class must implement this interface
#If 0 Then
Public Interface ACCommand
  Sub Start(ByVal controlCode As ControlCode, ByRef stepOn As Boolean, ByVal ParamArray param() As Integer)
  Sub Run(ByVal controlCode As ControlCode, ByRef stepOn As Boolean)
  Sub Cancel()
  ReadOnly Property IsOn() As Boolean
End Interface
#Else
Public Interface ACCommand
  Function Start(ByVal ParamArray param() As Integer) As Boolean
  Function Run() As Boolean
  Sub Cancel()
  ReadOnly Property IsOn() As Boolean
End Interface
#End If
#End If

    ' A user-written ControlCode class must implement this interface
  Public Interface ACControlCode
    Sub StartUp()
    Sub ShutDown()
    Sub Run()
  Function ReadInputs(ByVal dinp() As Boolean, ByVal aninp() As Short, ByVal temp() As Short) As Boolean
  Sub WriteOutputs(ByVal dout() As Boolean, ByVal anout() As Short)
  Sub DrawScreen(ByVal screen As Integer, ByVal row() As String)
  Sub ProgramStart()
  Sub ProgramStop()
End Interface

  <Flags()> _
  Public Enum CommandType
    Standard = 0
    BatchParameter = 1
    ParallelCommand = 2
  End Enum

  <AttributeUsage(AttributeTargets.Assembly)> _
  Public NotInheritable Class IncludeIPAttribute : Inherits Attribute
    Private ReadOnly value_ As Boolean
  Public Sub New(ByVal value As Boolean)
    value_ = value
  End Sub
  Public ReadOnly Property Value() As Boolean
    Get
      Return value_
    End Get
  End Property
End Class

<AttributeUsage(AttributeTargets.Class)> _
  Public NotInheritable Class CommandAttribute : Inherits Attribute
  Private ReadOnly longName_ As String, parameters_ As String, _
                   gradient_ As String, target_ As String, minutes_ As String, _
                   commandType_ As CommandType

  Public Sub New(ByVal longName As String, Optional ByVal parameters As String = "", _
                 Optional ByVal gradient As String = "", Optional ByVal target As String = "", _
                 Optional ByVal minutes As String = "", Optional ByVal commandType As CommandType = CommandType.Standard)
    longName_ = longName : parameters_ = parameters
    gradient_ = gradient : target_ = target : minutes_ = minutes
    commandType_ = commandType
  End Sub

  Public ReadOnly Property LongName() As String
    Get
      Return longName_
    End Get
  End Property
  Public ReadOnly Property Parameters() As String
    Get
      Return parameters_
    End Get
  End Property
  Public ReadOnly Property Gradient() As String
    Get
      Return gradient_
    End Get
  End Property
  Public ReadOnly Property Target() As String
    Get
      Return target_
    End Get
  End Property
  Public ReadOnly Property Minutes() As String
    Get
      Return minutes_
    End Get
  End Property
  Public ReadOnly Property CommandType() As CommandType
    Get
      Return commandType_
    End Get
  End Property
End Class

  <AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property)> _
  Public NotInheritable Class GraphTraceAttribute : Inherits Attribute
    Private ReadOnly minimumValue_ As Integer, maximumValue_ As Integer, minimumY_ As Integer, maximumY_ As Integer, _
                     colorName_, format_ As String
    Public Sub New(ByVal minimumValue As Integer, ByVal maximumValue As Integer, _
                   Optional ByVal minimumY As Integer = 0, Optional ByVal maximumY As Integer = 10000, _
                   Optional ByVal colorName As String = "", Optional ByVal format As String = "")
      minimumValue_ = minimumValue : maximumValue_ = maximumValue
      minimumY_ = minimumY : maximumY_ = maximumY
    colorName_ = colorName : format_ = format
    End Sub

    Public ReadOnly Property MinimumValue() As Integer
      Get
        Return minimumValue_
      End Get
    End Property
    Public ReadOnly Property MaximumValue() As Integer
      Get
        Return maximumValue_
      End Get
    End Property

    Public ReadOnly Property MinimumY() As Integer
      Get
        Return minimumY_
      End Get
    End Property
    Public ReadOnly Property MaximumY() As Integer
      Get
        Return maximumY_
      End Get
    End Property
    Public ReadOnly Property ColorName() As String
      Get
        Return colorName_
      End Get
    End Property
    Public ReadOnly Property Format() As String
      Get
        Return format_
      End Get
    End Property
  End Class

  <AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property, AllowMultiple:=True)> _
  Public NotInheritable Class GraphLabelAttribute : Inherits Attribute
    Private ReadOnly label_ As String, value_ As Integer
    Public Sub New(ByVal label As String, ByVal value As Integer)
      label_ = label : value_ = value
    End Sub

    Public ReadOnly Property Label() As String
      Get
        Return label_
      End Get
    End Property
    Public ReadOnly Property Value() As Integer
      Get
        Return value_
      End Get
    End Property
  End Class

  Public Enum IOType
    Normal
    Timer
    Dinp
    Dout
    Aninp
    Anout
    Temp
    Pid
    Counter
    Unknown = 255
  End Enum

Public Enum Override
  Prevent
  Allow
End Enum

<AttributeUsage(AttributeTargets.Field)> _
  Public NotInheritable Class IOAttribute : Inherits Attribute
  Private ReadOnly ioType_ As IOType, allowOverride_ As Override, icon_, format_ As String
  Private channel_ As Integer

  Public Sub New(ByVal ioType As IOType, ByVal channel As Integer, _
                 Optional ByVal allowOverride As Override = Override.Allow, _
                 Optional ByVal icon As String = "", Optional ByVal format As String = "")
    ioType_ = ioType : channel_ = channel : allowOverride_ = allowOverride
    icon_ = icon : format_ = format
  End Sub

  Public ReadOnly Property IOType() As IOType
    Get
      Return ioType_
    End Get
  End Property
  Public Property Channel() As Integer
    Get
      Return channel_
    End Get
    Set(ByVal value As Integer)
      channel_ = value
    End Set
  End Property
  Public ReadOnly Property AllowOverride() As Override
    Get
      Return allowOverride_
    End Get
  End Property
  Public ReadOnly Property Icon() As String
    Get
      Return icon_
    End Get
  End Property
  Public ReadOnly Property Format() As String
    Get
      Return format_
    End Get
  End Property
End Class

  <AttributeUsage(AttributeTargets.Field)> _
  Public NotInheritable Class IOChannelIncrementAttribute : Inherits Attribute
    Private ReadOnly increment_ As Integer

    Public Sub New(ByVal increment As Integer)
      increment_ = increment
    End Sub

    Public ReadOnly Property Increment() As Integer
      Get
        Return increment_
      End Get
    End Property
  End Class



  <AttributeUsage(AttributeTargets.Field)> _
  Public NotInheritable Class ParameterAttribute : Inherits Attribute
    Private ReadOnly minimumValue_ As Integer, maximumValue_ As Integer

    Public Sub New(ByVal minimumValue As Integer, ByVal maximumValue As Integer)
      minimumValue_ = minimumValue : maximumValue_ = maximumValue
    End Sub

    Public ReadOnly Property MinimumValue() As Integer
      Get
        Return minimumValue_
      End Get
    End Property
    Public ReadOnly Property MaximumValue() As Integer
      Get
        Return maximumValue_
      End Get
    End Property
  End Class

  Public Enum ButtonImage
    Vessel = 1
    Dial
    Thermometer
    FillBucket
    SideVessel
    Beam
    Information
  End Enum

  <AttributeUsage(AttributeTargets.Method, AllowMultiple:=True)> _
  Public NotInheritable Class ScreenButtonAttribute
    Inherits Attribute
    Private ReadOnly text_ As String, order_ As Integer, image_ As ButtonImage

    Public Sub New(ByVal text As String, ByVal order As Integer, ByVal image As ButtonImage)
      text_ = text : order_ = order : image_ = image
    End Sub
    Public ReadOnly Property Text() As String
      Get
        Return text_
      End Get
    End Property
    Public ReadOnly Property Order() As Integer
      Get
        Return order_
      End Get
    End Property
    Public ReadOnly Property Image() As ButtonImage
      Get
        Return image_
      End Get
    End Property
  End Class

' --------------------------------------

Public Enum AckStateValue
  Off
  UnackMessage
  AckMessage
  QQ
  MessageIsDone
  QQIsDoneAndAnswerIsYes
  QQIsDoneAndAnswerIsNo
End Enum
Public Enum ButtonPosition
  [Operator]
  Expert
End Enum
Public Enum LogEventType
  [Error] = 2
  Warning = 4
  Information = 8
End Enum
Public Enum Mode
  Run
  Debug
  Test
  Override
End Enum
Public Enum Running
  NotRunning
  RunningButPaused
  RunningNow
End Enum
Public Enum StandardButton
  WorkList
  Program
  Graph
  Mimic
  History
  IO
  Variables
  Parameters
  Programs
  Clean
  Sleep
  Ladder
End Enum

  Public Interface ACParent

    ReadOnly Property AckState() As AckStateValue

    ' The text currently showing on the button used for messages, etc.
    ReadOnly Property ButtonText() As String

    ' Set this to any message you want to signal to the operator. Becomes an empty string again when the operator acknowledges the message.
    Property Signal() As String

    ReadOnly Property UnacknowledgedAlarms() As String

    ' Call this to simulate pressing the Run, Pause, Stop, Yes AndAlso No buttons.  Only when the values go from false to true are they acted on.
    Sub PressButtons(ByVal run As Boolean, ByVal pause As Boolean, ByVal [stop] As Boolean, ByVal yes As Boolean, ByVal no As Boolean)

    ' True if a program (=job) is running right now. You can also get this by checking the 'Running' property.
    ReadOnly Property IsProgramRunning() As Boolean

    ' True if a job is running right now, but it has been paused. You can also get this by checking the 'Running' property
    ReadOnly Property IsPaused() As Boolean

    ' True if an unacknowledged signal is active
    ReadOnly Property IsSignalUnacknowledged() As Boolean

    ' True if an unacknowledged alarm is active
    ReadOnly Property IsAlarmUnacknowledged() As Boolean

    ' The message of given number 1 through 99
    ReadOnly Property Message(ByVal messageNumber As Integer) As String

    Property Mode() As Mode

    ' The number of the program that the sequencer has reached.  Be aware that this will change if the sequencer is paused AndAlso the step is being changed
    ReadOnly Property ProgramNumber() As Integer

    ' The name of the program that the sequencer has reached.  Be aware that this will change if the sequencer is paused AndAlso the step is being changed
    ReadOnly Property ProgramName() As String

    ' The number of the step that the sequencer has reached.  Be aware that this will change if the sequencer is paused AndAlso the step is being changed.
    ReadOnly Property StepNumber() As Integer

    ' The text of the step that the sequencer has reached.  Be aware that this will change if the sequencer is paused AndAlso the step is being changed
    ReadOnly Property StepText() As String

    ' How long the sequencer has spent in the current step versus how long it is supposed to spend there in total, e.g. '10 / 15'
    ReadOnly Property TimeInStep() As String

    ' The current step number, relative to all steps in all programs
    ReadOnly Property CurrentStep() As Integer

    ' The current changing step number, relative to all steps in all programs
    ReadOnly Property ChangingStep() As Integer

    ' The name of the job being run
    ReadOnly Property Job() As String

    ReadOnly Property PrefixedSteps() As String
    ReadOnly Property Messages() As String
    ReadOnly Property Programs() As String
    ReadOnly Property TimeInSteps() As String

    ReadOnly Property ElapsedTime() As TimeSpan
    ReadOnly Property ElapsedTimeExpected() As TimeSpan
    ReadOnly Property RemainingTime() As TimeSpan


    ' True if any alarm is active, whether OrElse not it is acknowledged
    ReadOnly Property IsAlarmActive() As Boolean

    ' The numbers of all programs that were specified when the current job was started
    ReadOnly Property ProgramNumbers() As Collections.ObjectModel.ReadOnlyCollection(Of Integer)

    ' The names of all active alarms, separated by commas
    ReadOnly Property ActiveAlarms() As String

    ' True if the system is sleeping
    ReadOnly Property IsSleeping() As Boolean


    ' These are used to get access to other control systems that may be running at the same time as us in our own world
    ReadOnly Property ControlSystemName() As String
    ReadOnly Property ControlSystemNumber() As Integer
    ReadOnly Property ControlSystem(ByVal index As Integer) As Object
    ' Try to find a control system on the network
    Sub GetControlSystem(ByVal connect As String, ByVal owner As Control, ByVal onGotControlSystem As Action(Of Object))
    ' Set the values of named properties
    Sub SetControlSystemValues(ByVal controlSystem As Object, ByVal ParamArray namesAndValues() As Object)


    ' These are for coupling
    ReadOnly Property CouplingMaster() As Object
    ReadOnly Property Coupled() As Boolean
    ReadOnly Property CouplingCombination() As Integer
    WriteOnly Property CouplingReady() As Boolean
    ReadOnly Property CouplingAllReady() As Boolean
    WriteOnly Property CouplingSafe() As Boolean
    ReadOnly Property CouplingAllSafe() As Boolean

    ' TODO: button was ToolStripItem
    Sub AddButton(ByVal button As Object, ByVal position As ButtonPosition, Optional ByVal view As Control = Nothing)
    Sub AddStandardButton(ByVal button As StandardButton, ByVal position As ButtonPosition, Optional ByVal text As String = Nothing, _
                          Optional ByVal options As Integer = 0)
    Sub PressButton(ByVal position As ButtonPosition, ByVal index As Integer)
    Sub SetButtonVisible(ByVal position As ButtonPosition, ByVal index As Integer, ByVal visible As Boolean)
    Sub SetExpertButtonVisible(ByVal visible As Boolean)
    Sub SetStatusView(ByVal value As Control)

    Function DbGetDataTable(ByVal sql As String) As DataTable
    Function DbExecute(ByVal sql As String, ByVal ParamArray parameterNamesAndValues() As Object) As Integer
    Function DbSqlString(ByVal value As Object) As String
    Sub DbUpdateSchema(ByVal sql As String)

    Sub StartJob(ByVal job As String, ByVal programNumbers As IEnumerable(Of Integer), _
                 Optional ByVal substituteSteps As IEnumerable(Of String) = Nothing)
    Sub StopJob()
    Sub NonStandardDb()
    Function CreateView(ByVal typeName As String) As Control
    Function CreateHistory(ByVal bytes() As Byte) As Object
    Function CreateControlCodeProxy(ByVal dataSource As Object) As Object

    ReadOnly Property History() As Object  ' the live history (if running)

    Function CreateProxy(ByVal classToProxy As Type, ByVal dataSource As Object) As Object

#If 0 Then
  Function GetValues(ByVal typ As Type, ByVal dataSource As Object, ByVal autoRefreshInterval As Integer, _
                     ByVal owner As Control, ByVal onGotValues As Action(Of Object)) As IDisposable
#End If
    Function GetValues(ByVal propertyNames() As String, ByVal dataSource As Object, ByVal autoRefreshInterval As Integer, _
                       ByVal owner As Control, ByVal onGotValues As Action(Of Object)) As IDisposable

    Sub LogEvent(ByVal eventType As LogEventType, ByVal id As Object, ByVal message As String)
    Sub LogException(ByVal ex As Exception)
    ReadOnly Property Setting(ByVal name As String) As String
    Sub SetIOChannel(ByVal name As String, ByVal channel As Integer)

    ReadOnly Property CultureName() As String
  End Interface

#If ADAPTIVECONTROLNAMESPACE Or TARGET <> "library" Then
End Namespace
#End If


#If CF Then
      ' These are missing from the CF libraries, so we define them here instead
      '<AttributeUsage(AttributeTargets.Field)> _
      Public NotInheritable Class DescriptionAttribute : Inherits Attribute
        Private ReadOnly description_ As String
        Public Sub New(ByVal description As String)
          description_ = description
        End Sub
        Public ReadOnly Property Description() As String
          Get
            Return description_
          End Get
        End Property
      End Class

      '<AttributeUsage(AttributeTargets.Field)> _
      Public NotInheritable Class CategoryAttribute : Inherits Attribute
        Private ReadOnly category_ As String
        Public Sub New(ByVal category As String)
          category_ = category
        End Sub
        Public ReadOnly Property Category() As String
          Get
            Return category_
          End Get
        End Property
      End Class
#End If