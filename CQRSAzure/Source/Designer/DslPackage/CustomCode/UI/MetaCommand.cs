using System;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace CQRSAzure.CQRSdsl.Dsl
{
    #region MetaCommand

    /// <summary>
    /// Meta-feature for commands. Keeps track of CommandID as well as
    /// whether command is custom or user-created. Provides installer and
    /// proper instantiation of MenuCommand objects.
    /// </summary>
    public sealed class MetaCommand : MetaFeature
    {
        private bool m_isCustomCommand;
        private CommandID m_customCommandId;

        public MetaCommand(MetaFeatureStore store, Type featureType)
            : base(store, featureType)
        {
            CommandAttribute commandData = ReflectionHelper.GetAttribute<CommandAttribute>(featureType);
            if (commandData == null)
                throw new ArgumentNullException("Feature type must have a CommandAttribute to be a command.");

            // It is a custom command if there is no default constructor on the class. In case when
            // there is a default constructor, user has already passed in the CommandID to the base class
            // and thus it is a known command.
            m_isCustomCommand = featureType.GetConstructor(new Type[0]) == null;
        }

        /// <summary>
        /// Gets whether command is custom-created (true) or predefined (false);
        /// </summary>
        public bool IsCustomCommand
        {
            [DebuggerStepThrough]
            get { return m_isCustomCommand; }
        }

        /// <summary>
        /// Gets command ID in case of a custom command.
        /// </summary>
        public CommandID CustomCommandId
        {
            [DebuggerStepThrough]
            get { return m_customCommandId; }
            [DebuggerStepThrough]
            internal set { m_customCommandId = value; }
        }

        /// <summary>
        /// Gets installer capable of installing/uninstalling this meta-feature at runtime.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override IFeatureInstaller Installer
        {
            [DebuggerStepThrough]
            get { return new CommandInstaller(this.Store); }
        }

        /// <summary>
        /// Does the actual work of instantiating feature object before Associate is called
        /// from CreateFeature (if required).
        /// </summary>
        /// <param name="serviceProvider">Context the feature is being created in.</param>
        /// <returns>Created feature object.</returns>
        protected override IDisposable DoCreateFeature(ITypedServiceProvider serviceProvider)
        {
            if (this.IsCustomCommand)
            {
                // Custom command requires CommandID to be passed in.
                return Activator.CreateInstance(this.FeatureType, this.CustomCommandId) as IDisposable;
            }
            else
            {
                // Predefined commands have default constructors.
                return Activator.CreateInstance(this.FeatureType) as IDisposable;
            }
        }
    }

    #endregion
}