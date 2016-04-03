using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    public interface INamedEntity
    {

        /// <summary>
        /// The unique name by which the entity is known
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The model qualified name by which the entity is known
        /// </summary>
        /// <remarks>
        /// This is needed to allow uniqueness between models
        /// </remarks>
        string FullyQualifiedName { get; }
    }
}
