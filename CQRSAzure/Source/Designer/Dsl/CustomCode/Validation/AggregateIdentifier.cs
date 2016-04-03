using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.CSharp;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;

namespace CQRSAzure.CQRSdsl.Dsl
{
    [ValidationState(ValidationState.Enabled ) ]
    public partial class AggregateIdentifier
        : IAggregateIdentifierEntity
    {


        /// <summary>
        /// The fully qualified name of this aggregate identifier
        /// </summary>
        public string FullyQualifiedName
        {
            get
            {
                if (CQRSModel != null)
                {
                    return CQRSModel.Name + @"." + Name;
                }
                else
                {
                    return @"OrphanModel." + Name;
                }
            }
        }

        // Validations to apply to the AggregateIdentifier class:-
        // 1) The name must not be blank
        // 2) The name must be unique of all aggregate identifiers in the domain
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeNameAsValidIdentifier(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                context.LogError(Dsl.CustomCode.Validation.ValidationMessages.AggregateNameNotBlank, "AggregateIdentifier 01", this);
            }
            else
            {
                // Aggregate name must be unique per model (AggregateNameUnique)
                if (this.CQRSModel.AggregateIdentifiers.Where(f => f.Name == Name).Count() > 1)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.AggregateNameUnique, "AggregateIdentifier 02", this);
                }
            }
        }
    }
}
