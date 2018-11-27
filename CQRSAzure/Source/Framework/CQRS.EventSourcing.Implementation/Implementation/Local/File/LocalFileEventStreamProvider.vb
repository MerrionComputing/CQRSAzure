Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Local.File

Namespace Local.File

    Public Class LocalFileEventStreamProvider(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TaggregateKey)
        Inherits LocalFileEventStreamBase
        Implements IEventStreamProvider(Of TAggregate, TaggregateKey)

        Protected ReadOnly m_setings As ILocalFileSettings
        Protected ReadOnly m_directory As System.IO.DirectoryInfo
        Protected ReadOnly m_converter As IKeyConverter(Of TaggregateKey)


        Private m_aggregatename As String
        Protected ReadOnly Property AggregateClassName As String Implements IEventStreamProvider(Of TAggregate, TaggregateKey).AggregateClassName
            Get
                If (String.IsNullOrWhiteSpace(m_aggregatename)) Then
                    Dim typeName As String = GetType(TAggregate).Name
                    m_aggregatename = MakeValidLocalFolderName(typeName)
                End If
                Return m_aggregatename
            End Get
        End Property

        Public Async Function GetAllStreamKeys(Optional asOfDate As Date? = Nothing) As Task(Of IEnumerable(Of TaggregateKey)) Implements IEventStreamProvider(Of TAggregate, TaggregateKey).GetAllStreamKeys

            Return Await Task(Of IEnumerable(Of TaggregateKey)).Run(Function()
                                                                        Dim ret As New List(Of TaggregateKey)
                                                                        If (m_directory IsNot Nothing) Then
                                                                            If (m_directory.Exists) Then
                                                                                For Each fd In m_directory.GetFiles("*" & EVENTSTREAM_SUFFIX, IO.SearchOption.TopDirectoryOnly)
                                                                                    If (Not asOfDate.HasValue) OrElse (fd.CreationTimeUtc >= asOfDate.Value) Then
                                                                                        Dim infoBlock As EventStreamDetailBlock = GetEventStreamDetailBlock(fd)
                                                                                        ret.Add(m_converter.FromString(infoBlock.KeyAsString))
                                                                                    End If
                                                                                Next
                                                                            End If
                                                                        End If

                                                                        Return ret
                                                                    End Function
                )

        End Function

        Public Sub Serialise(Of TSerialiseObject)(ByVal stream As IO.Stream, ByVal objectToSerialise As TSerialiseObject)

            If (objectToSerialise IsNot Nothing) Then
                If (m_setings Is Nothing) OrElse (m_setings.UnderlyingSerialiser = ILocalFileSettings.SerialiserType.Binary) Then
                    Dim bf As New BinaryFormatter
                    bf.Serialize(stream, objectToSerialise)
                Else
                    Dim jf As New Json.DataContractJsonSerializer(GetType(TSerialiseObject))
                    jf.WriteObject(stream, objectToSerialise)
                End If
            End If

        End Sub

        Public Function Deserialise(Of TSerialiseObject)(ByVal stream As IO.Stream) As TSerialiseObject

            If (m_setings Is Nothing) OrElse (m_setings.UnderlyingSerialiser = ILocalFileSettings.SerialiserType.Binary) Then
                Dim bf As New BinaryFormatter
                Return CType(bf.Deserialize(stream), TSerialiseObject)
            Else
                Dim jf As New Json.DataContractJsonSerializer(GetType(TSerialiseObject))
                Return CType(jf.ReadObject(stream), TSerialiseObject)
            End If

        End Function


        Protected Sub New(ByVal AggregateDomainName As String,
                          Optional ByVal settings As ILocalFileSettings = Nothing)

            m_converter = KeyConverterFactory.CreateKeyConverter(Of TaggregateKey)

            If (settings IsNot Nothing) Then
                m_setings = settings
                'set up the paths to use
                m_directory = MakeHomeDirectory(settings.EventStreamRootFolder, EVENTSTREAM_FOLDER, AggregateDomainName, AggregateClassName)
            Else
                m_setings = New CQRSAzureEventSourcingLocalFileSettingsElement() With {.EventStreamRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}
                'we have to use hard-coded paths (bad)
                m_directory = MakeHomeDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), EVENTSTREAM_FOLDER, AggregateDomainName, AggregateClassName)
            End If

            If (Not m_directory.Exists) Then
                m_directory.Create()
            End If
        End Sub

#Region "Shared functions"
        Public Shared Function GetEventStreamDetailBlock(ByVal eventStreamFile As System.IO.FileInfo,
                                                         Optional ByVal formatter As IFormatter = Nothing) As EventStreamDetailBlock

            If (eventStreamFile.Exists) Then
                Using fr = eventStreamFile.OpenRead()
                    If (formatter Is Nothing) Then
                        formatter = New BinaryFormatter()
                    End If
                    fr.Seek(0, SeekOrigin.Begin)
                    Return CType(formatter.Deserialize(fr), EventStreamDetailBlock)
                End Using
            Else
                Return Nothing
            End If

        End Function
#End Region

    End Class
End Namespace