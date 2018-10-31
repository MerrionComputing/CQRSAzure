Option Strict Off

Imports System.Reflection
Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table

    ''' <summary>
    ''' An event stream reader that gets its events from an Azure Table 
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate against which the events are stored
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type that uniquely identifies the aggregate instance that teh event stream is connected to
    ''' </typeparam>
    ''' <remarks>
    ''' Azure Table Storage uses a continuation token in the response header to indicate that there are additional results for a query. 
    ''' You can retrieve these results by issuing another request that is parameterized by the continuation token. 
    ''' This scenario enables you to retrieve items beyond the 1,000-entity limit.
    ''' </remarks>
    Public Class TableEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits TableEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamFilteredReader(Of TAggregate, TAggregateKey)

        Private ReadOnly m_validEventTypes As IEnumerable(Of Type)
        Private ReadOnly m_eventFilterFunction As FilterFunctions.EventFilterFunction

        Public ReadOnly Property Key As TAggregateKey Implements IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return MyBase.m_key
            End Get
        End Property

        Public Function IsEventValid(eventType As Type) As Boolean Implements IEventStreamFilteredReader(Of TAggregate, TAggregateKey).IsEventValid

            If (eventType IsNot Nothing) Then
                If (m_eventFilterFunction IsNot Nothing) Then
                    Return m_eventFilterFunction.Invoke(eventType)
                Else
                    If (m_validEventTypes Is Nothing) Then
                        Return True
                    Else
                        Return m_validEventTypes.Contains(eventType)
                    End If
                End If
            End If
            Return False

        End Function

        Public Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            Return GetEvents(0)

        End Function

        Public Function GetEvents(Optional ByVal StartingSequenceNumber As UInteger = 0,
                                  Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents

            If (MyBase.Table IsNot Nothing) Then
                Dim ret As New List(Of IEvent(Of TAggregate))()
                For Each dte As DynamicTableEntity In MyBase.Table.ExecuteQuery(CreateQuery(m_key.ToString(), StartingSequenceNumber), MyBase.RequestOptions)
                    Dim eventType As Type = GetDynamicTableEntityEventType(dte)
                    If IsEventValid(eventType) Then
                        Dim evt As IEvent(Of TAggregate) = Nothing
                        Dim deserialiser As IEventSerializer = EventSerializerFactory.GetSerialiserByType(eventType)
                        If (deserialiser IsNot Nothing) Then
                            'Use the event serialiser... 
                            evt = deserialiser.FromNameValuePairs(DynamicTableEntryToNameValuePairs(dte))
                        Else
                            evt = CType(Activator.CreateInstance(eventType),
                                IEvent(Of TAggregate))
                            PopulateDynamicTableEntityEvent(dte, evt)
                        End If
                        'if so, add it to the returned list 
                        If (evt IsNot Nothing) Then
                            ret.Add(evt)
                        End If
                    End If
                Next
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, m_key.ToString(), 0, "Missing or not initialised table reference")
            End If

        End Function



        Public Function GetEventsWithContext(Optional ByVal StartingSequenceNumber As UInteger = 0,
                                             Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEventContext) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEventsWithContext

            If (MyBase.Table IsNot Nothing) Then
                Dim ret As New List(Of IEventContext)
                For Each dte As DynamicTableEntity In MyBase.Table.ExecuteQuery(CreateQuery(m_key.ToString(), StartingSequenceNumber))
                    Dim eventType As Type = GetDynamicTableEntityEventType(dte)
                    'see if the event type is valid...
                    If IsEventValid(eventType) Then
                        Dim evt As IEvent(Of TAggregate)
                        Dim deserialiser As IEventSerializer = EventSerializerFactory.GetSerialiserByType(eventType)
                        If (deserialiser IsNot Nothing) Then
                            'Use the event serialiser... 
                            evt = deserialiser.FromNameValuePairs(DynamicTableEntryToNameValuePairs(dte))
                        Else
                            'if so, add it to the returned list 
                            evt = CType(Activator.CreateInstance(eventType),
                                IEvent(Of TAggregate))
                            PopulateDynamicTableEntityEvent(dte, evt)
                            'Wrap it with context from the table row
                        End If
                        ret.Add(WrapDynamicTableEntityEvent(dte, evt))
                    End If
                Next
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, m_key.ToString(), 0, "Missing or not initialised table reference")
            End If

        End Function

        Private Function WrapDynamicTableEntityEvent(dte As DynamicTableEntity, evt As IEvent(Of TAggregate)) As IEventContext

            Dim eventVersion As Integer = 0

            If dte.Properties.ContainsKey(FIELDNAME_VERSION) Then
                eventVersion = CInt(dte.Properties(FIELDNAME_VERSION).Int64Value.GetValueOrDefault())
            End If

            Dim eventInst = InstanceWrappedEvent(Of TAggregateKey).Wrap(m_key, evt, CUInt(eventVersion))

            'get the context properties of the event
            Dim sequence As Long = RowKeyToSequenceNumber(dte.RowKey)

            Dim comments As String = ""
            If dte.Properties.ContainsKey(FIELDNAME_COMMENTS) Then
                comments = dte.Properties(FIELDNAME_COMMENTS).StringValue
            End If

            Dim who As String = ""
            If dte.Properties.ContainsKey(FIELDNAME_WHO) Then
                who = dte.Properties(FIELDNAME_WHO).StringValue
            End If

            Dim source As String = ""
            If dte.Properties.ContainsKey(FIELDNAME_SOURCE) Then
                source = dte.Properties(FIELDNAME_SOURCE).StringValue
            End If

            Dim timestamp As DateTime = dte.Timestamp.UtcDateTime

            Return ContextWrappedEvent(Of TAggregateKey).Wrap(eventInst,
                                                              sequence,
                                                              comments,
                                                              source,
                                                              timestamp,
                                                              CUInt(eventVersion),
                                                              who)

        End Function

        ''' <summary>
        ''' Create a table query where the partition key is the aggregate instance identifier  and the sequence number is greater than or equal to the current starting
        ''' sequence number (which may be zero)
        ''' </summary>
        ''' <param name="StartingSequenceNumber">
        ''' The sequence number (0 based) of the first event to retrieve
        ''' </param>
        ''' <remarks>
        ''' There is no concept of order-by but this does not matter as we explicitly use the sequence number as the row key, and results are 
        ''' always in row key order
        ''' </remarks>
        Private Function CreateQuery(ByVal key As String, Optional ByVal StartingSequenceNumber As Long = 0) As TableQuery

            Return New TableQuery().Where(
            TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, SequenceNumberToRowkey(StartingSequenceNumber))))

        End Function



        Private Sub PopulateDynamicTableEntityEvent(Of TEvent As IEvent(Of TAggregate))(ByVal tableEntityRow As DynamicTableEntity, ByRef eventData As TEvent)

            If (tableEntityRow IsNot Nothing) Then
                For Each entityProperty In tableEntityRow.Properties
                    If Not (IsContextProperty(entityProperty.Key)) Then
                        'Set the property in the event data 
                        Dim pi As PropertyInfo = eventData.GetType().GetProperty(entityProperty.Key)
                        If (pi IsNot Nothing) Then
                            If (pi.CanWrite) Then
                                If IsEntityPropertyValueSet(pi, entityProperty.Value.PropertyAsObject) Then
                                    pi.SetValue(eventData, GetEntityPropertyValue(pi, entityProperty.Value.PropertyAsObject))
                                End If
                            End If
                        End If
                    End If
                Next
            End If

        End Sub






        ''' <summary>
        ''' Create a new windows azure tables stream reader to read events from the file
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The domain in which the aggregate resides
        ''' </param>
        ''' <remarks>
        ''' The unique key which identifies the instance of the aggregate to read the event stream for
        ''' </remarks>
        Private Sub New(ByVal AggregateDomainName As String,
                        ByVal AggregateKey As TAggregateKey,
                        Optional ByVal settings As ITableSettings = Nothing,
                        Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                        Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=False, connectionStringName:=GetReadConnectionStringName("", settings), settings:=settings)

            If (eventFilter IsNot Nothing) Then
                m_validEventTypes = eventFilter
            End If
            If (eventFilterFunction IsNot Nothing) Then
                m_eventFilterFunction = eventFilterFunction
            End If

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure blob storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As ITableSettings = Nothing,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                                      Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return New TableEventStreamReader(Of TAggregate, TAggregateKey)(DomainNameAttribute.GetDomainName(instance), instance.GetKey(), settings, eventFilter, eventFilterFunction)

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an azure blob backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                         Optional ByVal settings As ITableSettings = Nothing,
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                                                         Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregateKey)(Create(instance, settings, eventFilter, eventFilterFunction))

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

    Public Module TableEventStreamReaderFactory

        ''' <summary>
        ''' Factory method to create a type-safe event stream reader based off an SQL server back end event stream
        ''' </summary>
        ''' <typeparam name="TAggregate">
        ''' The type of the aggregate to which the event stream is attached
        ''' </typeparam>
        ''' <typeparam name="TAggregateKey">
        ''' The data type of the key which uniquely identifies the unique instance of the aggregate for which to get the event stream reader
        ''' </typeparam>
        ''' <param name="instance">
        ''' The unique instance of the aggregate for which we are getting the event stream reader
        ''' </param>
        ''' <param name="key">
        ''' The key that uniquely identifies the aggregate instance
        ''' </param>
        ''' <param name="settings">
        ''' (Optional) Additional settings to control how the event stream is read from the SQL Server database
        ''' </param>
        ''' <param name="eventFilter">
        ''' (Optional) A set of events to read - anything not in the set is ignored
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)(ByVal instance As TAggregate,
                                                    ByVal key As TAggregateKey,
                                      Optional ByVal settings As ITableSettings = Nothing,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return TableEventStreamReader(Of TAggregate, TAggregateKey).Create(CType(instance, IAggregationIdentifier(Of TAggregateKey)),
                                                                               settings,
                                                                               eventFilter)

        End Function


        ''' <summary>
        ''' Create a projection processor that works off an Azure tables backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Function CreateProjectionProcessor(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal instance As TAggregate,
                                                                                                                                    ByVal key As TAggregateKey,
                                                         Optional ByVal settings As ITableSettings = Nothing,
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return TableEventStreamReader(Of TAggregate, TAggregateKey).CreateProjectionProcessor(CType(instance, IAggregationIdentifier(Of TAggregateKey)),
                                                                                                  settings,
                                                                                                  eventFilter)

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
            Return New IAggregateImplementationMap.ReaderCreationFunction(Of TAggregate, TAggregateKey) _
                (AddressOf Create(Of TAggregate, TAggregateKey))

        End Function

    End Module
End Namespace