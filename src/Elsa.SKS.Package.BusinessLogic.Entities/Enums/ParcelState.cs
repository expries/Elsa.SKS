using System.Runtime.Serialization;

namespace Elsa.SKS.Package.BusinessLogic.Entities.Enums
{
    /// <summary>
    /// State of the parcel.
    /// </summary>
    /// <value>State of the parcel.</value>
    public enum ParcelState
    {
        /// <summary>
        /// Enum PickupEnum for Pickup
        /// </summary>
        Pickup = 0,
        /// <summary>
        /// Enum InTransportEnum for InTransport
        /// </summary>
        InTransport = 1,
        /// <summary>
        /// Enum InTruckDeliveryEnum for InTruckDelivery
        /// </summary>
        InTruckDelivery = 2,
        /// <summary>
        /// Enum TransferredEnum for Transferred
        /// </summary>
        Transferred = 3,
        /// <summary>
        /// Enum DeliveredEnum for Delivered
        /// </summary>
        Delivered = 4        
    }
}