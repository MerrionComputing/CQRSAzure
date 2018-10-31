Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.Dsl

''' <summary>
''' Utility class for creating the fields that will be used to create the DB tables backing the event stream and it's events
''' </summary>
Public Class TableFieldPropertyEntity
    Implements IPropertyEntity

    Public Const PROPERTYNAME_KEY As String = "Key" 'This can be overridden by the user choosing a business meaningful key name
    Public Const PROPERTYNAME_SEQUENCE As String = "Sequence"
    Public Const PROPERTYNAME_EVENTNAME As String = "EventName"
    Public Const PROPERTYNAME_EVENTVERSION As String = "EventVersion"
    Public Const PROPERTYNAME_TIMESTAMP As String = "Timestamp"

    ''' <summary>
    ''' Is this field a key field for this table
    ''' </summary>
    ''' <returns></returns>
    Public Property IsKeyField As Boolean

    Public Property DataType As PropertyDataType Implements IPropertyEntity.DataType

    Public Property Description As String Implements IDocumentedEntity.Description

    Public ReadOnly Property FullyQualifiedName As String Implements INamedEntity.FullyQualifiedName
        Get
            Return Name
        End Get
    End Property

    Public Property Name As String Implements INamedEntity.Name

    Public Property Notes As String Implements IDocumentedEntity.Notes


End Class
