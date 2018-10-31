Imports System.Runtime.Serialization
Imports Accounts.Account.eventDefinition

''' <summary>
''' Serialiser for copying the "Opened" event to/from an event stream
''' </summary>
Public Class OpenedEventSerialiser
    Inherits EventSerializer(Of Opened)



    Public Sub New(Optional getStreamFormatter As Func(Of IFormatter) = Nothing,
                   Optional saveNameValuePairsFunction As Func(Of Opened, IDictionary(Of String, Object)) = Nothing,
                   Optional readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), Opened) = Nothing)

        MyBase.New(getStreamFormatter, saveNameValuePairsFunction, readNameValuePairsFunction)

    End Sub


    Public Overloads Shared Function Create(Optional ByVal saveStreamFunction As Func(Of IFormatter) = Nothing,
                Optional ByVal saveNameValuePairsFunction As Func(Of Opened, IDictionary(Of String, Object)) = Nothing,
                Optional ByVal readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), Opened) = Nothing) As OpenedEventSerialiser

        If (saveNameValuePairsFunction Is Nothing) Then
            saveNameValuePairsFunction = New Func(Of Opened, IDictionary(Of String, Object))(AddressOf OpenedEventSerialiser.SaveNameValuePairsFunction)
        End If

        If (readNameValuePairsFunction Is Nothing) Then
            readNameValuePairsFunction = New Func(Of IDictionary(Of String, Object), Opened)(AddressOf OpenedEventSerialiser.readNameValuePairsFunction)
        End If

        Return New OpenedEventSerialiser(saveStreamFunction,
                                         saveNameValuePairsFunction,
                                         readNameValuePairsFunction)

    End Function



    Public Shared Function SaveNameValuePairsFunction(ByVal openedEvent As Opened) As IDictionary(Of String, Object)

        Dim ret As New Dictionary(Of String, Object)
        ret.Add(NameOf(Opened.Base_Currency), openedEvent.Base_Currency)
        ret.Add(NameOf(Opened.Date_Opened), openedEvent.Date_Opened)
        Return ret

    End Function

    'Currency to default to if not specified
    Public Const DEFAULT_ACCOUNT_CURRENCY As String = "EUR"

    Public Shared Function readNameValuePairsFunction(ByVal properties As IDictionary(Of String, Object)) As Opened

        If (properties IsNot Nothing) Then
            Dim baseCurrency As String
            If (properties.ContainsKey(NameOf(Opened.Base_Currency))) Then
                baseCurrency = properties(NameOf(Opened.Base_Currency)).ToString()
            Else
                baseCurrency = DEFAULT_ACCOUNT_CURRENCY
            End If
            Dim dateOpened As Date
            If (properties.ContainsKey(NameOf(Opened.Date_Opened))) Then
                dateOpened = DirectCast(properties(NameOf(Opened.Date_Opened)), Date)
            End If
            Return New Opened(dateOpened, baseCurrency)
        End If

        'If we get to here the deserialising was unsuccessful
        Return Nothing

    End Function

End Class
