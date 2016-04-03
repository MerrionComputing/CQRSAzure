using Microsoft.VisualStudio.Modeling.Validation;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;
using System.Linq;

namespace CQRSAzure.CQRSdsl.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    public partial class CommandParameter
        : ICommandParameterEntity
    {

        /// <summary>
        /// The fully qualified name of this command parameter
        /// </summary>
        public string FullyQualifiedName
        {
            get
            {
                return CommandDefinition.FullyQualifiedName + @"." + Name;
            }
        }

        // Validations to apply to the CommandParameter class:-
        // 1) The name must not be blank
        // 2) The name must be unique of all identifiers in the same command 
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeNameAsValidIdentifier(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                context.LogError(Dsl.CustomCode.Validation.ValidationMessages.CommandParameterNameNotBlank, nameof(CommandParameter) + " 01", this);
            }
            else
            {
                if (this.CommandDefinition.CommandParameters.Count(f => f.Name == Name) > 1)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.CommandParameterNameUnique, nameof(CommandParameter) + " 02", this);
                }
            }
        }
    }
}
