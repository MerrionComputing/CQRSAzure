using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;

namespace CQRSAzure.CQRSdsl.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    public partial class Classifier
        : IClassifierEntity 
    {

        /// <summary>
        /// The fully qualified name of this classifier
        /// </summary>
        public string FullyQualifiedName
        {
            get
            {
                return AggregateIdentifier.FullyQualifiedName + @"." + Name;
            }
        }

        // Validations to apply to the Classifier class:-
        // 1) The name must not be blank
        // 2) The name must be unique of all identifiers in the same aggregate identifier
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeNameAsValidIdentifier(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ClassifierNameNotBlank , nameof(Classifier) + " 01", this);
            }
            else
            {
                if (this.AggregateIdentifier.Classifiers.Count(f => f.Name == Name) > 1)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ClassifierNameUnique, nameof(Classifier) + " 02", this);
                }
            }
        }

    }
}
