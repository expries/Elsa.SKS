using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Backend.Services.DTOs;

namespace Elsa.SKS.Backend.Webhooks.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    public class WebhookProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        [ExcludeFromCodeCoverage]
        public WebhookProfile()
        {
            CreateMap<DataAccess.Entities.WebhookMessage, DTOs.WebhookMessage>().ReverseMap();
            CreateMap<DataAccess.Entities.HopArrival, HopArrival>().IncludeMembers(h => h.Hop);
            CreateMap<DataAccess.Entities.Hop, HopArrival>(MemberList.None);
        }
    }
}