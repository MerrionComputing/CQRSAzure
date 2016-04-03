using System;
using System.Drawing;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CQRSAzure.CQRSdsl.UI.Decorators
{

    /// <summary>
    /// A text field that wraps the content to fill the available area
    /// </summary>
    /// <remarks>
    /// This is used for comments or notes fields
    /// </remarks>
    public class WrappedTextField: TextField
    {


        public WrappedTextField(TextField prototype)
            : base(prototype.Name )
        {
            // Copy the starting properties form the prototype
            DefaultText = prototype.DefaultText;
            DefaultFocusable = prototype.DefaultFocusable;
            DefaultAutoSize = false;
            DefaultMultipleLine = true;
            AnchoringBehavior.Clear();
            AnchoringBehavior.CenterHorizontally(  );
            AnchoringBehavior.CenterVertically(  ); 
            DefaultAccessibleState = prototype.DefaultAccessibleState;
            
        }

    }
}
