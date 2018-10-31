using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface IClassifierPropertyEvaluationEntity
        : IDocumentedEntity
    {

        /// <summary>
        /// The property that is to be evaluated
        /// </summary>
        System.String SourceEventPropertyName { get; set; }

        /// <summary>
        /// The evaluation to perform on the property
        /// </summary>
        PropertyEvaluation PropertyEvaluationToPerform { get; set; }

        /// <summary>
        /// What the property evaluating to true means to the identity group
        /// </summary>
        IdentityGroupClassification OnTrue { get; set; }

        /// <summary>
        /// What the property evaluating to false means to the identity group
        /// </summary>
        IdentityGroupClassification OnFalse { get; set; }

        /// <summary>
        /// What the property value is being evaluated against
        /// </summary>
        System.String Target { get; set; }

        /// <summary>
        /// What is represented by the evaluation target
        /// </summary>
        EvaluationTargetType TargetType { get; set; }


    }
}
