Imports CQRSAzure.EventSourcing

Namespace Queries
    ''' <summary>
    ''' Aggregate identifier specifically for queries that are passed into the domain
    ''' </summary>
    Public Class QueryAggregate
        Implements IQueryAggregateIdentifier


        Private m_queryKey As Guid
        Public Sub SetKey(key As Guid) Implements IAggregationIdentifier(Of Guid).SetKey
            m_queryKey = key
        End Sub

        Public Function GetAggregateIdentifier() As String Implements IAggregationIdentifier.GetAggregateIdentifier
            Return m_queryKey.ToString("P")
        End Function

        Public Function GetKey() As Guid Implements IAggregationIdentifier(Of Guid).GetKey
            Return m_queryKey
        End Function

        Public Sub New(ByVal queryId As Guid)

            If (queryId = Guid.Empty) Then
                queryId = Guid.NewGuid
            End If
            m_queryKey = queryId

        End Sub

    End Class
End Namespace