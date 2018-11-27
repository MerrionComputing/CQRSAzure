Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob.Untyped


    ''' <summary>
    ''' Class to read untyped projection snapshot records that have been stored in an Azure blob record
    ''' </summary>
    Public NotInheritable Class BlobProjectionSnapshotReaderUntyped
        Inherits BlobProjectionSnapshotBaseUntyped
        Implements IProjectionSnapshotReaderUntyped

        Public Function GetSnapshot(key As String, Optional OnOrBeforeSequence As UInteger = 0) As IProjectionSnapshot Implements IProjectionSnapshotReaderUntyped.GetSnapshot

            If (MyBase.AppendBlob IsNot Nothing) Then
                Dim lastMatch As IProjectionSnapshot = Nothing
                Dim bf As New BinaryFormatter()
                Using rawStream As System.IO.Stream = GetUnderlyingStream()
                    While Not (rawStream.Position >= rawStream.Length)
                        Dim record As BlobBlockWrappedProjectionSnapshot = CType(bf.Deserialize(rawStream), BlobBlockWrappedProjectionSnapshot)
                        If (record IsNot Nothing) Then
                            If ((OnOrBeforeSequence = 0) OrElse (record.Sequence <= OnOrBeforeSequence)) Then
                                lastMatch = record.UnwrapUntyped()
                            Else
                                If (lastMatch IsNot Nothing) Then
                                    Return lastMatch
                                Else
                                    Return Nothing
                                End If
                            End If
                        End If
                    End While
                    If (lastMatch IsNot Nothing) Then
                        Return lastMatch
                    End If
                End Using
            End If

            'No snapshots stored for this projection
            Return Nothing

        End Function

        ''' <summary>
        ''' Get a snapshot of the append blob to use when reading this event stream
        ''' </summary>
        ''' <returns></returns>
        Private Function GetAppendBlobSnapshot() As CloudAppendBlob
            If (AppendBlob IsNot Nothing) Then
                Return AppendBlob.CreateSnapshotAsync().Result
            Else
                Return Nothing
            End If
        End Function

        Private Function GetUnderlyingStream() As System.IO.Stream

            If (AppendBlob IsNot Nothing) Then
                Dim targetStream As New System.IO.MemoryStream()
                Try
                    GetAppendBlobSnapshot().DownloadToStreamAsync(targetStream)
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamReadException(DomainName, AggregateClassName, InstanceKey, 0, "Unable to access underlying event stream", exBlob)
                End Try
                targetStream.Seek(0, IO.SeekOrigin.Begin)
                Return targetStream
            Else
                Return Nothing
            End If

        End Function

        Public Function GetLatestSnapshotSequence(key As String,
                                                  Optional OnOrBeforeSequence As UInteger = 0) As UInteger Implements IProjectionSnapshotReaderUntyped.GetLatestSnapshotSequence

            If (MyBase.AppendBlob IsNot Nothing) Then
                Return MyBase.GetHighestSequence()
            Else
                Return 0
            End If

        End Function

        Protected Sub New(ByVal identifier As IEventStreamUntypedIdentity,
                          ByVal projectionClassName As String,
                          Optional ByVal connectionStringName As String = "",
                          Optional ByVal settings As IBlobStreamSettings = Nothing)

            MyBase.New(identifier,
                       projectionClassName,
                       writeAccess:=False,
                       connectionStringName:=GetWriteConnectionStringName("", settings),
                       settings:=settings)

        End Sub

    End Class

End Namespace