Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup
''' <summary>
''' Base class to be implemented by any class that supplies an identity group for an aggregate identifier
''' </summary>
Public MustInherit Class IdentityGroupBase(Of TAggregateIdentifier As IAggregationIdentifier, TAggregateKey)
    Implements IIdentityGroup

    Public Const GROUPNAME_INSTANCE As String = "Instance"
    Public Const GROUPNAME_ALL As String = "All"

    ''' <summary>
    ''' The class implementing the identity group must provide its name
    ''' </summary>
    Public MustOverride ReadOnly Property Name As String Implements IIdentityGroup.Name

    ''' <summary>
    ''' The classifier to run to (re)generate this identity group
    ''' </summary>
    ''' <remarks>
    ''' This may be null for "Identity" or "All" identity groups as these do not need a 
    ''' projection to decide if any entity is in or out of the group
    ''' </remarks>
    Public MustOverride ReadOnly Property Classifier As IClassifier(Of TAggregateIdentifier, TAggregateKey)


    ''' <summary>
    ''' Get the list of aggregate identifiers in the group as at teh given dat e(or as of now if no date is passed in)
    ''' </summary>
    ''' <param name="AsOfDate">
    ''' The effective date for which we want to know thwe group membership.
    ''' (If this is not passed in then as of current date and time is assumed)
    ''' </param>
    ''' <returns>
    ''' A collection of aggregate identifiers considered to be "inside" the group
    ''' </returns>
    Public MustOverride Function GetMembers(Optional ByVal AsOfDate As DateTime = Nothing) As IEnumerable(Of TAggregateIdentifier)

End Class
