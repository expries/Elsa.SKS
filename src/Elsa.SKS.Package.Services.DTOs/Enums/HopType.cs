using System.Runtime.Serialization;

namespace Elsa.SKS.Package.Services.DTOs.Enums
{
    public enum HopType
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Warehouse")]
        Warehouse,
        [EnumMember(Value = "Truck")]
        Truck,
        [EnumMember(Value = "TransferWarehouse")]
        TransferWarehouse
    }
}