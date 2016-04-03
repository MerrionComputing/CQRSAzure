Imports CQRSAzure.EventSourcing.Azure.Blob

Namespace Azure.Blob


    Public Class AzureBlobClassifier(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)


        ''' <summary>
        ''' Create a classifier processor that works off an azure blob backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to run classifications
        ''' </param>
        ''' <returns>
        ''' A classification processor that can run classifiers over this event stream
        ''' </returns>
        Public Shared Function CreateClassifierProcessor(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregationKey)) As ClassifierProcessor(Of TAggregate, TAggregationKey)

            Return New ClassifierProcessor(Of TAggregate, TAggregationKey)(BlobEventStreamReader(Of TAggregate, TAggregationKey).Create(instance))

        End Function

    End Class
End Namespace