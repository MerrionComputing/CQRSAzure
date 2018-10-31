Imports System.Configuration
Imports CQRSAzure.EventSourcing.InMemory

''' <summary>
''' Settings for an in local memory backed event stream (typically used for testing)
''' </summary>
Public Class CQRSAzureEventSourcingInMemorySettingsElement
    Inherits ConfigurationElement
    Implements InMemory.IInMemorySettings

    <ConfigurationProperty(NameOf(DebugMessages), IsKey:=False, IsRequired:=False, DefaultValue:=True)>
    Public Property DebugMessages As Boolean Implements IInMemorySettings.DebugMessages
        Get
            Return Me(NameOf(DebugMessages))
        End Get
        Set(value As Boolean)
            Me(NameOf(DebugMessages)) = value
        End Set
    End Property

    Public Shared Function DefaultSettings() As CQRSAzureEventSourcingInMemorySettingsElement
        Return New CQRSAzureEventSourcingInMemorySettingsElement() With {.DebugMessages = True}
    End Function

End Class
