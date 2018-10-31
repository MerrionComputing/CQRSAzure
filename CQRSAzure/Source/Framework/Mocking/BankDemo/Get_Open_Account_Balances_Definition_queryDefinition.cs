//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Accounts.Account.queryDefinition
{
    using System;
    using System.Collections.Generic;
    using CQRSAzure.EventSourcing;
    using CQRSAzure.QueryDefinition;
    using Accounts.Account;
    
    
    /// <summary>
    /// Get the current balance for open accounts
    /// </summary>
    /// <remarks>
    /// Retrieves the account balances for all open accounts
    /// </remarks>
    [CQRSAzure.EventSourcing.DomainNameAttribute("Accounts")]
    [CQRSAzure.EventSourcing.Category("Financial")]
    [CQRSAzure.EventSourcing.IdentityGroup("Open Accounts")]
    public partial class Get_Open_Account_Balances_Definition : QueryDefinitionBase<IEnumerable<IGet_Open_Account_Balances_Definition_Return>>, IGet_Open_Account_Balances_Definition
    {
        
        /// <summary>
        /// The unique name of this query
        /// Get Open Account Balances
        /// </summary>
        public override string QueryName
        {
            get
            {
                return "Get Open Account Balances";
            }
        }
        
        /// <summary>
        /// The effective date for which to get the balance
        /// </summary>
        public System.DateTime As_Of_Date
        {
            get
            {
                return base.GetParameterValue<System.DateTime>("As Of Date", 0);
            }
            set
            {
                base.SetParameterValue("As Of Date", 0, ref value);
            }
        }
    }
}
