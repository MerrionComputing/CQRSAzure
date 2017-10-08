using System;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Monitors running documents table and issues events when documents are opened or closed.
    /// </summary>
    [CLSCompliant(false)]
    public interface IMonitorDocumentsService
    {
        /// <summary>
        /// Raised just before a new document is about to show.
        /// </summary>
        event EventHandler<DocumentEventArgs> DocumentOpened;

        /// <summary>
        /// Raised after document has been closed.
        /// </summary>
        event EventHandler<DocumentEventArgs> DocumentClosed;

        /// <summary>
        /// Runs specified action for all opened documents in the running document table.
        /// </summary>
        /// <param name="action">Action taking DocData object as a single parameter.</param>
        void ForAllOpenedDocuments(Action<DocumentEventArgs> action);
    }
}