Imports System.Security.Permissions
Imports System.Runtime.Serialization
Imports System

''' <summary>
''' Where an update does not lock an event stream before executing a command over it then it is possible that
''' another update has occured in that time.  If that occurs an exception is raised to get the command to 
''' refresh its state and try again
''' </summary>
<Serializable()>
Public Class EventStreamWriteConcurrencyException
    Inherits EventStreamWriteException


    Private ReadOnly m_expectedSequence As Long
    ''' <summary>
    ''' What we expected the max sequence number to be when we were writing to the 
    ''' event stream underlying an aggregate
    ''' </summary>
    Public ReadOnly Property ExpectedSequenceNumber As Long
        Get
            Return m_expectedSequence
        End Get
    End Property

    Private ReadOnly m_actualSequence As Long
    ''' <summary>
    ''' The actual sequence number of the end of the event stream when we attempted to write an event to it
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ActualSequenceNumber As Long
        Get
            Return m_actualSequence
        End Get
    End Property

    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        m_actualSequence = info.GetValue(NameOf(ActualSequenceNumber), GetType(Long))
        m_expectedSequence = info.GetValue(NameOf(ExpectedSequenceNumber), GetType(Long))

    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        info.AddValue(NameOf(ActualSequenceNumber), ActualSequenceNumber)
        info.AddValue(NameOf(ExpectedSequenceNumber), ExpectedSequenceNumber)

        MyBase.GetObjectData(info, context)
    End Sub
End Class
