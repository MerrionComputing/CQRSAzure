Imports System.Runtime.Serialization

Namespace Queries
    Public MustInherit Class QueryEventBase
        Implements IEvent(Of IQueryAggregateIdentifier)

        Public MustOverride ReadOnly Property Version As UInteger Implements IEvent(Of IQueryAggregateIdentifier).Version

        Public MustOverride Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData


        Protected Friend Sub New()

        End Sub

        ''' <summary>
        ''' Force all query events to have a serialisation constructor
        ''' </summary>
        Public Sub New(info As SerializationInfo,
               context As StreamingContext)

            'No code needed here - this is to force the parent class to implement this constructor

        End Sub
    End Class
End Namespace