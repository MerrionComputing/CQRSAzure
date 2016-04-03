using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    /// <summary>
    /// Base class for commands to add documentation information to an object
    /// </summary>
    /// <remarks>
    ///   [USING modelname]
    /// DOCUMENT [AGGREGATE | QUERY | COMMAND | EVENT | PROJECTION] objectname
    ///  DESCRIPTION => ''
    ///  NOTES => ''
    /// </remarks>
    public abstract class DocumentObjectCommandBase
        : ModelModificationCommandBase
    {

        /// <summary>
        /// The unique name (within the model) of the item being documented
        /// </summary>
        /// <remarks>
        /// This may be a dot-seperated hierrachical name (e.g. aggeregate.event.eventproperty)
        /// </remarks>
        private readonly string m_objectName;
        public string ObjectName
        {
            get
            {
                return m_objectName;
            }
        }

        private readonly string m_description;
        public string Description {
            get
            {
                return m_description;
            }
        }

        private readonly string m_notes;
        public string Notes {
            get
            {
                return m_notes;
            }
        }


        public override CommandActionType ActionType
        {
            get
            {
                return CommandActionType.Document;
            }
        }

        public DocumentObjectCommandBase(string modelName, 
            CommandTargetType target, 
            string objectName,
            string description,
            string notes)
            : base(modelName )
        {
            m_description = description;
            m_notes = notes;
            m_objectName = objectName;
        }
    }

    public class DocumentAggregateCommand
        : DocumentObjectCommandBase
    {

        public DocumentAggregateCommand(string modelName,
            string objectName,
            string description,
            string notes)
            : base(modelName , 
                  CommandTargetType.Aggregate ,
                  objectName ,
                  description,
                  notes)
        {

        }

        public override string Example
        {
            get
            {
                // TODO : Return the document example specific to aggregate?
                return CommandLineParser.EXAMPLE_DOCUMENT; 
            }
        }

        public override CommandTargetType Target
        {
            get
            {
                return CommandTargetType.Aggregate;
            }
        }

        public override bool IsValidCommand(string commandLine)
        {
            // TODO: Validate the command line given..
            throw new NotImplementedException();
        }
    }

    public class DocumentQueryDefinitionCommand
        : DocumentObjectCommandBase
    {

        public DocumentQueryDefinitionCommand(string modelName,
                                            string objectName,
                                            string description,
                                            string notes)
            : base(modelName , 
                  CommandTargetType.QueryDefinition  ,
                  objectName ,
                  description,
                  notes)
        {

        }

        public override string Example
        {
            get
            {
                // TODO : Return the document example specific to query definition?
                return CommandLineParser.EXAMPLE_DOCUMENT;
            }
        }

        public override CommandTargetType Target
        {
            get
            {
                return CommandTargetType.QueryDefinition;
            }
        }

        public override bool IsValidCommand(string commandLine)
        {
            throw new NotImplementedException();
        }
    }

    public class DocumentCommandDefinitionCommand
        : DocumentObjectCommandBase 
    {


        public DocumentCommandDefinitionCommand(string modelName,
                                    string objectName,
                                    string description,
                                    string notes)
            : base(modelName , 
                  CommandTargetType.CommandDefinition   ,
                  objectName ,
                  description,
                  notes)
        {

        }

        public override string Example
        {
            get
            {
                // TODO : Return the document example specific to command definition?
                return CommandLineParser.EXAMPLE_DOCUMENT;
            }
        }

        public override CommandTargetType Target
        {
            get
            {
                return CommandTargetType.CommandDefinition;
            }
        }

        public override bool IsValidCommand(string commandLine)
        {
            throw new NotImplementedException();
        }
    }

    public class DocumentEventDefinitionCommand
        : DocumentObjectCommandBase 
    {

        public DocumentEventDefinitionCommand(string modelName,
                            string objectName,
                            string description,
                            string notes)
            : base(modelName , 
                  CommandTargetType.Event    ,
                  objectName ,
                  description,
                  notes)
        {

        }

        public override string Example
        {
            get
            {
                // TODO : Return the document example specific to command definition?
                return CommandLineParser.EXAMPLE_DOCUMENT;
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
    }

    public class DocumentProjectionDefinitionCommand
        : DocumentObjectCommandBase
    {

        public DocumentProjectionDefinitionCommand(string modelName,
                    string objectName,
                    string description,
                    string notes)
            : base(modelName , 
                  CommandTargetType.Projection     ,
                  objectName ,
                  description,
                  notes)
        {

        }

        public override string Example
        {
            get
            {
                // TODO : Return the document example specific to command definition?
                return CommandLineParser.EXAMPLE_DOCUMENT;
            }
        }

        public override CommandTargetType Target
        {
            get
            {
                return CommandTargetType.Projection;
            }
        }

        public override bool IsValidCommand(string commandLine)
        {
            throw new NotImplementedException();
        }
    }

    // TODO : Documentation of properties and projection-event links
}
