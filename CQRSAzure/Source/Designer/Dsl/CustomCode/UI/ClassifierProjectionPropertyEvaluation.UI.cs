using CQRSAzure.CQRSdsl.CustomCode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class ClassifierProjectionPropertyEvaluation
    {

        /// <summary>
        /// Put together a string representing this projection evaluation operation
        /// </summary>
        public override string ToString()
        {
            StringBuilder sbret = new StringBuilder();
            if (this.PropertyEvaluationToPerform == PropertyEvaluation.Always)
            {
                sbret.Append(" Always set to ");
                sbret.Append(this.OnTrue.ToString());
            }
            else
            {
                sbret.Append("If ");
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
                if (this.OnFalse == IdentityGroupClassification.Unchanged)
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
