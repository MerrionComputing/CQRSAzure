Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob.Untyped
    Public NotInheritable Class BlobProjectionSnapshotWriterUntyped
        Inherits BlobProjectionSnapshotBaseUntyped
        Implements IProjectionSnapshotWriterUntyped

        Public Sub SaveSnapshot(key As String, snapshotToSave As IProjectionSnapshot) Implements IProjectionSnapshotWriterUntyped.SaveSnapshot

            If (snapshotToSave.Sequence > MyBase.GetHighestSequence()) Then
                If (MyBase.AppendBlob IsNot Nothing) Then

                    Dim snapshotToWrite As New BlobBlockWrappedProjectionSnapshot(snapshotToSave)

                    'Turn the event into a binary stream and append it to the blob
                    Dim recordWritten As Boolean = False
                    Try
                        Using es As System.IO.Stream = snapshotToWrite.ToBinaryStream()

                            Dim offset As Long = AppendBlob.AppendBlock(es)
                        End Using
                        recordWritten = True
                    Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                        Throw New EventStreamWriteException(DomainName, AggregateClassName, key, snapshotToSave.Sequence, "Unable to save a record to the projection snapshots - " & ProjectionClassName, exBlob)
                    End Try

                    If (recordWritten) Then
                        SetBlobSequence(snapshotToSave.Sequence)
                    End If
                End If
            Else
                'Attempt to save an out-of sequence snapshot
                Throw New OutOfSequenceSnapshotException(DomainName, AggregateClassName, key.ToString(), snapshotToSave.Sequence, MyBase.GetHighestSequence())
            End If
        End Sub

        Private Sub SetBlobSequence(sequence As UInteger)

            If (MyBase.AppendBlob IsNot Nothing) Then
                MyBase.AppendBlob.Metadata(METADATA_SEQUENCE) = sequence.ToString() 'Sequence started at zero
                MyBase.AppendBlob.SetMetadata()
            End If

        End Sub

        ''' <summary>
        ''' Clear down the snapshot collection
        ''' </summary>
        ''' <remarks>
        ''' This will delete existing snapshots so should not be done in any production environment therefore this is not
        ''' part of the IProjectionSnapshotWriter interface
        ''' </remarks>
        Public Sub Reset()

            If (AppendBlob IsNot Nothing) Then
                AppendBlob.Delete(DeleteSnapshotsOption.IncludeSnapshots)
                'Recreate the blob file that was deleted
                MyBase.ResetBlob()
            End If
        End Sub

        Protected Sub New(ByVal identifier As IEventStreamUntypedIdentity,
          ByVal projectionClassName As String,
          Optional ByVal connectionStringName As String = "",
          Optional ByVal settings As IBlobStreamSettings = Nothing)

            MyBase.New(identifier,
                       projectionClassName,
                       writeAccess:=True,
                       connectionStringName:=GetWriteConnectionStringName("", settings),
                       settings:=settings)

        End Sub

    End Class
End Namespace
