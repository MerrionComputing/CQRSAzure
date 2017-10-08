
namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    /// <summary>
    /// Interface for a class that supports a classifier for an identity group
    /// </summary>
    public interface IClassifierEntity
        : IDocumentedEntity, INamedEntity, ICategorisedEntity, ISnapshotSupport 
    {

        Dsl.ClassifierDataSourceType DataSourceType { get; set; }

    }
}
