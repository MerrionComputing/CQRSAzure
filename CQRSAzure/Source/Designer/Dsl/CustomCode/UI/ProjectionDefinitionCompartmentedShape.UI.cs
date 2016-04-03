using Microsoft.VisualStudio.Modeling;
using System;
using System.Collections;

namespace CQRSAzure.CQRSdsl.Dsl
{

    public partial class ProjectionDefinitionCompartmentShape
    {

    }

        /// <summary>
        /// Additional UI functionality to perform for the projection definition UI
        /// </summary>
        public partial class ProjectionDefinitionCompartmentShapeBase
    {


        static System.Collections.IList FilterElementsFromProjectionDefinitionForPropertyOperations(global::System.Collections.IEnumerable elements)
        {
            // Todo - return a custom filtered list of property 
            return new ArrayList() { elements } ;
        }

        /// <summary>
        /// Get the full name for the property operation to 
        /// </summary>
        /// <param name="element">
        /// The projection definition that provides the row 
        /// </param>
        /// <returns></returns>
        static string GetDisplayPropertyFromProjectionDefinitionForPropertyOperations(ModelElement element)
        {
            if (null != element)
            {
                ProjectionEventPropertyOperation op = element as ProjectionEventPropertyOperation;
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
