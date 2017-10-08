Imports System.Configuration
Imports CQRSAzure.EventSourcing.InMemory
Imports CQRSAzure.EventSourcing.Local.File

''' <summary>
''' Settings for an in local memory file system event stream (typically used for testing)
''' </summary>
Public Class CQRSAzureEventSourcingLocalFileSettingsElement
    Inherits ConfigurationElement
    Implements Local.File.ILocalFileSettings

    <ConfigurationProperty(NameOf(EventStreamRootFolder), IsKey:=False, IsRequired:=True)>
    Public Property EventStreamRootFolder As String Implements ILocalFileSettings.EventStreamRootFolder
        Get
            Return Me(NameOf(EventStreamRootFolder))
        End Get
        Set(value As String)
            Me(NameOf(EventStreamRootFolder)) = value
        End Set
    End Property

    <ConfigurationProperty(NameOf(SnapshotsRootFolder), IsKey:=False, IsRequired:=False)>
    Public Property SnapshotsRootFolder As String Implements ILocalFileSettings.SnapshotsRootFolder
        Get
            Return Me(NameOf(SnapshotsRootFolder))
        End Get
        Set(value As String)
            Me(NameOf(SnapshotsRootFolder)) = value
        End Set
    End Property

    <ConfigurationProperty(NameOf(UnderlyingSerialiser), IsKey:=False, IsRequired:=False, DefaultValue:=ILocalFileSettings.SerialiserType.Binary)>
    Public Property UnderlyingSerialiser As ILocalFileSettings.SerialiserType Implements ILocalFileSettings.UnderlyingSerialiser
        Get
            Return Me(NameOf(UnderlyingSerialiser))
        End Get
        Set(value As ILocalFileSettings.SerialiserType)
            Me(NameOf(UnderlyingSerialiser)) = value
        End Set
    End Property

End Class
