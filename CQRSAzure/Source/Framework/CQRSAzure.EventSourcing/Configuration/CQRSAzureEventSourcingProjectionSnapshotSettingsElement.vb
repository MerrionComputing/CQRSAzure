Imports System.Configuration
Imports CQRSAzure.EventSourcing

''' <summary>
''' A single setting to use for persisting projection snapshots against a backing storage technology
''' </summary>
Public Class CQRSAzureEventSourcingProjectionSnapshotSettingsElement
    Inherits ConfigurationElement

    ''' <summary>
    ''' The unique name of this specific snapshot setting
    ''' </summary>
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
    ''' The specific type of the underlying implementation 
    ''' </summary>
    <ConfigurationProperty(NameOf(ImplementationType),
                           DefaultValue:=ImplementationMap.SupportedEventStreamImplementations.Default,
                           IsRequired:=True)>
    Public Property ImplementationType As ImplementationMap.SupportedEventStreamImplementations
        Get
            Return Me(NameOf(ImplementationType))
        End Get
        Set(value As ImplementationMap.SupportedEventStreamImplementations)
            Me(NameOf(ImplementationType)) = value
        End Set
    End Property

    ''' <summary>
    ''' The settings specific to an Azure Blob backed event stream
    ''' </summary>
    <ConfigurationProperty(NameOf(BlobSettings),
                           DefaultValue:=Nothing,
                           IsRequired:=False)>
    Public Property BlobSettings As CQRSAzureEventSourcingBlobSettingsElement
        Get
            Return Me(NameOf(BlobSettings))
        End Get
        Set(value As CQRSAzureEventSourcingBlobSettingsElement)
            Me(NameOf(BlobSettings)) = value
        End Set
    End Property

    ''' <summary>
    ''' The settings specific to an Azure Blob backed event stream
    ''' </summary>
    <ConfigurationProperty(NameOf(FileSettings),
                           DefaultValue:=Nothing,
                           IsRequired:=False)>
    Public Property FileSettings As CQRSAzureEventSourcingFileSettingsElement
        Get
            Return Me(NameOf(FileSettings))
        End Get
        Set(value As CQRSAzureEventSourcingFileSettingsElement)
            Me(NameOf(FileSettings)) = value
        End Set
    End Property

    ''' <summary>
    ''' The settings specific to an Azure SQL Server backed event stream
    ''' </summary>
    <ConfigurationProperty(NameOf(SQLSettings),
                           DefaultValue:=Nothing,
                           IsRequired:=False)>
    Public Property SQLSettings As CQRSAzureEventSourcingSQLSettingsElement
        Get
            Return Me(NameOf(SQLSettings))
        End Get
        Set(value As CQRSAzureEventSourcingSQLSettingsElement)
            Me(NameOf(SQLSettings)) = value
        End Set
    End Property

    ''' <summary>
    ''' The settings specific to an Azure Table backed event stream
    ''' </summary>
    <ConfigurationProperty(NameOf(TableSettings),
                           DefaultValue:=Nothing,
                           IsRequired:=False)>
    Public Property TableSettings As CQRSAzureEventSourcingTableSettingsElement
        Get
            Return Me(NameOf(TableSettings))
        End Get
        Set(value As CQRSAzureEventSourcingTableSettingsElement)
            Me(NameOf(TableSettings)) = value
        End Set
    End Property

    ''' <summary>
    ''' The settings specific to an in-memory backed event stream
    ''' </summary>
    <ConfigurationProperty(NameOf(InMemorySettings),
                           DefaultValue:=Nothing,
                           IsRequired:=False)>
    Public Property InMemorySettings As CQRSAzureEventSourcingInMemorySettingsElement
        Get
            Return Me(NameOf(InMemorySettings))
        End Get
        Set(value As CQRSAzureEventSourcingInMemorySettingsElement)
            Me(NameOf(InMemorySettings)) = value
        End Set
    End Property

    ''' <summary>
    ''' The settings specific to a local file system backed event stream
    ''' </summary>
    <ConfigurationProperty(NameOf(LocalFileSettings),
                           DefaultValue:=Nothing,
                           IsRequired:=False)>
    Public Property LocalFileSettings As CQRSAzureEventSourcingLocalFileSettingsElement
        Get
            Return Me(NameOf(LocalFileSettings))
        End Get
        Set(value As CQRSAzureEventSourcingLocalFileSettingsElement)
            Me(NameOf(LocalFileSettings)) = value
        End Set
    End Property

    Public Shared Function CreateSnapshotSettingsFromImplementationSettings(implementationToUse As CQRSAzureEventSourcingImplementationSettingsElement) As CQRSAzureEventSourcingProjectionSnapshotSettingsElement

        Dim ret As CQRSAzureEventSourcingProjectionSnapshotSettingsElement = New CQRSAzureEventSourcingProjectionSnapshotSettingsElement() With
            {.Name = implementationToUse.Name,
            .ImplementationType = implementationToUse.ImplementationType
            }

        Select Case implementationToUse.ImplementationType
            Case SupportedEventStreamImplementations.AzureBlob
                ret.BlobSettings = implementationToUse.BlobSettings
            Case SupportedEventStreamImplementations.AzureFile
                ret.FileSettings = implementationToUse.FileSettings
            Case SupportedEventStreamImplementations.AzureSQL
                ret.SQLSettings = implementationToUse.SQLSettings
            Case SupportedEventStreamImplementations.AzureTable
                ret.TableSettings = implementationToUse.TableSettings
            Case SupportedEventStreamImplementations.InMemory
                ret.InMemorySettings = implementationToUse.InMemorySettings
            Case SupportedEventStreamImplementations.LocalFileSettings
                ret.LocalFileSettings = implementationToUse.LocalFileSettings
        End Select

        Return ret

    End Function
End Class
