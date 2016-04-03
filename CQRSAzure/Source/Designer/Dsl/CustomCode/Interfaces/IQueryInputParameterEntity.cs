using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface IQueryInputParameterEntity
        : INamedEntity, IDocumentedEntity, IPropertyEntity
    {


        /// <summary>
        /// Is this parameter the unique key of the aggregate over which to execute the query
        /// </summary>
        bool IsAggregateKey { get; set; }


    }
}

