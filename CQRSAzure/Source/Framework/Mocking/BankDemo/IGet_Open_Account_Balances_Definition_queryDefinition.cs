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
    
    
    public partial interface IGet_Open_Account_Balances_Definition_Return
    {
        
        /// <summary>
        /// Unique number of the account
        /// </summary>
        string Account_Number
        {
            get;
        }
        
        /// <summary>
        /// The balance of the account as at the given date
        /// </summary>
        decimal Balance
        {
            get;
        }
    }
    
    /// <summary>
    /// Get the current balance for open accounts
    /// </summary>
    /// <remarks>
    /// Retrieves the account balances for all open accounts
    /// </remarks>
    public partial interface IGet_Open_Account_Balances_Definition : IQueryDefinition<IEnumerable<IGet_Open_Account_Balances_Definition_Return>>
    {
        
        /// <summary>
        /// The effective date for which to get the balance
        /// </summary>
        System.DateTime As_Of_Date
        {
            get;
        }
    }
}
