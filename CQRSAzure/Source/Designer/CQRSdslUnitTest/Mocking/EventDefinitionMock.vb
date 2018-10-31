Imports CQRSAzure.CQRSdsl.Dsl
Imports Microsoft.VisualStudio.Modeling

''' <summary>
''' Function to generate mock EventDefinition instances for testing
''' </summary>
Module EventDefinitionMock

    Public Function CreateEventDefinition(ByVal name As String) As EventDefinition

        Dim aggStore As New Store(GetType(CQRSdslDomainModel))
        If (aggStore IsNot Nothing) Then
            Dim aggPartition As New Partition(aggStore)
            If (aggPartition IsNot Nothing) Then
                Dim eventNameProperty As New PropertyAssignment(EventDefinition.NameDomainPropertyId, name)

                Dim ret As EventDefinition = Nothing

                Dim aggTran As Microsoft.VisualStudio.Modeling.Transaction = aggStore.TransactionManager.BeginTransaction("Create test event definition")

                ret = New EventDefinition(aggPartition, eventNameProperty)

                aggTran.Commit()
                Return ret
            End If
        End If

        Return Nothing

    End Function

End Module
