Imports System.Collections.Concurrent
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory
    ''' <summary>
    ''' Base class for storing an identifier group snapshot in-memory
    ''' </summary>
    Public MustInherit Class InMemoryIdentifierGroupSnapshotBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)

        Private Shared m_snapshots As New ConcurrentDictionary(Of String, ConcurrentStack(Of IIdentifierGroupSnapshot(Of TAggregate, TAggregateKey)))

        Private ReadOnly m_debugMessages As Boolean
        Private ReadOnly m_groupName As String


        Protected Sub New(ByVal groupName As String,
                         Optional ByVal settings As IInMemorySettings = Nothing)


            m_groupName = groupName

            If (settings IsNot Nothing) Then
                m_debugMessages = settings.DebugMessages
            End If

        End Sub

    End Class
End Namespace