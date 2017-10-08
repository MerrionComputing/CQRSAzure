
namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    /// <summary>
    /// An entity that can be persisted to a point-in-time snapshot
    /// </summary>
    public interface ISnapshotableEntity
    {

        /// <summary>
        /// Can this entity be persisted to a snapshot safely or must it be rebuilt from scratch?
        /// </summary>
        bool CanSnapshot { get; set; }

    }
}
