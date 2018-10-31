using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CQRSAzure.CQRSdsl.Dsl
{

    #region FeatureGroup

    /// <summary>
    /// Base class for features which contain other features.
    /// </summary>
    public abstract class FeatureGroup : Feature, IFeatureContext
    {
        private Dictionary<MetaFeature, IDisposable> m_features;

        /// <summary>
        /// Called on a feature object right after it has been created from the
        /// meta-feature information.
        /// </summary>
        /// <param name="metaFeature">Corresponding MetaFeature which created the feature.</param>
        /// <param name="serviceProvider">Feature context (service provider).</param>
        public override void Associate(MetaFeature metaFeature, ITypedServiceProvider serviceProvider)
        {
            base.Associate(metaFeature, serviceProvider);

            IList<MetaFeature> children = metaFeature.Store.GetRolePlayers(metaFeature, MetaRelationshipKind.Child);
            Debug.Assert(children != null);
            if (children != null)
            {
                foreach (MetaFeature childMetaFeature in children)
                    if (childMetaFeature.Enabled)
                        this.CreateFeature(childMetaFeature.FeatureType);
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
                    if (m_features != null)
                    {
                        foreach (KeyValuePair<MetaFeature, IDisposable> e in m_features)
                        {
                            try
                            {
                                e.Value.Dispose();
                            }
                            catch (Exception ex)
                            {
                                disposeException = ex;
                            }
                        }
                        m_features = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);

                if (disposeException != null)
                    throw disposeException;
            }
        }

        /// <summary>
        /// Gets service of specified type.
        /// </summary>
        /// <param name="serviceType">The type of the service to retreive</typeparam>
        /// <returns>Retrieved service object or null if not supported.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public virtual object GetService(Type serviceType)
        {
            if (serviceType == typeof(IFeatureContext))
                return this;
            else
                return this.ServiceProvider.GetService(serviceType);
        }

        /// <summary>
        /// Gets service of specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service to retreive</typeparam>
        /// <returns>Retrieved service object or null if not supported.</returns>
        public T GetService<T>() where T : class
        {
            return this.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Searches for a feature of specified type in the current context only
        /// without looking up the context chain.
        /// </summary>
        /// <param name="featureType">Type of the feature to find.</param>
        /// <returns>Feature instance of found and null otherwise.</returns>
        public virtual object FindFeature(Type featureType)
        {
            if (m_features != null)
            {
                MetaFeature metaFeature = MetaFeature.Store.FindMetaFeature(featureType);
                if (metaFeature == null)
                    throw new ArgumentException("Meta-feature corrsponding to " + featureType + " was not found in the store.");

                IDisposable feature;
                if (m_features.TryGetValue(metaFeature, out feature))
                    return feature;
            }
            return null;
        }

        /// <summary>
        /// Searches for a feature of specified type in the current context only
        /// without looking up the context chain.
        /// </summary>
        /// <typeparam name="T">Type of the feature to find.</param>
        /// <returns>Feature instance of found and null otherwise.</returns>
        public T FindFeature<T>() where T : class
        {
            return this.FindFeature(typeof(T)) as T;
        }

        /// <summary>
        /// Finds or if not found, creates an feature of specified type from the meta-store.
        /// </summary>
        /// <param name="featureType">Type of the feature to load.</param>
        /// <returns>Created feature.</returns>
        public virtual object CreateFeature(Type featureType)
        {
            if (m_features == null)
                m_features = new Dictionary<MetaFeature, IDisposable>();

            MetaFeature metaFeature = MetaFeature.Store.FindMetaFeature(featureType);
            if (metaFeature == null)
                throw new ArgumentException("Meta-feature corrsponding to " + featureType + " was not found in the store.");

            if (!metaFeature.Enabled)
                return null;

            IDisposable feature;
            if (m_features.TryGetValue(metaFeature, out feature))
                return feature;
            else
                feature = metaFeature.CreateFeature(this);

            m_features[metaFeature] = feature;

            Debug.Assert(feature != null);
            return feature;
        }

        /// <summary>
        /// Finds or if not found, creates an feature of specified type from the meta-store.
        /// </summary>
        /// <typeparam name="T">Type of the feature to load.</param>
        /// <returns>Created feature.</returns>
        public T CreateFeature<T>() where T : class
        {
            return this.CreateFeature(typeof(T)) as T;
        }
    }

    #endregion


}