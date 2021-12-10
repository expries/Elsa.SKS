using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.Package.Webhooks.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebhookMessage : TrackingInformation
    { 
        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        [DataMember(Name="trackingId")]
        public string TrackingId { get; set; }
        
    }
}