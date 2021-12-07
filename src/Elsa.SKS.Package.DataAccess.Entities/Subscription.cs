using System;
using System.ComponentModel.DataAnnotations;

namespace Elsa.SKS.Package.DataAccess.Entities
{
    public class Subscription
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [Key]
        public long? Id { get; set; }

        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        public string TrackingId { get; set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>

        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>

        public DateTime? CreatedAt { get; set; }
    }
}