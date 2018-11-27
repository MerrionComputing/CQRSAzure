Imports System
Imports System.Collections.Generic
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Implementation

Partial Public NotInheritable Class IdentityGroupAggregateMapBuilder
    Inherits AggregateMapBuilderCore

    'Underlying aggregate map used to find the event streams
    Private ReadOnly m_underlyingMap As AggregateMapBuilder

    'A map between the aggregate type and how its backing event stream data is stored
    Private m_aggregateMap As Dictionary(Of Type, IAggregateImplementationMap) = New Dictionary(Of Type, IAggregateImplementationMap)()


    ''' <summary>
    ''' Using the implementations configurations put together an implementation map to allow 
    ''' rapid creation of reader/writer streams for the given aggregate
    ''' </summary>
    ''' <param name="aggregateType">
    ''' The type (implementing IAggregate) that events can be recorded for in an event stream
    ''' </param>
    ''' <returns></returns>
    Public Function CreateImplementationMap(aggregateType As Type) As IIdentityGroupClassifierImplementationMap

        If (m_underlyingMap IsNot Nothing) Then
            'Use the underlying map to build this map ...
            Dim aggregateMap As IAggregateImplementationMap = m_underlyingMap.CreateImplementationMap(aggregateType)
            If (aggregateMap IsNot Nothing) Then

            End If
        Else
            'Need to throw an error as will not be able to proceed
            Throw New Exceptions.ClassifierProcessorFactoryMissingException()
        End If

        Throw New NotImplementedException("Not coded yet")

    End Function

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
    ''' Create a new aggregate map builder using the "Create on demand" method by default unless told otherwise
    ''' </summary>
    Public Sub New(Optional CreationOption As MapCreationOption = MapCreationOption.OnDemand,
                   Optional Settings As CQRSAzure.EventSourcing.CQRSAzureEventSourcingConfigurationSection = Nothing)

        MyBase.New(CreationOption, Settings)

        m_underlyingMap = New AggregateMapBuilder(CreationOption, Settings)

        If (m_underlyingMap IsNot Nothing) Then
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
        End If

    End Sub

End Class

''' <summary>
''' Factory to use to create new instances of the identity group aggregate maps
''' </summary>
Public Module IdentityGroupAggregateMapBuilderFactory
    Private m_aggregateMaps As IDictionary(Of Type, IIdentityGroupClassifierImplementationMap)


    ''' <summary>
    ''' Create a new implementation map to use for a specific 
    ''' </summary>
    ''' <param name="aggregateType"></param>
    ''' <param name="implementationToUse"></param>
    ''' <param name="snapshotSettingsToUse"></param>
    ''' <returns></returns>
    Public Function CreateImplementationMap(aggregateType As Type,
                                            implementationToUse As CQRSAzureEventSourcingImplementationSettingsElement,
                                            snapshotSettingsToUse As CQRSAzureEventSourcingProjectionSnapshotSettingsElement) As IIdentityGroupClassifierImplementationMap

        If (m_aggregateMaps Is Nothing) Then
            ' Add any out-of-the-box implementation maps
            m_aggregateMaps = CreateDefaultAggregateMaps()
        End If

        If Not (m_aggregateMaps.ContainsKey(aggregateType)) Then
            'spin up a new instance
        End If

        If m_aggregateMaps.ContainsKey(aggregateType) Then
            Return m_aggregateMaps(aggregateType)
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Create the default collection of implementation maps including any "hard coded" ones
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public Function CreateDefaultAggregateMaps() As IDictionary(Of Type, IIdentityGroupClassifierImplementationMap)

        Return New Dictionary(Of Type, IIdentityGroupClassifierImplementationMap)

        'This is implemented as a separate function in case the need arises to build in specific application-wide maps in code

    End Function
End Module
