using System;
using System.IO;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;
namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// The code generation options as stored in the CQRS model
    /// </summary>
    public class ModelCodeGenerationOptions
        : IModelCodeGenerationOptions
    {

        private readonly ModelCodegenerationOptionsBase.SupportedLanguages _codelanguage;
        public ModelCodegenerationOptionsBase.SupportedLanguages CodeLanguage
        {
            get
            {
                return _codelanguage;
            }
        }

        private readonly ModelCodegenerationOptionsBase.ConstructorPreferenceSetting _constructorPreference;
        public ModelCodegenerationOptionsBase.ConstructorPreferenceSetting ConstructorPreference
        {
            get
            {
                return _constructorPreference;
            }
        }

        private readonly DirectoryInfo _directoryRoot;
        public DirectoryInfo DirectoryRoot
        {
            get
            {
                return _directoryRoot;
            }
        }

        private readonly bool _separateFolderPerAggregate;
        public bool SeparateFolderPerAggregate
        {
            get
            {
                return _separateFolderPerAggregate;
            }
        }

        private readonly bool _separateFolderPerModel;
        public bool SeparateFolderPerModel
        {
            get
            {
                return _separateFolderPerModel;
            }
        }

        private readonly bool _generateEntityFrameworkClasses;
        public bool GenerateEntityFrameworkClasses
        {
            get
            {
                return _generateEntityFrameworkClasses;
            }
        }

        private ModelCodeGenerationOptions(
                    ModelCodegenerationOptionsBase.SupportedLanguages CodeLanguageIn,
                    ModelCodegenerationOptionsBase.ConstructorPreferenceSetting ConstructorPreferenceIn,
                    System.IO.DirectoryInfo DirectoryRootIn,
                    bool SeparateFolderPerModelIn,
                    bool SeparateFolderPerAggregateIn,
                    bool GenerateEntityFrameworkClassesIn = false )
        {

            _codelanguage = CodeLanguageIn;
            _constructorPreference = ConstructorPreferenceIn;
            _directoryRoot = DirectoryRootIn;
            _separateFolderPerModel = SeparateFolderPerModelIn;
            _separateFolderPerAggregate = SeparateFolderPerAggregateIn;

            _generateEntityFrameworkClasses = GenerateEntityFrameworkClassesIn;
        }


        public static IModelCodeGenerationOptions Create(ModelCodegenerationOptionsBase.SupportedLanguages CodeLanguageIn,
                    ModelCodegenerationOptionsBase.ConstructorPreferenceSetting ConstructorPreferenceIn,
                    System.IO.DirectoryInfo DirectoryRootIn,
                    bool SeparateFolderPerModelIn,
                    bool SeparateFolderPerAggregateIn,
                    bool GenerateEntityFrameworkClassesIn = false)
        {

            return new ModelCodeGenerationOptions(CodeLanguageIn,
                ConstructorPreferenceIn,
                DirectoryRootIn,
                SeparateFolderPerModelIn,
                SeparateFolderPerAggregateIn,
                GenerateEntityFrameworkClassesIn);

        }

        public static IModelCodeGenerationOptions Default()
        {
            return new ModelCodeGenerationOptions(ModelCodegenerationOptionsBase.SupportedLanguages.CSharp,
                ModelCodegenerationOptionsBase.ConstructorPreferenceSetting.GenerateBoth,
               new System.IO.DirectoryInfo(System.IO.Path.GetTempPath()),
               true,
               true, 
               false
               );

        }

    }
}
