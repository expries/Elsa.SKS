using AutoMapper;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.MappingProfiles
{
    public class HopArrivalProfile : Profile
    {
        public HopArrivalProfile()
        {
            CreateMap<HopArrival, Package.BusinessLogic.Entities.HopArrival>().ReverseMap();
        }
    }

}