Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup


Public MustInherit Class IdentifierGroupProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits IdentifierGroupProcessor
    Implements IIdentifierGroupProcessor(Of TAggregate, TAggregateKey)

    Protected ReadOnly m_classifierFilterProvider As IClassifierFilterProvider(Of TAggregate, TAggregateKey)

    Public MustOverride Function GetAll(Optional effectiveDateTime As Date? = Nothing) As IEnumerable(Of TAggregateKey) Implements IIdentifierGroupProcessor(Of TAggregate, TAggregateKey).GetAll


    Public MustOverride Function GetMembers(IdentifierGroup As IIdentifierGroup(Of TAggregate),
                               Optional effectiveDateTime As Date? = Nothing,
                               Optional ByVal parentGroupProcessor As IIdentifierGroupProcessor(Of TAggregate, TAggregateKey) = Nothing) As IEnumerable(Of TAggregateKey) Implements IIdentifierGroupProcessor(Of TAggregate, TAggregateKey).GetMembers



    Public Sub New(ByVal classifierFilterProvider As IClassifierFilterProvider(Of TAggregate, TAggregateKey))

        m_classifierFilterProvider = classifierFilterProvider

    End Sub

End Class

''' <summary>
''' Class for the common parts of an identifier group classifier that do not need to know about the 
''' </summary>
Public MustInherit Class IdentifierGroupProcessor

    ''' <summary>
    ''' Identity group name that represents all the existing instances of an aggregate type (at a given point in time)
    ''' </summary>
    Public Const IDENTIFIER_GROUP_NAME_ALL As String = "All"
    ''' <summary>
    ''' identity group name that represents just one instance of an aggregate type
    ''' </summary>
    Public Const IDENTIFIER_GROUP_NAME_UNITY As String = "Instance"

End Class