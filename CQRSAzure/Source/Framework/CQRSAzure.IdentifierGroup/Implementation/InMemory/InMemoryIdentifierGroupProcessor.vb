Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory
    ''' <summary>
    ''' An identifier group processor that runs off in-memory based event streams
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The aggregate class for which we are getting identifier group members
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which instances of that aggregate are uniquely identified
    ''' </typeparam>
    Public NotInheritable Class InMemoryIdentifierGroupProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)
        Inherits IdentifierGroupProcessor(Of TAggregate, TAggregateKey)
        Implements IIdentifierGroupProcessor(Of TAggregate, TAggregateKey)

        Public Overrides Function GetAll(Optional effectiveDateTime As Date? = Nothing) As IEnumerable(Of TAggregateKey) Implements IIdentifierGroupProcessor(Of TAggregate, TAggregateKey).GetAll
            Throw New NotImplementedException()
        End Function

        Public Overrides Function GetMembers(IdentifierGroup As IIdentifierGroup(Of TAggregate),
                                   Optional effectiveDateTime As Date? = Nothing,
                                   Optional ByVal parentGroupProcessor As IIdentifierGroupProcessor(Of TAggregate, TAggregateKey) = Nothing) As IEnumerable(Of TAggregateKey) Implements IIdentifierGroupProcessor(Of TAggregate, TAggregateKey).GetMembers
            Throw New NotImplementedException()
        End Function

#Region "Constructors"
        Private Sub New()
            MyBase.New(CreateDefaultClassifierProvider())

        End Sub


        Private Sub New(ByVal classifierFilterProvider As IClassifierFilterProvider(Of TAggregate, TAggregateKey))
            MyBase.New(classifierFilterProvider)

        End Sub

#End Region

        ''' <summary>
        ''' Create the default classifier provider to use for this group processor
        ''' </summary>
        Private Shared Function CreateDefaultClassifierProvider() As IClassifierFilterProvider(Of TAggregate, TAggregateKey)
            Throw New NotImplementedException()
        End Function

    End Class
End Namespace
