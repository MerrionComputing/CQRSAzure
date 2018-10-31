
Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands

    ''' <summary>
    ''' A command was created (or issued) by a person or system external to the domain boundary
    ''' </summary>
    ''' <remarks>
    ''' This event provides all the context about the command
    ''' </remarks>
    <Serializable()>
    Public NotInheritable Class CommandCreatedEvent
        Inherits CommandEventBase
        Implements ICommandCreatedEvent
        Implements IEvent(Of CommandAggregateIdentifier)

        Private ReadOnly m_commandUniqueIdentifier As Guid
        ''' <summary>
        ''' The unique identifier given to the command to identify it
        ''' </summary>
        Public ReadOnly Property CommandUniqueIdentifier As Guid Implements ICommandCreatedEvent.CommandUniqueIdentifier
            Get
                Return m_commandUniqueIdentifier
            End Get
        End Property

        Private ReadOnly m_CommandName As String
        Public ReadOnly Property CommandName As String Implements ICommandCreatedEvent.CommandName
            Get
                Return m_CommandName
            End Get
        End Property

        Private ReadOnly m_parametersLocation As String
        Public ReadOnly Property CommandParameters As String Implements ICommandCreatedEvent.CommandParameters

            Get
                Return m_parametersLocation
            End Get
        End Property


        Private ReadOnly m_creationDate As Date?
        Public ReadOnly Property CreationDate As Date? Implements ICommandCreatedEvent.CreationDate
            Get
                Return m_creationDate
            End Get
        End Property

        Private ReadOnly m_IdentityGroupName As String
        Public ReadOnly Property IdentityGroupName As String Implements ICommandCreatedEvent.IdentityGroupName
            Get
                Return m_IdentityGroupName
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger Implements IEvent(Of ICommandAggregateIdentifier).Version,
            IEvent(Of CommandAggregateIdentifier).Version
            Get
                Return 1
            End Get
        End Property



        Public Overrides Sub GetObjectData(info As SerializationInfo,
                                 context As StreamingContext) Implements ISerializable.GetObjectData

            If Not (info Is Nothing) Then
                MyBase.GetObjectData(info, context)
                info.AddValue(NameOf(CommandUniqueIdentifier), CommandUniqueIdentifier)
                info.AddValue(NameOf(CommandName), CommandName)
                info.AddValue(NameOf(CreationDate), CreationDate)
                info.AddValue(NameOf(CommandParameters), CommandParameters)
                info.AddValue(NameOf(IdentityGroupName), IdentityGroupName)
            End If

        End Sub

        Private Sub New(CommandUniqueIdentifierIn As Guid,
                        CommandNameIn As String,
                        CreationDateIn As Nullable(Of DateTime),
                        Optional parametersLocationIn As String = "",
                        Optional identityGroupNameIn As String = "",
                        Optional ContextIn As CommandEventContext = Nothing)

            MyBase.New(ContextIn)

            m_commandUniqueIdentifier = CommandUniqueIdentifierIn
            m_CommandName = CommandNameIn
            m_creationDate = CreationDateIn
            m_parametersLocation = parametersLocationIn
            m_IdentityGroupName = identityGroupNameIn

        End Sub

        Public Sub New(info As SerializationInfo,
                       context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_commandUniqueIdentifier = info.GetValue(NameOf(CommandUniqueIdentifier), GetType(Guid))
                m_CommandName = info.GetString(NameOf(CommandName))
                m_creationDate = info.GetDateTime(NameOf(CreationDate))
                m_parametersLocation = info.GetString(NameOf(CommandParameters))
                m_IdentityGroupName = info.GetString(NameOf(IdentityGroupName))
            End If

        End Sub

        Public Overloads Shared Function Create(CommandUniqueIdentifierIn As Guid,
                        CommandNameIn As String,
                        CreationDateIn As Nullable(Of DateTime),
                        Optional parametersLocationIn As String = "",
                        Optional identityGroupNameIn As String = "",
                        Optional ContextIn As CommandEventContext = Nothing) As CommandCreatedEvent

            If Not CreationDateIn.HasValue Then
                CreationDateIn = DateTime.UtcNow
            End If

            If (CommandUniqueIdentifierIn = Guid.Empty) Then
                CommandUniqueIdentifierIn = Guid.NewGuid()
            End If

            Return New CommandCreatedEvent(CommandUniqueIdentifierIn,
                                           CommandNameIn,
                                           CreationDateIn,
                                           parametersLocationIn,
                                           identityGroupNameIn,
                                           ContextIn)

        End Function
    End Class
End Namespace