using CQRSAzure.CQRSdsl.CustomCode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRSAzure.CQRSdsl.Dsl
{

    /// <summary>
    /// Custom UI extension to the ClassifierEventEvaluation class to display the class in a single string
    /// </summary>
    public partial class ClassifierEventEvaluation
    {


        /// <summary>
        /// Put together a string representing this event evaluation operation
        /// </summary>
        public override string ToString()
        {
            StringBuilder sbret = new StringBuilder();
            if (! string.IsNullOrWhiteSpace(EventName ) )
            {
                sbret.Append("On " + EventName + ", ");
            }
            if (this.PropertyEvaluationToPerform == PropertyEvaluation.Always)
            {
                sbret.Append(" always set to ");
                sbret.Append(this.OnTrue.ToString());
            }
            else
            {
                sbret.Append("if ");
                sbret.Append(this.SourceEventPropertyName);
                sbret.Append(" ");
                sbret.Append(this.PropertyEvaluationToPerform.ToString());
                sbret.Append(" ");
                sbret.Append(this.Target);
                if (this.TargetType == EvaluationTargetType.Variable)
                {
                    sbret.Append(" (variable) ");
                }
                if (this.OnTrue == IdentityGroupClassification.Unchanged)
                {
                    sbret.Append(" leave unchanged");
                }
                else
                {
                    sbret.Append(" set to ");
                    sbret.Append(this.OnTrue.ToString());
                }
                sbret.Append(", otherwise");
                if (this.OnFalse  == IdentityGroupClassification.Unchanged)
                {
                    sbret.Append(" leave unchanged");
                }
                else
                {
                    sbret.Append(" set to ");
                    sbret.Append(this.OnFalse.ToString());
                }
            }
            return sbret.ToString();
        }

    }
}
