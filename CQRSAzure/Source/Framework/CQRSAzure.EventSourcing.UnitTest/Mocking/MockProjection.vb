Option Infer On

Imports Newtonsoft.Json.Linq

Namespace Mocking
    ''' <summary>
    ''' Simplest mock projection - just updates a total pro[perty
    ''' </summary>
    <DomainName("UnitTest")>
    Public Class MockProjection_Simple
        Inherits ProjectionBase(Of MockAggregate, String)
        Implements IProjection(Of MockAggregate, String)


        Public Overrides ReadOnly Property CurrentAsOfDate As Date Implements IProjection.CurrentAsOfDate
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsSnapshots As Boolean Implements IProjection.SupportsSnapshots
            Get
                Return False
            End Get
        End Property

        Public Overrides Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent) Implements IProjection(Of MockAggregate, String).HandleEvent

            Select Case eventToHandle.GetType()
                Case GetType(MockEventTypeOne)
                    HandleMockEventOne(Convert.ChangeType(eventToHandle, GetType(MockEventTypeOne)))
                Case Else
                    'Nothing to do with this event type
                    Throw New ArgumentException("Unexpected event type - " & eventToHandle.GetType().Name)
            End Select

        End Sub

        Private Sub HandleMockEventOne(ByVal eventToHandle As MockEventTypeOne)

            AddOrUpdateValue(Of Integer)(NameOf(Total), ProjectionSnapshotProperty.NO_ROW_NUMBER, Total + eventToHandle.EventOneIntegerProperty)

        End Sub


#Region "Output"

        ''' <summary>
        ''' The total number of the numbers in this event stream
        ''' </summary>
        Public ReadOnly Property Total As Integer
            Get
                Return GetPropertyValue(Of Integer)(NameOf(Total))
            End Get
        End Property



#End Region



        ''' <summary>
        ''' This mock projection only handles events of type MockEventTypeOne
        ''' </summary>
        ''' <param name="eventType"></param>
        ''' <returns></returns>
        Public Overrides Function HandlesEventType(eventType As Type) As Boolean Implements IProjection.HandlesEventType

            If eventType Is GetType(MockEventTypeOne) Then
                Return True
            End If

            Return False

        End Function

        Public Sub New()
            CreateProperty(Of Integer)(NameOf(Total))
        End Sub

    End Class


    ''' <summary>
    ''' Simplest mock projection - just updates a total property
    ''' </summary>
    <DomainName("UnitTest")>
    Public Class MockProjection_Untyped_Simple
        Inherits ProjectionBaseUntyped
        Implements IProjectionUntyped



        Public Overrides ReadOnly Property CurrentAsOfDate As Date Implements IProjection.CurrentAsOfDate
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsSnapshots As Boolean Implements IProjection.SupportsSnapshots
            Get
                Return False
            End Get
        End Property

        ''' <summary>
        ''' This mock projection only handles events of type MockEventTypeOne
        ''' </summary>
        ''' <param name="eventType">
        ''' The type of event we want to know if this projection cares about
        ''' </param>
        Public Overrides Function HandlesEventType(eventType As Type) As Boolean Implements IProjection.HandlesEventType

            If eventType Is GetType(MockEventTypeOne) Then
                Return True
            End If

            Return False

        End Function

        ''' <summary>
        ''' Handle an individual event from an event stream
        ''' </summary>
        ''' <typeparam name="TEvent">
        ''' The type of event this is
        ''' </typeparam>
        ''' <param name="eventToHandle">
        ''' The specific event to handle
        ''' </param>
        Public Overrides Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent) Implements IProjectionUntyped.HandleEvent

            Select Case eventToHandle.GetType()
                Case GetType(MockEventTypeOne)
                    HandleMockEventOne(Convert.ChangeType(eventToHandle, GetType(MockEventTypeOne)))
                Case Else
                    'Nothing to do with this event type
                    Throw New ArgumentException("Unexpected event type - " & eventToHandle.GetType().Name)
            End Select

        End Sub

        Private Sub HandleMockEventOne(ByVal eventToHandle As MockEventTypeOne)

            AddOrUpdateValue(Of Integer)(NameOf(Total), ProjectionSnapshotProperty.NO_ROW_NUMBER, Total + eventToHandle.EventOneIntegerProperty)

        End Sub

        Public Overrides Sub HandleEventJSon(eventFullName As String, eventToHandle As JObject)

            If eventFullName.Equals(GetType(MockEventTypeOne).FullName) Then
                Dim evt As MockEventTypeOne = eventToHandle.ToObject(Of MockEventTypeOne)
                HandleMockEventOne(evt)
            End If

        End Sub

        Public Overrides Function HandlesEventTypeByName(eventTypeFullName As String) As Boolean

            If eventTypeFullName.Equals(GetType(MockEventTypeOne).FullName) Then
                Return True
            End If

            Return False

        End Function

#Region "Output"

        ''' <summary>
        ''' The total number of the numbers in this event stream
        ''' </summary>
        Public ReadOnly Property Total As Integer
            Get
                Return GetPropertyValue(Of Integer)(NameOf(Total))
            End Get
        End Property



#End Region


        Public Sub New()
            CreateProperty(Of Integer)(NameOf(Total))
        End Sub


    End Class


    ''' <summary>
    ''' Simplest mock projection with snapshots
    ''' </summary>
    <DomainName("UnitTest")>
    Public Class MockProjection_Snapshots
        Inherits ProjectionBase(Of MockAggregate, String)
        Implements IProjection(Of MockAggregate, String)

        Private m_totalOut As Integer


        Public Overrides ReadOnly Property CurrentAsOfDate As Date Implements IProjection.CurrentAsOfDate
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsSnapshots As Boolean Implements IProjection.SupportsSnapshots
            Get
                Return True
            End Get
        End Property

        Public Overrides Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent) Implements IProjection(Of MockAggregate, String).HandleEvent

            Select Case eventToHandle.GetType()
                Case GetType(MockEventTypeOne)
                    HandleMockEventOne(Convert.ChangeType(eventToHandle, GetType(MockEventTypeOne)))
                Case Else
                    'Nothing to do with this event type
                    Throw New ArgumentException("Unexpected event type - " & eventToHandle.GetType().Name)
            End Select

        End Sub

        Private Sub HandleMockEventOne(ByVal eventToHandle As MockEventTypeOne)

            AddOrUpdateValue(Of Integer)(NameOf(Total), ProjectionSnapshotProperty.NO_ROW_NUMBER, Total + eventToHandle.EventOneIntegerProperty)

        End Sub


#Region "Output"

        ''' <summary>
        ''' The total number of the numbers in this event stream
        ''' </summary>
        Public ReadOnly Property Total As Integer
            Get
                Return GetPropertyValue(Of Integer)(NameOf(Total))
            End Get
        End Property



#End Region



        ''' <summary>
        ''' This mock projection only handles events of type MockEventTypeOne
        ''' </summary>
        ''' <param name="eventType"></param>
        ''' <returns></returns>
        Public Overrides Function HandlesEventType(eventType As Type) As Boolean Implements IProjection.HandlesEventType

            If eventType Is GetType(MockEventTypeOne) Then
                Return True
            End If

            Return False

        End Function

        Public Sub New()
            CreateProperty(Of Integer)(NameOf(Total))
        End Sub

    End Class

    ''' <summary>
    ''' Simplest mock projection with snapshots
    ''' </summary>
    <DomainName("UnitTest")>
    Public Class MockProjection_Untyped_Snapshots
        Inherits ProjectionBaseUntyped
        Implements IProjectionUntyped

        Private m_totalOut As Integer


        Public Overrides ReadOnly Property CurrentAsOfDate As Date Implements IProjection.CurrentAsOfDate
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Overrides ReadOnly Property SupportsSnapshots As Boolean Implements IProjection.SupportsSnapshots
            Get
                Return True
            End Get
        End Property

        Public Overrides Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent) Implements IProjection.HandleEvent

            Select Case eventToHandle.GetType()
                Case GetType(MockEventTypeOne)
                    HandleMockEventOne(Convert.ChangeType(eventToHandle, GetType(MockEventTypeOne)))
                Case Else
                    'Nothing to do with this event type
                    Throw New ArgumentException("Unexpected event type - " & eventToHandle.GetType().Name)
            End Select

        End Sub

        Private Sub HandleMockEventOne(ByVal eventToHandle As MockEventTypeOne)

            AddOrUpdateValue(Of Integer)(NameOf(Total), ProjectionSnapshotProperty.NO_ROW_NUMBER, Total + eventToHandle.EventOneIntegerProperty)

        End Sub


#Region "Output"

        ''' <summary>
        ''' The total number of the numbers in this event stream
        ''' </summary>
        Public ReadOnly Property Total As Integer
            Get
                Return GetPropertyValue(Of Integer)(NameOf(Total))
            End Get
        End Property



#End Region



        ''' <summary>
        ''' This mock projection only handles events of type MockEventTypeOne
        ''' </summary>
        ''' <param name="eventType"></param>
        ''' <returns></returns>
        Public Overrides Function HandlesEventType(eventType As Type) As Boolean Implements IProjection.HandlesEventType

            If eventType Is GetType(MockEventTypeOne) Then
                Return True
            End If

            Return False

        End Function

        Public Sub New()
            CreateProperty(Of Integer)(NameOf(Total))
        End Sub

        Public Overrides Sub HandleEventJSon(eventFullName As String, eventToHandle As JObject)

            If eventFullName.Equals(GetType(MockEventTypeOne).FullName) Then
                Dim evt As MockEventTypeOne = eventToHandle.ToObject(Of MockEventTypeOne)
                HandleMockEventOne(evt)
            End If

        End Sub

        Public Overrides Function HandlesEventTypeByName(eventTypeFullName As String) As Boolean

            If eventTypeFullName.Equals(GetType(MockEventTypeOne).FullName) Then
                Return True
            End If

            Return False

        End Function

    End Class

    ''' <summary>
    ''' A simple projection that only handles one event type and does not support snapshots
    ''' </summary>
    <DomainName("UnitTest")>
    Public Class MockProjectionNoSnapshots
        Inherits ProjectionBase(Of MockAggregate, String)
        Implements IProjection(Of MockAggregate, String)

        Private m_totalOut As Integer
        Private m_lastString As String


        Public Overrides ReadOnly Property SupportsSnapshots As Boolean Implements IProjection.SupportsSnapshots
            Get
                Return False
            End Get
        End Property

        Public Overrides Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent) Implements IProjection(Of MockAggregate, String).HandleEvent

            Select Case eventToHandle.GetType()
                Case GetType(MockEventTypeOne)
                    HandleMockEventOne(Convert.ChangeType(eventToHandle, GetType(MockEventTypeOne)))
                Case Else
                    'Nothing to do with this event type
                    Throw New ArgumentException("Unexpected event type - " & eventToHandle.GetType().Name)
            End Select

        End Sub

        Private Sub HandleMockEventOne(ByVal eventToHandle As MockEventTypeOne)

            m_totalOut += eventToHandle.EventOneIntegerProperty
            m_lastString = eventToHandle.EventOneStringProperty

        End Sub


#Region "Output"

        ''' <summary>
        ''' The last string value set in this event stream
        ''' </summary>
        Public ReadOnly Property LastString As String
            Get
                Return m_lastString
            End Get
        End Property

        ''' <summary>
        ''' The total number of the numbers in this event stream
        ''' </summary>
        Public ReadOnly Property Total As Integer
            Get
                Return m_totalOut
            End Get
        End Property

#End Region



        ''' <summary>
        ''' This mock projection only handles events of type MockEventTypeOne
        ''' </summary>
        ''' <param name="eventType"></param>
        ''' <returns></returns>
        Public Overrides Function HandlesEventType(eventType As Type) As Boolean Implements IProjection.HandlesEventType

            If eventType Is GetType(MockEventTypeOne) Then
                Return True
            End If

            Return False

        End Function


    End Class

    ''' <summary>
    ''' A more complicated projection that handles multiple event types but does not support snapshots
    ''' </summary>
    <DomainName("UnitTest")>
    Public Class MockProjectionMultipleEventsNoSnapshots
        Inherits ProjectionBase(Of MockAggregate, String)
        Implements IProjection(Of MockAggregate, String)

        Private m_totalOut As Integer

        Public Overrides ReadOnly Property SupportsSnapshots As Boolean Implements IProjection.SupportsSnapshots
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property CurrentAsOfDate As Date Implements IProjection.CurrentAsOfDate
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Overrides Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent) Implements IProjection(Of MockAggregate, String).HandleEvent

            Select Case eventToHandle.GetType()
                Case GetType(MockEventTypeOne)
                    HandleMockEventOne(Convert.ChangeType(eventToHandle, GetType(MockEventTypeOne)))
                Case GetType(MockEventTypeTwo)
                    HandleMockEventTwo(Convert.ChangeType(eventToHandle, GetType(MockEventTypeTwo)))
                Case Else
                    'Nothing to do with this event type
                    Throw New ArgumentException("Unexpected event type - " & eventToHandle.GetType().Name)
            End Select

        End Sub

        Private Sub HandleMockEventTwo(mockEventTypeTwo As MockEventTypeTwo)

            MyBase.AddOrUpdateValue(Of String)(NameOf(LastString), 0, mockEventTypeTwo.EventTwoStringProperty)

        End Sub

        Private Sub HandleMockEventOne(ByVal eventToHandle As MockEventTypeOne)

            m_totalOut += eventToHandle.EventOneIntegerProperty
            MyBase.AddOrUpdateValue(Of Integer)(NameOf(Total), 0, m_totalOut)
            MyBase.AddOrUpdateValue(Of String)(NameOf(LastString), 0, eventToHandle.EventOneStringProperty)

        End Sub


#Region "Output"

        ''' <summary>
        ''' The last string value set in this event stream
        ''' </summary>
        Public ReadOnly Property LastString As String
            Get
                Return MyBase.GetPropertyValue(Of String)(NameOf(LastString))
            End Get
        End Property

        ''' <summary>
        ''' The total number of the numbers in this event stream
        ''' </summary>
        Public ReadOnly Property Total As Integer
            Get
                Return MyBase.GetPropertyValue(Of Integer)(NameOf(Total))
            End Get
        End Property

#End Region



        ''' <summary>
        ''' This mock projection only handles events of type MockEventTypeOne
        ''' </summary>
        ''' <param name="eventType"></param>
        ''' <returns></returns>
        Public Overrides Function HandlesEventType(eventType As Type) As Boolean Implements IProjection.HandlesEventType

            If eventType Is GetType(MockEventTypeOne) Then
                Return True
            End If

            If eventType Is GetType(MockEventTypeTwo) Then
                Return True
            End If

            Return False

        End Function


    End Class


    ''' <summary>
    ''' A simple projection that only handles one event type and does support snapshots
    ''' </summary>
    <DomainName("UnitTest")>
    Public Class MockProjectionWithSnapshots
        Inherits ProjectionBase(Of MockAggregate, String)
        Implements IProjection(Of MockAggregate, String)

        Private m_totalOut As Integer
        Private m_lastString As String


        Public Overrides ReadOnly Property SupportsSnapshots As Boolean Implements IProjection.SupportsSnapshots
            Get
                Return True
            End Get
        End Property



        Public Overrides Sub HandleEvent(Of TEvent As IEvent)(eventToHandle As TEvent) Implements IProjection(Of MockAggregate, String).HandleEvent

            Select Case eventToHandle.GetType()
                Case GetType(MockEventTypeOne)
                    HandleMockEventOne(Convert.ChangeType(eventToHandle, GetType(MockEventTypeOne)))
                Case GetType(MockEventTypeTwo)
                    HandleMockEventTwo(Convert.ChangeType(eventToHandle, GetType(MockEventTypeTwo)))
                Case Else
                    'Nothing to do with this event type
                    Throw New ArgumentException("Unexpected event type - " & eventToHandle.GetType().Name)
            End Select


        End Sub

        Private Sub HandleMockEventTwo(mockEventTypeTwo As MockEventTypeTwo)
            AddOrUpdateValue(Of String)(NameOf(LastString), ProjectionSnapshotProperty.NO_ROW_NUMBER, mockEventTypeTwo.EventTwoStringProperty)
        End Sub

        Private Sub HandleMockEventOne(ByVal eventToHandle As MockEventTypeOne)

            AddOrUpdateValue(Of Integer)(NameOf(Total), ProjectionSnapshotProperty.NO_ROW_NUMBER, Total + eventToHandle.EventOneIntegerProperty)
            AddOrUpdateValue(Of String)(NameOf(LastString), ProjectionSnapshotProperty.NO_ROW_NUMBER, eventToHandle.EventOneStringProperty)

        End Sub


#Region "Output"

        ''' <summary>
        ''' The last string value set in this event stream
        ''' </summary>
        Public ReadOnly Property LastString As String
            Get
                Return GetPropertyValue(Of String)(NameOf(LastString))
            End Get
        End Property

        ''' <summary>
        ''' The total number of the numbers in this event stream
        ''' </summary>
        Public ReadOnly Property Total As Integer
            Get
                Return GetPropertyValue(Of Integer)(NameOf(Total))
            End Get
        End Property

#End Region

        Public Sub New()
            CreateProperty(Of Integer)(NameOf(Total))
            CreateProperty(Of String)(NameOf(LastString))
        End Sub

        ''' <summary>
        ''' This mock projection only handles events of type MockEventTypeOne
        ''' </summary>
        ''' <param name="eventType"></param>
        ''' <returns></returns>
        Public Overrides Function HandlesEventType(eventType As Type) As Boolean Implements IProjection.HandlesEventType

            If eventType Is GetType(MockEventTypeOne) Then
                Return True
            End If

            Return False

        End Function


    End Class

End Namespace