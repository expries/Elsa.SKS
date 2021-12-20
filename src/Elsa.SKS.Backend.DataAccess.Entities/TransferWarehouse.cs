using System.Diagnostics.CodeAnalysis;
using NetTopologySuite.Geometries;

namespace Elsa.SKS.Backend.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class TransferWarehouse : Hop
    {
        /// <summary>
        /// GeoRegion of the area covered by the logistics partner.
        /// </summary>
        /// <value>GeoJSON of the are covered by the logistics partner.</value>
        public virtual Geometry GeoRegion { get; set; }

        /// <summary>
        /// Name of the logistics partner.
        /// </summary>
        /// <value>Name of the logistics partner.</value>
        public string LogisticsPartner { get; set; }

        /// <summary>
        /// BaseURL of the logistics partner&#x27;s REST service.
        /// </summary>
        /// <value>BaseURL of the logistics partner&#x27;s REST service.</value>
        public string LogisticsPartnerUrl { get; set; }
    }
}