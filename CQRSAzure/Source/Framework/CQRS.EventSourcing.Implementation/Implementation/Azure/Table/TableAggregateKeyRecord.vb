Imports CQRSAzure.EventSourcing
Imports Microsoft.WindowsAzure.Storage
Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table

    ''' <summary>
    ''' A single row in the domain aggregates table
    ''' </summary>
    ''' <remarks>
    ''' This is to allow identity group operations withour requiring a table scan on the event stream tables
    ''' </remarks>
    Public NotInheritable Class TableAggregateKeyRecord
        Implements ITableEntity


        Private m_ETag As String
        Public Property ETag As String Implements ITableEntity.ETag
            Get
                Return m_ETag
            End Get
            Set(value As String)
                m_ETag = value
            End Set
        End Property

        Private m_aggregateClassName As String
        Public Property AggregateClassName As String Implements ITableEntity.PartitionKey
            Get
                Return m_aggregateClassName
            End Get
            Set(value As String)
                m_aggregateClassName = value
            End Set
        End Property

        Private m_aggregateKey As String
        Public Property RowKey As String Implements ITableEntity.RowKey
            Get
                Return m_aggregateKey
            End Get
            Set(value As String)
                m_aggregateKey = value
            End Set
        End Property

        Private m_timestamp As DateTimeOffset
        Public Property Timestamp As DateTimeOffset Implements ITableEntity.Timestamp
            Get
                Return m_timestamp
            End Get
            Set(value As DateTimeOffset)
                m_timestamp = value
            End Set
        End Property

        Private m_createdDateTime As DateTime
        ''' <summary>
        ''' The date/time this aggregate key instance was first created
        ''' </summary>
        ''' <remarks>
        ''' This can be used to derive the "All" group membership as at a point in time
        ''' </remarks>
        Public Property CreatedDateTime As DateTime
            Get
                Return m_createdDateTime
            End Get
            Set(value As DateTime)
                m_createdDateTime = value
            End Set
        End Property

        Private m_lastSequence As Long
        Public Property LastSequence As Long
            Get
                Return m_lastSequence
            End Get
            Set(value As Long)
                If (value >= m_lastSequence) Then
                    m_lastSequence = value
                Else
                    Throw New EventStreamWriteConflictException("", m_aggregateClassName, m_aggregateKey, m_lastSequence, "Invalid sequence number set")
                End If
            End Set
        End Property

        'any other properties you want to record per aggregate key can go here....

        Public Sub ReadEntity(properties As IDictionary(Of String, EntityProperty), operationContext As OperationContext) Implements ITableEntity.ReadEntity


            If properties.ContainsKey(NameOf(CreatedDateTime)) Then
                m_createdDateTime = properties(NameOf(CreatedDateTime)).DateTime
            End If
            If properties.ContainsKey(NameOf(LastSequence)) Then
                m_lastSequence = properties(NameOf(LastSequence)).Int64Value
            End If

        End Sub

        Public Function WriteEntity(operationContext As OperationContext) As IDictionary(Of String, EntityProperty) Implements ITableEntity.WriteEntity

            Dim properties As New Dictionary(Of String, EntityProperty)
            properties.Add(NameOf(CreatedDateTime), New EntityProperty(m_createdDateTime))
            properties.Add(NameOf(LastSequence), New EntityProperty(m_lastSequence))
            Return properties

        End Function

        Public Sub New(ByVal AggregateClassNameIn As String, ByVal AggregateInstanceKeyIn As String)
            m_aggregateClassName = AggregateClassNameIn
            m_aggregateKey = AggregateInstanceKeyIn
            m_createdDateTime = DateTime.UtcNow
        End Sub

        ''' <summary>
        ''' Parameterless constructor for serialisation
        ''' </summary>
        Public Sub New()

        End Sub
    End Class
End Namespace