Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File
    ''' <summary>
    ''' Class to save point-in-time snapshots of a classifier in a local file backed storage
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate that owns the event stream being used for the classification
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which an instance of this aggregate class is uniquely identified
    ''' </typeparam>
    ''' <typeparam name="TClassifier">
    ''' The data type of the classifier function being run over the event stream
    ''' </typeparam>
    Public NotInheritable Class LocalFileClassifierSnapshotWriter(Of TAggregate As IAggregationIdentifier,
                 TAggregateKey,
                 TClassifier As IClassifier)
        Inherits LocalFileClassifierSnapshotBase(Of TAggregate, TAggregateKey, TClassifier)
        Implements IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier)

        Public Sub SaveSnapshot(key As TAggregateKey, snapshotToSave As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier)) _
            Implements IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier).SaveSnapshot

            Throw New NotImplementedException()

        End Sub

        Protected Sub New(ByVal AggregateDomainName As String,
          ByVal AggregateKey As TAggregateKey,
          ByVal classifier As TClassifier,
          Optional ByVal settings As ILocalFileSettings = Nothing)
            MyBase.New(AggregateDomainName,
                       AggregateKey,
                       classifier,
                       writeAccess:=True,
                       settings:=settings)


        End Sub

#Region "Factory Methods"

        ''' <summary>
        ''' Creates an local file storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      ByVal classifier As TClassifier,
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            Return New LocalFileClassifierSnapshotWriter(Of TAggregate, TAggregateKey, TClassifier)(domainName, instance.GetKey(), classifier, settings)

        End Function

#End Region

    End Class

    Public Module LocalFileClassifierSnapshotWriterFactory

    End Module
End Namespace