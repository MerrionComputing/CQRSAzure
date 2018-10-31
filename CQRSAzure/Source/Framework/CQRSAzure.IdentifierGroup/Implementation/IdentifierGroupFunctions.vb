Imports CQRSAzure.EventSourcing
''' <summary>
''' Function delegates to allow the different implementations to be used to spin up a processor to perform the 
''' classification and group membership evaluation process
''' </summary>
''' <remarks>
''' This is required to segregate the business functionality (projections, classifiers, query definitions etc.) from the 
''' implementation functionality (readers, writers, snapshots etc.) so each can be tested independently
''' </remarks>
Public Module IdentifierGroupFunctions

    ''' <summary>
    ''' Function to create a classifier processor for the given aggregate and classifier given the unique identifier of the aggregate
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the aggregate that owns the event stream that is being read to run the classifier
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which that aggregate can be uniquely identified
    ''' </typeparam>
    ''' <typeparam name="TClassifier">
    ''' The classifier function being run to decide if an instance is inside or outside the group
    ''' </typeparam>
    ''' <param name="instance">
    ''' The key by which the unique instance we are testing is identified
    ''' </param>
    ''' <returns>
    ''' A function that creates the classifier processor that would be needed to run this classification over the given aggregate isnatnce's
    ''' event stream to classify it.
    ''' </returns>
    Delegate Function CreateDefaultClassifierProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey, TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})(ByVal instance As TAggregateKey) As IClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier)


    ''' <summary>
    ''' Function to create an identifier group processor for the 
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the aggregate that owns the event stream that is being read to run the classifier
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which that aggregate can be uniquely identified
    ''' </typeparam> 
    ''' <typeparam name="TGroup">
    ''' The class of the group that we are processing
    ''' </typeparam>
    ''' <param name="groupInstance">
    ''' The specific instance of the group to create a processor for
    ''' </param>
    Delegate Function CreateDefaultIdentifierGroupProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey, TGroup As IIdentifierGroup(Of TAggregate))(ByVal groupInstance As TGroup) As IIdentifierGroupProcessor(Of TAggregate, TAggregateKey)

End Module
