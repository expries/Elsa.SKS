using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Package.DataAccess.Entities.Enums;

namespace Elsa.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Parcel
    {
        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        public float? Weight { get; set; }

        /// <summary>
        /// Gets or Sets Recipient
        /// </summary>
        public User Recipient { get; set; }

        /// <summary>
        /// Gets or Sets Sender
        /// </summary>
        public User Sender { get; set; }

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
        public List<HopArrival> VisitedHops { get; set; }

        /// <summary>
        /// Hops coming up in the future - their times are estimations.
        /// </summary>
        /// <value>Hops coming up in the future - their times are estimations.</value>
        public List<HopArrival> FutureHops { get; set; }
    }
}