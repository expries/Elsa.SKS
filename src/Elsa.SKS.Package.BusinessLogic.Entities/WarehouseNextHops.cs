using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHops
    {
        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        public int? TravelTimeInMinutes { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        public Hop Hop { get; set; }
    }
}