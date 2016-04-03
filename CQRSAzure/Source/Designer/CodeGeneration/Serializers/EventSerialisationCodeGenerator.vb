Imports System.CodeDom
Imports System.Runtime.Serialization
Imports CQRSAzure.CQRSdsl.CodeGeneration
Imports CQRSAzure.CQRSdsl.Dsl

''' <summary>
''' Code generator to create the partial class responsible for the serialisation/deserialisation 
''' of an event instance
''' </summary>
''' <remarks>
''' This is required because different versions of an event may add, alter or even remove properties
''' and the serialiser needs to be able to cope with this
''' </remarks>
Public Class EventSerialisationCodeGenerator
    Implements IEntityImplementationCodeGenerator

    Public Const EVENT_FILENAME_IDENTIFIER = "eventSerialisation"
    Private ReadOnly m_event As EventDefinition

    Public ReadOnly Property FilenameBase As String Implements IEntityCodeGeneratorBase.FilenameBase
        Get
            Return ModelCodeGenerator.MakeValidCodeFilenameBase(m_event.Name & "_" & m_event.Version.ToString()) & "." & EVENT_FILENAME_IDENTIFIER
        End Get
    End Property

    ''' <summary>
    ''' Get the namespace of the aggregate that owns this event
    ''' (This will be the same as the event definition namespace as this is a partial class of same)
    ''' </summary>
    Public ReadOnly Property NamespaceHierarchy As IList(Of String) Implements IEntityCodeGeneratorBase.NamespaceHierarchy
        Get
            If (m_event IsNot Nothing) Then
                Return {m_event.AggregateIdentifier.CQRSModel.Name,
                    m_event.AggregateIdentifier.Name,
                    EventCodeGenerator.EVENT_FILENAME_IDENTIFIER}
            Else
                Return {}
            End If
        End Get
    End Property

    Public ReadOnly Property RequiredNamespaces As IEnumerable(Of CodeNamespaceImport) Implements IEntityCodeGeneratorBase.RequiredNamespaces
        Get
            Return {
                New CodeNamespaceImport("CQRSAzure"),
                New CodeNamespaceImport("CQRSAzure.EventSourcing"),
                New CodeNamespaceImport("CQRSAzure.Aggregation"),
                CodeGenerationUtilities.CreateNamespaceImport({m_event.AggregateIdentifier.CQRSModel.Name,
                    m_event.AggregateIdentifier.Name})
                }
        End Get
    End Property

    Private m_options As ModelCodeGenerationOptions = ModelCodeGenerationOptions.DefaultOptions()
    Public Sub SetCodeGenerationOptions(options As ModelCodeGenerationOptions) Implements IEntityCodeGeneratorBase.SetCodeGenerationOptions
        m_options = options
    End Sub

    Public Function GenerateImplementation() As CodeCompileUnit Implements IEntityImplementationCodeGenerator.GenerateImplementation

        'add the imports namespace
        Dim eventClasseRet As New CodeCompileUnit()


        Dim aggregateNamespace As CodeNamespace = CodeGenerationUtilities.CreateNamespace(Me.NamespaceHierarchy)
        'Add the imports
        For Each importNamespace As CodeNamespaceImport In RequiredNamespaces
            aggregateNamespace.Imports.Add(importNamespace)
        Next
        eventClasseRet.Namespaces.Add(aggregateNamespace)
        If (Not String.IsNullOrWhiteSpace(m_event.AggregateIdentifier.Notes)) Then
            aggregateNamespace.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_event.AggregateIdentifier.Notes}))
        End If

        ' Add the class declaration (partial)
        Dim classDeclaration As CodeTypeDeclaration = ClassCodeGeneration.ClassDeclaration(m_event.Name)
        If (Not String.IsNullOrWhiteSpace(m_event.Description)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.SummaryCommentSection({m_event.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(m_event.Notes)) Then
            classDeclaration.Comments.AddRange(CommentGeneration.RemarksCommentSection({m_event.Notes}))
        End If

        'Todo - The four serialisation methods...
        Dim serialiseParameters As New List(Of CodeParameterDeclarationExpression)
        serialiseParameters.Add(
            New CodeParameterDeclarationExpression(
                    New CodeTypeReference(GetType(StreamingContext)),
                    "context")
                    )


        ' <OnSerializing()> Friend Sub OnSerializingMethod(ByVal context As StreamingContext)
        Dim onSerialisingSub As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("OnSerializingMethod",
                                                                                              serialiseParameters)
        If (onSerialisingSub IsNot Nothing) Then
            onSerialisingSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Method called during the serialising process"}))
            onSerialisingSub.Comments.AddRange(CommentGeneration.RemarksCommentSection({"Any changes to the standard way that the event instance is serialised should go here",
                                                                                       "In which case you should add a [NonSerialized] attribute to any members you want to override the serialisation of"}))


            onSerialisingSub.CustomAttributes.Add(New CodeAttributeDeclaration("OnSerializing"))

            classDeclaration.Members.Add(onSerialisingSub)
        End If

        ' <OnSerialized()>  Friend Sub OnSerializedMethod(ByVal context As StreamingContext)
        Dim onSerialisedSub As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("OnSerializedMethod",
                                                                                              serialiseParameters)

        If (onSerialisedSub IsNot Nothing) Then
            onSerialisedSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Method called after the completion the serialising process"}))
            onSerialisedSub.Comments.AddRange(CommentGeneration.RemarksCommentSection({"Any resetting of parameters after they are serialised should go here"}))

            onSerialisedSub.CustomAttributes.Add(New CodeAttributeDeclaration("OnSerialized"))

            classDeclaration.Members.Add(onSerialisedSub)
        End If

        ' <OnDeserializing()> Friend Sub OnDeserializingMethod(ByVal context As StreamingContext)
        Dim onDeserialisingSub As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("OnDeserializingMethod",
                                                                                              serialiseParameters)

        If (onDeserialisingSub IsNot Nothing) Then
            onDeserialisingSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Method called during the deserialising process"}))
            onDeserialisingSub.Comments.AddRange(CommentGeneration.RemarksCommentSection({"Any manipulation of properties as they are deserialized should go here"}))


            onDeserialisingSub.CustomAttributes.Add(New CodeAttributeDeclaration("OnDeserializing"))

            classDeclaration.Members.Add(onDeserialisingSub)
        End If

        ' <OnDeserialized()>  Friend Sub OnDeserializedMethod(ByVal context As StreamingContext)
        Dim onDeserialisedSub As CodeMemberMethod = MethodCodeGenerator.PublicParameterisedSub("OnDeserializedMethod",
                                                                                              serialiseParameters)

        If (onDeserialisedSub IsNot Nothing) Then
            onDeserialisedSub.Comments.AddRange(CommentGeneration.SummaryCommentSection({"Method called after the deserialising process completes"}))
            onDeserialisedSub.Comments.AddRange(CommentGeneration.RemarksCommentSection({"Any manipulation of properties after they are deserialized should go here"}))


            onDeserialisedSub.CustomAttributes.Add(New CodeAttributeDeclaration("OnDeserialized"))

            classDeclaration.Members.Add(onDeserialisedSub)

        End If

        'put the resulting built class into the namespace
        aggregateNamespace.Types.Add(classDeclaration)

        Return eventClasseRet
    End Function

    Public Sub New(ByVal eventToGenerate As EventDefinition)
        m_event = eventToGenerate
    End Sub
End Class
