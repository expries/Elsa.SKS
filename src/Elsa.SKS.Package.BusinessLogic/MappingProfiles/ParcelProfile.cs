using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;


namespace Elsa.SKS.Package.BusinessLogic.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ParcelProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ParcelProfile()
        {
            CreateMap<Parcel, Package.DataAccess.Entities.Parcel>().ReverseMap();
            CreateMap<User, Package.DataAccess.Entities.User>().ReverseMap();
            
            CreateMap<HopArrival, Package.DataAccess.Entities.HopArrival>();
            CreateMap<HopArrival, Package.DataAccess.Entities.Hop>();
            
            CreateMap<Package.DataAccess.Entities.HopArrival, HopArrival>().IncludeMembers(h => h.Hop);
            CreateMap<Package.DataAccess.Entities.Hop, HopArrival>();
            
            // Service Agent Mapping
            CreateMap<User, Package.ServiceAgents.Entities.Address>();
        }
    }

}