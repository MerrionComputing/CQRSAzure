Imports System
Imports CQRSAzure.EventSourcing

Namespace Commands
    ''' <summary>
    ''' Aggregate identifier specifically for commands that are passed into the domain
    ''' </summary>
    Public Class CommandAggregateIdentifier
        Implements ICommandAggregateIdentifier

        Private m_commandKey As Guid

        Public Sub SetKey(key As Guid) Implements IAggregationIdentifier(Of Guid).SetKey
            m_commandKey = key
        End Sub

        ''' <summary>
        ''' Turn the GUID key to a string for any storage systems that require that
        ''' </summary>
        Public Function GetAggregateIdentifier() As String Implements IAggregationIdentifier.GetAggregateIdentifier
            Return m_commandKey.ToString("P")
        End Function

        Public Function GetKey() As Guid Implements IAggregationIdentifier(Of Guid).GetKey
            Return m_commandKey
        End Function

        Private Sub New(ByVal commandIdentifier As Guid)
            m_commandKey = commandIdentifier
        End Sub

        ''' <summary>
        ''' Empty constructor for use in serialisation
        ''' </summary>
        Public Sub New()

        End Sub

        Public Shared Function Create(ByVal commandidentifier As Guid) As CommandAggregateIdentifier
            Return New CommandAggregateIdentifier(commandidentifier)
        End Function

    End Class
End Namespace