Imports CQRSAzure.EventSourcing
''' <summary>
''' An interface for a provider that performs the classifier process for an identifier group processor to decide if the 
''' requested members are inside or outside of the identifier group
''' </summary>
''' <remarks>
''' This allows the provision of classifiers to be injected into the identifier group processor which allows for them, for example to be
''' scaled across machines or tested locally without altering and recompiling code.
''' Each identifier group processor will need to provide a default implementation in case there is no implementation passed in 
''' </remarks>
Public Interface IClassifierFilterProvider(Of TAggregate As IAggregationIdentifier,
                                          TAggregateKey,
                                          TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})
    Inherits IClassifierFilterProvider(Of TAggregate, TAggregateKey)




End Interface

Public Interface IClassifierFilterProvider(Of TAggregate As IAggregationIdentifier,
                                          TAggregateKey)

    ''' <summary>
    ''' Get all of the members of the input group that are classified as in the identifier group 
    ''' </summary>
    ''' <param name="setToFilter"></param>
    ''' <returns></returns>
    Function GetMembers(ByVal setToFilter As IEnumerable(Of TAggregateKey),
                        Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As IEnumerable(Of TAggregateKey)


    ''' <summary>
    ''' Is the given unique identifier a member of the identifier group 
    ''' </summary>
    ''' <param name="identifierToTest"></param>
    ''' <param name="effectiveDateTime"></param>
    ''' <returns></returns>
    Function IsMember(ByVal identifierToTest As TAggregateKey,
                      Optional ByVal effectiveDateTime As Nullable(Of DateTime) = Nothing) As Boolean

End Interface
