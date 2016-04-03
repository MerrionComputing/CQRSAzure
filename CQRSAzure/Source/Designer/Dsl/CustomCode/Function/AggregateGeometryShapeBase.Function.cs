using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class AggregateGeometryShapeBase
    {

        /// <summary>
        /// Are the linked events/projections/queries/commands
        /// </summary>
        private bool m_childrenHidden = false;
        public bool ChildrenHidden
        {
            get
            {
                return m_childrenHidden;
            }
            set
            {
                m_childrenHidden = value ;
            }
        }

        /// <summary>
        /// Access to the AggregateIdentifier class that underlies this aggregate geometry shape
        /// </summary>
        AggregateIdentifier BaseClass
        {
            get
            {
                return this.ModelElement as AggregateIdentifier;
            }
        }


    }
}
