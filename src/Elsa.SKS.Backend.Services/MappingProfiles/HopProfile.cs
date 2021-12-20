using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Backend.Services.DTOs;
using Elsa.SKS.Backend.Services.DTOs.Converters;

namespace Elsa.SKS.Backend.Services.MappingProfiles
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
            CreateMap<Hop, BusinessLogic.Entities.Hop>().IncludeAllDerived();
            CreateMap<BusinessLogic.Entities.Hop, Hop>().IncludeAllDerived();
            CreateMap<Warehouse, BusinessLogic.Entities.Warehouse>().ReverseMap();

            CreateMap<TransferWarehouse, BusinessLogic.Entities.TransferWarehouse>()
                .ForMember(_ => _.GeoRegion, o => o.ConvertUsing(new GeometryConverter(), _ => _.RegionGeoJson));

            CreateMap<BusinessLogic.Entities.TransferWarehouse, TransferWarehouse>()
                .ForMember(_ => _.RegionGeoJson, o => o.ConvertUsing(new GeometryConverter(), _ => _.GeoRegion));

            CreateMap<Truck, BusinessLogic.Entities.Truck>()
                .ForMember(_ => _.GeoRegion, o => o.ConvertUsing(new GeometryConverter(), _ => _.RegionGeoJson));

            CreateMap<BusinessLogic.Entities.Truck, Truck>()
                .ForMember(_ => _.RegionGeoJson, o => o.ConvertUsing(new GeometryConverter(), _ => _.GeoRegion));

            CreateMap<WarehouseNextHops, BusinessLogic.Entities.WarehouseNextHop>().ReverseMap();
            CreateMap<GeoCoordinates, BusinessLogic.Entities.GeoCoordinates>().ReverseMap();
        }
    }
}