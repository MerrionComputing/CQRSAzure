using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    public class CreateModelCommand
        : ModelModificationCommandBase
    {

        /// <summary>
        /// The command type is "CREATE"
        /// </summary>
        public override CommandActionType ActionType
        {
            get
            {
                return CommandActionType.Create;
            }
        }

        /// <summary>
        /// The target type is "Model"
        /// </summary>
        public override CommandTargetType Target
        {
            get
            {
                return CommandTargetType.Model;
            }
        }

        /// <summary>
        /// is the command line a valid "Create model" command
        /// </summary>
        /// <param name="commandLine">
        /// The command passed in :- CREATE MODEL modelname
        /// </param>
        public override bool IsValidCommand(string commandLine)
        {
            return CreateModelCommand.IsValidCreateModelCommand(commandLine); 
        }


        public override string Example
        {
            get
            {
                return CommandLineParser.EXAMPLE_CREATE_MODEL;
            }
        }

        public CreateModelCommand(string modelNameToCreate)
            : base(modelNameToCreate)
        {
        }

        public static bool IsValidCreateModelCommand(string commandLine)
        {
            if (!string.IsNullOrWhiteSpace(commandLine))
            {
                string[] commandParts = commandLine.Split(' ');
                if (commandParts != null)
                {
                    if (commandParts.GetLength(0) >= 3)
                    {
                        // Is this a CREATE command?
                        if (commandParts[0].Equals(CommandLineParser.COMMAND_CREATE, StringComparison.OrdinalIgnoreCase))
                        {
                            // Is this command for a model..
                            if (commandParts[1].Equals(CommandLineParser.TARGET_MODEL, StringComparison.OrdinalIgnoreCase))
                            {
                                // Is there a valid model name?
                                return IsValidModelName(commandParts[2]);
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Validate the model name to make sure it is valid
        /// </summary>
        /// <param name="modelName">
        /// The name of the model we are going to use
        /// </param>
        public static bool IsValidModelName(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName) )
            {
                return false;
            }
            if (modelName.IndexOfAny("'@{}[];:/?,<.>!£$%^&*()-=+".ToCharArray()  ) >= 0)
            {
                // Illegal characters in the model name
                return false;
            }

            // If we get here then all is well
            return true;
        }
    }
}
