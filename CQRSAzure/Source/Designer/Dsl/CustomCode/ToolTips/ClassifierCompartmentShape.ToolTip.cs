using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class ClassifierCompartmentShape
    {

        protected override bool HasCustomToolTip
        {
            get
            {
                return true;
            }
        }

        private string GetVariableTooltipText(DiagramItem item)
        {
            // Todo: Get the description of the classifier
            //TODO - If we are over the body of the shape, display the classifier description
            return BaseClass.Description; 
            // Otherwise if this is a classifier operation, display its full text
        }
    }
}
