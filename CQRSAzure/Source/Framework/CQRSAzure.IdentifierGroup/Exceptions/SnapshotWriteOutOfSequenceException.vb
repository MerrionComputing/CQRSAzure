Imports System.Security.Permissions
Imports System.Runtime.Serialization

Namespace Exceptions

    ''' <summary>
    ''' A snapshot write is attempted but an existing higher-sequence snapshot
    ''' already exists
    ''' </summary>
    Public Class SnapshotWriteOutOfSequenceException
        Inherits Exception

        Private ReadOnly m_currentTopSequenceNumber As UInteger
        ''' <summary>
        ''' The highest sequence number for which a snapshot already exists
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CurrentTopSequenceNumber As UInteger
            Get
                Return m_currentTopSequenceNumber
            End Get
        End Property

        Private ReadOnly m_snapshotSequenceNumber As UInteger
        ''' <summary>
        ''' The sequence number of the snapshot we attempted to write
        ''' </summary>
        Public ReadOnly Property SnapshotSequenceNumber As UInteger
            Get
                Return m_snapshotSequenceNumber
            End Get
        End Property

        Public Sub New(ByVal snapshotSequenceNumberIn As UInteger, ByVal currentHighestSequenceIn As UInteger)
            MyBase.New("A snapshot write is attempted but an existing higher-sequence snapshot already exists")

            m_currentTopSequenceNumber = currentHighestSequenceIn
            m_snapshotSequenceNumber = snapshotSequenceNumberIn

        End Sub


        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
            If (info Is Nothing) Then Throw New ArgumentNullException("info")

            m_currentTopSequenceNumber = info.GetValue(NameOf(CurrentTopSequenceNumber), GetType(UInteger))
            m_snapshotSequenceNumber = info.GetValue(NameOf(SnapshotSequenceNumber), GetType(UInteger))

        End Sub

        <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
        Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

            If (info Is Nothing) Then Throw New ArgumentNullException("info")
            info.AddValue(NameOf(CurrentTopSequenceNumber), CurrentTopSequenceNumber)
            info.AddValue(NameOf(SnapshotSequenceNumber), SnapshotSequenceNumber)

            MyBase.GetObjectData(info, context)
        End Sub
    End Class
End Namespace
