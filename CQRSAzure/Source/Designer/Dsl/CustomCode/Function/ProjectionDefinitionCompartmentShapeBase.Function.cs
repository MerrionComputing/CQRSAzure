using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class ProjectionDefinitionCompartmentShapeBase
    {

        /// <summary>
        /// Access to the ProjectionDefinition class that underlies this projection definition shape
        /// </summary>
        ProjectionDefinition BaseClass
        {
            get
            {
                return this.ModelElement as ProjectionDefinition;
            }
        }
    }
}
