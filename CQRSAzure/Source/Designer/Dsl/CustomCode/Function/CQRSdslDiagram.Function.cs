using Microsoft.VisualStudio.Modeling;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Functionality for the CQRS Model diagram as a whole
    /// </summary>
    /// <remarks>
    /// Thes esettings are stored in the diagram rather than in the model so that teh model can be copied between diagrams with - for instance - 
    /// different target languages without causing problems
    /// </remarks>
    public partial class CQRSdslDiagram
    {

        #region Code Generation Settings

        /// <summary>
        /// Additional handling of changes to the "Sub Folder per Model" setting
        /// </summary>
        internal sealed partial class SubfolderPerModelPropertyHandler
        {

            protected override void OnValueChanged(CQRSdslDiagram element, bool oldValue, bool newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

                // Pass on the change to the CQRS model class
                
            }

        }

        /// <summary>
        /// Additional handling of changes to the "Sub Folder per Aggregate" setting
        /// </summary>
        internal sealed partial class SubfolderPerAggregatePropertyHandler
        {

            protected override void OnValueChanged(CQRSdslDiagram element, bool oldValue, bool newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

                // Pass on the change to the CQRS model class

            }

        }

        /// <summary>
        /// Additional handling of the "Output Code Language" setting
        /// </summary>
        internal sealed partial class OutputCodeLanguagePropertyHandler
        {
            protected override void OnValueChanged(CQRSdslDiagram element, TargetLanguage oldValue, TargetLanguage newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

                // Pass on the change to the CQRS model

            }
        }

        #endregion

        #region Documentation settings

        /// <summary>
        /// Additional handling of changes to the "Copyright Notice" text
        /// </summary>
        internal sealed partial class CopyrightNoticePropertyHandler
        {
            protected override void OnValueChanged(CQRSdslDiagram element, string oldValue, string newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

            }
        }

        /// <summary>
        /// Additional handling of changes to the "Company name" text
        /// </summary>
        internal sealed partial class CompanyNamePropertyHandler
        {
            protected override void OnValueChanged(CQRSdslDiagram element, string oldValue, string newValue)
            {
                base.OnValueChanged(element, oldValue, newValue);

            }
        }

        #endregion

    }
}
