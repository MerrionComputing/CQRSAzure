using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CQRSAzure.CQRSdsl.Dsl.CustomCode.Extensions
{
    /// <summary>
    /// Extensions to the classifier event evaluations collection
    /// </summary>
    public static class ClassifierEventEvaluationExtension
    {


        public static IOrderedEnumerable<ClassifierEventEvaluation> OrderByEvent(
           this IEnumerable<ClassifierEventEvaluation> source)
        {

            return source.OrderBy(f => f.EventName);

        }
    }

}
