Imports System.Runtime.Serialization
Imports Accounts.Account.eventDefinition


Public Class ClosedEventSerialiser
    Inherits EventSerializer(Of Closed)


    Public Sub New(Optional getStreamFormatter As Func(Of IFormatter) = Nothing,
               Optional saveNameValuePairsFunction As Func(Of Closed, IDictionary(Of String, Object)) = Nothing,
               Optional readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), Closed) = Nothing)

        MyBase.New(getStreamFormatter, saveNameValuePairsFunction, readNameValuePairsFunction)

    End Sub


    Public Overloads Shared Function Create(Optional ByVal saveStreamFunction As Func(Of IFormatter) = Nothing,
               Optional ByVal saveNameValuePairsFunction As Func(Of Closed, IDictionary(Of String, Object)) = Nothing,
               Optional ByVal readNameValuePairsFunction As Func(Of IDictionary(Of String, Object), Closed) = Nothing) As ClosedEventSerialiser

        If (saveNameValuePairsFunction Is Nothing) Then
            saveNameValuePairsFunction = New Func(Of Closed, IDictionary(Of String, Object)) _
            (AddressOf ClosedEventSerialiser.SaveNameValuePairsFunction)
        End If

        If (readNameValuePairsFunction Is Nothing) Then
            readNameValuePairsFunction = New Func(Of IDictionary(Of String, Object), Closed) _
                (AddressOf ClosedEventSerialiser.readNameValuePairsFunction)
        End If

        Return New ClosedEventSerialiser(saveStreamFunction,
                                         saveNameValuePairsFunction,
                                         readNameValuePairsFunction)

    End Function



    Public Shared Function SaveNameValuePairsFunction(ByVal closedEvent As Closed) As IDictionary(Of String, Object)

        Dim ret As New Dictionary(Of String, Object)
        ret.Add(NameOf(Closed.Reason), closedEvent.Reason)
        ret.Add(NameOf(Closed.Date_Closed), closedEvent.Date_Closed)
        Return ret

    End Function


    Public Shared Function readNameValuePairsFunction(ByVal properties As IDictionary(Of String, Object)) As Closed

        If (properties IsNot Nothing) Then
            Dim reason As String = ""
            If (properties.ContainsKey(NameOf(Closed.Reason))) Then
                reason = properties(NameOf(Closed.Reason)).ToString()
            End If
            Dim dateClosed As Date = Date.MinValue
            If (properties.ContainsKey(NameOf(Closed.Date_Closed))) Then
                dateClosed = DirectCast(properties(NameOf(Closed.Date_Closed)), Date)
            End If
            Return New Closed(dateClosed, reason)
        End If

        'If we get to here the deserialising was unsuccessful
        Return Nothing

    End Function
End Class
