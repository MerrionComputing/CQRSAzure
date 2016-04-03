Option Explicit On

Imports CQRSAzure.EventSourcing
''' <summary>
''' An identity group that represents one (and only one) instance of the aggregate identifier
''' </summary>
Public Class InstanceIdentityGroup(Of TAggregateIdentifier As IAggregationIdentifier, TAggregateKey)
    Inherits IdentityGroupBase(Of TAggregateIdentifier, TAggregateKey)

    Private ReadOnly m_instance As TAggregateIdentifier

    ''' <summary>
    ''' Always use the name "Instance" for this group of one and only one aggregate identifier
    ''' </summary>
    Public NotOverridable Overrides ReadOnly Property Name As String
        Get
            Return GROUPNAME_INSTANCE
        End Get
    End Property

    ''' <summary>
    ''' There is no projection for this group as only the specified identifier can be in the group
    ''' </summary>
    Public Overrides ReadOnly Property Classifier As IClassifier(Of TAggregateIdentifier, TAggregateKey)
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Returns the one instance this group contains
    ''' </summary>
    ''' <param name="AsOfDate">
    ''' This is meaningless for the instance group and is ignored
    ''' </param>
    ''' <returns>
    ''' A collection of just the one defiend instance of the aggregate identifier
    ''' </returns>
    Public Overrides Function GetMembers(Optional AsOfDate As Date = #1/1/0001 12:00:00 AM#) As IEnumerable(Of TAggregateIdentifier)
        Return {m_instance}
    End Function

    Public Sub New(ByVal instance As TAggregateIdentifier)
        m_instance = instance
    End Sub

End Class
