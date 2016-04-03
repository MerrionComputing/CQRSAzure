using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    /// <summary>
    /// A command line to add an aggregate identifier to a model
    /// </summary>
    public class AddAggregateCommand
        : ModelModificationCommandBase
    {

        /// <summary>
        /// The command type is ADD
        /// </summary>
        public override CommandActionType ActionType
        {
            get
            {
                return CommandActionType.Create;
            }
        }

        /// <summary>
        /// The command target is AGGREGATE
        /// </summary>
        public override CommandTargetType Target
        {
            get
            {
                return CommandTargetType.Aggregate;
            }
        }

        private readonly string m_aggregateName;
        /// <summary>
        /// The name of the aggregate to create with this command
        /// </summary>
        public string AggregateName
        {
            get
            {
                return m_aggregateName;
            }
        }

        public override string Example
        {
            get
            {
                return CommandLineParser.EXAMPLE_CREATE_AGGREGATE;
            }
        }

        public override bool IsValidCommand(string commandLine)
        {
            return IsValidAddAggregateCommand(commandLine);
        }


        public AddAggregateCommand(string modelName, string aggregateName)
            : base(modelName )
        {
            m_aggregateName = aggregateName;
        }


        public static bool IsValidAddAggregateCommand(string commandLine)
        {
            if (!string.IsNullOrWhiteSpace(commandLine))
            {
                string[] commandParts = commandLine.Split(' ');
                if (commandParts != null)
                {
                    if (commandParts.GetLength(0) >= 3)
                    {
                        // Is this an ADD command?
                        if (commandParts[0].Equals(CommandLineParser.COMMAND_ADD, StringComparison.OrdinalIgnoreCase))
                        {
                            // Is this command for an aggregate..
                            if (commandParts[1].Equals(CommandLineParser.TARGET_AGGREGATE, StringComparison.OrdinalIgnoreCase))
                            {
                                // Is there a valid aggregate name?
                                return IsValidAggregateName(commandParts[2]);
                            }
                        }
                    }
                }
            }
            return false;
        }

        public  static bool IsValidAggregateName(string aggregateName)
        {
            if (string.IsNullOrWhiteSpace(aggregateName))
            {
                return false;
            }
            if (aggregateName.IndexOfAny("'@{}[];:/?,<.>!£$%^&*()-=+".ToCharArray()) >= 0)
            {
                // Illegal characters in the model name
                return false;
            }

            // If we get here then all is well
            return true;
        }
    }
}
