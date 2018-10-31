Imports System.Runtime.Serialization
Imports Accounts.Account.eventDefinition

Public Class WithdrawnEventSerialiser
    Inherits EventSerializer(Of Money_Withdrawn)

    Public Sub New(Optional getStreamFormatter As Func(Of IFormatter) = Nothing,
       Optional saveNameValuePairsFunction As Func(Of Money_Withdrawn, IDictionary(Of String, Object)) = Nothing,
       Optional readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), Money_Withdrawn) = Nothing)

        MyBase.New(getStreamFormatter, saveNameValuePairsFunction, readNameValuePairsFunction)

    End Sub


    Public Shared Function SaveNameValuePairsFunction(ByVal withdrawnEvent As Money_Withdrawn) As IDictionary(Of String, Object)

        Dim ret As New Dictionary(Of String, Object)
        ret.Add(NameOf(Money_Withdrawn.Amount), withdrawnEvent.Amount)
        ret.Add(NameOf(Money_Withdrawn.Method), withdrawnEvent.Method)
        ret.Add(NameOf(Money_Withdrawn.Notes), withdrawnEvent.Notes)
        ret.Add(NameOf(Money_Withdrawn.Withdrawn_Date), withdrawnEvent.Withdrawn_Date)
        Return ret

    End Function


    Public Shared Function readNameValuePairsFunction(ByVal properties As IDictionary(Of String, Object)) As Money_Withdrawn

        If (properties IsNot Nothing) Then
            Dim amount As Decimal = 0.00D
            If (properties.ContainsKey(NameOf(Money_Withdrawn.Amount))) Then
                amount = properties(NameOf(Money_Withdrawn.Amount))
            End If
            Dim dateWithdrawn As Date
            If (properties.ContainsKey(NameOf(Money_Withdrawn.Withdrawn_Date))) Then
                dateWithdrawn = properties(NameOf(Money_Withdrawn.Withdrawn_Date))
            End If
            Dim method As String = ""
            If (properties.ContainsKey(NameOf(Money_Withdrawn.Method))) Then
                method = properties(NameOf(Money_Withdrawn.Method))
            End If
            Dim notes As String = ""
            If (properties.ContainsKey(NameOf(Money_Withdrawn.Notes))) Then
                method = properties(NameOf(Money_Withdrawn.Notes))
            End If

            Return New Money_Withdrawn(amount,
                                       dateWithdrawn,
                                       method,
                                       notes)

        End If

        'If we get to here the deserialising was unsuccessful
        Return Nothing

    End Function

End Class
