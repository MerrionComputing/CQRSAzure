Imports CQRSAzure.EventSourcing
Imports CQRSAzure.IdentifierGroup

''' <summary>
''' A class to perform the classifier filter provider functionality locally - i.e. all the classifiers are run in the same process as the 
''' identifier group processor that is using them to get the group membership
''' </summary>
Public NotInheritable Class LocalClassifierFilterProvider(Of TAggregate As IAggregationIdentifier,
                                                    TAggregateKey,
                                                    TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})
    Implements IClassifierFilterProvider(Of TAggregate,
                                          TAggregateKey,
                                          TClassifier)


    'Need some factory to create classifier processors...?
    Private ReadOnly m_factory As IClassifierProcessorFactory(Of TAggregate, TAggregateKey, TClassifier)

    Public Function GetMembers(setToFilter As IEnumerable(Of TAggregateKey),
                               Optional effectiveDateTime As Date? = Nothing) As IEnumerable(Of TAggregateKey) Implements IClassifierFilterProvider(Of TAggregate, TAggregateKey).GetMembers


        If (m_factory Is Nothing) Then
#Region "Tracing"
            IdentifierGroup.LogError("Unable to create an identity group classifier as the factory is not initialised")
#End Region
            'Cannot classify the member
            Throw New Exceptions.ClassifierProcessorFactoryMissingException("No factory specified when creating this Local Classifier Filter")
        Else

            Dim filterMemberQuery = setToFilter.AsParallel().Where(Function(ByVal key As TAggregateKey)
                                                                       Return IsMember(key, effectiveDateTime)
                                                                   End Function)

            Return filterMemberQuery.AsEnumerable()


        End If
    End Function

    Public Function IsMember(identifierToTest As TAggregateKey,
                             Optional effectiveDateTime As Date? = Nothing) As Boolean Implements IClassifierFilterProvider(Of TAggregate, TAggregateKey).IsMember


        If (m_factory Is Nothing) Then
            'Cannot classify the member
            Throw New Exceptions.ClassifierProcessorFactoryMissingException("No factory specified when creating this Local Classifier Filter")
        Else
#Region "Tracing"
            IdentifierGroup.LogVerboseInfo("Checking if " & identifierToTest.ToString() & " is a member of this identifier group ")
#End Region
            'Use the factory to create a classifier and run it
            Return m_factory.GetClassifierFilterProvider(identifierToTest).Classify(effectiveDateTime:=effectiveDateTime)
        End If

        Return False

    End Function




    Private Sub New(ByVal filterProviderFactory As IClassifierProcessorFactory(Of TAggregate, TAggregateKey, TClassifier))
        m_factory = filterProviderFactory

    End Sub

End Class
