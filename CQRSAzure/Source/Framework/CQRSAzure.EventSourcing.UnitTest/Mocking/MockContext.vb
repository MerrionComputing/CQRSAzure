Imports CQRSAzure.EventSourcing

Namespace Mocking
    ''' <summary>
    ''' A context to use to set to the different writer classes for unit testing
    ''' </summary>
    Public Class MockContext
        Implements IWriteContext


        Private ReadOnly m_commentary As String
        Public ReadOnly Property Commentary As String Implements IWriteContext.Commentary
            Get
                Return m_commentary
            End Get
        End Property

        Private ReadOnly m_source As String
        Public ReadOnly Property Source As String Implements IWriteContext.Source
            Get
                Return m_source
            End Get
        End Property

        Private ReadOnly m_who As String
        Public ReadOnly Property Who As String Implements IWriteContext.Who
            Get
                Return m_who
            End Get
        End Property

        Private ReadOnly m_correlationIdentifier As String
        Public ReadOnly Property CorrelationIdentifier As String Implements IWriteContext.CorrelationIdentifier
            Get
                Return m_correlationIdentifier
            End Get
        End Property

        Public Sub New(ByVal CommentaryIn As String,
                   ByVal SourceIn As String,
                   ByVal whoIn As String,
                   ByVal correlationIdentifierIn As String)

            m_commentary = CommentaryIn
            m_source = SourceIn
            m_who = whoIn
            m_correlationIdentifier = correlationIdentifierIn

        End Sub

    End Class
End Namespace