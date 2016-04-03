using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.Diagrams;


namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class QueryDefinitionShapeBase
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
            // Todo: Get the description of the query definition
            return BaseClass.Description;
        }
    }
}
