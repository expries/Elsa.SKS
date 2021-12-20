using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Elsa.SKS.Backend.Services.DTOs;

namespace Elsa.SKS.Backend.Webhooks.DTOs
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
        [DataMember(Name = "trackingId")]
        public string TrackingId { get; set; }

    }
}