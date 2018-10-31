Imports System.IO
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File
    ''' <summary>
    ''' Base class for the functionality shred by readers and writers of classifier snapshots which use the local
    ''' file system to store snapshots
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
    Public MustInherit Class LocalFileClassifierSnapshotBase(Of TAggregate As IAggregationIdentifier,
                 TAggregateKey,
                 TClassifier As IClassifier)
        Inherits LocalFileClassifierSnapshotBase

        Protected ReadOnly m_key As TAggregateKey
        Protected ReadOnly m_directory As System.IO.DirectoryInfo



        Protected Sub New(ByVal AggregateDomainName As String,
                  ByVal AggregateKey As TAggregateKey,
                  ByVal classifier As TClassifier,
                  Optional ByVal writeAccess As Boolean = False,
                  Optional ByVal settings As ILocalFileSettings = Nothing)

        End Sub

    End Class


    ''' <summary>
    ''' Base class for common functionality that is independent of the aggregate of classifier type being persisted
    ''' </summary>
    Public MustInherit Class LocalFileClassifierSnapshotBase


        Public Const CLASSIFIERS_FOLDER As String = "classifiers"


        ''' <summary>
        ''' Create a directory reference for where we are going to store the event streams for this aggregate
        ''' </summary>
        ''' <param name="rootPath"></param>
        ''' <param name="aggregateClassName"></param>
        ''' <returns>
        ''' {root}\{domain}\{aggregate name}\classifiers\{name}.snapshot.{as-of-date}
        ''' </returns>
        Public Shared Function MakeHomeDirectory(rootPath As String, domainName As String, aggregateClassName As String) As DirectoryInfo

            Return New DirectoryInfo(Path.Combine(
                                     {
                                      rootPath,
                                      LocalFileEventStreamBase.MakeValidLocalFolderName(domainName),
                                      LocalFileEventStreamBase.MakeValidLocalFolderName(aggregateClassName),
                                      CLASSIFIERS_FOLDER
                                     }))

        End Function

    End Class
End Namespace