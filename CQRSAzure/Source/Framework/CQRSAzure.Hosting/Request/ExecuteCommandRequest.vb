Option Strict On
Option Explicit On

Imports CQRSAzure.Hosting
Imports CQRSAzure.CommandDefinition
Imports System

Namespace Request

    ''' <summary>
    ''' A request to execute a given command
    ''' </summary>
    Public NotInheritable Class ExecuteCommandRequest
        Inherits HostRequestBase

        'TODO: Add payload to identify the command and its parameters
        Private m_commandDefinition As ICommandDefinition


        Public Sub New(originatorIn As IHost,
                       senderIn As IHost,
                       targetIn As IHost,
                       uniqueIdentifierIn As Guid,
                       commandDefinitionIn As ICommandDefinition)

            MyBase.New(originatorIn, senderIn, targetIn, uniqueIdentifierIn, RequestCategories.ExecuteCommand)

            m_commandDefinition = commandDefinitionIn

        End Sub
    End Class
End Namespace