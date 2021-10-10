using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Package.BusinessLogic.Entities.Enums;

namespace Elsa.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class Parcel
    {
        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        [Required]
        public float? Weight { get; set; }

        /// <summary>
        /// Gets or Sets Recipient
        /// </summary>
        [Required]
        public User Recipient { get; set; }

        /// <summary>
        /// Gets or Sets Sender
        /// </summary>
        [Required]
        public User Sender { get; set; }

        /// <summary>
        /// The tracking ID of the parcel. 
        /// </summary>
        /// <value>The tracking ID of the parcel. </value>
        [RegularExpression("^[A-Z0-9]{9}$")]
        public string TrackingId { get; set; }

        /// <summary>
        /// State of the parcel.
        /// </summary>
        /// <value>State of the parcel.</value>
        [Required]
        public ParcelState? State { get; set; }

        /// <summary>
        /// Hops visited in the past.
        /// </summary>
        /// <value>Hops visited in the past.</value>
        [Required]
        public List<HopArrival> VisitedHops { get; set; }

        /// <summary>
        /// Hops coming up in the future - their times are estimations.
        /// </summary>
        /// <value>Hops coming up in the future - their times are estimations.</value>
        [Required]
        public List<HopArrival> FutureHops { get; set; }
    }
}