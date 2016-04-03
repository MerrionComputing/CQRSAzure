using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    /// <summary>
    /// Optional command line part that specifies the aggregate name for which this command pertains
    /// </summary>
    /// <remarks>
    /// If this is not specified then the command is applied to the most recently active aggregate identifier
    /// </remarks>
    public class ForAggregateCommand
        : ModelCommandPartBase
    {

        private readonly string m_aggregateName;
        public string AggregateName
        {
            get { return m_aggregateName; }
        }

        public override string Example
        {
            get
            {
                return CommandLineParser.EXAMPLE_FOR_AGGREGATE;
            }
        }

        public ForAggregateCommand(string aggregateName)
        {
            if (! ForAggregateCommand.IsValidAggregateName(aggregateName ) )
            {
                throw new ArgumentException(aggregateName + @" is not a valid aggregate name ", nameof(aggregateName)); 
            }
            m_aggregateName = aggregateName;
        }

        public static bool IsValidForAggregateCommand(string commandLine)
        {
            if (!string.IsNullOrWhiteSpace(commandLine))
            {
                string[] commandParts = commandLine.Split(' ');
                if (commandParts != null)
                {
                    if (commandParts.GetLength(0) >= 2)
                    {
                        // Is this a USING command?
                        if (commandParts[0].Equals(CommandLineParser.COMMAND_FOR, StringComparison.OrdinalIgnoreCase))
                        {
                            // Is there a valid aggregate name?
                            return ForAggregateCommand.IsValidAggregateName(commandParts[1]);
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsValidAggregateName(string aggregateName)
        {
            if (string.IsNullOrWhiteSpace(aggregateName))
            {
                return false;
            }
            if (aggregateName.IndexOfAny("'@{}[];:/?,<.>!£$%^&*()-=+".ToCharArray()) >= 0)
            {
                // Illegal characters in the aggregate name
                return false;
            }

            // If we get here then all is well
            return true;
        }
    }
}
