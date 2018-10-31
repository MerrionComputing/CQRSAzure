using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface ICommandParameterEntity
        : INamedEntity , IDocumentedEntity , ITargettedParameterEntity 
    {

        /// <summary>
        /// The data type underlying this command parameter
        /// </summary>
        PropertyDataType ParameterType { get; set; }


    }
}
