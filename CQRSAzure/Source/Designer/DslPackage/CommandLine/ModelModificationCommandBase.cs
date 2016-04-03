using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    /// <summary>
    /// Base class for all model modification commands
    /// </summary>
    public abstract class ModelModificationCommandBase
        : ModelCommandPartBase, IModelModificationCommand
    {


        readonly string m_modelName;
        /// <summary>
        /// The name of the model this codification command is applicable to 
        /// </summary>
        public string ModelName
        {
            get {
                return m_modelName;
            }
        }
        

        /// <summary>
        /// What is the level of thing being impacted by the command line
        /// </summary>
        /// <remarks>
        /// If the level is not explicitly specified then the top level MODEL is assumed
        /// </remarks>
        public enum CommandTargetType
        {
            /// <summary>
            /// The command target cannot be determined
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// The command is for the MODEL
            /// </summary>
            Model = 1,
            /// <summary>
            /// The command is for an AGGREGATE identifier
            /// </summary>
            Aggregate = 2,
            /// <summary>
            /// The command is for a COMMAND
            /// </summary>
            CommandDefinition = 3,
            /// <summary>
            /// The command is for a QUERY definition
            /// </summary>
            QueryDefinition = 4,
            /// <summary>
            /// The command is for an EVENT
            /// </summary>
            Event = 5,
            /// <summary>
            /// The command is for a PROJECTION
            /// </summary>
            Projection = 6,
            /// <summary>
            /// The command is for a command PARAMETER
            /// </summary>
            CommandInputParameter = 7,
            /// <summary>
            /// The command is for a query input PARAMETER
            /// </summary>
            QueryInputParameter = 8,
            /// <summary>
            /// The command is for a query return PROPERTY
            /// </summary>
            QueryReturnProperty = 9,
            /// <summary>
            /// The command is for an event PROPERTY
            /// </summary>
            EventProperty = 10,
            /// <summary>
            /// The command is for a return property of a projection
            /// </summary>
            ProjectionProeprty = 11,
            /// <summary>
            /// The command is for the link between a projection and an event being handled
            /// </summary>
            ProjectionEventLink = 12,
            /// <summary>
            /// The command is for a property part of the projection specified
            /// </summary>
            ProjectionEventPropertyOperation = 13
        }

        /// <summary>
        /// The target type this command operates on
        /// </summary>
        public abstract CommandTargetType Target { get; }

        /// <summary>
        /// What action is the command taking
        /// </summary>
        public enum CommandActionType
        {
            /// <summary>
            /// The command is an empty command that does not do any action
            /// </summary>
            /// <remarks>
            /// This can be useful if the parser does not understand the command
            /// </remarks>
            NoAction = 0,
            /// <summary>
            /// Create (or add) a new thing 
            /// </summary>
            Create = 1,
            /// <summary>
            /// Delete an existing thing
            /// </summary>
            Delete = 2,
            /// <summary>
            /// Add notes./comments to a thing
            /// </summary>
            Document = 3,
            /// <summary>
            /// Mark an event property as depreciated (event properties cannot be deleted)
            /// </summary>
            Depreciate = 4,
            /// <summary>
            /// Increment the version number of an event
            /// </summary>
            IncrementVersion = 5,
            /// <summary>
            /// (Re)Generate code or documentation
            /// </summary>
            Generate = 6
        }

        /// <summary>
        /// The type of action being performed
        /// </summary>
        public abstract CommandActionType ActionType { get; }

        public ModelModificationCommandBase(string modelNameToOperateOn)
        {
            m_modelName = modelNameToOperateOn;
        }

        public abstract bool IsValidCommand(string commandLine);
    }

    /// <summary>
    /// Base class for command snippets that modify the command
    /// </summary>
    public abstract class ModelCommandModifierBase
        : ModelCommandPartBase
    {
    }

    /// <summary>
    /// Lowest level base class for any snippet that forms part of a well formed model
    /// command line
    /// </summary>
    public abstract class ModelCommandPartBase
    {

        /// <summary>
        /// An example of the syntax of this command line snippet
        /// </summary>
        /// <remarks>
        /// This allows a common feedback to the user when a command line is 
        /// incorrect or when help is sought
        /// </remarks>
        public abstract string Example { get; }

    }
}
