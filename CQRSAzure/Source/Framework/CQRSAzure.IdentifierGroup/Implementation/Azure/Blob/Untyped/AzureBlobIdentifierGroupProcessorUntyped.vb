Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Blob
Imports CQRSAzure.IdentifierGroup
Imports Microsoft.WindowsAzure.Storage
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Azure.Blob.Untyped


    Public NotInheritable Class AzureBlobIdentifierGroupProcessorUntyped
        Inherits AzureStorageEventStreamBase
        Implements IIdentifierGroupProcessorUntyped

#Region "private members"
        Private ReadOnly m_blobClient As CloudBlobClient
        Private ReadOnly m_blobBasePath As CloudBlobContainer


        Private ReadOnly m_folder As String
        Private ReadOnly m_connectionStringName As String
#End Region

        ''' <summary>
        ''' Get the set of unique identifiers of the all of the aggregates that are in the system as at a given point in time, or as at now if no
        ''' given point in time is specified
        ''' </summary>
        ''' <param name="effectiveDateTime">
        ''' If specified the date/time for which we want to get the members of the group
        ''' </param>
        Public Async Function GetAll(Optional effectiveDateTime As Date? = Nothing) As Task(Of IEnumerable(Of String)) Implements IIdentifierGroupProcessorUntyped.GetAll


            If (String.IsNullOrWhiteSpace(m_folder)) Then
                Return Enumerable.Empty(Of String)
            End If

            'list all the blobs in the folder dated before the effective time 
            Dim ret As New List(Of String)


            If m_blobBasePath IsNot Nothing Then
                Dim bd As CloudBlobDirectory = m_blobBasePath.GetDirectoryReference(m_folder)
                Dim continueToken As New BlobContinuationToken()
                Dim reqOpts As New Microsoft.WindowsAzure.Storage.Blob.BlobRequestOptions() With {.RetryPolicy = New RetryPolicies.ExponentialRetry()}
                Dim opContext As New OperationContext()
                Dim listResults = Await bd.ListBlobsSegmentedAsync(useFlatBlobListing:=False,
                                                                blobListingDetails:=BlobListingDetails.Metadata,
                                                                maxResults:=Nothing,
                                                                currentToken:=continueToken,
                                                                options:=reqOpts, operationContext:=opContext)

                For Each thisBlob In listResults.Results

                    Dim blobFile As CloudBlob = TryCast(thisBlob, CloudBlob)
                    Dim ignore As Boolean = False
                    If (blobFile IsNot Nothing) Then
                        'Get the metadata for this blob
                        If (effectiveDateTime.HasValue) Then
                            'if the creation date is after as-of-date then skip it
                            Dim creationDate As DateTime
                            If (blobFile.Metadata.ContainsKey(BlobEventStreamBase.METADATA_DATE_CREATED)) Then
                                If DateTime.TryParseExact(blobFile.Metadata(BlobEventStreamBase.METADATA_DATE_CREATED), "O", Nothing, Globalization.DateTimeStyles.None, creationDate) Then
                                    If (creationDate.Year > 1900) Then
                                        If (creationDate < effectiveDateTime.Value) Then
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
                                'Add tghis unqiue identifier
                                ret.Add(keyString)
                            End If
                        End If
                    End If
                Next
            End If


            Return ret

        End Function



        ''' <summary>
        ''' Get the set of unique identifiers of the aggregates that are in this group as at a given point in time, or as at now if no
        ''' given point in time is specified
        ''' </summary>
        ''' <param name="IdentifierGroup">
        ''' The specific identifier group for which we are getting the membership set
        ''' </param>
        ''' <param name="effectiveDateTime">
        ''' If specified the date/time for which we want to get the members of the group
        ''' </param>
        ''' <param name="parentGroupProcessor">
        ''' If this is a "nested" identifier group, this is the processor to get the the members 
        ''' </param>
        ''' <remarks>
        ''' If the rules of a classifier are invariant (which they should be) it is possible to cache group members as at a given point in 
        ''' time so as to speed up the resolving of group membership
        ''' </remarks>
        Public Async Function GetMembers(IdentifierGroup As IIdentifierGroupUntyped,
                                   Optional effectiveDateTime As Date? = Nothing,
                                   Optional parentGroupProcessor As IIdentifierGroupProcessorUntyped = Nothing) As Task(Of IEnumerable(Of String)) Implements IIdentifierGroupProcessorUntyped.GetMembers

            Throw New NotImplementedException()

        End Function


        Public Sub New(ByVal domainNameIn As String,
                        ByVal aggregateTypeNameIn As String,
                        Optional ByVal connectionStringName As String = "",
                        Optional ByVal settings As IBlobStreamSettings = Nothing)

            MyBase.New(domainNameIn, writeAccess:=False, connectionStringName:=connectionStringName, settings:=settings)

            m_folder = BlobEventStreamBase.GetEventStreamStorageFolderPath(aggregateTypeNameIn)

            If (m_storageAccount IsNot Nothing) Then
                m_blobClient = m_storageAccount.CreateCloudBlobClient()
                If (m_blobClient IsNot Nothing) Then
                    m_blobBasePath = m_blobClient.GetContainerReference(BlobEventStreamBase.MakeValidStorageFolderName(DomainName))
                End If
            End If

        End Sub

    End Class
End Namespace
