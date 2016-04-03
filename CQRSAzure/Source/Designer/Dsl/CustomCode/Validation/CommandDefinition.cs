using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.CSharp;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;

namespace CQRSAzure.CQRSdsl.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    public partial class CommandDefinition
        : ICommandDefinitionEntity
    {

        public string FullyQualifiedName
        {
            get
            {
                return AggregateIdentifier.FullyQualifiedName + @"." + Name;
            }
        }

        // Validations to apply to the CommandDefinition class:-
        // 1) The name must not be blank
        // 2) The name must be unique of all identifiers in the same aggregate identifier
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeNameAsValidIdentifier(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                context.LogError(Dsl.CustomCode.Validation.ValidationMessages.CommandDefinitionNameNotBlank, nameof(CommandDefinition) + " 01", this);
            }
            else
            {
                if (this.AggregateIdentifier.CommandDefinitions.Count(f => f.Name == Name) > 1)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.CommandDefinitionNameUnique, nameof(CommandDefinition) + " 02", this);
                }
            }
        }
    }
}
