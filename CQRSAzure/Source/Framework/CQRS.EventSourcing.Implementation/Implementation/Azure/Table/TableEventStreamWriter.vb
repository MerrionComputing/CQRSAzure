Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports CQRSAzure.EventSourcing.Azure.Table
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table
    Public Class TableEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits TableEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamWriter(Of TAggregate, TAggregateKey)

#Region "Event stream information"
        Public ReadOnly Property Key As TAggregateKey Implements IEventStream(Of TAggregate, TAggregateKey).Key, IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return m_key
            End Get
        End Property

        Private nextSequence As Long = 0
        Public ReadOnly Property RecordCount As ULong Implements IEventStream(Of TAggregate, TAggregateKey).RecordCount
            Get
                Return nextSequence
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStream(Of TAggregate, TAggregateKey).LastAddition
            Get
                Throw New NotImplementedException()
            End Get
        End Property

#End Region



        Public Async Function AppendEvent(EventToAppend As IEvent(Of TAggregate),
                               Optional ByVal ExpectedTopSequence As Long = 0) As Task Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvent

            'Set the next highest sequence (in case another writer has appended events)
            nextSequence = 1 + Await GetCurrentHighestSequence(m_converter.ToUniqueString(m_key))

            'Update the current highest event number
            UpdateSequenceNumber(nextSequence)

            'Append the event
            Await AppendEventInternal(EventToAppend)


        End Function



        Private Async Function AppendEventInternal(EventToAppend As IEvent(Of TAggregate)) As Task

            If (MyBase.Table IsNot Nothing) Then
                'Wrap the event in its context information

                Dim commentary As String = ""
                Dim source As String = ""
                Dim who As String = ""

                If (m_context IsNot Nothing) Then
                    commentary = m_context.Commentary
                    source = m_context.Source
                    who = m_context.Who
                End If

                'make an event instance...
                Dim eventInstance = InstanceWrappedEvent(Of TAggregateKey).Wrap(m_key, EventToAppend, EventToAppend.Version)

                Dim wrappedEvent = ContextWrappedEvent(Of TAggregateKey).Wrap(eventInstance,
                                                                                nextSequence,
                                                                                commentary,
                                                                                source,
                                                                                DateTime.UtcNow,
                                                                                eventInstance.Version,
                                                                                who)

                If (wrappedEvent IsNot Nothing) Then
                    'And add that to the table
                    Await MyBase.Table.ExecuteAsync(TableOperation.Insert(MakeDynamicTableEntity(wrappedEvent))) ', MyBase.RequestOptions)
                    'and increment the next sequence number
                    nextSequence += 1
                End If


            End If

        End Function

        Public Async Function AppendEvents(StartingSequence As Long, Events As IEnumerable(Of IEvent(Of TAggregate))) As Task Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvents

            'Set the next highest sequence (in case another writer has appended events)
            nextSequence = 1 + Await GetCurrentHighestSequence(m_converter.ToUniqueString(m_key))

            If (Events IsNot Nothing) Then
                If (Events.Count > 0) Then
                    If (StartingSequence < nextSequence) Then
                        Throw New ArgumentException("Out of sequence event(s) appended")
                    Else
                        'Set the current sequence number
                        nextSequence = StartingSequence
                        UpdateSequenceNumber(nextSequence)
                        For Each evt In Events
                            Await AppendEventInternal(evt)
                        Next
                    End If
                End If
            End If

        End Function

        Private Overloads Sub UpdateSequenceNumber(nextSequence As Long)

            MyBase.UpdateSequenceNumber(nextSequence, m_converter.ToUniqueString(m_key))

        End Sub

        Private m_context As IWriteContext
        Public Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).SetContext
            m_context = writerContext
        End Sub

        ''' <summary>
        ''' Clear out this event stream 
        ''' </summary>
        Public Sub Reset()
            If MyBase.Table IsNot Nothing Then
                'A Batch Operation allows a maximum 100 entities in the batch which must share the same PartitionKey 
                Dim projectionQuery = New TableQuery(Of DynamicTableEntity)().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, MyBase.AggregateKey)).Select({"RowKey"}).Take(100)

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
                UpdateSequenceNumber(nextSequence)
            End If
        End Sub



        Private Sub New(ByVal AggregateDomainName As String, ByVal AggregateKey As TAggregateKey,
                        Optional ByVal settings As ITableSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=True,
                       connectionStringName:=GetWriteConnectionStringName("", settings),
                       settings:=settings)


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
                                      Optional ByVal settings As ITableSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If

            Return New TableEventStreamWriter(Of TAggregate, TAggregateKey)(domainName, instance.GetKey(), settings)

        End Function

#End Region

    End Class

    Public Module TableEventStreamWriterFactory

        ''' <summary>
        ''' Creates an azure blob storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)(ByVal instance As TAggregate,
                                      ByVal key As TAggregateKey,
                                      Optional ByVal settings As ITableSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Return TableEventStreamWriter(Of TAggregate, TAggregateKey).Create(instance, settings)

        End Function

        ''' <summary>
        ''' Generate a function that can be used to create an event stream writer of the given type
        ''' </summary>
        ''' <typeparam name="TAggregate">
        ''' The data type of the aggregate class
        ''' </typeparam>
        ''' <typeparam name="TAggregateKey">
        ''' The data type that provides the unique identification of an instance of the reader class
        ''' </typeparam>
        Public Function GenerateCreationFunctionDelegate(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)() As IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey)


            'Make delegate for this module Create() function....
            Return New IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey)(AddressOf Create(Of TAggregate, TAggregateKey))

        End Function

    End Module

End Namespace