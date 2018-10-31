Imports System.Runtime.Serialization
Imports Accounts.Account.eventDefinition

Public Class DepositedEventSerialiser
    Inherits EventSerializer(Of Money_Deposited)

    Public Sub New(Optional getStreamFormatter As Func(Of IFormatter) = Nothing,
           Optional saveNameValuePairsFunction As Func(Of Money_Deposited, IDictionary(Of String, Object)) = Nothing,
           Optional readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), Money_Deposited) = Nothing)

        MyBase.New(getStreamFormatter, saveNameValuePairsFunction, readNameValuePairsFunction)

    End Sub


    Public Overloads Shared Function Create(Optional ByVal saveStreamFunction As Func(Of IFormatter) = Nothing,
               Optional ByVal saveNameValuePairsFunction As Func(Of Money_Deposited, IDictionary(Of String, Object)) = Nothing,
               Optional ByVal readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), Money_Deposited) = Nothing) As DepositedEventSerialiser

        If (saveNameValuePairsFunction Is Nothing) Then
            saveNameValuePairsFunction = New Func(Of Money_Deposited, IDictionary(Of String, Object)) _
            (AddressOf DepositedEventSerialiser.SaveNameValuePairsFunction)
        End If

        If (readNameValuePairsFunction Is Nothing) Then
            readNameValuePairsFunction = New Func(Of IDictionary(Of String, Object), Money_Deposited) _
                (AddressOf DepositedEventSerialiser.readNameValuePairsFunction)
        End If

        Return New DepositedEventSerialiser(saveStreamFunction,
                                         saveNameValuePairsFunction,
                                         readNameValuePairsFunction)

    End Function


    Public Shared Function SaveNameValuePairsFunction(ByVal depositedEvent As Money_Deposited) As IDictionary(Of String, Object)

        Dim ret As New Dictionary(Of String, Object)
        ret.Add(NameOf(Money_Deposited.Amount), depositedEvent.Amount)
        ret.Add(NameOf(Money_Deposited.Date_Applied), depositedEvent.Date_Applied)
        ret.Add(NameOf(Money_Deposited.Date_Deposited), depositedEvent.Date_Deposited)
        ret.Add(NameOf(Money_Deposited.Exchange_Currency), depositedEvent.Exchange_Currency)
        ret.Add(NameOf(Money_Deposited.Exchange_Rate), depositedEvent.Exchange_Rate)
        Return ret

    End Function


    Public Shared Function readNameValuePairsFunction(ByVal properties As IDictionary(Of String, Object)) As Money_Deposited

        If (properties IsNot Nothing) Then
            Dim amount As Decimal = 0.00D
            If (properties.ContainsKey(NameOf(Money_Deposited.Amount))) Then
                amount = properties(NameOf(Money_Deposited.Amount))
            End If
            Dim dateDeposited As Date
            If (properties.ContainsKey(NameOf(Money_Deposited.Date_Deposited))) Then
                dateDeposited = properties(NameOf(Money_Deposited.Date_Deposited))
            End If
            Dim dateApplied As Date
            If (properties.ContainsKey(NameOf(Money_Deposited.Date_Applied))) Then
                dateApplied = properties(NameOf(Money_Deposited.Date_Applied))
            Else
                dateApplied = dateDeposited
            End If
            Dim exchangeCurrency As String = ""
            If (properties.ContainsKey(NameOf(Money_Deposited.Exchange_Currency))) Then
                exchangeCurrency = properties(NameOf(Money_Deposited.Exchange_Currency))
            End If
            Dim exchangeRate As Decimal = 1D
            If (properties.ContainsKey(NameOf(Money_Deposited.Exchange_Rate))) Then
                exchangeRate = properties(NameOf(Money_Deposited.Exchange_Rate))
            End If

            Return New Money_Deposited(amount,
                                       dateDeposited,
                                       dateApplied,
                                       exchangeCurrency,
                                       exchangeRate)

        End If

        'If we get to here the deserialising was unsuccessful
        Return Nothing

    End Function

End Class
