using System;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Interface which feature classes implement in order to receive meta-feature and context
    /// information on creation. If a feature only needs the service provider, it may choose to
    /// just provide a public constructor with IServiceProvider argument.
    /// </summary>
    public interface IFeature : IDisposable
    {
        /// <summary>
        /// Called on a feature object right after it has been created from the
        /// meta-feature information.
        /// </summary>
        /// <param name="metaFeature">Corresponding MetaFeature which created the feature.</param>
        /// <param name="serviceProvider">Feature context (service provider).</param>
        void Associate(MetaFeature metaFeature, ITypedServiceProvider serviceProvider);
    }

}