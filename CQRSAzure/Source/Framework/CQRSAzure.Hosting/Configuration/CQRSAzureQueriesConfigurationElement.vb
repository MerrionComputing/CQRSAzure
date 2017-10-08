Imports System.Configuration

''' <summary>
''' The configuration settings that define where a domain's query definitions and handlers come from
''' </summary>
Public Class CQRSAzureQueriesConfigurationElement
    Inherits ConfigurationElement

    <ConfigurationProperty(NameOf(Definitions), IsRequired:=False)>
    Public Property Definitions As CQRSAzureDefinitionsConfigurationElement
        Get
            Return Me(NameOf(Definitions))
        End Get
        Set(value As CQRSAzureDefinitionsConfigurationElement)
            Me(NameOf(Definitions)) = value
        End Set
    End Property

    <ConfigurationProperty(NameOf(Handlers), IsRequired:=False)>
    Public Property Handlers As CQRSAzureHandlersConfigurationElement
        Get
            Return Me(NameOf(Handlers))
        End Get
        Set(value As CQRSAzureHandlersConfigurationElement)
            Me(NameOf(Handlers)) = value
        End Set
    End Property
End Class
