Imports System
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Table
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table

    Public NotInheritable Class TableProjectionSnapshotReader(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                  TAggregateKey,
                                                                  TProjection As IProjection)
        Inherits TableProjectionSnapshotBase(Of TAggregate, TAggregateKey, TProjection)
        Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)



        Public Async Function GetSnapshot(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As Task(Of IProjectionSnapshot(Of TAggregate, TAggregateKey)) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetSnapshot

            If (OnOrBeforeSequence = 0) Then
                OnOrBeforeSequence = UInteger.MaxValue
            End If

            OnOrBeforeSequence = Await GetLatestSnapshotSequence(key, OnOrBeforeSequence)

            If (MyBase.Table IsNot Nothing) Then



                Dim latestSnapshotQuery = New TableQuery(Of DynamicTableEntity)().Where(
                    TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, MyBase.Key),
                    TableOperators.And,
                    TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey",
                    QueryComparisons.LessThanOrEqual,
                                SequenceAndRowToRowKey(1 + OnOrBeforeSequence, 0)),
                                TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey",
                    QueryComparisons.GreaterThanOrEqual,
                                SequenceAndRowToRowKey(OnOrBeforeSequence, 0))
                    ))
                    )

                Dim ret As ProjectionSnapshot(Of TAggregate, TAggregateKey) = ProjectionSnapshot.Create(Of TAggregate, TAggregateKey)(OnOrBeforeSequence, DateTime.MinValue)

                Dim token As New TableContinuationToken()
                Dim querySegment = MyBase.Table.ExecuteQuerySegmentedAsync(latestSnapshotQuery, token)
                For Each e In querySegment.Result
                    'each record is a row of values for the projection
                    Dim rowNumber = RowNumberFromRowKey(e.RowKey)
                    For Each field In e.Properties
                        Select Case field.Value.PropertyType
                            Case EdmType.Binary
                                ret.AddValue(rowNumber, field.Key, field.Value.BinaryValue)
                            Case EdmType.Boolean
                                ret.AddValue(rowNumber, field.Key, field.Value.BooleanValue)
                            Case EdmType.DateTime
                                ret.AddValue(rowNumber, field.Key, field.Value.DateTime)
                            Case EdmType.Double
                                ret.AddValue(rowNumber, field.Key, field.Value.DoubleValue)
                            Case EdmType.Guid
                                ret.AddValue(rowNumber, field.Key, field.Value.GuidValue)
                            Case EdmType.Int32
                                ret.AddValue(rowNumber, field.Key, field.Value.Int32Value)
                            Case EdmType.Int64
                                ret.AddValue(rowNumber, field.Key, field.Value.Int64Value)
                            Case EdmType.String
                                ret.AddValue(rowNumber, field.Key, field.Value.StringValue)
                            Case Else
                                ret.AddValue(rowNumber, field.Key, field.Value.PropertyAsObject)
                        End Select
                    Next
                Next

                Return ret

            Else
                Throw New EventStreamReadException(DomainName, MyBase.TableName, key.ToString(), OnOrBeforeSequence, "Projection snapshot table not initialised")
            End If


        End Function

        Public Function GetLatestSnapshotSequence(key As TAggregateKey, Optional OnOrBeforeSequence As UInteger = 0) As Task(Of UInteger) Implements IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).GetLatestSnapshotSequence

            Return Task.Factory.StartNew(Of UInteger)(Function()



                                                          If (OnOrBeforeSequence = 0) Then
                                                              OnOrBeforeSequence = UInteger.MaxValue
                                                          End If

                                                          If (MyBase.Table IsNot Nothing) Then

                                                              Dim latestSnapshotQuery = New TableQuery(Of DynamicTableEntity)().Where(
                    TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, MyBase.Key),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey",
                    QueryComparisons.LessThanOrEqual,
                                SequenceAndRowToRowKey(OnOrBeforeSequence, 0))
                    )
                    ).Select({"RowKey"})

                                                              Dim currentHighest As Integer
                                                              Dim token As New TableContinuationToken()
                                                              Dim querySegment = MyBase.Table.ExecuteQuerySegmentedAsync(latestSnapshotQuery, token)
                                                              For Each e In querySegment.Result
                                                                  If SequenceFromRowKey(e.RowKey) > currentHighest Then
                                                                      currentHighest = SequenceFromRowKey(e.RowKey)
                                                                  End If
                                                              Next
                                                              Return currentHighest

                                                          End If

                                                          'if we didn't find any snapshot - return 0
                                                          Return 0

                                                      End Function
            )

        End Function


        Public Sub New(AggregateDomainName As String,
                       AggregateKey As TAggregateKey,
                       Optional connectionStringName As String = "",
                       Optional settings As ITableSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, False, connectionStringName, settings)

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure table storage based projection snapshot reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the snapshot
        ''' </param>
        ''' <param name="key">
        ''' The key that uniquely identifies the instance for which we are reading a snapshot
        ''' </param>
        ''' <param name="projection">
        ''' The projection itself for which the snapshot is being loaded
        ''' </param>
        Public Shared Function Create(ByVal instance As TAggregate,
                                      ByVal key As TAggregateKey,
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As ITableSettings = Nothing) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            If settings IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(settings.DomainName) Then
                    domainName = settings.DomainName
                End If
            End If
            Return New TableProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)(domainName, key, "", settings)

        End Function


#End Region
    End Class

    Public Module TableProjectionSnapshotReaderFactory

        ''' <summary>
        ''' Creates an azure table storage based projection snapshot reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the snapshot
        ''' </param>
        ''' <param name="key">
        ''' The key that uniquely identifies the instance for which we are reading a snapshot
        ''' </param>
        ''' <param name="projection">
        ''' The projection itself for which the snapshot is being loaded
        ''' </param>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                  TAggregateKey,
                                                                  TProjection As IProjection)(ByVal instance As TAggregate,
                                      ByVal key As TAggregateKey,
                                      ByVal projection As TProjection,
                                      Optional ByVal settings As ITableSettings = Nothing) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection)

            Return TableProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection).Create(instance, key, projection, settings)

        End Function

    End Module

End Namespace