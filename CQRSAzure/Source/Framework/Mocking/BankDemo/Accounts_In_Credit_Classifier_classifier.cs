//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using CQRSAzure.IdentifierGroup;
using Accounts.Account.projection;

namespace Accounts.Account.classifier
{


    /// <summary>
    /// Accounts with a balance greater than zero
    /// </summary>
    [CQRSAzure.EventSourcing.DomainNameAttribute("Accounts")]
    [CQRSAzure.EventSourcing.Category("Financial")]
    public partial class Accounts_In_Credit_Classifier : object, IAccounts_In_Credit_Classifier
    {
        
        /// <summary>
        /// Empty constructor for serialisation
        /// This should be removed if serialisation is not needed
        /// </summary>
        public Accounts_In_Credit_Classifier()
        {
        }
        
        /// <summary>
        /// The running balance of the account
        /// </summary>
        public IClassifierDataSourceHandler.EvaluationResult EvaluateProjection(IRunning_Balance projectionToEvaluate)
        {
            if ((projectionToEvaluate.Balance >= 0m))
            {
                return IClassifierDataSourceHandler.EvaluationResult.Include;
            }
            return IClassifierDataSourceHandler.EvaluationResult.Exclude;
        }
    }
}