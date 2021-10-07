﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Elsa.SKS.Package.BusinessLogic.Entities.Enums;

namespace Elsa.SKS.Package.BusinessLogic.Entities
{
    public class Parcel
    {
        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        [Required]
        public float? Weight { get; set; } = 0;

        /// <summary>
        /// Gets or Sets Recipient
        /// </summary>
        [Required]
        public User Recipient { get; set; } = new User();

        /// <summary>
        /// Gets or Sets Sender
        /// </summary>
        [Required]
        public User Sender { get; set; } = new User();

        /// <summary>
        /// The tracking ID of the parcel. 
        /// </summary>
        /// <value>The tracking ID of the parcel. </value>
        [RegularExpression("^[A-Z0-9]{9}$")]
        public string TrackingId { get; set; } = String.Empty;

        /// <summary>
        /// State of the parcel.
        /// </summary>
        /// <value>State of the parcel.</value>
        [Required]
        public ParcelState? State { get; set; } = new ParcelState();

        /// <summary>
        /// Hops visited in the past.
        /// </summary>
        /// <value>Hops visited in the past.</value>
        [Required]
        public List<HopArrival> VisitedHops { get; set; } = new List<HopArrival>();

        /// <summary>
        /// Hops coming up in the future - their times are estimations.
        /// </summary>
        /// <value>Hops coming up in the future - their times are estimations.</value>
        [Required]
        public List<HopArrival> FutureHops { get; set; } = new List<HopArrival>();
    }
}