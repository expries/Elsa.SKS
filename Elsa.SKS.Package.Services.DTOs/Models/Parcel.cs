/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Elsa.SKS.Package.Services.DTOs.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Parcel
    { 
        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        [Required]

        [DataMember(Name="weight")]
        public float? Weight { get; set; }

        /// <summary>
        /// Gets or Sets Receipient
        /// </summary>
        [Required]

        [DataMember(Name="receipient")]
        public Receipient Receipient { get; set; }

        /// <summary>
        /// Gets or Sets Sender
        /// </summary>
        [Required]

        [DataMember(Name="sender")]
        public Receipient Sender { get; set; }
        
    }
}
