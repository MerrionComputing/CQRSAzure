using System;
using System.IO;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;


namespace CQRSAzure.CQRSdsl.Dsl
{
    public class DocumentationGenerationOptions
        : IDocumentationGenerationOptions
    {

        private readonly DirectoryInfo _directoryRoot;
        public DirectoryInfo DirectoryRoot
        {
            get
            {
                return _directoryRoot;
            }
        }

        private DocumentationGenerationOptions(
            System.IO.DirectoryInfo DirectoryRootIn)
        {
            
            _directoryRoot = DirectoryRootIn;

        }


        public static DocumentationGenerationOptions Create(System.IO.DirectoryInfo DirectoryRootIn)
        {
            return new Dsl.DocumentationGenerationOptions(DirectoryRootIn);
        }

    }
}
