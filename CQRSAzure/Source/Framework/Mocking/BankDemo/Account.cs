//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Accounts.Account
{


    /// <summary>
    /// The individual account
    /// </summary>
    [CQRSAzure.EventSourcing.DomainNameAttribute("Accounts")]
    public partial class Account : object, IAccount
    {
        
        private string _Account_Number;
        
        /// <summary>
        /// Empty constructor for serialisation
        /// This should be removed if serialisation is not needed
        /// </summary>
        Account()
        {
        }
        
        /// <summary>
        /// Create an instance of the aggregate from its key identifier
        /// </summary>
        public Account(string Account_Number_In)
        {
            _Account_Number = Account_Number_In;
        }
        
        /// <summary>
        /// Returns the unique identifier of this Account
        /// </summary>
        public string GetAggregateIdentifier()
        {
            return _Account_Number;
        }
        
        /// <summary>
        /// Returns the unique identifier of this Account
        /// </summary>
        public string GetKey()
        {
            return _Account_Number;
        }
        
        /// <summary>
        /// Sets the unique identifier of this Account
        /// </summary>
        public void SetKey(string Account_Number_In)
        {
            _Account_Number = Account_Number_In;
        }
    }
}
