using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    /// <summary>
    /// An entity that has documentation properties added so that it can be turned into 
    /// project documentation (HTML etc.)
    /// </summary>
    public interface IDocumentedEntity
    {

        /// <summary>
        /// The displayed description of the entity
        /// </summary>
         string Description { get; set; }

        /// <summary>
        /// Additional developer notes
        /// </summary>
         string Notes { get; set; }

    }
}
