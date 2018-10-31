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


    Public NotInheritable Class AzureBlobClassifierUntyped


        ''' <summary>
        ''' Create a classifier processor to run the given classifier over the event stream
        ''' </summary>
        ''' <param name="identifier">
        ''' The unique identifier of the thing we are going to run the classifier over the event stream of
        ''' </param>
        ''' <param name="classifier">
        ''' The classifier logic to run over the event stream
        ''' </param>
        ''' <param name="settings">
        ''' Configuration settings to use to access the underlying blob
        ''' </param>
        ''' <param name="streamReader">
        ''' A created stream reader to use to read the event stream
        ''' (if this is not passed in a default stream reader will be created)
        ''' </param>
        ''' <param name="snapshotReader">
        ''' (Optional) A reader to use to load snapshots of this classifier 
        ''' </param>
        ''' <param name="snapshotWriter">
        ''' (Optional) A writer to use to save snapshots of this classifier
        ''' </param>
        Public Shared Function CreateClassifierProcessor(ByVal identifier As IEventStreamUntypedIdentity,
                                                         ByVal classifier As IClassifierUntyped,
                                                         Optional ByVal settings As IBlobStreamSettings = Nothing,
                                                         Optional streamReader As IEventStreamReaderUntyped = Nothing,
                                                         Optional snapshotReader As IClassifierSnapshotReaderUntyped = Nothing,
                                                         Optional snapshotWriter As IClassifierSnapshotWriterUntyped = Nothing) As ClassifierProcessorUntyped



            If (streamReader Is Nothing) Then
                streamReader = Untyped.BlobEventStreamReaderUntyped.Create(identifier, settings)
            End If

            Return New ClassifierProcessorUntyped(streamReader, classifier, snapshotReader, snapshotWriter)

        End Function

    End Class


End Namespace