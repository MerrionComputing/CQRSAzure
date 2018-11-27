Imports CQRSAzure.EventSourcing.Azure.Table
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table
    Public NotInheritable Class TableEventStreamProvider(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)
        Inherits TableEventStreamBase
        Implements IEventStreamProvider(Of TAggregate, TaggregateKey)

        Private ReadOnly m_domainName As String
        Private ReadOnly m_settings As ITableSettings = Nothing
        Protected ReadOnly m_converter As IKeyConverter(Of TaggregateKey)

        Private m_aggregatename As String
        Protected ReadOnly Property AggregateClassName As String Implements IEventStreamProvider(Of TAggregate, TaggregateKey).AggregateClassName
            Get
                If (String.IsNullOrWhiteSpace(m_aggregatename)) Then
                    Dim typeName As String = GetType(TAggregate).Name
                    m_aggregatename = TableEventStreamBase.MakeValidStorageTableName(typeName)
                End If
                Return m_aggregatename
            End Get
        End Property


        Public Async Function GetAllStreamKeys(Optional asOfDate As Date? = Nothing) As Task(Of IEnumerable(Of TaggregateKey)) Implements IEventStreamProvider(Of TAggregate, TaggregateKey).GetAllStreamKeys

            Dim ret As New List(Of TaggregateKey)
            If (MyBase.AggregateKeyTable IsNot Nothing) Then
                If Await MyBase.AggregateKeyTable.ExistsAsync() Then
                    Dim qryKeys = New TableQuery(Of TableAggregateKeyRecord)().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, AggregateClassName))

                    Dim continueToken As New TableContinuationToken()
                    Dim records = Await MyBase.AggregateKeyTable.ExecuteQuerySegmentedAsync(Of TableAggregateKeyRecord)(qryKeys, continueToken)
                    For Each aggKey As TableAggregateKeyRecord In records
                        If Not asOfDate.HasValue OrElse (asOfDate.Value < aggKey.CreatedDateTime) Then
                            ret.Add(m_converter.FromString(aggKey.RowKey))
                        End If
                    Next
                End If
            End If

            Return ret

        End Function


        Private Sub New(ByVal AggregateDomainName As String,
                        Optional ByVal connectionStringName As String = "",
                        Optional ByVal settings As ITableSettings = Nothing)

            MyBase.New(AggregateDomainName, GetType(TAggregate).Name, False, connectionStringName, settings)

            m_converter = KeyConverterFactory.CreateKeyConverter(Of TaggregateKey)

            m_domainName = AggregateDomainName

            If (settings IsNot Nothing) Then
                m_settings = settings
                If (Not String.IsNullOrWhiteSpace(settings.DomainName)) Then
                    m_domainName = settings.DomainName
                End If
            End If



        End Sub

        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier,
                      ByVal key As TaggregateKey,
                      Optional ByVal settings As ITableSettings = Nothing) As IEventStreamProvider(Of TAggregate, TaggregateKey)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New TableEventStreamProvider(Of TAggregate, TaggregateKey)(domainName, settings:=settings)

        End Function

        Public Shared Function Create(Optional domainName As String = "",
                      Optional ByVal settings As ITableSettings = Nothing) As IEventStreamProvider(Of TAggregate, TaggregateKey)

            If (String.IsNullOrWhiteSpace(domainName)) Then
                domainName = DomainNameAttribute.GetDomainName(GetType(TAggregate))
            End If

            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New TableEventStreamProvider(Of TAggregate, TaggregateKey)(domainName, settings:=settings)

        End Function

    End Class

    Public Module TableEventStreamProviderFactory

        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TaggregateKey),
    ByVal key As TaggregateKey,
    Optional ByVal settings As ITableSettings = Nothing) _
    As IEventStreamProvider(Of TAggregate, TaggregateKey)

            Return TableEventStreamProvider(Of TAggregate, TaggregateKey).Create(instance, key, settings)

        End Function

        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)(Optional domainName As String = "",
    Optional ByVal settings As ITableSettings = Nothing) _
    As IEventStreamProvider(Of TAggregate, TaggregateKey)

            Return TableEventStreamProvider(Of TAggregate, TaggregateKey).Create(domainName, settings)

        End Function

    End Module
End Namespace