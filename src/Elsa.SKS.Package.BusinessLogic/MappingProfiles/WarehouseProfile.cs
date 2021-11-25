﻿using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.BusinessLogic.MappingProfiles
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
            CreateMap<WarehouseNextHops, DataAccess.Entities.WarehouseNextHops>().ReverseMap();
            CreateMap<GeoCoordinate, Package.DataAccess.Entities.GeoCoordinates>().ReverseMap();

            
        }
    }
}