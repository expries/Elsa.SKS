using System.Collections.Generic;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS
{
    public static class TestConstants
    {
        public const string TrackingIdOfParcelThatIsTransferred = "ABCDEF123";
        
        public const string TrackingIdOfParcelThatIsNotTransferred = "123456ABC";

        public const string TrackingIdOfExistentParcel = "ABCDEF123";

        public const string TrackingIdOfNonExistentParcel = "000000000";

        public const string TrackingIdOfParcelThatCanNotBeTracked = "XXXXXXXXX";
        
        public const string TrackingIdOfSubmittedParcel = "ABCDEF123";
        
        public const string TrackingIdOfParcelThatCanNotBeDelivered = "XXXXXXXXX";

        public static Warehouse ExistingWarehouses = new Warehouse
        {
            NextHops = new List<WarehouseNextHops>
            {
                new WarehouseNextHops() { Hop = new Truck() },
                new WarehouseNextHops() { Hop = new Truck() }
            }
        };


    }
}