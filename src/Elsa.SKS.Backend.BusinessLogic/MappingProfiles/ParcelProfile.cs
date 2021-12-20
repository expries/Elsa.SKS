using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Entities;


namespace Elsa.SKS.Backend.BusinessLogic.MappingProfiles
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
            CreateMap<Parcel, Backend.DataAccess.Entities.Parcel>().ReverseMap();
            CreateMap<User, Backend.DataAccess.Entities.User>().ReverseMap();
            
            CreateMap<HopArrival, Backend.DataAccess.Entities.HopArrival>();
            CreateMap<HopArrival, Backend.DataAccess.Entities.Hop>();
            
            CreateMap<Backend.DataAccess.Entities.HopArrival, HopArrival>().IncludeMembers(h => h.Hop);
            CreateMap<Backend.DataAccess.Entities.Hop, HopArrival>();
            
            // Service Agent Mapping
            CreateMap<User, Backend.ServiceAgents.Entities.Address>();
        }
    }

}