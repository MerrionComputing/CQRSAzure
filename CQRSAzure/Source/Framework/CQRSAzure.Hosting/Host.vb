Option Strict On
Imports System
Imports System.Collections.Generic

''' <summary>
''' A host is a container responsible for one or more hosted domain.  It is responsible for all interaction 
''' between them and the outside world, and for their monitoring and maintenance
''' </summary>
Public NotInheritable Class Host
    Implements IHost
    Implements IHostControl

    '
    Private m_hostedDomains As New Dictionary(Of String, IHostedDomainModel)

    Private ReadOnly m_name As String
    ''' <summary>
    ''' Unique name by which the host is known
    ''' </summary>
    ''' <remarks>
    ''' Ideally a human readable unique name should be used to aid debugging/logging
    ''' </remarks>
    Public ReadOnly Property Name As String Implements IHost.Name
        Get
            Return m_name
        End Get
    End Property

#Region "IHostControl implementation"

    ''' <summary>
    ''' Determine if the issuer of a command is authorised to issue it
    ''' </summary>
    ''' <param name="commandName">
    ''' The name of the command being issued
    ''' </param>
    ''' <param name="commandSource">
    ''' The source of the command
    ''' </param>
    ''' <param name="commandToken">
    ''' A token to use to validate that the command came from an authorised source
    ''' </param>
    ''' <returns>
    ''' True if the issue is allowed to issue the given command
    ''' </returns>
    Public Function ValidateCommandAuthority(commandName As String, commandSource As String, commandToken As String) As Boolean Implements IHostControl.ValidateCommandAuthority
        Throw New NotImplementedException()
    End Function


    ''' <summary>
    ''' Force the host operation to shutdown completely
    ''' </summary>
    ''' <param name="shutDownMessage">
    ''' The message to be logged as to why the shutdown is being performed
    ''' </param>
    ''' <param name="forceClose">
    ''' If true, close even if there are commands in process
    ''' </param>
    ''' <param name="commandSource">
    ''' The source of the shutdown command
    ''' </param>
    ''' <param name="commandToken">
    ''' A token to use to validate that the command came from an authorised source
    ''' </param>
    Public Sub ShutDown(shutDownMessage As String, forceClose As Boolean, commandSource As String, commandToken As String) Implements IHostControl.ShutDown

        If (ValidateCommandAuthority(NameOf(ShutDown), commandSource, commandToken)) Then
            'log the shutdown message
            OnControlMessage(NameOf(ShutDown), shutDownMessage)
            'shut down the host

        End If

    End Sub


    ''' <summary>
    ''' A control message has occured and should be logged / notified
    ''' </summary>
    ''' <param name="messageType">
    ''' The type of control message
    ''' </param>
    ''' <param name="messageDetail">
    ''' (Optional) Additional message details 
    ''' </param>
    Private Sub OnControlMessage(ByVal messageType As String, ByVal messageDetail As String)
        Throw New NotImplementedException()
    End Sub

    ''' <summary>
    ''' Load the named domain and start reading work from its various queues
    ''' </summary>
    ''' <param name="domainNameToLoad">
    ''' The unique name of the domain to load into the host
    ''' </param>
    ''' <param name="commandSource">
    ''' The source of the domain load command
    ''' </param>
    ''' <param name="commandToken">
    ''' A token to use to validate that the command came from an authorised source
    ''' </param>
    ''' <param name="domainConfiguration">
    ''' (Optional) Configuration information to use when loading the domain.  If not set then the host will read the 
    ''' configuration settings from the .config file
    ''' </param>
    Public Sub LoadDomain(domainNameToLoad As String, commandSource As String, commandToken As String, Optional domainConfiguration As IHostedDomainModel = Nothing) Implements IHostControl.LoadDomain

        If (ValidateCommandAuthority(NameOf(LoadDomain), commandSource, commandToken)) Then
            If (m_hostedDomains.ContainsKey(domainNameToLoad)) Then
                Throw New DomainAlreadyLoadedException(domainNameToLoad)
            Else
                'if no configuration passed in, get it from the app config
                If (domainConfiguration Is Nothing) Then
                    domainConfiguration = LoadDomainConfigurationSettings(domainNameToLoad)
                End If
                'load the named domain...

            End If

        End If

    End Sub

    ''' <summary>
    ''' Load the hosted domain settings from the .config file for the given named domain
    ''' </summary>
    ''' <param name="domainNameToLoad">
    ''' The name of the domain we wish to load
    ''' </param>
    Private Function LoadDomainConfigurationSettings(domainNameToLoad As String) As IHostedDomainModel
        Throw New NotImplementedException()
    End Function

    Public Sub UnloadDomain(domainNameToUnload As String, commandSource As String, commandToken As String) Implements IHostControl.UnloadDomain

        If (ValidateCommandAuthority(NameOf(UnloadDomain), commandSource, commandToken)) Then
            'signal the named domain to shut down cleanly and unload 
            If (m_hostedDomains.ContainsKey(domainNameToUnload)) Then

            Else
                ' The expected domain was not actually present to unload
                Throw New DomainNotFoundException(domainNameToUnload)
            End If
        End If

    End Sub


#End Region

End Class
