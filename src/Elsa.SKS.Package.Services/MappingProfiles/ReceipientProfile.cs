using AutoMapper;
using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.MappingProfiles
{
    public class ReceipientProfile : Profile
    {
        public ReceipientProfile()
        {
            CreateMap<Recipient, Package.BusinessLogic.Entities.User>();
        }
    }

}