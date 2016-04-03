using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    /// <summary>
    /// A parameter returned from a query
    /// </summary>
    public interface IQueryReturnParameter
        : INamedEntity, IDocumentedEntity
    {

        /// <summary>
        /// The data type of the parameter returned from a query
        /// </summary>
         PropertyDataType DataType { get; set; }

    }
}
