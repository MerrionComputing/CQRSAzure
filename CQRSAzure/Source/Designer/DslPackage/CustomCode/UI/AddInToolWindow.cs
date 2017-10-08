using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using VsShell = Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Modeling.Shell;

namespace CQRSAzure.CQRSdsl.Dsl
{

    /// <summary>
    /// Common base class for tool windows created from add-ins.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    [CLSCompliant(false)]
    public abstract class AddInToolWindow : ToolWindow, IFeature
    {
        private MetaFeature m_metaFeature;
        private ITypedServiceProvider m_serviceProvider;
        private Control m_control;
        private bool m_disposed;

        protected AddInToolWindow(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Called on a feature object right after it has been created from the
        /// meta-feature information.
        /// </summary>
        /// <param name="metaFeature">Corresponding MetaFeature which created the feature.</param>
        /// <param name="serviceProvider">Feature context (service provider).</param>
        public void Associate(MetaFeature metaFeature, ITypedServiceProvider serviceProvider)
        {
            Debug.Assert(!m_disposed);

            if (metaFeature == null) throw new ArgumentNullException("metaFeature");
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");

            m_metaFeature = metaFeature;
            m_serviceProvider = serviceProvider;

            VsShell.IVsUIShell vsUIShell = m_serviceProvider.GetService<VsShell.IVsUIShell>();
            Debug.Assert(vsUIShell != null);
            if (vsUIShell != null)
            {
                Guid toolWindowID = this.GetType().GUID;
                int[] position = new int[1];
                VsShell.IVsWindowFrame frame;
                Guid emptyGuid = Guid.Empty;
                int hr = vsUIShell.CreateToolWindow(
                    (int) VsShell.__VSCREATETOOLWIN.CTW_fInitNew,
                    0,
                    this,
                    ref emptyGuid,
                    ref toolWindowID,
                    ref emptyGuid,
                    null,
                    m_metaFeature.Name,
                    position,
                    out frame);

                // If the add-in is reloaded, the CreateToolWindow method may fail because
                // previous tool window may not be destroyed correctly. We try to create
                // tool window again.
                if (ErrorHandler.Failed(hr))
                {
                    ErrorHandler.ThrowOnFailure(vsUIShell.CreateToolWindow(
                        (int) VsShell.__VSCREATETOOLWIN.CTW_fInitNew,
                        0,
                        this,
                        ref emptyGuid,
                        ref toolWindowID,
                        ref emptyGuid,
                        null,
                        m_metaFeature.Name,
                        position,
                        out frame));
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                Debug.Assert(disposing, "Finalizing a tool window without disposing.");
                if (disposing)
                {
                    if (!m_disposed)
                    {
                        VsShell.IVsWindowFrame frame = this.Frame;
                        if (frame != null)
                        {
                            ErrorHandler.ThrowOnFailure(frame.CloseFrame((uint) VsShell.__FRAMECLOSE.FRAMECLOSE_NoSave));
                        }

                        if (m_control != null)
                        {
                            m_control.Dispose();
                            m_control = null;
                        }

                        m_disposed = true;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Gets the meta-feature instance this feature was created from.
        /// </summary>
        public MetaFeature MetaFeature
        {
            [DebuggerStepThrough]
            get { return m_metaFeature; }
        }

        /// <summary>
        /// Gets context (IServiceProvider) to be used for service access by this feature.
        /// </summary>
        public new ITypedServiceProvider ServiceProvider
        {
            [DebuggerStepThrough]
            get { return m_serviceProvider; }
        }

        /// <summary>
        /// Gets whether this tool window should be created as a tabbed document window.
        /// </summary>
        protected virtual bool IsTabbedDocument
        {
            [DebuggerStepThrough]
            get { return false; }
        }

        /// <summary>
        /// Gets tool window title shown in UI.
        /// </summary>
        public override string WindowTitle
        {
            [DebuggerStepThrough]
            get
            {
                Debug.Assert(!m_disposed);
                if (m_metaFeature != null)
                    // the easy way (after Associate)
                    return m_metaFeature.Name;
                else
                    // the hard way (before)
                    return ReflectionHelper.GetAttribute<FeatureAttribute>(this.GetType()).Name;
            }
        }

        /// <summary>
        /// Gets the control shown in the tool window.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override IWin32Window Window
        {
            [DebuggerStepThrough]
            get
            {
                if (m_control == null && !m_disposed)
                {
                    m_control = CreateControl();
                }
                return m_control;
            }
        }

        protected override void OnToolWindowCreate()
        {

            Debug.Assert(!m_disposed);

            base.OnToolWindowCreate();  

            if (!m_disposed && this.IsTabbedDocument)
            {
                // make this a tabbed document window
                Debug.Assert(this.Frame != null);
                if (this.Frame != null)
                {
                    ErrorHandler.ThrowOnFailure(this.Frame.SetProperty(
                        (int) VsShell.__VSFPROPID.VSFPROPID_FrameMode,
                        VsShell.VSFRAMEMODE.VSFM_MdiChild));
                }
            }
        }

        /// <summary>
        /// Called by the Window property to create control to be show in the window.
        /// </summary>
        /// <returns>Created control instance.</returns>
        protected abstract Control CreateControl();
    }

    /// <summary>
    /// Common base class for tool windows creeated from add-ins parameterized with the used control class.
    /// </summary>
    /// <typeparam name="TControl">Control showing in the tool window.</typeparam>
    [CLSCompliant(false)]
    public abstract class AddInToolWindow<TControl> : AddInToolWindow
        where TControl : Control, new()
    {
        protected AddInToolWindow(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        protected override Control CreateControl()
        {
            return new TControl();
        }
    }
}