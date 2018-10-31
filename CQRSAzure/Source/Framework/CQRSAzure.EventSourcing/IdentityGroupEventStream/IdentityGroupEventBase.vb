Imports System.Runtime.Serialization

Namespace IdentityGroups

    Public MustInherit Class IdentityGroupEventBase
        Implements IEvent(Of IIdentityGroupIdentifier)

        Public MustOverride ReadOnly Property Version As UInteger Implements IEvent(Of IIdentityGroupIdentifier).Version

        Public MustOverride Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData

        ''' <summary>
        ''' Force all identity group events to have a serialisation constructor
        ''' </summary>
        Public Sub New(info As SerializationInfo,
               context As StreamingContext)

            'No code needed here - this is to force the parent class to implement this constructor

        End Sub

        Protected Sub New()

        End Sub
    End Class
End Namespace