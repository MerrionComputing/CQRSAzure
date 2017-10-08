Imports System.Configuration

''' <summary>
''' Settings for an individual CQRS Azure domain hosted on this host
''' </summary>
Public Class CQRSAzureHostedDomainElement
    Inherits ConfigurationElement

    ''' <summary>
    ''' The unique name of the domain
    ''' </summary>
    ''' <remarks>
    ''' This must be unqiue within the CQRSAzureHostedDomainElementCollection
    ''' </remarks>
    <ConfigurationProperty(NameOf(Name), IsKey:=True, IsRequired:=True)>
    Public Property Name As String
        Get
            Return Me(NameOf(Name))
        End Get
        Set(value As String)
            Me(NameOf(Name)) = value
        End Set
    End Property

    ''' <summary>
    ''' The description of the domain for use in logging messages etc.
    ''' </summary>
    <ConfigurationProperty(NameOf(Description), IsKey:=False, IsRequired:=False)>
    Public Property Description As String
        Get
            Return Me(NameOf(Description))
        End Get
        Set(value As String)
            Me(NameOf(Description)) = value
        End Set
    End Property

    <ConfigurationProperty(NameOf(EventSourcing), IsRequired:=False)>
    Public Property EventSourcing As CQRSAzureEventSourcingConfigurationElement
        Get
            Return Me(NameOf(EventSourcing))
        End Get
        Set(value As CQRSAzureEventSourcingConfigurationElement)
            Me(NameOf(EventSourcing)) = value
        End Set
    End Property

    <ConfigurationProperty(NameOf(IdentifierGroups), IsRequired:=False)>
    Public Property IdentifierGroups As CQRSAzureIdentifierGroupsConfigurationElement
        Get
            Return Me(NameOf(IdentifierGroups))
        End Get
        Set(value As CQRSAzureIdentifierGroupsConfigurationElement)
            Me(NameOf(IdentifierGroups)) = value
        End Set
    End Property

    <ConfigurationProperty(NameOf(Commands), IsRequired:=False)>
    Public Property Commands As CQRSAzureCommandsConfigurationElement
        Get
            Return Me(NameOf(Commands))
        End Get
        Set(value As CQRSAzureCommandsConfigurationElement)
            Me(NameOf(Commands)) = value
        End Set
    End Property

    <ConfigurationProperty(NameOf(Queries), IsRequired:=False)>
    Public Property Queries As CQRSAzureQueriesConfigurationElement
        Get
            Return Me(NameOf(Queries))
        End Get
        Set(value As CQRSAzureQueriesConfigurationElement)
            Me(NameOf(Queries)) = value
        End Set
    End Property

End Class
