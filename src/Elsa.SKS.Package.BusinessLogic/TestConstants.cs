using System.Collections.Generic;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.Package.BusinessLogic
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
        
        public const string ExistentWareHouseCode = "ABCDEF123";
        
        public const string NonExistentWarehouseCode = "000000000";

        public const string FaultyWarehouseCode = "XXXXXXXXX";

        public const string NonExistentHopCode = "000000000";

        public const string ExistentHopCode = "ABCDEF123";

        public const string TrackingIdOfParcelThatCanNotBeReported = "XXXXXXXXX";

        public static Warehouse ExistingWarehouses = new Warehouse
        {
            Code = "W123456789",
            NextHops = new List<WarehouseNextHops>
            {
                new WarehouseNextHops() { Hop = new Truck() },
                new WarehouseNextHops() { Hop = new Truck() }
            }
        };


    }
}