Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage.File

Namespace Azure.File

    Public Class FileEventStreamProvider(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)
        Inherits FileEventStreamBase
        Implements IEventStreamProvider(Of TAggregate, TaggregateKey)

        Private ReadOnly m_domainName As String
        Private ReadOnly m_converter As IKeyConverter(Of TaggregateKey)
        Private ReadOnly m_directory As CloudFileDirectory

        Private m_aggregatename As String
        Protected ReadOnly Property AggregateClassName As String Implements IEventStreamProvider(Of TAggregate, TaggregateKey).AggregateClassName
            Get
                If (String.IsNullOrWhiteSpace(m_aggregatename)) Then
                    Dim typeName As String = GetType(TAggregate).Name
                    m_aggregatename = FileEventStreamBase.MakeValidStorageFolderName(typeName)
                End If
                Return m_aggregatename
            End Get
        End Property

        Public Function GetAllStreamKeys(Optional asOfDate As Date? = Nothing) As IEnumerable(Of TaggregateKey) Implements IEventStreamProvider(Of TAggregate, TaggregateKey).GetAllStreamKeys

            'list all the files in this folder...
            Dim ret As New List(Of TaggregateKey)
            If (m_directory IsNot Nothing) Then
                If (m_directory.Exists) Then
                    'List all the files in the folder
                    For Each streamFileEntry In m_directory.ListFilesAndDirectories()
                        Dim streamFile As CloudFile = TryCast(streamFileEntry, CloudFile)
                        'streamFile.Properties.
                        If (streamFile IsNot Nothing) Then
                            Dim ignore As Boolean = False
                            streamFile.FetchAttributes()
                            If (asOfDate.HasValue) Then
                                'compare to file create time
                                'if the creation date is after as-of-date then skip it
                                Dim creationDate As DateTime
                                If (streamFile.Metadata.ContainsKey(FileEventStreamBase.METADATA_DATE_CREATED)) Then
                                    If DateTime.TryParseExact(streamFile.Metadata(FileEventStreamBase.METADATA_DATE_CREATED), "O", Nothing, Globalization.DateTimeStyles.None, creationDate) Then
                                        If (creationDate.Year > 1900) Then
                                            If (creationDate < asOfDate.Value) Then
                                                ignore = True
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            If Not ignore Then
                                'add the key to the returned set
                                Dim keyString As String = streamFile.Metadata(FileEventStreamBase.METADATA_AGGREGATE_KEY)
                                If Not String.IsNullOrWhiteSpace(keyString) Then
                                    'Try and turn it to the TAggregateKey
                                    If (GetType(TaggregateKey) Is GetType(String)) Then
                                        ret.Add(CTypeDynamic(Of TaggregateKey)(keyString))
                                    Else
                                        'need to convert the key as we had to store it as a string
                                        ret.Add(m_converter.FromString(keyString))
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            Return ret

        End Function


        Private Sub New(ByVal AggregateDomainName As String,
                        Optional connectionStringName As String = "",
                Optional ByVal settings As IFileStreamSettings = Nothing)


            MyBase.New(AggregateDomainName, False, connectionStringName, settings)

            m_domainName = AggregateDomainName

            m_converter = KeyConverterFactory.CreateKeyConverter(Of TaggregateKey)

            If (BaseFileShare IsNot Nothing) Then
                'Get the directory for the aggregation type
                m_directory = BaseFileShare.GetRootDirectoryReference().GetDirectoryReference(FileEventStreamBase.MakeValidStorageFolderName(AggregateClassName))
            End If

        End Sub

        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier,
                              ByVal key As TaggregateKey,
                              Optional ByVal settings As IFileStreamSettings = Nothing) As IEventStreamProvider(Of TAggregate, TaggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New FileEventStreamProvider(Of TAggregate, TaggregateKey)(domainName, settings:=settings)

        End Function

        Public Shared Function Create(Optional domainName As String = "",
                      Optional ByVal settings As IFileStreamSettings = Nothing) As IEventStreamProvider(Of TAggregate, TaggregateKey)

            If (String.IsNullOrWhiteSpace(domainName)) Then
                domainName = DomainNameAttribute.GetDomainName(GetType(TAggregate))
            End If

            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New FileEventStreamProvider(Of TAggregate, TaggregateKey)(domainName, settings:=settings)

        End Function

    End Class

    Public Module FileEventStreamProviderFactory

        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TaggregateKey),
            ByVal key As TaggregateKey,
            Optional ByVal settings As IFileStreamSettings = Nothing) _
            As IEventStreamProvider(Of TAggregate, TaggregateKey)

            Return FileEventStreamProvider(Of TAggregate, TaggregateKey).Create(instance, key, settings)

        End Function

        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)(Optional domainName As String = "",
    Optional ByVal settings As IFileStreamSettings = Nothing) _
    As IEventStreamProvider(Of TAggregate, TaggregateKey)

            Return FileEventStreamProvider(Of TAggregate, TaggregateKey).Create(domainName, settings)

        End Function

    End Module

End Namespace