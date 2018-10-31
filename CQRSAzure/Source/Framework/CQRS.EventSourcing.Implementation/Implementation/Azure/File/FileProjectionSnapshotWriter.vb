Imports Microsoft.WindowsAzure.Storage.File
Imports CQRSAzure.EventSourcing

Namespace Azure.File


    Public NotInheritable Class FileProjectionSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                 TAggregateKey,
                                                                 TProjection As IProjection)
        Inherits FileProjectionSnapshotBase(Of TAggregate, TAggregateKey, TProjection)
        Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

        Public Sub SaveSnapshot(key As TAggregateKey, snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).SaveSnapshot

            If (MyBase.SnapshotsDirectory IsNot Nothing) Then
                ' Get a filename with which to save the snapshot
                Dim filename As String = MyBase.MakeSnapshotFilename(snapshotToSave.Sequence, snapshotToSave.AsOfDate)
                Dim wrappedSnapshot As New FileBlockWrappedProjectionSnapshot(snapshotToSave)
                If (wrappedSnapshot IsNot Nothing) Then
                    Dim newSnapshotFile As CloudFile = MyBase.SnapshotsDirectory.GetFileReference(filename)
                    If (newSnapshotFile IsNot Nothing) Then
                        Dim streamToWrite As System.IO.Stream = wrappedSnapshot.ToBinaryStream
                        newSnapshotFile.Create(streamToWrite.Length)
                        Using fs As CloudFileStream = newSnapshotFile.OpenWrite(Nothing)
                            'write the event to the stream here..
                            wrappedSnapshot.WriteToBinaryStream(fs)
                        End Using
                    End If
                Else
                    Throw New EventStreamWriteException(DomainName, AggregateClassName, key.ToString(), snapshotToSave.Sequence, "Unable to wrap snapshot for saving to an Azure file")
                End If
            End If

        End Sub


        ''' <summary>
        ''' Clear down all of the saved snapshots
        ''' </summary>
        ''' <remarks>
        ''' This will delete existing snapshot records so should not be done in any production environment therefore this is not
        ''' part of the IProjectionSnapshotWriter interface
        ''' </remarks>
        Public Sub Reset()

            If (MyBase.SnapshotsDirectory IsNot Nothing) Then
                If (MyBase.SnapshotsDirectory.Exists) Then
                    For Each f As IListFileItem In MyBase.ListSnapshotFiles()
                        If (f Is GetType(CloudFile)) Then
                            CType(f, CloudFile).Delete()
                        End If
                    Next
                Else
                    MyBase.SnapshotsDirectory.Create()
                End If
            End If

        End Sub

        Private Sub New(ByVal AggregateDomainName As String,
                ByVal AggregateKey As TAggregateKey,
                Optional ByVal settings As IFileStreamSettings = Nothing)

            MyBase.New(AggregateDomainName,
                       AggregateKey,
                       writeAccess:=True,
                       connectionStringName:=GetWriteConnectionStringName("", settings),
                       settings:=settings)

        End Sub



#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure file storage based event stream writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="projection">
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As IFileStreamSettings = Nothing) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New FileProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)(domainName, instance.GetKey(), settings)

        End Function


#End Region

    End Class

    Public Module FileProjectionSnapshotWriterFactory

        ''' <summary>
        ''' Creates an azure file storage based projection snapshot writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="projection">
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                 TAggregateKey,
                                                                 TProjection As IProjection)(ByVal instance As TAggregate,
                                                                                             ByVal key As TAggregateKey,
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As IFileStreamSettings = Nothing) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return FileProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).Create(instance, projection, settings)

        End Function

    End Module

End Namespace