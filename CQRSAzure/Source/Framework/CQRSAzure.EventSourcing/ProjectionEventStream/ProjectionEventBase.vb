Imports System.Runtime.Serialization

Namespace Projections
    Public MustInherit Class ProjectionEventBase
        Implements IEvent(Of IProjectionInstanceIdentifier)

        Public MustOverride ReadOnly Property Version As UInteger Implements IEvent(Of IProjectionInstanceIdentifier).Version


        Public MustOverride Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData

        ''' <summary>
        ''' Force all projection events to have a serialisation constructor
        ''' </summary>
        Public Sub New(info As SerializationInfo,
               context As StreamingContext)

            'No code needed here - this is to force the parent class to implement this constructor

        End Sub

        Protected Sub New()

        End Sub
    End Class
End Namespace
