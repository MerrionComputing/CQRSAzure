Imports System
Imports CQRSAzure.EventSourcing.Azure.File
Imports Microsoft.WindowsAzure.Storage.File

Namespace Azure.File

    ''' <summary>
    ''' Class to write events to an untyped event stream implemented as a windows Azure file
    ''' </summary>
    Public Class FileEventStreamWriterUntyped
        Inherits FileEventStreamBaseUntyped
        Implements IEventStreamWriterUntyped

        Public ReadOnly Property RecordCount As ULong Implements IEventStreamUntyped.RecordCount
            Get
                If (MyBase.File IsNot Nothing) Then
                    Return MyBase.GetRecordCount()
                Else
                    'No file means no records
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStreamUntyped.LastAddition
            Get
                If (MyBase.File IsNot Nothing) Then
                    If MyBase.File.Properties.LastModified.HasValue Then
                        Return MyBase.File.Properties.LastModified.Value.Date
                    End If
                End If
            End Get
        End Property

        Private m_context As IWriteContext
        Public Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriterUntyped.SetContext
            m_context = writerContext
        End Sub

        ''' <summary>
        ''' Append an event to the end of this event stream
        ''' </summary>
        ''' <param name="EventInstance">
        ''' The event to append - this will be wrapped in the current 
        ''' </param>
        ''' <param name="ExpectedTopSequence">
        ''' The expected highest sequence number in the stream - if this is set it can be used to verify that no asynchronous writes have 
        ''' occured that should prevent this write from occuring
        ''' </param>
        ''' <param name="Version">
        ''' Event version number of the event being written
        ''' </param>
        Public Async Function AppendEvent(EventInstance As IEvent,
                               Optional ExpectedTopSequence As Long = 0,
                               Optional Version As UInteger = 1) As Task Implements IEventStreamWriterUntyped.AppendEvent

            If (MyBase.File IsNot Nothing) Then
                If (EventInstance IsNot Nothing) Then
                    Dim actualTopSequence As Long = Await MyBase.GetSequence()

                    'Get the correlation id 
                    Dim correlationIdentifier As String = ""
                    If (m_context IsNot Nothing) Then
                        correlationIdentifier = m_context.CorrelationIdentifier
                    End If

                    Dim evtToWrite As New FileBlockJSonWrappedEvent(Await GetSequence(), Version, DateTime.UtcNow, EventInstance, correlationIdentifier)

                    ' Check if the sequence number matches our expectation

                    If ((ExpectedTopSequence > 0) AndAlso (actualTopSequence > ExpectedTopSequence)) Then
                        Throw New EventStreamWriteException(DomainName, AggregateTypeName, InstanceKey,
                                                        actualTopSequence,
                                                        $"Stream sequence error: expected {ExpectedTopSequence}, actual {actualTopSequence}")
                    End If

                    Using fs As CloudFileStream = MyBase.File.OpenWriteAsync(Nothing).Result
                        fs.Seek(Await GetSequence(), IO.SeekOrigin.Begin)
                        'write the event to the stream here..
                        evtToWrite.WriteToBinaryStream(fs)
                        SetSequence(fs.Position)
                    End Using
                    Await IncrementRecordCount()
                End If
            End If
        End Function

        Private Async Function IncrementRecordCount() As Task

            If (MyBase.File IsNot Nothing) Then
                Await MyBase.File.FetchAttributesAsync()
                Dim m_records As Long = 0
                If (MyBase.File.Metadata.ContainsKey(METADATA_RECORD_COUNT)) Then
                    If (Long.TryParse(MyBase.File.Metadata(METADATA_RECORD_COUNT), m_records)) Then
                        m_records += 1
                    Else
                        m_records = 0
                    End If
                End If
                MyBase.File.Metadata(METADATA_RECORD_COUNT) = m_records.ToString()
                Await MyBase.File.SetMetadataAsync()
            End If

        End Function

        ''' <summary>
        ''' Create a new stream writer to save events to an untyped Azure file
        ''' </summary>
        ''' <param name="identifier">
        ''' The unique identifier of the entity whose stream we will write to
        ''' </param>
        ''' <param name="connectionStringName">
        ''' The name of the connection string to use to write the 
        ''' </param>
        ''' <param name="settings">
        ''' Additional settings that control how a file is used as an event stream
        ''' </param>
        Public Sub New(ByVal identifier As IEventStreamUntypedIdentity,
                  Optional ByVal connectionStringName As String = "",
                  Optional ByVal settings As IFileStreamSettings = Nothing)

            MyBase.New(identifier, True, connectionStringName, settings)

        End Sub



    End Class

End Namespace

