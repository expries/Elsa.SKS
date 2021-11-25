using System.Collections.Generic;
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
            CreateMap<WarehouseNextHops, Package.BusinessLogic.Entities.WarehouseNextHops>().ReverseMap();
            CreateMap<GeoCoordinate, Package.BusinessLogic.Entities.GeoCoordinate>().ReverseMap();
        }
    }

}