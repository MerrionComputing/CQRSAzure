Imports System.Reflection
Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table
    Public MustInherit Class TableEventStreamBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits TableEventStreamBase

#Region "Field names"
        Public Const FIELDNAME_EVENTTYPE As String = "EventType"
        Public Const FIELDNAME_VERSION As String = "Version"
        Public Const FIELDNAME_COMMENTS As String = "Commentary"
        Public Const FIELDNAME_WHO As String = "Who"
        Public Const FIELDNAME_SOURCE As String = "Source"
#End Region



        Protected Friend ReadOnly m_settings As ITableSettings
        Protected ReadOnly m_converter As IKeyConverter(Of TAggregateKey)

        Protected ReadOnly m_key As TAggregateKey
        Protected ReadOnly Property AggregateKey As String
            Get
                If m_key IsNot Nothing Then
                    Return m_converter.ToUniqueString(m_key)
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        Private ReadOnly m_aggregateClassName As String
        Protected ReadOnly Property AggregateClassName As String
            Get
                Return m_aggregateClassName
            End Get
        End Property

        Private ReadOnly m_tableName As String
        ''' <summary>
        ''' The name of the table in which the event stream for this aggregate type is stored
        ''' </summary>
        Public ReadOnly Property TableName As String
            Get
                Return m_tableName
            End Get
        End Property

        Private ReadOnly m_table As CloudTable

        ''' <summary>
        ''' The table reference to use when accessing the underlying event stream
        ''' </summary>
        Protected ReadOnly Property Table As CloudTable
            Get
                Return m_table
            End Get
        End Property

        Private ReadOnly m_aggregatekeyTable As CloudTable
        ''' <summary>
        ''' The table reference to use when accessing the aggregate key details
        ''' </summary>
        ''' <remarks>
        ''' This can also be used to give a secondary key lookup if an aggregate has different candidates for unqiue
        ''' identifier (e.g. CUSIP/ISIN/SEDOL etc for stocks, Chassis No/Veh Reg for vehicle etc..)
        ''' </remarks>
        Protected ReadOnly Property AggregateKeyTable As CloudTable
            Get
                Return m_aggregatekeyTable
            End Get
        End Property

        Protected ReadOnly Property RequestOptions As TableRequestOptions
            Get
                Return New TableRequestOptions() With {.RetryPolicy = New Microsoft.WindowsAzure.Storage.RetryPolicies.ExponentialRetry()}
            End Get
        End Property

        ''' <summary>
        ''' Turn an event and its data load into a row to save in an Azure table
        ''' </summary>
        ''' <param name="eventToSave">
        ''' The instance of the event and related context data to save to the table
        ''' </param>
        ''' <returns>
        ''' A dynamic table entity that represnets the event in a format that can be saved into the table
        ''' </returns>
        ''' <remarks>
        ''' Currently Azure tables has a hard limit of 252 properties so we must throw an error if the event has more 
        ''' properties than that (although this may change in future)
        ''' </remarks>
        Public Function MakeDynamicTableEntity(ByVal eventToSave As IEventContext(Of TAggregateKey)) As DynamicTableEntity

            Dim ret As New DynamicTableEntity

            ret.PartitionKey = eventToSave.AggregateKey.ToString()
            ret.RowKey = SequenceNumberToRowkey(eventToSave.SequenceNumber)

            'Add the event type - currently this is the event class name
            ret.Properties.Add(FIELDNAME_EVENTTYPE,
                  New EntityProperty(eventToSave.EventInstance.GetType().AssemblyQualifiedName))

            ret.Properties.Add(FIELDNAME_VERSION,
                  New EntityProperty(eventToSave.Version))


            If (Not String.IsNullOrWhiteSpace(eventToSave.Commentary)) Then
                ret.Properties.Add(FIELDNAME_COMMENTS,
                     New EntityProperty(eventToSave.Commentary))
            End If



            If (Not String.IsNullOrWhiteSpace(eventToSave.Who)) Then
                ret.Properties.Add(FIELDNAME_WHO, New EntityProperty(eventToSave.Who))
            End If


            If (Not String.IsNullOrWhiteSpace(eventToSave.Source)) Then
                ret.Properties.Add(FIELDNAME_SOURCE, New EntityProperty(eventToSave.Source))
            End If

            ret.Timestamp = eventToSave.Timestamp

            'Now add in the different properties of the payload
            Dim propertiesCount As Integer = 0
            For Each pi As System.Reflection.PropertyInfo In eventToSave.EventInstance.GetType().GetProperties()
                If (pi.CanRead) Then
                    If Not IsContextProperty(pi.Name) Then
                        If (propertiesCount > MAX_FREE_DATA_FIELDS) Then
                            'Throw an error to indicate that our event exceeds the storage capabilities of an azure table
                            Throw New EventStreamWriteException(DomainName, AggregateClassName, ret.PartitionKey, eventToSave.Version, "Too many properties for event - cannot store in Azure table")
                        Else
                            If Not IsPropertyEmpty(pi, eventToSave.EventInstance) Then
                                ret.Properties.Add(pi.Name, MakeEntityProperty(pi, eventToSave.EventInstance))
                            End If
                        End If
                        propertiesCount = propertiesCount + 1
                    End If
                End If
            Next pi

            Return ret
        End Function

        Private Function IsPropertyEmpty(pi As PropertyInfo, eventInstance As IEvent) As Boolean

            Dim oVal = pi.GetValue(eventInstance, Nothing)

            If (oVal Is Nothing) Then
                Return True
            End If

            'special case - dates before 1601-01-01
            'DateTimeOffset
            If pi.PropertyType Is GetType(DateTimeOffset) Then
                Dim Val As DateTimeOffset = DirectCast(oVal, DateTimeOffset)
                Return (Val.Year <= 1601)
            End If

            'Date
            If pi.PropertyType Is GetType(Date) Then
                Dim Val As Date = DirectCast(oVal, Date)
                Return (Val.Year <= 1601)
            End If

            Return False

        End Function

        Protected Function IsContextProperty(ByVal propertyName As String) As Boolean

            Return {FIELDNAME_COMMENTS,
                    FIELDNAME_EVENTTYPE,
                    FIELDNAME_SOURCE,
                    FIELDNAME_VERSION,
                    FIELDNAME_WHO}.Contains(propertyName, StringComparer.OrdinalIgnoreCase())

        End Function


        Protected Function GetCurrentHighestSequence() As Long

            If (m_aggregatekeyTable IsNot Nothing) Then
                'get the TableAggregateKeyRecord
                Dim qryKeyRecord = New TableQuery(Of TableAggregateKeyRecord)().Where(
                    TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, m_aggregateClassName),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, m_converter.ToUniqueString(m_key))
                     )
                   )
                Dim currentRecord As TableAggregateKeyRecord = m_aggregatekeyTable.ExecuteQuery(Of TableAggregateKeyRecord)(qryKeyRecord).FirstOrDefault()
                If (currentRecord Is Nothing) Then
                    currentRecord = New TableAggregateKeyRecord(m_aggregateClassName, m_converter.ToUniqueString(m_key))
                    currentRecord.LastSequence = 0
                    m_aggregatekeyTable.Execute(TableOperation.Insert(currentRecord))
                End If
                Return currentRecord.LastSequence
            End If

            Throw New EventStreamReadException(DomainName, AggregateClassName, m_key.ToString, 0, "Unable to determine current highest sequence number")

        End Function

        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String,
                          ByVal AggregateKey As TAggregateKey,
                          Optional ByVal writeAccess As Boolean = False,
                          Optional ByVal connectionStringName As String = "",
                          Optional ByVal settings As ITableSettings = Nothing)

            MyBase.New(AggregateDomainName, writeAccess, connectionStringName, settings)

            'Get the aggregation instance key to use when creating a blob file name
            m_key = AggregateKey

            If (settings IsNot Nothing) Then
                m_settings = settings
            End If

            Dim typeName As String = GetType(TAggregate).Name
            m_aggregateClassName = MakeValidStorageTableName(typeName)
            If Not String.IsNullOrWhiteSpace(AggregateDomainName) Then
                Dim includeDomain As Boolean = False
                If (settings IsNot Nothing) Then
                    includeDomain = settings.IncludeDomainInTableName
                End If
                If (includeDomain) Then
                    m_tableName = MakeValidStorageTableName(AggregateDomainName & typeName)
                Else
                    m_tableName = m_aggregateClassName
                End If
            Else
                m_tableName = m_aggregateClassName
            End If

            If (m_cloudTableClient IsNot Nothing) Then
                m_table = m_cloudTableClient.GetTableReference(m_tableName)
                If (m_table IsNot Nothing) Then
                    m_table.CreateIfNotExists()
                End If
                m_aggregatekeyTable = m_cloudTableClient.GetTableReference(MakeValidStorageTableName(AggregateDomainName & " " & TABLENAME_SUFFIX_KEYS))
                If (m_aggregatekeyTable IsNot Nothing) Then
                    m_aggregatekeyTable.CreateIfNotExists()
                End If
            End If

            m_converter = KeyConverterFactory.CreateKeyConverter(Of TAggregateKey)

        End Sub

        Protected Function GetAllStreamKeysBase(asOfDate As Date?) As IEnumerable(Of TAggregateKey)

            Dim ret As New List(Of TAggregateKey)
            If (m_aggregatekeyTable IsNot Nothing) Then
                Dim qryKeys = New TableQuery(Of TableAggregateKeyRecord)().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, m_aggregateClassName))

                For Each aggKey As TableAggregateKeyRecord In m_aggregatekeyTable.ExecuteQuery(Of TableAggregateKeyRecord)(qryKeys)
                    If Not asOfDate.HasValue OrElse (asOfDate.Value < aggKey.CreatedDateTime) Then
                        ret.Add(m_converter.FromString(aggKey.RowKey))
                    End If
                Next

            End If
            Return ret

        End Function
    End Class


    Public MustInherit Class TableEventStreamBase
        Inherits AzureStorageEventStreamBase

        Public Const MAX_FREE_DATA_FIELDS = 248
        Public Const TABLENAME_SUFFIX_KEYS = "Aggregates"

#Region "private members"
        Private ReadOnly m_domainName As String
        Protected ReadOnly m_cloudTableClient As CloudTableClient
#End Region



        Protected Sub New(ByVal AggregateDomainName As String,
                          Optional ByVal writeAccess As Boolean = False,
                          Optional ByVal connectionStringName As String = "",
                          Optional ByVal settings As ITableSettings = Nothing)

            MyBase.New(AggregateDomainName, writeAccess, connectionStringName, settings)

            If (settings IsNot Nothing) Then
                If Not String.IsNullOrWhiteSpace(settings.SequenceNumberFormat) Then
                    VERSION_FORMAT = settings.SequenceNumberFormat
                End If
            End If

            If (m_storageAccount IsNot Nothing) Then
                m_cloudTableClient = m_storageAccount.CreateCloudTableClient()
            End If

        End Sub

#Region "Shared methods"
        Private ReadOnly VERSION_FORMAT As String = CQRSAzureEventSourcingTableSettingsElement.DEFAULT_SEQUENCENUMBERFORMAT
        Public Function SequenceNumberToRowkey(ByVal version As Long) As String

            If (version < 0) Then
                Return SequenceNumberToRowkey(0)
            Else
                Return version.ToString(VERSION_FORMAT)
            End If

        End Function

        Public Function RowKeyToSequenceNumber(ByVal rowKey As String) As Long

            If Not String.IsNullOrWhiteSpace(rowKey) Then
                Dim seqNumPart As String = rowKey.Substring(0, VERSION_FORMAT.Length).TrimStart("0")
                If Not String.IsNullOrWhiteSpace(seqNumPart) Then
                    Dim ret As UInteger = 0
                    If (UInteger.TryParse(seqNumPart, ret)) Then
                        Return ret
                    End If
                End If
            End If

            Return 0

        End Function


        Public Shared Function MakeEntityProperty(piType As Type, oval As Object) As EntityProperty



            If (oval Is Nothing) Then
                Return New EntityProperty(String.Empty)
            End If

            'cast it to the correct type?
            If piType Is GetType(Boolean) Then
                Dim boolVal As Boolean = DirectCast(oval, Boolean)
                Return MakeEntityProperty(boolVal)
            End If

            If piType Is GetType(Double) Then
                Dim dblVal As Double = DirectCast(oval, Double)
                Return MakeEntityProperty(dblVal)
            End If

            If piType Is GetType(Decimal) Then
                Dim dclVal As Decimal = DirectCast(oval, Decimal)
                Dim dblVal As Double = dclVal
                Return MakeEntityProperty(dblVal)
            End If

            If piType Is GetType(Integer) Then
                Dim intVal As Integer = DirectCast(oval, Integer)
                Return MakeEntityProperty(intVal)
            End If

            If piType Is GetType(Long) Then
                Dim lngVal As Long = DirectCast(oval, Long)
                Return MakeEntityProperty(lngVal)
            End If

            'DateTimeOffset
            If piType Is GetType(DateTimeOffset) Then
                Dim Val As DateTimeOffset = DirectCast(oval, DateTimeOffset)
                Return MakeEntityProperty(Val)
            End If

            'Guid
            If piType Is GetType(Guid) Then
                Dim guidVal As Guid = DirectCast(oval, Guid)
                Return MakeEntityProperty(guidVal)
            End If

            'Date
            If piType Is GetType(Date) Then
                Dim dateVal As Date = DirectCast(oval, Date)
                Return MakeEntityProperty(dateVal)
            End If

            'Byte()
            If piType Is GetType(Byte()) Then
                Dim byteVal As Byte() = DirectCast(oval, Byte())
                Return MakeEntityProperty(byteVal)
            End If

            Return MakeEntityProperty(oval)
        End Function

        ''' <summary>
        ''' Turn a property of an event into an entity property that can be stored in an Azure table
        ''' </summary>
        ''' <param name="pi">
        ''' The specific property in from the event
        ''' </param>
        ''' <param name="inputEvent">
        ''' The parent event to which that proeprty belongs
        ''' </param>
        ''' <remarks>
        ''' For a production system we will probably replace reflection with an interface with "ToDynamicTableEntity" to be implemented by every 
        ''' event class but this reflection based system is useful for getting started and rapid prototyping
        ''' </remarks>
        Protected Shared Function MakeEntityProperty(pi As Reflection.PropertyInfo, inputEvent As IEvent) As EntityProperty

            'If the property is one of the Azure entity property types, use that
            Dim oVal = pi.GetValue(inputEvent, Nothing)
            Return MakeEntityProperty(pi.PropertyType, oVal)

        End Function
        Public Shared Function MakeEntityProperty(oVal As Boolean) As EntityProperty

            Return New EntityProperty(oVal)

        End Function

        Public Shared Function MakeEntityProperty(oVal As Double) As EntityProperty

            Return New EntityProperty(oVal)

        End Function

        Public Shared Function MakeEntityProperty(oVal As Integer) As EntityProperty

            Return New EntityProperty(oVal)

        End Function

        Public Shared Function MakeEntityProperty(oVal As Long) As EntityProperty

            Return New EntityProperty(oVal)

        End Function

        Public Shared Function MakeEntityProperty(oVal As DateTimeOffset) As EntityProperty

            Return New EntityProperty(oVal)

        End Function

        Public Shared Function MakeEntityProperty(oVal As Guid) As EntityProperty

            Return New EntityProperty(oVal)

        End Function


        Public Shared Function MakeEntityProperty(oVal As Date) As EntityProperty

            Return New EntityProperty(oVal)

        End Function

        Public Shared Function MakeEntityProperty(oVal As Byte()) As EntityProperty

            Return New EntityProperty(oVal)

        End Function

        Public Shared Function MakeEntityProperty(oVal As Object) As EntityProperty

            If (oVal Is Nothing) Then
                Return New EntityProperty(String.Empty)
            End If



            'Otherwise default to saving it as a string 
            Return New EntityProperty(oVal.ToString)

        End Function

        Public Const ORPHANS_TABLE As String = "Uncategorised"
        ''' <summary>
        ''' Turn the raw input into a valid Microsoft Azure table name
        ''' </summary>
        ''' <param name="rawName">
        ''' The raw class name that we want to turn into a table
        ''' </param>
        ''' <remarks>
        ''' Table names must conform to these rules:
        ''' • Table names may contain only alphanumeric characters.
        ''' • A table name may Not begin with a numeric character. 
        ''' • Table names are case-insensitive.
        ''' • Table names must be from 3 through 63 characters long.
        ''' </remarks>
        Public Shared Function MakeValidStorageTableName(ByVal rawName As String) As String

            If (String.IsNullOrWhiteSpace(rawName)) Then
                ' Don't allow empty table names - assign an orphan table for it
                Return ORPHANS_TABLE
            Else
                Dim invalidCharacters As Char() = " !,.;':@£$%^&*()-+=/\#~{}[]?<>_"
                Dim cleanName As String = String.Join("", rawName.Split(invalidCharacters))
                If (cleanName.Length < 3) Then
                    cleanName += "DATA"
                End If
                If (cleanName.Length > 63) Then
                    Dim uniqueness As Integer = Math.Abs(cleanName.GetHashCode())
                    Dim uniqueid As String = uniqueness.ToString()
                    cleanName = cleanName.Substring(0, 63 - uniqueid.Length) & uniqueid
                End If
                Return cleanName
            End If
        End Function
#End Region

    End Class
End Namespace