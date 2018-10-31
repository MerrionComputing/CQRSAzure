using System;

namespace CQRSAzure.CQRSdsl.Dsl
{
    #region ITypedServiceProvider

    public interface ITypedServiceProvider : IServiceProvider
    {
        /// <summary>
        /// Gets service of specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service to retreive</typeparam>
        /// <returns>Retrieved service object or null if not supported.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        T GetService<T>() where T : class;
    }

    #endregion
}