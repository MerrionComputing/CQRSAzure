using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface IEventPropertyEntity
        : IDocumentedEntity, IPropertyEntity
    {
        
        /// <summary>
        /// Does this property provide the effective date part of the event 
        /// </summary>
        /// <remarks>
        /// This can be used where "as-of" queries are run to give a notice of when the 
        /// event actually occured
        /// </remarks>
        bool IsEffectiveDate { get; set; }

    }
}
