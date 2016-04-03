using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    /// <summary>
    /// Optional command part that specifies which CQRS model the rest of the command applies to
    /// </summary>
    /// <remarks>
    /// If a command does not include this then the current (last selected) model is assumed
    /// </remarks>
    public class UsingModelCommand
        : ModelCommandPartBase 
    {

        private readonly string m_modelName;
        public string ModelName
        {
            get
            {
                return m_modelName;
            }
        }

        public override string Example
        {
            get
            {
                return CommandLineParser.EXAMPLE_USING_MODEL; 
            }
        }

        public UsingModelCommand(string modelname)
        {
            if (! CreateModelCommand.IsValidModelName(modelname) )
            {
                throw new ArgumentException(modelname + " is not a valid model name ", nameof(modelname));
            }
            m_modelName = modelname;
        }

        /// <summary>
        /// Does the command line include a valid USING [modelname] section
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        public static bool IsValidUsingModelCommand(string commandLine)
        {
            if (!string.IsNullOrWhiteSpace(commandLine))
            {
                string[] commandParts = commandLine.Split(' ');
                if (commandParts != null)
                {
                    if (commandParts.GetLength(0) >= 2)
                    {
                        // Is this a USING command?
                        if (commandParts[0].Equals(CommandLineParser.COMMAND_USING, StringComparison.OrdinalIgnoreCase))
                        {
                            // Is there a valid model name?
                            return CreateModelCommand.IsValidModelName(commandParts[1]);
                        }
                    }
                }
            }
            return false;
        }

    }
}
