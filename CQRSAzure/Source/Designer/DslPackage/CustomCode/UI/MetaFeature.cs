using System;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Encapsulates data and functionality required to install and instantiate a feature.
    /// </summary>
    /// <remarks>
    /// This is not an abstract class and thus can be used to represent features which don't require
    /// more data or special installation besides that provided here.
    /// </remarks>
    public class MetaFeature
    {
        private IMetaFeatureService m_store;
        private Type m_featureType;
        private FeatureAttribute m_featureData;
        private bool m_enabled = true;

        [CLSCompliant(false)]
        public MetaFeature(IMetaFeatureService store, Type featureType)
        {
            if (store == null) throw new ArgumentNullException("store");
            m_store = store;

            if (featureType == null) throw new ArgumentNullException("featureType");
            m_featureType = featureType;

            if (m_featureType.GetInterfaceMap(typeof(IDisposable)).InterfaceType == null)
                throw new ArgumentException("featureType runtime type must implement the System.IDisposable interface to be used as a feature type.");

            m_featureData = ReflectionHelper.GetAttribute<FeatureAttribute>(m_featureType);
            if (m_featureData == null)
                throw new ArgumentException("featureType must have a FeatureAttribute or derived applied.");

            Debug.Assert(m_featureData.MetaFeature == this.GetType());
        }

        /// <summary>
        /// Gets meta-feature store this meta-feature belongs to.
        /// </summary>
        [CLSCompliant(false)]
        public IMetaFeatureService Store
        {
            [DebuggerStepThrough]
            get { return m_store; }
        }

        /// <summary>
        /// Gets runtime type of the feature class which is created when feature is instantiated.
        /// </summary>
        public Type FeatureType
        {
            [DebuggerStepThrough]
            get { return m_featureType; }
        }

        /// <summary>
        /// Gets the name of the feature to displayed in UI.
        /// </summary>
        /// <remarks>
        /// Default feature name is the name of its runtime type.
        /// </remarks>
        public virtual string Name
        {
            [DebuggerStepThrough]
            get { return m_featureData.Name != null ? m_featureData.Name : this.FeatureType.Name; }
        }

        /// <summary>
        /// Gets user-friendly description of the feature.
        /// </summary>
        public virtual string Description
        {
            [DebuggerStepThrough]
            get { return m_featureData.Description; }
        }

        /// <summary>
        /// Gets meta-feature within which this feature is contained.
        /// </summary>
        /// <remarks>
        /// At runtime parent feature is created before child feature is created and controls the lifetime
        /// of its child features. When parent is disposed, all its children are disposed as well.
        /// </remarks>
        public virtual Type ContainerMetaFeature
        {
            [DebuggerStepThrough]
            get { return m_featureData.Container; }
        }

        /// <summary>
        /// Gets installer capable of installing/uninstalling this meta-feature at runtime.
        /// </summary>
        public virtual IFeatureInstaller Installer
        {
            [DebuggerStepThrough]
            get { return DefaultFeatureInstaller.Instance; }
        }

        /// <summary>
        /// Gets or sets whether this feature is enabled.
        /// </summary>
        public bool Enabled
        {
            [DebuggerStepThrough]
            get { return m_enabled; }
            [DebuggerStepThrough]
            set { m_enabled = value; }
        }

        /// <summary>
        /// Creates feature of this meta-feature type in specified context.
        /// </summary>
        /// <param name="serviceProvider">The context in which the new feature is created.</param>
        /// <returns>Created feature.</returns>
        /// <remarks>
        /// Note that even though feature might choose to implement IFeature interface, this is not
        /// a requirement (whereas IDisposable is a requirement).
        /// </remarks>
        public IDisposable CreateFeature(ITypedServiceProvider serviceProvider)
        {

            IDisposable disposableFeature = this.DoCreateFeature(serviceProvider);
            Debug.Assert(disposableFeature != null);

            IFeature feature = disposableFeature as IFeature;
            if (feature != null)
            {
                feature.Associate(this, serviceProvider);
            }

            Debug.WriteLine("Created feature " + this + ".");
            return disposableFeature;
        }

        /// <summary>
        /// Does the actual work of instantiating feature object before Associate is called
        /// from CreateFeature (if required).
        /// </summary>
        /// <param name="serviceProvider">Context the feature is being created in.</param>
        /// <returns>Created feature object.</returns>
        protected virtual IDisposable DoCreateFeature(ITypedServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");

            // It is relatively common for types to take a IServiceProvider interface as an argument,
            // so we do provide a special case here for such situations (tool-windows and dialogs are
            // good examples of this).
            ConstructorInfo spConstructor = m_featureType.GetConstructor(new Type[] { typeof(IServiceProvider) });
            if (spConstructor != null)
            {
                return spConstructor.Invoke(new object[] { serviceProvider }) as IDisposable;
            }
            else
            {
                // If failed, assume the type has a defalut constructor.
                return Activator.CreateInstance(m_featureType) as IDisposable;
            }
        }

        /// <summary>
        /// Gets string representation of this instance.
        /// </summary>
        /// <returns>User-friendly string description of this meta-feature.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0} '{1}' ({2})",
                this.GetType().Name,
                this.m_featureData.Name,
                m_featureType.Name);
        }
    }
}