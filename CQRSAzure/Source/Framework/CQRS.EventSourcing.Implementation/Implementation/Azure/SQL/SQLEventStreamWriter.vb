Imports CQRSAzure.EventSourcing

Namespace Azure.SQL
    Public Class SQLEventStreamWriter(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregateKey)
        Inherits SQLEventStreamBase(Of TAggregate, TAggregateKey)
        Implements IEventStreamWriter(Of TAggregate, TAggregateKey)

#Region "Event stream information"

        Public ReadOnly Property Key As TAggregateKey Implements IEventStream(Of TAggregate, TAggregateKey).Key, IEventStreamInstanceProvider(Of TAggregate, TAggregateKey).Key
            Get
                Return m_key
            End Get
        End Property

        Public ReadOnly Property RecordCount As ULong Implements IEventStream(Of TAggregate, TAggregateKey).RecordCount
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property LastAddition As Date? Implements IEventStream(Of TAggregate, TAggregateKey).LastAddition
            Get
                Throw New NotImplementedException()
            End Get
        End Property

#End Region

        Public Sub AppendEvent(EventInstance As IEvent(Of TAggregate),
                               Optional ByVal ExpectedTopSequence As Long = 0) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub AppendEvents(StartingVersion As Long, Events As IEnumerable(Of IEvent(Of TAggregate))) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).AppendEvents
            Throw New NotImplementedException()
        End Sub

        Private m_context As IWriteContext
        Public Sub SetContext(writerContext As IWriteContext) Implements IEventStreamWriter(Of TAggregate, TAggregateKey).SetContext
            m_context = writerContext
        End Sub

        ''' <summary>
        ''' Create an SQL "INSERT INTO" type of command to insert the data for this event into the event detail table
        ''' </summary>
        ''' <param name="eventInstance">
        ''' The event we are looking to save to the database
        ''' </param>
        Private Function MakeEventInsertCommand(ByVal eventInstance As IEvent(Of TAggregate)) As String
            Return MakeEventInsertCommand(eventInstance.GetType())
        End Function

        ''' <summary>
        ''' Create an SQL "INSERT INTO" type of command to insert the data for this event into the event detail table
        ''' </summary>
        ''' <param name="eventInstanceType">
        ''' The type of the event we are looking to save to the database
        ''' </param>
        Private Function MakeEventInsertCommand(ByVal eventInstanceType As Type) As String

            Dim ret As New System.Text.StringBuilder

            ret.Append("INSERT INTO (")
            ret.Append(GetEventTableName(eventInstanceType))
            Dim firstRecord As Boolean = True
            For Each pi As System.Reflection.PropertyInfo In eventInstanceType.GetProperties
                If pi.CanRead Then
                    If Not firstRecord Then
                        ret.Append(",")
                    Else
                        firstRecord = False
                    End If
                    'Add the field name
                    ret.Append(MakeFieldName(pi.Name))
                End If
            Next
            ret.Append(") VALUES (")
            firstRecord = True
            For Each pi As System.Reflection.PropertyInfo In eventInstanceType.GetProperties
                If pi.CanRead Then
                    If Not firstRecord Then
                        ret.Append(",")
                    Else
                        firstRecord = False
                    End If
                    'Add a parameter to store the property for the field
                    ret.Append(MakeFieldParameter(pi.Name))
                End If
            Next
            ret.Append(")")

            Return ret.ToString()


        End Function




        ''' <param name="AggregateDomainName">
        ''' The domain in which the aggregate resides
        ''' </param>
        ''' <remarks>
        ''' The unique key which identifies the instance of the aggregate to read the event stream for
        ''' </remarks>
        ''' <param name="settings">
        ''' Configuration settings that affect how/where the event stream is written
        ''' </param>
        Private Sub New(ByVal AggregateDomainName As String,
                        ByVal AggregateKey As TAggregateKey,
                        Optional ByVal settings As ISQLSettings = Nothing)
            MyBase.New(AggregateDomainName, AggregateKey, writeAccess:=True, connectionStringName:=GetWriteConnectionStringName("", settings), settings:=settings)

        End Sub

#Region "Factory methods"

        ''' <summary>
        ''' Creates an azure blob storage based event stream reader for the given aggregate
        ''' </summary>
        ''' <param name="instance">
        ''' The instance of the aggregate for which we want to read the event stream
        ''' </param>
        ''' <returns>
        ''' </returns>
        Public Shared Function Create(ByVal instance As CQRSAzure.EventSourcing.IAggregationIdentifier(Of TAggregateKey),
                                      Optional ByVal settings As ISQLSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Return New SQLEventStreamWriter(Of TAggregate, TAggregateKey)(DomainNameAttribute.GetDomainName(instance),
                                                                            instance.GetKey(),
                                                                            settings
                                                                            )

        End Function

#End Region

    End Class

    Public Module SQLEventStreamWriterFactory

        ''' <summary>
        ''' Factory method to create a type-safe event stream writer to add events to a stream on an SQL database
        ''' </summary>
        ''' <typeparam name="TAggregate">
        ''' The type of the aggregate to which the event stream is linked
        ''' </typeparam>
        ''' <typeparam name="TAggregateKey">
        ''' The data type by which this aggregate is uniquely identified
        ''' </typeparam>
        ''' <param name="instance">
        ''' The specific instance of the aggregate to write events for
        ''' </param>
        ''' <param name="key">
        ''' The unique key by which to identify the specific instance of the aggregate to write events for
        ''' </param>
        ''' <param name="settings">
        ''' Additional settings to control how events are written to an SQL server database
        ''' </param>
        ''' <returns></returns>
        Public Function Create(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)(ByVal instance As TAggregate,
                                                    ByVal key As TAggregateKey,
                                      Optional ByVal settings As ISQLSettings = Nothing) As IEventStreamWriter(Of TAggregate, TAggregateKey)

            Return SQLEventStreamWriter(Of TAggregate, TAggregateKey).Create(instance, settings)

        End Function

        ''' <summary>
        ''' Generate a function that can be used to create an event stream writer of the given type
        ''' </summary>
        ''' <typeparam name="TAggregate">
        ''' The data type of the aggregate class
        ''' </typeparam>
        ''' <typeparam name="TAggregateKey">
        ''' The data type that provides the unique identification of an instance of the reader class
        ''' </typeparam>
        Public Function GenerateCreationFunctionDelegate(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier,
                                   TAggregateKey)() As IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey)


            'Make delegate for this module Create() function....
            Return New IAggregateImplementationMap.WriterCreationFunction(Of TAggregate, TAggregateKey)(AddressOf Create(Of TAggregate, TAggregateKey))

        End Function
    End Module
End Namespace