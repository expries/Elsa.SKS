using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class GeoCoordinates
    {
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
    }
}