Imports System.Linq
Imports System.CodeDom
Imports System.CodeDom.Compiler
Imports CQRSAzure.CQRSdsl.Dsl
Imports Microsoft.CodeDom.Providers.DotNetCompilerPlatform
Imports Microsoft.VisualStudio.Modeling
Imports CQRSAzure.CQRSdsl.CustomCode.Interfaces
Imports CQRSAzure.CQRSdsl.CodeGeneration.ModelCodeGenerationOptions

''' <summary>
''' A class to perform the code generation for a complete CQRS model from it's XML
''' </summary>
''' <remarks>
''' Code will be generated in partial classes / partial interfaces so that it can readily be 
''' integrated with customisations
''' </remarks>
Public Class ModelCodeGenerator

    Private ReadOnly m_model As CQRSModel
    Private m_options As ModelCodeGenerationOptions = ModelCodeGenerationOptions.DefaultOptions()

    ''' <summary>
    ''' The code generation options to use when generating the source code for this model
    ''' </summary>
    Public ReadOnly Property Options As ModelCodeGenerationOptions
        Get
            If (m_options Is Nothing) Then
                m_options = ModelCodeGenerationOptions.DefaultOptions()
            End If
            Return m_options
        End Get
    End Property

    Public Sub GenerateCSharpCode()

        Using provider As New CSharpCodeProvider
            'C-Sharp specific initialisation
            Dim cSharpOptions As New CodeDom.Compiler.CodeGeneratorOptions()
            cSharpOptions.BlankLinesBetweenMembers = True
            cSharpOptions.BracingStyle = "C" ' Change this to "Block" to have the open brace on the current line (freak)

            GenerateModelCode(provider, cSharpOptions)

        End Using
    End Sub

    Public Sub GenerateVBNetCode()

        Using provider As New VBCodeProvider
            'Visual Basic specific initialisation
            Dim vbNetOptions As New CodeDom.Compiler.CodeGeneratorOptions()
            vbNetOptions.BlankLinesBetweenMembers = True

            GenerateModelCode(provider, vbNetOptions)
        End Using
    End Sub

    ''' <summary>
    ''' Generate the code in the language currently selected in the options
    ''' </summary>
    Public Sub GenerateCode()

        'perform any pre-generation operations
        If (m_options.SeparateFolderPerModel) Then

        End If

        If (m_options.CodeLanguage = ModelCodeGenerationOptions.SupportedLanguages.CSharp) Then
            GenerateCSharpCode()
        Else
            GenerateVBNetCode()
        End If

    End Sub

    ''' <summary>
    ''' Generate source code files for the given CQRS model given the code generator and options provided
    ''' </summary>
    ''' <param name="provider">
    ''' The code DOM provider for the specific language to generate the code in
    ''' </param>
    ''' <param name="codeGenOptions">
    ''' Additional formatting options to use when generating the code
    ''' </param>
    Private Sub GenerateModelCode(ByVal provider As CodeDomProvider, ByVal codeGenOptions As CodeGeneratorOptions)



        'Compile the model contents...
        For Each aggregate As AggregateIdentifier In m_model.AggregateIdentifiers
            Dim aggregateCodeGen As New AggregateIdentifierCodeGenerator(aggregate)
            aggregateCodeGen.SetCodeGenerationOptions(m_options)
            'Make the interface for the aggregate identifier
            CreateSourceCodeFile(MakeInterfaceName(aggregate.Name), aggregateCodeGen.GenerateInterface(), m_options, m_model.Name)
            'Make the class for the aggregate identifier
            CreateSourceCodeFile(MakeImplementationClassName(aggregate.Name), aggregateCodeGen.GenerateImplementation(), m_options, m_model.Name)

            'Walk the tree and create the code required for each element
            For Each eventDef As EventDefinition In aggregate.EventDefinitions
                Dim eventCodeGen As New EventCodeGenerator(eventDef)
                eventCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the event definition
                CreateSourceCodeFile(MakeInterfaceName(eventCodeGen.FilenameBase), eventCodeGen.GenerateInterface(), m_options, m_model.Name)
                'Make the class for the event definition
                CreateSourceCodeFile(MakeImplementationClassName(eventCodeGen.FilenameBase), eventCodeGen.GenerateImplementation(), m_options, m_model.Name)
                'Make the class for serialising the event definition
                Dim eventSerialisationCodeGen As New EventSerialisationCodeGenerator(eventDef)
                CreateSourceCodeFile(MakeImplementationClassName(eventSerialisationCodeGen.FilenameBase), eventSerialisationCodeGen.GenerateImplementation(), m_options, m_model.Name)
            Next

            For Each projDef As ProjectionDefinition In aggregate.ProjectionDefinitions
                Dim projCodeGen As New ProjectionCodeGenerator(projDef)
                projCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the projection definition
                CreateSourceCodeFile(MakeInterfaceName(projCodeGen.FilenameBase), projCodeGen.GenerateInterface(), m_options, m_model.Name)
                'Make the class for the projection definition
                CreateSourceCodeFile(MakeImplementationClassName(projCodeGen.FilenameBase), projCodeGen.GenerateImplementation(), m_options, m_model.Name)
            Next

            For Each cmdDef As CommandDefinition In aggregate.CommandDefinitions
                Dim cmdCodeGen As New CommandDefinitionCodeGenerator(cmdDef)
                cmdCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the command definition
                CreateSourceCodeFile(MakeInterfaceName(cmdCodeGen.FilenameBase), cmdCodeGen.GenerateInterface(), m_options, m_model.Name)
                'Make the class for the command definition
                CreateSourceCodeFile(MakeImplementationClassName(cmdCodeGen.FilenameBase), cmdCodeGen.GenerateImplementation(), m_options, m_model.Name)

                Dim cmdHandleCodeGen As New CommandHandlerCodeGenerator(cmdDef)
                cmdHandleCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the command handler
                CreateSourceCodeFile(MakeInterfaceName(cmdHandleCodeGen.FilenameBase), cmdHandleCodeGen.GenerateInterface(), m_options, m_model.Name)
                'Make the class for the command handler
                CreateSourceCodeFile(MakeImplementationClassName(cmdHandleCodeGen.FilenameBase), cmdHandleCodeGen.GenerateImplementation(), m_options, m_model.Name)
            Next

            For Each qryDef As QueryDefinition In aggregate.QueryDefinitions
                Dim qryCodeGen As New QueryDefinitionCodeGenerator(qryDef)
                qryCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the query handler
                CreateSourceCodeFile(MakeInterfaceName(qryCodeGen.FilenameBase), qryCodeGen.GenerateInterface(), m_options, m_model.Name)
                'Make the class for the query handler
                CreateSourceCodeFile(MakeImplementationClassName(qryCodeGen.FilenameBase), qryCodeGen.GenerateImplementation(), m_options, m_model.Name)
                Dim qryHandleCodeGen As New QueryHandlerCodeGenerator(qryDef)
                qryHandleCodeGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the query handler
                CreateSourceCodeFile(MakeInterfaceName(qryHandleCodeGen.FilenameBase), qryHandleCodeGen.GenerateInterface(), m_options, m_model.Name)
                'Make the class for the query handler
                CreateSourceCodeFile(MakeImplementationClassName(qryHandleCodeGen.FilenameBase), qryHandleCodeGen.GenerateImplementation(), m_options, m_model.Name)
            Next

            For Each idGrpDef As IdentityGroup In aggregate.IdentityGrouped
                Dim idGrpGen As New IdentityGroupCodeGenerator(idGrpDef)
                idGrpGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the identity group 
                CreateSourceCodeFile(MakeInterfaceName(idGrpGen.FilenameBase), idGrpGen.GenerateInterface(), m_options, m_model.Name)
                'Make the class for the identity group 
                CreateSourceCodeFile(MakeImplementationClassName(idGrpGen.FilenameBase), idGrpGen.GenerateImplementation(), m_options, m_model.Name)
            Next

            For Each classInst As Classifier In aggregate.Classifiers
                Dim classGen As New ClassifierCodeGenerator(classInst)
                classGen.SetCodeGenerationOptions(m_options)
                'Make the interface for the identity group 
                CreateSourceCodeFile(MakeInterfaceName(classGen.FilenameBase), classGen.GenerateInterface(), m_options, m_model.Name)
                'Make the class for the identity group 
                CreateSourceCodeFile(MakeImplementationClassName(classGen.FilenameBase), classGen.GenerateImplementation(), m_options, m_model.Name)
            Next

        Next
    End Sub

#Region "Constructors"
    ''' <summary>
    ''' Create a code generator for the specific CQRS model
    ''' </summary>
    ''' <param name="modelToGenerate">
    ''' The CQRS model that will be turned into code
    ''' </param>
    Public Sub New(ByVal modelToGenerate As CQRSModel)
        m_model = modelToGenerate
        m_options = ModelCodeGenerationOptions.DefaultOptions
    End Sub

    ''' <summary>
    ''' Create a code generator for the specific CQRS model with teh given code gen options
    ''' </summary>
    ''' <param name="modelToGenerate">
    ''' The CQRS model that will be turned into code
    ''' </param>
    Public Sub New(ByVal modelToGenerate As CQRSModel, ByVal codeGenOptions As ModelCodeGenerationOptions)
        m_model = modelToGenerate
        m_options = codeGenOptions
    End Sub
#End Region

    Public Shared Function MakeValidCodeFilenameBase(ByVal entityName As String) As String

        'For now, just strip spaces
        Return String.Join("_", entityName.Split(System.IO.Path.GetInvalidFileNameChars()))

    End Function

    Public Shared Function MakeInterfaceName(ByVal entityName As String) As String

        Return "I" & MakeImplementationClassName(entityName)

    End Function

    Public Shared Function MakeImplementationClassName(ByVal entityName As String) As String

        Return MakeValidCodeName(entityName)

    End Function

    Public Shared Function MakeValidCodeName(ByVal nameIn As String) As String
        Dim invalidCharacters As Char() = " -!,.;':@£$%^&*()-+=/\#~"
        Return String.Join("_", nameIn.Split(invalidCharacters))
    End Function

    ''' <summary>
    ''' Standard code generated attribute to add onto everything we generate
    ''' </summary>
    ''' <returns>
    ''' &lt;GeneratedCodeAttribute("CQRS On Azure - DSL", "1.0.0.0") &gt;
    ''' </returns>
    Public Shared Function StandardCodeGeneratedAttribute() As CodeAttributeDeclaration

        ' Declare a new generated code attribute
        Dim generatedCodeAttributeRet As GeneratedCodeAttribute =
            New GeneratedCodeAttribute("CQRS On Azure - DSL", "1.0.0.0")

        ' Use the generated code attribute members in the attribute declaration
        Dim codeAttrDecl As CodeAttributeDeclaration =
            New CodeAttributeDeclaration(GetType(GeneratedCodeAttribute).Name,
                New CodeAttributeArgument(
                    New CodePrimitiveExpression(generatedCodeAttributeRet.Tool)),
                New CodeAttributeArgument(
                    New CodePrimitiveExpression(generatedCodeAttributeRet.Version)))


        Return codeAttrDecl

    End Function


    Public Shared Sub CreateSourceCodeFile(ByVal filenameBase As String,
                                           ByVal codeUnit As CodeCompileUnit,
                                           ByVal options As ModelCodeGenerationOptions,
                                           Optional ByVal modelName As String = "")

        Dim CodeLanguage As SupportedLanguages = options.CodeLanguage
        If (CodeLanguage = SupportedLanguages.CSharp) Then
            filenameBase = filenameBase.Trim() & ".cs"
        Else
            filenameBase = filenameBase.Trim() & ".vb"
        End If

        Dim targetDirectoryName As String = options.DirectoryRoot.FullName
        If (Not options.DirectoryRoot.Exists) Then
            options.DirectoryRoot.Create()
        End If

        If (options.SeparateFolderPerModel) Then
            If Not String.IsNullOrWhiteSpace(modelName) Then
                Dim modelDirectory As New System.IO.DirectoryInfo(System.IO.Path.Combine(targetDirectoryName, MakeValidCodeName(modelName)))
                If Not modelDirectory.Exists() Then
                    modelDirectory.Create()
                End If
                targetDirectoryName = modelDirectory.FullName
            End If
        End If

        Using fWrite As IO.FileStream = System.IO.File.OpenWrite(System.IO.Path.Combine(targetDirectoryName, filenameBase))
            If (fWrite.CanWrite) Then
                Using sw As New System.IO.StreamWriter(fWrite)
                    If (CodeLanguage = SupportedLanguages.CSharp) Then
                        sw.Write(CodeGenerationUtilities.ToCSharpCodeString(codeUnit))
                    Else
                        sw.Write(CodeGenerationUtilities.ToVBCodeString(codeUnit))
                    End If
                End Using
            End If
        End Using

    End Sub



End Class

''' <summary>
''' Utility functions for generating comments in the code
''' </summary>
Public Class CommentGeneration

    ''' <summary>
    ''' Add a summary comment section
    ''' </summary>
    ''' <param name="commentText">
    ''' The lines of comment to put in the summary section
    ''' </param>
    Public Shared Function SummaryCommentSection(ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Return BaseCommentSection("summary", Nothing, commentText)

    End Function

    ''' <summary>
    ''' Add a returns comment section
    ''' </summary>
    ''' <param name="commentText">
    ''' The lines of comment to put in the returns section
    ''' </param>
    Public Shared Function ReturnsCommentSection(ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Return BaseCommentSection("returns", Nothing, commentText)

    End Function

    ''' <summary>
    ''' Add a remarks comment section
    ''' </summary>
    ''' <param name="commentText">
    ''' The lines of comment to put in the remarks section
    ''' </param>
    Public Shared Function RemarksCommentSection(ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Return BaseCommentSection("remarks", Nothing, commentText)

    End Function

    ''' <summary>
    ''' Add a parameter comment for the named parameter
    ''' </summary>
    ''' <param name="parameterName">
    ''' The name of the parameter
    ''' </param>
    ''' <param name="commentText">
    ''' The comment to attach to the parameter
    ''' </param>
    ''' <returns></returns>
    Public Shared Function ParamCommentsSection(ByVal parameterName As String, ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Dim nameValue As New Dictionary(Of String, String)()
        nameValue.Add("name", parameterName)

        Return BaseCommentSection("param", nameValue, commentText)

    End Function

    ''' <summary>
    ''' A general function to wrap a comment in XML documentation attributes
    ''' </summary>
    ''' <param name="sectionName">The name of the section - e.g. summary, remarks, returns </param>
    ''' <param name="commentText">The content of the section
    ''' </param>
    ''' <returns>
    ''' A code comment collection that can be attached to anything that takes XML comments
    ''' </returns>
    Private Shared Function BaseCommentSection(ByVal sectionName As String,
                                                  ByVal sectionAttributes As Dictionary(Of String, String),
                                                  ByVal commentText As IEnumerable(Of String)) As CodeCommentStatementCollection

        Dim ret As New CodeCommentStatementCollection()

        ret.Add(OpenCommentSection(sectionName, sectionAttributes))
        For Each commentLine As String In commentText
            'Add any non-blank strings
            If Not String.IsNullOrEmpty(commentLine) Then
                ret.Add(New CodeCommentStatement(commentLine, True))
            End If
        Next
        ret.Add(CloseCommentSection(sectionName))

        Return ret

    End Function

    ''' <summary>
    ''' Create a comment to start an XML comment section
    ''' </summary>
    ''' <param name="sectionName">
    ''' The name of the section - remarks, summary, returns, param etc.
    ''' </param>
    ''' <param name="sectionAttributes">
    ''' Additional attributes to put in the open section tag (optional)
    ''' </param>
    Private Shared Function OpenCommentSection(ByVal sectionName As String, ByVal sectionAttributes As Dictionary(Of String, String)) As CodeCommentStatement

        Dim commentInner As String = sectionName

        If (sectionAttributes IsNot Nothing) Then
            For Each sectionNameAttribute As String In sectionAttributes.Keys
                commentInner &= " " & sectionNameAttribute & "=""" & sectionAttributes(sectionNameAttribute) & """"
            Next
        End If

        Return New CodeCommentStatement("<" & commentInner.Trim & ">", True)

    End Function

    Private Shared Function CloseCommentSection(ByVal sectionName As String) As CodeCommentStatement

        Return New CodeCommentStatement("</" & sectionName.Trim & ">", True)

    End Function



End Class

''' <summary>
''' Utility functions for generating interfaces in the code generation
''' </summary>
Public Class InterfaceCodeGeneration

    Public Shared Function SimplePropertyDeclaration(ByVal readOnlyFlag As Boolean,
                                                     ByVal propertyName As String,
                                                     ByVal propertyType As PropertyDataType) As CodeMemberProperty

        Dim ret As New CodeMemberProperty()
        ret.Name = ModelCodeGenerator.MakeValidCodeName(propertyName)
        If (readOnlyFlag) Then
            ret.HasSet = False
        Else
            ret.HasSet = True
        End If

        Select Case propertyType
            Case PropertyDataType.Boolean
                ret.Type = New CodeTypeReference(GetType(Boolean))
            Case PropertyDataType.Date
                ret.Type = New CodeTypeReference(GetType(DateTime))
            Case PropertyDataType.Decimal
                ret.Type = New CodeTypeReference(GetType(Decimal))
            Case PropertyDataType.FloatingPointNumber
                ret.Type = New CodeTypeReference(GetType(Double))
            Case PropertyDataType.Image
                ret.Type = New CodeTypeReference(GetType(Byte()))
            Case PropertyDataType.Integer
                ret.Type = New CodeTypeReference(GetType(Integer))
            Case PropertyDataType.String
                ret.Type = New CodeTypeReference(GetType(String))
        End Select

        Return ret

    End Function

    Public Shared Function InterfaceDeclaration(ByVal entityName As String) As CodeTypeDeclaration

        Dim interfaceDeclarationRet As CodeTypeDeclaration = New CodeTypeDeclaration(ModelCodeGenerator.MakeInterfaceName(entityName))
        interfaceDeclarationRet.IsPartial = True
        interfaceDeclarationRet.IsInterface = True

        Return interfaceDeclarationRet

    End Function

    Public Shared Function ImplementsInterfaceReference(ByVal entityName As String) As CodeTypeReference

        Return New CodeTypeReference(ModelCodeGenerator.MakeInterfaceName(entityName))

    End Function

    Public Shared Function ImplementsInterfaceReference(ByVal interfaceName As String, ByVal propertyName As String) As CodeTypeReference

        Return New CodeTypeReference(interfaceName.Trim() & "." & propertyName.Trim())

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="interfaceName">
    ''' The base name of the interface
    ''' </param>
    ''' <param name="genericTypes">
    ''' The set of types (in order) of the generic interface
    ''' </param>
    ''' <returns></returns>
    Public Shared Function ImplementsGenericInterfaceReference(ByVal interfaceName As String, ByVal genericTypes As IList(Of CodeTypeReference)) As CodeTypeReference

        If (genericTypes.Count = 0) Then
            Return New CodeTypeReference(interfaceName)
        Else
            Dim ret As New CodeTypeReference(interfaceName)
            ret.TypeArguments.AddRange(genericTypes.ToArray)
            Return ret
        End If

    End Function

End Class

Public Class ClassCodeGeneration

    Public Shared Function ClassDeclaration(ByVal entityName As String) As CodeTypeDeclaration

        Dim classDeclarationRet As CodeTypeDeclaration = New CodeTypeDeclaration(ModelCodeGenerator.MakeImplementationClassName(entityName))
        classDeclarationRet.IsPartial = True
        classDeclarationRet.IsClass = True

        Return classDeclarationRet

    End Function

End Class

''' <summary>
''' Utility functions for creating the properties of the classes in code generation
''' </summary>
Public Class PropertyCodeGeneration


    ''' <summary>
    ''' Create a new public constant for the given name and data type and value
    ''' </summary>
    ''' <param name="constantName">
    ''' The name of the constant 
    ''' </param>
    ''' <param name="dataType">
    ''' The data type of the constant
    ''' </param>
    ''' <param name="value">
    ''' The value that the constant is set to
    ''' </param>
    ''' <returns></returns>
    Public Shared Function PublicConstant(ByVal constantName As String, ByVal dataType As CodeTypeReference, ByVal value As CodePrimitiveExpression) As CodeMemberField

        Dim constMember As New CodeMemberField()
        constMember.Name = EventCodeGenerator.ToMemberName(constantName, False)
        constMember.Type = dataType

        ' Clear the scope mask
        constMember.Attributes = (constMember.Attributes And (Not MemberAttributes.ScopeMask))
        ' Make it public accessible outside the class
        constMember.Attributes = (constMember.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public Or MemberAttributes.Const

        'Put the constant value in
        constMember.InitExpression = value

        Return constMember
    End Function

    ''' <summary>
    ''' Create the set of private fields backing an event/projection etc. property 
    ''' </summary>
    ''' <param name="eventProperties">
    ''' The set of properties for which to create a member declaration
    ''' </param>
    ''' <param name="readOnlyProperty">
    ''' If true, make the proeprty read-only so it may only be set in the class constructor
    ''' </param>
    ''' <returns>
    ''' Ideally the properties should be read only so that the class can be considered immutable
    ''' </returns>
    Public Overloads Shared Function PrivateBackingMembers(eventProperties As IList(Of IPropertyEntity), Optional readOnlyProperty As Boolean = False) As CodeTypeMember()

        Dim ret As New List(Of CodeTypeMember)

        For Each eventProperty In eventProperties
            Dim privateMember As New CodeMemberField(EventCodeGenerator.ToMemberType(eventProperty.DataType), EventCodeGenerator.ToMemberName(eventProperty.Name, True))
            If (readOnlyProperty) Then
                'This is not possible in CodeDOM as not all languages support this....you'd need to use a CodeSnippet (VB.Net or C#)..
                privateMember.Comments.Add(New CodeCommentStatement("TODO: Make this property read only"))
            End If
            ret.Add(privateMember)
        Next

        RegionCodeGeneration.WrapInRegion(RegionCodeGeneration.REGION_PRIVATE_MEMBERS, ret)

        Return ret.ToArray

    End Function

    Public Overloads Shared Function PrivateBackingMembers(eventProperties As IList(Of IEventPropertyEntity), Optional readOnlyProperty As Boolean = False) As CodeTypeMember()

        Return PrivateBackingMembers(eventProperties.Cast(Of IPropertyEntity).ToList(), readOnlyProperty)

    End Function

    ''' <summary>
    ''' Generate a public (get-only) property backed by the private member 
    ''' </summary>
    ''' <param name="eventProp">
    ''' The property desfinition for which to create a public member declaration
    ''' </param>
    ''' <param name="interfaceName">
    ''' The name of the interface this property is implementing
    ''' </param>
    ''' <returns></returns>
    Public Shared Function PublicMember(eventProp As IPropertyEntity, Optional ByVal interfaceName As String = "") As CodeMemberProperty

        Dim ret As New CodeMemberProperty()
        ret.Name = EventCodeGenerator.ToMemberName(eventProp.Name, False)
        ret.Type = EventCodeGenerator.ToMemberType(eventProp.DataType)

        ' Make it final
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.ScopeMask)) Or MemberAttributes.Final
        ' Make it public accessible outside the class
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public

        'Make it implement the interface if one is passed in...
        If (Not String.IsNullOrWhiteSpace(interfaceName)) Then
            ret.ImplementationTypes.Add(New CodeTypeReference(interfaceName))
        End If

        'add the getter
        Dim memberReference As New CodeFieldReferenceExpression()
        memberReference.FieldName = EventCodeGenerator.ToMemberName(eventProp.Name, True)

        ret.GetStatements.Add((New CodeMethodReturnStatement(memberReference)))


        ' Add-in the documentation for the property
        If (Not String.IsNullOrWhiteSpace(eventProp.Description)) Then
            ret.Comments.AddRange(CommentGeneration.SummaryCommentSection({eventProp.Description}))
        End If
        If (Not String.IsNullOrWhiteSpace(eventProp.Notes)) Then
            ret.Comments.AddRange(CommentGeneration.RemarksCommentSection({eventProp.Notes}))
        End If

        Return ret

    End Function
End Class

Public Class RegionCodeGeneration

    Public Const REGION_PRIVATE_MEMBERS As String = "Private members"

    Public Shared Sub WrapInRegion(ByVal RegionText As String, ByVal codeLines As ICollection(Of CodeTypeMember))

        If (codeLines.Count > 0) Then
            codeLines(0).StartDirectives.Add(New CodeRegionDirective(CodeRegionMode.Start, RegionText))
            codeLines(codeLines.Count - 1).EndDirectives.Add(New CodeRegionDirective(CodeRegionMode.End, String.Empty))
        End If

    End Sub

End Class


Public Class ConstructorCodeGenerator


    Public Shared Function ParameterisedConstructor(eventProperties As IList(Of IEventPropertyEntity)) As CodeConstructor

        Dim fromParametersConstructor As New CodeConstructor()
        fromParametersConstructor.Attributes += MemberAttributes.Public

        If (eventProperties IsNot Nothing) Then
            For Each eventProperty As IEventPropertyEntity In eventProperties
                fromParametersConstructor.Parameters.Add(New CodeParameterDeclarationExpression(EventCodeGenerator.ToMemberType(eventProperty.DataType), EventCodeGenerator.ToMemberName(eventProperty.Name, False, True)))
                ' add an assignment from this parameter to the internal field
                Dim targetField As New CodeFieldReferenceExpression()
                targetField.FieldName = EventCodeGenerator.ToMemberName(eventProperty.Name, True)
                Dim sourceField As New CodeFieldReferenceExpression()
                sourceField.FieldName = EventCodeGenerator.ToMemberName(eventProperty.Name, False, True)
                fromParametersConstructor.Statements.Add(New CodeAssignStatement(targetField, sourceField))
            Next
        End If

        Return fromParametersConstructor

    End Function

    Public Shared Function InterfaceBasedConstructor(eventProperties As IList(Of IEventPropertyEntity), ByVal interfaceTypeName As String, ByVal interfaceName As String) As CodeConstructor

        Dim fromInterfaceConstructor As New CodeConstructor()
        fromInterfaceConstructor.Attributes += MemberAttributes.Public

        'add a Init parameter..
        fromInterfaceConstructor.Parameters.Add(New CodeParameterDeclarationExpression(interfaceTypeName, interfaceName))

        If (eventProperties IsNot Nothing) Then
            For Each eventProperty As IEventPropertyEntity In eventProperties
                ' add an assignment from this parameter to the internal field
                Dim targetField As New CodeFieldReferenceExpression()
                targetField.FieldName = EventCodeGenerator.ToMemberName(eventProperty.Name, True)
                Dim sourceField As New CodeFieldReferenceExpression()
                sourceField.TargetObject = New CodeArgumentReferenceExpression(interfaceName)
                sourceField.FieldName = EventCodeGenerator.ToMemberName(eventProperty.Name, False)
                fromInterfaceConstructor.Statements.Add(New CodeAssignStatement(targetField, sourceField))
            Next
        End If

        Return fromInterfaceConstructor
    End Function

    ''' <summary>
    ''' Returns a call to invoke the given event class using the parameters passed in
    ''' </summary>
    ''' <param name="classToInvoke">
    ''' The reference to the class to invoke the constructor on
    ''' </param>
    ''' <param name="eventProperties">
    ''' The properties that will be turned into parameters 
    ''' </param>
    ''' <returns></returns>
    Public Shared Function EventConstructor(ByVal classToInvoke As CodeTypeReference, eventProperties As IList(Of IEventPropertyEntity)) As CodeObjectCreateExpression

        Dim objCreate As New CodeObjectCreateExpression(classToInvoke)
        For Each prop As IEventPropertyEntity In eventProperties
            Dim sourceField As New CodeFieldReferenceExpression()
            sourceField.FieldName = EventCodeGenerator.ToMemberName(prop.Name, False, True)
            objCreate.Parameters.Add(sourceField)
        Next

        Return objCreate

    End Function

End Class

Public Class MethodCodeGenerator

    ''' <summary>
    ''' Return the definition of a parameterised function 
    ''' </summary>
    ''' <param name="functionName">
    ''' The name to use for this function
    ''' </param>
    ''' <param name="returnType">
    ''' The data type to be returned from the function
    ''' </param>
    ''' <param name="parameters">
    ''' The set of input parameters for the function
    ''' </param>
    Public Shared Function PublicParameterisedFunction(ByVal functionName As String, ByVal returnType As CodeTypeReference, ByVal parameters As IList(Of CodeParameterDeclarationExpression)) As CodeMemberMethod

        Dim ret As New CodeMemberMethod()
        ret.Name = functionName
        ret.ReturnType = returnType
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public
        If (parameters IsNot Nothing) Then
            If (parameters.Count > 0) Then
                ret.Parameters.AddRange(parameters.ToArray)
            End If
        End If


        Return ret

    End Function

    ''' <summary>
    ''' Return the definition of a parameterised subroutine (void function) 
    ''' </summary>
    ''' <param name="functionName">
    ''' The name to use for this function
    ''' </param>
    ''' <param name="parameters">
    ''' The set of input parameters for the function
    ''' </param>
    ''' <param name="makeStatic">
    ''' Whether to make this a static method or not
    ''' </param>
    Public Shared Function PublicParameterisedSub(ByVal functionName As String,
                                                  ByVal parameters As IList(Of CodeParameterDeclarationExpression),
                                                  Optional ByVal makeStatic As Boolean = False) As CodeMemberMethod

        Return PublicParameterisedFunction(functionName, parameters, makeStatic:=makeStatic)

    End Function

    Public Shared Function PublicParameterisedFunction(ByVal functionName As String,
                                              ByVal parameters As IList(Of CodeParameterDeclarationExpression),
                                              Optional ByVal returnType As CodeTypeReference = Nothing,
                                              Optional ByVal makeStatic As Boolean = False) As CodeMemberMethod

        Dim ret As New CodeMemberMethod()
        ret.Name = functionName
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Public
        If (makeStatic) Then
            ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Static
        End If
        If (returnType IsNot Nothing) Then
            ret.ReturnType = returnType
        End If
        If (parameters IsNot Nothing) Then
            If (parameters.Count > 0) Then
                ret.Parameters.AddRange(parameters.ToArray)
            End If
        End If


        Return ret

    End Function

    Public Shared Function PrivateParameterisedSub(ByVal functionName As String,
                                                  ByVal parameters As IList(Of CodeParameterDeclarationExpression),
                                                  Optional ByVal makeStatic As Boolean = False) As CodeMemberMethod

        Return PrivateParameterisedFunction(functionName, parameters, makeStatic:=makeStatic)

    End Function

    Public Shared Function PrivateParameterisedFunction(ByVal functionName As String,
                                          ByVal parameters As IList(Of CodeParameterDeclarationExpression),
                                          Optional ByVal returnType As CodeTypeReference = Nothing,
                                          Optional ByVal makeStatic As Boolean = False) As CodeMemberMethod

        Dim ret As New CodeMemberMethod()
        ret.Name = functionName
        ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Private
        If (makeStatic) Then
            ret.Attributes = (ret.Attributes And (Not MemberAttributes.AccessMask)) Or MemberAttributes.Static
        End If
        If (returnType IsNot Nothing) Then
            ret.ReturnType = returnType
        End If
        If (parameters IsNot Nothing) Then
            If (parameters.Count > 0) Then
                ret.Parameters.AddRange(parameters.ToArray)
            End If
        End If


        Return ret

    End Function

End Class

Public Class AttributeCodeGenerator

    ''' <summary>
    ''' Create a parameterised attribute to tag a class or property
    ''' </summary>
    ''' <param name="AttributeName">
    ''' The attribute name to use - this should be a fully qualified name if ambiguity is possible
    ''' </param>
    ''' <param name="parameters">
    ''' The set of parameters to use to construct the attribute 
    ''' </param>
    ''' <returns></returns>
    Public Shared Function ParameterisedAttribute(ByVal AttributeName As String,
                                                  ByVal parameters As IList(Of CodeAttributeArgument)
                                                  ) As CodeAttributeDeclaration

        Dim ret As New CodeAttributeDeclaration(AttributeName)

        If parameters IsNot Nothing Then
            If parameters.Count > 0 Then
                For Each param As CodeAttributeArgument In parameters
                    ret.Arguments.Add(param)
                Next
            End If
        End If

        Return ret

    End Function

End Class

Public Class ParameterPropertyDefinition
    Implements CustomCode.Interfaces.IEventPropertyEntity


    Private m_DataType As PropertyDataType
    Public Property DataType As PropertyDataType Implements IEventPropertyEntity.DataType
        Get
            Return m_DataType
        End Get
        Set(value As PropertyDataType)
            m_DataType = value
        End Set
    End Property

    Private m_description As String
    Public Property Description As String Implements IDocumentedEntity.Description
        Get
            Return m_description
        End Get
        Set(value As String)
            m_description = value
        End Set
    End Property

    Private m_name As String
    Public Property Name As String Implements INamedEntity.Name
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Private m_notes As String
    Public Property Notes As String Implements IDocumentedEntity.Notes
        Get
            Return m_notes
        End Get
        Set(value As String)
            m_notes = value
        End Set
    End Property

    Public ReadOnly Property FullyQualifiedName As String Implements INamedEntity.FullyQualifiedName
        Get
            Return ""
        End Get
    End Property

    Public Sub New(ByVal nameIn As String, ByVal dataTypeIn As PropertyDataType)
        m_name = nameIn
        m_DataType = dataTypeIn
    End Sub
End Class