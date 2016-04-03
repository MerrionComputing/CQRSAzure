using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CQRSAzure.CQRSdsl.Dsl.CustomCode.UI
{
    /// <summary>
    /// Make the source property name come from the properties of the select ed event
    /// </summary>
    public sealed class ClassifierEventSourceEventPropertyNameUITypeEditor
        : UITypeEditor 
    {

        private IWindowsFormsEditorService _editorService;

        /// <summary>
        /// Editor type for this property is a drop down list
        /// </summary>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (null != context)
            {
                return UITypeEditorEditStyle.DropDown;
            }
            else
            {
                return base.GetEditStyle(context);
            }
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((null != context) && (null != provider))
            {
                _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                ClassifierEventEvaluation clev = null;
                // Note - context.Instance may be either a DSL shape or the underlying domain class
                // so if we add a shape we need to look up the underlying domain class here
                clev = context.Instance as ClassifierEventEvaluation;
                if (null != clev)
                {
                    // Create a listbox to hold all the event names in the parent projection
                    ListBox lb = new ListBox();
                    lb.SelectionMode = SelectionMode.One;
                    lb.SelectedValueChanged += OnListBoxSelectedValueChanged;

                    // Add an Empty item
                    lb.Items.Add("");

                    if (null != clev.SelectedEvent)
                    {
                        foreach (var prop in clev.SelectedEvent.EventProperties)
                      {
                            lb.Items.Add(prop.Name);
                      }
                    }

                    _editorService.DropDownControl(lb);
                    if (lb.SelectedItem == null) // no selection, return the passed-in value as is
                        return value;

                    return lb.SelectedItem;

                }
            }
            return base.EditValue(context, provider, value);
        }


        private void OnListBoxSelectedValueChanged(object sender, EventArgs e)
        {
            if (null != _editorService)
            {
                // close the drop down as soon as something is clicked
                _editorService.CloseDropDown();
            }
        }


    }
}
