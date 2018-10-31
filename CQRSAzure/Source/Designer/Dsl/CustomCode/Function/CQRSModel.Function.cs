
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;
using DslModeling = global::Microsoft.VisualStudio.Modeling;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class CQRSModel
    {

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public CQRSModel(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
        	: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
        }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="partition">
        /// Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">
        /// List of domain property id/value pairs to set once the element is created.
        /// </param>
        /// <remarks>
        /// This allows the default of mandatory properties for the ModelSettings
        /// </remarks>
        public CQRSModel(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
        	: base(partition, propertyAssignments)
        {

            // If the default language is not set, set it to VB.Net

        }

        /// <summary>
        /// Get the code generation options that have been set at the model level
        /// </summary>
        /// <remarks>
        /// These may be overridden on a per-user basis as part of the build process
        /// </remarks>
        public IModelCodeGenerationOptions GetCodeGenerationOptions()
        {
            ModelCodegenerationOptionsBase.SupportedLanguages CodeLanguageIn = ModelCodegenerationOptionsBase.SupportedLanguages.VBNet ;
            ModelCodegenerationOptionsBase.ConstructorPreferenceSetting ConstructorPreferenceIn = ModelCodegenerationOptionsBase.ConstructorPreferenceSetting.GenerateBoth ;
            System.IO.DirectoryInfo DirectoryRootIn = null;
            bool SeparateFolderPerModelIn = true ;
            bool SeparateFolderPerAggregateIn = true ;
            bool GenerateEntityFrameworkClassesIn = false;

            // Read the model properties that affect code generation
            if (this.DefaultCodeGenerationLanguage == TargetLanguage.CSharp)
            {
                CodeLanguageIn = ModelCodegenerationOptionsBase.SupportedLanguages.CSharp;
            }
            if (! this.SubfolderPerDomain )
            {
                SeparateFolderPerModelIn = false;
            }
            if (!this.SubfolderPerAggregate)
            {
                SeparateFolderPerAggregateIn = false;
            }

            if (! string.IsNullOrWhiteSpace (this.CodeRootFolder ))
            {
                if (System.IO.Directory.Exists(this.CodeRootFolder))
                {
                    DirectoryRootIn = new System.IO.DirectoryInfo(this.CodeRootFolder);
                }
                else
                {
                    // Can you make this into a relative path??
                    if (!System.IO.Path.IsPathRooted(this.DocumentationRootFolder))
                    {
                        DirectoryRootIn = new System.IO.DirectoryInfo(
                            System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory
                                , this.CodeRootFolder));
                    }
                }
            }
            else
            {
                DirectoryRootIn = new System.IO.DirectoryInfo(System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                    this.Name,
                    "Code"));
            }

            if (this.GenerateEntityFrameworkClasses )
            {
                GenerateEntityFrameworkClassesIn = true;
            }

            // Read any per-user overrides ??

            return ModelCodeGenerationOptions.Create(CodeLanguageIn,
                ConstructorPreferenceIn,
                DirectoryRootIn,
                SeparateFolderPerModelIn,
                SeparateFolderPerAggregateIn,
                GenerateEntityFrameworkClassesIn);
        }

        /// <summary>
        /// Get the code generation options that have been set at the model level
        /// </summary>
        public IDocumentationGenerationOptions GetDocumentationGenerationOptions()
        {
            System.IO.DirectoryInfo DirectoryRootIn = null;

            if (!string.IsNullOrWhiteSpace(this.DocumentationRootFolder))
            {
                if (System.IO.Directory.Exists(this.DocumentationRootFolder))
                {
                    DirectoryRootIn = new System.IO.DirectoryInfo(this.DocumentationRootFolder);
                }
                else
                {
                    // Can you make this into a relative path??
                    if (! System.IO.Path.IsPathRooted(this.DocumentationRootFolder) )
                    {
                        DirectoryRootIn = new System.IO.DirectoryInfo(
                            System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory
                                , this.DocumentationRootFolder));
                    }
                }
            }
            else
            {
                DirectoryRootIn = new System.IO.DirectoryInfo(System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                    this.Name,
                    "Documentation"));
            }

            return DocumentationGenerationOptions.Create(DirectoryRootIn);
        }

    }
}
