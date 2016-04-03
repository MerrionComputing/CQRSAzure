''' <summary>
''' A ping command is a command that doesn't perform any action as such but can be used to check that all of the 
''' command handling processes are functional
''' </summary>
''' <remarks>
''' The parameters of this command are used to verify the command system is persisting and handling commands correctly
''' </remarks>
Public Class PingCommandDefinition
    Inherits CommandDefinitionBase

    ''' <summary>
    ''' The date/time that this ping was initiated
    ''' </summary>
    ''' <remarks>
    ''' This can be used to measure latency in the command handler process
    ''' </remarks>
    Public ReadOnly Property Initiated As DateTime
        Get
            Return MyBase.GetParameterValue(Of DateTime)("PingInitiated", 0)
        End Get
    End Property

    ''' <summary>
    ''' The source that sent the ping command
    ''' </summary>
    Public ReadOnly Property Source As String
        Get
            Return MyBase.GetParameterValue(Of String)("PingSource", 0)
        End Get
    End Property

    ''' <summary>
    ''' The command name - "Ping"
    ''' </summary>
    Public Overrides ReadOnly Property CommandName As String
        Get
            Return "Ping"
        End Get
    End Property

#Region "Constructor"
    Public Sub New(ByVal PingInitiated As DateTime, ByVal PingSource As String)

        MyBase.AddParameter(CommandParameter(Of DateTime).Create("PingInitiated", 0, PingInitiated))
        MyBase.AddParameter(CommandParameter(Of String).Create("PingSource", 0, PingSource))

    End Sub
#End Region

End Class
