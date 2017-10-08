Imports System.Runtime.Serialization
Imports CQRSAzure.EventSourcing

Namespace Commands

    ''' <summary>
    ''' Base class for events in the command event stream
    ''' </summary>
    ''' <remarks>
    ''' This is used to force them to have a serialisation constructor
    ''' </remarks>
    Public MustInherit Class CommandEventBase
        Inherits CommandEventContext
        Implements IEvent(Of ICommandAggregateIdentifier)

        Public MustOverride ReadOnly Property Version As UInteger Implements IEvent(Of ICommandAggregateIdentifier).Version

        Public Overridable Overloads Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData

            If (info IsNot Nothing) Then
                MyBase.GetObjectData(info, context)
            End If

        End Sub

        Protected Friend Sub New()

        End Sub

        Protected Sub New(Optional ByVal contextIn As CommandEventContext = Nothing)
            MyBase.New(contextIn)
        End Sub

        ''' <summary>
        ''' Force all command events to have a serialisation constructor
        ''' </summary>
        Public Sub New(info As SerializationInfo,
               context As StreamingContext)

            MyBase.New(info, context)
            'No code needed here - this is to force the parent class to implement this constructor

        End Sub

    End Class
End Namespace