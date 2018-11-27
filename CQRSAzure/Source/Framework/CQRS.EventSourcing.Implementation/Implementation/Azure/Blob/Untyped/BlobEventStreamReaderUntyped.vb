Imports Microsoft.WindowsAzure.Storage.Blob
Imports CQRSAzure.EventSourcing
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports Microsoft.WindowsAzure.Storage

Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports System

Namespace Azure.Blob.Untyped

    ''' <summary>
    ''' Class to read events from an untyped event stream implemented as a windows azure append blob
    ''' </summary>
    Public NotInheritable Class BlobEventStreamReaderUntyped
        Inherits BlobEventStreamBaseUntyped
        Implements IEventStreamReaderUntyped

        Public ReadOnly Property Key As String Implements IEventStreamReaderUntyped.Key
            Get
                Return MyBase.InstanceKey
            End Get
        End Property

        ''' <summary>
        ''' Get a snapshot of the append blob to use when reading this event stream
        ''' </summary>
        ''' <returns></returns>
        Private Async Function GetAppendBlobSnapshot() As Task(Of CloudAppendBlob)
            If (AppendBlob IsNot Nothing) Then
                Return Await AppendBlob.CreateSnapshotAsync()
            Else
                Return Nothing
            End If
        End Function

        Private Async Function GetUnderlyingStream() As Task(Of System.IO.Stream)

            If (AppendBlob IsNot Nothing) Then
                Dim targetStream As New System.IO.MemoryStream()
                Try
                    Await GetAppendBlobSnapshot().Result.DownloadToStreamAsync(targetStream)
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamReadException(DomainName, AggregateClassName, Me.Key, 0, "Unable to access underlying event stream", exBlob)
                End Try
                targetStream.Seek(0, IO.SeekOrigin.Begin)
                Return targetStream
            Else
                Return Nothing
            End If

        End Function

        Public Async Function GetEvents() As Task(Of IEnumerable(Of IEvent)) Implements IEventStreamReaderUntyped.GetEvents
            Return Await GetEvents(StartingSequenceNumber:=0, effectiveDateTime:=Nothing)
        End Function

        Public Async Function GetEvents(Optional StartingSequenceNumber As UInteger = 0,
                                        Optional effectiveDateTime As Date? = Nothing) As Task(Of IEnumerable(Of IEvent)) Implements IEventStreamReaderUntyped.GetEvents

            If (AppendBlob IsNot Nothing) Then
                Dim ret As New List(Of IEvent)

                Using rawStream As System.IO.Stream = Await GetUnderlyingStream()
                    If Not (rawStream.Position >= rawStream.Length) Then
                        For Each record As BlobBlockJsonWrappedEvent In BlobBlockJsonWrappedEvent.FromBinaryStream(rawStream)
                            If (record IsNot Nothing) Then
                                If (record.Sequence >= StartingSequenceNumber) Then
                                    ret.Add(record.EventInstance)
                                End If
                            End If
                        Next
                    End If
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName,
                                                   AggregateClassName,
                                                   MyBase.InstanceKey, 0,
                                                   "Unable to read events - Azure blob not initialised")
            End If

        End Function

        Public Async Function GetEventsWithContext(Optional StartingSequenceNumber As UInteger = 0,
                                                   Optional effectiveDateTime As Date? = Nothing) As Task(Of IEnumerable(Of IEventContext)) Implements IEventStreamReaderUntyped.GetEventsWithContext

            If (AppendBlob IsNot Nothing) Then
                Dim ret As New List(Of IEventContext)

                Using rawStream As System.IO.Stream = Await GetUnderlyingStream()
                    If Not (rawStream.Position >= rawStream.Length) Then
                        For Each record As BlobBlockJsonWrappedEvent In BlobBlockJsonWrappedEvent.FromBinaryStream(rawStream)
                            If (record IsNot Nothing) Then
                                If (record.Sequence >= StartingSequenceNumber) Then
                                    Dim instance = InstanceWrappedEvent(Of String).Wrap(Me.Key, record.EventInstance, record.Version)
                                    ret.Add(ContextWrappedEventUntyped.Wrap(instance, record.Sequence, "", "", record.Timestamp, "", ""))
                                End If
                            End If
                        Next
                    End If
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName,
                                                   AggregateClassName,
                                                   Me.Key,
                                                   0,
                                                   "Unable to read events - Azure blob not initialised")
            End If
        End Function

        Public Sub New(ByVal identifier As IEventStreamUntypedIdentity,
               Optional ByVal connectionStringName As String = "",
               Optional ByVal settings As IBlobStreamSettings = Nothing)

            MyBase.New(identifier,
                       writeAccess:=False,
                       connectionStringName:=GetWriteConnectionStringName("", settings),
                       settings:=settings)

        End Sub


        ''' <summary>
        ''' Creates an azure blob storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="settings">
        ''' The settings to use to connect to the azure storage account to use
        ''' </param>
        Public Shared Function Create(ByVal identifier As IEventStreamUntypedIdentity,
                                      Optional ByVal settings As IBlobStreamSettings = Nothing) As IEventStreamReaderUntyped

            Dim connectionStringToUse As String = String.Empty
            If (settings IsNot Nothing) Then
                connectionStringToUse = GetReadConnectionStringName(identifier.DomainName.Trim() & "ConnectionString", settings)
            End If


            Return New BlobEventStreamReaderUntyped(identifier,
                                                    connectionStringName:=connectionStringToUse)

        End Function

        ''' <summary>
        ''' Create a projection processor that works off an azure blob backed event stream
        ''' </summary>
        ''' <returns>
        ''' A projection processor that can run projections over this event stream
        ''' </returns>
        Public Shared Function CreateProjectionProcessor(ByVal identifier As IEventStreamUntypedIdentity,
                                                         Optional ByVal settings As IBlobStreamSettings = Nothing,
                                                         Optional ByVal snapshotProcessor As ISnapshotProcessorUntyped = Nothing) As ProjectionProcessorUntyped

            Return New ProjectionProcessorUntyped(Create(identifier, settings), snapshotProcessor)

        End Function

    End Class

End Namespace
