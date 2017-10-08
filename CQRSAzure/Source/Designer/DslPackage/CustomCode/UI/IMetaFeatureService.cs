using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSAzure.CQRSdsl.Dsl
{


    #region MetaRealtionshipKind
    public enum MetaRelationshipKind
    {
        Depends,
        Dependants,
        Parent,
        Child,
    }
    #endregion

    /// <summary>
    /// Meta-feature services provider.
    /// </summary>
    [CLSCompliant(false)]
    public interface IMetaFeatureService : IServiceProvider
    {
        /// <summary>
        /// Gets service of specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service to retreive</typeparam>
        /// <returns>Retrieved service object or null if not supported.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        T GetService<T>() where T : class;

        /// <summary>
        /// Adds specified meta-feature to the store.
        /// </summary>
        /// <param name="featureType">Type of feature to add.</param>
        void AddMetaFeature(Type featureType);

        /// <summary>
        /// Finds all runtime types marked with Feature or derived attributes in specified assembly
        /// and adds them as meta-features to the store if they satisfy provided filter.
        /// </summary>
        /// <param name="assembly">Assembly to look in for type.</param>
        /// <param name="typeFilter">Filter to apply to types.</param>
        void AddMetaFeatures(Assembly assembly, Predicate<Type> typeFilter);

        /// <summary>
        /// Finds meta-feature corresponding to the specified runtime type.
        /// </summary>
        /// <param name="featureType">Runtime type.</param>
        /// <returns>Corresponding MetaFeature instance or null if not found.</returns>
        MetaFeature FindMetaFeature(Type featureType);

        /// <summary>
        /// Gets meta-features on the opposite side of relationship.
        /// </summary>
        /// <param name="metaFeature">MetaFeature end of relationship.</param>
        /// <param name="role">The role of returned meta-features with regards to the given meta-feature.</param>
        /// <returns>List of opposite role players.</returns>
        IList<MetaFeature> GetRolePlayers(MetaFeature metaFeature, MetaRelationshipKind role);
    }

}