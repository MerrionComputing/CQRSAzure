using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class QueryDefinitionShapeBase
    {

        /// <summary>
        /// Access to the QueryDefinition class that underlies this query definition shape
        /// </summary>
        QueryDefinition BaseClass
        {
            get
            {
                return this.ModelElement as QueryDefinition;
            }
        }
    }
}
