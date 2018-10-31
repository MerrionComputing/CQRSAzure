Imports System.Security.Permissions
Imports System.Runtime.Serialization

''' <summary>
''' An exception to indicate an error that occured when trying to get the underlying storage technology 
''' used to store event streams for aggregates in the system
''' </summary>
''' <remarks>
''' This can be a transient error or a permanent error such as a missing or invalid connection string
''' </remarks>
Public Class EventStreamUnderlyingStorageUnavailableException
    Inherits Exception

    Private ReadOnly m_domainname As String
    ''' <summary>
    ''' The domain in which the event stream that had the problem resides
    ''' </summary>
    Public ReadOnly Property DomainName As String
        Get
            Return m_domainname
        End Get
    End Property


    Private ReadOnly m_aggregateName As String
    ''' <summary>
    ''' The aggregate type that the event stream that had the connection problem belongs to
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property AggregateName As String
        Get
            Return m_aggregateName
        End Get
    End Property

    Private ReadOnly m_storageType As String
    ''' <summary>
    ''' The storage typ that this event stream is expected to be stored in
    ''' </summary>
    Public ReadOnly Property StorageType As String
        Get
            Return m_storageType
        End Get
    End Property

    Public Sub New(ByVal DomainNameIn As String,
               ByVal AggregateNameIn As String,
               ByVal StorageTypeIn As String,
               Optional ByVal MessageIn As String = "",
               Optional ByVal innerExceptionIn As Exception = Nothing)

        MyBase.New(MessageIn, innerExceptionIn)

        m_domainname = DomainNameIn
        m_aggregateName = AggregateNameIn
        m_storageType = StorageTypeIn

    End Sub

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        m_domainname = info.GetValue(NameOf(DomainName), GetType(String))
        m_aggregateName = info.GetValue(NameOf(AggregateName), GetType(String))
        m_storageType = info.GetValue(NameOf(StorageType), GetType(String))

    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        info.AddValue(NameOf(DomainName), DomainName)
        info.AddValue(NameOf(AggregateName), AggregateName)
        info.AddValue(NameOf(StorageType), StorageType)

        MyBase.GetObjectData(info, context)
    End Sub

End Class
