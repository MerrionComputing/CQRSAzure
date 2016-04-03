using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.CSharp;
using CQRSAzure.CQRSdsl.CustomCode.Interfaces;

namespace CQRSAzure.CQRSdsl.Dsl
{
    /// <summary>
    /// Functions to help the user interface dealing with aggregate identifier records
    /// </summary>
    public partial class AggregateIdentifier
    {

        /// <summary>
        /// Get a list of the name sof all the events this aggregate identifier has
        /// </summary>
        public IList<string> GetEventNames()
        {

            var qryName = from f in this.EventDefinitions
                    select f.Name;

            return qryName.ToList();

        }

        /// <summary>
        /// Gets the list of event property names for the selected event
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public IList<string> GetEventFieldNames(string eventName)
        {

            EventDefinition  evt = this.EventDefinitions.Find(f => f.Name == eventName);
            if (null != evt)
            {
                var qryEvtName = from ep in evt.EventProperties
                                 select ep.Name;

                return qryEvtName.ToList();
            }
            else
            {
                return new List<string>();
            }

        }

        /// <summary>
        /// Get the list of identity group names for this aggregate identifier
        /// </summary>
        /// <returns></returns>
        public IList<string> GetIdentityGroupNames()
        {

            var qryName = from f in this.IdentityGrouped 
                          select f.Name;

            return qryName.ToList();

        }

        /// <summary>
        /// Gets the lsit of projection names for this aggregate identifier
        /// </summary>
        public IList<string > GetProjectionNames()
        {
            var qryName = from f in this.ProjectionDefinitions 
                          select f.Name;

            return qryName.ToList();
        }
            
        /// <summary>
        /// Gets the list of names of query definition on this aggregate identifier
        /// </summary>
        public IList<string> GetQueryNames()
        {
            var qryName = from f in this.QueryDefinitions
                          select f.Name;

            return qryName.ToList();
        }

        /// <summary>
        /// Gets the list of commands on this aggregate identifier
        /// </summary>
        public IList<string > GetCommandNames()
        {
            var qryName = from f in this.CommandDefinitions 
                          select f.Name;

            return qryName.ToList();
        }

        /// <summary>
        /// Gets a list of all the classifiers linked to this aggregate identifier
        /// </summary>
        public IList<string> GetClassifierNames()
        {
            var qryName = from f in this.Classifiers
                          select f.Name;

            return qryName.ToList();
        }
    }
}
