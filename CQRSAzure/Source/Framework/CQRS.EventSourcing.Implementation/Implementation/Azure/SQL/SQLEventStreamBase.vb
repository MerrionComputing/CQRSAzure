Imports System
Imports System.Collections.Generic
Imports CQRSAzure.EventSourcing
Imports CQRSAzure.EventSourcing.Azure.SQL

Namespace Azure.SQL

    Public MustInherit Class SQLEventStreamBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits SQLEventStreamBase

        Protected Friend ReadOnly m_settings As ISQLSettings
        Protected Friend ReadOnly m_key As TAggregateKey

        ''' <summary>
        ''' The name of the field in which the unique identifier of the aggregate is stored
        ''' </summary>
        Protected ReadOnly Property AggregateIdentifierField As String
            Get
                If (m_settings IsNot Nothing) Then
                    Return m_settings.AggregateIdentifierField
                Else
                    Return CQRSAzureEventSourcingSQLSettingsElement.DEFAULT_AGGREGATEIDENTIFIER_FIELD
                End If
            End Get
        End Property

        ''' <summary>
        ''' The name of the field in which the event type is stored
        ''' </summary>
        Protected ReadOnly Property EventTypeField As String
            Get
                If (m_settings IsNot Nothing) Then
                    Return m_settings.EventTypeField
                Else
                    Return CQRSAzureEventSourcingSQLSettingsElement.DEFAULT_EVENTTYPE_FIELD
                End If
            End Get
        End Property

        ''' <summary>
        ''' The name of the field in which the event version is held
        ''' </summary>
        Protected ReadOnly Property EventVersionField As String
            Get
                If (m_settings IsNot Nothing) Then
                    Return m_settings.EventVersionField
                Else
                    Return CQRSAzureEventSourcingSQLSettingsElement.DEFAULT_EVENTVERSION_FIELD
                End If
            End Get
        End Property

        Protected Function GetAllStreamKeysBase(asOfDate As Date?) As IEnumerable(Of TAggregateKey)
            Throw New NotImplementedException()
        End Function

        ''' <summary>
        ''' The field name that holds the event sequence number
        ''' </summary>
        ''' <returns></returns>
        Protected ReadOnly Property SequenceField As String
            Get
                If (m_settings IsNot Nothing) Then
                    Return m_settings.SequenceField
                Else
                    Return CQRSAzureEventSourcingSQLSettingsElement.DEFAULT_SEQUENCE_FIELD
                End If
            End Get
        End Property


        Private ReadOnly m_tableName As String
        ''' <summary>
        ''' The name of the table that the event stream is held in
        ''' </summary>
        Public ReadOnly Property TableName As String
            Get
                Return m_tableName
            End Get
        End Property


        ''' <summary>
        ''' Get the table name to use for a given event instance type
        ''' </summary>
        ''' <param name="eventInstanceType">
        ''' The CLR type for the event being persisted to / read from the database
        ''' </param>
        ''' <returns>
        ''' aggregate name + event name + "Detail" e.g. [Cow Purchased Detail] , [Player Injured Detail]
        ''' </returns>
        Protected Friend Function GetEventTableName(eventInstanceType As Type) As String

            Dim aggregateTable As String = m_tableName
            If (aggregateTable.EndsWith("Events]")) Then
                aggregateTable = aggregateTable.Substring(0, aggregateTable.Length - 8)
            End If

            'add the table name for the event
            Dim eventTable As String = EventNameAttribute.GetEventName(eventInstanceType)

            Return MakeTableName(aggregateTable & " " & eventTable & " Detail")

        End Function


        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        ''' <param name="settings">
        ''' Configuration settings that affect how/where the event stream is written
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String,
                          ByVal AggregateKey As TAggregateKey,
                          Optional ByVal writeAccess As Boolean = False,
                          Optional ByVal connectionStringName As String = "",
                          Optional ByVal settings As ISQLSettings = Nothing)
            MyBase.New(AggregateDomainName, writeAccess, connectionStringName, settings)

            m_key = AggregateKey

            If (settings IsNot Nothing) Then
                m_settings = settings
            End If

            If (m_settings IsNot Nothing) Then
                If Not String.IsNullOrWhiteSpace(m_settings.EventStreamTableName) Then
                    m_tableName = MakeTableName(m_settings.EventStreamTableName)
                End If
            End If
            If (String.IsNullOrWhiteSpace(m_tableName)) Then
                'If we get here fall back on the default table name
                m_tableName = MakeTableName(GetType(TAggregate).Name & " Events")
            End If

        End Sub
    End Class

    Public MustInherit Class SQLEventStreamBase
        Inherits AzureStorageEventStreamBase

        Private ReadOnly m_domainName As String


        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        ''' <param name="settings">
        ''' Configuration settings that affect how/where the event stream is written
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String,
                          Optional ByVal writeAccess As Boolean = False,
                          Optional ByVal connectionStringName As String = "",
                          Optional ByVal settings As ISQLSettings = Nothing)

            MyBase.New(AggregateDomainName, writeAccess, connectionStringName, settings)

        End Sub


        Public Shared Function MakeTableName(nameSouce As String) As String

            Dim invalidCharacters As Char() = "!,.;':@£$%^&*()-+=/\#~{}[]?<>"
            Dim cleanName As String = String.Join("", nameSouce.Split(invalidCharacters))

            If (cleanName.Contains(" ")) Then
                Return "[" & cleanName.Trim() & "]"
            Else
                Return cleanName
            End If

        End Function

        ''' <summary>
        ''' In SQL server the rules for a field name match those for a table name
        ''' </summary>
        ''' <param name="propertyName">
        ''' The name of the .NET property
        ''' </param>
        Public Shared Function MakeFieldName(ByVal propertyName As String) As String
            Return MakeTableName(propertyName)
        End Function

        ''' <summary>
        ''' Tuen a property name into a parameter
        ''' </summary>
        ''' <param name="propertyName">The property name from the class</param>
        Public Shared Function MakeFieldParameter(ByVal propertyName As String) As String

            Dim invalidCharacters As Char() = "!,.;':@£$%^&*()-+=/\#~{}[]?<>"
            Dim cleanName As String = String.Join("", propertyName.Split(invalidCharacters))

            cleanName = cleanName.Replace(" ", "_") 'spaces are not allowed in parameter names
            Return "@" & cleanName.Trim()

        End Function

    End Class
End Namespace