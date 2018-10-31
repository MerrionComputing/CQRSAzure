Imports System.Runtime.Serialization
Imports System.Security.Permissions

<Serializable>
Public Class OutOfSequenceSnapshotException
    Inherits EventStreamExceptionBase


    Private m_MaximumSequenceNumber As UInteger
    Public ReadOnly Property CurrentMaximumSequenceNumber As UInteger
        Get
            Return m_MaximumSequenceNumber
        End Get
    End Property

    Public Sub New(ByVal DomainNameIn As String,
                   ByVal AggregateNameIn As String,
                   ByVal AggregateKeyIn As String,
                   ByVal SequenceNumberIn As Long,
                   ByVal MaximumSequenceNumber As UInteger)

        MyBase.New(DomainNameIn, AggregateNameIn, AggregateKeyIn, SequenceNumberIn)
        m_MaximumSequenceNumber = MaximumSequenceNumber

    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
        m_MaximumSequenceNumber = info.GetValue("MaximumSequenceNumber", GetType(Long))
    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        info.AddValue("MaximumSequenceNumber", CurrentMaximumSequenceNumber)
    End Sub

End Class

