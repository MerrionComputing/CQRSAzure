

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface ISnapshotSupport
    {

        /// <summary>
        /// Can we persist a snapshot of the state of this item?
        /// </summary>
        bool CanSnapshot { get; set; }

    }
}
