Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports CQRSAzure.EventSourcing

Namespace Local.File
    ''' <summary>
    ''' A projection snapshot wrapped up in a way that allows it to be stored in a local file
    ''' </summary>
    ''' <remarks>
    ''' Only one snapshot is held per file - this class only exists to allow additional fields to be stored pertinent to the 
    ''' snapshot circumstance rather than its data
    ''' </remarks>
    <Serializable()>
    <DataContract()>
    Public NotInheritable Class LocalFileWrappedProjectionSnapshot
        Implements IProjectionSnapshot

        <IgnoreDataMember()>
        <NonSerialized()>
        Private ReadOnly m_formatter As ILocalFileSettings.SerialiserType

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
        Private ReadOnly m_values As New List(Of LocalFileWrappedProjectionSnapshotProperty)
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

        Public Function ToBinaryStream(Optional formatter As IFormatter = Nothing) As System.IO.Stream

            Dim ms As New System.IO.MemoryStream()

            If formatter Is Nothing Then
                If (m_formatter = ILocalFileSettings.SerialiserType.Binary) Then
                    formatter = New BinaryFormatter
                    formatter.Serialize(ms, Me)
                Else
                    Throw New NotImplementedException()
                End If
            Else
                formatter.Serialize(ms, Me)
            End If
            'Move to the start of the binary stream so we can write it to a file
            ms.Seek(0, IO.SeekOrigin.Begin)
            Return ms

        End Function



        Public Sub WriteToBinaryStream(ByVal stream As System.IO.Stream,
                                       Optional formatter As IFormatter = Nothing)

            If formatter Is Nothing Then
                If (m_formatter = ILocalFileSettings.SerialiserType.Binary) Then
                    formatter = New BinaryFormatter
                    formatter.Serialize(stream, Me)
                Else
                    Throw New NotImplementedException()
                End If
            Else
                formatter.Serialize(stream, Me)
            End If

        End Sub

        Public Shared Function FromBinaryStream(ByVal binaryStream As System.IO.Stream,
                                                Optional formatterType As ILocalFileSettings.SerialiserType = ILocalFileSettings.SerialiserType.Binary) As LocalFileWrappedProjectionSnapshot

            If formatterType = ILocalFileSettings.SerialiserType.NameValuePairs Then
                Throw New NotImplementedException()
            Else
                Dim formatter As New BinaryFormatter()
                Return CType(formatter.Deserialize(binaryStream), LocalFileWrappedProjectionSnapshot)
            End If



        End Function

        Public Function Unwrap(Of TAggregate As IAggregationIdentifier, TAggregateKey)() As IProjectionSnapshot(Of TAggregate, TAggregateKey)

            Return ProjectionSnapshot.Create(Of TAggregate, TAggregateKey)(Me)

        End Function

        Public Sub New(ByVal projectionToWrap As IProjectionSnapshot,
                       Optional formatterType As ILocalFileSettings.SerialiserType = ILocalFileSettings.SerialiserType.Binary)

            m_formatter = formatterType

            m_Sequence = projectionToWrap.Sequence
            m_AsOfDate = projectionToWrap.AsOfDate
            m_RowCount = projectionToWrap.RowCount

            'wrap the projectionToWrap.Values
            For Each projectionValue In projectionToWrap.Values
                m_values.Add(New LocalFileWrappedProjectionSnapshotProperty(projectionValue))
            Next

        End Sub
    End Class


    ''' <summary>
    ''' An individual property of a snapshot, wrapped for saving in an local file
    ''' </summary>
    <Serializable()>
    <DataContract()>
    Public NotInheritable Class LocalFileWrappedProjectionSnapshotProperty
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