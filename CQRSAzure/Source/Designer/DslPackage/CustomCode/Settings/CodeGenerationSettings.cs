using System;
using CQRSAzure.CQRSdsl.CodeGeneration;


namespace CQRSAzure.CQRSdsl.CustomCode.Settings
{
    /// <summary>
    /// Settings that control how the code is generated from the CQRS model
    /// </summary>
    /// <remarks>
    /// Some settings are on a per user basis, but most are on a per-project basis
    /// </remarks>
    public class CodeGenerationSettings 
    {

        private ModelCodeGenerationOptions m_options;
        private  ModelCodeGenerationOptions Options
        {
            get
            {
                if (null == m_options)
                {
                    m_options = ModelCodeGenerationOptions.DefaultOptions();
                }
                return m_options;
            }
        }


        [SettingStorage("CodeGeneration", "CodeLanguage", SettingStorageAttribute.StorageTarget.SolutionSetting ) ]
        public ModelCodeGenerationOptions.SupportedLanguages CodeLanguage
        {
            get {
                    return Options.CodeLanguage;

            }
            set {
                Options.CodeLanguage = value;
            }
        }

        [SettingStorage("CodeGeneration", "RootDirectory", SettingStorageAttribute.StorageTarget.UserSetting )]
        public System.IO.DirectoryInfo DirectoryRoot
        {
            get
            {
                return Options.DirectoryRoot;
            }
            set
            {
                Options.DirectoryRoot = value;
            }
        }

        [SettingStorage("CodeGeneration", "ConstructorPreference", SettingStorageAttribute.StorageTarget.SolutionSetting)]
        public ModelCodeGenerationOptions.ConstructorPreferenceSetting ConstructorPreference
        {
            get { return Options.ConstructorPreference; }
            set { Options.ConstructorPreference = value; }
        }


        /// <summary>
        /// Convert these saved settings to a model generation option set for use by the code generation library
        /// </summary>
        public ModelCodeGenerationOptions ToModelCodeGenerationOptions()
        {
            return Options;
        }

        #region Constructors
        /// <summary>
        /// Empty constructor for serialisation 
        /// </summary>
        public CodeGenerationSettings()
        {

        }
        #endregion

    }
}
