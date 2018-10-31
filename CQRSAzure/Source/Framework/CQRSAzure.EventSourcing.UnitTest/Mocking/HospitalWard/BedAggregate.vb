Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Mocking
    ''' <summary>
    ''' A business-realistic type of aggregate identifier for use in unit testing
    ''' </summary>
    <DomainName("HospitalWard")>
    <AggregateName("Bed")>
    <AggregateKeyDataType(GetType(Integer))>
    Public Class BedAggregate
        Implements IAggregationIdentifier(Of Integer)

        Private m_key As Integer

        Public Sub SetKey(key As Integer) Implements IAggregationIdentifier(Of Integer).SetKey
            m_key = key
        End Sub

        Public Function GetKey() As Integer Implements IAggregationIdentifier(Of Integer).GetKey
            Return m_key
        End Function

        Public Function GetAggregateIdentifier() As String Implements IAggregationIdentifier.GetAggregateIdentifier
            Return m_key.ToString()
        End Function

        Public Sub New(ByVal uniqueid As Integer)
            m_key = uniqueid
        End Sub

    End Class
End Namespace
