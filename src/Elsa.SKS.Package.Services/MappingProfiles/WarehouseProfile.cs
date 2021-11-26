using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WarehouseProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public WarehouseProfile()
        {
            CreateMap<WarehouseNextHops, Package.BusinessLogic.Entities.WarehouseNextHop>().ReverseMap();
            CreateMap<GeoCoordinates, Package.BusinessLogic.Entities.GeoCoordinates>().ReverseMap();
        }
    }

}