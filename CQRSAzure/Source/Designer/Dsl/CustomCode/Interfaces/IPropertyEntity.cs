using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    /// <summary>
    /// Interface for an entity that describes a property (having a name and data type)
    /// </summary>
    public interface IPropertyEntity
        : INamedEntity , IDocumentedEntity 
    {

        /// <summary>
        /// The simple data type backing this property
        /// </summary>
        PropertyDataType DataType { get; set; }

    }
}
