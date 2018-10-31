Imports CQRSAzure.EventSourcing

Partial Public Interface IAggregateImplementationMap(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits IAggregateImplementationMap

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
    Function CreateClassifierProcessor(Of TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})(ByVal aggregate As TAggregate, key As TAggregateKey, classifier As TClassifier) As IClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier)


    ''' <summary>
    ''' Create an identifier group processor to be used to identify members of an identifier group
    ''' </summary>
    Function CreateIdentifierGroupProcessor() As IIdentifierGroupProcessor(Of TAggregate, TAggregateKey)

End Interface
