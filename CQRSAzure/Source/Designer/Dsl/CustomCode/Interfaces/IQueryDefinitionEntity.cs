using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface IQueryDefinitionEntity
        : IDocumentedEntity, INamedEntity, ICategorisedEntity
    {

        /// <summary>
        /// Does the query return a table of data (multi-row) as opposed to a single record
        /// </summary>
        bool MultiRowResults { get; set; }

    }
}
