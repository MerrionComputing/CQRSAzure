Imports CQRSAzure.EventSourcing

''' <summary>
''' A mapping between an aggregate identifier and the event stream techniology used as backing to it
''' </summary>
''' <typeparam name="TAggregate">
''' </typeparam>
''' <remarks>
''' This allows different aggregate types to have different backing store technologies
''' </remarks>
Public Class AggregateEventStreamImplementationMap(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Implements IAggregateImplementationMap(Of TAggregate, TAggregateKey)

    Private ReadOnly m_settings As IEventStreamSettings
    Private ReadOnly m_readerCreator As IAggregateImplementationMap.ReaderCreationFunction(Of TAggregate, TAggregateKey)
    Private ReadOnly m_writerCreator As IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey)

    Public Function CreateReader(ByVal aggregate As TAggregate, key As TAggregateKey) As IEventStreamReader(Of TAggregate, TAggregateKey) Implements IAggregateImplementationMap(Of TAggregate, TAggregateKey).CreateReader

        If (m_readerCreator IsNot Nothing) Then
            Return m_readerCreator.Invoke(aggregate, key, m_settings)
        Else
            'Notify an unmapped aggregate exists
            Throw New UnmappedAggregateException(aggregate.GetType.Name, aggregate.GetAggregateIdentifier)
        End If

    End Function

    Public Function CreateProjectionProcessor(aggregate As TAggregate, key As TAggregateKey) As IProjectionProcessor(Of TAggregate, TAggregateKey) Implements IAggregateImplementationMap(Of TAggregate, TAggregateKey).CreateProjectionProcessor
        Return New ProjectionProcessor(Of TAggregate, TAggregateKey)(CreateReader(aggregate, key))
    End Function

    Public Function CreateWriter(ByVal aggregate As TAggregate, key As TAggregateKey) As IEventStreamWriter(Of TAggregate, TAggregateKey) Implements IAggregateImplementationMap(Of TAggregate, TAggregateKey).CreateWriter

        If (m_writerCreator IsNot Nothing) Then
            Return m_writerCreator.Invoke(aggregate, key, m_settings)
        Else
            'Notify an unmapped aggregate exists
            Throw New UnmappedAggregateException(aggregate.GetType.Name, aggregate.GetAggregateIdentifier)
        End If

    End Function


    Public Function CreateSnapshotReader(Of TProjection As IProjection)(instance As IAggregationIdentifier(Of TAggregateKey), key As TAggregateKey) As IProjectionSnapshotReader(Of TAggregate, TAggregateKey, TProjection) Implements IAggregateImplementationMap(Of TAggregate, TAggregateKey).CreateSnapshotReader
        Throw New NotImplementedException()
    End Function

    Public Function CreateSnapshotWriter(Of TProjection As IProjection)(instance As IAggregationIdentifier(Of TAggregateKey), key As TAggregateKey) As IProjectionSnapshotWriter(Of TAggregate, TAggregateKey, TProjection) Implements IAggregateImplementationMap(Of TAggregate, TAggregateKey).CreateSnapshotWriter
        Throw New NotImplementedException()
    End Function


    Private Sub New(ByVal readerCreator As IAggregateImplementationMap.ReaderCreationFunction(Of TAggregate, TAggregateKey),
                    ByVal writerCreator As IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey),
                    ByVal eventStreamSettings As IEventStreamSettings,
                    ByVal snapshotSettingsToUse As CQRSAzureEventSourcingProjectionSnapshotSettingsElement
                    )

        m_readerCreator = readerCreator
        m_writerCreator = writerCreator
        m_settings = eventStreamSettings

        If (snapshotSettingsToUse IsNot Nothing) Then
            'additional settings for reading/writing snapshots in addition to how event stream is read/written

        Else
            'make snapshot reader/writer from the stream reader/writer settings

        End If

    End Sub

    ''' <summary>
    ''' Create an implementation map to use for a given aggregate type's underlying event stream and snapshot storage
    ''' </summary>
    ''' <param name="implementationToUse">
    ''' The configuration setings for the event stream implementation
    ''' </param>
    ''' <param name="snapshotSettingsToUse">
    ''' The configuration settings for the snapshot persistenece for that event stream
    ''' </param>
    ''' <returns></returns>
    Public Shared Function Create(ByVal implementationToUse As CQRSAzureEventSourcingImplementationSettingsElement,
                                  ByVal snapshotSettingsToUse As CQRSAzureEventSourcingProjectionSnapshotSettingsElement) As AggregateEventStreamImplementationMap(Of TAggregate, TAggregateKey)



        Dim readerCreator As IAggregateImplementationMap.ReaderCreationFunction(Of TAggregate, TAggregateKey) = Nothing
        Dim writerCreator As IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey) = Nothing
        Dim settings As IEventStreamSettings = Nothing

        'Decide that reader and writer creators are needed for the implementation to use..
        If (implementationToUse IsNot Nothing) Then
            Select Case implementationToUse.ImplementationType
                Case SupportedEventStreamImplementations.AzureBlob
                    'use the settings to make reader/writer for Azure Blob for the event streams
                    If (implementationToUse.BlobSettings IsNot Nothing) Then
                        settings = implementationToUse.BlobSettings
                    Else
                        'Not sure - can we default these??
                        Throw New NotImplementedException("No default setting exists for the Azure Blob implementation")
                    End If
                    readerCreator = Azure.Blob.BlobEventStreamReaderFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)
                    writerCreator = Azure.Blob.BlobEventStreamWriterFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)

                Case SupportedEventStreamImplementations.AzureFile
                    'use the settings to make a reader/writer for Azure File for the event streams
                    If (implementationToUse.FileSettings IsNot Nothing) Then
                        settings = implementationToUse.FileSettings
                    Else
                        'Not sure - can we default these??
                        Throw New NotImplementedException("No default setting exists for the Azure File implementation")
                    End If
                    readerCreator = Azure.File.FileEventStreamReaderFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)
                    writerCreator = Azure.File.FileEventStreamWriterFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)

                Case SupportedEventStreamImplementations.AzureSQL
                    Throw New NotImplementedException("Azure SQL event streams not implemented")

                Case SupportedEventStreamImplementations.AzureTable
                    'use the settings to make a reader/writer for Azure Table (NoSQL) for the event streams
                    If (implementationToUse.TableSettings IsNot Nothing) Then
                        settings = implementationToUse.TableSettings
                    Else
                        'Not sure - can we default these??
                        Throw New NotImplementedException("No default setting exists for the Azure Table (NoSQL) implementation")
                    End If
                    readerCreator = Azure.Table.TableEventStreamReaderFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)
                    writerCreator = Azure.Table.TableEventStreamWriterFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)

                Case SupportedEventStreamImplementations.InMemory
                    'use the settinsg to make an in-memory reader/writer for the event streams
                    If (implementationToUse.InMemorySettings IsNot Nothing) Then
                        settings = implementationToUse.InMemorySettings
                    Else
                        'Not sure - can we default these??
                        Throw New NotImplementedException("No default setting exists for the in-memory implementation")
                    End If
                    readerCreator = InMemory.InMemoryEventStreamReaderFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)
                    writerCreator = InMemory.InMemoryEventStreamWriterFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)

                Case SupportedEventStreamImplementations.LocalFileSettings
                    'use the settings to make a local file based reader/writer for the event streams
                    If (implementationToUse.LocalFileSettings IsNot Nothing) Then
                        settings = implementationToUse.LocalFileSettings
                    Else
                        'Not sure - can we default these??
                        Throw New NotImplementedException("No default setting exists for the local file implementation")
                    End If
                    readerCreator = Local.File.LocalFileEventStreamReaderFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)
                    writerCreator = Local.File.LocalFileEventStreamWriterFactory.GenerateCreationFunctionDelegate(Of TAggregate, TAggregateKey)

            End Select
        End If

        Return New AggregateEventStreamImplementationMap(Of TAggregate, TAggregateKey)(readerCreator, writerCreator, settings, snapshotSettingsToUse)

    End Function


End Class


Public Module AggregateEventStreamImplementationMapFactory


    ''' <summary>
    ''' Create a typed event stream implementation map for the given aggregate and key
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The type of aggregate for which to create an implementation map
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type of the unique key of the aggregate instance
    ''' </typeparam>
    ''' <param name="aggregateInstance">
    ''' An instance of the aggregate 
    ''' </param>
    ''' <param name="aggregateIdentifierKey">
    ''' An unique identifier identifying an instance of the aggregate
    ''' </param>
    ''' <param name="implementationToUse">
    ''' Settings to use to store the event stream for the aggregate
    ''' </param>
    ''' <param name="snapshotSettingsToUse">
    ''' Settings to use to store snapshots of the aggregate instance
    ''' </param>
    ''' <returns></returns>
    Public Function Create(Of TAggregate As IAggregationIdentifier, TAggregateKey)(
                                  ByVal aggregateInstance As TAggregate,
                                  ByVal aggregateIdentifierKey As TAggregateKey,
                                  ByVal implementationToUse As CQRSAzureEventSourcingImplementationSettingsElement,
                                  ByVal snapshotSettingsToUse As CQRSAzureEventSourcingProjectionSnapshotSettingsElement) As AggregateEventStreamImplementationMap(Of TAggregate, TAggregateKey)



        Return AggregateEventStreamImplementationMap(Of TAggregate, TAggregateKey).Create(implementationToUse, snapshotSettingsToUse)

    End Function


End Module