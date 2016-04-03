using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface IEventDefinitionEntity
        : IDocumentedEntity , INamedEntity, ICategorisedEntity
    {

        /// <summary>
        /// The current version number (revision) of the event
        /// </summary>
        /// <remarks>
        /// This is made read-only so we can only increment it
        /// </remarks>
        int Version { get;  }

    }
}
