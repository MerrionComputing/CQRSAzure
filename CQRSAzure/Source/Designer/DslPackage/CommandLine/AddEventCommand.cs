using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{

    /// <summary>
    /// Command to add an event to an aggregate
    /// </summary>
    public class AddEventCommand
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
                return CommandActionType.Create;
            }
        }

        public override string Example
        {
            get
            {
                return CommandLineParser.EXAMPLE_ADD_EVENT; 
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
            throw new NotImplementedException();
        }

        public AddEventCommand(string modelName,string eventName)
            :base(modelName )
        {
            if (! CreateModelCommand.IsValidModelName(modelName)  )
            {
                throw new ArgumentException(modelName + " is not a valid model name", nameof(modelName));
            }
            if (! AddEventCommand.IsValidEventName(eventName ) )
            {
                throw new ArgumentException(eventName + " is not a valid event name", nameof(eventName));
            }
            m_eventName = eventName;
        }


        public static bool IsValidEventName(string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                return false;
            }
            if (eventName.IndexOfAny("'@{}[];:/?,<.>!£$%^&*()-=+".ToCharArray()) >= 0)
            {
                // Illegal characters in the aggregate name
                return false;
            }

            // If we get here then all is well
            return true;
        }
    }
}
