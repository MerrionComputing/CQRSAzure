Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Mocking

    ''' <summary>
    ''' A business-realistic type of aggregate identifier for use in unit testing
    ''' </summary>
    <DomainName("HospitalWard")>
    <AggregateName("Patient")>
    Public Class PatientAggregate
        Implements IAggregationIdentifier(Of String)

        Private m_key As String
        Public Sub SetKey(key As String) Implements IAggregationIdentifier(Of String).SetKey
            m_key = key
        End Sub

        Public Function GetKey() As String Implements IAggregationIdentifier(Of String).GetKey
            Return m_key
        End Function

        Public Function GetAggregateIdentifier() As String Implements IAggregationIdentifier.GetAggregateIdentifier
            Return m_key
        End Function

        Public Sub New(ByVal uniqueid As String)
            m_key = uniqueid
        End Sub
    End Class
End Namespace
