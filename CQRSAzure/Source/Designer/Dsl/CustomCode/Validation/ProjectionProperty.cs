using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;

namespace CQRSAzure.CQRSdsl.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    public partial class ProjectionProperty
        : IProjectionPropertyEntity
    {

        public string FullyQualifiedName
        {
            get
            {
                return ProjectionDefinition.FullyQualifiedName + @"." + Name;
            }
        }

        // Validations to apply to the ProjectionProperty class:-
        // 1) The name must not be blank
        // 2) The name must be unique of all identifiers in the same projection identifier
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeNameAsValidIdentifier(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ProjectionPropertyNameNotBlank, nameof(ProjectionProperty) + " 01", this);
            }
            else
            {
                if (this.ProjectionDefinition.ProjectionProperties.Count(f => f.Name == Name) > 1)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ProjectionPropertyNameUnique, nameof(ProjectionProperty) + " 02", this);
                }
            }
        }
    }
}
