Imports CQRSAzure.EventSourcing

''' <summary>
''' A snapshot of the membership of an identifier group as at a given point in time
''' </summary>
''' <typeparam name="TAggregate">
''' The type of the aggregate whose members may be in this group
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The type by which such instances are uniquely identifiable
''' </typeparam>
''' <remarks>
''' Note that this cannot rely on the event stream sequence number as different event streams may be at a different
''' sequence as at any given point in time.  
''' If the event streams have no effective-date properties in their events then the snapshots are qualified with an
''' "as of snapshot generated" time stamp.
''' </remarks>
Public Interface IIdentifierGroupSnapshot(Of TAggregate As IAggregationIdentifier,
                                              TAggregateKey)


    ''' <summary>
    ''' The effective date/time for the snapshot
    ''' </summary>
    ''' <remarks>
    ''' This will require the underlying events in the stream to have some as-of date property 
    ''' </remarks>
    ReadOnly Property AsOfDate As DateTime

    ''' <summary>
    ''' The set of unique identifiers that were in the identity group as at the point in time when the snapshot 
    ''' was taken
    ''' </summary>
    ReadOnly Property Members As IEnumerable(Of TAggregateKey)

End Interface
