using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class CQRSdslDiagram
    {

        /// <summary>
        /// Text set to the background of the diagram - this should be blank when production ready
        /// but have the project URL until then
        /// </summary>
        public override string WatermarkText
        {
            get
            {
                return Dsl.CustomCode.UI.UserInterfaceMessages.WatermarkText; 
            }
        }

    }
}
