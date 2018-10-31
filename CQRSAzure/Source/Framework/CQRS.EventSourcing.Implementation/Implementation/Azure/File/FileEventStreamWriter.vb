Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage.File

Namespace Azure.File

    ''' <summary>
    ''' Implementation class to write to an event stream based on an Azure File backing store
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the aggregate this event stream is attached to
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type of the key that uniquely identifies an instance of that key
    ''' </typeparam>
    Public Class FileEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits FileEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamWriter(Of TAggregate, TAggregateKey)

#Region "Event stream properties"
        Public ReadOnly Property Key As TAggregateKey Implements IEventStream(Of TAggregate, TAggregateKey).Key, IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return MyBase.m_key
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStream(Of TAggregate, TAggregateKey).LastAddition
            Get
                If (MyBase.File IsNot Nothing) Then
                    If MyBase.File.Properties.LastModified.HasValue Then
                        Return MyBase.File.Properties.LastModified.Value.Date
                    End If
                End If
            End Get
        End Property

        Public ReadOnly Property RecordCount As ULong Implements IEventStream(Of TAggregate, TAggregateKey).RecordCount
            Get
                If (MyBase.File IsNot Nothing) Then
                    Return MyBase.GetRecordCount()
                Else
                    'No file means no records
                    Return 0
                End If
            End Get
        End Property

#End Region

        Public Sub AppendEvent(EventInstance As IEvent(Of TAggregate),
                               Optional ByVal ExpectedTopSequence As Long = 0) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvent

            If (MyBase.File IsNot Nothing) Then
                Dim evtToWrite As New FileBlockWrappedEvent(GetSequence(), EventInstance.Version, DateTime.UtcNow, EventInstance)
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

        Public Sub AppendEvents(StartingSequence As Long, Events As IEnumerable(Of IEvent(Of TAggregate))) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvents

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

        Private m_context As IWriteContext
        Public Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).SetContext
            m_context = writerContext
        End Sub

        ''' <summary>
        ''' Clear down the event stream
        ''' </summary>
        ''' <remarks>
        ''' This will delete existing events so should not be done in any production environment therefore this is not
        ''' part of the IEventStreamWriter interface
        ''' </remarks>
        Public Sub Reset()

            If (MyBase.File IsNot Nothing) Then
                MyBase.File.Delete()
                'Recreate the blob file that was deleted
                MyBase.ResetFile()
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
        Private Sub New(ByVal AggregateDomainName As String,
                        ByVal AggregateKey As TAggregateKey,
                        Optional ByVal settings As IFileStreamSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=True, connectionStringName:=GetWriteConnectionStringName("", settings), settings:=settings)



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
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IFileStreamSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)


            Return Create(instance, instance.GetKey(), settings)

        End Function


        ''' <summary>
        ''' Creates an azure file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                      ByVal key As TAggregateKey,
                                      Optional ByVal settings As IFileStreamSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New FileEventStreamWriter(Of TAggregate, TAggregateKey)(domainName, key, settings)

        End Function

#End Region

    End Class



    Public Module FileEventStreamWriterFactory

        ''' <summary>
        ''' Creates an azure file storage based event stream writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to write the event stream
        ''' </param>
        ''' <param name="settings">
        ''' Any additional settings 
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)(ByVal instance As TAggregate,
            ByVal key As TAggregateKey,
            Optional ByVal settings As IFileStreamSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Return FileEventStreamWriter(Of TAggregate, TAggregateKey).Create(instance, key, settings)

        End Function

        ''' <summary>
        ''' Generate a function that can be used to create an event stream writer of the given type
        ''' </summary>
        ''' <typeparam name="TAggregate">
        ''' The data type of the aggregate class
        ''' </typeparam>
        ''' <typeparam name="TAggregateKey">
        ''' The data type that provides the unique identification of an instance of the reader class
        ''' </typeparam>
        Public Function GenerateCreationFunctionDelegate(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)() As IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey)


            'Make delegate for this module Create() function....
            Return New IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey)(AddressOf Create(Of TAggregate, TAggregateKey))

        End Function

    End Module

End Namespace