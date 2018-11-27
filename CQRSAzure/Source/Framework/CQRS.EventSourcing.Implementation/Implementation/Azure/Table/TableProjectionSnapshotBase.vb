Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Table
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table

    ''' <summary>
    ''' Base functionality for storing projection snapshots in an Azure table
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of the aggregation to which the event stream and projection are being run
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type of the key that uniquely identifies an instance of this aggregation
    ''' </typeparam>
    ''' <typeparam name="TProjection">
    ''' The type of the projection being snapshotted
    ''' </typeparam>
    ''' <remarks>
    ''' The snapshot table contains one row per key/sequence number and row
    ''' The as-of date is held as a field in that row 
    ''' </remarks>
    Public MustInherit Class TableProjectionSnapshotBase(Of TAggregate As IAggregationIdentifier,
                                                            TAggregateKey,
                                                            TProjection As IProjection)
        Inherits TableProjectionSnapshotBase

        'The table reference to use when accessing the underlying snapshots table
        Protected ReadOnly m_table As CloudTable
        Protected ReadOnly m_cloudTableClient As CloudTableClient

        Protected Friend ReadOnly Property Table As CloudTable
            Get
                Return m_table
            End Get
        End Property

        Private m_key As TAggregateKey

        Protected Friend ReadOnly Property Key As String
            Get
                If (m_key Is Nothing) Then
                    Return String.Empty
                Else
                    Return m_key.ToString()
                End If
            End Get
        End Property

        Private ReadOnly Property m_tableName As String
        Protected ReadOnly Property TableName As String
            Get
                Return m_tableName
            End Get
        End Property

        Private ReadOnly Property m_projectionClassName As String

        Protected Sub New(ByVal AggregateDomainName As String,
                  ByVal AggregateKey As TAggregateKey,
                  Optional ByVal writeAccess As Boolean = False,
                  Optional ByVal connectionStringName As String = "",
                  Optional ByVal settings As ITableSettings = Nothing)

            MyBase.New(AggregateDomainName, writeAccess, connectionStringName, settings)

            'Get the aggregation instance key to use when creating a table row key
            m_key = AggregateKey

            'make the valid table name
            Dim typeName As String = GetType(TProjection).Name
            m_projectionClassName = TableEventStreamBase.MakeValidStorageTableName(typeName)
            If Not String.IsNullOrWhiteSpace(AggregateDomainName) Then
                Dim includeDomain As Boolean = False
                If (settings IsNot Nothing) Then
                    includeDomain = settings.IncludeDomainInTableName
                End If
                If (includeDomain) Then
                    m_tableName = TableEventStreamBase.MakeValidStorageTableName(AggregateDomainName & typeName)
                Else
                    m_tableName = m_projectionClassName
                End If
            Else
                m_tableName = m_projectionClassName
            End If

            If (m_storageAccount IsNot Nothing) Then
                m_cloudTableClient = m_storageAccount.CreateCloudTableClient()
            End If

            If (m_cloudTableClient IsNot Nothing) Then
                m_table = m_cloudTableClient.GetTableReference(m_tableName)
                If (m_table IsNot Nothing) Then
                    m_table.CreateIfNotExistsAsync()
                End If
            End If

        End Sub

    End Class

    Public MustInherit Class TableProjectionSnapshotBase
        Inherits AzureStorageEventStreamBase


        Private ReadOnly VERSION_FORMAT As String = CQRSAzureEventSourcingTableSettingsElement.DEFAULT_SEQUENCENUMBERFORMAT
        Private ReadOnly ROW_FORMAT As String = CQRSAzureEventSourcingTableSettingsElement.DEFAULT_SEQUENCENUMBERFORMAT

        Public Sub New(AggregateDomainName As String,
                       Optional writeAccess As Boolean = False,
                       Optional connectionStringName As String = "",
                       Optional settings As ITableSettings = Nothing)

            MyBase.New(AggregateDomainName, writeAccess, connectionStringName, settings)

            If (settings IsNot Nothing) Then
                If Not String.IsNullOrWhiteSpace(settings.SequenceNumberFormat) Then
                    VERSION_FORMAT = settings.SequenceNumberFormat
                End If
                If Not String.IsNullOrWhiteSpace(settings.RowNumberFormat) Then
                    ROW_FORMAT = settings.RowNumberFormat
                End If
            End If

        End Sub

#Region "Shared functions"

        ''' <summary>
        ''' Turns teh sequence number and row pair into a key that canbe used to search for a snapshot row in a table
        ''' </summary>
        ''' <param name="sequenceNumber">
        ''' The event stream sequence number as of which the snapshot was taken
        ''' </param>
        ''' <param name="rowNumber">
        ''' The row number of the row of data that the snapshot is for
        ''' </param>
        ''' <remarks>
        ''' A snapshot will always have a row 0 so this can be used to find a snapshot by sequence number only
        ''' </remarks>
        Public Function SequenceAndRowToRowKey(ByVal sequenceNumber As UInteger, ByVal rowNumber As UInteger) As String

            Return sequenceNumber.ToString(VERSION_FORMAT) & "." & rowNumber.ToString(ROW_FORMAT)

        End Function

        ''' <summary>
        ''' Turns the row key into a sequence number
        ''' </summary>
        ''' <remarks>
        ''' A snapshot will always have a row 0 so this can be used to find a snapshot by sequence number only
        ''' </remarks>
        Public Function SequenceFromRowKey(ByVal rowKey As String) As UInteger

            Dim seqNumPart As String = rowKey.Substring(0, VERSION_FORMAT.Length).TrimStart("0")
            If Not String.IsNullOrWhiteSpace(seqNumPart) Then
                Dim ret As UInteger = 0
                If (UInteger.TryParse(seqNumPart, ret)) Then
                    Return ret
                End If
            End If

            Return 0

        End Function

        Public Function RowNumberFromRowKey(ByVal rowKey As String) As UInteger

            Dim seqNumPart As String = rowKey.Substring(VERSION_FORMAT.Length + 1).TrimStart("0")
            If Not String.IsNullOrWhiteSpace(seqNumPart) Then
                Dim ret As UInteger = 0
                If (UInteger.TryParse(seqNumPart, ret)) Then
                    Return ret
                End If
            End If
            Return 0
        End Function

#End Region

    End Class

End Namespace