using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHops
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? Id { get; set; }
        
        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        public int? TravelTimeInMinutes { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        public virtual Hop Hop { get; set; }
    }
}