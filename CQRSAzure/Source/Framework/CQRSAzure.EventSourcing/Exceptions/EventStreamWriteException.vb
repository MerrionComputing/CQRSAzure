Imports System.Security.Permissions
Imports System.Runtime.Serialization

''' <summary>
''' An error prevented an event from being written to an event stream
''' </summary>
Public Class EventStreamWriteException
    Inherits EventStreamExceptionBase

    'TODO - Add any additional properties for write specific exceptions

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


    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        MyBase.GetObjectData(info, context)
    End Sub

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

    End Sub

End Class
