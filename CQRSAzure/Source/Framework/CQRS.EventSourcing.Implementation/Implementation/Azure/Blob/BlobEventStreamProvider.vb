Imports System
Imports System.Collections.Generic
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob
    Public NotInheritable Class BlobEventStreamProvider(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)
        Inherits BlobEventStreamBase
        Implements IEventStreamProvider(Of TAggregate, TaggregateKey)

        Private ReadOnly m_domainName As String
        Private ReadOnly m_converter As IKeyConverter(Of TaggregateKey)

        Private m_aggregatename As String
        Protected ReadOnly Property AggregateClassName As String Implements IEventStreamProvider(Of TAggregate, TaggregateKey).AggregateClassName
            Get
                If (String.IsNullOrWhiteSpace(m_aggregatename)) Then
                    Dim typeName As String = GetType(TAggregate).Name
                    m_aggregatename = BlobEventStreamBase.MakeValidStorageFolderName(typeName)
                End If
                Return m_aggregatename
            End Get
        End Property

        ''' <summary>
        ''' The storage location that the event streams for this event stream are located
        ''' </summary>
        ''' <returns>
        ''' /eventstreams/[aggregate-class-name]/
        ''' </returns>
        ''' <remarks>
        ''' This is a virtual path that is added to the blob name to create a traversable path
        ''' </remarks>
        Protected ReadOnly Property ContainerBasePath As String
            Get
                Return EVENTSTREAM_FOLDER & "/" & MakeValidStorageFolderName(AggregateClassName) & "/"
            End Get
        End Property

        Public Async Function GetAllStreamKeys(Optional asOfDate As Date? = Nothing) As Task(Of IEnumerable(Of TaggregateKey)) Implements IEventStreamProvider(Of TAggregate, TaggregateKey).GetAllStreamKeys

            Dim ret As New List(Of TaggregateKey)

            If MyBase.BlobContainer IsNot Nothing Then
                Dim bd As CloudBlobDirectory = MyBase.BlobContainer.GetDirectoryReference(ContainerBasePath)
                Dim continueToken As New BlobContinuationToken()
                Dim allBlobs = Await bd.ListBlobsSegmentedAsync(False,
                                                                BlobListingDetails.Metadata,
                                                                Nothing,
                                                                continueToken,
                                                                Nothing,
                                                                Nothing)

                For Each thisBlob In allBlobs.Results

                    Dim blobFile As CloudBlob = TryCast(thisBlob, CloudBlob)
                    Dim ignore As Boolean = False
                    If (blobFile IsNot Nothing) Then
                        'Get the metadata for this blob
                        If (asOfDate.HasValue) Then
                            'if the creation date is after as-of-date then skip it
                            Dim creationDate As DateTime
                            If (blobFile.Metadata.ContainsKey(BlobEventStreamBase.METADATA_DATE_CREATED)) Then
                                If DateTime.TryParseExact(blobFile.Metadata(BlobEventStreamBase.METADATA_DATE_CREATED), "O", Nothing, Globalization.DateTimeStyles.None, creationDate) Then
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
                            Dim keyString As String = blobFile.Metadata(BlobEventStreamBase.METADATA_AGGREGATE_KEY)
                            If Not String.IsNullOrWhiteSpace(keyString) Then
                                'Try and turn it to the TAggregateKey
                                'need to convert the key as we had to store it as a string
                                ret.Add(m_converter.FromString(keyString))
                            End If
                        End If
                    End If
                Next
            End If

            Return ret

        End Function

        Private Sub New(ByVal AggregateDomainName As String,
                        Optional ByVal connectionStringName As String = "",
                        Optional ByVal settings As IBlobStreamSettings = Nothing)

            MyBase.New(AggregateDomainName, False, connectionStringName,
                       settings)

            m_domainName = AggregateDomainName

            m_converter = KeyConverterFactory.CreateKeyConverter(Of TaggregateKey)

        End Sub

        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier,
                      ByVal key As TaggregateKey,
                      Optional ByVal settings As IBlobStreamSettings = Nothing) As IEventStreamProvider(Of TAggregate, TaggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return Create(domainName,
                          settings:=settings)

        End Function

        Public Shared Function Create(Optional domainName As String = "",
                      Optional ByVal settings As IBlobStreamSettings = Nothing) As IEventStreamProvider(Of TAggregate, TaggregateKey)

            'If there are settings passed in their domain name attribute takes preference
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If



            Return New BlobEventStreamProvider(Of TAggregate, TaggregateKey)(domainName,
                                                                             settings:=settings)

        End Function



    End Class


    Public Module BlobEventStreamProviderFactory

        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TaggregateKey),
    ByVal key As TaggregateKey,
    Optional ByVal settings As IBlobStreamSettings = Nothing) _
    As IEventStreamProvider(Of TAggregate, TaggregateKey)

            Return BlobEventStreamProvider(Of TAggregate, TaggregateKey).Create(instance, key, settings)

        End Function

        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)(Optional domainName As String = "",
                                                                                                               Optional ByVal settings As IBlobStreamSettings = Nothing) _
                                                                                                               As IEventStreamProvider(Of TAggregate, TaggregateKey)

            Return BlobEventStreamProvider(Of TAggregate, TaggregateKey).Create(domainName,
                                                                                settings)

        End Function

    End Module
End Namespace