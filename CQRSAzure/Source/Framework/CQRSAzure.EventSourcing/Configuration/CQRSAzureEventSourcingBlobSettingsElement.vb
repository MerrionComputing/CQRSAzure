Imports System.Configuration

''' <summary>
''' Specific configuration settings for the Azure Append-Only blob based streams 
''' </summary>
Public Class CQRSAzureEventSourcingBlobSettingsElement
    Inherits CQRSAzureEventSourcingAzureStorageSettingsBase
    Implements Azure.Blob.IBlobStreamSettings


End Class
