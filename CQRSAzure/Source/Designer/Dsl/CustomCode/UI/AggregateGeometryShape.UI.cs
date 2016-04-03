using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class AggregateGeometryShape
    {
        public const string FIELDNAME_NAME = @"NameTextDecorator";
        public const string FIELDNAME_DESCRIPTION = @"DescriptionTextDecorator";

        protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
        {
            base.InitializeShapeFields(shapeFields);
            // Find the Comments decorator and make it wrapped
            TextField commentField = FindShapeField(shapeFields, FIELDNAME_DESCRIPTION) as TextField;
            if (null != commentField)
            {        
                // Replace with wrapped text field
                UI.Decorators.WrappedTextField wrappedCommentField = new UI.Decorators.WrappedTextField(commentField);
                shapeFields.Remove(commentField);
                shapeFields.Add(wrappedCommentField);
            }
        }

        /// <summary>
        /// The set of all the event definition shapes 
        /// </summary>
        public IList<EventDefinitionCompartmentShape > EventDefinitionShapes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The set of all command definition shapes linked to this aggregate
        /// </summary>
        public IList<CommandDefinitionCompartmentShape > CommandDefinitionShapes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The set of all query definitions linked to this aggregate
        /// </summary>
        public IList<QueryDefinitionShape > QueryDefinitionShapes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The set of all the classifier shapes connected to this aggregate shape
        /// </summary>
        /// <remarks>
        /// This is to allow items to be shown/hidden by type (onion-skinning)
        /// </remarks>
        public IList<ClassifierCompartmentShape > ClassifierShapes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The set of all identity groups linked to this aggregate
        /// </summary>
        public IList<IdentityGroupGeometryShape> IdentityGroupShapes
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }

    public partial class AggregateGeometryShapeBase
    {

    }
}
