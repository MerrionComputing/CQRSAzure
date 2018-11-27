Imports System
Imports System.Configuration
Imports CQRSAzure.EventSourcing.Azure.SQL

<Obsolete("SQL implementation not supported yet")>
Public Class CQRSAzureEventSourcingSQLSettingsElement
    Inherits CQRSAzureEventSourcingAzureStorageSettingsBase
    Implements Azure.SQL.ISQLSettings
#Region "Default values"
    Public Const DEFAULT_AGGREGATEIDENTIFIER_FIELD As String = "UniqueKey"
    Public Const DEFAULT_EVENTTYPE_FIELD As String = "Event"
    Public Const DEFAULT_EVENTVERSION_FIELD As String = "EventVersion"
    Public Const DEFAULT_SEQUENCE_FIELD As String = "Sequence"
#End Region

    ''' <summary>
    ''' What is the field name of the unique identifier of the aggregate in the table
    ''' </summary>
    <ConfigurationProperty(NameOf(AggregateIdentifierField), IsKey:=False, IsRequired:=False)>
    Public Property AggregateIdentifierField As String Implements ISQLSettings.AggregateIdentifierField
        Get
            Return Me(NameOf(AggregateIdentifierField))
        End Get
        Set(value As String)
            If (String.IsNullOrWhiteSpace(value)) Then
                Me(NameOf(AggregateIdentifierField)) = DEFAULT_AGGREGATEIDENTIFIER_FIELD
            Else
                Me(NameOf(AggregateIdentifierField)) = value
            End If
        End Set
    End Property

    <ConfigurationProperty(NameOf(EventTypeField), IsKey:=False, IsRequired:=False)>
    Public Property EventTypeField As String Implements ISQLSettings.EventTypeField
        Get
            Return Me(NameOf(EventTypeField))
        End Get
        Set(value As String)
            If (String.IsNullOrWhiteSpace(value)) Then
                Me(NameOf(EventTypeField)) = DEFAULT_EVENTTYPE_FIELD
            Else
                Me(NameOf(EventTypeField)) = value
            End If
        End Set
    End Property

    <ConfigurationProperty(NameOf(EventVersionField), IsKey:=False, IsRequired:=False)>
    Public Property EventVersionField As String Implements ISQLSettings.EventVersionField
        Get
            Return Me(NameOf(EventVersionField))
        End Get
        Set(value As String)
            If (String.IsNullOrWhiteSpace(value)) Then
                Me(NameOf(EventVersionField)) = DEFAULT_EVENTVERSION_FIELD
            Else
                Me(NameOf(EventVersionField)) = value
            End If
        End Set
    End Property

    <ConfigurationProperty(NameOf(SequenceField), IsKey:=False, IsRequired:=False)>
    Public Property SequenceField As String Implements ISQLSettings.SequenceField
        Get
            Return Me(NameOf(SequenceField))
        End Get
        Set(value As String)
            If (String.IsNullOrWhiteSpace(value)) Then
                Me(NameOf(SequenceField)) = DEFAULT_SEQUENCE_FIELD
            Else
                Me(NameOf(SequenceField)) = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' What to use as the name of the base table for the event stream
    ''' </summary>
    ''' <remarks>
    ''' if this is blank then an name made from the aggregate name + Events] is used
    ''' </remarks>
    <ConfigurationProperty(NameOf(EventStreamTableName), IsKey:=False, IsRequired:=False)>
    Public Property EventStreamTableName As String Implements ISQLSettings.EventStreamTableName
        Get
            Return Me(NameOf(EventStreamTableName))
        End Get
        Set(value As String)
            Me(NameOf(EventStreamTableName)) = value
        End Set
    End Property

End Class
