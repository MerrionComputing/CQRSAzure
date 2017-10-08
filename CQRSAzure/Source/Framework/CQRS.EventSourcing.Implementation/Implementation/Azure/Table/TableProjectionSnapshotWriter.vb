Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table

    Public NotInheritable Class TableProjectionSnapshotWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                  TAggregateKey,
                                                                  TProjection As IProjection)
        Inherits TableProjectionSnapshotBase(Of TAggregate, TAggregateKey, TProjection)
        Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

        Public Sub SaveSnapshot(key As TAggregateKey,
                                snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).SaveSnapshot


            'Save each row of values separately
            Dim insertRows As New TableBatchOperation()
            For valuesRow As Integer = 0 To snapshotToSave.RowCount - 1
                insertRows.Insert(MakeTableEntity(key, valuesRow, snapshotToSave))
                If (valuesRow > 0) AndAlso (valuesRow Mod 100) = 0 Then
                    'save this batch
                    MyBase.Table.ExecuteBatch(insertRows)
                    insertRows = New TableBatchOperation()
                End If
            Next

            If (insertRows.Count > 0) Then
                MyBase.Table.ExecuteBatch(insertRows)
            End If

        End Sub

        ''' <summary>
        ''' Make a table entity to store one row of values for this snapshot
        ''' </summary>
        ''' <param name="key">
        ''' The key that uniquely identifies the aggregate instance being snapshotted
        ''' </param>
        ''' <param name="valuesRow">
        ''' The ordinal position of the row of values being saved
        ''' </param>
        ''' <param name="snapshotToSave">
        ''' The snapshot with all the data values to save
        ''' </param>
        ''' <returns></returns>
        Private Function MakeTableEntity(key As TAggregateKey, valuesRow As Integer, snapshotToSave As IProjectionSnapshot(Of TAggregate, TAggregateKey)) As DynamicTableEntity

            Dim ret As New DynamicTableEntity()

            ret.PartitionKey = key.ToString()
            ret.RowKey = SequenceAndRowToRowKey(snapshotToSave.Sequence, valuesRow)

            'all the values save as name-value pairs...
            Dim propertiesCount As Integer = 0
            For Each rowValue In snapshotToSave.Values.Where(Function(f) f.RowNumber = valuesRow)
                If (propertiesCount > TableEventStreamBase.MAX_FREE_DATA_FIELDS) Then
                    'Throw an error to indicate that our event exceeds the storage capabilities of an azure table
                    Throw New EventStreamWriteException(DomainName, "", ret.PartitionKey, snapshotToSave.Sequence, "Too many properties for snapshot - cannot store in Azure table")
                Else
                    If (rowValue.Value IsNot Nothing) Then
                        ret.Properties.Add(rowValue.Name, TableEventStreamBase.MakeEntityProperty(rowValue.Value.GetType(), rowValue.Value))
                    End If
                End If
                propertiesCount = propertiesCount + 1
            Next

            Return ret

        End Function

        ''' <summary>
        ''' Reset the snapshot list
        ''' </summary>
        ''' <remarks>
        ''' This should only be used for unit testing so is not part of the core IProjectionSnapshotWriter interface
        ''' </remarks>
        Public Sub Reset()
            If MyBase.Table IsNot Nothing Then
                'A Batch Operation allows a maximum 100 entities in the batch which must share the same PartitionKey 
                Dim projectionQuery = New TableQuery(Of DynamicTableEntity)().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, MyBase.Key)).Select({"RowKey"}).Take(99)

                Dim moreBatches As Boolean = True
                While moreBatches
                    Dim batchDelete = New TableBatchOperation()
                    For Each e In MyBase.m_table.ExecuteQuery(projectionQuery)
                        batchDelete.Delete(e)
                    Next

                    moreBatches = (batchDelete.Count >= 99)

                    If (batchDelete.Count > 0) Then
                        MyBase.m_table.ExecuteBatch(batchDelete)
                    End If
                End While
            End If
        End Sub

        Public Sub New(AggregateDomainName As String,
               AggregateKey As TAggregateKey,
               Optional connectionStringName As String = "",
               Optional settings As ITableSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, True, connectionStringName, settings)

        End Sub


#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure table storage based projection snapshot writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to write the snapshot
        ''' </param>
        ''' <param name="key">
        ''' The key that uniquely identifies the instance for which we are writing a snapshot
        ''' </param>
        ''' <param name="projection">
        ''' The projection itself for which the snapshot is being saved
        ''' </param>
        Public Shared Function Create(ByVal instance As TAggregate,
                                      ByVal key As TAggregateKey,
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As ITableSettings = Nothing) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New TableProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)(domainName, key, "", settings)

        End Function


#End Region

    End Class

    Public Module TableProjectionSnapshotWriterFactory

        ''' <summary>
        ''' Creates an azure file storage based event stream writer for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <param name="projection">
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                  TAggregateKey,
                                                                  TProjection As IProjection) _
                                      (ByVal instance As TAggregate,
                                      ByVal key As TAggregateKey,
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As ITableSettings = Nothing) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection)


            Return TableProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection).Create(instance, key, projection, settings)

        End Function

    End Module

End Namespace
