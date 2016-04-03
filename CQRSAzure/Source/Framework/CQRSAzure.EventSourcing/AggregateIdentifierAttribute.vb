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

End Class
