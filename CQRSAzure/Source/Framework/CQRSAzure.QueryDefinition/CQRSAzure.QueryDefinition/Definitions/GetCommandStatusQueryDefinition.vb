Imports CQRSAzure.QueryDefinition
''' <summary>
''' A special query used to get the status of a given command that has been sent
''' for execution
''' </summary>
''' <remarks>
''' This allows for a complete separation of command and query sides while still 
''' making it possible to 
''' </remarks>
Public Class GetCommandStatusQueryDefinition
    Inherits QueryDefinitionBase(Of ICommandStatusResult)
    Implements IQueryDefinition(Of ICommandStatusResult)

    Public Overrides ReadOnly Property QueryName As String
        Get
            Return "Get Command Status"
        End Get
    End Property

    Public ReadOnly Property CommandIdentifier As Guid
        Get
            Return MyBase.GetParameterValue(Of Guid)(NameOf(CommandIdentifier), 0)
        End Get
    End Property

    Public Sub New(ByVal commandIdentifierToGetStatus As Guid)
        MyBase.New

        ' Add the mandatory parameter for the command for which we want to get a status
        MyBase.AddParameter(Of Guid)(QueryParameter(Of Guid).Create(NameOf(CommandIdentifier), 0, commandIdentifierToGetStatus))

    End Sub

End Class


''' <summary>
''' Data transfer object for reporting the status of a command that has been sent for processing
''' </summary>
Public Class CommandStatusResult
    Implements ICommandStatusResult

    Private ReadOnly m_commandInstanceIdentifier As Guid
    Public ReadOnly Property CommandInstanceIdentifier As Guid Implements ICommandStatusResult.CommandInstanceIdentifier
        Get
            Return m_commandInstanceIdentifier
        End Get
    End Property


    Private ReadOnly m_commandName As String
    Public ReadOnly Property CommandName As String Implements ICommandStatusResult.CommandName
        Get
            Return m_commandName
        End Get
    End Property

    Private ReadOnly m_commandExecutationStatus As ICommandStatusResult.CommandExecutionStatus
    Public ReadOnly Property ExecutationStatus As ICommandStatusResult.CommandExecutionStatus Implements ICommandStatusResult.ExecutationStatus
        Get
            Return m_commandExecutationStatus
        End Get
    End Property

    Private ReadOnly m_failureCause As ICommandStatusResult.CommandFailureCause
    Public ReadOnly Property FailureCause As ICommandStatusResult.CommandFailureCause Implements ICommandStatusResult.FailureCause
        Get
            Return m_failureCause
        End Get
    End Property

    Private ReadOnly m_statusMessage As String
    Public ReadOnly Property StatusMessage As String Implements ICommandStatusResult.StatusMessage
        Get
            Return m_statusMessage
        End Get
    End Property
End Class