
Namespace Azure.SQL

    Public MustInherit Class SQLEventStreamBase(Of TAggregate As CQRSAzure.EventSourcing.IAggregationIdentifier, TAggregationKey)
        Inherits SQLEventStreamBase

        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String, ByVal AggregateKey As TAggregationKey)
            MyBase.New(AggregateDomainName)
        End Sub
    End Class

    Public MustInherit Class SQLEventStreamBase

        Private ReadOnly m_domainName As String

        ''' <summary>
        ''' Create a new base for a reader or writer class in the given domain
        ''' </summary>
        ''' <param name="AggregateDomainName">
        ''' The name of the domain to store/retrieve the event streams under
        ''' </param>
        Protected Sub New(ByVal AggregateDomainName As String)
            m_domainName = AggregateDomainName
        End Sub
    End Class
End Namespace