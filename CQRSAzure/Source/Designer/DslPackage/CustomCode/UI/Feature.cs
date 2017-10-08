using System;
using System.Diagnostics;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Base class for features not tied to other inhertiance hierarchies.
    /// </summary>
    public abstract class Feature : IFeature
    {
        private MetaFeature m_metaFeature;
        private ITypedServiceProvider m_serviceProvider;

        /// <summary>
        /// Called on a feature object right after it has been created from the
        /// meta-feature information.
        /// </summary>
        /// <param name="metaFeature">Corresponding MetaFeature which created the feature.</param>
        /// <param name="serviceProvider">Feature context (service provider).</param>
        public virtual void Associate(MetaFeature metaFeature, ITypedServiceProvider serviceProvider)
        {
            if (m_metaFeature != null || m_serviceProvider != null) throw new InvalidOperationException();

            if (metaFeature == null) throw new ArgumentNullException("metaFeature");
            m_metaFeature = metaFeature;

            // We do allow global context to have no parent context which is expected.
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");
            m_serviceProvider = serviceProvider;
        }

        ~Feature()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // This could happen: when user disconnects add-in
            // Debug.Assert(disposing, "Finalizing " + this.GetType().Name + " without disposing.");

            m_serviceProvider = null;
            m_metaFeature = null;

            Debug.WriteLine("Disposed feature of type: " + m_metaFeature);
        }

        /// <summary>
        /// Gets the meta-feature instance this feature was created from.
        /// </summary>
        public MetaFeature MetaFeature
        {
            [DebuggerStepThrough]
            get
            {
                Debug.Assert(m_metaFeature != null, "Accessing MetaFeature before Associate call.");
                return m_metaFeature;
            }
        }

        /// <summary>
        /// Gets context (IServiceProvider) to be used for service access by this feature.
        /// </summary>
        public ITypedServiceProvider ServiceProvider
        {
            [DebuggerStepThrough]
            get
            {
                Debug.Assert(m_serviceProvider != null, "Accessing Context before Associate call.");
                return m_serviceProvider;
            }
        }
    }

}