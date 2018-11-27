Imports System.Security.Permissions
Imports System.Runtime.Serialization
Imports System

''' <summary>
''' Common function shread by all projection snapshot exceptions
''' </summary>
''' <remarks>
''' This is used to identify the specific projection and snapshot in which the error occured
''' </remarks>
<Serializable()>
Public MustInherit Class ProjectionSnapshotExceptionBase
    Inherits Exception

    Private ReadOnly m_aggregateName As String
    ''' <summary>
    ''' The aggregate type that the event stream that had the problem belongs to
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property AggregateName As String
        Get
            Return m_aggregateName
        End Get
    End Property

    Private ReadOnly m_key As String
    ''' <summary>
    ''' The unique key identifying the instance of the aggregate that teh event stream that had a problem belongs to
    ''' </summary>
    Public ReadOnly Property AggregateKey As String
        Get
            Return m_key
        End Get
    End Property

    Private ReadOnly m_projection As String
    Public ReadOnly Property ProjectionName As String
        Get
            Return m_projection
        End Get
    End Property

    Public Sub New(ByVal AggregateNameIn As String,
               ByVal AggregateKeyIn As String,
               ByVal ProjectionNameIn As String,
               Optional ByVal MessageIn As String = "",
               Optional ByVal innerExceptionIn As Exception = Nothing)

        MyBase.New(MessageIn, innerExceptionIn)

        m_aggregateName = AggregateNameIn
        m_key = AggregateKeyIn
        m_projection = ProjectionNameIn

    End Sub

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        m_aggregateName = info.GetString(NameOf(AggregateName))
        m_key = info.GetString(NameOf(AggregateKey))
        m_projection = info.GetString(NameOf(ProjectionName))

    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        info.AddValue(NameOf(AggregateName), AggregateName)
        info.AddValue(NameOf(AggregateKey), AggregateKey)
        info.AddValue(NameOf(ProjectionName), ProjectionName)

        MyBase.GetObjectData(info, context)
    End Sub
End Class
