''' <summary>
''' Common functionality that needs to be available throughout the library
''' </summary>
Module EventSourcing

#Region "Tracing settings"

    ''' <summary>
    ''' This switch controls the tracing of the common parts of the event sourcing
    ''' library.  
    ''' </summary>
    ''' <remarks>
    ''' Individual implementations will have their own trace switches which add fine grain
    ''' tracing control for each 
    ''' </remarks>
    Public EventSourcingTraceSwitch As New TraceSwitch("CQRSAzure.EventSourcing.Trace",
                                                       "EventSourcing Library Tracing")

#End Region


    ''' <summary>
    ''' Log an error to the trace listener(s)
    ''' </summary>
    ''' <param name="errorMessage">
    ''' The message to log
    ''' </param>
    <DebuggerStepThrough()>
    Public Sub LogError(ByVal errorMessage As String)

        If (EventSourcingTraceSwitch.TraceError) Then
            System.Diagnostics.Trace.TraceError(errorMessage)
        End If

    End Sub

    ''' <summary>
    ''' Log a warning to the trace listener(s)
    ''' </summary>
    ''' <param name="warningMessage">
    ''' The warning message to log
    ''' </param>
    <DebuggerStepThrough()>
    Public Sub LogWarning(ByVal warningMessage As String)

        If (EventSourcingTraceSwitch.TraceWarning) Then
            System.Diagnostics.Trace.TraceWarning(warningMessage)
        End If

    End Sub

    ''' <summary>
    ''' Log information to the trace listener(s)
    ''' </summary>
    ''' <param name="infoMessage">
    ''' The warning message to log
    ''' </param>
    <DebuggerStepThrough()>
    Public Sub LogInfo(ByVal infoMessage As String)

        If (EventSourcingTraceSwitch.TraceInfo) Then
            System.Diagnostics.Trace.TraceInformation(infoMessage)
        End If

    End Sub


    ''' <summary>
    ''' Log verbose information to the trace listener(s)
    ''' </summary>
    ''' <param name="infoMessage">
    ''' The warning message to log
    ''' </param>
    <DebuggerStepThrough()>
    Public Sub LogVerboseInfo(ByVal infoMessage As String)

        If (EventSourcingTraceSwitch.TraceVerbose) Then
            System.Diagnostics.Trace.TraceInformation(infoMessage)
        End If

    End Sub

End Module
