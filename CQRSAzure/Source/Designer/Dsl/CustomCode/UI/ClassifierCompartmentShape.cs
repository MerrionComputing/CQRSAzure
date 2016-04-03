using Microsoft.VisualStudio.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class ClassifierCompartmentShape
    {

        static string GetDisplayPropertyFromClassifierForEventEvaluationsCompartment(ModelElement element)
        {
            if (null != element)
            {
                ClassifierEventEvaluation op = element as ClassifierEventEvaluation;
                if (null != op)
                {
                    return op.ToString();
                }
                return "";
            }
            else
            {
                return "";
            }
        }
    }
}
