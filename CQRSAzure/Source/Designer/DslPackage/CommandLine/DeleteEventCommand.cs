using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    /// <summary>
    /// Command to remove an event from an aggregate
    /// </summary>
    public class DeleteEventCommand
        : ModelModificationCommandBase
    {

        private readonly string m_eventName;
        public string EventName
        {
            get
            {
                return m_eventName;
            }
        }

        public override CommandActionType ActionType
        {
            get
            {
                return CommandActionType.Delete;
            }
        }

        public override string Example
        {
            get
            {
                return CommandLineParser.COMMAND_DELETE; 
            }
        }

        public override CommandTargetType Target
        {
            get
            {
                return CommandTargetType.Event;
            }
        }

        public override bool IsValidCommand(string commandLine)
        {
            // TODO: Validate this command
            throw new NotImplementedException();
        }

        public DeleteEventCommand(string modelName, string eventName)
            :base(modelName )
        {
            if (!CreateModelCommand.IsValidModelName(modelName))
            {
                throw new ArgumentException(modelName + " is not a valid model name", nameof(modelName));
            }
            if (!AddEventCommand.IsValidEventName(eventName))
            {
                throw new ArgumentException(eventName + " is not a valid event name", nameof(eventName));
            }
            m_eventName = eventName;
        }
    }
}
