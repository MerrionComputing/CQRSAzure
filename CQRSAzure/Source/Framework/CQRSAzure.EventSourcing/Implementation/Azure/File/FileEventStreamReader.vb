Imports System.Runtime.Serialization.Formatters.Binary
Imports Microsoft.WindowsAzure.Storage.File

Namespace Azure.File

    ''' <summary>
    ''' Class to read events from an event stream implemented as a windows azure append blob
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate that the event stream belongs to
    ''' </typeparam>
    ''' <typeparam name="TAggregationKey">
    ''' The type which provides the unique key by which to recognise an instance of the aggregate
    ''' </typeparam>
    Public Class FileEventStreamReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
        Inherits FileEventStreamBase(Of TAggregate, TAggregationKey)
        Implements IEventStreamReader(Of TAggregate, TAggregationKey)

        Public Function GetEvents() As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregationKey).GetEvents

            If (MyBase.File IsNot Nothing) Then
                Dim ret As New List(Of IEvent(Of TAggregate))
                Dim bf As New BinaryFormatter()
                Dim currentLength As Long = GetSequence()
                Using fs As System.IO.Stream = MyBase.File.OpenRead()
                    fs.Seek(0, IO.SeekOrigin.Begin)
                    While Not (fs.Position >= currentLength)
                        Dim record As FileBlockWrappedEvent = CTypeDynamic(Of FileBlockWrappedEvent)(bf.Deserialize(fs))
                        If (record IsNot Nothing) Then
                            ret.Add(record.EventInstance)
                        End If
                    End While
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, MyBase.m_key.ToString(), 0, "Unable to read events - Azure file not initialised")
            End If

        End Function

        Public Function GetEvents(StartingVersion As UInteger) As IEnumerable(Of IEvent(Of TAggregate)) Implements IEventStreamReader(Of TAggregate, TAggregationKey).GetEvents

            If (MyBase.File IsNot Nothing) Then
                Dim ret As New List(Of IEvent(Of TAggregate))
                Dim bf As New BinaryFormatter()
                Dim currentLength As Long = GetSequence()
                Using fs As System.IO.Stream = MyBase.File.OpenRead()
                    'Go to the starting 
                    fs.Seek(StartingVersion, IO.SeekOrigin.Begin)
                    While Not (fs.Position >= currentLength)
                        Dim record As FileBlockWrappedEvent = CTypeDynamic(Of FileBlockWrappedEvent)(bf.Deserialize(fs))
                        If (record IsNot Nothing) Then
                            ret.Add(record.EventInstance)
                        End If
                    End While
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, MyBase.m_key.ToString(), 0, "Unable to read events - Azure file not initialised")
            End If

        End Function

        Public Function GetEventsWithContext() As IEnumerable(Of IEventContext) Implements IEventStreamReader(Of TAggregate, TAggregationKey).GetEventsWithContext

            If (MyBase.File IsNot Nothing) Then
                Dim currentLength As Long = GetSequence()
                Dim ret As New List(Of IEventContext)
                Dim bf As New BinaryFormatter()
                Using fs As System.IO.Stream = MyBase.File.OpenRead()
                    fs.Seek(0, IO.SeekOrigin.Begin)
                    While Not (fs.Position >= currentLength)
                        Dim record As FileBlockWrappedEvent = CTypeDynamic(Of FileBlockWrappedEvent)(bf.Deserialize(fs))
                        If (record IsNot Nothing) Then
                            Dim instance = InstanceWrappedEvent(Of TAggregationKey).Wrap(m_key, record.EventInstance, record.Version)
                            ret.Add(ContextWrappedEvent(Of TAggregationKey).Wrap(instance, record.Sequence, "", "", record.Timestamp, record.Version, ""))
                        End If
                    End While
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName, AggregateClassName, MyBase.m_key.ToString(), 0, "Unable to read events - Azure file not initialised")
            End If

        End Function

        ''' <summary>
        ''' Create a new windows azure file stream reader to read events from the file
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The domain in which the aggregate resides
        ''' </param>
        ''' <remarks>
        ''' The unique key which identifies the instance of the aggregate to read the event stream for
        ''' </remarks>
        Private Sub New(ByVal AggregateDomainName As String, ByVal AggregateKey As TAggregationKey)
            MyBase.New(AggregateDomainName, AggregateKey)

        End Sub


#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As IEventStreamReader(Of TAggregate, TAggregationKey)

            Return New FileEventStreamReader(Of TAggregate, TAggregationKey)(DomainNameAttribute.GetDomainName(instance), instance.GetKey())

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an azure blob backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run projections
        ''' </param>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As ProjectionProcessor(Of TAggregate, TAggregationKey)

            Return New ProjectionProcessor(Of TAggregate, TAggregationKey)(Create(instance))

        End Function
#End Region
    End Class
End Namespace