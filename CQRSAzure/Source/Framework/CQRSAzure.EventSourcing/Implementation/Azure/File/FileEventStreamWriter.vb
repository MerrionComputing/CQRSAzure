Imports Microsoft.WindowsAzure.Storage.File

Namespace Azure.File

    Public Class FileEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
        Inherits FileEventStreamBase(Of TAggregate, TAggregationKey)
        Implements IEventStreamWriter(Of TAggregate, TAggregationKey)

        Public ReadOnly Property Key As TAggregationKey Implements IEventStream(Of TAggregate, TAggregationKey).Key
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStream(Of TAggregate, TAggregationKey).LastAddition
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property RecordCount As ULong Implements IEventStream(Of TAggregate, TAggregationKey).RecordCount
            Get
                If (MyBase.File IsNot Nothing) Then
                    Return MyBase.GetRecordCount()
                Else
                    'No file means no records
                    Return 0
                End If
            End Get
        End Property

        Public Sub AppendEvent(EventInstance As IEvent(Of TAggregate)) Implements IEventStreamWriter(Of TAggregate, TAggregationKey).AppendEvent

            If (MyBase.File IsNot Nothing) Then
                Dim evtToWrite As New FileBlockWrappedEvent(GetSequence(), 0, DateTime.UtcNow, EventInstance)
                If (evtToWrite IsNot Nothing) Then
                    Using fs As CloudFileStream = MyBase.File.OpenWrite(Nothing)
                        fs.Seek(GetSequence(), IO.SeekOrigin.Begin)
                        'write the event to the stream here..
                        evtToWrite.WriteToBinaryStream(fs)
                        SetSequence(fs.Position)
                    End Using
                    IncrementRecordCount()
                End If
            End If

        End Sub

        Private Sub IncrementRecordCount()

            If (MyBase.File IsNot Nothing) Then
                MyBase.File.FetchAttributes()
                Dim m_records As Long = 0
                If (MyBase.File.Metadata.ContainsKey(METADATA_RECORD_COUNT)) Then
                    If (Long.TryParse(MyBase.File.Metadata(METADATA_RECORD_COUNT), m_records)) Then
                        m_records += 1
                    Else
                        m_records = 0
                    End If
                End If
                MyBase.File.Metadata(METADATA_RECORD_COUNT) = m_records.ToString()
                MyBase.File.SetMetadata()
            End If

        End Sub

        Public Sub AppendEvents(StartingSequence As UInteger, Events As IEnumerable(Of IEvent(Of TAggregate))) Implements IEventStreamWriter(Of TAggregate, TAggregationKey).AppendEvents

            If (Events IsNot Nothing) Then
                If (Events.Count > 0) Then
                    If (StartingSequence < GetSequence()) Then
                        Throw New ArgumentException("Out of sequence event(s) appended")
                    Else
                        'Set the current version to StartingVersion
                        SetSequence(StartingSequence)
                        For Each evt In Events
                            AppendEvent(evt)
                        Next
                    End If
                End If
            End If

        End Sub



        ''' <summary>
        ''' Create a new windows azure file stream reader to write events to the file
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
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As IEventStreamWriter(Of TAggregate, TAggregationKey)

            Return New FileEventStreamWriter(Of TAggregate, TAggregationKey)(DomainNameAttribute.GetDomainName(instance), instance.GetKey())

        End Function


#End Region

    End Class
End Namespace