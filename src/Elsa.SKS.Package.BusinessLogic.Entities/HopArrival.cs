using System;
using System.ComponentModel.DataAnnotations;

namespace Elsa.SKS.Package.BusinessLogic.Entities
{
    public class HopArrival
    {
        /// <summary>
        /// Reference of Hop.
        /// </summary>
        /// <value>Reference of Hop.</value>
        [Required]
        public Hop Hop { get; set; }
        
        /// <summary>
        /// The date/time the parcel arrived at the hop.
        /// </summary>
        /// <value>The date/time the parcel arrived at the hop.</value>
        [Required]
        public DateTime? DateTime { get; set; }
    }
}