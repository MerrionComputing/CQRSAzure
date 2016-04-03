''' <summary>
''' A mapping between an aggregate identifier and the event stream techniology used as backing to it
''' </summary>
''' <typeparam name="TAggregate">
''' </typeparam>
''' <remarks>
''' This allows different aggregate types to have different backing store technologies
''' </remarks>
Public Class AggregateEventStreamImplementationMap(Of TAggregate As IAggregationIdentifier, TAggregateKey)


    Private ReadOnly m_readerCreator As ReaderCreationFunction(Of TAggregate, TAggregateKey)
    Private ReadOnly m_writerCreator As WriterCreationFunction(Of TAggregate, TAggregateKey)

    Public Function CreateReader(ByVal aggregate As TAggregate) As IEventStreamReader(Of TAggregate, TAggregateKey)

        If (m_readerCreator IsNot Nothing) Then
            Return m_readerCreator.Invoke(aggregate)
        Else
            'Notify an unmapped aggregate exists
            Throw New UnmappedAggregateException(aggregate.GetType.Name, aggregate.GetAggregateIdentifier)
        End If

    End Function

    Public Function CreateWriter(ByVal aggregate As TAggregate) As IEventStreamWriter(Of TAggregate, TAggregateKey)

        If (m_writerCreator IsNot Nothing) Then
            Return m_writerCreator.Invoke(aggregate)
        Else
            'Notify an unmapped aggregate exists
            Throw New UnmappedAggregateException(aggregate.GetType.Name, aggregate.GetAggregateIdentifier)
        End If

    End Function

    Private Sub New(ByVal readerCreator As ReaderCreationFunction(Of TAggregate, TAggregateKey),
                    ByVal writerCreator As WriterCreationFunction(Of TAggregate, TAggregateKey))

        m_readerCreator = readerCreator
        m_writerCreator = writerCreator

    End Sub

End Class
