Imports System
Imports System.Collections.Generic
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory.Untyped


    Public MustInherit Class InMemoryEventStreamBaseUntyped
        Implements IEventStreamUntypedIdentity
        Implements IEventStreamProviderUntyped


        Private ReadOnly m_domainName As String
        Public ReadOnly Property DomainName As String Implements IEventStreamUntypedIdentity.DomainName
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Private ReadOnly m_aggregateTpeName As String
        Public ReadOnly Property AggregateTypeName As String Implements IEventStreamUntypedIdentity.AggregateTypeName
            Get
                Return m_aggregateTpeName
            End Get
        End Property

        Private ReadOnly m_InstanceKey As String
        Public ReadOnly Property InstanceKey As String Implements IEventStreamUntypedIdentity.InstanceKey
            Get
                Return m_InstanceKey
            End Get
        End Property

        Public ReadOnly Property AggregateClassName As String Implements IEventStreamProviderUntyped.AggregateClassName
            Get
                Return m_aggregateTpeName
            End Get
        End Property



        Public Sub New(ByVal identifier As IEventStreamUntypedIdentity,
                       Optional ByVal writeAccess As Boolean = False,
                       Optional ByVal settings As IInMemorySettings = Nothing)

            m_domainName = identifier.DomainName
            m_aggregateTpeName = identifier.AggregateTypeName
            m_InstanceKey = identifier.InstanceKey



        End Sub

        Public Function GetAllStreamKeys(Optional asOfDate As Date? = Nothing) As IEnumerable(Of String) Implements IEventStreamProviderUntyped.GetAllStreamKeys
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace