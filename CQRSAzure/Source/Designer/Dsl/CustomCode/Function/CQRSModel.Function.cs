
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
        /// Thers emay be overridden on a per-user basis as part of the build process
        /// </remarks>
        public IModelCodeGenerationOptions GetCodeGenerationOptions()
        {
            ModelCodegenerationOptionsBase.SupportedLanguages CodeLanguageIn = ModelCodegenerationOptionsBase.SupportedLanguages.VBNet ;
            ModelCodegenerationOptionsBase.ConstructorPreferenceSetting ConstructorPreferenceIn = ModelCodegenerationOptionsBase.ConstructorPreferenceSetting.GenerateBoth ;
            System.IO.DirectoryInfo DirectoryRootIn = null;
            bool SeparateFolderPerModelIn = true ;
            bool SeparateFolderPerAggregateIn = true ;

            // Read the model properties that affect code generation
            

            return ModelCodeGenerationOptions.Create(CodeLanguageIn,
                ConstructorPreferenceIn,
                DirectoryRootIn,
                SeparateFolderPerModelIn,
                SeparateFolderPerAggregateIn);
        }

    }
}
