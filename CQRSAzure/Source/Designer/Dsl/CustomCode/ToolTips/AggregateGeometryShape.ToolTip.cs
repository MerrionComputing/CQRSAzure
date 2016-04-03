using System;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace  CQRSAzure.CQRSdsl.Dsl
{
    public partial class AggregateGeometryShapeBase
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
            // Todo: Get the description of the aggregate
            return BaseClass.Description;
        }
    }
}
