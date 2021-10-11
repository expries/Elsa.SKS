using AutoMapper;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class HopProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public HopProfile()
        {
            CreateMap<Hop, Package.BusinessLogic.Entities.Hop>().IncludeAllDerived().ReverseMap();
            
            CreateMap<Truck, Package.BusinessLogic.Entities.Truck>().ReverseMap();
            CreateMap<Warehouse, Package.BusinessLogic.Entities.Warehouse>().ReverseMap();
            CreateMap<TransferWarehouse, Package.BusinessLogic.Entities.TransferWarehouse>().ReverseMap();
        }
    }
}