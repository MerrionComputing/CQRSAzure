
''' <summary>
''' An attribute to tag an aggregate or event as belonging to a particular domain
''' </summary>
<AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)>
Public NotInheritable Class DomainNameAttribute
    Inherits Attribute

    Public Const UNKNOWN_DOMAIN As String = "Unclassified"

    Private ReadOnly m_domain As String
    ''' <summary>
    ''' The name of the business domain that this class is part of
    ''' </summary>
    Public ReadOnly Property Domain As String
        Get
            Return m_domain
        End Get
    End Property

#Region "Constructors"
    Public Sub New(Optional ByVal domainNameIn As String = UNKNOWN_DOMAIN)
        m_domain = domainNameIn
    End Sub
#End Region


    Public Shared Function GetDomainName(ByVal domainedObjectType As Type) As String

        For Each domainNameAttr As DomainNameAttribute In domainedObjectType.GetCustomAttributes(GetType(DomainNameAttribute), True)
#Region "Tracing"
            EventSourcing.LogVerboseInfo(domainedObjectType.ToString() & " has the domain name attribute set to " & domainNameAttr.Domain)
#End Region
            Return domainNameAttr.Domain
        Next

#Region "Tracing"
        EventSourcing.LogVerboseInfo(domainedObjectType.ToString() & " has no domain name attribute - defaulting to " & UNKNOWN_DOMAIN)
#End Region

        ' No attribute - return an UNKNOWN domain
        Return UNKNOWN_DOMAIN

    End Function

    Public Shared Function GetAggregateDomainQualifiedName(ByVal domainedObjectType As Type) As String

        Return GetDomainName(domainedObjectType) & "." & AggregateNameAttribute.GetAggregateName(domainedObjectType)

    End Function

    Public Shared Function GetDomainName(ByVal domainedObject As Object) As String

        Return GetDomainName(domainedObject.GetType())

    End Function


End Class
