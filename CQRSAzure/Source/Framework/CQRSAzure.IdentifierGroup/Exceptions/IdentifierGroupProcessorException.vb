Imports System
Imports System.Runtime.Serialization
Imports System.Security.Permissions

Namespace Exceptions

    ''' <summary>
    ''' There was a problem starting the classifier process factory
    ''' </summary>
    Public Class IdentifierGroupProcessorFactoryMissingException
        Inherits IdentifierGroupProcessorException

        Public Const EXCEPTION_MISSING_FACTORY As String = "Missing identifier group processor factory"


        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)

        End Sub

        Public Sub New()
            MyBase.New(EXCEPTION_MISSING_FACTORY)
        End Sub

    End Class

    ''' <summary>
    ''' Base class for all identifier group related exceptions
    ''' </summary>
    Public MustInherit Class IdentifierGroupProcessorException
        Inherits Exception


        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)

        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)

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
End Namespace