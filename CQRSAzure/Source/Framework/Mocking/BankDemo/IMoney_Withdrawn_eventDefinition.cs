//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using CQRSAzure.EventSourcing;
using Accounts.Account;

namespace Accounts.Account.eventDefinition
{

    
    /// <summary>
    /// Money was taken from the account
    /// </summary>
    public partial interface IMoney_Withdrawn : IEvent<IAccount>
    {
        
        /// <summary>
        /// The amount withdrawn
        /// </summary>
        /// <remarks>
        /// Thius is always in the account base currency
        /// </remarks>
        decimal Amount
        {
            get;
        }
        
        /// <summary>
        /// The date the money was withdrawn
        /// </summary>
        /// <remarks>
        /// The date withdrawn is also the date applied to the balance
        /// </remarks>
        System.DateTime Withdrawn_Date
        {
            get;
        }
        
        /// <summary>
        /// How the money was withdran
        /// </summary>
        /// <remarks>
        /// Over the counter, ATM, internet etc.
        /// </remarks>
        string Method
        {
            get;
        }
        
        /// <summary>
        /// Additional notes for the money withdrawal
        /// </summary>
        string Notes
        {
            get;
        }
    }
}