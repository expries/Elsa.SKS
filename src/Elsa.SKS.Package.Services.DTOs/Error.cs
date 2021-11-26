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

namespace Elsa.SKS.Package.Services.DTOs
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [ExcludeFromCodeCoverage]
    public class Error 
    { 
        /// <summary>
        /// The error message.
        /// </summary>
        /// <value>The error message.</value>
        [DataMember(Name="errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
