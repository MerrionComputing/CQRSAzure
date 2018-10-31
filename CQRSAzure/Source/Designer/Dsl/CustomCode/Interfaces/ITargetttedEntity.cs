using CQRSAzure.CQRSdsl.Dsl;

namespace CQRSAzure.CQRSdsl.CustomCode.Interfaces
{
    /// <summary>
    /// Indicates that a parameter is used to indicate the target for a command or query
    /// </summary>
    /// <remarks>
    /// This allows a command or query definition to indicate that a paremeter is used to tell which aggregate(s) it should run against - either directly or 
    /// by implied membership of 
    /// </remarks>
    public interface ITargettedParameterEntity
    {

        /// <summary>
        /// Does this parameter uniquely identify the aggregate to which the command/query applies?
        /// </summary>
        bool IsAggregateKey { get; set; }

        /// <summary>
        /// Does this parameter refer to the name of the identity group that the command/query should apply to?
        /// </summary>
        bool IsIdentityGroupName { get; set; }

    }
}
