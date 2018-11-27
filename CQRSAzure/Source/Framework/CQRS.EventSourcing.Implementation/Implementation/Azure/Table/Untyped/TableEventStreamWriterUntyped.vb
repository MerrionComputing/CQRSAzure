Imports CQRSAzure.EventSourcing.Azure.Table
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table.Untyped

    ''' <summary>
    ''' An event stream writer with Azure Tables as the backing store that allows untyped aggregate definition
    ''' </summary>
    ''' <remarks>
    ''' Care must be taken to make sure this remains compatible with the type-safe implementation
    ''' </remarks>
    Public Class TableEventStreamWriterUntyped
        Inherits TableEventStreamBaseUntyped
        Implements IEventStreamWriterUntyped


        Private m_context As IWriteContext
        Public Overloads Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriterUntyped.SetContext
            m_context = writerContext
        End Sub

        Private nextSequence As Long = 0
        Public Overloads ReadOnly Property RecordCount As ULong Implements IEventStreamUntyped.RecordCount
            Get
                Return nextSequence
            End Get
        End Property

        Public Overloads ReadOnly Property LastAddition As Date? Implements IEventStreamUntyped.LastAddition
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Overloads Async Function AppendEvent(EventToAppend As IEvent,
                               Optional ExpectedTopSequence As Long = 0,
                               Optional Version As UInteger = 1) As Task Implements IEventStreamWriterUntyped.AppendEvent

            'Set the next highest sequence (in case another writer has appended events)
            nextSequence = 1 + GetCurrentHighestSequence(MyBase.InstanceKey)

            'Update the current highest event number
            UpdateSequenceNumber(nextSequence, MyBase.InstanceKey)

            'Append the event
            Await AppendEventInternal(EventToAppend)

        End Function


        Private Async Function AppendEventInternal(EventToAppend As IEvent, Optional ByVal version As Integer = 1) As Task

            If (MyBase.Table IsNot Nothing) Then
                'Wrap the event in its context information

                Dim commentary As String = ""
                Dim source As String = ""
                Dim who As String = ""
                Dim correlationId As String = ""

                If (m_context IsNot Nothing) Then
                    commentary = m_context.Commentary
                    source = m_context.Source
                    who = m_context.Who
                    correlationId = m_context.CorrelationIdentifier
                End If

                'make an event instance...
                Dim wrappedEvent = ContextWrappedEventUntyped.Wrap(InstanceKey,
                                                                   EventToAppend,
                                                                    nextSequence,
                                                                    commentary,
                                                                    source,
                                                                    DateTime.UtcNow,
                                                                    version,
                                                                    who,
                                                                    correlationId)

                If (wrappedEvent IsNot Nothing) Then
                    'And add that to the table
                    Await MyBase.Table.ExecuteAsync(TableOperation.Insert(MakeDynamicTableEntity(wrappedEvent))) ', MyBase.RequestOptions)
                    'and increment the next sequence number
                    nextSequence += 1
                End If


            End If
        End Function

        ''' <summary>
        ''' Clear down the event stream
        ''' </summary>
        ''' <remarks>
        ''' This is only used for unit testing so is not part of the Event Stream Writer interface
        ''' </remarks>
        Public Sub Reset()

            If MyBase.Table IsNot Nothing Then
                'A Batch Operation allows a maximum 100 entities in the batch which must share the same PartitionKey 
                Dim projectionQuery = New TableQuery(Of DynamicTableEntity)().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, InstanceKey)).Select({"RowKey"}).Take(100)

                Dim moreBatches As Boolean = True
                While moreBatches
                    Dim batchDelete = New TableBatchOperation()
                    Dim continueToken As New TableContinuationToken()
                    For Each e In MyBase.Table.ExecuteQuerySegmentedAsync(projectionQuery, continueToken).Result
                        batchDelete.Delete(e)
                    Next

                    moreBatches = (batchDelete.Count >= 100)

                    If (batchDelete.Count > 0) Then
                        MyBase.Table.ExecuteBatchAsync(batchDelete)
                    End If
                End While
                'Reset the next sequence number too
                nextSequence = 0
                UpdateSequenceNumber(nextSequence, aggregateInstanceKey:=InstanceKey)
            End If
        End Sub

        Public Function MakeDynamicTableEntity(ByVal eventToSave As IEventContext) As DynamicTableEntity

            Dim ret As New DynamicTableEntity With {
                .PartitionKey = InstanceKey,
                .RowKey = SequenceNumberToRowkey(eventToSave.SequenceNumber)
            }

            'Add the event type - currently this is the event class name
            ret.Properties.Add(FIELDNAME_EVENTTYPE,
                  New EntityProperty(eventToSave.EventInstance.GetType().AssemblyQualifiedName))

            ret.Properties.Add(FIELDNAME_VERSION,
                  New EntityProperty(eventToSave.Version))


            If (Not String.IsNullOrWhiteSpace(eventToSave.Commentary)) Then
                ret.Properties.Add(FIELDNAME_COMMENTS,
                     New EntityProperty(eventToSave.Commentary))
            End If



            If (Not String.IsNullOrWhiteSpace(eventToSave.Who)) Then
                ret.Properties.Add(FIELDNAME_WHO, New EntityProperty(eventToSave.Who))
            End If


            If (Not String.IsNullOrWhiteSpace(eventToSave.Source)) Then
                ret.Properties.Add(FIELDNAME_SOURCE, New EntityProperty(eventToSave.Source))
            End If

            ret.Timestamp = eventToSave.Timestamp

            'Now add in the different properties of the payload

            'TODO - Use the event serialiser's "ToNameValuePairs" not reflection

            Dim propertiesCount As Integer = 0
            For Each pi As System.Reflection.PropertyInfo In eventToSave.EventInstance.GetType().GetProperties()
                If (pi.CanRead) Then
                    If Not IsContextProperty(pi.Name) Then
                        If (propertiesCount > MAX_FREE_DATA_FIELDS) Then
                            'Throw an error to indicate that our event exceeds the storage capabilities of an azure table
                            Throw New EventStreamWriteException(DomainName, AggregateClassName, ret.PartitionKey, eventToSave.Version, "Too many properties for event - cannot store in Azure table")
                        Else
                            If Not IsPropertyEmpty(pi, eventToSave.EventInstance) Then
                                ret.Properties.Add(pi.Name, MakeEntityProperty(pi, eventToSave.EventInstance))
                            End If
                        End If
                        propertiesCount = propertiesCount + 1
                    End If
                End If
            Next pi

            Return ret
        End Function


        Protected Sub New(identifier As IEventStreamUntypedIdentity,
                  Optional connectionStringName As String = "",
                  Optional settings As ITableSettings = Nothing)

            MyBase.New(identifier, writeAccess:=True,
                       connectionStringName:=connectionStringName,
                       settings:=settings)

        End Sub


        ''' <summary>
        ''' Creates an azure table storage based event stream writer for the given aggregate - untyped
        ''' </summary>
        ''' <param name="settings">
        ''' The settings to use to connect to the azure storage account to use
        ''' </param>
        Public Shared Function Create(ByVal identifier As IEventStreamUntypedIdentity,
                                      Optional ByVal connectionStringToUse As String = Nothing,
                                      Optional ByVal settings As ITableSettings = Nothing) As IEventStreamWriterUntyped

            If (String.IsNullOrWhiteSpace(connectionStringToUse)) Then
                If (settings IsNot Nothing) Then
                    connectionStringToUse = GetReadConnectionStringName(identifier.DomainName.Trim() & "ConnectionString", settings)
                End If
            End If

            Return New TableEventStreamWriterUntyped(identifier,
                                                    connectionStringName:=connectionStringToUse,
                                                    settings:=settings)

        End Function
    End Class
End Namespace
