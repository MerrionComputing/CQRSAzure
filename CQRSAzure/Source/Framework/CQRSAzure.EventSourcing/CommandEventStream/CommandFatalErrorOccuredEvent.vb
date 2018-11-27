Imports System
Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands

    ''' <summary>
    ''' A fatal error has occured while processing this command that means it cannot (or should not)
    ''' be retried
    ''' </summary>
    <Serializable()>
    Public NotInheritable Class CommandFatalErrorOccuredEvent
        Inherits CommandEventBase
        Implements ICommandFatalErrorOccuredEvent
        Implements IEvent(Of CommandAggregateIdentifier)




        Private ReadOnly m_errorDate As Date?
        Public ReadOnly Property ErrorDate As Date? Implements ICommandFatalErrorOccuredEvent.ErrorDate
            Get
                Return m_errorDate
            End Get
        End Property


        Private ReadOnly m_errorMessage As String
        Public ReadOnly Property ErrorMessage As String Implements ICommandFatalErrorOccuredEvent.ErrorMessage
            Get
                Return m_errorMessage
            End Get
        End Property


        Private ReadOnly m_stepNumber As Integer
        Public ReadOnly Property StepNumber As Integer Implements ICommandFatalErrorOccuredEvent.StepNumber
            Get
                Return m_stepNumber
            End Get
        End Property

        Public Overrides ReadOnly Property Version As UInteger Implements IEvent(Of CommandAggregateIdentifier).Version
            Get
                Return 1
            End Get
        End Property

        Public Overrides Sub GetObjectData(info As SerializationInfo, context As StreamingContext)

            If Not (info Is Nothing) Then
                MyBase.GetObjectData(info, context)
                info.AddValue(NameOf(ErrorDate), ErrorDate)
                info.AddValue(NameOf(ErrorMessage), ErrorMessage)
                info.AddValue(NameOf(StepNumber), StepNumber)
            End If

        End Sub

        Public Sub New(info As SerializationInfo,
       context As StreamingContext)

            MyBase.New(info, context)

            If Not (info Is Nothing) Then
                'Populate the members of the event from the context
                m_errorMessage = info.GetString(NameOf(ErrorMessage))
                m_errorDate = info.GetDateTime(NameOf(ErrorDate))
                m_stepNumber = info.GetInt32(NameOf(StepNumber))
            End If

        End Sub

        Private Sub New(ByVal errorDateIn As DateTime,
                        ByVal errorMessageIn As String,
                        ByVal stepNumberIn As Integer,
                        Optional ByVal contextIn As CommandEventContext = Nothing)
            MyBase.New(contextIn)
            m_errorDate = errorDateIn
            m_errorMessage = errorMessageIn
            m_stepNumber = stepNumberIn


        End Sub

        Public Overloads Shared Function Create(ByVal errorDateIn As DateTime,
                        ByVal errorMessageIn As String,
                        ByVal stepNumberIn As Integer,
                        Optional ByVal contextIn As CommandEventContext = Nothing) As CommandFatalErrorOccuredEvent

            Return New CommandFatalErrorOccuredEvent(errorDateIn,
                                                     errorMessageIn,
                                                     stepNumberIn,
                                                     contextIn)

        End Function

    End Class
End Namespace
