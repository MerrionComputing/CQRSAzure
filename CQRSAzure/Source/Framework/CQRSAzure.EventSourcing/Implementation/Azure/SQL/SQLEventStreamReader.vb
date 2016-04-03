Namespace Azure.SQL
    Public Class SQLEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
        Inherits SQLEventStreamBase(Of TAggregate, TAggregationKey)
        Implements IEventStreamReader(Of TAggregate, TAggregationKey)

        Public Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregationKey).GetEvents
            Throw New NotImplementedException()
        End Function

        Public Function GetEvents(StartingVersion As UInteger) As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregationKey).GetEvents
            Throw New NotImplementedException()
        End Function

        Public Function GetEventsWithContext() As IEnumerable(Of IEventContext) Implements IEventStreamReader(Of TAggregate, TAggregationKey).GetEventsWithContext
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
        Private Sub New(ByVal AggregateDomainName As String, ByVal AggregateKey As TAggregationKey)
            MyBase.New(AggregateDomainName, AggregateKey)

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
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As IEventStreamReader(Of TAggregate, TAggregationKey)

            Return New SQLEventStreamReader(Of TAggregate, TAggregationKey)(DomainNameAttribute.GetDomainName(instance), instance.GetKey())

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
        Public Shared Function CreateProjectionProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As ProjectionProcessor(Of TAggregate, TAggregationKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregationKey)(Create(instance))

        End Function

#End Region
    End Class
End Namespace