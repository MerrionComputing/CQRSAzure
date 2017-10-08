''' <summary>
''' A snapshot of a projection as at a particular point in time
''' </summary>
Public Interface IProjectionSnapshot(Of TAggregate As IAggregationIdentifier, TAggregateKey)
    Inherits IProjectionSnapshot

    ''' <summary>
    ''' The number of rows of values in this snapshot
    ''' </summary>
    ''' <returns>
    ''' This will be 1 if this is a signle-row data set
    ''' </returns>
    ReadOnly Property RowCount As Integer

End Interface

''' <summary>
''' A snapshot of a projection as at a particular point in time
''' </summary>
Public Interface IProjectionSnapshot

    ''' <summary>
    ''' The effective date/time for the snapshot
    ''' </summary>
    ''' <remarks>
    ''' This will require the underlying events in the stream to have some as-of date property 
    ''' </remarks>
    ReadOnly Property AsOfDate As DateTime

    ''' <summary>
    ''' The sequence number of the last event that was part of the snapshot
    ''' </summary>
    ''' <returns>
    ''' If the event stream current max sequence is higher than this then the snapshot is not up to date and the 
    ''' snapshot processor should continue from this point
    ''' </returns>
    ReadOnly Property Sequence As UInteger

    ''' <summary>
    ''' The projection values as at this snapshot
    ''' </summary>
    ReadOnly Property Values As IEnumerable(Of IProjectionSnapshotProperty)

End Interface

''' <summary>
''' A single property from a projection snapshot
''' </summary>
''' <typeparam name="TValue">
''' 
''' </typeparam>
Public Interface IProjectionSnapshotProperty(Of TValue)
    Inherits IProjectionSnapshotProperty

    ''' <summary>
    ''' The value of the property as it was as at the time of the snapshot
    ''' </summary>
    Overloads ReadOnly Property Value As TValue

    ''' <summary>
    ''' Create a deep copy of this property so it can be saved to an in-memory area without being still connected to the property it came from
    ''' </summary>
    Overloads Function Copy() As IProjectionSnapshotProperty(Of TValue)

End Interface

Public Interface IProjectionSnapshotProperty

    ''' <summary>
    ''' The unique name of the property in the projection that is being saved in a snapshot
    ''' </summary>
    ReadOnly Property Name As String

    ''' <summary>
    ''' The ordinal of the row of data 
    ''' </summary>
    ''' <remarks>
    ''' This can be -1 if this projection does not return multiple rows of data
    ''' </remarks>
    ReadOnly Property RowNumber As Integer

    ''' <summary>
    ''' The value of the property as it was as at the time of the snapshot
    ''' </summary>
    ReadOnly Property Value As Object

    ''' <summary>
    ''' Set the pro
    ''' </summary>
    ''' <param name="newValue">
    ''' </param>
    Sub UpdateValue(ByVal newValue As Object)

    ''' <summary>
    ''' Create a deep copy of this property so it can be saved to an in-memory area without being still connected to the property it came from
    ''' </summary>
    Function Copy() As IProjectionSnapshotProperty

End Interface