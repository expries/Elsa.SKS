using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using Elsa.SKS.Backend.Services.DTOs;
using HopArrivalDto = Elsa.SKS.Backend.Services.DTOs.HopArrival;
using HopArrivalEntity = Elsa.SKS.Backend.BusinessLogic.Entities.HopArrival;
using ParcelDto = Elsa.SKS.Backend.Services.DTOs.Parcel;
using ParcelEntity = Elsa.SKS.Backend.BusinessLogic.Entities.Parcel;
using HopEntity = Elsa.SKS.Backend.BusinessLogic.Entities.Hop;

namespace Elsa.SKS.Backend.Services.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class ParcelProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ParcelProfile()
        {
            CreateMap<ParcelDto, ParcelEntity>(MemberList.Source).ReverseMap();
            CreateMap<Recipient, User>().ReverseMap();
            CreateMap<ParcelEntity, TrackingInformation>().ReverseMap();

            CreateMap<ParcelEntity, NewParcelInfo>().ReverseMap();

            CreateMap<HopArrivalDto, HopArrivalEntity>(MemberList.None)
                .ForMember(h => h.Hop, o => o.MapFrom(dto => dto));
            CreateMap<HopArrivalDto, HopEntity>(MemberList.None);

            CreateMap<HopArrivalEntity, HopArrivalDto>().IncludeMembers(h => h.Hop);
            CreateMap<HopEntity, HopArrivalDto>(MemberList.None);
        }
    }

}