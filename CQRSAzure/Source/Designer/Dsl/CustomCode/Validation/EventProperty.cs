using Microsoft.VisualStudio.Modeling.Validation;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;


namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class EventProperty
        : IEventPropertyEntity
    {

        // TODO: Add any custom validation logic

        public string FullyQualifiedName
        {
            get
            {
                return EventDefinition.FullyQualifiedName + @"." + Name; 
            }
        }
    }
}
