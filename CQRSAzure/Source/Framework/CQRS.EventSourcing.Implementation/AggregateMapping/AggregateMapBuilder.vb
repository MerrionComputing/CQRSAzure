Imports System.Configuration
Imports System.Linq
Imports CQRSAzure.EventSourcing

''' <summary>
''' A class to build a mapping between aggregate classes and the classes that provide access to the underlying event stream data for
''' those aggregate classes
''' </summary>
''' <remarks>
''' This is conceptually similar to the way the DbContextBuilder class works in entity framework
''' </remarks>
Partial Public NotInheritable Class AggregateMapBuilder
    Inherits AggregateMapBuilderCore

    Private ReadOnly m_implementationMaps As CQRSAzureEventSourcingAggregateMapElementCollection
    Private ReadOnly m_implementations As CQRSAzureEventSourcingImplementationSettingsElementCollection
    Private ReadOnly m_snapshotImplementations As CQRSAzureEventSourcingProjectionSnapshotSettingsElementCollection

    'A map between the aggregate type and how its backing event stream data is stored
    Private m_aggregateMap As Dictionary(Of Type, IAggregateImplementationMap) = New Dictionary(Of Type, IAggregateImplementationMap)()



#Region "Outputs methods"

    ''' <summary>
    ''' Get the appropriate event stream reader to use to process the event stream for the given aggregate
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The aggregate type for which we want to get the event stream
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type which uniquely identifies an instance of that aggregate (ideally this should be a simple type - string, int, guid etc.)
    ''' </typeparam>
    ''' <param name="aggregateInstance">
    ''' The specific instance of the aggregate we are getting the event stream for
    ''' </param>
    ''' <param name="key">
    ''' The key which uniquely identifies the aggregate instance whose event stream we are getting
    ''' </param>
    ''' <remarks>
    ''' Although this functions is available directly, in practice it will be called when creating a class such as a Projection Processor or Classifier Processor
    ''' that needs read access to an event stream
    ''' </remarks>
    Public Function GetEventStreamReader(Of TAggregate As IAggregationIdentifier, TAggregateKey)(ByVal aggregateInstance As TAggregate, ByVal key As TAggregateKey) As IEventStreamReader(Of TAggregate, TAggregateKey)

        Throw New NotImplementedException("Not yet coded")

    End Function

    ''' <summary>
    ''' Get the appropriate event stream writer to use to process the event stream for the given aggregate
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The aggregate type for which we want to get the event stream
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type which uniquely identifies an instance of that aggregate (ideally this should be a simple type - string, int, guid etc.)
    ''' </typeparam>
    ''' <param name="aggregateInstance">
    ''' The specific instance of the aggregate we are getting the event stream to write to for
    ''' </param>
    ''' <param name="key">
    ''' The key which uniquely identifies the aggregate instance whose event stream we are getting
    ''' </param>
    Public Function GetEventStreamWriter(Of TAggregate As IAggregationIdentifier, TAggregateKey)(ByVal aggregateInstance As TAggregate, ByVal key As TAggregateKey) As IEventStreamWriter(Of TAggregate, TAggregateKey)

        Throw New NotImplementedException("Not yet coded")

    End Function

    ''' <summary>
    ''' Get a projection processor set up to run a projection over the given aggregate's event stream
    ''' </summary>
    ''' <typeparam name="TAggregate">
    ''' The aggregate type for which we want to get the event stream
    ''' </typeparam>
    ''' <typeparam name="TAggregateKey">
    ''' The type which uniquely identifies an instance of that aggregate (ideally this should be a simple type - string, int, guid etc.)
    ''' </typeparam>
    ''' <param name="aggregateInstance">
    ''' The specific instance of the aggregate we are getting the event stream to write to for
    ''' </param>
    ''' <param name="key">
    ''' The key which uniquely identifies the aggregate instance whose event stream we are getting
    ''' </param>
    Public Function GetProjectionProcessor(Of TAggregate As IAggregationIdentifier, TAggregateKey)(ByVal aggregateInstance As TAggregate, ByVal key As TAggregateKey) As ProjectionProcessor(Of TAggregate, TAggregateKey)

        Throw New NotImplementedException("Not yet coded")

    End Function

#End Region


    ''' <summary>
    ''' Add an entry to the internal map such that the class can be used to find an appropriate persistence 
    ''' class instance to read or write to its back end storage mechanism
    ''' </summary>
    ''' <param name="AggregateType">
    ''' The type (implementing IAggregate) that events can be recorded for in an event stream
    ''' </param>
    Private Sub MapAggregateClassByType(ByVal AggregateType As Type)

#Region "Tracing"
        System.Diagnostics.Trace.TraceInformation("Mapping " & AggregateType.Name)
#End Region

        If Not m_aggregateMap.ContainsKey(AggregateType) Then
            m_aggregateMap.Add(AggregateType, CreateImplementationMap(AggregateType))
        End If

    End Sub

    ''' <summary>
    ''' Using the implementations configurations put together an implementation map to allow 
    ''' rapid creation of reader/writer streams for the given aggregate
    ''' </summary>
    ''' <param name="aggregateType">
    ''' The type (implementing IAggregate) that events can be recorded for in an event stream
    ''' </param>
    ''' <returns></returns>
    Public Function CreateImplementationMap(aggregateType As Type) As IAggregateImplementationMap

        'Get any domain name tagged to this aggregate type
        Dim domainQualifiedName As String = DomainNameAttribute.GetAggregateDomainQualifiedName(aggregateType)
        Dim implementationName As String = ""
        Dim snapshotSettingsName As String = ""

        If (m_implementationMaps IsNot Nothing) Then
            'get the implementation map to use..
            For Each map As CQRSAzureEventSourcingAggregateMapElement In m_implementationMaps
                If (map.AggregateDomainQualifiedName = domainQualifiedName) Then
                    implementationName = map.ImplementationName
                    snapshotSettingsName = map.SnapshotSettingsName
                    Exit For
                End If
            Next
        End If


        'If no matching implementation name was found we have to fall back on the default...
        If (String.IsNullOrWhiteSpace(implementationName)) Then
            implementationName = CQRSAzureEventSourcingAggregateMapElement.DEFAULT_MAPPING_NAME
        End If

        Dim implementationToUse As CQRSAzureEventSourcingImplementationSettingsElement = Nothing
        If (m_implementations IsNot Nothing) Then
            'do we have a matching implementation for this class in [AggregateDomainQualifiedName]
            For Each implementation As CQRSAzureEventSourcingImplementationSettingsElement In m_implementations
                If (implementation.Name = implementationName) Then
                    implementationToUse = implementation
                    Exit For
                End If
            Next
        End If

        If (implementationToUse Is Nothing) Then
            'Fall back on the system-wide default implementation
            implementationToUse = CQRSAzureEventSourcingImplementationSettingsElement.DefaultImplementation
        End If

        If (String.IsNullOrWhiteSpace(snapshotSettingsName)) Then
            'fall back on implementation to provide the snapshot settings
            snapshotSettingsName = implementationName
        End If

        Dim snapshotSettingsToUse As CQRSAzureEventSourcingProjectionSnapshotSettingsElement = Nothing
        If (m_snapshotImplementations IsNot Nothing) Then
            'Look for the matching snapshot implementation
            For Each snapshot As CQRSAzureEventSourcingProjectionSnapshotSettingsElement In m_snapshotImplementations
                If (snapshot.Name = snapshotSettingsName) Then
                    snapshotSettingsToUse = snapshot
                    Exit For
                End If
            Next
        End If

        If (snapshotSettingsToUse Is Nothing) Then
            'create a snapshot setting from the implementation setting
            snapshotSettingsToUse = CQRSAzureEventSourcingProjectionSnapshotSettingsElement.CreateSnapshotSettingsFromImplementationSettings(implementationToUse)
        End If

        Return AggregateMapBuilderFactory.CreateImplementationMap(aggregateType, implementationToUse, snapshotSettingsToUse)

    End Function



    ''' <summary>
    ''' Create a new aggregate map builder using the "Create on demand" method by default unless told otherwise
    ''' </summary>
    Public Sub New(Optional CreationOption As MapCreationOption = MapCreationOption.OnDemand,
                   Optional Settings As CQRSAzure.EventSourcing.CQRSAzureEventSourcingConfigurationSection = Nothing)

        MyBase.New(CreationOption, Settings)

        If (Settings IsNot Nothing) Then
            If (Settings.ImplementationMaps IsNot Nothing) Then
                m_implementationMaps = Settings.ImplementationMaps
            End If
            If (Settings.SnapshotSettings IsNot Nothing) Then
                m_snapshotImplementations = Settings.SnapshotSettings
            End If
            If (Settings.Implementations IsNot Nothing) Then
                m_implementations = Settings.Implementations
            End If
        Else
            'Try and create map settings from the application configuration
            Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            If (config IsNot Nothing) Then

                Dim sectionName As String = DEFAULT_CONFIG_SECTION_NAME
                Dim objSection As Object = config.GetSection(sectionName)
                Dim sectionConfig As CQRSAzure.EventSourcing.CQRSAzureEventSourcingConfigurationSection = Nothing

                If (objSection IsNot Nothing) Then
                    sectionConfig = CTypeDynamic(Of CQRSAzure.EventSourcing.CQRSAzureEventSourcingConfigurationSection)(objSection)
                Else
                    'Get the first section of type : type="CQRSAzure.EventSourcing.CQRSAzureEventSourcingConfigurationSection,CQRSAzure.EventSourcing"
                    For Each objSection In config.Sections
                        If (objSection.GetType() Is GetType(CQRSAzureEventSourcingConfigurationSection)) Then
                            sectionConfig = CTypeDynamic(Of CQRSAzureEventSourcingConfigurationSection)(objSection)
                            If (sectionConfig IsNot Nothing) Then
                                Exit For
                            End If
                        End If
                    Next
                End If

                If (sectionConfig IsNot Nothing) Then
                    If (sectionConfig.ImplementationMaps IsNot Nothing) Then
                        m_implementationMaps = sectionConfig.ImplementationMaps
                    End If
                    If (sectionConfig.Implementations IsNot Nothing) Then
                        m_implementations = sectionConfig.Implementations
                    End If
                    If (sectionConfig.SnapshotSettings IsNot Nothing) Then
                        m_snapshotImplementations = sectionConfig.SnapshotSettings
                    End If
                End If
            End If
        End If

        If (MyBase.MapCreation = MapCreationOption.UpFront) Then
            'Create all the maps for classes known to the application
            For Each domainAssembly In AppDomain.CurrentDomain.GetAssemblies()
                For Each aggType As Type In domainAssembly.GetTypes()
                    If (aggType.IsClass) Then
                        If (aggType.GetInterface(NameOf(IAggregationIdentifier))) IsNot Nothing Then
                            MapAggregateClassByType(aggType)
                        End If
                    End If
                Next
            Next
        End If

    End Sub

End Class


''' <summary>
''' Common functionality to be shared between an AggregateMapBuilder and an IdentityGroupAggregateMapBuilder
''' </summary>
''' <remarks>
''' We need to do this to preserve some DRY while still keeping the code used to instantiate identity groups
''' physically separate to the code used in the common CQRS/ES functionality (so that end users can opt not to use the
''' concept of identity groups if they choose)
''' </remarks>
Public Class AggregateMapBuilderCore

    Public Const DEFAULT_CONFIG_SECTION_NAME As String = "CQRSAzureEventSourcingConfiguration"

    ''' <summary>
    ''' How should new aggregate-persistence map records be created?
    ''' </summary>
    Public Enum MapCreationOption
        ''' <summary>
        ''' Create a new map instance the first time an aggregate class storage is requested
        ''' </summary>
        OnDemand = 0
        ''' <summary>
        ''' Create all the map instances up-front when the application is started
        ''' </summary>
        UpFront = 1
    End Enum

    Private ReadOnly m_mapCreationOption As MapCreationOption
    ''' <summary>
    ''' How/when should new aggregate-persistence map records be created?
    ''' </summary>
    Public ReadOnly Property MapCreation As MapCreationOption
        Get
            Return m_mapCreationOption
        End Get
    End Property


    Private ReadOnly m_mapSettings As CQRSAzure.EventSourcing.CQRSAzureEventSourcingAggregateMapElementCollection


    Public ReadOnly Property MapSettings As CQRSAzureEventSourcingAggregateMapElementCollection
        Get
            Return m_mapSettings
        End Get
    End Property


    Protected Friend Sub New(Optional CreationOption As MapCreationOption = MapCreationOption.OnDemand,
                   Optional Settings As CQRSAzure.EventSourcing.CQRSAzureEventSourcingConfigurationSection = Nothing)

        m_mapCreationOption = CreationOption

        If (Settings IsNot Nothing) Then
            m_mapSettings = Settings.ImplementationMaps
        Else
            'Try and create map settings from the application configuration
            Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            If (config IsNot Nothing) Then

                Dim sectionName As String = DEFAULT_CONFIG_SECTION_NAME
                Dim objSection As Object = config.GetSection(sectionName)
                Dim sectionConfig As CQRSAzure.EventSourcing.CQRSAzureEventSourcingConfigurationSection = Nothing

                If (objSection IsNot Nothing) Then
                    sectionConfig = CTypeDynamic(Of CQRSAzure.EventSourcing.CQRSAzureEventSourcingConfigurationSection)(objSection)
                Else
                    'Get the first section of type : type="CQRSAzure.EventSourcing.CQRSAzureEventSourcingConfigurationSection,CQRSAzure.EventSourcing"
                    For Each objSection In config.Sections
                        If (objSection.GetType() Is GetType(CQRSAzureEventSourcingConfigurationSection)) Then
                            sectionConfig = CTypeDynamic(Of CQRSAzureEventSourcingConfigurationSection)(objSection)
                            If (sectionConfig IsNot Nothing) Then
                                Exit For
                            End If
                        End If
                    Next
                End If

                If (sectionConfig IsNot Nothing) Then
                    If (sectionConfig.ImplementationMaps IsNot Nothing) Then
                        m_mapSettings = sectionConfig.ImplementationMaps
                    End If
                End If
            End If
        End If

    End Sub


End Class

Public Module AggregateMapBuilderFactory


    Private m_aggregateMaps As IDictionary(Of Type, IAggregateImplementationMap)

    ''' <summary>
    ''' Create an implementation map for the given aggregate type using the settings passed in
    ''' </summary>
    ''' <param name="aggregateType">
    ''' The type (implementing IAggregate) that events can be recorded for in an event stream
    ''' </param>
    ''' <param name="implementationToUse">
    ''' The settings to use for the implementation of the event stream
    ''' </param>
    ''' <param name="snapshotSettingsToUse">
    ''' The settings to use for the snapshot functionality 
    ''' </param>
    ''' <returns></returns>
    Public Function CreateImplementationMap(aggregateType As Type,
                                                   implementationToUse As CQRSAzureEventSourcingImplementationSettingsElement,
                                                   snapshotSettingsToUse As CQRSAzureEventSourcingProjectionSnapshotSettingsElement) As IAggregateImplementationMap


        If (m_aggregateMaps Is Nothing) Then
            ' Add any out-of-the-box implementation maps
            m_aggregateMaps = CreateDefaultAggregateMaps()
        End If


        If Not (m_aggregateMaps.ContainsKey(aggregateType)) Then
            Dim ret As IAggregateImplementationMap = CreateNewImplementationMap(aggregateType, AggregateKeyDataTypeAttribute.GetAggregateKeyDataType(aggregateType), implementationToUse, snapshotSettingsToUse)
            If (ret IsNot Nothing) Then
                m_aggregateMaps.Add(aggregateType, ret)
            End If
        End If

        If m_aggregateMaps.ContainsKey(aggregateType) Then
            Return m_aggregateMaps(aggregateType)
        Else
            Return Nothing
        End If


    End Function

    ''' <summary>
    ''' Using the implementation and snapshot settings passed in,create a new IAggregateImplementationMap(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    ''' for the aggregate type passed in
    ''' </summary>
    ''' <param name="aggregateType">
    ''' The aggregate type to create an event stream for
    ''' </param>
    ''' <param name="aggregateKeyType">
    ''' The data type that provides the unique identifier of the aggregate
    ''' </param>
    ''' <param name="implementationToUse">
    ''' The backing technology setting to use to hold the event stream for that aggregate
    ''' </param>
    ''' <param name="snapshotSettingsToUse">
    ''' The backing technology to use to hold snapshots of the event stream for the aggregate 
    ''' </param>
    ''' <returns>
    ''' An aggregate implementation map that can be used to spin up connectiosn to the underlyiong data for instances of the 
    ''' given aggregate
    ''' </returns>
    Public Function CreateNewImplementationMap(aggregateType As Type,
                                                aggregateKeyType As Type,
                                                implementationToUse As CQRSAzureEventSourcingImplementationSettingsElement,
                                                Optional snapshotSettingsToUse As CQRSAzureEventSourcingProjectionSnapshotSettingsElement = Nothing) As IAggregateImplementationMap


        'Create a new AggregateEventStreamImplementationMap(Of TAggregate As IAggregationIdentifier, TAggregateKey)
        Dim mapType As Type = GetType(AggregateEventStreamImplementationMap(Of ,))
        If (mapType IsNot Nothing) Then
            Dim returnMapType As Type = mapType.MakeGenericType({aggregateType, aggregateKeyType})
            If (returnMapType IsNot Nothing) Then
                'Invoke the Create function that takes...
                '  Create(ByVal implementationToUse As CQRSAzureEventSourcingImplementationSettingsElement,
                '         ByVal snapshotSettingsToUse As CQRSAzureEventSourcingProjectionSnapshotSettingsElement)
                Dim miCreate As System.Reflection.MethodInfo = returnMapType.GetMethod("Create", {GetType(CQRSAzureEventSourcingImplementationSettingsElement), GetType(CQRSAzureEventSourcingProjectionSnapshotSettingsElement)})
                If (miCreate IsNot Nothing) Then
                    Return miCreate.Invoke(Nothing, {implementationToUse, snapshotSettingsToUse})
                End If
            End If
        End If

        Return Nothing

    End Function



    Public Function CreateDefaultAggregateMaps() As IDictionary(Of Type, IAggregateImplementationMap)

        Return New Dictionary(Of Type, IAggregateImplementationMap)

        'This is implemented as a separate function in case the need arises to build in specific application-wide maps in code

    End Function


End Module