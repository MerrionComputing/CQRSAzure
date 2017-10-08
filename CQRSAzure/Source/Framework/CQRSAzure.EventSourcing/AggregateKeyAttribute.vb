''' <summary>
''' Attribute to use to tag an input parameter of a command or query as being the key identifier of the aggregate to which the
''' query or command should be applied
''' </summary>
<AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class AggregateKeyAttribute
    Inherits Attribute


#Region "Constructors"
    Public Sub New()

    End Sub
#End Region



End Class

<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class AggregateKeyDataTypeAttribute
    Inherits Attribute

    Private ReadOnly m_keyType As Type


    Public ReadOnly Property AggregateKeyType As Type

        Get
            Return m_keyType
        End Get
    End Property

    Public Sub New(ByVal keyTypeIn As Type)
        m_keyType = keyTypeIn
    End Sub

    Public Shared Function GetAggregateKeyDataType(ByVal aggregateType As Type) As Type


        ' First try and get the key type from an attribute attached to the class
        For Each aggregateKeyAttr As AggregateKeyDataTypeAttribute In aggregateType.GetCustomAttributes(GetType(AggregateKeyDataTypeAttribute), True)
#Region "Tracing"
            EventSourcing.LogVerboseInfo(aggregateType.ToString() & " has the aggregate key data type attribute of " & aggregateKeyAttr.AggregateKeyType.ToString())
#End Region
            Return aggregateKeyAttr.AggregateKeyType
        Next

        'Does the class implement IAggregationIdentifier
        Dim IAggregationIdentifierType As Type = aggregateType.GetInterface("IAggregationIdentifier`1")
        If (IAggregationIdentifierType IsNot Nothing) Then
            Return IAggregationIdentifierType.GetGenericArguments()(0)
        End If

#Region "Tracing"
        EventSourcing.LogVerboseInfo(aggregateType.ToString() & " has no aggregate key data type attribute - defaulting to string")
#End Region

        'If we get here return the default data type
        Return GetType(String)

    End Function

End Class