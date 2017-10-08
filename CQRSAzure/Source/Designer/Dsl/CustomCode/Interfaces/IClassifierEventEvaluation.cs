using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface IClassifierEventEvaluationEntity
        : IDocumentedEntity, IClassifierPropertyEvaluationEntity 
    {

        /// <summary>
        /// The name of the event the classifier is handling
        /// </summary>
        System.String EventName { get; set; }

    }
}
