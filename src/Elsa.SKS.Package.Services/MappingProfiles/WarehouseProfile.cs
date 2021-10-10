using AutoMapper;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.MappingProfiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<Warehouse, Package.BusinessLogic.Entities.Warehouse>();
            CreateMap<WarehouseNextHops, Package.BusinessLogic.Entities.WarehouseNextHops>();
            CreateMap<Hop, Package.BusinessLogic.Entities.Hop>();
            CreateMap<GeoCoordinate, Package.BusinessLogic.Entities.GeoCoordinate>();
        }
    }

}