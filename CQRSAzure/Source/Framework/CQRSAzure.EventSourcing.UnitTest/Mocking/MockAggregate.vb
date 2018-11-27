Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing.Implementation

Namespace Mocking
    <DomainName("UnitTest")>
    Public Class MockAggregate
        Implements IAggregationIdentifier(Of String)
        Implements IEventStreamUntypedIdentity

        Private m_key As String

        Public ReadOnly Property DomainName As String Implements IEventStreamUntypedIdentity.DomainName
            Get
                Return DomainNameAttribute.GetDomainName(Me)
            End Get
        End Property

        Public ReadOnly Property AggregateTypeName As String Implements IEventStreamUntypedIdentity.AggregateTypeName
            Get
                Return AggregateNameAttribute.GetAggregateName(Me)
            End Get
        End Property

        Public ReadOnly Property InstanceKey As String Implements IEventStreamUntypedIdentity.InstanceKey
            Get
                Return m_key
            End Get
        End Property

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