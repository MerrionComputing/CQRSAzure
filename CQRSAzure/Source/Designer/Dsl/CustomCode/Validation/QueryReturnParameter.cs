using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;

namespace CQRSAzure.CQRSdsl.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    public partial class QueryReturnParameter
        : IQueryReturnParameter 
    {

        public string FullyQualifiedName
        {
            get
            {
                return QueryDefinition.FullyQualifiedName + @"." + Name;
            }
        }

        // Validations to apply to the QueryDefinition class:-
        // 1) The name must not be blank
        // 2) The name must be unique of all identifiers in the same query identifier
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeNameAsValidIdentifier(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                context.LogError(Dsl.CustomCode.Validation.ValidationMessages.QueryParameterNameNotBlank, nameof(QueryReturnParameter) + " 01", this);
            }
            else
            {
                if (this.QueryDefinition.QueryInputParameters.Count(f => f.Name == Name) > 0)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.QueryParameterNameUnique, nameof(QueryReturnParameter) + " 02", this);
                }
                if (this.QueryDefinition.QueryReturnParameters.Count(f => f.Name == Name) > 1)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.QueryParameterNameUnique, nameof(QueryReturnParameter) + " 02", this);
                }
            }
        }
    }
}
