using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class ClassifierCompartmentShape
    {

        /// <summary>
        /// Access to the classifier class that underlies this classifier compartment shape
        /// </summary>
        Classifier BaseClass
        {
            get
            {
                return this.ModelElement as Classifier;
            }
        }

    }
}
