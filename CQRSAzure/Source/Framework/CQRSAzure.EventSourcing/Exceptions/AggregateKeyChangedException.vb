Imports System.Security.Permissions
Imports System.Runtime.Serialization

''' <summary>
''' An aggregate may not have its key changed once it has been set
''' </summary>
<Serializable()>
Public NotInheritable Class AggregateKeyChangedException
    Inherits Exception

    Private ReadOnly m_newKey As Object
    ''' <summary>
    ''' The new key we are attempting to assign to an aggregate
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property NewKey As Object
        Get
            Return m_newKey
        End Get
    End Property

    Private ReadOnly m_oldKey As Object
    ''' <summary>
    ''' The existing key we have for the object
    ''' </summary>
    Public ReadOnly Property OldKey As Object
        Get
            Return m_oldKey
        End Get
    End Property


    Public Sub New(ByVal OldKeyIn As Object, ByVal NewKeyIn As Object, Optional ByVal innerException As Exception = Nothing)
        MyBase.New("Attempt to change an aggregate key", innerException)

        m_oldKey = OldKeyIn
        m_newKey = NewKeyIn

    End Sub


    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        If (info Is Nothing) Then Throw New ArgumentNullException("info")

        m_newKey = info.GetValue("NewKey", GetType(Object))
        m_oldKey = info.GetValue("OldKey", GetType(Object))

    End Sub

    <SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags:=SecurityPermissionFlag.SerializationFormatter)>
    Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)

        If (info Is Nothing) Then Throw New ArgumentNullException("info")
        info.AddValue("NewKey", NewKey)
        info.AddValue("OldKey", OldKey)

        MyBase.GetObjectData(info, context)
    End Sub

End Class
