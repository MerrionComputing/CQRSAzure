Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing.Azure.File
Imports Microsoft.WindowsAzure.Storage.File

Namespace Azure.File


    Public NotInheritable Class FileProjectionSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                 TAggregateKey,
                                                                 TProjection As IProjection)
        Inherits FileProjectionSnapshotBase(Of TAggregate, TAggregateKey, TProjection)
        Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

        Public Async Function GetSnapshot(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As Task(Of IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetSnapshot

            Dim actualSequenceToGet As UInteger = GetLatestSnapshotSequence(key, OnOrBeforeSequence).Result
            If (actualSequenceToGet = 0) Then
                Return Nothing
            Else
                'get the filename starting with actualsequencetoget...
                If (MyBase.SnapshotsDirectory IsNot Nothing) Then
                    For Each f In Await MyBase.ListSnapshotFiles()
                        If (f.GetType() Is GetType(CloudFile)) Then
                            Dim filename As String = CType(f, CloudFile).Name
                            Dim thisSequence As UInteger = FileProjectionSnapshotBase.FilenameToSequenceNumber(filename)
                            If (thisSequence = actualSequenceToGet) Then
                                Dim fileToRead As CloudFile = MyBase.SnapshotsDirectory.GetFileReference(filename)
                                Using fs As System.IO.Stream = Await fileToRead.OpenReadAsync()
                                    Dim bf As New BinaryFormatter()
                                    Dim record As FileBlockWrappedProjectionSnapshot = CType(bf.Deserialize(fs), FileBlockWrappedProjectionSnapshot)
                                    Return record.Unwrap(Of TAggregate, TAggregateKey)
                                End Using
                            End If
                        End If
                    Next
                End If
                Return Nothing
            End If

        End Function

        Public Async Function GetLatestSnapshotSequence(key As TAggregateKey,
                                                  Optional OnOrBeforeSequence As UInteger = 0) As Task(Of UInteger) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetLatestSnapshotSequence



            Dim currentMax As UInteger = 0

            If (OnOrBeforeSequence = 0) Then
                OnOrBeforeSequence = UInteger.MaxValue
            End If

            'list all the files in the snapshots directory
            For Each f As IListFileItem In Await MyBase.ListSnapshotFiles()
                If (f.GetType() Is GetType(CloudFile)) Then
                    Dim filename As String = CType(f, CloudFile).Name
                    Dim thisSequence As UInteger = FileProjectionSnapshotBase.FilenameToSequenceNumber(filename)
                    If ((thisSequence > currentMax) AndAlso (thisSequence <= OnOrBeforeSequence)) Then
                        currentMax = thisSequence
                    End If
                End If
            Next

            Return currentMax


        End Function

        Private Sub New(ByVal AggregateDomainName As String,
        ByVal AggregateKey As TAggregateKey,
        Optional ByVal settings As IFileStreamSettings = Nothing)

            MyBase.New(AggregateDomainName,
                       AggregateKey,
                       writeAccess:=False,
                       connectionStringName:=GetReadConnectionStringName("", settings),
                       settings:=settings)

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="projection">
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As IFileStreamSettings = Nothing) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New FileProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)(domainName, instance.GetKey(), settings)

        End Function


#End Region

    End Class


    Public Module FileProjectionSnapshotReaderFactory

        ''' <summary>
        ''' Creates an azure file storage based event stream reader for the given aggregate
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
                                      Optional ByVal settings As IFileStreamSettings = Nothing) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return FileProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).Create(instance, projection, settings)

        End Function

    End Module

End Namespace