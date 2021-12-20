using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHop
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or Sets TravelTimeInMinutes
        /// </summary>
        public int? TravelTimeInMinutes { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        public virtual Hop NextHop { get; set; }

        /// <summary>
        /// Gets or Sets Warehouse
        /// </summary>
        [Required]
        public virtual Warehouse Warehouse { get; set; }
    }
}