Public NotInheritable Class IdentityGroupAttribute
    Inherits Attribute

    Public Const IDENTITYGROUPNAME_INSTANCE As String = "Instance"
    Public Const IDENTITYGROUPNAME_ALL As String = "All"

#Region "private members"
    Private ReadOnly m_explicitGroupName As String
#End Region

    ''' <summary>
    ''' The explicitly set group name to use (if set)
    ''' </summary>
    Public ReadOnly Property IdentityGroupname As String
        Get
            Return m_explicitGroupName
        End Get
    End Property

#Region "Public constructors"

    ''' <summary>
    ''' Constructor with no explicitly named identity group
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Constructor for explicitly named identity group
    ''' </summary>
    ''' <param name="IdentityGroupIn">
    ''' The name of the identity group explicitly specified by the attribute
    ''' </param>
    Public Sub New(ByVal IdentityGroupIn As String)
        m_explicitGroupName = IdentityGroupIn
    End Sub

#End Region

    Public Shared Function GetIdentityGroup(ByVal categorisedObjectType As Type) As String

        For Each categoryAttr As IdentityGroupAttribute In categorisedObjectType.GetCustomAttributes(GetType(IdentityGroupAttribute), True)
            Return categoryAttr.IdentityGroupname
        Next

        ' No attribute - return an UNKNOWN domain
        Return String.Empty

    End Function

    Public Shared Function GetIdentityGroup(ByVal categorisedObject As Object) As String

        Return GetIdentityGroup(categorisedObject.GetType())

    End Function

End Class
