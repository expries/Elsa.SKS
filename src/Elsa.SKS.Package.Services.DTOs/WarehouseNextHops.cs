/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Elsa.SKS.Package.Services.DTOs
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHops 
    { 
        /// <summary>
        /// Gets or Sets TraveltimeMins
        /// </summary>
        [Required]
        [DataMember(Name="traveltimeMins")]
        public int? TravelTimeInMinutes { get; set; }

        /// <summary>
        /// Gets or Sets Hop
        /// </summary>
        [Required]
        [DataMember(Name="hop")]
        public Hop Hop { get; set; }
    }
}