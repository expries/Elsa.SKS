using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Package.Services.DTOs;
using Elsa.SKS.Package.Services.DTOs.Converters;

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
            CreateMap<Hop, Package.BusinessLogic.Entities.Hop>().IncludeAllDerived();
            CreateMap<Package.BusinessLogic.Entities.Hop, Hop>().IncludeAllDerived();
            CreateMap<Warehouse, Package.BusinessLogic.Entities.Warehouse>().ReverseMap();

            CreateMap<TransferWarehouse, Package.BusinessLogic.Entities.TransferWarehouse>()
                .ForMember(_ => _.GeoRegion, o => o.ConvertUsing(new GeometryConverter(), _ => _.RegionGeoJson));

            CreateMap<Package.BusinessLogic.Entities.TransferWarehouse, TransferWarehouse>()
                .ForMember(_ => _.RegionGeoJson, o => o.ConvertUsing(new GeometryConverter(), _ => _.GeoRegion));

            CreateMap<Truck, Package.BusinessLogic.Entities.Truck>()
                .ForMember(_ => _.GeoRegion, o => o.ConvertUsing(new GeometryConverter(), _ => _.RegionGeoJson));

            CreateMap<Package.BusinessLogic.Entities.Truck, Truck>()
                .ForMember(_ => _.RegionGeoJson, o => o.ConvertUsing(new GeometryConverter(), _ => _.GeoRegion));
            
            CreateMap<WarehouseNextHops, Package.BusinessLogic.Entities.WarehouseNextHop>().ReverseMap();
            CreateMap<GeoCoordinates, Package.BusinessLogic.Entities.GeoCoordinates>().ReverseMap();
        }
    }
}