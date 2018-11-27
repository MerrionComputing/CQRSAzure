Imports System.Configuration
Imports System.Linq
Imports CQRSAzure.EventSourcing.Azure.Table

''' <summary>
''' Specific settings for using Azure Tables as a backing store for an event stream
''' </summary>
Public Class CQRSAzureEventSourcingTableSettingsElement
    Inherits CQRSAzureEventSourcingAzureStorageSettingsBase
    Implements Azure.Table.ITableSettings

#Region "Default values"
    Public Const DEFAULT_SEQUENCENUMBERFORMAT As String = "0000000000" 'max 4294967295
    Public Const DEFAULT_ROWNUMBERFORMAT As String = "0000000000"
#End Region

    ''' <summary>
    ''' If set, use this format string to turn the sequence number into a row key of the record
    ''' </summary>
    ''' <remarks>
    ''' Default is "0000000000"
    ''' </remarks>
    <ConfigurationProperty(NameOf(SequenceNumberFormat), IsKey:=False, IsRequired:=False, DefaultValue:=DEFAULT_SEQUENCENUMBERFORMAT)>
    Public Property SequenceNumberFormat As String Implements ITableSettings.SequenceNumberFormat
        Get
            Return Me(NameOf(SequenceNumberFormat))
        End Get
        Set(value As String)
            If (String.IsNullOrWhiteSpace(value)) Then
                value = DEFAULT_SEQUENCENUMBERFORMAT
            Else
                'Check the format is a valid number format
                If Not IsValidCustomNumberFormat(value) Then
                    If Not ValidStandardNumericFormats.Contains(value) Then
                        value = DEFAULT_SEQUENCENUMBERFORMAT
                    End If
                End If
            End If
            Me(NameOf(SequenceNumberFormat)) = value
        End Set
    End Property

    ''' <summary>
    ''' If set, include the domain name in the table name
    ''' </summary>
    ''' <returns></returns>
    <ConfigurationProperty(NameOf(IncludeDomainInTableName), IsKey:=False, IsRequired:=False, DefaultValue:=False)>
    Public Property IncludeDomainInTableName As Boolean Implements ITableSettings.IncludeDomainInTableName
        Get
            Return Me(NameOf(IncludeDomainInTableName))
        End Get
        Set(value As Boolean)
            Me(NameOf(IncludeDomainInTableName)) = value
        End Set
    End Property

    ''' <summary>
    ''' The subset of the .NET standard numeric formats we can use for formatting the sequence number
    ''' </summary>
    ''' <remarks>
    ''' Commas, currency and any regional variation must be avoided
    ''' </remarks>
    Public Shared ReadOnly Property ValidStandardNumericFormats As String()
        Get
            Return {"D", "d", "X", "x"}
        End Get
    End Property



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="customNumberFormat">
    ''' The custom number format to use - e.g. "0000"
    ''' </param>
    ''' <remarks>
    ''' The sequence number is a signed Integer - from 0 to 4,294,967,295
    ''' </remarks>
    Public Shared Function IsValidCustomNumberFormat(ByVal customNumberFormat As String) As Boolean

        If String.IsNullOrWhiteSpace(customNumberFormat) Then
            Return False
        Else
            'does it have anything except zeros in it
            If Not String.IsNullOrWhiteSpace(customNumberFormat.Replace("0", "")) Then
                Return False
            End If
            'is it longer than 4294967295
            If customNumberFormat.Length > 10 Then
                Return False
            End If
            Return True
        End If

    End Function


    ''' <summary>
    ''' If set, use this format string to turn the row number into a row key of the snapshot record
    ''' </summary>
    ''' <remarks>
    ''' Default is "0000000000"
    ''' </remarks>
    <ConfigurationProperty(NameOf(SequenceNumberFormat), IsKey:=False, IsRequired:=False, DefaultValue:=DEFAULT_ROWNUMBERFORMAT)>
    Public Property RowNumberFormat As String Implements ITableSettings.RowNumberFormat
        Get
            Return Me(NameOf(RowNumberFormat))
        End Get
        Set(value As String)
            If (String.IsNullOrWhiteSpace(value)) Then
                value = DEFAULT_ROWNUMBERFORMAT
            Else
                'Check the format is a valid number format
                If Not IsValidCustomNumberFormat(value) Then
                    If Not ValidStandardNumericFormats.Contains(value) Then
                        value = DEFAULT_ROWNUMBERFORMAT
                    End If
                End If
            End If
            Me(NameOf(RowNumberFormat)) = value
        End Set
    End Property

End Class
