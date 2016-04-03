
namespace CQRSAzure.CQRSdsl.Dsl
{
    public partial class CommandDefinitionCompartmentShapeBase
    {


        /// <summary>
        /// Access to the CommandDefinition class that underlies this command definition shape
        /// </summary>
        CommandDefinition BaseClass
        {
            get
            {
                return this.ModelElement as CommandDefinition;
            }
        }

    }
}
