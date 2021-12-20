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

namespace Elsa.SKS.Backend.Services.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ExcludeFromCodeCoverage]
    public class Parcel
    {
        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        [DataMember(Name = "weight")]
        public float? Weight { get; set; }

        /// <summary>
        /// Gets or Sets Recipient
        /// </summary>
        [DataMember(Name = "recipient")]
        public Recipient Recipient { get; set; }

        /// <summary>
        /// Gets or Sets Sender
        /// </summary>
        [DataMember(Name = "sender")]
        public Recipient Sender { get; set; }
    }
}