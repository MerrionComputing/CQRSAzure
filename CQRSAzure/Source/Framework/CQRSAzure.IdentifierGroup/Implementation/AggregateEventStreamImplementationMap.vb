Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup

Partial Public Class AggregateEventStreamImplementationMap(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Implements IAggregateImplementationMap(Of TAggregate, TAggregateKey)


    ''' <summary>
    ''' Create a classifier processor to run over event streams of this aggregate
    ''' </summary>
    ''' <param name="aggregate">
    ''' The instance of the aggregate for which we want to run projections
    ''' </param>
    ''' <param name="key">
    ''' The unique identifier of the instance of the aggregate for which we want to run projections
    ''' </param>
    ''' <param name="classifier">
    ''' The classifier to process
    ''' </param>
    Public Function CreateClassifierProcessor(Of TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New}) _
                                             (aggregate As TAggregate,
                                              key As TAggregateKey,
                                              classifier As TClassifier) As IClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier) _
                                              Implements IAggregateImplementationMap(Of TAggregate, TAggregateKey).CreateClassifierProcessor


        Throw New NotImplementedException()
    End Function


    ''' <summary>
    ''' Create an identifier group processor to be used to identify members of an identifier group
    ''' </summary>
    Public Function CreateIdentifierGroupProcessor() As IIdentifierGroupProcessor(Of TAggregate, TAggregateKey) _
        Implements IAggregateImplementationMap(Of TAggregate, TAggregateKey).CreateIdentifierGroupProcessor

        Throw New NotImplementedException()
    End Function
End Class
