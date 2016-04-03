using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Operation performed during a projection when an event is encountered
    /// </summary>
    public partial class ProjectionEventPropertyOperation
    {


        /// <summary>
        /// A human-readable version of this property operation 
        /// </summary>
        /// <returns>
        /// For [traget field], [operation] [source field]
        /// </returns>
        public override string ToString()
        {
            string ret = "On " + EventName + ", For " + TargetPropertyName
                + "," + PropertyOperationToString(this.PropertyOperationToPerform);

            // If the source property is involved, add it to the description
            if (
                (this.PropertyOperationToPerform == PropertyOperation.DecrementByValue)
                ||
                (this.PropertyOperationToPerform == PropertyOperation.IncrementByValue)
                ||
                (this.PropertyOperationToPerform == PropertyOperation.SetToValue ))
            {
                ret += this.SourceEventPropertyName;
            }

            return ret;
        }


        /// <summary>
        /// Convert the property operation type to a readable string
        /// </summary>
        /// <param name="operation">
        /// The property operation to perform
        /// </param>
        public static string PropertyOperationToString(PropertyOperation operation)
        {
            switch(operation )
            {
                case PropertyOperation.IncrementCount:
                    {
                        return " increase the counter ";
                    }
                case PropertyOperation.DecrementCount:
                    {
                        return " decrease the counter ";
                    }
                case PropertyOperation.IncrementByValue:
                    {
                        return @" increment by value of ";
                    }
                case PropertyOperation.DecrementByValue:
                    {
                        return " decrement by value of ";
                     }
                case PropertyOperation.SetFlag:
                    {
                        return " set the flag ";
                    }
                case PropertyOperation.SetToValue:
                    {
                        return " set to the value ";
                    }
                case PropertyOperation.UnsetFlag:
                    {
                        return " unset the flag ";
                    }
                default:
                    {
                        return "";
                    }
            }
        }
    }

}
