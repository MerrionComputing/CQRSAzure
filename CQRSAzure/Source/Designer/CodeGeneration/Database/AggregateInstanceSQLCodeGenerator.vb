Imports System.CodeDom
Imports System.Runtime.Serialization
Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.Dsl


''' <summary>
''' Code generate for the top-level event stream table that all events occuring to an
''' aggregate are stored into
''' </summary>
''' <remarks>
''' This class has a property for the unique key, sequence number, event type and timestamp which can
''' then be linked to a details table for each event instance 
''' </remarks>
Public Class AggregateInstanceSQLCodeGenerator
    Implements IEntityImplementationCodeGenerator

    Public Const EVENT_FILENAME_IDENTIFIER = "EventStream"
    Private ReadOnly m_aggregateIdentifier As AggregateIdentifier

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGeneratorBase.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_aggregateIdentifier.Name) & "." & EVENT_FILENAME_IDENTIFIER
        End Get
    End Property

    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGeneratorBase.NamespaceHierarchy
        Get
            If (m_aggregateIdentifier IsNot Nothing) Then
                Return {m_aggregateIdentifier.CQRSModel.Name,
                    m_aggregateIdentifier.Name,
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
        Dim dbAggregateClassRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        dbAggregateClassRet.Namespaces.Add(aggregateNamespace)

        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(Me.m_aggregateIdentifier.Name & "_Entity")
        If (Not String.IsNullOrWhiteSpace(m_aggregateIdentifier.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_aggregateIdentifier.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_aggregateIdentifier.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_aggregateIdentifier.Notes}))
        End If

        Dim tableProperties As New List(Of IPropertyEntity)

        'Add the "Key" property
        Dim keyParameterName As String = "Key"
        If (Not String.IsNullOrWhiteSpace(m_aggregateIdentifier.KeyName)) Then
            keyParameterName = m_aggregateIdentifier.KeyName.Trim()
        End If
        tableProperties.Add(New TableFieldPropertyEntity() With {.Name = keyParameterName, .DataType = m_aggregateIdentifier.KeyDataType, .Description = "Unique identifier of the instance of " & m_aggregateIdentifier.Name, .IsKeyField = True})
        'Add the sequence number property
        tableProperties.Add(New TableFieldPropertyEntity() With {.Name = TableFieldPropertyEntity.PROPERTYNAME_SEQUENCE, .DataType = PropertyDataType.Integer, .Description = "Event Sequence", .IsKeyField = True})
        'Add the event type property
        tableProperties.Add(New TableFieldPropertyEntity() With {.Name = TableFieldPropertyEntity.PROPERTYNAME_EVENTNAME, .DataType = PropertyDataType.String, .Description = "Event Type", .IsKeyField = False})
        'Add the timestamp property
        tableProperties.Add(New TableFieldPropertyEntity() With {.Name = TableFieldPropertyEntity.PROPERTYNAME_TIMESTAMP, .DataType = PropertyDataType.Date, .Description = "Event Timestamp", .IsKeyField = False})

        'add the private backing members
        classDeclaration.Members.AddRange(PropertyCodeGeneration.PrivateBackingMembers(tableProperties, False))

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
                End If
                classDeclaration.Members.Add(propertyMember)
            End If
            columnNumber += 1
        Next

        'add the event types
        For Each evt As EventDefinition In m_aggregateIdentifier.EventDefinitions

        Next

        'put the built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return dbAggregateClassRet

    End Function

    Public Sub New(ByVal aggregateToGenerate As AggregateIdentifier)
        m_aggregateIdentifier = aggregateToGenerate
    End Sub

End Class
