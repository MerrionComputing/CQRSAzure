Imports CQRSAzure.EventSourcing
''' <summary>
''' Interface to create classifier processors 
''' </summary>
Public Interface IClassifierProcessorFactory(Of TAggregate As IAggregationIdentifier,
                                                    TAggregateKey,
                                                    TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})


    ''' <summary>
    ''' Get a classifier filter provider to use to create classifiers for a group membership function
    ''' </summary>
    ''' <param name="key">
    ''' The unique key of the aggregate over which the classifier will be executed
    ''' </param>
    Function GetClassifierFilterProvider(ByVal key As TAggregateKey) As IClassifierProcessor(Of TAggregate,
                                          TAggregateKey,
                                          TClassifier)


End Interface


