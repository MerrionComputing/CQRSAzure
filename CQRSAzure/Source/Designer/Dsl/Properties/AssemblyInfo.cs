#region Using directives

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

#endregion

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("CQRS Azure domain specific language")]
[assembly: AssemblyDescription("CQRS modelling domain-specific language")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Duncan Jones")]
[assembly: AssemblyProduct("CQRSdsl")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("Duncan Jones")]
[assembly: AssemblyCulture("")]
[assembly: System.Resources.NeutralResourcesLanguage("en")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("3.0.0.*")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]

//
// Make the Dsl project internally visible to the DslPackage assembly
//
[assembly: InternalsVisibleTo(@"CQRSAzure.CQRSdsl.DslPackage, PublicKey=0024000004800000940000000602000000240000525341310004000001000100E92A661F22F4BC641A4C0AECF58A640B3A73D096BA11F0223E02CF9E0F6C86AAA24339F545FF7E0832D287B8A586F3DE595FAAC3AB3C55CF9AAD852397BA8AF568B64BEB9E6BF3DCFCAE47FEF36859887665A9D6BD946C998B11C8F5E128ED10A22DFA97018CC6B834FBA0F95C752860D92D4ED79020FC9F1EE33D70F2BED0C0")]
[assembly: AssemblyFileVersion("3.0.0.0")]

