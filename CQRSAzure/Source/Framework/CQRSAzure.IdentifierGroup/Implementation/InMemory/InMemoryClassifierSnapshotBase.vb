Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.InMemory
Imports System
Imports System.Collections.Concurrent
Imports System.Linq

Namespace InMemory

    Public MustInherit Class InMemoryClassifierSnapshotBase(Of TAggregate As IAggregationIdentifier,
                 TAggregateKey,
                 TClassifier As IClassifier)

        Private Shared m_snapshots As New ConcurrentDictionary(Of TAggregateKey, ConcurrentStack(Of IClassifierSnapshot))

        Private ReadOnly m_key As TAggregateKey
        Private ReadOnly m_debugMessages As Boolean

        Private Sub CreateSnapshotDictionaryIfNotCreated(ByVal key As TAggregateKey)
            If Not (m_snapshots.ContainsKey(key)) Then
                m_snapshots.AddOrUpdate(key, New ConcurrentStack(Of IClassifierSnapshot), Function(itemkey, oldValue) oldValue)
            End If
        End Sub

        Protected Sub AppendSnapshot(ByVal snapshotToAppend As IClassifierSnapshot)

            CreateSnapshotDictionaryIfNotCreated(m_key)

            If (m_snapshots(m_key).Count > 0) Then
                'check if the highest snapshot is greater than this one
                If (m_snapshots(m_key).Last().EffectiveSequenceNumber > snapshotToAppend.EffectiveSequenceNumber) Then
                    Throw New OutOfSequenceSnapshotException(DomainNameAttribute.GetDomainName(GetType(TAggregate)),
                                                             GetType(TAggregate).Name,
                                                             m_key.ToString(),
                                                             snapshotToAppend.EffectiveSequenceNumber,
                                                             m_snapshots(m_key).Last().EffectiveSequenceNumber)
                End If
            End If

            'Append it on the end
            m_snapshots(m_key).Push(snapshotToAppend)

        End Sub

        Protected Function GetSnapshot(Optional OnOrBeforeSequence As UInteger = 0) As IClassifierSnapshot

            CreateSnapshotDictionaryIfNotCreated(m_key)

            If (OnOrBeforeSequence = 0) Then
                'just get the last snapshot
                Return m_snapshots(m_key).LastOrDefault()
            Else
                Return m_snapshots(m_key).Where(Function(ByVal t As IClassifierSnapshot) t.EffectiveSequenceNumber <= OnOrBeforeSequence).LastOrDefault()
            End If

        End Function

        Protected Function GetSnapshot(Optional OnOrBeforeDate As Nullable(Of DateTime) = Nothing) As IClassifierSnapshot

            CreateSnapshotDictionaryIfNotCreated(m_key)

            If Not (OnOrBeforeDate.HasValue) Then
                'just get the last snapshot
                Return m_snapshots(m_key).LastOrDefault()
            Else
                Return m_snapshots(m_key).Where(Function(ByVal t As IClassifierSnapshot) t.EffectiveDateTime <= OnOrBeforeDate.Value).LastOrDefault()
            End If

        End Function

        Protected Sub New(ByVal aggregateIdentityKey As TAggregateKey,
                        Optional ByVal settings As IInMemorySettings = Nothing)

            m_key = aggregateIdentityKey

            If (settings IsNot Nothing) Then
                m_debugMessages = settings.DebugMessages
            End If

        End Sub
    End Class

End Namespace
