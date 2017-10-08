''' <summary>
''' Interface for any class that can process projections over an event stream
''' </summary>
''' <typeparam name="TAggregate">
''' The aggregate type providing the underlying event stream
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The type for the key that uniquely identifies an instance of thaqt aggregate type
''' </typeparam>
Public Interface IProjectionProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)

    ''' <summary>
    ''' Process the given projection using the event stream reader we have set up
    ''' </summary>
    ''' <param name="projectionToProcess">
    ''' The class that defines the projection operation we are going to process
    ''' </param>
    Sub Process(ByVal projectionToProcess As IProjection(Of TAggregate, TAggregateKey))

End Interface
