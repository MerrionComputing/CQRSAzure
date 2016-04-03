''' <summary>
''' An attribute to tag an entity for business role classification
''' </summary>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class CategoryAttribute

    Public Const UNKNOWN_CATEGORY As String = "Unclassified"

    Private ReadOnly m_category As String
    Public ReadOnly Property Category As String
        Get
            Return m_category
        End Get
    End Property

#Region "Constructors"
    Public Sub New(ByVal categoryNameIn As String)
        m_category = categoryNameIn
    End Sub
#End Region


    Public Shared Function GetCategory(ByVal categorisedObjectType As Type) As String

        For Each categoryAttr As CategoryAttribute In categorisedObjectType.GetCustomAttributes(GetType(CategoryAttribute), True)
            Return categoryAttr.Category
        Next

        ' No attribute - return an UNKNOWN domain
        Return UNKNOWN_CATEGORY

    End Function

    Public Shared Function GetCategory(ByVal categorisedObject As Object) As String

        Return GetCategory(categorisedObject.GetType())

    End Function

End Class
