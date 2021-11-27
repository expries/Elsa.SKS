using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHop
    {
        /// <summary>
        /// Gets or Sets TravelTimeInMinutes
        /// </summary>
        public int? TravelTimeInMinutes { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        public Hop NextHop { get; set; }
    }
}