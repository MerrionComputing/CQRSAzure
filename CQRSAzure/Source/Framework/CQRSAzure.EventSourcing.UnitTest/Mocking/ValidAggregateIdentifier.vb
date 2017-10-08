Namespace Mocking
    ''' <summary>
    ''' A valid class that does inheriyt an aggregate identifier
    ''' </summary>
    <DomainName("UnitTest")>
    Public Class ValidAggregateIdentifier
        Implements IAggregationIdentifier(Of Integer)

        Private m_key As Integer
        Public Sub SetKey(key As Integer) Implements IAggregationIdentifier(Of Integer).SetKey
            m_key = key
        End Sub

        Public Function GetAggregateIdentifier() As String Implements IAggregationIdentifier.GetAggregateIdentifier
            Return m_key.ToString()
        End Function

        Public Function GetKey() As Integer Implements IAggregationIdentifier(Of Integer).GetKey
            Return m_key
        End Function
    End Class

End Namespace