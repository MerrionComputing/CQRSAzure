using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CommandLine
{
    /// <summary>
    /// Interface defining a command that can be executed against a CQRS model
    /// </summary>
    public interface IModelModificationCommand
    {

        /// <summary>
        /// The name of the model being impacted by this command
        /// </summary>
        string ModelName { get; }

        /// <summary>
        /// The model object type / level that is the target of this command
        /// </summary>
        ModelModificationCommandBase.CommandTargetType Target { get; } 

        /// <summary>
        /// The action type being performed by this command
        /// </summary>
        ModelModificationCommandBase.CommandActionType ActionType { get; }

    }
}
