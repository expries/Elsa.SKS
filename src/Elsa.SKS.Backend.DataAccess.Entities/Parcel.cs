using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Backend.DataAccess.Entities.Enums;

namespace Elsa.SKS.Backend.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Parcel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        public float? Weight { get; set; }

        /// <summary>
        /// Gets or Sets Recipient
        /// </summary>
        public virtual User Recipient { get; set; }

        /// <summary>
        /// Gets or Sets Sender
        /// </summary>
        public virtual User Sender { get; set; }

        /// <summary>
        /// The tracking ID of the parcel. 
        /// </summary>
        /// <value>The tracking ID of the parcel. </value>
        public string TrackingId { get; set; }

        /// <summary>
        /// State of the parcel.
        /// </summary>
        /// <value>State of the parcel.</value>
        public ParcelState? State { get; set; }

        /// <summary>
        /// Hops visited in the past.
        /// </summary>
        /// <value>Hops visited in the past.</value>
        public virtual List<HopArrival> VisitedHops { get; set; }

        /// <summary>
        /// Hops coming up in the future - their times are estimations.
        /// </summary>
        /// <value>Hops coming up in the future - their times are estimations.</value>
        public virtual List<HopArrival> FutureHops { get; set; }
    }
}