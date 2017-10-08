using System;
using System.Diagnostics;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Attribute used to specify where command features get installed in the command bar UI hierarchy.
    /// This attributes can be used multiple times on a single command class to add command to several places.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class CommandPlacementAttribute : Attribute
    {
        private string m_commandBarPath;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="commandBarPath">Path in UI where command should be placed - e.g. 'MenuBar|View[1]'.</param>
        public CommandPlacementAttribute(string commandBarPath)
        {
            if (string.IsNullOrEmpty(commandBarPath)) throw new ArgumentNullException("commandBarPath");
            m_commandBarPath = commandBarPath;
        }

        /// <summary>
        /// Gets command bar path where the command should be placed.
        /// </summary>
        public string CommandBarPath
        {
            [DebuggerStepThrough]
            get { return m_commandBarPath; }
        }
    }
}