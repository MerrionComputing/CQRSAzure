Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.Blob

Namespace Azure.Blob


    Public NotInheritable Class AzureBlobClassifier(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                        TAggregateKey,
                                                        TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})


        ''' <summary>
        ''' Create a classifier processor that works off an azure blob backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run classifications
        ''' </param>
        ''' <param name="settings">
        ''' (Optional) Settings used to read the event stream from a blob
        ''' </param> 
        ''' <param name="eventFilter">
        ''' (Optional) Filter to only read certain events from the blob event stream
        ''' </param>
        ''' <param name="classifier">
        ''' (Optional) The classifier used to test if the instance is in the group or not
        ''' </param>
        ''' <param name="snapshotReader">
        ''' (Optional) Class to read snapshots for the classifier
        ''' </param>
        ''' <param name="snapshotWriter">
        ''' (Optional) Class to write snapshots for the classifier
        ''' </param>
        ''' <returns>
        ''' A classification processor that can run classifiers over this event stream
        ''' </returns>
        Public Shared Function CreateClassifierProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                                         Optional ByVal settings As IBlobStreamSettings = Nothing,
                                                         Optional ByVal eventFilter As IEnumerable(Of Type) = Nothing,
                                                         Optional classifier As TClassifier = Nothing,
                                                         Optional streamReader As IEventStreamReader(Of TAggregate, TAggregateKey) = Nothing,
                                                         Optional snapshotReader As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier) = Nothing,
                                                         Optional snapshotWriter As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier) = Nothing) As ClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier)

            If (classifier Is Nothing) Then
                classifier = New TClassifier()
            End If

            If (streamReader Is Nothing) Then
                streamReader = BlobEventStreamReader(Of TAggregate, TAggregateKey).Create(instance, settings, eventFilter)
            End If

            Return New ClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier)(streamReader,
                                                                                        classifier,
                                                                                        snapshotReader,
                                                                                        snapshotWriter)

        End Function

    End Class
End Namespace