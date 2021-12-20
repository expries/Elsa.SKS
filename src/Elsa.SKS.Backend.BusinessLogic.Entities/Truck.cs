using System.Diagnostics.CodeAnalysis;
using NetTopologySuite.Geometries;

namespace Elsa.SKS.Backend.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class Truck : Hop
    {
        /// <summary>
        /// GeoRegion of the area covered by the truck.
        /// </summary>
        /// <value>GeoJSON of the are covered by the truck.</value>
        public Geometry GeoRegion { get; set; }

        /// <summary>
        /// The truck&#x27;s number plate.
        /// </summary>
        /// <value>The truck&#x27;s number plate.</value>
        public string NumberPlate { get; set; }
    }
}