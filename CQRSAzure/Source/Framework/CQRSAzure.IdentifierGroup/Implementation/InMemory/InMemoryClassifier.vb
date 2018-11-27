Imports CQRSAzure.EventSourcing.Implementation.InMemory
Imports CQRSAzure.EventSourcing.InMemory

Namespace InMemory
    Public NotInheritable Class InMemoryClassifier(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                       TAggregateKey,
                                                       TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})


        ''' <summary>
        ''' Create a classifier processor that works off an in memory backed event stream
        ''' </summary>
        ''' <param name="instance">
        ''' The unique identifier of the instance of the aggregate for which we want to run classifications
        ''' </param>
        ''' <param name="settings">
        ''' (Optional) Settings used to read the event stream from  in memory
        ''' </param> 
        ''' <param name="eventsFilterFunction">
        ''' (Optional) Filter function to only read certain events from the in-memory event stream
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
        Public Shared Function CreateClassifierProcessor(ByVal instance As TAggregateKey,
                                                         Optional ByVal settings As IInMemorySettings = Nothing,
                                                         Optional ByVal eventsFilterFunction As EventSourcing.FilterFunctions.EventFilterFunction = Nothing,
                                                         Optional classifier As TClassifier = Nothing,
                                                         Optional snapshotReader As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier) = Nothing,
                                                         Optional snapshotWriter As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier) = Nothing) As ClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier)

            If classifier Is Nothing Then
                classifier = New TClassifier()
            End If

            Return New ClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier)(InMemoryEventStreamReader(Of TAggregate, TAggregateKey).Create(instance,
                                                                                                                                                     settings:=settings,
                                                                                                                                                     eventFilterFunction:=eventsFilterFunction),
                                                                                        classifier,
                                                                                        snapshotReader,
                                                                                        snapshotWriter)

        End Function

    End Class

    ''' <summary>
    ''' Interface to create classifier processors over an in-memory event stream
    ''' </summary>
    ''' <remarks>
    ''' This can be passed to the classifier filter provider to classify identifier groups on demand
    ''' </remarks>
    Public NotInheritable Class InMemoryClassifierProcessorFactory(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                       TAggregateKey,
                                                       TClassifier As {IClassifier(Of TAggregate, TAggregateKey), New})
        Implements IClassifierProcessorFactory(Of TAggregate, TAggregateKey, TClassifier)

#Region "Private variables"
        Private ReadOnly m_settings As IInMemorySettings
        Private ReadOnly m_eventsFilterFunction As EventSourcing.FilterFunctions.EventFilterFunction
        Private ReadOnly m_snapshotReader As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier)
        Private ReadOnly m_snapshotWriter As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier)
#End Region

        Public Function GetClassifierFilterProvider(key As TAggregateKey) As IClassifierProcessor(Of TAggregate, TAggregateKey, TClassifier) Implements IClassifierProcessorFactory(Of TAggregate, TAggregateKey, TClassifier).GetClassifierFilterProvider

            'Always create a fresh classifier so no state variables can leak..
            Dim classifierToUse As TClassifier = New TClassifier()

            Return InMemoryClassifier(Of TAggregate, TAggregateKey, TClassifier).CreateClassifierProcessor(key,
                                                                                                    settings:=m_settings,
                                                                                                    eventsFilterFunction:=m_eventsFilterFunction,
                                                                                                    snapshotReader:=m_snapshotReader,
                                                                                                    snapshotWriter:=m_snapshotWriter)

        End Function


        Public Sub New(Optional ByVal settings As IInMemorySettings = Nothing,
                       Optional ByVal eventsFilterFunction As EventSourcing.FilterFunctions.EventFilterFunction = Nothing,
                       Optional snapshotReader As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier) = Nothing,
                       Optional snapshotWriter As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier) = Nothing)

            If (settings IsNot Nothing) Then
                m_settings = settings
            End If

            If (eventsFilterFunction IsNot Nothing) Then
                m_eventsFilterFunction = eventsFilterFunction
            End If

            If (snapshotReader IsNot Nothing) Then
                m_snapshotReader = snapshotReader
            End If

            If (snapshotWriter IsNot Nothing) Then
                m_snapshotWriter = snapshotWriter
            End If

        End Sub

    End Class
End Namespace