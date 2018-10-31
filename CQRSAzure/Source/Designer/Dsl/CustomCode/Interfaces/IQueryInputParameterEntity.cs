using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface IQueryInputParameterEntity
        : INamedEntity, 
        IDocumentedEntity, 
        IPropertyEntity,
        ITargettedParameterEntity
    {


    }
}

