Imports System.Configuration

''' <summary>
''' Configuration settings to customise how an event stream is stored in an Azure file
''' </summary>
Public Class CQRSAzureEventSourcingFileSettingsElement
    Inherits CQRSAzureEventSourcingAzureStorageSettingsBase
    Implements Azure.File.IFileStreamSettings

    Public Const DEFAULT_INITIAL_SIZE As Long = 1048576

    ''' <summary>
    ''' The initialsize to allocate for the file when creating it
    ''' </summary>
    <ConfigurationProperty(NameOf(InitialSize), IsKey:=False, IsRequired:=False, DefaultValue:=DEFAULT_INITIAL_SIZE)>
    Public Property InitialSize As Long Implements Azure.File.IFileStreamSettings.InitialSize
        Get
            Return Me(NameOf(InitialSize))
        End Get
        Set(value As Long)
            If (value <= 0) Then
                value = 1048576
            End If
            Me(NameOf(InitialSize)) = value
        End Set
    End Property

End Class
