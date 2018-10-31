Imports System.Runtime.Serialization

Namespace Queries

    ''' <summary>
    ''' The result of a prohection has been returned to the query that asked for it
    ''' </summary>
    ''' <typeparam name="TAggregateIdentifier">
    ''' The data type by which the event stream over which the projection is run is uniquely identified
    ''' </typeparam>
    <Serializable()>
    Public NotInheritable Class QueryProjectionReturnedEvent(Of TAggregateIdentifier)
        Inherits QueryEventBase
        Implements IQueryProjectionReturnedEvent(Of TAggregateIdentifier)

        Private ReadOnly m_AggregateUniqueIdentifier As TAggregateIdentifier
        Public ReadOnly Property AggregateUniqueIdentifier As TAggregateIdentifier Implements IQueryProjectionReturnedEvent(Of TAggregateIdentifier).AggregateUniqueIdentifier
            Get
                Return m_AggregateUniqueIdentifier
            End Get
        End Property

        Private ReadOnly m_AsOfDate As Date?
        Public ReadOnly Property AsOfDate As Date? Implements IQueryProjectionReturnedEvent(Of TAggregateIdentifier).AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        Private ReadOnly m_projectionName As String
        Public ReadOnly Property ProjectionName As String Implements IQueryProjectionReturnedEvent(Of TAggregateIdentifier).ProjectionName
            Get
                Return m_projectionName
            End Get
        End Property

        Private ReadOnly m_ResultsLocation As String
        Public ReadOnly Property ResultsLocation As String Implements IQueryProjectionReturnedEvent(Of TAggregateIdentifier).ResultsLocation
            Get
                Return m_ResultsLocation
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger
            Get
                Return 1
            End Get
        End Property

        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)

            If Not (info Is Nothing) Then
                info.AddValue(NameOf(AggregateUniqueIdentifier), AggregateUniqueIdentifier)
                info.AddValue(NameOf(AsOfDate), AsOfDate)
                info.AddValue(NameOf(ProjectionName), ProjectionName)
                info.AddValue(NameOf(ResultsLocation), ResultsLocation)
            End If

        End Sub

        Private Sub New(ByVal ProjectionNameIn As String,
                ByVal AsOfDateIn As Nullable(Of DateTime),
                ByVal AggregateUniqueIdentifierIn As TAggregateIdentifier,
                 ByVal ResultsLocationIn As String)

            m_projectionName = ProjectionNameIn
            m_AsOfDate = AsOfDateIn
            m_AggregateUniqueIdentifier = AggregateUniqueIdentifierIn
            m_ResultsLocation = ResultsLocationIn

        End Sub

        Public Sub New(info As SerializationInfo,
               context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_projectionName = info.GetString(NameOf(ProjectionName))
                m_AsOfDate = info.GetDateTime(NameOf(AsOfDate))
                m_AggregateUniqueIdentifier = info.GetValue(NameOf(AggregateUniqueIdentifier), GetType(TAggregateIdentifier))
                m_ResultsLocation = info.GetString(NameOf(ResultsLocation))
            End If

        End Sub

        Public Shared Function Create(ByVal ProjectionNameIn As String,
                ByVal AsOfDateIn As Nullable(Of DateTime),
                ByVal AggregateUniqueIdentifierIn As TAggregateIdentifier,
                ByVal ResultsLocationIn As String) As QueryProjectionReturnedEvent(Of TAggregateIdentifier)

            Return New QueryProjectionReturnedEvent(Of TAggregateIdentifier)(ProjectionNameIn,
                                                                             AsOfDateIn,
                                                                             AggregateUniqueIdentifierIn,
                                                                             ResultsLocationIn)


        End Function
    End Class
End Namespace
