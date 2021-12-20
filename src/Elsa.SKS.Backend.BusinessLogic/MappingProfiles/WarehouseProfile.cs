using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Entities;

namespace Elsa.SKS.Backend.BusinessLogic.MappingProfiles
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
            CreateMap<WarehouseNextHop, DataAccess.Entities.WarehouseNextHop>().ReverseMap();
            CreateMap<GeoCoordinates, Backend.DataAccess.Entities.GeoCoordinates>().ReverseMap();
        }
    }
}