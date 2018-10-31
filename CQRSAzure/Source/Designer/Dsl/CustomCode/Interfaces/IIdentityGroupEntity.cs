
namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    /// <summary>
    /// Interface for a class that represents a group of related instances of an aggregate identifier
    /// </summary>
    public interface IIdentityGroupEntity
        : IDocumentedEntity, INamedEntity, ICategorisedEntity, ISnapshotSupport 
    {

        /// <summary>
        /// The name of the parent identity group of which this group is a wholy contained subset
        /// </summary>
        string ParentName { get; set; }
    }
}
