Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Mocking
    <DomainName("UnitTest")>
    Public Class MockAggregate
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

    <DomainName("UnitTest")>
    Public Class MockGuidAggregate
        Implements IAggregationIdentifier(Of Guid)

        Private converter As New GUIDKeyConverter()

        <DataMember(Name:="Key")>
        Private m_key As Guid
        Public Sub SetKey(key As Guid) Implements IAggregationIdentifier(Of Guid).SetKey
            m_key = key
        End Sub

        Public Function GetAggregateIdentifier() As String Implements IAggregationIdentifier.GetAggregateIdentifier
            Return converter.ToUniqueString(m_key)
        End Function

        Public Function GetKey() As Guid Implements IAggregationIdentifier(Of Guid).GetKey
            Return m_key
        End Function

        Public Sub New(ByVal uniquekey As Guid)
            m_key = uniquekey
        End Sub

    End Class

End Namespace