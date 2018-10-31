using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling.Shell;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using VsShell = Microsoft.VisualStudio.Shell.Interop;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Base class for all command features.
    /// </summary>
    /// <typeparam name="TSelection">Type of selection used by the command.</typeparam>
    [CLSCompliant(false)]
    public abstract class CustomCommand<TSelection> : DynamicStatusMenuCommand, IFeature
        where TSelection : Selection, new()
    {
        #region Constructor/dispose

        private MetaCommand m_metaCommand;
        private ITypedServiceProvider m_serviceProvider;
        private TSelection m_selection;

        protected CustomCommand(CommandID commandId) : base(UpdateStatus, Invoke, commandId) { }

        public void Associate(MetaFeature metaFeature, ITypedServiceProvider serviceProvider)
        {
            if (m_metaCommand != null || m_serviceProvider != null)
                throw new InvalidOperationException("Associate should only be called once on a IFeature instance.");

            if (metaFeature == null) throw new ArgumentNullException("metaFeature");
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");

            m_metaCommand = metaFeature as MetaCommand;
            m_serviceProvider = serviceProvider;

            IMenuCommandService menuService = m_serviceProvider.GetService<IMenuCommandService>();
            Debug.Assert(menuService != null);
            if (menuService != null)
            {
                menuService.AddCommand(this);
            }
        }

        ~CustomCommand()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Debug.Assert(disposing, "Finalizing CustomCommand without disposing.");

            if (disposing)
            {
                if (m_serviceProvider != null)
                {
                    IMenuCommandService menuService = m_serviceProvider.GetService<IMenuCommandService>();
                    if (menuService != null)
                        menuService.RemoveCommand(this);
                    m_serviceProvider = null;
                }
            }

            m_metaCommand = null;
            m_selection = null;

            Debug.WriteLine("Unloaded and disposed command " + m_metaCommand);
        }

        #endregion

        #region General Properties

        public ITypedServiceProvider ServiceProvider
        {
            [DebuggerStepThrough]
            get
            {
                Debug.Assert(m_serviceProvider != null);
                return m_serviceProvider;
            }
        }

        public MetaCommand MetaCommand
        {
            [DebuggerStepThrough]
            get
            {
                Debug.Assert(m_metaCommand != null);
                return m_metaCommand;
            }
        }

        protected TSelection Selection
        {
            [DebuggerStepThrough]
            get
            {
                Debug.Assert(m_selection != null);
                return m_selection;
            }
            [DebuggerStepThrough]
            private set
            {
                m_selection = value;
            }
        }

        protected virtual bool UseWaitCursor
        {
            [DebuggerStepThrough]
            get { return true; }
        }

        protected virtual bool UseAnimation
        {
            [DebuggerStepThrough]
            get { return false; }
        }

        #endregion

        #region Command invoke/status

        private static void UpdateStatus(object sender, EventArgs args)
        {
            CustomCommand<TSelection> commandSender = sender as CustomCommand<TSelection>;
            Debug.Assert(commandSender != null);
            try
            {
                commandSender.Selection = CQRSAzure.CQRSdsl.Dsl.Selection.Create<TSelection>(commandSender.ServiceProvider, null);
                commandSender.OnUpdateStatus();
            }
            finally
            {
                commandSender.Selection = null;
            }
        }

        protected virtual void OnUpdateStatus()
        {
            this.Visible = true;
            this.Enabled = true;
        }

        private static void Invoke(object sender, EventArgs args)
        {
            CustomCommand<TSelection> commandSender = sender as CustomCommand<TSelection>;
            Debug.Assert(commandSender != null);
            try
            {
                commandSender.Selection = CQRSAzure.CQRSdsl.Dsl.Selection.Create<TSelection>(commandSender.ServiceProvider, null);
                commandSender.OnInvoking();
                bool success = false;
                try
                {
                    commandSender.OnInvoke();
                    success = true;
                }
                catch (CommandErrorException error)
                {
                    commandSender.HandleCommandError(error);
                }
                finally
                {
                    commandSender.OnInvoked(success);
                }
            }
            finally
            {
                commandSender.Selection = null;
            }
        }

        protected virtual void OnInvoking()
        {
            if (this.UseWaitCursor)
                ShowWaitCursor();

            if (this.UseAnimation)
            {
                EnvDTE.DTE dte = ServiceProvider.GetService<EnvDTE.DTE>();
                Debug.Assert(dte != null);
                if (dte != null)
                {
                    dte.StatusBar.Animate(true, EnvDTE.vsStatusAnimation.vsStatusAnimationGeneral);
                }
            }
        }

        protected virtual void OnInvoke()
        {
            // do nothing by default
        }

        protected virtual void OnInvoked(bool success)
        {
            if (this.UseAnimation)
            {
                EnvDTE.DTE dte = ServiceProvider.GetService<EnvDTE.DTE>();
                Debug.Assert(dte != null);
                if (dte != null)
                {
                    dte.StatusBar.Animate(false, EnvDTE.vsStatusAnimation.vsStatusAnimationGeneral);
                }
            }
        }

        #endregion

        #region Error handling

        [DebuggerStepThrough]
        protected CommandErrorException CreateCommandError(string message)
        {
            return new CommandErrorException(message);
        }

        [DebuggerStepThrough]
        protected CommandErrorException CreateCommandError(MessageBoxIcon icon, string message)
        {
            return new CommandErrorException(icon, message);
        }

        [DebuggerStepThrough]
        protected virtual void HandleCommandError(CommandErrorException error)
        {
            ShowMessage(error.Icon, error.Message);
        }

        #endregion

        #region UI helper methods

        [DebuggerStepThrough]
        protected virtual void ShowMessage(string message)
        {
            ShowMessage(MessageBoxIcon.Information, message);
        }

        protected virtual void ShowMessage(MessageBoxIcon icon, string message)
        {
            VsShell.OLEMSGICON oleIcon;
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    oleIcon = VsShell.OLEMSGICON.OLEMSGICON_CRITICAL;
                    break;
                case MessageBoxIcon.Information:
                    oleIcon = VsShell.OLEMSGICON.OLEMSGICON_INFO;
                    break;
                case MessageBoxIcon.None:
                    oleIcon = VsShell.OLEMSGICON.OLEMSGICON_NOICON;
                    break;
                case MessageBoxIcon.Question:
                    oleIcon = VsShell.OLEMSGICON.OLEMSGICON_QUERY;
                    break;
                case MessageBoxIcon.Warning:
                    oleIcon = VsShell.OLEMSGICON.OLEMSGICON_WARNING;
                    break;
                default:
                    Debug.Fail("Invalid icon kind.");
                    oleIcon = VsShell.OLEMSGICON.OLEMSGICON_NOICON;
                    break;
            }

            PackageUtility.ShowMessageBox(
                this.ServiceProvider,
                message,
                VsShell.OLEMSGBUTTON.OLEMSGBUTTON_OK,
                VsShell.OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                oleIcon);
        }

        protected void ShowWaitCursor()
        {
            VsShell.IVsUIShell uiShell = this.ServiceProvider.GetService<VsShell.IVsUIShell>();
            Debug.Assert(uiShell != null);
            if (uiShell != null)
            {
                ErrorHandler.ThrowOnFailure(uiShell.SetWaitCursor());
            }
        }

        protected void SetStatusText(string text, bool highlight)
        {
            EnvDTE.DTE dte = this.ServiceProvider.GetService<EnvDTE.DTE>();
            Debug.Assert(dte != null);
            if (dte != null)
            {
                dte.StatusBar.Text = text;
                dte.StatusBar.Highlight(highlight);
            }
        }

        #endregion
    }
}