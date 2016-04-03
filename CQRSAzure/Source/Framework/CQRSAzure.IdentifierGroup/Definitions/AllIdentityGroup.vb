Option Explicit On

Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup

''' <summary>
''' An identity group that represents all of the instances of a given aggregate identifier known to the system
''' </summary>
''' <typeparam name="TAggregateIdentifier">
''' The type of aggregate identifier for which we are getting the group of all instances
''' </typeparam>
Public NotInheritable Class AllIdentityGroup(Of TAggregateIdentifier As IAggregationIdentifier, TAggregateKey)
    Inherits IdentityGroupBase(Of TAggregateIdentifier, TAggregateKey)

    Public Overrides ReadOnly Property Classifier As IClassifier(Of TAggregateIdentifier, TAggregateKey)
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    ''' <summary>
    ''' Always use the name "All" for the group name of all aggregate identifiers
    ''' </summary>
    ''' <returns></returns>
    Public Overrides ReadOnly Property Name As String
        Get
            Return GROUPNAME_ALL
        End Get
    End Property

    Public Overrides Function GetMembers(Optional AsOfDate As Date = #1/1/0001 12:00:00 AM#) As IEnumerable(Of TAggregateIdentifier)
        Throw New NotImplementedException()
    End Function
End Class
