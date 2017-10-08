Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob
    Public NotInheritable Class BlobProjectionSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                 TAggregateKey,
                                                                 TProjection As IProjection)
        Inherits BlobProjectionSnapshotBase(Of TAggregate, TAggregateKey, TProjection)
        Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

        Public Sub SaveSnapshot(key As TAggregateKey, snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).SaveSnapshot

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
                        Throw New EventStreamWriteException(DomainName, AggregateClassName, key.ToString(), snapshotToSave.Sequence, "Unable to save a record to the projection snapshots - " & ProjectionClassName, exBlob)
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

        Private Sub New(ByVal AggregateDomainName As String,
                ByVal AggregateKey As TAggregateKey,
                Optional ByVal settings As IBlobStreamSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=True, connectionStringName:=GetWriteConnectionStringName("", settings), settings:=settings)

        End Sub



#Region "Factory methods"

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IBlobStreamSettings = Nothing) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If

            Return New BlobProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)(domainName, instance.GetKey(), settings)

        End Function

#End Region

    End Class

    Public NotInheritable Class BlobProjectionSnapshotWriter

#Region "Factory methods"

        ''' <summary>
        ''' Creates an in-memory event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                          TAggregateKey,
                                          TProjection As IProjection)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IBlobStreamSettings = Nothing) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)



            Return BlobProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).Create(instance, settings)

        End Function

#End Region

    End Class

End Namespace