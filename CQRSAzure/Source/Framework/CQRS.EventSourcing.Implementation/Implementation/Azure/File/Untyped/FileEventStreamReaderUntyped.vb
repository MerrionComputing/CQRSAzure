
Imports System
Imports System.Collections.Generic
Imports CQRSAzure.EventSourcing.Azure.File
Imports Microsoft.WindowsAzure.Storage.File

Namespace Azure.File

    ''' <summary>
    ''' Class to read events from an untyped event stream implemented as a windows Azure file
    ''' </summary>
    Public Class FileEventStreamReaderUntyped
        Inherits FileEventStreamBaseUntyped
        Implements IEventStreamReaderUntyped

        Public ReadOnly Property Key As String Implements IEventStreamReaderUntyped.Key
            Get
                Return MyBase.InstanceKey
            End Get
        End Property

        Public Async Function GetEvents() As Task(Of IEnumerable(Of IEvent)) Implements IEventStreamReaderUntyped.GetEvents

            Return Await GetEvents(0, Nothing)

        End Function

        Public Async Function GetEvents(Optional StartingSequenceNumber As UInteger = 0,
                                  Optional effectiveDateTime As Date? = Nothing) As Task(Of IEnumerable(Of IEvent)) Implements IEventStreamReaderUntyped.GetEvents

            If (MyBase.File IsNot Nothing) Then
                Dim ret As New List(Of IEvent)

                Using fs As IO.Stream = MyBase.File.OpenReadAsync().Result
                    Dim currentLength As Long = Await GetSequence()
                    fs.Seek(0, IO.SeekOrigin.Begin)
                    If Not (fs.Position >= fs.Length) Then
                        Dim record As FileBlockJSonWrappedEvent = FileBlockJSonWrappedEvent.ReadFromStream(fs)
                        If (record IsNot Nothing) Then
                            ret.Add(record.EventInstance)
                        End If
                    End If
                End Using
                Return ret
            Else
                Throw New EventStreamReadException(DomainName,
                                                   MyBase.AggregateTypeName,
                                                   Key,
                                                   0,
                                                   "Unable to read events - Azure file not initialised")
            End If
        End Function

        Public Async Function GetEventsWithContext(Optional StartingSequenceNumber As UInteger = 0,
                                             Optional effectiveDateTime As Date? = Nothing) As Task(Of IEnumerable(Of IEventContext)) Implements IEventStreamReaderUntyped.GetEventsWithContext

            Return Await Task.FromException(Of IEnumerable(Of IEventContext))(New NotImplementedException())

        End Function

        Public Sub New(ByVal identifier As IEventStreamUntypedIdentity,
                          Optional ByVal connectionStringName As String = "",
                          Optional ByVal settings As IFileStreamSettings = Nothing)

            MyBase.New(identifier, False, connectionStringName, settings)

        End Sub


    End Class
End Namespace