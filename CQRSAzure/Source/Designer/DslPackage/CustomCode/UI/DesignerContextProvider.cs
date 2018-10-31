using Microsoft.VisualStudio.Modeling.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CQRSAzure.CQRSdsl.Dsl
{
    #region DesignerContextProvider

    [Feature(Name = "Designer Context Provider", Container = typeof(GlobalContext),
        Description = "This feature watches active documents and instantiates designer and diagram contexts as new diagrams are opened.")]
    public class DesignerContextProvider : Feature
    {
        private IMonitorDocumentsService m_monitorDocumentsService;
        private IFeature m_designerContext;
        private Dictionary<uint, DiagramContext> m_diagramContexts;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<uint, DiagramContext> DiagramContexts
        {
            [DebuggerStepThrough]
            get
            {
                if (m_diagramContexts == null)
                    m_diagramContexts = new Dictionary<uint, DiagramContext>();
                return m_diagramContexts;
            }
        }

        public override void Associate(MetaFeature metaFeature, ITypedServiceProvider serviceProvider)
        {
            base.Associate(metaFeature, serviceProvider);

            m_monitorDocumentsService = serviceProvider.GetService<IMonitorDocumentsService>();
            Debug.Assert(m_monitorDocumentsService != null);
            if (m_monitorDocumentsService != null)
            {
                m_monitorDocumentsService.ForAllOpenedDocuments(delegate (DocumentEventArgs args)
                {
                    OnDocumentOpened(m_monitorDocumentsService, args);
                });
                m_monitorDocumentsService.DocumentOpened += OnDocumentOpened;
                m_monitorDocumentsService.DocumentClosed += OnDocumentClosed;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void Dispose(bool disposing)
        {
            Exception disposeException = null;
            try
            {
                if (disposing)
                {
                    if (m_monitorDocumentsService != null)
                    {
                        m_monitorDocumentsService.DocumentOpened -= OnDocumentOpened;
                        m_monitorDocumentsService.DocumentClosed -= OnDocumentClosed;
                        m_monitorDocumentsService = null;
                    }

                    if (m_diagramContexts != null)
                    {
                        foreach (DiagramContext diagramContext in m_diagramContexts.Values)
                        {
                            try
                            {
                                diagramContext.Dispose();
                            }
                            catch (Exception ex)
                            {
                                disposeException = ex;
                            }
                        }
                        m_diagramContexts = null;
                    }

                    if (m_designerContext != null)
                    {
                        try
                        {
                            m_designerContext.Dispose();
                            m_designerContext = null;
                        }
                        catch (Exception ex)
                        {
                            disposeException = ex;
                        }
                    }
                }
                else
                {
                    m_monitorDocumentsService = null;
                    m_designerContext = null;
                    m_diagramContexts = null;
                }
            }
            finally
            {
                base.Dispose(disposing);

                // rethrow dispose exception if any
                if (disposeException != null)
                    throw disposeException;
            }
        }

        private void OnDocumentOpened(object sender, DocumentEventArgs args)
        {
            if (args != null && IsDocDataSupported(args.DocData))
            {
                if (m_designerContext == null)
                {
                    m_designerContext = CreateDesignerContext();
                }

                DiagramContext diagramContext;
                if (this.DiagramContexts.TryGetValue(args.DocCookie, out diagramContext))
                {
                    Debug.Fail("Diagram context already existed - disposing it.");
                    this.DiagramContexts.Remove(args.DocCookie);
                    diagramContext.Dispose();
                }

                diagramContext = CreateDiagramContext(args.DocData);
                Debug.Assert(diagramContext != null);
                if (diagramContext != null)
                {
                    MetaFeature diagramMetaFeature = this.MetaFeature.Store.FindMetaFeature(diagramContext.GetType());
                    if (diagramMetaFeature != null)
                    {
                        diagramContext.DiagramDocView = ((ModelingDocData) args.DocData).DocViews[0] as DiagramDocView;
                        diagramContext.Associate(diagramMetaFeature, this.ServiceProvider);
                        this.DiagramContexts[args.DocCookie] = diagramContext;
                    }
                    else
                    {
                        diagramContext.Dispose();
                    }
                }
            }
        }

        private void OnDocumentClosed(object sender, DocumentEventArgs args)
        {
            if (args != null && IsDocDataSupported(args.DocData))
            {
                DiagramContext diagramContext;
                if (this.DiagramContexts.TryGetValue(args.DocCookie, out diagramContext))
                {
                    this.DiagramContexts.Remove(args.DocCookie);
                    diagramContext.Dispose();
                }
            }
        }

        protected virtual bool IsDocDataSupported(object docData)
        {
            return docData is ModelingDocData;
        }

        protected virtual DesignerContext CreateDesignerContext()
        {
            IFeatureContext featureContext = this.ServiceProvider.GetService<IFeatureContext>();
            Debug.Assert(featureContext != null);
            if (featureContext != null)
            {
                return featureContext.CreateFeature<DesignerContext>();
            }
            return null;
        }

        [CLSCompliant(false)]
        protected virtual DiagramContext CreateDiagramContext(object docData)
        {
            return new DiagramContext();
        }
    }

    #endregion
}