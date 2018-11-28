Imports System.Reflection
Imports CQRSAzure.EventSourcing.Azure.Table
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table.Untyped

    ''' <summary>
    ''' An event stream reader with Azure Tables as the backing store that allows untyped aggregate definition
    ''' </summary>
    ''' <remarks>
    ''' Care must be taken to make sure this remains compatible with the type-safe implementation
    ''' </remarks>
    Public Class TableEventStreamReaderUntyped
        Inherits TableEventStreamBaseUntyped
        Implements IEventStreamReaderUntyped

        Private ReadOnly m_validEventTypes As IEnumerable(Of Type)
        Private ReadOnly m_eventFilterFunction As FilterFunctions.EventFilterFunction

        Public ReadOnly Property Key As String Implements IEventStreamReaderUntyped.Key
            Get
                Return MyBase.InstanceKey
            End Get
        End Property


        Public Async Function GetEvents() As Task(Of IEnumerable(Of IEvent)) Implements IEventStreamReaderUntyped.GetEvents

            Return Await GetEvents(0)

        End Function

        Public Async Function GetEvents(Optional StartingSequenceNumber As UInteger = 0, Optional effectiveDateTime As Date? = Nothing) As Task(Of IEnumerable(Of IEvent)) Implements IEventStreamReaderUntyped.GetEvents


            If (MyBase.Table IsNot Nothing) Then
                Dim ret As New List(Of IEvent)()
                Dim continueToken As New TableContinuationToken()
                Dim query = CreateQuery(StartingSequenceNumber)
                Do
                    Dim seg = Await MyBase.Table.ExecuteQuerySegmentedAsync(query, continueToken)

                    For Each dte As DynamicTableEntity In seg
                        Dim eventType As Type = GetDynamicTableEntityEventType(dte)
                        If IsEventValid(eventType) Then
                            Dim evt As IEvent = Nothing
                            Dim deserialiser As IEventSerializer = EventSerializerFactory.GetSerialiserByType(eventType)
                            If (deserialiser IsNot Nothing) Then
                                'Use the event serialiser... 
                                evt = deserialiser.FromNameValuePairs(DynamicTableEntryToNameValuePairs(dte))
                            Else
                                evt = CType(Activator.CreateInstance(eventType), IEvent)
                                PopulateDynamicTableEntityEvent(dte, evt)
                            End If
                            'if so, add it to the returned list 
                            If (evt IsNot Nothing) Then
                                ret.Add(evt)
                            End If
                        End If
                    Next
                    continueToken = seg.ContinuationToken
                Loop While (continueToken IsNot Nothing)
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, InstanceKey, 0, "Missing or not initialised table reference")
            End If

        End Function

        Private Sub PopulateDynamicTableEntityEvent(Of TEvent As IEvent)(ByVal tableEntityRow As DynamicTableEntity, ByRef eventData As TEvent)

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

        Public Function IsEventValid(eventType As Type) As Boolean

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
        Private Function CreateQuery(Optional ByVal StartingSequenceNumber As Long = 0) As TableQuery

            Return New TableQuery().Where(
            TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, InstanceKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, SequenceNumberToRowkey(StartingSequenceNumber))))

        End Function

        Public Async Function GetEventsWithContext(Optional StartingSequenceNumber As UInteger = 0,
                                             Optional effectiveDateTime As Date? = Nothing) As Task(Of IEnumerable(Of IEventContext)) Implements IEventStreamReaderUntyped.GetEventsWithContext


            If (MyBase.Table IsNot Nothing) Then
                Dim ret As New List(Of IEventContext)
                Dim continueToken As New TableContinuationToken()
                Dim query = CreateQuery(StartingSequenceNumber)
                Do
                    Dim seg = Await MyBase.Table.ExecuteQuerySegmentedAsync(query, continueToken)

                    For Each dte As DynamicTableEntity In seg
                        Dim eventType As Type = GetDynamicTableEntityEventType(dte)
                        'see if the event type is valid...
                        If IsEventValid(eventType) Then
                            Dim evt As IEvent = Nothing
                            Dim deserialiser As IEventSerializer = EventSerializerFactory.GetSerialiserByType(eventType)
                            If (deserialiser IsNot Nothing) Then
                                'Use the event serialiser... 
                                evt = deserialiser.FromNameValuePairs(DynamicTableEntryToNameValuePairs(dte))
                            Else
                                'if so, add it to the returned list 
                                evt = CType(Activator.CreateInstance(eventType),
                                IEvent)
                                PopulateDynamicTableEntityEvent(dte, evt)
                                'Wrap it with context from the table row
                            End If
                            ret.Add(WrapDynamicTableEntityEvent(dte, evt))
                        End If
                    Next
                    continueToken = seg.ContinuationToken
                Loop While (continueToken IsNot Nothing)
                Return ret
            Else
                Throw New EventStreamReadException(DomainName,
                                                   AggregateClassName,
                                                   InstanceKey,
                                                   0,
                                                   "Missing or not initialised table reference")
            End If

        End Function

        Private Function WrapDynamicTableEntityEvent(dte As DynamicTableEntity, evt As IEvent) As IEventContext

            Dim eventVersion As Integer = 0

            If dte.Properties.ContainsKey(FIELDNAME_VERSION) Then
                eventVersion = CInt(dte.Properties(FIELDNAME_VERSION).Int64Value.GetValueOrDefault())
            End If


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

            Dim correlationId As String = ""
            If dte.Properties.ContainsKey(FIELDNAME_CORRELATION_IDENTIFIER) Then
                correlationId = dte.Properties(FIELDNAME_CORRELATION_IDENTIFIER).StringValue
            End If

            Dim timestamp As DateTime = dte.Timestamp.UtcDateTime

            Return ContextWrappedEventUntyped.Wrap(InstanceKey,
                                                   evt,
                                                    sequence,
                                                    comments,
                                                    source,
                                                    timestamp,
                                                    CUInt(eventVersion),
                                                    who,
                                                    correlationId)

        End Function

        Protected Sub New(identifier As IEventStreamUntypedIdentity,
                  Optional connectionStringName As String = "",
                  Optional settings As ITableSettings = Nothing)

            MyBase.New(identifier,
                       writeAccess:=False,
                       connectionStringName:=connectionStringName,
                       settings:=settings)

        End Sub


        ''' <summary>
        ''' Creates an azure blob storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="settings">
        ''' The settings to use to connect to the azure storage account to use
        ''' </param>
        Public Shared Function Create(ByVal identifier As IEventStreamUntypedIdentity,
                                      Optional ByVal connectionString As String = Nothing,
                                      Optional ByVal settings As ITableSettings = Nothing) As IEventStreamReaderUntyped

            If (String.IsNullOrWhiteSpace(connectionString)) Then
                If (settings IsNot Nothing) Then
                    connectionString = GetReadConnectionStringName(identifier.DomainName.Trim() & "ConnectionString", settings)
                End If
            End If


            Return New TableEventStreamReaderUntyped(identifier,
                                                    connectionStringName:=connectionString,
                                                     settings:=settings)

        End Function


        ''' <summary>
        ''' Create a projection processor that works off an azure blob backed event stream
        ''' </summary>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal identifier As IEventStreamUntypedIdentity,
                                                         Optional ByVal connectionString As String = Nothing,
                                                         Optional ByVal settings As ITableSettings = Nothing,
                                                         Optional ByVal snapshotProcessor As ISnapshotProcessorUntyped = Nothing) As ProjectionProcessorUntyped

            Return New ProjectionProcessorUntyped(Create(identifier, connectionString, settings), snapshotProcessor)

        End Function



    End Class

End Namespace
