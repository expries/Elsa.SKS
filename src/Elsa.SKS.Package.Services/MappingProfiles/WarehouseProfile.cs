using AutoMapper;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class WarehouseProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public WarehouseProfile()
        {
            CreateMap<Warehouse, Package.BusinessLogic.Entities.Warehouse>().ReverseMap();
            CreateMap<WarehouseNextHops, Package.BusinessLogic.Entities.WarehouseNextHops>().ReverseMap();
            CreateMap<Hop, Package.BusinessLogic.Entities.Hop>().ReverseMap();
            CreateMap<GeoCoordinate, Package.BusinessLogic.Entities.GeoCoordinate>().ReverseMap();
        }
    }

}