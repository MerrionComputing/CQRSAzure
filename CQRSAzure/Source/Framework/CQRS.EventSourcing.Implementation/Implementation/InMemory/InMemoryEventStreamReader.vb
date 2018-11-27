Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory
    Public NotInheritable Class InMemoryEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits InMemoryEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamFilteredReader(Of TAggregate, TAggregateKey)
        Implements IEventStreamDebugReader(Of TAggregate, TAggregateKey)

        Private ReadOnly m_validEventTypes As IEnumerable(Of Type)
        Private ReadOnly m_eventFilterFunction As FilterFunctions.EventFilterFunction

        Public ReadOnly Property Key As TAggregateKey Implements IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return MyBase.AggregationKey
            End Get
        End Property

        ''' <summary>
        ''' Decide if an event type is valid to be read for this event stream reader
        ''' </summary>
        ''' <param name="eventType">
        ''' The event type read (or to be read) from the stream
        ''' </param>
        ''' <returns>
        ''' True if the event type should be passed on
        ''' </returns>
        Public Function IsEventValid(eventType As Type) As Boolean Implements IEventStreamFilteredReader(Of TAggregate, TAggregateKey).IsEventValid

#Region "Debug messages"
            If (m_debugMessages) Then
                System.Diagnostics.Debug.WriteLine("Evaluating filter for event of type {0}", eventType)
            End If
#End Region

            If (m_eventFilterFunction IsNot Nothing) Then
                Return m_eventFilterFunction.Invoke(eventType)
            Else
                If (m_validEventTypes Is Nothing) Then
                    Return True
                Else
                    Return m_validEventTypes.Contains(eventType)
                End If
            End If

        End Function


        Public Overloads Async Function GetEvents(Optional ByVal StartingSequenceNumber As UInteger = 0,
                                            Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As Task(Of IEnumerable(Of IEvent(Of TAggregate))) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            ' If we do not yet have an event stream for this aggregagate, create one
            CreateStreamIfNotCreated()

            Return Await Task.Run(Function()
                                      'Strip all the wrappers off the events
                                      Dim dryAllEvents = From x In m_eventStream(AggregationKey)
                                                         Where (x.SequenceNumber >= StartingSequenceNumber) AndAlso IsEventValid(x.EventInstance.GetType())
                                                         Order By x.SequenceNumber
                                                         Select x.EventInstance


                                      Return dryAllEvents.AsEnumerable().Cast(Of IEvent(Of TAggregate))

                                  End Function
                )

        End Function

        Public Overloads Async Function GetEventsWithContext(Optional ByVal StartingSequenceNumber As UInteger = 0,
                                                       Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As Task(Of IEnumerable(Of IEventContext)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEventsWithContext

            ' If we do not yet have an event stream for this aggregagate, create one
            CreateStreamIfNotCreated()
            Return Await Task.Run(Function()
                                      Dim dryAllEvents = From x In m_eventStream(AggregationKey)
                                                         Where (x.SequenceNumber >= StartingSequenceNumber) AndAlso IsEventValid(x.EventInstance.GetType())
                                                         Order By x.SequenceNumber
                                                         Select x


                                      Return dryAllEvents.AsEnumerable()

                                  End Function
                )
        End Function

        Public Overloads Async Function GetEvents() As Task(Of IEnumerable(Of IEvent(Of TAggregate))) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            ' Start from the begining sequence number
            Return Await Task.Run(Function()
                                      Return GetEvents(0)
                                  End Function
                                      )

        End Function

#Region "Debug Reader"

        Private m_lastSequenceNumber As Long = 0

        ''' <summary>
        ''' The current event sequence in the stream being read
        ''' </summary>
        ''' <remarks>
        ''' This is somewhat analoguous to an object pointer to the event in a memory based system
        ''' </remarks>
        Public ReadOnly Property CurrentSequenceNumber As Long Implements IEventStreamDebugReader(Of TAggregate, TAggregateKey).CurrentSequenceNumber
            Get
                Return m_lastSequenceNumber
            End Get
        End Property



        ''' <summary>
        ''' Get the next event from the stream given the current sequence number
        ''' </summary>
        ''' <returns>
        ''' An event wrapped in the event context information
        ''' </returns>
        ''' <remarks>
        ''' This will be null if there are no more events that match the filter conditions
        ''' </remarks>
        Public Function GetNextEventWithContext() As IEventContext Implements IEventStreamDebugReader(Of TAggregate, TAggregateKey).GetNextEventWithContext

            ' If we do not yet have an event stream for this aggregagate, create one
            CreateStreamIfNotCreated()

            'Strip all the wrappers off the events
            Dim dryNextEvents = From x In m_eventStream(AggregationKey)
                                Where (x.SequenceNumber >= m_lastSequenceNumber) AndAlso IsEventValid(x.EventInstance.GetType())
                                Order By x.SequenceNumber
                                Select x


            Return dryNextEvents.FirstOrDefault()

        End Function

        ''' <summary>
        ''' Set the current sequence pointer back to zero to restart reading the event stream
        ''' </summary>
        Public Sub ResetToStart() Implements IEventStreamDebugReader(Of TAggregate, TAggregateKey).ResetToStart
            m_lastSequenceNumber = 0
        End Sub

#End Region


        ''' <summary>
        ''' Creates a new event stream writer to read events from the event stream for the given aggregate
        ''' </summary>
        ''' <param name="aggregateIdentityKey">
        ''' The unique identifier fo the instance of that aggregate class
        ''' </param>
        ''' <param name="eventFilter">
        ''' A set of type to filter the event stream by
        ''' </param>
        Private Sub New(ByVal aggregateIdentityKey As TAggregateKey,
                        Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                        Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing,
                        Optional ByVal settings As IInMemorySettings = Nothing
                        )
            MyBase.New(aggregateIdentityKey, settings)

            If (eventFilter IsNot Nothing) Then
                m_validEventTypes = eventFilter
            End If

            If (eventFilterFunction IsNot Nothing) Then
                m_eventFilterFunction = eventFilterFunction
            End If

        End Sub



#Region "Factory methods"

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="eventFilter">
        ''' An optional array of event definitions to use to filter the incoming event stream by
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                                      Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing,
                                      Optional ByVal settings As IInMemorySettings = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return Create(instance.GetKey(), eventFilter, eventFilterFunction, settings)

        End Function

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instanceKey">
        ''' The unique identifier of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="eventFilter">
        ''' An optional array of event definitions to use to filter the incoming event stream by
        ''' </param>
        Public Shared Function Create(ByVal instanceKey As TAggregateKey,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                                      Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing,
                                      Optional ByVal settings As IInMemorySettings = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return New InMemoryEventStreamReader(Of TAggregate, TAggregateKey)(instanceKey, eventFilter, eventFilterFunction, settings)

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an in-memory backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                                                         Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing,
                                                         Optional ByVal settings As IInMemorySettings = Nothing) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregateKey)(Create(instance, eventFilter, eventFilterFunction, settings))

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an in-memory backed event stream
        ''' </summary>
        ''' <param name="instanceKey">
        ''' The unique identifier of the instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal instanceKey As TAggregateKey,
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                                                         Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing,
                                                         Optional ByVal settings As IInMemorySettings = Nothing) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregateKey)(Create(instanceKey, eventFilter, eventFilterFunction, settings))

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an in-memory backed event stream
        ''' </summary>
        ''' <param name="readerToUse">
        ''' The event stream reader to use to run the projection
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal readerToUse As IEventStreamReader(Of TAggregate, TAggregateKey)) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregateKey)(readerToUse)

        End Function
#End Region

    End Class

    Public Module InMemoryEventStreamReaderFactory

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="eventFilter">
        ''' An optional array of event definitions to use to filter the incoming event stream by
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)(ByVal instance As TAggregate,
                                      ByVal key As TAggregateKey,
                                      ByVal settings As IInMemorySettings,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                                      Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing
                                      ) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return InMemoryEventStreamReader(Of TAggregate, TAggregateKey).Create(instance, eventFilter, eventFilterFunction, settings)

        End Function

        ''' <summary>
        ''' Generate a function that can be used to create a reader of the given type
        ''' </summary>
        ''' <typeparam name="TAggregate">
        ''' The data type of the aggregate class
        ''' </typeparam>
        ''' <typeparam name="TAggregateKey">
        ''' The data type that provides the unique identification of an instance of the reader class
        ''' </typeparam>
        Public Function GenerateCreationFunctionDelegate(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)() As IAggregateImplementationMap.ReaderCreationFunction(Of TAggregate, TAggregateKey)


            'Make delegate for this module Create() function....
            Return New IAggregateImplementationMap.ReaderCreationFunction(Of TAggregate, TAggregateKey)(AddressOf Create(Of TAggregate, TAggregateKey))

        End Function


    End Module
End Namespace