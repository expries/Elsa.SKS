using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Entities;

namespace Elsa.SKS.Backend.BusinessLogic.MappingProfiles
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebhookProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public WebhookProfile()
        {
            CreateMap<Subscription, DataAccess.Entities.Subscription>().ReverseMap();
            CreateMap<Parcel, DataAccess.Entities.WebhookMessage>().ReverseMap();
        }
    }
}