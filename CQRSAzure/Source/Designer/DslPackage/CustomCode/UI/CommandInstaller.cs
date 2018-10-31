using System;
using System.Diagnostics;

namespace CQRSAzure.CQRSdsl.Dsl
{
    #region CommandInstaller

    /// <summary>
    /// Feature installer for commands.
    /// </summary>
    public sealed class CommandInstaller : IFeatureInstaller
    {
        private INamedCommandService m_commandService;

        public CommandInstaller(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");
            this.m_commandService = serviceProvider.GetService(typeof(INamedCommandService)) as INamedCommandService;
            Debug.Assert(this.m_commandService != null);
        }

        /// <summary>
        /// Installs the command into the product.
        /// </summary>
        /// <param name="metaFeature">MetaCommand to install.</param>
        public void Install(MetaFeature metaFeature)
        {
            MetaCommand metaCommand = metaFeature as MetaCommand;
            Debug.Assert(metaCommand != null && !IsInstalled(metaCommand));
            if (metaCommand != null && metaCommand.IsCustomCommand)
            {
                string canonicalName = m_commandService.GetCanonicalName(metaCommand.Name);
                if (metaCommand.CustomCommandId == null)
                {
                    metaCommand.CustomCommandId = m_commandService.FindCommandId(canonicalName);
                    if (metaCommand.CustomCommandId == null)
                    {
                        metaCommand.CustomCommandId = m_commandService.CreateCommand(canonicalName, metaCommand.Name, metaCommand.Description);
                    }
                }

                foreach (CommandPlacementAttribute attr in ReflectionHelper.GetAttributes<CommandPlacementAttribute>(metaCommand.FeatureType))
                {
                    m_commandService.PlaceCommand(canonicalName, attr.CommandBarPath);
                }
            }
        }

        /// <summary>
        /// Checks whether given command is already installed.
        /// </summary>
        /// <param name="metaFeature">MetaCommand.</param>
        /// <returns>True if installed and false otherwise.</returns>
        public bool IsInstalled(MetaFeature metaFeature)
        {
            MetaCommand metaCommand = metaFeature as MetaCommand;
            Debug.Assert(metaCommand != null);
            if (metaCommand != null)
            {
                if (metaCommand.IsCustomCommand)
                {
                    if (metaCommand.CustomCommandId == null)
                        metaCommand.CustomCommandId = m_commandService.FindCommandId(m_commandService.GetCanonicalName(metaCommand.Name));
                    return metaCommand.CustomCommandId != null;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Uninstalls specified command from the product.
        /// </summary>
        /// <param name="metaFeature">MetaCommand to uninstall.</param>
        public void Uninstall(MetaFeature metaFeature)
        {
            MetaCommand metaCommand = metaFeature as MetaCommand;
            Debug.Assert(metaCommand != null);
            if (metaCommand != null && metaCommand.IsCustomCommand)
            {
                string canonicalName = m_commandService.GetCanonicalName(metaCommand.Name);

                if (metaCommand.CustomCommandId == null)
                    metaCommand.CustomCommandId = m_commandService.FindCommandId(canonicalName);

                if (metaCommand.CustomCommandId != null)
                {
                    m_commandService.DeleteCommand(canonicalName);
                    metaCommand.CustomCommandId = null;
                }
            }
        }
    }

    #endregion

}