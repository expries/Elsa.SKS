using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHops
    {
        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        [Required]
        public int? TravelTimeInMinutes { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        [Required]
        public Hop Hop { get; set; }
    }
}