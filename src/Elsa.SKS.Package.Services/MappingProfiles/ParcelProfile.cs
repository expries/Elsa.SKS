using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.Services.DTOs;
using HopArrivalDto = Elsa.SKS.Package.Services.DTOs.HopArrival;
using HopArrivalEntity = Elsa.SKS.Package.BusinessLogic.Entities.HopArrival; 
using ParcelDto = Elsa.SKS.Package.Services.DTOs.Parcel;
using ParcelEntity = Elsa.SKS.Package.BusinessLogic.Entities.Parcel;
using HopEntity = Elsa.SKS.Package.BusinessLogic.Entities.Hop;

namespace Elsa.SKS.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class ParcelProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public ParcelProfile()
        {
            CreateMap<ParcelDto, ParcelEntity>(MemberList.Source).ReverseMap();
            CreateMap<Recipient, User>().ReverseMap();
            CreateMap<ParcelEntity, TrackingInformation>().ReverseMap();
            
            CreateMap<HopArrivalDto, HopArrivalEntity>(MemberList.None)
                .ForMember(h => h.Hop, o => o.MapFrom(dto => dto));
            CreateMap<HopArrivalDto, HopEntity>(MemberList.None);
            
            CreateMap<HopArrivalEntity, HopArrivalDto>().IncludeMembers(h => h.Hop);
            CreateMap<HopEntity, HopArrivalDto>(MemberList.None);
        }
    }

}