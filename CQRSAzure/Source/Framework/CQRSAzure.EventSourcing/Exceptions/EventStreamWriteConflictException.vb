Imports System.Security.Permissions
Imports System.Runtime.Serialization

''' <summary>
''' An event stream writer attempted to write to an event stream that was already exclusively opened by another wirter
''' </summary>
''' <remarks>
''' Stream readers are inherently parallel, but only one writer may write to a stream at any given time
''' </remarks>
Public Class EventStreamWriteConflictException
    Inherits Exception


    Public Sub New()
        MyBase.New("Attempt to take exclusive access an event stream writer that is already locked")
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
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
