using System;

namespace CQRSAzure.CQRSdsl.Dsl
{

    #region IFeatureContext

    /// <summary>
    /// Provides environment context to a loaded feature.
    /// </summary>
    public interface IFeatureContext : ITypedServiceProvider
    {
        /// <summary>
        /// Searches for a feature of specified type in the current context only
        /// without looking up the context chain.
        /// </summary>
        /// <param name="featureType">Type of the feature to find.</param>
        /// <returns>Feature instance of found and null otherwise.</returns>
        object FindFeature(Type featureType);

        /// <summary>
        /// Searches for a feature of specified type in the current context only
        /// without looking up the context chain.
        /// </summary>
        /// <typeparam name="T">Type of the feature to find.</param>
        /// <returns>Feature instance of found and null otherwise.</returns>
        T FindFeature<T>() where T : class;

        /// <summary>
        /// Finds or if not found, creates an feature of specified type from the meta-store.
        /// </summary>
        /// <param name="featureType">Type of the feature to load.</param>
        /// <returns>Created feature.</returns>
        object CreateFeature(Type featureType);

        /// <summary>
        /// Finds or if not found, creates an feature of specified type from the meta-store.
        /// </summary>
        /// <typeparam name="T">Type of the feature to load.</param>
        /// <returns>Created feature.</returns>
        T CreateFeature<T>() where T : class;
    }

    #endregion


}