Imports Microsoft.WindowsAzure.Storage.Table

Namespace Azure.Table
    Public MustInherit Class TableEventStreamBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
        Inherits TableEventStreamBase

        Private Const MAX_FREE_DATA_FIELDS = 248

        Private m_key As TAggregationKey

        Protected ReadOnly Property AggregateClassName As String
            Get
                Dim typeName As String = GetType(TAggregate).Name
                Return MakeValidStorageTableName(typeName)
            End Get
        End Property

        ''' <summary>
        ''' Turn an event and its data load into a row to save in an Azure table
        ''' </summary>
        ''' <param name="eventToSave">
        ''' The instance of the event and related context data to save to the table
        ''' </param>
        ''' <returns>
        ''' A dynamic table entity that represnets the event in a format that can be saved into the table
        ''' </returns>
        ''' <remarks>
        ''' Currently Azure tables has a hard limit of 252 properties so we must throw an error if the event has more 
        ''' properties than that (although this may change in future)
        ''' </remarks>
        Public Function MakeDynamicTableEntity(ByVal eventToSave As IEventContext(Of TAggregationKey)) As DynamicTableEntity

            Dim ret As New DynamicTableEntity

            ret.PartitionKey = eventToSave.AggregateKey.ToString()
            ret.RowKey = VersionToRowkey(eventToSave.Version)

            'Add the event type - currently this is the event class name
            ret.Properties.Add("EventType",
                  New EntityProperty(eventToSave.EventInstance.GetType().Name))

            'Add the context
            If (eventToSave.SequenceNumber <= 0) Then
                'Default sequence number is the current UTC date
                ret.Properties.Add("SequenceNumber",
                    New EntityProperty(DateTime.UtcNow.Ticks))
            Else
                ret.Properties.Add("SequenceNumber",
                    New EntityProperty(eventToSave.SequenceNumber))
            End If

            If (Not String.IsNullOrWhiteSpace(eventToSave.Commentary)) Then
                ret.Properties.Add("Commentary",
                     New EntityProperty(eventToSave.Commentary))
            End If



            If (Not String.IsNullOrWhiteSpace(eventToSave.Who)) Then
                ret.Properties.Add("Who", New EntityProperty(eventToSave.Who))
            End If


            If (Not String.IsNullOrWhiteSpace(eventToSave.Source)) Then
                ret.Properties.Add("Source", New EntityProperty(eventToSave.Source))
            End If


            'Now add in the different properties of the payload
            Dim propertiesCount As Integer = 0
            For Each pi As System.Reflection.PropertyInfo In eventToSave.EventInstance.GetType().GetProperties()
                If (pi.CanRead) Then
                    If (propertiesCount > MAX_FREE_DATA_FIELDS) Then
                        'Throw an error to indicate that our event exceeds the storage capabilities of an azure table
                        Throw New EventStreamWriteException(DomainName, AggregateClassName, ret.PartitionKey, eventToSave.Version, "Too many properties for event - cannot store in Azure table")
                    Else
                        ret.Properties.Add(pi.Name, MakeEntityProperty(pi, eventToSave.EventInstance))
                    End If
                    propertiesCount = propertiesCount + 1
                End If
            Next pi

            Return ret
        End Function

        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String, ByVal AggregateKey As TAggregationKey)
            MyBase.New(AggregateDomainName)

            'Get the aggregation instance key to use when creating a blob file name
            m_key = AggregateKey



        End Sub

    End Class


    Public MustInherit Class TableEventStreamBase

#Region "private members"
        Private ReadOnly m_domainName As String
#End Region

        ''' <summary>
        ''' The name of the domain model this event stream belongs to
        ''' </summary>
        ''' <remarks>
        ''' This allows multiple domains to share the same Azure storage account/area
        ''' </remarks>
        Protected ReadOnly Property DomainName As String
            Get
                Return m_domainName
            End Get
        End Property

        Protected Sub New(ByVal AggregateDomainName As String)
            m_domainName = AggregateDomainName

        End Sub

#Region "Shared methods"
        Private Const VERSION_FORMAT As String = "0000000000000000000"
        Public Shared Function VersionToRowkey(ByVal version As Long) As String

            If (version <= 0) Then
                Return VERSION_FORMAT
            Else
                Return version.ToString(VERSION_FORMAT)
            End If

        End Function


        ''' <summary>
        ''' Turn a property of an event into an entity property that can be stored in an Azure table
        ''' </summary>
        ''' <param name="pi">
        ''' The specific property in from the event
        ''' </param>
        ''' <param name="iEvent">
        ''' The parent event to which that proeprty belongs
        ''' </param>
        ''' <remarks>
        ''' For a production system we will probably replace reflection with an interface with "ToDynamicTableEntity" to be implemented by every 
        ''' event class but this reflection based system is useful for getting started and rapid prototyping
        ''' </remarks>
        Protected Shared Function MakeEntityProperty(pi As Reflection.PropertyInfo, iEvent As IEvent(Of IAggregationIdentifier)) As EntityProperty

            'If the property is one of the Azure entity property types, use that
            Dim oVal As Object = pi.GetValue(iEvent, Nothing)

            If (oVal Is Nothing) Then
                Return New EntityProperty(String.Empty)
            End If

            If TypeOf (oVal) Is Boolean Then
                Return New EntityProperty(DirectCast(oVal, Boolean))
            End If

            If TypeOf (oVal) Is Double Then
                Return New EntityProperty(DirectCast(oVal, Double))
            End If

            If TypeOf (oVal) Is Integer Then
                Return New EntityProperty(DirectCast(oVal, Integer))
            End If

            If TypeOf (oVal) Is Long Then
                Return New EntityProperty(DirectCast(oVal, Long))
            End If

            If TypeOf (oVal) Is DateTimeOffset Then
                Return New EntityProperty(DirectCast(oVal, DateTimeOffset))
            End If

            If TypeOf (oVal) Is Guid Then
                Return New EntityProperty(DirectCast(oVal, Guid))
            End If

            If TypeOf (oVal) Is Date Then
                Return New EntityProperty(DirectCast(oVal, Date))
            End If

            If TypeOf (oVal) Is Byte() Then
                Return New EntityProperty(DirectCast(oVal, Byte()))
            End If

            'Otherwise default to saving it as a string 
            Return New EntityProperty(oVal.ToString)
        End Function


        Public Const ORPHANS_TABLE As String = "Uncategorised"
        ''' <summary>
        ''' Turn the raw input into a valid Microsoft Azure table name
        ''' </summary>
        ''' <param name="rawName">
        ''' The raw class name that we want to turn into a table
        ''' </param>
        ''' <remarks>
        ''' Table names must conform to these rules:
        ''' • Table names may contain only alphanumeric characters.
        ''' • A table name may Not begin with a numeric character. 
        ''' • Table names are case-insensitive.
        ''' • Table names must be from 3 through 63 characters long.
        ''' </remarks>
        Public Shared Function MakeValidStorageTableName(ByVal rawName As String) As String

            If (String.IsNullOrWhiteSpace(rawName)) Then
                ' Don't allow empty table names - assign an orphan table for it
                Return ORPHANS_TABLE
            Else
                Dim invalidCharacters As Char() = " !,.;':@£$%^&*()-+=/\#~{}[]?<>"
                Dim cleanName As String = String.Join("", rawName.Split(invalidCharacters))
                If (cleanName.Length < 3) Then
                    cleanName += "DATA"
                End If
                If (cleanName.Length > 63) Then
                    Dim uniqueness As Integer = Math.Abs(cleanName.GetHashCode())
                    Dim uniqueid As String = uniqueness.ToString()
                    cleanName = cleanName.Substring(0, 63 - uniqueid.Length) & uniqueid
                End If
                Return cleanName
            End If
        End Function
#End Region

    End Class
End Namespace