Imports CQRSAzure.EventSourcing

Public NotInheritable Class StateChange(Of TFieldDataType)
    Inherits StateChange
    Implements IStateChange(Of TFieldDataType)


    Private ReadOnly m_OldValue As TFieldDataType
    Public ReadOnly Property OldValue As TFieldDataType Implements IStateChange(Of TFieldDataType).OldValue
        Get
            Return m_OldValue
        End Get
    End Property


    Private m_NewValue As TFieldDataType
    Public ReadOnly Property NewValue As TFieldDataType Implements IStateChange(Of TFieldDataType).NewValue
        Get
            Return m_NewValue
        End Get
    End Property

    Private ReadOnly Property OldValueAsObject As Object Implements IStateChange.OldValue
        Get
            Return OldValue
        End Get
    End Property

    Private ReadOnly Property NewValueAsObject As Object Implements IStateChange.NewValue
        Get
            Return NewValue
        End Get
    End Property


    Private Sub New(ByVal oldValueInit As TFieldDataType, ByVal newValueInit As TFieldDataType)
        m_OldValue = oldValueInit
        m_NewValue = newValueInit
    End Sub


    Public Overloads Shared Function Create(Of T)(ByVal oldValueInit As T, ByVal newValueInit As T) As StateChange(Of T)

        Return New StateChange(Of T)(oldValueInit, newValueInit)

    End Function

    Public Shared Function Update(Of T)(ByVal priorToChange As StateChange(Of T), newValueInit As T) As StateChange(Of T)

        Return StateChange(Of T).Create(priorToChange.OldValue, newValueInit)

    End Function

End Class

Public Class StateChange


    Public Shared Function Create(Of T)(ByVal oldValueInit As T, ByVal newValueInit As T) As StateChange(Of T)

        Return StateChange(Of T).Create(oldValueInit, newValueInit)

    End Function


End Class