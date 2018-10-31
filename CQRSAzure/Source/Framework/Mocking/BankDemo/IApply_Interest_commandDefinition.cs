//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Accounts.Account.commandDefinition
{
    using System;
    using CQRSAzure.EventSourcing;
    using CQRSAzure.CommandDefinition;
    using Accounts.Account;
    
    
    /// <summary>
    /// Apply interest to each open account
    /// </summary>
    /// <remarks>
    /// This is run as an "overnight" batch process
    /// </remarks>
    public partial interface IApply_Interest_Definition : ICommandDefinition
    {
        
        /// <summary>
        /// Apply interest to all the open accounts
        /// </summary>
        double Interest_Rate
        {
            get;
        }
    }
}
