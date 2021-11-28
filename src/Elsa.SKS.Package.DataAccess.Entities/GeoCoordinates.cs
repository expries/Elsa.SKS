using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class GeoCoordinates
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? Id { get; set; }
        
        /// <summary>
        /// Latitude of the coordinate.
        /// </summary>
        /// <value>Latitude of the coordinate.</value>
        public double? Lat { get; set; }

        /// <summary>
        /// Longitude of the coordinate.
        /// </summary>
        /// <value>Longitude of the coordinate.</value>
        public double? Lon { get; set; }
        
        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        [Required]
        public virtual Hop Hop { get; set; }
    }
}