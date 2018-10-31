Imports System.Configuration

Public Class CQRSAzureEventSourcingConfigurationElement
    Inherits ConfigurationElement


    ''' <summary>
    ''' The full name of the assembly that contains the event sourcing definitions for this domain
    ''' </summary>
    <ConfigurationProperty(NameOf(FullName), IsRequired:=True)>
    Public Property FullName As String
        Get
            Return Me(NameOf(FullName))
        End Get
        Set(value As String)
            Me(NameOf(FullName)) = value
        End Set
    End Property

End Class
