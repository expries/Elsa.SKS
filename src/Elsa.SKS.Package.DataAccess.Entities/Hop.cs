using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Hop
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? Id { get; set; }
        
        /// <summary>
        /// Unique CODE of the hop.
        /// </summary>
        /// <value>Unique CODE of the hop.</value>
        public string Code { get; set; }

        /// <summary>
        /// Description of the hop.
        /// </summary>
        /// <value>Description of the hop.</value>
        public string Description { get; set; }

        /// <summary>
        /// Delay processing takes on the hop.
        /// </summary>
        /// <value>Delay processing takes on the hop.</value>
        public int? ProcessingDelayMins { get; set; }

        /// <summary>
        /// Name of the location (village, city, ..) of the hop.
        /// </summary>
        /// <value>Name of the location (village, city, ..) of the hop.</value>
        [Required]
        public string LocationName { get; set; }
        
        /// <summary>
        /// Gets or Sets HopArrivals
        /// </summary>
        public virtual ICollection<HopArrival> HopArrivals { get; set; }
        
        /// <summary>
        /// Gets or Sets WarehouseNextHops
        /// </summary>
        public virtual ICollection<WarehouseNextHops> WarehouseNextHops { get; set; }

        /// <summary>
        /// Gets or Sets LocationCoordinates
        /// </summary>
        [Required]
        public virtual GeoCoordinates LocationCoordinates { get; set; }
    }
}