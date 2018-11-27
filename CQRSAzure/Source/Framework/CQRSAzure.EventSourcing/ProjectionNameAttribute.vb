Imports System
''' <summary>
''' An attribute to tag a projection implementation class with a specific name
''' </summary>
''' <remarks>
''' If a projection is not explicitly named then the class name is returned
''' </remarks>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class ProjectionNameAttribute
    Inherits Attribute

    Private ReadOnly m_projection As String
    ''' <summary>
    ''' The name of the projection that this class implements
    ''' </summary>
    Public ReadOnly Property ProjectionName As String
        Get
            Return m_projection
        End Get
    End Property

#Region "Constructors"
    Public Sub New(ByVal projectionNameIn As String)
        m_projection = projectionNameIn
    End Sub
#End Region


    Public Shared Function GetProjectionName(ByVal projectionClassType As Type) As String

        For Each aggregateNameAttr As ProjectionNameAttribute In projectionClassType.GetCustomAttributes(GetType(ProjectionNameAttribute), True)
#Region "Tracing"
            EventSourcing.LogVerboseInfo(projectionClassType.ToString() & " has the projection name attribute set to " & aggregateNameAttr.ProjectionName)
#End Region
            Return aggregateNameAttr.ProjectionName
        Next

#Region "Tracing"
        EventSourcing.LogVerboseInfo(projectionClassType.ToString() & " has no projection name attribute set")
#End Region

        ' No attribute - return an the type name as the aggregate name
        Return projectionClassType.Name

    End Function

    Public Shared Function GetProjectionName(ByVal projectionClass As Object) As String

        Return GetProjectionName(projectionClass.GetType())

    End Function

End Class
