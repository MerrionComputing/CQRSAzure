Imports System
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File
    ''' <summary>
    ''' Base class for storing projection state in a local file
    ''' </summary>
    Public NotInheritable Class LocalFileProjectionSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                 TAggregateKey,
                                                                 TProjection As IProjection)
        Inherits LocalFileProjectionSnapshotBase(Of TAggregate, TAggregateKey, TProjection)
        Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

        Public Sub SaveSnapshot(key As TAggregateKey, snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).SaveSnapshot

            If (MyBase.m_directory IsNot Nothing) Then
                m_directory.Refresh()
                If Not m_directory.Exists Then
                    m_directory.Create()
                End If
                Dim filename As String = MyBase.MakeFilename(snapshotToSave.Sequence)
                If Not String.IsNullOrWhiteSpace(filename) Then
                    'wrap it up and save it...
                    Dim wrappedSnapshot As New LocalFileWrappedProjectionSnapshot(snapshotToSave)
                    If (wrappedSnapshot IsNot Nothing) Then
                        Dim targetFile As New System.IO.FileInfo(System.IO.Path.Combine(m_directory.FullName, filename))
                        Using fr = targetFile.OpenWrite()
                            wrappedSnapshot.WriteToBinaryStream(fr)
                        End Using
                    End If
                End If
            End If


        End Sub

        Public Sub Reset()
            ' Delete all snapshot files pertaining to this key
            If (MyBase.m_directory IsNot Nothing) Then
                If MyBase.m_directory.Exists Then
                    Dim filesToDelete As System.IO.FileInfo() = MyBase.m_directory.GetFiles(MyBase.MakeFilenameBase & "*", IO.SearchOption.TopDirectoryOnly)
                    For Each fi As System.IO.FileInfo In filesToDelete
                        fi.Delete()
                    Next
                End If
            End If

        End Sub

        Protected Sub New(AggregateDomainName As String,
               AggregateKey As TAggregateKey,
               Optional settings As ILocalFileSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=True, settings:=settings)
        End Sub

#Region "Factory methods"
        ''' <summary>
        ''' Creates an local file storage based event stream writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="projection">
        ''' The projection instance of which snapshots are taken
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            Return New LocalFileProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)(domainName, instance.GetKey(), settings)

        End Function
#End Region

    End Class

    Public Module LocalFileProjectionSnapshotWriterFactory

        ''' <summary>
        ''' Creates a local file storage based projection snapshot writer for the given aggregate
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
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            Return LocalFileProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).Create(instance, projection, settings)

        End Function

    End Module
End Namespace