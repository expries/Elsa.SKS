using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.Services.DTOs;
using Parcel = Elsa.SKS.Package.Services.DTOs.Parcel;

namespace Elsa.SKS.MappingProfiles
{
    public class ParcelProfile : Profile
    {
        public ParcelProfile()
        {
            CreateMap<Parcel, Package.BusinessLogic.Entities.Parcel>(MemberList.Source).ReverseMap();
            CreateMap<Recipient, User>().ReverseMap();;
        }
    }

}