Imports CQRSAzure.EventSourcing

''' <summary>
''' Interface to be implemented by any class that can process identifier groups to determine what entites are in the group as at a given point 
''' in time
''' </summary>
''' <typeparam name="TAggregate">
''' The type of the aggregate whose members may be in this group
''' </typeparam>
''' <typeparam name="TAggregateKey">
''' The type by which such instances are uniquely identifiable
''' </typeparam>
Public Interface IIdentifierGroupProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)


    ''' <summary>
    ''' Get the set of unique identifiers of the aggregates that are in this group as at a given point in time, or as at now if no
    ''' given point in time is specified
    ''' </summary>
    ''' <param name="IdentifierGroup">
    ''' The specific identifier group for which we are getting the membership set
    ''' </param>
    ''' <param name="effectiveDateTime">
    ''' If specified the date/time for which we want to get the members of the group
    ''' </param>
    ''' <param name="parentGroupProcessor">
    ''' If this is a "nested" identifier group, this is the processor to get the the members 
    ''' </param>
    ''' <remarks>
    ''' If the rules of a classifier are invariant (which they should be) it is possible to cache group members as at a given point in 
    ''' time so as to speed up the resolving of group membership
    ''' </remarks>
    Function GetMembers(ByVal IdentifierGroup As IIdentifierGroup(Of TAggregate),
                        Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing,
                        Optional ByVal parentGroupProcessor As IIdentifierGroupProcessor(Of TAggregate, TAggregateKey) = Nothing) As IEnumerable(Of TAggregateKey)

    ''' <summary>
    ''' Get the set of unique identifiers of the all of the aggregates that are in the system as at a given point in time, or as at now if no
    ''' given point in time is specified
    ''' </summary>
    ''' <param name="effectiveDateTime">
    ''' If specified the date/time for which we want to get the members of the group
    ''' </param>
    Function GetAll(Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of TAggregateKey)

End Interface
