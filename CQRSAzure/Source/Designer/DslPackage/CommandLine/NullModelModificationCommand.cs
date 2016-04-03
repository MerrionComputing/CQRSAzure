using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    /// <summary>
    /// A model modification command that does not do anything 
    /// </summary>
    /// <remarks>
    /// This is retruned if the command line parser does not understand the command
    /// </remarks>
    public class NullModelModificationCommand
        :   IModelModificationCommand
    {
        public  ModelModificationCommandBase.CommandActionType ActionType
        {
            get
            {
                return ModelModificationCommandBase.CommandActionType.NoAction;
            }
        }

        public  string ModelName
        {
            get
            {
                return string.Empty;
            }
        }

        public  ModelModificationCommandBase.CommandTargetType Target
        {
            get
            {
                return ModelModificationCommandBase.CommandTargetType.Unknown;
            }
        }

        private readonly string m_commandLine;
        public string CommandLine
        {
            get
            {
                return m_commandLine;
            }
        }

        public NullModelModificationCommand(string commandLine)
        {
            m_commandLine = commandLine; 
        } 
    }
}
