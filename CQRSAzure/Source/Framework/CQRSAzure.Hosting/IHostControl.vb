''' <summary>
''' Interface for control messages that a host should react to
''' </summary>
''' <remarks>
''' Each command must be accompanied by a source and a token - to allow validation that it comes from a known,
''' authorised command sender
''' </remarks>
Public Interface IHostControl

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
    Function ValidateCommandAuthority(ByVal commandName As String,
                                      ByVal commandSource As String,
                                      ByVal commandToken As String) As Boolean

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
    Sub ShutDown(ByVal shutDownMessage As String,
                 ByVal forceClose As Boolean,
                 ByVal commandSource As String,
                 ByVal commandToken As String)



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
    Sub LoadDomain(ByVal domainNameToLoad As String,
                   ByVal commandSource As String,
                   ByVal commandToken As String,
                   Optional ByVal domainConfiguration As IHostedDomainModel = Nothing
                   )

    ''' <summary>
    ''' Unload the named domain safely
    ''' </summary>
    ''' <param name="domainNameToUnload">
    ''' The name of the domain to unload
    ''' </param>
    ''' <param name="commandSource">
    ''' The source of the domain unload command
    ''' </param>
    ''' <param name="commandToken">
    ''' A token to use to validate that the command came from an authorised source
    ''' </param>
    Sub UnloadDomain(ByVal domainNameToUnload As String,
                     ByVal commandSource As String,
                     ByVal commandToken As String)



End Interface
