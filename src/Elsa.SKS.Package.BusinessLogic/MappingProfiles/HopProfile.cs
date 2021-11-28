using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.BusinessLogic.MappingProfiles
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
            CreateMap<Hop, DataAccess.Entities.Hop>().IncludeAllDerived();
            CreateMap<DataAccess.Entities.Hop, Hop>().IncludeAllDerived();
            CreateMap<Warehouse, DataAccess.Entities.Warehouse>().ReverseMap();
            CreateMap<TransferWarehouse, DataAccess.Entities.TransferWarehouse>().ReverseMap();
            CreateMap<Truck, DataAccess.Entities.Truck>().ReverseMap();
        }

    }
}