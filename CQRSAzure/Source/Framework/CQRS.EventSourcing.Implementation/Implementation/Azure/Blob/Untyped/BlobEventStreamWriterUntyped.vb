﻿Imports System
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob.Untyped

    ''' <summary>
    ''' An untyped writer to append events to the end of an event stream implemented as an AppendBlob
    ''' </summary>
    Public NotInheritable Class BlobEventStreamWriterUntyped
        Inherits BlobEventStreamBaseUntyped
        Implements IEventStreamWriterUntyped

#Region "Event stream information"

        Private m_recordCount As ULong
        Public ReadOnly Property RecordCount As ULong Implements IEventStreamWriterUntyped.RecordCount
            Get
                Return m_recordCount
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStreamWriterUntyped.LastAddition
            Get
                If (AppendBlob IsNot Nothing) Then
                    Return AppendBlob.Properties.LastModified.GetValueOrDefault.UtcDateTime
                Else
                    Return Nothing
                End If
            End Get
        End Property
#End Region


        Private m_context As IWriteContext
        Public Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriterUntyped.SetContext
            m_context = writerContext
        End Sub

        Public Async Function AppendEvent(EventInstance As IEvent,
                               Optional ExpectedTopSequence As Long = 0,
                               Optional ByVal Version As UInteger = 1) As Task Implements IEventStreamWriterUntyped.AppendEvent

            If (AppendBlob IsNot Nothing) Then
                Dim nextSequence As Long = Await GetSequence()

                Dim correlationIdentifier As String = ""
                If (m_context IsNot Nothing) Then
                    correlationIdentifier = m_context.CorrelationIdentifier
                End If

                Dim evtToWrite As New BlobBlockJsonWrappedEvent(nextSequence, Version, DateTime.UtcNow, EventInstance, correlationIdentifier)
                'Turn the event into a binary stream and append it to the blob
                Dim recordWritten As Boolean = False
                Try
                    Await AppendBlob.AppendBlockAsync(New IO.MemoryStream(Text.Encoding.UTF8.GetBytes(evtToWrite.ToJSonText)))

                    recordWritten = True
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamWriteException(DomainName, AggregateClassName, InstanceKey, nextSequence, "Unable to save a record to the event stream - " & evtToWrite.EventName, exBlob)
                End Try

                If (recordWritten) Then
                    Await IncrementRecordCountAndSequence()
                End If
            End If
        End Function

        ''' <summary>
        ''' Does an event stream already exist for this Domain/Type/Instance
        ''' </summary>
        ''' <remarks>
        ''' This can be used for e.g. checking it exists as part of a validation
        ''' </remarks>
        Public Function Exists() As Task(Of Boolean) Implements IEventStreamUntyped.Exists
            If AppendBlob IsNot Nothing Then
                Return AppendBlob.ExistsAsync()
            Else
                Return Task(Of Boolean).FromResult(Of Boolean)(False)
            End If
        End Function

        ''' <summary>
        ''' Update the sequence number metadata and return the new sequence number
        ''' </summary>
        Private Async Function IncrementRecordCountAndSequence() As Task(Of Long)

            If (MyBase.AppendBlob IsNot Nothing) Then
                Try
                    Await AppendBlob.FetchAttributesAsync()
                    If Not AppendBlob.Metadata.ContainsKey(METADATA_RECORD_COUNT) Then
                        AppendBlob.Metadata(METADATA_RECORD_COUNT) = "0"
                    End If
                    If Not AppendBlob.Metadata.ContainsKey(METADATA_SEQUENCE) Then
                        AppendBlob.Metadata(METADATA_SEQUENCE) = "0"
                    End If
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_RECORD_COUNT), m_recordCount)) Then
                        m_recordCount += 1
                        AppendBlob.Metadata(METADATA_RECORD_COUNT) = m_recordCount.ToString()
                    End If
                    Dim currentSequence As Long
                    If (Long.TryParse(AppendBlob.Metadata(METADATA_SEQUENCE), currentSequence)) Then
                        currentSequence += 1
                        AppendBlob.Metadata(METADATA_SEQUENCE) = currentSequence.ToString()
                    End If
                    Await AppendBlob.SetMetadataAsync()
                    Return m_recordCount
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamWriteException(DomainName, AggregateClassName, InstanceKey, 0,
                                                        "Unable to increment the record count for this event stream", exBlob)
                End Try
            Else
                Throw New EventStreamWriteException(DomainName, AggregateClassName, InstanceKey, 0,
                                                    "Unable to increment the record count for this event stream")
            End If

        End Function

        ''' <summary>
        ''' Clear down the event stream
        ''' </summary>
        ''' <remarks>
        ''' This will delete existing events so should not be done in any production environment therefore this is not
        ''' part of the IEventStreamWriter interface
        ''' </remarks>
        Public Async Function Reset() As Task

            If (AppendBlob IsNot Nothing) Then
                If AppendBlob.ExistsAsync.Result Then
                    Await AppendBlob.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots, Nothing,
                                             New BlobRequestOptions(),
                                             New Microsoft.WindowsAzure.Storage.OperationContext())
                End If
                'Recreate the blob file that was deleted
                Await MyBase.ResetBlob()
            End If
        End Function


        ''' <summary>
        ''' Create the underlying stream storage if it does not already exist
        ''' </summary>
        Public Async Function CreateIfNotExists() As Task Implements IEventStreamUntyped.CreateIfNotExists

            Dim alreadyExists As Boolean = Await Me.Exists()
            If (Not alreadyExists) Then
                Await MyBase.ResetBlob()
            End If

        End Function

        Public Sub New(ByVal identifier As IEventStreamUntypedIdentity,
               Optional ByVal connectionStringName As String = "",
               Optional ByVal settings As IBlobStreamSettings = Nothing)

            MyBase.New(identifier,
                       writeAccess:=True,
                       connectionStringName:=GetWriteConnectionStringName("", settings),
                       settings:=settings)


        End Sub

        ''' <summary>
        ''' Creates an azure blob storage based event stream writer for the given aggregate
        ''' </summary>
        ''' <param name="settings">
        ''' The settings to use to connect to the azure storage account to use
        ''' </param>
        Public Shared Function Create(ByVal identifier As IEventStreamUntypedIdentity,
                                      Optional ByVal connectionStringToUse As String = Nothing,
                                      Optional ByVal settings As IBlobStreamSettings = Nothing) As IEventStreamWriterUntyped

            If (String.IsNullOrWhiteSpace(connectionStringToUse)) Then
                If (settings IsNot Nothing) Then
                    connectionStringToUse = GetReadConnectionStringName(identifier.DomainName.Trim() & "ConnectionString", settings)
                End If
            End If

            Return New BlobEventStreamWriterUntyped(identifier,
                                                    connectionStringName:=connectionStringToUse,
                                                    settings:=settings)

        End Function


    End Class
End Namespace
