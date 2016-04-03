
using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface IAggregateIdentifierEntity
        : IDocumentedEntity, INamedEntity, ICategorisedEntity
    {

        /// <summary>
        /// The business-meaningful name by which the unique key of this aggregate is refered to
        /// </summary>
        string KeyName { get; set; }

        /// <summary>
        /// The data type used to store the unique identifier for this aggregate
        /// </summary>
        KeyDataType KeyDataType { get; set; }

    }
}
