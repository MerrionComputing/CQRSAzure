using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Globalization;
using System.Windows.Forms.Design;

namespace CQRSAzure.CQRSdsl.CustomCode.UI
{
    public class CodeGenerationSettingsTypeEditor : UITypeEditor
    {

        #region Members
        private IWindowsFormsEditorService formsEditorService;
        #endregion

        public override UITypeEditorEditStyle GetEditStyle
        (ITypeDescriptorContext context)
        {
            if (context != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }

    }
}
