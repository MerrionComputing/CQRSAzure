using System.ComponentModel.Design;

namespace CQRSAzure.CQRSdsl.Dsl
{

    #region INamedCommandService

    /// <summary>
    /// A wrapping service for command support in add-ins.
    /// </summary>
    public interface INamedCommandService : IMenuCommandService
    {
        /// <summary>
        /// Given command text produces a full name of an add-in command.
        /// </summary>
        /// <param name="text">Command text.</param>
        /// <returns>Command name to be used with all calls to this service.</returns>
        string GetCanonicalName(string text);

        /// <summary>
        /// Finds CommandID by canonical command name.
        /// </summary>
        /// <param name="commandName">Canonical command name (produced by GetCanonicalName).</param>
        /// <returns>CommandID if command exists and null otherwise.</returns>
        CommandID FindCommandId(string commandName);

        /// <summary>
        /// Finds MenuCommand by canonical command name.
        /// </summary>
        /// <param name="commandName">Canonical command name (produced by GetCanonicalName).</param>
        /// <returns>MenuCommand if it was previously added and null otherwise.</returns>
        MenuCommand FindCommand(string commandName);

        /// <summary>
        /// Creates a new command with given name, text and description.
        /// </summary>
        /// <param name="commandName">Canonical command name (produced by GetCanonicalName).</param>
        /// <param name="text">Command text which will appear in UI.</param>
        /// <param name="description">Command description for UI.</param>
        /// <returns>Created commmand's CommandID.</returns>
        CommandID CreateCommand(string commandName, string text, string description);

        /// <summary>
        /// Places given command on a command bar at specified command bar UI path.
        /// </summary>
        /// <param name="commandName">Canonical command name (produced by GetCanonicalName).</param>
        /// <param name="commandBarPath">Command bar path.</param>
        void PlaceCommand(string commandName, string commandBarPath);

        /// <summary>
        /// Deletes specified command.
        /// </summary>
        /// <param name="commandName">Canonical command name (produced by GetCanonicalName).</param>
        void DeleteCommand(string commandName);

        /// <summary>
        /// Invokes specified command.
        /// </summary>
        /// <param name="commandName">Canonical command name (produced by GetCanonicalName).</param>
        /// <returns>Whether command was actually invoked.</returns>
        bool GlobalInvoke(string commandName);
    }

    #endregion
}