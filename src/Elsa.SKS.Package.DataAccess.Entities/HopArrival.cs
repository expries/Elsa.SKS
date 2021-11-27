using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class HopArrival
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? Id { get; set; }
        
        /// <summary>
        /// Reference of Hop.
        /// </summary>
        /// <value>Reference of Hop.</value>
        [Required]
        public virtual Hop Hop { get; set; }
        
        /// <summary>
        /// The date/time the parcel arrived at the hop.
        /// </summary>
        /// <value>The date/time the parcel arrived at the hop.</value>
        public DateTime? DateTime { get; set; }
    }
}