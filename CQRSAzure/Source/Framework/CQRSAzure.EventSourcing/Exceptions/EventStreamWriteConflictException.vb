Imports System.Security.Permissions
Imports System.Runtime.Serialization

''' <summary>
''' An event stream writer attempted to write to an event stream that was already exclusively opened by another writer
''' </summary>
''' <remarks>
''' Stream readers are inherently parallel, but only one writer may write to a stream at any given time
''' </remarks>
<Serializable()>
Public Class EventStreamWriteConflictException
    Inherits EventStreamWriteException


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
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        MyBase.GetObjectData(info, context)
    End Sub
End Class
