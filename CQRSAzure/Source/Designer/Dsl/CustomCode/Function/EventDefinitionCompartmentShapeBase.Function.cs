using Microsoft.VisualStudio.Modeling.Extensibility;


namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class EventDefinitionCompartmentShapeBase
    {

        /// <summary>
        /// Increment the version number of the event definition held by this
        /// compartment shape
        /// </summary>
        /// <remarks>
        /// Any properties added or deleted use this version number to implement their as-of and until
        /// range
        /// </remarks>
        public void IncrementVersionNumber()
        {
            BaseClass.Version += 1;
        }

        EventDefinition BaseClass
        {
            get
            {
                return this.ModelElement as EventDefinition;
            }
        }

    }
}
