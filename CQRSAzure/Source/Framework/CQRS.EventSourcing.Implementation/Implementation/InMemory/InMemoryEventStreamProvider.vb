Option Explicit On
Option Strict On
Imports System.Collections.Concurrent
Imports CQRSAzure.EventSourcing

Namespace InMemory

    Public Class InMemoryEventStreamProvider(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)
        Implements IEventStreamProvider(Of TAggregate, TaggregateKey)

        Protected Shared m_eventStreamCreation As New ConcurrentDictionary(Of TaggregateKey, DateTime)

        Private m_aggregatename As String
        Protected ReadOnly Property AggregateClassName As String Implements IEventStreamProvider(Of TAggregate, TaggregateKey).AggregateClassName
            Get
                If (String.IsNullOrWhiteSpace(m_aggregatename)) Then
                    Dim typeName As String = GetType(TAggregate).Name
                    m_aggregatename = typeName
                End If
                Return m_aggregatename
            End Get
        End Property

        Public Function GetAllStreamKeys(Optional asOfDate As Date? = Nothing) As IEnumerable(Of TaggregateKey) Implements IEventStreamProvider(Of TAggregate, TaggregateKey).GetAllStreamKeys

            If (asOfDate.HasValue) Then
                Dim ret As New List(Of TaggregateKey)
                For Each kvp In m_eventStreamCreation
                    If (kvp.Value >= asOfDate.GetValueOrDefault()) Then
                        ret.Add(kvp.Key)
                    End If
                Next
                Return ret
            Else
                Return m_eventStreamCreation.Keys
            End If

        End Function

    End Class
End Namespace