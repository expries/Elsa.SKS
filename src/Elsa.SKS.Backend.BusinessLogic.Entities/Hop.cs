using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class Hop
    {
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
        public int? ProcessingDelayMinutes { get; set; }

        /// <summary>
        /// Name of the location (village, city, ..) of the hop.
        /// </summary>
        /// <value>Name of the location (village, city, ..) of the hop.</value>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or Sets PreviousHop
        /// </summary>
        public WarehouseNextHop PreviousHop { get; set; }

        /// <summary>
        /// Gets or Sets LocationCoordinates
        /// </summary>
        public GeoCoordinates LocationCoordinates { get; set; }

        /// <summary>
        /// Gets or Sets Arrivals
        /// </summary>
        public List<HopArrival> Arrivals { get; set; }
    }
}