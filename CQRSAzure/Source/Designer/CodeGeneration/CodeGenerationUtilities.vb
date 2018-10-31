Imports System.CodeDom
Imports System.CodeDom.Compiler
Imports Microsoft.CodeDom.Providers.DotNetCompilerPlatform
Imports Microsoft.CSharp

''' <summary>
''' General utility functions for making code generatio easier
''' </summary>
Public Class CodeGenerationUtilities


    ''' <summary>
    ''' Turn whatever code compile unit is passed in to VB.Net code in a multi-line string
    ''' </summary>
    ''' <param name="codeUnitToShow">
    ''' The program graph (partial or complete) to turn into VB code
    ''' </param>
    Public Shared Function ToVBCodeString(ByVal codeUnitToShow As CodeCompileUnit) As String

        Using provider As New VBCodeProvider
            'Visual Basic specific initialisation
            Dim vbNetOptions As New CodeDom.Compiler.CodeGeneratorOptions()
            vbNetOptions.BlankLinesBetweenMembers = True

            Dim sbRet As New System.Text.StringBuilder
            Using textWriter As New System.IO.StringWriter(sbRet)
                Using codeWriter As New IndentedTextWriter(textWriter)
                    provider.GenerateCodeFromCompileUnit(codeUnitToShow, codeWriter, vbNetOptions)
                End Using
            End Using
            Return sbRet.ToString()
        End Using


    End Function

    ''' <summary>
    ''' Turn whatever code compile unit is passed in to C# code in a multi-line string
    ''' </summary>
    ''' <param name="codeUnitToShow">
    ''' The program graph (partial or complete) to turn into C# code
    ''' </param>
    Public Shared Function ToCSharpCodeString(ByVal codeUnitToShow As CodeCompileUnit) As String

        Using provider As New Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider
            Dim cSharpOptions As New CodeDom.Compiler.CodeGeneratorOptions()
            cSharpOptions.BlankLinesBetweenMembers = True
            cSharpOptions.BracingStyle = "C" ' Change this to "Block" to have the open brace on the current line (freak)

            Dim sbRet As New System.Text.StringBuilder
            Using textWriter As New System.IO.StringWriter(sbRet)
                Using codeWriter As New IndentedTextWriter(textWriter)
                    provider.GenerateCodeFromCompileUnit(codeUnitToShow, codeWriter, cSharpOptions)
                End Using
            End Using
            Return sbRet.ToString()
        End Using


    End Function



    ''' <summary>
    ''' Wraps a type in a namespace so it can be generated as code
    ''' </summary>
    ''' <param name="codeClass">
    ''' The class we want to wrap up in a namespace for converting to a code snippet
    ''' </param>
    ''' <returns></returns>
    Public Shared Function Wrap(ByVal codeClass As CodeTypeDeclaration) As CodeCompileUnit

        Dim ret As New CodeCompileUnit()

        Dim nsDummy As New CodeNamespace("dummy")
        nsDummy.Types.Add(codeClass)
        ret.Namespaces.Add(nsDummy)

        Return ret

    End Function

    Public Shared Function Wrap(ByVal codeMember As CodeMemberProperty) As CodeCompileUnit

        Dim ret As New CodeTypeDeclaration("testClass")
        ret.Members.Add(codeMember)
        Return Wrap(ret)

    End Function

    Public Shared Function CreateNamespace(ByVal namespaceHierarchy As IList(Of String)) As CodeNamespace

        If (namespaceHierarchy IsNot Nothing) Then
            If (namespaceHierarchy.Count > 0) Then
                Dim namespaceName As String = String.Join(".", namespaceHierarchy)
                Return New CodeNamespace(ValidNamespaceString(namespaceName))
            End If
        End If

        Return Nothing
    End Function

    Public Shared Function CreateNamespaceImport(ByVal namespaceHierarchy As IList(Of String)) As CodeNamespaceImport

        If (namespaceHierarchy IsNot Nothing) Then
            If (namespaceHierarchy.Count > 0) Then
                Dim namespaceName As String = String.Join(".", namespaceHierarchy)
                Return New CodeNamespaceImport(ValidNamespaceString(namespaceName))
            End If
        End If

        Return Nothing
    End Function

    Public Shared Function ValidNamespaceString(ByVal entyIn As String) As String

        ' Dot is allowed 
        Dim invalidCharacters As Char() = " -!,;':@£$%^&*()-+=/\#~"
        Dim returnMembername As String = String.Join("_", entyIn.Split(invalidCharacters)).Trim()

        Return returnMembername

    End Function

End Class
