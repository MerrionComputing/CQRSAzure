Imports System
Imports System.Linq
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File

    Public NotInheritable Class LocalFileProjectionSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                 TAggregateKey,
                                                                 TProjection As IProjection)
        Inherits LocalFileProjectionSnapshotBase(Of TAggregate, TAggregateKey, TProjection)
        Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)


        Public Function GetLatestSnapshotSequence(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As Task(Of UInteger) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetLatestSnapshotSequence

            'if we didn't return anything then there is no latest snapshot
            Return Task.Factory.StartNew(Of UInteger)(
                Function()
                    Dim ret As UInteger = 0
                    If (MyBase.m_directory IsNot Nothing) Then
                        For Each fi As System.IO.FileInfo In m_directory.GetFiles(MyBase.MakeFilenameBase() & "*", IO.SearchOption.TopDirectoryOnly)
                            Dim val As UInteger = GetSequenceNumberFromFilename(fi.Name)
                            If (val > ret) Then
                                ret = val
                            End If
                        Next
                    End If

                    Return ret
                End Function
                )

        End Function



        Public Function GetSnapshot(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As Task(Of IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetSnapshot

            'Find the snapshot file
            Dim highestSnapshot As String = ""
            If (MyBase.m_directory IsNot Nothing) Then
                m_directory.Refresh()
                Dim filesToLoad As System.IO.FileInfo() = MyBase.m_directory.GetFiles(MyBase.MakeFilenameBase & "*", IO.SearchOption.TopDirectoryOnly)
                For Each f As System.IO.FileInfo In filesToLoad.OrderBy(Of String)(Function(name) name.Extension)
                    If Not String.IsNullOrWhiteSpace(f.Extension) Then
                        If (OnOrBeforeSequence > 0) Then
                            Dim snapshotSequence As UInteger
                            If (UInteger.TryParse(f.Extension.Substring(1), snapshotSequence)) Then
                                If (snapshotSequence <= OnOrBeforeSequence) Then
                                    highestSnapshot = f.Extension.Substring(1)
                                End If
                            End If
                        Else
                            highestSnapshot = f.Extension.Substring(1)
                        End If
                    End If
                Next
            End If

            If Not String.IsNullOrWhiteSpace(highestSnapshot) Then
                Dim fileToLoad As System.IO.FileInfo = MyBase.m_directory.GetFiles(MyBase.MakeFilenameBase & highestSnapshot).FirstOrDefault()
                fileToLoad.Refresh()
                If (fileToLoad.Exists) Then
                    'Read the content 
                    Using fr = fileToLoad.OpenRead()
                        'Deserialise into a wrapped projection snapshot
                        Dim wrappedSnapshot = LocalFileWrappedProjectionSnapshot.FromBinaryStream(fr)
                        If (wrappedSnapshot IsNot Nothing) Then
                            Return wrappedSnapshot.Unwrap(Of TAggregate, TAggregateKey)
                        End If
                    End Using
                End If
            End If

            ' If no appropriate snapshot was found explicitly return nothing
            Return Nothing

        End Function


        Protected Sub New(AggregateDomainName As String,
                       AggregateKey As TAggregateKey,
                       Optional settings As ILocalFileSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=False, settings:=settings)
        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an local file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="projection">
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            Return New LocalFileProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)(domainName, instance.GetKey(), settings)

        End Function


#End Region


    End Class

    Public Module LocalFileProjectionSnapshotReaderFactory

        ''' <summary>
        ''' Creates an azure file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="projection">
        ''' The projection instance for which snapshots are to be read
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                 TAggregateKey,
                                                                 TProjection As IProjection)(ByVal instance As TAggregate,
                                                                                             ByVal key As TAggregateKey,
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            Return LocalFileProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).Create(instance, projection, settings)

        End Function

    End Module
End Namespace