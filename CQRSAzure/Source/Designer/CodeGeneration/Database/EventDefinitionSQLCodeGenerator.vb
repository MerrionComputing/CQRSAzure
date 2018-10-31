Imports System.CodeDom
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.Dsl

''' <summary>
''' Code generator to create the entity framework classes that correspond to the events that can occur for any given aggregate identifier
''' </summary>
Public Class EventDefinitionSQLCodeGenerator
    Implements IEntityImplementationCodeGenerator

    Public Const EVENT_FILENAME_IDENTIFIER = "EventDetail"
    Private ReadOnly m_event As EventDefinition

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGeneratorBase.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_event.Name) & "." & EVENT_FILENAME_IDENTIFIER
        End Get
    End Property

    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGeneratorBase.NamespaceHierarchy
        Get
            If (m_event IsNot Nothing) Then
                Return {m_event.AggregateIdentifier.CQRSModel.Name,
                    m_event.AggregateIdentifier.Name,
                    m_event.Name,
                    EVENT_FILENAME_IDENTIFIER}
            Else
                Return {}
            End If
        End Get
    End Property

    Public ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport) Implements IEntityCodeGeneratorBase.RequiredNamespaces
        Get
            Return {
                New CodeNamespaceImport("System.Data.Entity"),
                New CodeNamespaceImport("System.ComponentModel.DataAnnotations"), 'For KeyAttribute etc..
                New CodeNamespaceImport("CQRSAzure.EventSourcing")
                }
        End Get
    End Property

    Private m_options As IModelCodeGenerationOptions = ModelCodeGenerationOptions.Default()
    Public Sub SetCodeGenerationOptions(options As IModelCodeGenerationOptions) Implements IEntityCodeGeneratorBase.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityImplementationCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim dbEventClassRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        dbEventClassRet.Namespaces.Add(aggregateNamespace)

        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(m_event.AggregateIdentifier.Name & "_" & m_event.Name & "_Entity")
        If (Not String.IsNullOrWhiteSpace(m_event.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_event.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_event.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_event.Notes}))
        End If

        'Add the foreign key properties...
        Dim tableProperties As New List(Of IPropertyEntity)
        'Add the "Key" property
        Dim keyParameterName As String = "Key"
        If (Not String.IsNullOrWhiteSpace(m_event.AggregateIdentifier.KeyName)) Then
            keyParameterName = m_event.AggregateIdentifier.KeyName.Trim()
        End If
        tableProperties.Add(New TableFieldPropertyEntity() With {.Name = keyParameterName, .DataType = m_event.AggregateIdentifier.KeyDataType, .Description = "Unique identifier of the parent instance of " & m_event.AggregateIdentifier.Name, .IsKeyField = True})
        'Add the sequence number property
        tableProperties.Add(New TableFieldPropertyEntity() With {.Name = TableFieldPropertyEntity.PROPERTYNAME_SEQUENCE, .DataType = PropertyDataType.Integer, .Description = "Event Sequence", .IsKeyField = True})
        'Add the event type property
        tableProperties.Add(New TableFieldPropertyEntity() With {.Name = TableFieldPropertyEntity.PROPERTYNAME_EVENTNAME, .DataType = PropertyDataType.String, .Description = "Event Type", .IsKeyField = False})
        'Add the timestamp property
        tableProperties.Add(New TableFieldPropertyEntity() With {.Name = TableFieldPropertyEntity.PROPERTYNAME_TIMESTAMP, .DataType = PropertyDataType.Date, .Description = "Event Timestamp", .IsKeyField = False})


        'Add the data properties
        For Each evtProp In m_event.EventProperties
            tableProperties.Add(New TableFieldPropertyEntity() With {.Name = evtProp.Name, .DataType = evtProp.DataType, .Description = evtProp.Description, .Notes = evtProp.Notes})
        Next

        'make the backing variables for these
        classDeclaration.Members.AddRange(PropertyCodeGeneration.PrivateBackingMembers(tableProperties, False))

        'Then the main properties
        Dim columnNumber As Integer = 0
        For Each fieldProperty As TableFieldPropertyEntity In tableProperties
            Dim propertyMember As CodeMemberProperty = PropertyCodeGeneration.PublicMember(fieldProperty)
            If (propertyMember IsNot Nothing) Then
                If (fieldProperty.IsKeyField) Then
                    'add a Key attribute to it
                    propertyMember.CustomAttributes.Add(AttributeCodeGenerator.ParameterisedAttribute("Key", Nothing))
                    'and a column number so it can be used in foreign keys
                    Dim orderAttribute As New CodeAttributeArgument("Order", New CodePrimitiveExpression(columnNumber))
                    propertyMember.CustomAttributes.Add(AttributeCodeGenerator.ParameterisedAttribute("Column", {orderAttribute}))
                    'add a ForeignKey attribute

                End If
                classDeclaration.Members.Add(propertyMember)
            End If
            columnNumber += 1
        Next


        Return dbEventClassRet

    End Function

    Public Sub New(ByVal eventToGenerate As EventDefinition)
        m_event = eventToGenerate
    End Sub

End Class
