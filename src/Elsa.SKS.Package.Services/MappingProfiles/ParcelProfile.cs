using System.Collections.Generic;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.Services.DTOs;
using Hop = Elsa.SKS.Package.Services.DTOs.Hop;
using HopArrival = Elsa.SKS.Package.Services.DTOs.HopArrival;
using Parcel = Elsa.SKS.Package.Services.DTOs.Parcel;

namespace Elsa.SKS.MappingProfiles
{
    public class ParcelProfile : Profile
    {
        public ParcelProfile()
        {
            CreateMap<Parcel, Package.BusinessLogic.Entities.Parcel>(MemberList.Source).ReverseMap();
            CreateMap<Recipient, User>().ReverseMap();
            CreateMap<Package.BusinessLogic.Entities.Parcel, TrackingInformation>().ReverseMap();
            
            CreateMap<HopArrival, Package.BusinessLogic.Entities.HopArrival>()
                .ForMember(
                    h => h.Hop, 
                    o => o.MapFrom(dto 
                        => dto));

            CreateMap<HopArrival, Package.BusinessLogic.Entities.Hop>();
            
            CreateMap<Package.BusinessLogic.Entities.Hop, HopArrival>();
            CreateMap<Package.BusinessLogic.Entities.HopArrival, HopArrival>().IncludeMembers(h => h.Hop);
        }
    }

}