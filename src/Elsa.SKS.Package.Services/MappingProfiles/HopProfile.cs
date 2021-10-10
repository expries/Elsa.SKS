using AutoMapper;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.MappingProfiles
{
    public class HopProfile : Profile
    {
        public HopProfile()
        {
            CreateMap<Hop, Package.BusinessLogic.Entities.Hop>()
                .IncludeAllDerived();
            
            CreateMap<Truck, Package.BusinessLogic.Entities.Truck>();
            CreateMap<Warehouse, Package.BusinessLogic.Entities.Warehouse>();
            CreateMap<TransferWarehouse, Package.BusinessLogic.Entities.TransferWarehouse>();
        }
    }
}