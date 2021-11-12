﻿using System.Collections.Generic;
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
            CreateMap<Warehouse, Package.BusinessLogic.Entities.Warehouse>().ReverseMap();
            CreateMap<List<WarehouseNextHops>, List<Package.BusinessLogic.Entities.WarehouseNextHops>>().ReverseMap();
            CreateMap<Hop, Package.BusinessLogic.Entities.Hop>().ReverseMap();
            CreateMap<GeoCoordinate, Package.BusinessLogic.Entities.GeoCoordinate>().ReverseMap();
        }
    }

}