//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Accounts.Account.projection;

namespace Accounts.Account.classifier
{



    /// <summary>
    /// Accounts with a balance greater than zero
    /// </summary>
    public partial interface IAccounts_In_Credit_Classifier 
        : CQRSAzure.IdentifierGroup.IClassifierProjectionHandler<IRunning_Balance>
    {
    }
}