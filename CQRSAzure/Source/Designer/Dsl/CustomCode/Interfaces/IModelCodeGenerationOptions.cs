namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    /// <summary>
    /// Interface for storing the code generation options inside a DSL model
    /// </summary>
    /// <remarks>
    /// These may be overridden by a build process or by user specific settings in the code generation 
    /// project
    /// </remarks>
    public interface IModelCodeGenerationOptions
    {

        /// <summary>
        /// The language in which to generate the source code files
        /// </summary>
        ModelCodegenerationOptionsBase.SupportedLanguages CodeLanguage { get; }

        /// <summary>
        /// How to create constructiors for the classes generated
        /// </summary>
        ModelCodegenerationOptionsBase.ConstructorPreferenceSetting ConstructorPreference { get; }

        /// <summary>
        /// The root directory into which code is to be generated
        /// </summary>
        System.IO.DirectoryInfo DirectoryRoot { get; }

        /// <summary>
        /// Should each model be put in its own code subdirectory?
        /// </summary>
        bool SeparateFolderPerModel { get; }

        /// <summary>
        /// Should each aggregate be put in its own code subdirectory?
        /// </summary>
        bool SeparateFolderPerAggregate { get; }
    }


    public static class ModelCodegenerationOptionsBase
    {
        /// <summary>
        /// The languages that the code generation can be done in
        /// </summary>
        /// <remarks>
        /// If additional languages are supported (F# , C++ maybe?) they should be added here
        /// </remarks>
        public enum SupportedLanguages
        {
            /// <summary>
            /// Code generation in VB.Net
            /// </summary>
            VBNet = 0,
            /// <summary>
            /// Code generation in C Sharp
            /// </summary>
            CSharp = 1
        }


        /// <summary>
        /// What type of constructor to use in the generated classes
        /// </summary>
        /// <remarks>
        /// This can be a per-model rather than per-user preference 
        /// </remarks>
        public enum ConstructorPreferenceSetting
        {
            /// <summary>
            /// (Default) Generate both an interface derived constructor and a parameters derived constructor
            /// </summary>
            GenerateBoth = 0,
            /// <summary>
            /// Only generate an interface derived constructor
            /// </summary>
            InterfaceOnly = 1,
            /// <summary>
            /// Only derive a parameters derived constructor
            /// </summary>
            ParametersOnly = 2
        }
    }
}
