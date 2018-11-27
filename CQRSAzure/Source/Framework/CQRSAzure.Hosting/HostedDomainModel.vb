Option Explicit On
Option Strict On


Imports System
Imports CQRSAzure.Hosting
''' <summary>
''' A single domain model that is hosted by this host
''' </summary>
''' <remarks>
''' A single host may host multiple domains (in a conceptually similar manner to a single 
''' database server might host multiple databases) but equally a domain may be hosted by 
''' more than one host instance so as to allow for scaling across machines 
''' </remarks>
Public NotInheritable Class HostedDomainModel
    Implements IHostedDomainModel

    Private ReadOnly Property m_Name As String
    ''' <summary>
    ''' The unique name fo the domain model
    ''' </summary>
    Public ReadOnly Property Name As String Implements IHostedDomainModel.Name
        Get
            Return m_Name
        End Get
    End Property

    Private ReadOnly m_CommandDefinitionNamespace As String
    ''' <summary>
    ''' The namespace to load the command definitions for this model from
    ''' </summary>
    Public ReadOnly Property CommandDefinitionNamespace As String Implements IHostedDomainModel.CommandDefinitionNamespace
        Get
            Return m_CommandDefinitionNamespace
        End Get
    End Property


    Private ReadOnly Property m_CommandHandlerNamespace As String
    ''' <summary>
    ''' The namespace containing the command handlers that handle the commands for this domain
    ''' </summary>
    Public ReadOnly Property CommandHandlerNamespace As String Implements IHostedDomainModel.CommandHandlerNamespace
        Get
            Return m_CommandHandlerNamespace
        End Get
    End Property

    Private ReadOnly Property m_EventSourceingNamespace As String
    ''' <summary>
    ''' The namespace to load the model's event definitions from
    ''' </summary>
    ''' <remarks>
    ''' This is the event definitions and projections
    ''' </remarks>
    Public ReadOnly Property EventSourceingNamespace As String Implements IHostedDomainModel.EventSourceingNamespace
        Get
            Return m_EventSourceingNamespace
        End Get
    End Property

    Private ReadOnly Property m_IdentifierGroupNamespace As String
    ''' <summary>
    ''' The namespace to load the identifier group implementations from
    ''' </summary>
    Public ReadOnly Property IdentifierGroupNamespace As String Implements IHostedDomainModel.IdentifierGroupNamespace
        Get
            Return m_IdentifierGroupNamespace
        End Get
    End Property


    Private ReadOnly Property m_QueryDefinitionNamespace As String
    ''' <summary>
    ''' The namespace containing the query definitions for this domain model
    ''' </summary>
    Public ReadOnly Property QueryDefinitionNamespace As String Implements IHostedDomainModel.QueryDefinitionNamespace
        Get
            Return m_QueryDefinitionNamespace
        End Get
    End Property

    Private ReadOnly Property m_QueryHandlerNamespace As String
    ''' <summary>
    ''' The namespace containing the query handlers for this domain model
    ''' </summary>
    Public ReadOnly Property QueryHandlerNamespace As String Implements IHostedDomainModel.QueryHandlerNamespace
        Get
            Return m_QueryHandlerNamespace
        End Get
    End Property


    Protected Sub New(ByVal NameIn As String,
                      ByVal EventSourceingNamespaceIn As String,
                      ByVal IdentifierGroupNamespaceIn As String,
                      ByVal CommandDefinitionNamespaceIn As String,
                      ByVal CommandHandlerNamespaceIn As String,
                      ByVal QueryDefinitionNamespaceIn As String,
                      ByVal QueryHandlerNamespaceIn As String)

        m_Name = NameIn
        m_EventSourceingNamespace = EventSourceingNamespaceIn
        m_IdentifierGroupNamespace = IdentifierGroupNamespaceIn
        m_CommandDefinitionNamespace = CommandDefinitionNamespaceIn
        m_CommandHandlerNamespace = CommandHandlerNamespaceIn
        m_QueryDefinitionNamespace = QueryDefinitionNamespaceIn
        m_QueryHandlerNamespace = QueryHandlerNamespaceIn

    End Sub


    Public Shared Function Create(ByVal NameIn As String,
                      ByVal EventSourceingNamespaceIn As String,
                      ByVal IdentifierGroupNamespaceIn As String,
                      ByVal CommandDefinitionNamespaceIn As String,
                      ByVal CommandHandlerNamespaceIn As String,
                      ByVal QueryDefinitionNamespaceIn As String,
                      ByVal QueryHandlerNamespaceIn As String) As HostedDomainModel

        'Any mandatory validations
        If (String.IsNullOrWhiteSpace(NameIn)) Then
            Throw New ArgumentException("Domain name may not be empty", NameOf(NameIn))
        End If


        Return New HostedDomainModel(NameIn,
                                     EventSourceingNamespaceIn,
                                     IdentifierGroupNamespaceIn,
                                     CommandDefinitionNamespaceIn,
                                     CommandHandlerNamespaceIn,
                                     QueryDefinitionNamespaceIn,
                                     QueryHandlerNamespaceIn)

    End Function

    Public Shared Function Create(ByVal createFrom As IHostedDomainModel) As HostedDomainModel

        Return Create(createFrom.Name,
                      createFrom.EventSourceingNamespace,
                      createFrom.IdentifierGroupNamespace,
                      createFrom.CommandDefinitionNamespace,
                      createFrom.CommandHandlerNamespace,
                      createFrom.QueryDefinitionNamespace,
                      createFrom.QueryHandlerNamespace)

    End Function

End Class
