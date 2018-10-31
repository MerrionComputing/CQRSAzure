Imports CQRSAzure.EventSourcing.Azure.Table

Namespace Azure.Table

    Public Class AzureTableClassifier(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                          TAggregateKey,
                                          TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})

        ''' <summary>
        ''' Create a classifier processor that works off an azure table backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run classifications
        ''' </param>
        ''' <returns>
        ''' A classification processor that can run classifiers over this event stream
        ''' </returns>
        Public Shared Function CreateClassifierProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                         Optional ByVal settings As ITableSettings = Nothing,
                                                         Optional ByVal eventsToFilter As IEnumerable(Of Type) = Nothing,
                                                         Optional classifier As TClassifier = Nothing,
                                                         Optional snapshotReader As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier) = Nothing,
                                                         Optional snapshotWriter As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier) = Nothing) As ClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier)

            If (classifier Is Nothing) Then
                classifier = New TClassifier()
            End If

            Return New ClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier)(TableEventStreamReader(Of TAggregate, TAggregateKey).Create(instance, settings, eventsToFilter),
                                                                                        classifier,
                                                                                        snapshotReader,
                                                                                        snapshotWriter)

        End Function

    End Class
End Namespace
