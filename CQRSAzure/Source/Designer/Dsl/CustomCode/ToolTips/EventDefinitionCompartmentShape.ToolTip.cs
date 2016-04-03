using System;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class EventDefinitionCompartmentShapeBase
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
            // Todo: Get the description of the event definition
            return BaseClass.Name + "(" + BaseClass.Version.ToString() + ")";  
        }

    }
}
