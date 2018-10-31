Imports CQRSAzure.EventSourcing
''' <summary>
''' A collection of zero or more aggregate identifiers over which the projection underlying a query
''' definition is to be run
''' </summary>
''' <remarks>
''' The group is uniquely named per aggregate identifier type, and is populated by its own projection which decides
''' if any given aggregate identifier is in or out of the group
''' </remarks>
Public Interface IIdentifierGroup

    ''' <summary>
    ''' The unique name of the identity group
    ''' </summary>
    ''' <remarks>
    ''' This name can be passed as a parameter for a query definition.  There are two predefined names:-
    ''' "Identity" being the group of one specified aggregate identifier and "All" being the group of all 
    ''' instances of an aggregate identifier type.
    ''' </remarks>
    ReadOnly Property Name As String

    ''' <summary>
    ''' The name of the outer parent group of which all members must be members of to be checked for membership
    ''' of this group
    ''' </summary>
    ''' <remarks>
    ''' This can be used to speed up evaluation of group membership by starting from a smaller intital group than "All"
    ''' If not set then "All" is assumed
    ''' </remarks>
    ReadOnly Property ParentGroupName As String

End Interface

Public Interface IIdentifierGroup(Of TAggregateIdentifier As IAggregationIdentifier)
    Inherits IIdentifierGroup

End Interface

''' <summary>
''' A collection of zero or more aggregate identifiers over which the projection underlying a query
''' definition is to be run
''' </summary>
''' <remarks>
''' The group is uniquely named per aggregate identifier type, and is populated by its own projection which decides
''' if any given aggregate identifier is in or out of the group
''' </remarks>
Public Interface IIdentifierGroup(Of TAggregateIdentifier As IAggregationIdentifier, TAggregateKey)
    Inherits IIdentifierGroup(Of TAggregateIdentifier)

End Interface

Public Interface IIdentifierGroupUntyped
    Inherits IIdentifierGroup

End Interface