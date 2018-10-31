using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CQRSAzure.CQRSdsl.Dsl
{

    /// <summary>
    /// Store persisting information about meta-features and their relationships.
    /// </summary>
    public sealed class MetaFeatureStore : IMetaFeatureService, IDisposable
    {
        #region Private data, constructors

        private IServiceProvider m_serviceProvider;
        private Dictionary<Type, MetaFeature> m_metaFeatures = new Dictionary<Type, MetaFeature>();
        private Dictionary<MetaRelationshipKind, Dictionary<MetaFeature, List<MetaFeature>>> m_relationships = new Dictionary<MetaRelationshipKind, Dictionary<MetaFeature, List<MetaFeature>>>();
        private bool m_relationshipsDirty;

        internal MetaFeatureStore(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");
            this.m_serviceProvider = serviceProvider;
        }

        #endregion

        #region IDisposable implementation

        ~MetaFeatureStore()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            Debug.Assert(disposing, "Finalizing of FeatureStore without disposing!");
            if (disposing)
            {
                // TODO: Free other state (managed objects).
            }
            this.m_metaFeatures = null;
            this.m_relationships = null;
            this.m_serviceProvider = null;
        }

        #endregion

        #region IServiceProvider Members

        /// <summary>
        /// Gets service of specified type.
        /// </summary>
        /// <param name="serviceType">The type of the service to retreive</typeparam>
        /// <returns>Retrieved service object or null if not supported.</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IMetaFeatureService))
                return this;
            else
                return m_serviceProvider.GetService(serviceType);
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

        #endregion

        #region IMetaFeatureService Members

        /// <summary>
        /// Finds meta-feature corresponding to the specified runtime type.
        /// </summary>
        /// <param name="featureType">Runtime type.</param>
        /// <returns>Corresponding MetaFeature instance or null if not found.</returns>
        public MetaFeature FindMetaFeature(Type featureType)
        {
            MetaFeature metaFeature;
            m_metaFeatures.TryGetValue(featureType, out metaFeature);
            return metaFeature;
        }

        /// <summary>
        /// Adds specified meta-feature to the store.
        /// </summary>
        /// <param name="featureType">Type of feature to add.</param>
        public void AddMetaFeature(Type featureType)
        {
            if (featureType == null) throw new ArgumentNullException("featureType");

            FeatureAttribute attribute = ReflectionHelper.GetAttribute<FeatureAttribute>(featureType);
            if (attribute == null) throw new ArgumentException("featureType must have a FeatureAttribute or derived applied to it.");

            Type metaFeatureType = attribute.MetaFeature;
            Debug.Assert(metaFeatureType != null);

            ConstructorInfo constructor = metaFeatureType.GetConstructor(new Type[] { typeof(MetaFeatureStore), typeof(Type) });
            if (constructor == null) throw new ArgumentException("MetaFeature type must have a constructor taking MetaFeatureStore and Type arguments.");

            MetaFeature metaFeature = constructor.Invoke(new object[] { this, featureType }) as MetaFeature;

            Debug.Assert(!m_metaFeatures.ContainsKey(metaFeature.FeatureType));
            if (!m_metaFeatures.ContainsKey(metaFeature.FeatureType))
            {
                Debug.WriteLine("MetaFeatureStore: Adding meta-feature to the store: " + metaFeature.ToString());
                m_metaFeatures[metaFeature.FeatureType] = metaFeature;
                m_relationshipsDirty = true;
            }
        }

        /// <summary>
        /// Finds all runtime types marked with Feature or derived attributes in specified assembly
        /// and adds them as meta-features to the store if they satisfy provided filter.
        /// </summary>
        /// <param name="assembly">Assembly to look in for type.</param>
        /// <param name="typeFilter">Filter to apply to types.</param>
        [CLSCompliant(false)]
        public void AddMetaFeatures(Assembly assembly, Predicate<Type> typeFilter)
        {
            foreach (Type featureType in assembly.GetTypes())
            {
                if (!featureType.IsAbstract)
                {
                    FeatureAttribute attribute = ReflectionHelper.GetAttribute<FeatureAttribute>(featureType);
                    if (attribute != null && (typeFilter == null || typeFilter(featureType)))
                    {
                        AddMetaFeature(featureType);
                    }
                }
            }
        }

        /// <summary>
        /// Gets meta-features on the opposite side of relationship.
        /// </summary>
        /// <param name="metaFeature">MetaFeature end of relationship.</param>
        /// <param name="role">The role of returned meta-features with regards to the given meta-feature.</param>
        /// <returns>List of opposite role players.</returns>
        public IList<MetaFeature> GetRolePlayers(MetaFeature metaFeature, MetaRelationshipKind role)
        {
            if (metaFeature == null) throw new ArgumentNullException("feature");

            if (this.m_relationshipsDirty) RebuildRelationships();

            Dictionary<MetaFeature, List<MetaFeature>> s1;
            if (!m_relationships.TryGetValue(role, out s1))
                return new List<MetaFeature>();

            List<MetaFeature> result;
            if (!s1.TryGetValue(metaFeature, out result))
                return new List<MetaFeature>();

            return result;
        }

        #endregion

        #region Relationships calculation

        private void RebuildRelationships()
        {
            m_relationships.Clear();
            foreach (MetaFeature metaFeature in m_metaFeatures.Values)
            {
                if (metaFeature.FeatureType != typeof(GlobalContext))
                {
                    FeatureAttribute attr = ReflectionHelper.GetAttribute<FeatureAttribute>(metaFeature.FeatureType);
                    Debug.Assert(attr != null);
                    AddMetaRelationship(
                        metaFeature,
                        MetaRelationshipKind.Parent,
                        FindMetaFeature(attr.Container));
                }
            }
            m_relationshipsDirty = false;
        }

        private void AddMetaRelationship(MetaFeature feature1, MetaRelationshipKind kind, MetaFeature feature2)
        {
            if (feature1 == null) throw new ArgumentNullException("feature1");
            if (feature2 == null) throw new ArgumentNullException("feature2");
            if (feature1 == feature2) throw new InvalidOperationException();

            AddOneSideRelationship(feature1, kind, feature2);
            switch (kind)
            {
                case MetaRelationshipKind.Child:
                    kind = MetaRelationshipKind.Parent;
                    break;
                case MetaRelationshipKind.Dependants:
                    kind = MetaRelationshipKind.Depends;
                    break;
                case MetaRelationshipKind.Depends:
                    kind = MetaRelationshipKind.Dependants;
                    break;
                case MetaRelationshipKind.Parent:
                    kind = MetaRelationshipKind.Child;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            AddOneSideRelationship(feature2, kind, feature1);
        }

        private void AddOneSideRelationship(MetaFeature feature1, MetaRelationshipKind kind, MetaFeature feature2)
        {
            Dictionary<MetaFeature, List<MetaFeature>> s1;
            if (!m_relationships.TryGetValue(kind, out s1))
            {
                s1 = new Dictionary<MetaFeature, List<MetaFeature>>();
                m_relationships[kind] = s1;
            }
            List<MetaFeature> s2;
            if (!s1.TryGetValue(feature1, out s2))
            {
                s2 = new List<MetaFeature>();
                s1[feature1] = s2;
            }
            s2.Add(feature2);
        }

        #endregion

        #region Lifecycle services

        /// <summary>
        /// Installs all meta-features loaded into the store which are not already installed.
        /// </summary>
        public void Install()
        {
            foreach (MetaFeature metaFeature in m_metaFeatures.Values)
            {
                IFeatureInstaller installer = metaFeature.Installer;
                Debug.Assert(installer != null);
                if (installer != null && !installer.IsInstalled(metaFeature))
                {
                    installer.Install(metaFeature);
                    Debug.Assert(installer.IsInstalled(metaFeature));
                }
            }
        }

        /// <summary>
        /// Uninstalls all meta-features loaded into the store which are installed.
        /// </summary>
        public void Uninstall()
        {
            foreach (MetaFeature metaFeature in m_metaFeatures.Values)
            {
                IFeatureInstaller installer = metaFeature.Installer;
                Debug.Assert(installer != null);
                if (installer != null && installer.IsInstalled(metaFeature))
                {
                    installer.Uninstall(metaFeature);
                    // TODO: for some features this doesn't work - then we shouldn't call Uninstall!
                    // Debug.Assert(installer.IsInstalled(metaFeature));
                }
            }
        }

        #endregion
    }
}