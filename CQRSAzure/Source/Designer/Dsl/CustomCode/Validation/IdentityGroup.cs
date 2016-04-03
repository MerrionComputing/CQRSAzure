using CQRSAzure.CQRSdsl.CustomCode.Interfaces;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.CSharp;

using System.Linq;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class IdentityGroup
        : IIdentityGroupEntity
    {

        /// <summary>
        /// The fully qualified name of this aggregate identifier
        /// </summary>
        public string FullyQualifiedName
        {
            get
            {
                return  AggregateIdentifier.FullyQualifiedName + @"." + Name;
            }
        }

        // Validations to apply to the IdentityGroup class:-
        // 1) The name must not be blank
        // 2) The name must be unique of all identity groups in this aggregate identifier
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeNameAsValidIdentifier(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                context.LogError(Dsl.CustomCode.Validation.ValidationMessages.IdentityGroupNameNotBlank, nameof(IdentityGroup ) + "  01", this);
            }
            else
            {
                if (this.AggregateIdentifier.IdentityGrouped.Count(f => f.Name == Name) > 1)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.IdentityGroupNameUnique, nameof(IdentityGroup) + "  02", this);
                }
            }
        } 
    }
}
