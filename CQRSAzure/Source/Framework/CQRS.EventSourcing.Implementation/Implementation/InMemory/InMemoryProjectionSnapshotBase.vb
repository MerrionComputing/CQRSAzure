Imports System.Collections.Generic
Imports System.Linq
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory
    ''' <summary>
    ''' Base class for saving projection snapshots in memory
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the base class to which the event stream is attached
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which an instance of that aggregation base class is uniquely identified
    ''' </typeparam>
    Public MustInherit Class InMemoryProjectionSnapshotBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)

        Protected ReadOnly m_debugMessages As Boolean
        ReadOnly m_key As TAggregateKey

        'The shared data area that backs this aggregate type's projection snapshots
        Protected Shared m_snapShots As New Dictionary(Of TAggregateKey, List(Of IProjectionSnapshot(Of TAggregate, TAggregateKey)))

        Private Sub CreateSnapshotDictionaryIfNotCreated()
            If Not (m_snapShots.ContainsKey(m_key)) Then
                m_snapShots.Add(m_key, New List(Of IProjectionSnapshot(Of TAggregate, TAggregateKey)))
            End If
        End Sub

        Protected Sub AppendSnapshot(ByVal snapshotToAppend As IProjectionSnapshot(Of TAggregate, TAggregateKey))

            CreateSnapshotDictionaryIfNotCreated()

            If (m_snapShots(m_key).Count > 0) Then
                'check if the highest snapshot is greater than this one
                If (m_snapShots(m_key).Last().Sequence > snapshotToAppend.Sequence) Then
                    Throw New OutOfSequenceSnapshotException(DomainNameAttribute.GetDomainName(GetType(TAggregate)),
                                                             GetType(TAggregate).Name,
                                                             m_key.ToString(),
                                                             snapshotToAppend.Sequence,
                                                             m_snapShots(m_key).Last().Sequence)
                End If
            End If

            'Append it on the end
            m_snapShots(m_key).Add(snapshotToAppend)

        End Sub

        Protected Function GetSnapshot(Optional OnOrBeforeSequence As UInteger = 0) As IProjectionSnapshot(Of TAggregate, TAggregateKey)

            CreateSnapshotDictionaryIfNotCreated()

            If (OnOrBeforeSequence = 0) Then
                'just get the last snapshot
                Return m_snapShots(m_key).LastOrDefault()
            Else
                Return m_snapShots(m_key).Where(Function(ByVal t As IProjectionSnapshot(Of TAggregate, TAggregateKey)) t.Sequence <= OnOrBeforeSequence).LastOrDefault()
            End If

        End Function

        Protected Sub New(ByVal aggregateType As TAggregate,
                          ByVal aggregateIdentityKey As TAggregateKey,
                          Optional ByVal settings As IInMemorySettings = Nothing)

            m_key = aggregateIdentityKey

            If (settings IsNot Nothing) Then
                m_debugMessages = settings.DebugMessages
            End If

            ' If we do not yet have an event stream for this aggregagate, create one
            CreateSnapshotDictionaryIfNotCreated()

        End Sub

    End Class
End Namespace