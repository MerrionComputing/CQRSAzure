''' <summary>
''' A collection of zero or more aggregate identifiers over which the projection underlying a query
''' definition is to be run
''' </summary>
''' <remarks>
''' The group is uniquely named per aggregate identifier type, and is populated by its own projection which decides
''' if any given aggregate identifier is in or out of the group
''' </remarks>
Public Interface IIdentityGroup

    ''' <summary>
    ''' The unique name of the identity group
    ''' </summary>
    ''' <remarks>
    ''' This name can be passed as a parameter for a query definition.  There are two predefined names:-
    ''' "Identity" being the group of one specified aggregate identifier and "All" being the group of all 
    ''' instances of an aggregate identifier type.
    ''' </remarks>
    ReadOnly Property Name As String

End Interface
