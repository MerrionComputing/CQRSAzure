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

namespace Accounts.Account.eventDefinition
{


    /// <summary>
    /// The account was closed
    /// </summary>
    [Serializable()]
    [CQRSAzure.EventSourcing.DomainNameAttribute("Accounts")]
    [CQRSAzure.EventSourcing.EventAsOfDateAttribute("Date_Closed")]
    public partial class Closed : 
        object, 
        IClosed,
        CQRSAzure.EventSourcing.IEvent<Account>
    {
        
        // Version number - always increment this if the event definition changes
        public const int EVENT_VERSION = 0;
        
        #region Private members
        // TODO: Make this property read only
        private System.DateTime _Date_Closed;
        
        // TODO: Make this property read only
        private string _Reason;
        #endregion
        
        /// <summary>
        /// Empty constructor For serialisation
        /// This should be removed If serialisation Is Not needed
        /// </summary>
        public Closed()
        {
        }
        
        /// <summary>
        /// Create And populate a New instance Of this Class from the underlying Interface
        /// </summary>
        /// <remarks>
        /// This should be called When the Event Is created from an Event stream
        /// </remarks>
        public Closed(IClosed ClosedInit)
        {
            _Date_Closed = ClosedInit.Date_Closed;
            _Reason = ClosedInit.Reason;
        }
        
        /// <summary>
        /// Create And populate a New instance Of this Class from the underlying properties
        /// </summary>
        /// <remarks>
        /// This should be called When the Event Is created from an Event stream
        /// </remarks>
        /// <param name="Date Closed">
        /// The date as of which the account was closed
        /// </param>
        /// <param name="Reason">
        /// Why the account was closed
        /// </param>
        public Closed(System.DateTime Date_Closed_In, string Reason_In)
        {
            _Date_Closed = Date_Closed_In;
            _Reason = Reason_In;
        }
        
        /// <summary>
        /// Create And populate a New instance Of this Class from the serialized data
        /// </summary>
        /// <param name="info">
        /// The SerializationInfo passed In containing the values Of this Event
        /// </param>
        /// <param name="context">
        /// Additional StreamingContext On how the Event Is streamed
        /// </param>
        Closed(SerializationInfo info, StreamingContext context)
        {
            _Date_Closed = info.GetDateTime("Date_Closed");
            _Reason = info.GetString("Reason");
        }
        
        public uint Version
        {
            get
            {
                return EVENT_VERSION;
            }
        }
        
        /// <summary>
        /// The date as of which the account was closed
        /// </summary>
        public System.DateTime Date_Closed
        {
            get
            {
                return _Date_Closed;
            }
        }
        
        /// <summary>
        /// Why the account was closed
        /// </summary>
        public string Reason
        {
            get
            {
                return _Reason;
            }
        }
        
        /// <summary>
        /// Factory method To create an instance Of this Event
        /// </summary>
        /// <param name="Date Closed">
        /// The date as of which the account was closed
        /// </param>
        /// <param name="Reason">
        /// Why the account was closed
        /// </param>
        static IClosed Create(System.DateTime Date_Closed_In, string Reason_In)
        {
            return new Closed(Date_Closed_In, Reason_In);
        }
        
        /// <summary>
        /// Populates a SerializationInfo with the data needed to serialize this event instance
        /// </summary>
        /// <remarks>
        /// The version number is also to be saved
        /// </remarks>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Date_Closed", _Date_Closed);
            info.AddValue("Reason", _Reason);
            info.AddValue("EVENT_VERSION", EVENT_VERSION);
        }
    }
}
