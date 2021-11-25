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
            CreateMap<Hop, Package.DataAccess.Entities.Hop>().IncludeAllDerived();
            CreateMap<Package.DataAccess.Entities.Hop, Hop>().IncludeAllDerived();
            CreateMap<Warehouse, Package.DataAccess.Entities.Warehouse>().ReverseMap();
            CreateMap<TransferWarehouse, Package.DataAccess.Entities.TransferWarehouse>().ReverseMap();
            CreateMap<Truck, Package.DataAccess.Entities.Truck>().ReverseMap();
        }

    }
}