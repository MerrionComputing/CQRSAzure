
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Custom tooltip provision for the identity group geometry shape
    /// </summary>
    public partial class IdentityGroupGeometryShapeBase
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
            // Todo: Get the description of the identity group definition
            //TODO - If we are over the body of the shape, display the identity group description
            return BaseClass.Description; 
        }
    }
}
