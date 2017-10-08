

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    /// <summary>
    /// Options to control how the documentation for any given 
    /// </summary>
    public interface IDocumentationGenerationOptions
    {

        /// <summary>
        /// The root directory into which documentation is to be generated
        /// </summary>
        System.IO.DirectoryInfo DirectoryRoot { get; }

    }
}
