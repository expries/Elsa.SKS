using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Package.Services.DTOs;

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
            CreateMap<TransferWarehouse, Package.BusinessLogic.Entities.TransferWarehouse>().ReverseMap();
            CreateMap<Truck, Package.BusinessLogic.Entities.Truck>().ReverseMap();
        }
    }
}