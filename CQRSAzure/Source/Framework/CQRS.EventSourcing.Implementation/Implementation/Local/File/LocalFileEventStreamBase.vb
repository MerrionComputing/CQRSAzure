Imports System
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File
    Public MustInherit Class LocalFileEventStreamBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits LocalFileEventStreamProvider(Of TAggregate, TAggregateKey)

        Protected ReadOnly m_key As TAggregateKey
        Protected ReadOnly m_file As System.IO.FileInfo

        Protected m_eventStreamDetailBlock As New EventStreamDetailBlock()
        Protected m_streamstart As Long = 0



        ''' <summary>
        ''' The filename in which the event stream is written
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Filename As String
            Get
                If (m_file IsNot Nothing) Then
                    Return m_file.FullName
                Else
                    Return "File not created"
                End If
            End Get
        End Property

        Public Function GetKeyFromFilename(name As String) As TAggregateKey

            If (name.EndsWith(EVENTSTREAM_SUFFIX)) Then
                name = name.TrimEnd(EVENTSTREAM_SUFFIX)
            End If

            Return m_converter.FromString(name)

        End Function

        Public Function KeyToFilename(ByVal key As TAggregateKey) As String
            Return MakeValidLocalFolderName(m_converter.ToUniqueString(key)) & EVENTSTREAM_SUFFIX
        End Function


        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        ''' <param name="AggregateKey">
        ''' The key by which the instance of the aggregate whose event stream is accessed is identified
        ''' </param>
        ''' <param name="writeAccess">
        ''' Should this class be able to write to the file 
        ''' </param>
        ''' <param name="settings">
        ''' Configuration settings that affect where (and how) the files are stored
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String,
                          ByVal AggregateKey As TAggregateKey,
                          Optional ByVal writeAccess As Boolean = False,
                          Optional ByVal settings As ILocalFileSettings = Nothing)

            MyBase.New(AggregateDomainName, settings)

            m_key = AggregateKey

            'initialise the file to use to read/write
            m_file = New FileInfo(Path.Combine(m_directory.FullName, EventStreamFilename))
            m_file.Refresh()
            If (m_file.Exists) Then
                LoadEventStreamDetailBlock()
            End If

            m_eventStreamDetailBlock.KeyAsString = m_converter.ToUniqueString(m_key)

        End Sub

        ''' <summary>
        ''' Load the EventStreamDetailBlock from the start of the file
        ''' </summary>
        Protected Sub LoadEventStreamDetailBlock()

            Using fr = m_file.OpenRead()
                LoadEventStreamDetailBlock(fr)
            End Using

        End Sub

        ''' <summary>
        ''' Load the EventStreamDetailBlock from the start of the file
        ''' </summary>
        Protected Sub LoadEventStreamDetailBlock(ByVal stream As System.IO.Stream)



            stream.Seek(0, SeekOrigin.Begin)
            m_eventStreamDetailBlock = MyBase.Deserialise(Of EventStreamDetailBlock)(stream)
            m_streamstart = stream.Position

        End Sub

        Protected ReadOnly Property EventStreamFilename As String
            Get
                Return KeyToFilename(m_key)
            End Get
        End Property






    End Class

    Public MustInherit Class LocalFileEventStreamBase

        Public Const ORPHANS_FOLDER As String = "uncategorised"
        Public Const EVENTSTREAM_FOLDER As String = "eventstreams"
        Public Const SNAPSHOTS_FOLDER As String = "snapshots"

        Public Const EVENTSTREAM_SUFFIX As String = ".eventstream"
        Public Const SNAPSHOT_SUFFIX As String = ".snapshot"
        Public Const KEYS_SUFFIX As String = ".keys"

        ''' <summary>
        ''' Create a directory reference for where we are going to store the event streams for this aggregate
        ''' </summary>
        ''' <param name="rootPath"></param>
        ''' <param name="aggregateClassName"></param>
        ''' <returns>
        ''' {root}\{domain}\{aggregate name}\eventstreams\
        ''' </returns>
        Public Shared Function MakeHomeDirectory(rootPath As String, folderType As String, domainName As String, aggregateClassName As String) As DirectoryInfo

            Return New DirectoryInfo(Path.Combine(
                                     {
                                      rootPath,
                                      MakeValidLocalFolderName(domainName),
                                      MakeValidLocalFolderName(aggregateClassName),
                                      folderType
                                     }))

        End Function

        ''' <summary>
        ''' Turn a name into a valid folder name for azure file storage
        ''' </summary>
        ''' <param name="rawName">
        ''' The name of the thing we want to turn into a file storage folder name
        ''' </param>
        ''' <returns>
        ''' A folder name that can be used to locate this object type's event streams
        ''' </returns>
        ''' <remarks>
        ''' Container names must start With a letter Or number, And can contain only letters, numbers, And the dash (-) character.
        ''' Every dash (-) character must be immediately preceded And followed by a letter Or number; consecutive dashes are Not permitted in container names.
        ''' All letters in a container name must be lowercase.
        ''' Container names must be from 3 through 63 characters long.
        ''' </remarks>
        Public Shared Function MakeValidLocalFolderName(ByVal rawName As String) As String

            If (String.IsNullOrWhiteSpace(rawName)) Then
                ' Don't allow empty folder names - assign an orphan folder for it
                Return ORPHANS_FOLDER
            Else
                Dim invalidCharacters As Char() = " _!,;':@£$%^&*()+=/\#~{}[]?<>"
                Dim cleanName As String = String.Join("-", rawName.Split(invalidCharacters))
                If (cleanName.StartsWith("-")) Then
                    cleanName = cleanName.TrimStart("-")
                End If
                If (cleanName.EndsWith("-")) Then
                    cleanName = cleanName.TrimEnd("-")
                End If
                If (cleanName.Length < 1) Then
                    cleanName += "-a"
                End If
                If (cleanName.Length > 63) Then
                    Dim uniqueness As Integer = cleanName.GetHashCode()
                    Dim uniqueid As String = uniqueness.ToString()
                    cleanName = cleanName.Substring(0, 63 - uniqueid.Length) & uniqueid
                End If
                cleanName = cleanName.Replace("--", "-")
                Return cleanName.ToLower()

            End If

        End Function

    End Class

    ''' <summary>
    ''' Connection between a file and the data it contains
    ''' </summary>
    <Serializable()>
    <DataContract()>
    Public Class EventStreamDetailBlock


        ''' <summary>
        ''' The key of the aggregate as convterted to a string
        ''' </summary>
        <DataMember(Name:="Key", Order:=0, IsRequired:=True)>
        Public Property KeyAsString As String

        ''' <summary>
        ''' The current sequence number of the event stream
        ''' </summary>
        <DataMember(Name:="Sequence", Order:=1, IsRequired:=True)>
        Public Property SequenceNumber As UInteger

        ''' <summary>
        ''' The date/time the event stream was created
        ''' </summary>
        ''' <remarks>
        ''' We could use the file properties but this would preclude event streams being copied between test
        ''' and production machines
        ''' </remarks>
        <DataMember(Name:="DateCreated", Order:=2, IsRequired:=True)>
        Public Property DateCreated As DateTime

        ''' <summary>
        ''' The number of records in the stream
        ''' </summary>
        <DataMember(Name:="RecordCount", Order:=1, IsRequired:=True)>
        Public Property RecordCount As UInteger


    End Class

End Namespace