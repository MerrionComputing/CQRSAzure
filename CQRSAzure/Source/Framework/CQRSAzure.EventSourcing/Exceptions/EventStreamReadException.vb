Imports System.Security.Permissions
Imports System.Runtime.Serialization
Imports System

''' <summary>
''' An error occured while reading from an event stream
''' </summary>
Public Class EventStreamReadException
    Inherits EventStreamExceptionBase

    'TODO - Add any additional properties for read specific exceptions

    Public Sub New(ByVal DomainNameIn As String,
               ByVal AggregateNameIn As String,
               ByVal AggregateKeyIn As String,
               ByVal SequenceNumberIn As Long,
               Optional ByVal MessageIn As String = "",
               Optional ByVal innerExceptionIn As Exception = Nothing)

        MyBase.New(DomainNameIn,
                   AggregateNameIn,
                   AggregateKeyIn,
                   SequenceNumberIn,
                   MessageIn,
                   innerExceptionIn)

    End Sub
End Class
