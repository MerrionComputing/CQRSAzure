using System.Linq;

using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.CSharp;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;

namespace CQRSAzure.CQRSdsl.Dsl
{
    [ValidationState(ValidationState.Enabled)]
    public partial class ProjectionEventPropertyOperation
    {

        /// <summary>
        /// Validations to apply to the EventName property of this class:-
        ///  1) The name must not be blank
        ///  2) The name must be one of the projection linked events' names
        /// </summary>
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeEventName(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(this.EventName))
            {
                context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ProjectionEventPropertyOperantionEventBlank, nameof(ProjectionEventPropertyOperation) + " 01", this);
            }
            else
            {
                // event name must be in the 
                var qryParentEvents = from ev in this.ProjectionDefinition.EventDefinitions
                                      where ev.Name == this.EventName
                                      select ev.Name;

                if (qryParentEvents.Count() <= 0)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ProjectionEventPropertyOperantionEventInvalid, nameof(ProjectionEventPropertyOperation) + " 02", this);
                }
            }
        }

        /// <summary>
        /// Validations to perform on SourceEventPropertyName:
        /// </summary>
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeSourceEventPropertyName(ValidationContext context)
        {
            if (SourceFieldRequired())
            { 
                if (string.IsNullOrWhiteSpace(this.SourceEventPropertyName ) )
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ProjectionEventPropertyOperationSourceBlank, nameof(ProjectionEventPropertyOperation) + " 03", this);
                }
                else
                {
                    if (null != SelectedEvent  )
                    {
                        var qryEventFields = from fld in SelectedEvent.EventProperties
                                             where fld.Name == this.SourceEventPropertyName
                                             select fld.Name;

                        if (qryEventFields.Count() <= 0 )
                        {
                            context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ProjectionEventPropertyOperationSourceInvalid, nameof(ProjectionEventPropertyOperation) + " 04", this);
                        }
                    }
                }
            }
        }

        //TargetPropertyName
        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        private void ValidateAttributeTargetPropertyName(ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(TargetPropertyName ) )
            {
                context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ProjectionEventPropertyOperationTargetBlank, nameof(ProjectionEventPropertyOperation) + " 05", this);
            }
            else
            {
                var qryTargets = from prop in this.ProjectionDefinition.ProjectionProperties
                                 where prop.Name == TargetPropertyName
                                 select prop.Name;
                
                if (qryTargets.Count() <= 0)
                {
                    context.LogError(Dsl.CustomCode.Validation.ValidationMessages.ProjectionEventPropertyOperationTargetInvalid, nameof(ProjectionEventPropertyOperation) + " 06", this);
                } 
            }
        }

        /// <summary>
        /// Is a source field required for this kind of property operation
        /// </summary>
        public bool SourceFieldRequired()
        {
            switch (this.PropertyOperationToPerform)
            {
                case PropertyOperation.DecrementByValue:
                    {
                        return true;
                    }
                case PropertyOperation.IncrementByValue:
                    {
                        return true;
                    }
                case PropertyOperation.SetToValue:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        /// <summary>
        /// Get the currently selected event
        /// </summary>
        public EventDefinition SelectedEvent
        {
            get {
                if (string.IsNullOrWhiteSpace(this.EventName))
                {
                    return null;
                }
                else
                {
                    var qryEvt = from evt in this.ProjectionDefinition.EventDefinitions
                                 where evt.Name == this.EventName
                                 select evt;

                    return qryEvt.FirstOrDefault();  
                }
            }
            set {
                this.EventName = value.Name;
            }
        }
    }
}
