''' <summary>
''' An attribute to mark a class with the aggregate identifier to which it pertains
''' </summary>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=False)>
Public NotInheritable Class AggregateIdentifierAttribute
    Inherits Attribute

    ReadOnly m_attributeType As Type
    Public ReadOnly Property LinkedAttributeIdentifier As Type
        Get
            Return m_attributeType
        End Get
    End Property

    Public Sub New(ByVal attributeTypeLink As Type)
        If (attributeTypeLink.GetInterface(NameOf(IAggregationIdentifier)) Is Nothing) Then
            Throw New InvalidCastException("The aggregate identifier class must implement IAggregationIdentifier")
        End If
        m_attributeType = attributeTypeLink
    End Sub

    Public Shared Function GetAggregateName(type As Type) As String

        'if the type has a linked attribute return its name
        For Each aggregateAttribute As AggregateIdentifierAttribute In type.GetCustomAttributes(GetType(AggregateIdentifierAttribute), True)
            If (aggregateAttribute.LinkedAttributeIdentifier IsNot Nothing) Then
#Region "Tracing"
                EventSourcing.LogVerboseInfo(type.ToString() & " has the aggregate identifier attribute " & aggregateAttribute.LinkedAttributeIdentifier.Name)
#End Region
                Return aggregateAttribute.LinkedAttributeIdentifier.Name
            End If
        Next

#Region "Tracing"
        EventSourcing.LogVerboseInfo(type.ToString() & " has no aggregate identifier attribute")
#End Region

        'otherwise return the type name
        Return type.Name

    End Function


End Class
