using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace CQRSAzure.CQRSdsl.Dsl
{
    #region DiagramContext

    [Feature(Name = "Diagram Context", Container = typeof(DesignerContextProvider),
        Description = "A container for per-diagram features. An instance of this feature is created for each diagram opened.")]
    [CLSCompliant(false)]
    public class DiagramContext : FeatureGroup
    {
        private DiagramDocView m_diagramDocView;
        private IMenuCommandService m_diagramMenuService;

        public DiagramDocView DiagramDocView
        {
            [DebuggerStepThrough]
            get
            {
                Debug.Assert(m_diagramDocView != null);
                return m_diagramDocView;
            }
            [DebuggerStepThrough]
            internal set
            {
                Debug.Assert(value != null);
                m_diagramDocView = value;
            }
        }

        public VSDiagramView DiagramView
        {
            [DebuggerStepThrough]
            get { return this.DiagramDocView.CurrentDesigner; }
        }

        public DiagramClientView DiagramClientView
        {
            [DebuggerStepThrough]
            get { return this.DiagramView.DiagramClientView; }
        }

        public override object GetService(Type serviceType)
        {
            // Chain diagram's menu service before the parent service so that
            // commands register directly with the diagram's service.
            if (serviceType == typeof(IMenuCommandService))
            {
                if (m_diagramMenuService == null)
                    m_diagramMenuService = ((IServiceProvider) DiagramDocView).GetService(serviceType) as IMenuCommandService;

                Debug.Assert(m_diagramMenuService != null);
                if (m_diagramMenuService != null)
                    return m_diagramMenuService;
            }
            return base.GetService(serviceType);
        }
    }

    #endregion
}