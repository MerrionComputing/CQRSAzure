Imports System
Imports System.IO
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Implementation.Local.File
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File
    ''' <summary>
    ''' Base class for readers or writers using local file storage for snapshots of identifier group membership
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The data type of the aggregate whose members are in the group
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The data type by which these aggregate instances are uniquely identified
    ''' </typeparam>
    ''' <remarks>
    ''' Snapshots go in e.g. :-
    ''' {root}\{domain}\{aggregate name}\identifiergroups\{name}.snapshot.{as-of-date}
    ''' as a simple array
    ''' </remarks>
    Public MustInherit Class LocalFileIdentifierGroupSnapshotBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                                      TAggregateKey)
        Inherits LocalFileIdentifierGroupSnapshotBase

        Protected ReadOnly m_directory As System.IO.DirectoryInfo
        Protected ReadOnly m_groupName As String

        Private Const DATE_FORMAT_STRING As String = "000000000000000000"

        ''' <summary>
        ''' Make the filename which will be used to store a group snapshot as at the given point in time
        ''' </summary>
        ''' <param name="asOfDate"></param>
        ''' <returns></returns>
        Public Function MakeFilename(ByVal asOfDate As DateTime) As String

            Return MakeFilenameBase() & asOfDate.Ticks.ToString(DATE_FORMAT_STRING)

        End Function

        Protected Function MakeFilenameBase() As String

            Return LocalFileEventStreamBase.MakeValidLocalFolderName(m_groupName) & "." & LocalFileEventStreamBase.SNAPSHOT_SUFFIX & "."

        End Function

    End Class


    Public MustInherit Class LocalFileIdentifierGroupSnapshotBase

        Public Const IDENTIFIERGROUPS_FOLDER As String = "identifiergroups"


        ''' <summary>
        ''' Create a directory reference for where we are going to store the event streams for this aggregate
        ''' </summary>
        ''' <param name="rootPath"></param>
        ''' <param name="aggregateClassName"></param>
        ''' <returns>
        ''' {root}\{domain}\{aggregate name}\identifiergroups\{name}.snapshot.{as-of-date}
        ''' </returns>
        Public Shared Function MakeHomeDirectory(rootPath As String, domainName As String, aggregateClassName As String) As DirectoryInfo

            Return New DirectoryInfo(Path.Combine(
                                     {
                                      rootPath,
                                      LocalFileEventStreamBase.MakeValidLocalFolderName(domainName),
                                      LocalFileEventStreamBase.MakeValidLocalFolderName(aggregateClassName),
                                      IDENTIFIERGROUPS_FOLDER
                                     }))

        End Function



    End Class

End Namespace