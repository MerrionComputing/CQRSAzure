using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    public class DeleteAggregateCommand
        : ModelModificationCommandBase
    {
        public override CommandActionType ActionType
        {
            get
            {
                return CommandActionType.Delete;
            }
        }

        public override CommandTargetType Target
        {
            get
            {
                return CommandTargetType.Aggregate;
            }
        }

        private readonly string m_aggregateName;
        /// <summary>
        /// The name of the aggregate to delete with this command
        /// </summary>
        public string AggregateName
        {
            get
            {
                return m_aggregateName;
            }
        }


        public override bool IsValidCommand(string commandLine)
        {
            throw new NotImplementedException();
        }

        public override string Example
        {
            get
            {
                return CommandLineParser.EXAMPLE_DELETE_AGGREGATE;
            }
        }

        public DeleteAggregateCommand(string modelName, string aggregateName)
            : base(modelName )
        {
            m_aggregateName = aggregateName;
        }

        public static bool IsValidDeleteAggregateCommand(string commandLine)
        {
            if (!string.IsNullOrWhiteSpace(commandLine))
            {
                string[] commandParts = commandLine.Split(' ');
                if (commandParts != null)
                {
                    if (commandParts.GetLength(0) >= 3)
                    {
                        // Is this a DELETE command?
                        if (commandParts[0].Equals(CommandLineParser.COMMAND_DELETE, StringComparison.OrdinalIgnoreCase))
                        {
                            // Is this command for an aggregate..
                            if (commandParts[1].Equals(CommandLineParser.TARGET_AGGREGATE, StringComparison.OrdinalIgnoreCase))
                            {
                                // Is there a valid aggregate name?
                                return AddAggregateCommand.IsValidAggregateName(commandParts[2]);
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
