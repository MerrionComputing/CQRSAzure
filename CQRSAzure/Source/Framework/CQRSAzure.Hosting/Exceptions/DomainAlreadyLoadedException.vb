Imports System.Security.Permissions
Imports System.Runtime.Serialization
Imports System

''' <summary>
''' An attempt was made to load a domain that was already loaded
''' </summary>
<Serializable()>
Public NotInheritable Class DomainAlreadyLoadedException
    Inherits Exception

    Private ReadOnly m_domainName As String
    ''' <summary>
    ''' The name of the domain we want to operate on
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property DomainName As String
        Get
            Return m_domainName
        End Get
    End Property


    Public Sub New(ByVal MissingDomainNameIn As String, Optional ByVal innerException As Exception = Nothing)
        MyBase.New("Hosted Domain Not Found", innerException)

        m_domainName = MissingDomainNameIn

    End Sub


    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        m_domainName = info.GetString(NameOf(DomainName))


    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")
        info.AddValue(NameOf(DomainName), DomainName)

        MyBase.GetObjectData(info, context)
    End Sub
End Class
