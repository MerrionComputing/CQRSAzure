Imports System
Imports System.Runtime.Serialization

Namespace Queries

    <Serializable()>
    Public NotInheritable Class QueryCreatedEvent
        Inherits QueryEventBase
        Implements IQueryCreatedEvent

        Private ReadOnly m_queryUniqueIdentifier As Guid
        Public ReadOnly Property QueryUniqueIdentifier As Guid Implements IQueryCreatedEvent.QueryUniqueIdentifier
            Get
                Return m_queryUniqueIdentifier
            End Get
        End Property


        Private ReadOnly m_queryName As String
        Public ReadOnly Property QueryName As String Implements IQueryCreatedEvent.QueryName
            Get
                Return m_queryName
            End Get
        End Property


        Private ReadOnly m_CreationDate As Date?
        Public ReadOnly Property CreationDate As Date? Implements IQueryCreatedEvent.CreationDate
            Get
                Return m_CreationDate
            End Get
        End Property


        Private ReadOnly m_Source As String
        Public ReadOnly Property Source As String Implements IQueryCreatedEvent.Source
            Get
                Return m_Source
            End Get
        End Property


        Private ReadOnly m_Username As String
        Public ReadOnly Property Username As String Implements IQueryCreatedEvent.Username
            Get
                Return m_Username
            End Get
        End Property

        Private ReadOnly m_IdentityGroupName As String
        Public ReadOnly Property IdentityGroupName As String Implements IQueryCreatedEvent.IdentityGroupName
            Get
                Return m_IdentityGroupName
            End Get
        End Property

        Private ReadOnly m_QueryParameters As String
        Public ReadOnly Property QueryParameters As String Implements IQueryCreatedEvent.QueryParameters
            Get
                Return m_QueryParameters
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger Implements IEvent(Of IQueryAggregateIdentifier).Version
            Get
                Return 1
            End Get
        End Property



        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData

            If Not (info Is Nothing) Then
                info.AddValue(NameOf(QueryUniqueIdentifier), QueryUniqueIdentifier)
                info.AddValue(NameOf(QueryName), QueryName)
                info.AddValue(NameOf(Source), Source)
                info.AddValue(NameOf(Username), Username)
                info.AddValue(NameOf(CreationDate), CreationDate)
                info.AddValue(NameOf(IdentityGroupName), IdentityGroupName)
                info.AddValue(NameOf(QueryParameters), QueryParameters)
            End If

        End Sub

        ' TODO: Constructors
        Private Sub New(ByVal QueryIdentifierIn As Guid,
                        ByVal QueryNameIn As String,
                        ByVal CreationDateIn As Nullable(Of DateTime),
                        ByVal SourceIn As String,
                        ByVal UsernameIn As String,
                        ByVal IdentityGroupNameIn As String,
                        ByVal QueryParametersIn As String)

            m_queryUniqueIdentifier = QueryIdentifierIn
            m_queryName = QueryNameIn
            m_CreationDate = CreationDateIn
            m_Source = SourceIn
            m_Username = UsernameIn
            m_IdentityGroupName = IdentityGroupNameIn
            m_QueryParameters = QueryParametersIn

        End Sub

        Public Sub New(info As SerializationInfo,
               context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_queryUniqueIdentifier = info.GetValue(NameOf(QueryUniqueIdentifier), GetType(Guid))
                m_queryName = info.GetString(NameOf(QueryName))
                m_Source = info.GetString(NameOf(Source))
                m_Username = info.GetString(NameOf(Username))
                m_CreationDate = info.GetDateTime(NameOf(CreationDate))
                m_IdentityGroupName = info.GetString(NameOf(IdentityGroupName))
                m_QueryParameters = info.GetString(NameOf(QueryParameters))
            End If

        End Sub


        Public Shared Function Create(ByVal QueryIdentifierIn As Guid,
                        ByVal QueryNameIn As String,
                        ByVal CreationDateIn As Nullable(Of DateTime),
                        ByVal SourceIn As String,
                        ByVal UsernameIn As String,
                        ByVal IdentityGroupNameIn As String,
                        ByVal QueryParametersIn As String) As QueryCreatedEvent

            If Not CreationDateIn.HasValue Then
                CreationDateIn = DateTime.UtcNow
            End If

            If (QueryIdentifierIn = Guid.Empty) Then
                QueryIdentifierIn = Guid.NewGuid()
            End If

            Return New QueryCreatedEvent(QueryIdentifierIn,
                                         QueryNameIn,
                                         CreationDateIn,
                                         SourceIn,
                                         UsernameIn,
                                         IdentityGroupNameIn,
                                         QueryParametersIn)

        End Function
    End Class

End Namespace
