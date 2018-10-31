using System.Diagnostics;

namespace CQRSAzure.CQRSdsl.Dsl
{
    #region FeatureInstaller

    /// <summary>
    /// Provides an abstraction on meta-feature installation functionality.
    /// </summary>
    public interface IFeatureInstaller
    {
        /// <summary>
        /// Installs the meta-feature into the product.
        /// </summary>
        /// <param name="metaFeature">Meta-feature to install.</param>
        void Install(MetaFeature metaFeature);

        /// <summary>
        /// Checks whether given meta-feature is already installed.
        /// </summary>
        /// <param name="metaFeature">Meta-feature.</param>
        /// <returns>True if installed and false otherwise.</returns>
        bool IsInstalled(MetaFeature metaFeature);

        /// <summary>
        /// Uninstalls specified meta-feature from the product.
        /// </summary>
        /// <param name="metaFeature">Meta-feature to uninstall.</param>
        void Uninstall(MetaFeature metaFeature);
    }

    /// <summary>
    /// Default implementation of the IFeatureInstaller interface which claims that
    /// a feature is always installed and does nothing on Install/Uninstall calls.
    /// </summary>
    internal sealed class DefaultFeatureInstaller : IFeatureInstaller
    {
        private DefaultFeatureInstaller() { }

        private static DefaultFeatureInstaller s_instance;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static DefaultFeatureInstaller Instance
        {
            [DebuggerStepThrough]
            get
            {
                if (s_instance == null)
                    s_instance = new DefaultFeatureInstaller();
                return s_instance;
            }
        }

        public void Install(MetaFeature metaFeature)
        {
            Debug.Assert(metaFeature != null);
            Debug.Assert(!this.IsInstalled(metaFeature));
        }

        public bool IsInstalled(MetaFeature metaFeature)
        {
            Debug.Assert(metaFeature != null);
            return true;
        }

        public void Uninstall(MetaFeature metaFeature)
        {
            Debug.Assert(metaFeature != null);
        }
    }

    #endregion
}