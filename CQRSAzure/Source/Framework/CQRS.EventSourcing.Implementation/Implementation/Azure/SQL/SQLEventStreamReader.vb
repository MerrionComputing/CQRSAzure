Imports CQRSAzure.EventSourcing

Namespace Azure.SQL
    Public Class SQLEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits SQLEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamFilteredReader(Of TAggregate, TAggregateKey)

        'List of all the individual queries to populate event data
        Private eventQueries As New Dictionary(Of Type, SqlClient.SqlCommand)
        Private ReadOnly m_eventStreamQuery As SqlClient.SqlCommand

        Public ReadOnly Property Key As TAggregateKey Implements IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return MyBase.m_key
            End Get
        End Property

        Public Function IsEventValid(eventType As Type) As Boolean Implements IEventStreamFilteredReader(Of TAggregate, TAggregateKey).IsEventValid
            Throw New NotImplementedException()
        End Function

        Public Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents
            Throw New NotImplementedException()
        End Function

        Public Function GetEvents(Optional ByVal StartingVersion As UInteger = 0,
                                  Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEvents
            Throw New NotImplementedException()
        End Function

        Public Function GetEventsWithContext(Optional ByVal StartingVersion As UInteger = 0,
                                             Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of IEventContext) Implements IEventStreamReader(Of TAggregate, TAggregateKey).GetEventsWithContext
            Throw New NotImplementedException()
        End Function


        ''' <summary>
        ''' Create a new windows azure file stream reader to read events from the file
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The domain in which the aggregate resides
        ''' </param>
        ''' <remarks>
        ''' The unique key which identifies the instance of the aggregate to read the event stream for
        ''' </remarks>
        Private Sub New(ByVal AggregateDomainName As String,
                        ByVal AggregateKey As TAggregateKey,
                        Optional ByVal settings As ISQLSettings = Nothing,
                        Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                        Optional ByVal eventFilterFunction As FilterFunctions.EventFilterFunction = Nothing)

            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=False, connectionStringName:=GetReadConnectionStringName("", settings), settings:=settings)

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
                                      Optional ByVal settings As ISQLSettings = Nothing,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return New SQLEventStreamReader(Of TAggregate, TAggregateKey)(DomainNameAttribute.GetDomainName(instance),
                                                                            instance.GetKey()
                                                                            )

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
                                                         Optional ByVal settings As ISQLSettings = Nothing,
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregateKey)(Create(instance, settings, eventFilter))

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

    Public Module SQLEventStreamReaderFactory

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
                                      Optional ByVal settings As ISQLSettings = Nothing,
                                      Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As IEventStreamReader(Of TAggregate, TAggregateKey)

            Return SQLEventStreamReader(Of TAggregate, TAggregateKey).Create(instance, settings, eventFilter)

        End Function


        ''' <summary>
        ''' Create a projection processor that works off an SQL server backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Function CreateProjectionProcessor(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal instance As TAggregate,
                                                                                                                                    ByVal key As TAggregateKey,
                                                         Optional ByVal settings As ISQLSettings = Nothing,
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing) As ProjectionProcessor(Of TAggregate, TAggregateKey)

            Return SQLEventStreamReader(Of TAggregate, TAggregateKey).CreateProjectionProcessor(instance, settings, eventFilter)

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