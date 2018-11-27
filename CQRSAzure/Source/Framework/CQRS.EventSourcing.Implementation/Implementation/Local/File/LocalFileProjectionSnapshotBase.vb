Imports System
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File
    ''' <summary>
    ''' Base class for storing projection state in a local file
    ''' </summary>
    ''' <remarks>
    ''' Snapshots go in e.g. :-
    ''' {root}\{domain}\{aggregate name}\snapshots\{key}.snapshot.{as-of-sequence-number}
    ''' </remarks>
    Public MustInherit Class LocalFileProjectionSnapshotBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                                            TAggregateKey,
                                                            TProjection As IProjection)
        Inherits LocalFileProjectionSnapshotBase

        Protected ReadOnly m_setings As ILocalFileSettings
        Protected ReadOnly m_key As TAggregateKey
        Protected ReadOnly m_directory As System.IO.DirectoryInfo
        Protected ReadOnly m_converter As IKeyConverter(Of TAggregateKey)

        Private m_aggregatename As String
        Protected ReadOnly Property AggregateClassName As String
            Get
                If (String.IsNullOrWhiteSpace(m_aggregatename)) Then
                    Dim typeName As String = GetType(TAggregate).Name
                    m_aggregatename = LocalFileEventStreamBase.MakeValidLocalFolderName(typeName)
                End If
                Return m_aggregatename
            End Get
        End Property

        Protected Sub New(ByVal AggregateDomainName As String,
                  ByVal AggregateKey As TAggregateKey,
                  Optional ByVal writeAccess As Boolean = False,
                  Optional ByVal settings As ILocalFileSettings = Nothing)


            m_key = AggregateKey

            m_converter = KeyConverterFactory.CreateKeyConverter(Of TAggregateKey)

            If (settings IsNot Nothing) Then
                m_setings = settings
                'set up the paths to use
                m_directory = LocalFileEventStreamBase.MakeHomeDirectory(settings.EventStreamRootFolder, LocalFileEventStreamBase.SNAPSHOTS_FOLDER, AggregateDomainName, AggregateClassName)
            Else
                m_setings = New CQRSAzureEventSourcingLocalFileSettingsElement() With {.EventStreamRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}
                'we have to use hard-coded paths (bad)
                m_directory = LocalFileEventStreamBase.MakeHomeDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), LocalFileEventStreamBase.SNAPSHOTS_FOLDER, AggregateDomainName, AggregateClassName)
            End If

        End Sub

        Public Function MakeFilename(ByVal asOfSequence As UInteger) As String

            Return MakeFilenameBase() & asOfSequence.ToString()

        End Function

        Public Function MakeFilenameBase() As String

            Return LocalFileEventStreamBase.MakeValidLocalFolderName(m_converter.ToUniqueString(m_key)) & LocalFileEventStreamBase.SNAPSHOT_SUFFIX & "."

        End Function

        Public Shared Function GetSequenceNumberFromFilename(name As String) As UInteger

            Dim sections As String() = name.Split(".")
            Dim suffix As String = sections.LastOrDefault()
            If Not String.IsNullOrWhiteSpace(suffix) Then
                Dim ret As UInteger
                If UInteger.TryParse(suffix, ret) Then
                    Return ret
                End If
            End If

            Return 0

        End Function

    End Class

    Public MustInherit Class LocalFileProjectionSnapshotBase

    End Class
End Namespace