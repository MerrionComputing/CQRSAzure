Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob


    ''' <summary>
    ''' Class to read projection snapshot records that have been stored in an Azure blob record
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the base class to which the event stream is attached
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which an instance of that aggregation base class is uniquely identified
    ''' </typeparam>
    Public NotInheritable Class BlobProjectionSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                 TAggregateKey,
                                                                 TProjection As IProjection)
        Inherits BlobProjectionSnapshotBase(Of TAggregate, TAggregateKey, TProjection)
        Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

        Public Function GetSnapshot(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As IProjectionSnapshot(Of TAggregate, TAggregateKey) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetSnapshot

            If (MyBase.AppendBlob IsNot Nothing) Then
                Dim lastMatch As IProjectionSnapshot(Of TAggregate, TAggregateKey) = Nothing
                Dim bf As New BinaryFormatter()
                Using rawStream As System.IO.Stream = GetUnderlyingStream()
                    While Not (rawStream.Position >= rawStream.Length)
                        Dim record As BlobBlockWrappedProjectionSnapshot = CTypeDynamic(Of BlobBlockWrappedProjectionSnapshot)(bf.Deserialize(rawStream))
                        If (record IsNot Nothing) Then
                            If ((OnOrBeforeSequence = 0) OrElse (record.Sequence <= OnOrBeforeSequence)) Then
                                lastMatch = record.Unwrap(Of TAggregate, TAggregateKey)
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

        Public Function GetLatestSnapshotSequence(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As UInteger Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetLatestSnapshotSequence

            If (MyBase.AppendBlob IsNot Nothing) Then
                Return MyBase.GetHighestSequence()
            Else
                Return 0
            End If

        End Function


        ''' <summary>
        ''' Get a snapshot of the append blob to use when reading this event stream
        ''' </summary>
        ''' <returns></returns>
        Private Function GetAppendBlobSnapshot() As CloudAppendBlob
            If (AppendBlob IsNot Nothing) Then
                Return AppendBlob.CreateSnapshot()
            Else
                Return Nothing
            End If
        End Function

        Private Function GetUnderlyingStream() As System.IO.Stream

            If (AppendBlob IsNot Nothing) Then
                Dim targetStream As New System.IO.MemoryStream()
                Try
                    GetAppendBlobSnapshot().DownloadToStream(targetStream)
                Catch exBlob As Microsoft.WindowsAzure.Storage.StorageException
                    Throw New EventStreamReadException(DomainName, AggregateClassName, m_key.ToString(), 0, "Unable to access underlying event stream", exBlob)
                End Try
                targetStream.Seek(0, IO.SeekOrigin.Begin)
                Return targetStream
            Else
                Return Nothing
            End If

        End Function


        Private Sub New(ByVal AggregateDomainName As String,
                ByVal AggregateKey As TAggregateKey,
                Optional ByVal settings As IBlobStreamSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=False, connectionStringName:=GetReadConnectionStringName("", settings), settings:=settings)

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates a projection reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IBlobStreamSettings = Nothing) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If

            Return New BlobProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)(domainName, instance.GetKey(), settings)

        End Function

#End Region

    End Class


    Public NotInheritable Class BlobProjectionSnapshotReader

#Region "Factory methods"

        ''' <summary>
        ''' Creates a projection reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey, TProjection As IProjection)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As IBlobStreamSettings = Nothing) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If

            Return BlobProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).Create(instance, settings)

        End Function

#End Region

    End Class
End Namespace