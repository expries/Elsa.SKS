using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.BusinessLogic.MappingProfiles
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
            CreateMap<Warehouse, DataAccess.Entities.Warehouse>().ReverseMap();
            CreateMap<WarehouseNextHops, DataAccess.Entities.WarehouseNextHops>().ReverseMap();
            CreateMap<Hop, DataAccess.Entities.Hop>().ReverseMap();
            CreateMap<GeoCoordinate, DataAccess.Entities.GeoCoordinate>().ReverseMap();
        }
    }

}