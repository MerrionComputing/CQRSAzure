Imports System
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Azure.Blob
    ''' <summary>
    ''' A class to wrap up a projection snapshot so it can be written to an Azure append blob
    ''' </summary>
    ''' <remarks>
    ''' Each block in an append blob can be a different size, up to a maximum of 4 MB, and an append blob can include a maximum of 50,000 blocks.
    ''' </remarks>
    <Serializable()>
    <DataContract()>
    Public Class BlobBlockWrappedProjectionSnapshot
        Implements IProjectionSnapshot

        <DataMember(Name:=NameOf(Sequence), Order:=0)>
        Private ReadOnly m_Sequence As UInteger
        ''' <summary>
        ''' The effective sequence number of the projection snapshot taken
        ''' </summary>
        Public ReadOnly Property Sequence As UInteger Implements IProjectionSnapshot.Sequence
            Get
                Return m_Sequence
            End Get
        End Property

        <DataMember(Name:=NameOf(AsOfDate), Order:=1)>
        Private ReadOnly m_AsOfDate As DateTime
        ''' <summary>
        ''' The effective date of the data as at the point in time that the snapshot was taken
        ''' </summary>
        Public ReadOnly Property AsOfDate As DateTime Implements IProjectionSnapshot.AsOfDate
            Get
                Return m_AsOfDate
            End Get
        End Property

        <DataMember(Name:=NameOf(Values), Order:=2)>
        Private ReadOnly m_values As New List(Of BlobBlockWrappedProjectionSnapshotProperty)
        Public ReadOnly Property Values As IEnumerable(Of IProjectionSnapshotProperty) Implements IProjectionSnapshot.Values
            Get
                Return m_values
            End Get
        End Property

        Private ReadOnly m_RowCount As Integer
        Public ReadOnly Property RowCount As Integer Implements IProjectionSnapshot.RowCount

            Get
                Return m_RowCount
            End Get
        End Property

        Public Function ToBinaryStream() As System.IO.Stream

            Dim ms As New System.IO.MemoryStream()

            Dim bf As New BinaryFormatter()
            bf.Serialize(ms, Me)
            'Move to the start of the binary stream so we can write it to an event
            ms.Seek(0, IO.SeekOrigin.Begin)
            Return ms

        End Function

        Public Shared Function FromBinaryStream(ByVal binaryStream As System.IO.Stream) As BlobBlockWrappedProjectionSnapshot

            Dim bf As BinaryFormatter = New BinaryFormatter()
            Return CType(bf.Deserialize(binaryStream), BlobBlockWrappedProjectionSnapshot)

        End Function

        Public Function Unwrap(Of TAggregate As IAggregationIdentifier, TAggregateKey)() As IProjectionSnapshot(Of TAggregate, TAggregateKey)

            Return ProjectionSnapshot.Create(Of TAggregate, TAggregateKey)(Me)

        End Function

        Public Function UnwrapUntyped() As IProjectionSnapshot
            Return Me
        End Function

        Public Sub New(ByVal projectionToWrap As IProjectionSnapshot)

            m_Sequence = projectionToWrap.Sequence
            m_AsOfDate = projectionToWrap.AsOfDate
            m_RowCount = projectionToWrap.RowCount

            'wrap the projectionToWrap.Values
            For Each projectionValue In projectionToWrap.Values
                m_values.Add(New BlobBlockWrappedProjectionSnapshotProperty(projectionValue))
            Next

        End Sub

    End Class

    ''' <summary>
    ''' An individual property of a snapshot, wrapped for saving in an Azure blob
    ''' </summary>
    <Serializable()>
    <DataContract()>
    Public NotInheritable Class BlobBlockWrappedProjectionSnapshotProperty
        Implements IProjectionSnapshotProperty

        <DataMember(Name:=NameOf(Name), Order:=0)>
        Private ReadOnly m_name As String
        Public ReadOnly Property Name As String Implements IProjectionSnapshotProperty.Name
            Get
                Return m_name
            End Get
        End Property

        <DataMember(Name:=NameOf(RowNumber), Order:=1)>
        Private m_RowNumber As Integer
        Public ReadOnly Property RowNumber As Integer Implements IProjectionSnapshotProperty.RowNumber
            Get
                Return m_RowNumber
            End Get
        End Property

        <DataMember(Name:=NameOf(Value), Order:=2)>
        Private ReadOnly m_value As Object
        Public ReadOnly Property Value As Object Implements IProjectionSnapshotProperty.Value
            Get
                Return m_value
            End Get
        End Property

        ''' <summary>
        ''' This is not implemented as you should never update a value that has been taken from a projection
        ''' </summary>
        Public Sub UpdateValue(newValue As Object) Implements IProjectionSnapshotProperty.UpdateValue
            Throw New NotImplementedException()
        End Sub

        Public Function Copy() As IProjectionSnapshotProperty Implements IProjectionSnapshotProperty.Copy
            Return Me
        End Function

        Public Sub New(ByVal propertyToWrap As IProjectionSnapshotProperty)

            m_name = propertyToWrap.Name
            m_RowNumber = propertyToWrap.RowNumber
            m_value = propertyToWrap.Value

        End Sub

    End Class
End Namespace