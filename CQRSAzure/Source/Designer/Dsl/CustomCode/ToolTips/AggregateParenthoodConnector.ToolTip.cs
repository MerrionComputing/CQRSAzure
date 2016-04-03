using System;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class AggregateParenthoodConnector
    {

        /// <summary>
        /// The aggregate-parentto-aggregate always has a tooltip
        /// </summary>
        public override bool HasToolTip
        {
            get
            {
                return true;
            }
        }

        public override string GetToolTipText(DiagramItem item)
        {

            string fromName = this.FromShape.AccessibleName;
            string toName = this.ToShape.AccessibleName;

            return string.Format("Aggregate [{0}] is hierarchically a parent to [{1}]", fromName, toName);
        }
    }
}
