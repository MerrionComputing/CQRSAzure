using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace CQRSAzure.CQRSdsl.Dsl
{

    internal partial class CQRSdslCommandSet
    {

        private Guid guidCustomDiagramMenuCmdSet = new Guid("986DE2DB-3E48-429B-8474-1248E06D8C27");
        private int cmdidShowHideModelTipsContextMenuCommand = 0x00001;
        private int cmdidIncrementEventVersionContextMenuCommand = 0x00002;
        private int cmdidExpandCollapseAggregateContextMenuCommand = 0x00003;
        private int cmdidModelSettings = 0x00004;

        private int cmdidGenerateCQRSModelCode =  0x00011;

        private int cmdidGenerateCQRSModelDocumentation = 0x00021;

        private int cmdidDataTopLevelMenuCommand = 0x00031;

        private int cmdidDataAddAggregateInstanceMenuCommand = 0x00032;
        private int cmdidDataAddEventToAggregateInstanceMenuCommand = 0x00033;
        private int cmdidDataShowEventsForAggregateInstanceMenuCommand = 0x00034;
        private int cmdidDataRunProjectionForAggregateInstanceMenuCommand = 0x00035;
        private int cmdidDataRunClassifierForAggregateInstanceMenuCommand = 0x00036;
        private int cmdidDataShowIdentifierGroupMembersMenuCommand = 0x00037;
        private int cmdidDataRunQueryForIdentifierGroupMenuCommand = 0x00038;
        private int cmdidDataRunCommandForIdentifierGroupMenuCommand = 0x00039;
        private int cmdidDataImportpMenuCommand = 0x00040;
        private int cmdidDataExportMenuCommand = 0x00041;

        /// <summary>
        /// Get the set of commands for this CQRS/DSL tool
        /// </summary>
        protected override IList<MenuCommand> GetMenuCommands()
        {
            // Get the standard base commands
            global::System.Collections.Generic.IList<global::System.ComponentModel.Design.MenuCommand> commands = base.GetMenuCommands();

            // Add custom commands
            global::System.ComponentModel.Design.MenuCommand menuCommand = null;

            // Add handler for "Show/Hide tooltips" menu command 
            menuCommand = new DynamicStatusMenuCommand(new EventHandler(OnStatusShowHideModelTips),
                new EventHandler(OnMenuShowHideModelTips),
                new CommandID(guidCustomDiagramMenuCmdSet, cmdidShowHideModelTipsContextMenuCommand));
            commands.Add(menuCommand);

            // Add handler for "Increment event version" menu command 
            menuCommand = new DynamicStatusMenuCommand(new EventHandler(OnStatusIncrementEventVersion ),
                new EventHandler(OnMenuIncrementEventVersion ),
                new CommandID(guidCustomDiagramMenuCmdSet, cmdidIncrementEventVersionContextMenuCommand));
            commands.Add(menuCommand);

            // Add handler for "Expand / CollapseAggregate" menu command 
            menuCommand = new DynamicStatusMenuCommand(new EventHandler(OnStatusExpandCollapseAggregate),
                new EventHandler(OnMenuExpandCollapseAggregate),
                new CommandID(guidCustomDiagramMenuCmdSet, cmdidExpandCollapseAggregateContextMenuCommand));
            commands.Add(menuCommand);

            // Add handler for the "Model properties" menu command
            menuCommand = new DynamicStatusMenuCommand(new EventHandler(OnStatusModelProperties),
                new EventHandler(OnMenuModelProperties),
                new CommandID(guidCustomDiagramMenuCmdSet, cmdidModelSettings)) ;
            commands.Add(menuCommand);

            // Add handler for "Generate code" menu command
            menuCommand = new DynamicStatusMenuCommand(new EventHandler(OnStatusGenerateCQRSModelCode),
                new EventHandler(OnMenuGenerateCQRSModelCode),
                new CommandID(guidCustomDiagramMenuCmdSet, cmdidGenerateCQRSModelCode));
            commands.Add(menuCommand);

            // Add handler for "Generate documentation" menu command
            menuCommand = new DynamicStatusMenuCommand(new EventHandler(OnStatusGenerateCQRSModelDocumentation),
                new EventHandler(OnMenuGenerateCQRSModelDocumentation),
                new CommandID(guidCustomDiagramMenuCmdSet, cmdidGenerateCQRSModelDocumentation));
            commands.Add(menuCommand);

            // Add handler for the "Data" top level menu
            menuCommand = new MenuCommand (
                new EventHandler(OnMenuDataTopLevelMenuCommand),
                new CommandID(guidCustomDiagramMenuCmdSet, cmdidDataTopLevelMenuCommand));
            commands.Add(menuCommand);

            // Add handler for "AddAggregateInstance" Menu Command
            menuCommand = new DynamicStatusMenuCommand(new EventHandler(OnStatusAddAggregateInstanceMenuCommand),
                new EventHandler(OnMenuAddAggregateInstanceMenuCommand),
                new CommandID(guidCustomDiagramMenuCmdSet, cmdidDataAddAggregateInstanceMenuCommand));
            commands.Add(menuCommand);

            // Add handler for "AddEventToAggregateInstance" Menu Command 
            // Add handler for "ShowEventsForAggregateInstance" Menu Command 
            // Add handler for "RunProjectionForAggregateInstance" Menu Command
            // Add handler for "RunClassifierForAggregateInstance" Menu Command
            // Add handler for "ShowIdentifierGroupMembers" Menu Command
            // Add handler for "RunQueryForIdentifierGroup" Menu Command
            // Add handler for "RunCommandForIdentifierGroup" Menu Command

            // add handler for the "DataImport" menu command
            menuCommand = new MenuCommand(new EventHandler(OnDataImportMenuCommand),
                new CommandID(guidCustomDiagramMenuCmdSet, cmdidDataImportpMenuCommand ));
            commands.Add(menuCommand);

            // add handler for the DataExport command


            // return the resulting list
            return commands;

        }


        // Command menu constants...


        // 1) Diagram menu - commands for the DSL diagram
        // 




        // 1.1 cmdidIncrementEventVersionContextMenuCommand
        internal virtual void OnStatusIncrementEventVersion(object sender, System.EventArgs e)
        {
            global::System.ComponentModel.Design.MenuCommand cmd = sender as global::System.ComponentModel.Design.MenuCommand;
            cmd.Visible = cmd.Enabled = false;

            if( this.CurrentSelection.OfType<EventDefinitionCompartmentShape>().Count() ==1)
            {
                // One and only one "Event" selected so show the Increment version command
                cmd.Visible = cmd.Enabled = true;
            }
        }
        /// <summary>
        /// Handler for incrementing the version of the selected version
        /// </summary>
        internal virtual void OnMenuIncrementEventVersion(object sender, global::System.EventArgs e)
        {
            EventDefinitionCompartmentShape evt = this.CurrentSelection.OfType<EventDefinitionCompartmentShape>().FirstOrDefault();
            if (null != evt)
            {
                Microsoft.VisualStudio.Modeling.Transaction tVersion = evt.Store.TransactionManager.BeginTransaction("Version increment");
                evt.IncrementVersionNumber();
                tVersion.Commit();
            } 
        }

        // 1.2 cmdidExpandCollapseAggregateContextMenuCommand
        internal virtual void OnStatusExpandCollapseAggregate(object sender, System.EventArgs e)
        {
            global::System.ComponentModel.Design.MenuCommand cmd = sender as global::System.ComponentModel.Design.MenuCommand;
            cmd.Visible = true;
            cmd.Enabled = false;
            if (this.CurrentSelection.OfType<AggregateGeometryShape>().Count() == 1)
            {
                // One and only one "Aggregate" selected so show the expand/collapse command
                cmd.Enabled = true;
            }
        }
        /// <summary>
        /// Handler for incrementing the version of the selected version
        /// </summary>
        internal virtual void OnMenuExpandCollapseAggregate(object sender, global::System.EventArgs e)
        {
            AggregateGeometryShape agg = this.CurrentSelection.OfType<AggregateGeometryShape>().FirstOrDefault();
            if (null != agg)
            {

                agg.ChildrenHidden = !agg.ChildrenHidden;

                Microsoft.VisualStudio.Modeling.Transaction tShowHide = agg.Store.TransactionManager.BeginTransaction("Show or hide children");
                // Show/hide all the child links and shapes
                foreach (BinaryLinkShape linkedChild in agg.FromRoleLinkShapes.OfType<BinaryLinkShape >() )
                {
                    linkedChild.SetShowHideState(!agg.ChildrenHidden);
                    if (linkedChild.ToShape != null)
                    { 
                        linkedChild.ToShape.SetShowHideState(!agg.ChildrenHidden);
                    }
                }       
                tShowHide.Commit();
            }
        }

        // 1.3 cmdidShowHideModelTipsContextMenuCommand
        internal virtual void OnStatusShowHideModelTips(object sender, System.EventArgs e)
        {
            // For now - always shown 
            global::System.ComponentModel.Design.MenuCommand cmd = sender as global::System.ComponentModel.Design.MenuCommand;
            if (null != cmd)
            {
                cmd.Visible = true;
                cmd.Enabled = true;
            }
        }

        /// <summary>
        /// Show or hide the model tool tips
        /// </summary>
        internal virtual void OnMenuShowHideModelTips(object sender, EventArgs e)
        {

        }

        // 1.4 cmdidModelPropertiesMenuCommand
        internal virtual void OnStatusModelProperties(object sender, EventArgs e)
        {
            // For now - always shown 
            global::System.ComponentModel.Design.MenuCommand cmd = sender as global::System.ComponentModel.Design.MenuCommand;
            if (null != cmd)
            {
                cmd.Visible = true;
                cmd.Enabled = true;
            }
        }

        /// <summary>
        /// Show the model settings dialog
        /// </summary>
        internal virtual void OnMenuModelProperties(object sender, EventArgs e)
        {
            if (null != GetCurrentCQRSModel())
            {

            }
        }





        // 2) Code generation..only show if there is one or more things to generate
        internal void OnStatusGenerateCQRSModelCode(object sender, EventArgs e)
        {
            global::System.ComponentModel.Design.MenuCommand cmd = sender as global::System.ComponentModel.Design.MenuCommand;
            if (null != cmd)
            {
                cmd.Visible = true;
                if (this.IsCurrentDiagramEmpty())
                {
                    cmd.Enabled = false ;
                }
                else
                {
                    cmd.Enabled = true;
                }
            }
        }

        internal void OnMenuGenerateCQRSModelCode(object sender, EventArgs e)
        {
            if (null != GetCurrentCQRSModel())
            {
                // TODO: Show the model code settings, to allow the user to change them if neccessary
                CQRSAzure.CQRSdsl.CustomCode.Interfaces.IModelCodeGenerationOptions options = GetCurrentCQRSModel().GetCodeGenerationOptions();
                if (null == options)
                {
                    options = ModelCodeGenerationOptions.Default();
                }


                // Code generation for the specified model...
                CodeGeneration.ModelCodeGenerator codeGen = new CodeGeneration.ModelCodeGenerator(GetCurrentCQRSModel(), options );
                if (null != codeGen)
                {
                    codeGen.GenerateCode();
                }
            }
        }

        // 3) Documentation generation
        internal void OnStatusGenerateCQRSModelDocumentation(object sender, EventArgs e)
        {
            global::System.ComponentModel.Design.MenuCommand cmd = sender as global::System.ComponentModel.Design.MenuCommand;
            if (null != cmd)
            {
                cmd.Visible = true;
                if (this.IsCurrentDiagramEmpty())
                {
                    cmd.Enabled = false;
                }
                else
                {
                    cmd.Enabled = true;
                }
            }
        }

        internal void OnMenuGenerateCQRSModelDocumentation(object sender, EventArgs e)
        {
            if (null != GetCurrentCQRSModel())
            {
                CQRSAzure.CQRSdsl.CustomCode.Interfaces.IDocumentationGenerationOptions options = GetCurrentCQRSModel().GetDocumentationGenerationOptions();
                if (null == options)
                {
                    options = DocumentationGenerationOptions.Create(new System.IO.DirectoryInfo ( GetCurrentCQRSModel().DocumentationRootFolder ));
                }

                // Document the specified model...
                DocumentationGeneration.ModelDocumentationGenerator modelDoc = new DocumentationGeneration.ModelDocumentationGenerator(GetCurrentCQRSModel(), options );
                if (null != modelDoc)
                {
                    modelDoc.GenerateDocumentation();
                }
            }
        }

        // Data top level command
        private void OnMenuDataTopLevelMenuCommand(object sender, EventArgs e)
        {
            // TODO - Maybe show the appropriate data related submenu(s) according to what is selected
        }

        // "AddAggregateInstance" Menu Command
        private void OnMenuAddAggregateInstanceMenuCommand(object sender, EventArgs e)
        {
            // TODO - Show a pop-up to allow the user to add a new instance of the selected aggregate...

        }

        private void OnStatusAddAggregateInstanceMenuCommand(object sender, EventArgs e)
        {
            global::System.ComponentModel.Design.MenuCommand cmd = sender as global::System.ComponentModel.Design.MenuCommand;
            cmd.Visible = false   ;
            cmd.Enabled = false;
            if (this.CurrentSelection.OfType<AggregateGeometryShape>().Count() == 1)
            {
                // One and only one "Aggregate" selected so show the "Add aggregate instance" command
                cmd.Visible = true;
                cmd.Enabled = true;
            }
        }


        // "Import data"
        private void OnDataImportMenuCommand(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        // "Export data"


        internal CQRSModel GetCurrentCQRSModel()
        {
            if (null != this.CurrentCQRSdslDocData)
            {
                CQRSModel ret = CurrentCQRSdslDocData.RootElement as CQRSModel;
                if (null != ret)
                {
                    return ret;
                }
            }
            return null;
        }
    }
}
