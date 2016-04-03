Imports System.Configuration

''' <summary>
''' Specific settings for using Azure Tables as a backing store for an event stream
''' </summary>
Public Class CQRSAzureEventSourcingTableSettingsElement
    Inherits CQRSAzureEventSourcingAzureStorageSettingsBase
    Implements Azure.Table.ITableSettings


End Class
