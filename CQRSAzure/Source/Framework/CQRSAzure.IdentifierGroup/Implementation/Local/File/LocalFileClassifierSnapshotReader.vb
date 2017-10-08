
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File

    ''' <summary>
    ''' Class to read point-in-time (or latest) snapshots of a classifier from a local file backed storage
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
    Public NotInheritable Class LocalFileClassifierSnapshotReader(Of TAggregate As IAggregationIdentifier,
                 TAggregateKey,
                 TClassifier As IClassifier)
        Inherits LocalFileClassifierSnapshotBase(Of TAggregate, TAggregateKey, TClassifier)
        Implements IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier)

        Public Function GetSnapshot(key As TAggregateKey, Optional OnOrBeforeTimestamp As Date? = Nothing) As IClassifierSnapshot(Of TAggregate, TAggregateKey, TClassifier) Implements IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier).GetSnapshot
            Throw New NotImplementedException()
        End Function


        Protected Sub New(ByVal AggregateDomainName As String,
                  ByVal AggregateKey As TAggregateKey,
                  ByVal classifier As TClassifier,
                  Optional ByVal settings As ILocalFileSettings = Nothing)
            MyBase.New(AggregateDomainName,
                       AggregateKey,
                       classifier,
                       writeAccess:=False,
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
                                      Optional ByVal settings As ILocalFileSettings = Nothing) As IClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier)

            Dim domainName As String = DomainNameAttribute.GetDomainName(instance)
            Return New LocalFileClassifierSnapshotReader(Of TAggregate, TAggregateKey, TClassifier)(domainName, instance.GetKey(), classifier, settings)

        End Function

#End Region

    End Class

    Public Module LocalFileClassifierSnapshotReaderFactory

    End Module

End Namespace
