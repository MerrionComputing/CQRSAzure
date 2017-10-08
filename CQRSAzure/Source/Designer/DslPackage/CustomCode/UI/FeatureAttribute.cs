using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace CQRSAzure.CQRSdsl.Dsl
{

    #region FeatureAttribute

    /// <summary>
    /// Applying this attribute on a class makes that class a feature.
    /// A feature class must implement IDisposable interface.
    /// It can also optionally implement IFeature interface to receive context and meta-feature information on creation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
    public class FeatureAttribute : Attribute
    {
        private string m_name;
        private string m_description;
        private Type m_container;

        /// <summary>
        /// User-friendly name of the feature.
        /// </summary>
        public string Name
        {
            [DebuggerStepThrough]
            get { return m_name; }
            [DebuggerStepThrough]
            set { m_name = value; }
        }

        /// <summary>
        /// User-friendly description of the feature.
        /// </summary>
        public string Description
        {
            [DebuggerStepThrough]
            get { return m_description != null ? m_description : string.Empty; }
            [DebuggerStepThrough]
            set { m_description = value; }
        }

        /// <summary>
        /// The type of container meta-feature (if missing, GlobalContext is assumed).
        /// </summary>
        public Type Container
        {
            [DebuggerStepThrough]
            get { return m_container != null ? m_container : typeof(GlobalContext); }
            [DebuggerStepThrough]
            set { m_container = value; }
        }

        /// <summary>
        /// The type of corresponding MetaFeature class (default is MetaFeature class itself).
        /// </summary>
        public virtual Type MetaFeature
        {
            [DebuggerStepThrough]
            get { return typeof(MetaFeature); }
        }
    }

    #endregion
}