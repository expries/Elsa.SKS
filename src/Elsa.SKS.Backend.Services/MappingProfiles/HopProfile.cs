using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Backend.Services.DTOs;
using Elsa.SKS.Backend.Services.DTOs.Converters;

namespace Elsa.SKS.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HopProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public HopProfile()
        {
            CreateMap<Hop, Backend.BusinessLogic.Entities.Hop>().IncludeAllDerived();
            CreateMap<Backend.BusinessLogic.Entities.Hop, Hop>().IncludeAllDerived();
            CreateMap<Warehouse, Backend.BusinessLogic.Entities.Warehouse>().ReverseMap();

            CreateMap<TransferWarehouse, Backend.BusinessLogic.Entities.TransferWarehouse>()
                .ForMember(_ => _.GeoRegion, o => o.ConvertUsing(new GeometryConverter(), _ => _.RegionGeoJson));

            CreateMap<Backend.BusinessLogic.Entities.TransferWarehouse, TransferWarehouse>()
                .ForMember(_ => _.RegionGeoJson, o => o.ConvertUsing(new GeometryConverter(), _ => _.GeoRegion));

            CreateMap<Truck, Backend.BusinessLogic.Entities.Truck>()
                .ForMember(_ => _.GeoRegion, o => o.ConvertUsing(new GeometryConverter(), _ => _.RegionGeoJson));

            CreateMap<Backend.BusinessLogic.Entities.Truck, Truck>()
                .ForMember(_ => _.RegionGeoJson, o => o.ConvertUsing(new GeometryConverter(), _ => _.GeoRegion));
            
            CreateMap<WarehouseNextHops, Backend.BusinessLogic.Entities.WarehouseNextHop>().ReverseMap();
            CreateMap<GeoCoordinates, Backend.BusinessLogic.Entities.GeoCoordinates>().ReverseMap();
        }
    }
}