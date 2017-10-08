Imports System.Security.Permissions
Imports System.Runtime.Serialization

''' <summary>
''' An aggregate has not been mapped to a reader or writer class
''' </summary>
<Serializable()>
Public NotInheritable Class UnmappedAggregateException
    Inherits Exception

    Private ReadOnly m_AggregateTypeName As String
    ''' <summary>
    ''' The name of the aggregate type that has no mapped reader
    ''' </summary>
    Public ReadOnly Property AggregateTypeName As String
        Get
            Return m_AggregateTypeName
        End Get
    End Property


    Private ReadOnly m_aggregateKey As String
    ''' <summary>
    ''' The unique key that identifies the aggregate instance
    ''' </summary>
    ''' <returns>
    ''' Thsi allows for per-key event stream mapping, although that is a very unlikely scenario
    ''' </returns>
    Public ReadOnly Property AggregateKey As String
        Get
            Return m_aggregateKey
        End Get
    End Property


    Public Sub New(ByVal AggregateTypeIn As Object, ByVal AggregateKeyIn As Object)
        MyBase.New("Aggregate type has no backing storage configuration map")

        m_AggregateTypeName = AggregateTypeIn
        m_aggregateKey = AggregateKeyIn

    End Sub


    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        m_AggregateTypeName = info.GetValue(NameOf(AggregateTypeName), GetType(String))
        m_aggregateKey = info.GetValue(NameOf(AggregateKey), GetType(String))

    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")
        info.AddValue(NameOf(AggregateTypeName), AggregateTypeName)
        info.AddValue(NameOf(AggregateKey), AggregateKey)

        MyBase.GetObjectData(info, context)
    End Sub

End Class
