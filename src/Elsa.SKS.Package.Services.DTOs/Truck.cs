/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Elsa.SKS.Package.Services.DTOs.Enums;

namespace Elsa.SKS.Package.Services.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ExcludeFromCodeCoverage]
    public class Truck : Hop
    { 
        /// <summary>
        /// GeoJSON of the are covered by the truck.
        /// </summary>
        /// <value>GeoJSON of the are covered by the truck.</value>
        [DataMember(Name="regionGeoJson")]
        public string RegionGeoJson { get; set; }

        /// <summary>
        /// The truck&#x27;s number plate.
        /// </summary>
        /// <value>The truck&#x27;s number plate.</value>
        [DataMember(Name="numberPlate")]
        public string NumberPlate { get; set; }

        public Truck()
        {
            HopType = HopType.Truck;
        }
    }
}
