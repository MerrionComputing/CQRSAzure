Imports System.Configuration

''' <summary>
''' Settings for an in local memory backed event stream (typically used for testing)
''' </summary>
Public Class CQRSAzureEventSourcingInMemorySettingsElement
    Inherits ConfigurationElement
    Implements InMemory.IInMemorySettings

End Class
