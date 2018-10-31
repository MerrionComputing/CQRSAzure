using Microsoft.VisualStudio;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using VsShell = Microsoft.VisualStudio.Shell.Interop;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Arguments for document events.
    /// </summary>
    public class DocumentEventArgs : EventArgs
    {
        private VsShell.IVsRunningDocumentTable m_rdt;
        private uint m_docCookie;
        private object m_docData;

        [CLSCompliant(false)]
        public DocumentEventArgs(VsShell.IVsRunningDocumentTable rdt, uint docCookie)
        {
            if (rdt == null) throw new ArgumentNullException("rdt");
            if (docCookie <= 0) throw new ArgumentOutOfRangeException("docCookie");

            m_rdt = rdt;
            m_docCookie = docCookie;
        }

        [CLSCompliant(false)]
        public uint DocCookie
        {
            [DebuggerStepThrough]
            get { return m_docCookie; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public object DocData
        {
            [DebuggerStepThrough]
            get
            {
                if (m_docData == null)
                {
                    uint rdtFlags, readLocks, editLocks, itemID;
                    string itemName;
                    VsShell.IVsHierarchy hierarchy;
                    IntPtr punkDocData = IntPtr.Zero;
                    try
                    {
                        if (ErrorHandler.Succeeded(m_rdt.GetDocumentInfo(m_docCookie, out rdtFlags, out readLocks, out editLocks, out itemName, out hierarchy, out itemID, out punkDocData)))
                        {
                            m_docData = Marshal.GetObjectForIUnknown(punkDocData);
                        }
                    }
                    finally
                    {
                        if (punkDocData != IntPtr.Zero)
                            Marshal.Release(punkDocData);
                    }
                }
                return m_docData;
            }
        }
    }
}