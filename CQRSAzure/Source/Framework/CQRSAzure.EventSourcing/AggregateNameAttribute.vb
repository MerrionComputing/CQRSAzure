Imports System

''' <summary>
''' An attribute to tag an aggregate identifier class with a specific name
''' </summary>
''' <remarks>
''' If an aggregate identifier is not explicitly named then the class name is returned
''' </remarks>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class AggregateNameAttribute
    Inherits Attribute

    Private ReadOnly m_aggregate As String
    ''' <summary>
    ''' The business meaningful name of the aggregate identifier that this class implements
    ''' </summary>
    Public ReadOnly Property AggregateName As String
        Get
            Return m_aggregate
        End Get
    End Property

#Region "Constructors"
    Public Sub New(ByVal aggregateNameIn As String)
        m_aggregate = aggregateNameIn
    End Sub
#End Region


    Public Shared Function GetAggregateName(ByVal aggregateIdentifierType As Type) As String

        For Each aggregateNameAttr As AggregateNameAttribute In aggregateIdentifierType.GetCustomAttributes(GetType(AggregateNameAttribute), True)
#Region "Tracing"
            EventSourcing.LogVerboseInfo(aggregateIdentifierType.ToString() & " has the aggregate name attribute of " & aggregateNameAttr.AggregateName)
#End Region
            Return aggregateNameAttr.AggregateName
        Next

#Region "Tracing"
        EventSourcing.LogVerboseInfo(aggregateIdentifierType.ToString() & " has no aggregate name attribute - defaulting to class name")
#End Region

        ' No attribute - return an the type name as the aggregate name
        Return aggregateIdentifierType.Name

    End Function

    Public Shared Function GetAggregateName(ByVal aggregateIdentifier As Object) As String

        Return GetAggregateName(aggregateIdentifier.GetType())

    End Function

End Class
