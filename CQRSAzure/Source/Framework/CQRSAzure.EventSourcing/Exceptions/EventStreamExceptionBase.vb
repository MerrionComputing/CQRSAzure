Imports System.Security.Permissions
Imports System.Runtime.Serialization

''' <summary>
''' Common function shread by all event stream exceptions
''' </summary>
''' <remarks>
''' This is used to identify the specific event stream in which the error occured
''' </remarks>
Public MustInherit Class EventStreamExceptionBase
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

    Private ReadOnly m_sequence As Long
    ''' <summary>
    ''' The sequence number of the event to which the problem occured
    ''' </summary>
    ''' <remarks>
    ''' This may be zero if the failure occured before the sequence number could be determined
    ''' </remarks>
    Public ReadOnly Property SequenceNumber As Long
        Get
            Return m_sequence
        End Get
    End Property

    Public Sub New(ByVal DomainNameIn As String,
                   ByVal AggregateNameIn As String,
                   ByVal AggregateKeyIn As String,
                   ByVal SequenceNumberIn As Long,
                   Optional ByVal MessageIn As String = "",
                   Optional ByVal innerExceptionIn As Exception = Nothing)

        MyBase.New(MessageIn, innerExceptionIn)

        m_domainname = DomainNameIn
        m_aggregateName = AggregateNameIn
        m_key = AggregateKeyIn
        m_sequence = SequenceNumberIn


    End Sub

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        m_domainname = info.GetValue(NameOf(DomainName), GetType(String))
        m_aggregateName = info.GetValue(NameOf(AggregateName), GetType(String))
        m_key = info.GetValue(NameOf(AggregateKey), GetType(String))
        m_sequence = info.GetValue(NameOf(SequenceNumber), GetType(Long))

    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        info.AddValue(NameOf(DomainName), DomainName)
        info.AddValue(NameOf(AggregateName), AggregateName)
        info.AddValue(NameOf(AggregateKey), AggregateKey)
        info.AddValue(NameOf(SequenceNumber), SequenceNumber)

        MyBase.GetObjectData(info, context)
    End Sub

End Class
